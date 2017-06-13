// <copyright file="SearchRequest.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace Zoombraco.Search
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using Examine;
    using Examine.LuceneEngine;
    using Examine.LuceneEngine.SearchCriteria;
    using Examine.SearchCriteria;
    using Lucene.Net.Analysis;
    using Lucene.Net.Analysis.Standard;
    using Lucene.Net.Highlight;
    using Lucene.Net.QueryParsers;
    using Lucene.Net.Search;
    using Umbraco.Web;
    using UmbracoExamine;

    /// <summary>
    /// Allows the creation and execution of searches against the Examine index.
    /// </summary>
    internal class SearchRequest
    {
        /// <summary>
        /// The umbraco helper.
        /// </summary>
        private readonly UmbracoHelper helper;

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchRequest"/> class.
        /// </summary>
        /// <param name="helper">The Umbraco helper</param>
        public SearchRequest(UmbracoHelper helper)
        {
            this.helper = helper;
        }

        /// <summary>
        /// Gets or sets the search phrase - free text search query.
        /// </summary>
        public string Query { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether the search should use wildcards.
        /// </summary>
        public bool UseWildcards { get; set; }

        /// <summary>
        /// Gets or sets the number of results to skip from the beginning of the result set.
        /// </summary>
        public int Skip { get; set; } = 0;

        /// <summary>
        /// Gets or sets the limit of the number of results to return.
        /// </summary>
        public int Take { get; set; } = int.MaxValue;

        /// <summary>
        /// Gets or sets the categories by which to filter the search.
        /// </summary>
        public string[] Categories { get; set; }

        /// <summary>
        /// Gets or sets the cultures by which to filter the search.
        /// </summary>
        public CultureInfo[] Cultures { get; set; } = { Thread.CurrentThread.CurrentUICulture };

        /// <summary>
        /// Executes the search. If the <paramref name="rootId"/> value is set the search will be executed for the site containing
        /// that content node only.
        /// </summary>
        /// <param name="rootId">The root id of the site to search within.</param>
        /// <returns>The <see cref="SearchResponse"/> containing the search results.</returns>
        public SearchResponse Execute(string rootId = "")
        {
            SearchResponse searchResponse = new SearchResponse();
            UmbracoExamineSearcher searchProvider = (UmbracoExamineSearcher)ExamineManager.Instance.SearchProviderCollection[ZoombracoConstants.Search.SearcherName];
            Analyzer analyzer = searchProvider.IndexingAnalyzer;

            // Wildcards are only supported using the languages using the standard analyzer.
            this.UseWildcards = this.UseWildcards && typeof(StandardAnalyzer) == analyzer.GetType();

            IBooleanOperation searchCriteria = searchProvider.CreateSearchCriteria().OrderBy(string.Empty);

            if (!string.IsNullOrWhiteSpace(this.Query))
            {
                string[] mergedFields = new string[this.Cultures.Length];
                for (int i = 0; i < this.Cultures.Length; i++)
                {
                    mergedFields[i] = string.Format(ZoombracoConstants.Search.MergedDataFieldTemplate, this.Cultures[i].Name);
                }

                if (this.UseWildcards)
                {
                    searchCriteria = searchProvider
                        .CreateSearchCriteria()
                        .GroupedOr(
                        mergedFields,
                        this.Query.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries).Select(w => w.Trim().MultipleCharacterWildcard())
                        .ToArray());
                }
                else
                {
                    searchCriteria = searchProvider
                        .CreateSearchCriteria()
                        .GroupedAnd(mergedFields, this.Query);
                }
            }

            if (this.Categories != null && this.Categories.Any())
            {
                searchCriteria.And().Field(ZoombracoConstants.Search.CategoryField, string.Join(" ", this.Categories));
            }

            if (!string.IsNullOrWhiteSpace(rootId))
            {
                searchCriteria.And().Field(ZoombracoConstants.Search.SiteField, rootId);
            }

            if (searchCriteria != null)
            {
                ISearchResults searchResults = null;
                try
                {
                    searchResults = searchProvider.Search(searchCriteria.Compile());
                }
                catch (NullReferenceException)
                {
                    // If the query object can't be compiled then an exception within Examine is raised
                }

                if (searchResults != null)
                {
                    Formatter formatter = new SimpleHTMLFormatter("<strong>", "</strong>");

                    foreach (SearchResult searchResult in searchResults.Skip(this.Skip).Take(this.Take))
                    {
                        foreach (CultureInfo culture in this.Cultures)
                        {
                            string fieldName = string.Format(ZoombracoConstants.Search.MergedDataFieldTemplate, culture.Name);
                            string fieldResult = searchResult.Fields[fieldName];
                            this.AddSearchMatch(analyzer, formatter, searchResults, searchResponse, searchResult, fieldName, fieldResult);
                        }
                    }

                    searchResponse.TotalCount = searchResults.TotalItemCount;
                }
            }

            return searchResponse;
        }

        /// <summary>
        /// Gets the query fragment scorer for highlighting.
        /// </summary>
        /// <param name="analyzer">The analyzer which extracts index terms from text.</param>
        /// <param name="query">The query search term.</param>
        /// <param name="highlightField">The highlight field from which to create highlights.</param>
        /// <param name="searchResults">The search results.</param>
        /// <returns>The <see cref="QueryScorer"/></returns>
        private QueryScorer FragmentScorer(Analyzer analyzer, string query, string highlightField, SearchResults searchResults)
        {
            return new QueryScorer(this.GetLuceneQueryObject(analyzer, query, highlightField).Rewrite(((IndexSearcher)searchResults.LuceneSearcher).GetIndexReader()));
        }

        /// <summary>
        /// Gets the Lucene query for creating highlight from.
        /// </summary>
        /// <param name="analyzer">The analyzer which extracts index terms from text.</param>
        /// <param name="query">The query search term.</param>
        /// <param name="highlightField">The highlight field from which to create highlights.</param>
        /// <returns>The <see cref="Query"/></returns>
        private Query GetLuceneQueryObject(Analyzer analyzer, string query, string highlightField)
        {
            QueryParser parser = new QueryParser(Lucene.Net.Util.Version.LUCENE_29, highlightField, analyzer);

            // Allow for wildcard fragments.
            if (this.UseWildcards)
            {
                parser.SetMultiTermRewriteMethod(MultiTermQuery.SCORING_BOOLEAN_QUERY_REWRITE);
                return parser.Parse(QueryParser.Escape($"{highlightField}:{string.Join(" ", query.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries).Select(w => w.Trim().MultipleCharacterWildcard().Value))}"));
            }

            return parser.Parse(QueryParser.Escape($"{highlightField}:{query}"));
        }

        /// <summary>
        /// Adds a highlighted search match to search response.
        /// </summary>
        /// <param name="analyzer">The analyzer which extracts index terms from text.</param>
        /// <param name="formatter">The formatter that adds markup highlighting matches.</param>
        /// <param name="searchResults">The search results</param>
        /// <param name="searchResponse">The response containing matches for the request.</param>
        /// <param name="searchResult">The search result.</param>
        /// <param name="fieldName">The field name.</param>
        /// <param name="fieldResult">The field result.</param>
        private void AddSearchMatch(Analyzer analyzer, Formatter formatter, ISearchResults searchResults, SearchResponse searchResponse, SearchResult searchResult, string fieldName, string fieldResult)
        {
            string highlight = this.GetHighlight(analyzer, formatter, (SearchResults)searchResults, fieldName, fieldResult);

            string[] categories = searchResult.Fields[ZoombracoConstants.Search.CategoryField]?.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries) ?? new string[0];

            switch (searchResult.Fields["__IndexType"])
            {
                case "content":
                    searchResponse.SearchMatches.Add(new SearchMatch(searchResult.Id, highlight, this.helper, categories));
                    break;

                case "media":
                    searchResponse.SearchMatches.Add(new SearchMatch(searchResult.Id, highlight, this.helper, categories));
                    break;
            }
        }

        /// <summary>
        /// Gets a highlight text for a given search result item
        /// </summary>
        /// <param name="analyzer">The analyzer which extracts index terms from text.</param>
        /// <param name="formatter">The formatter that adds markup highlighting matches.</param>
        /// <param name="searchResults">The search results.</param>
        /// <param name="fieldName">The field name.</param>
        /// <param name="fieldResult">The field value.</param>
        /// <returns>The <see cref="string"/></returns>
        private string GetHighlight(Analyzer analyzer, Formatter formatter, SearchResults searchResults, string fieldName, string fieldResult)
        {
            Highlighter highlighter = new Highlighter(formatter, this.FragmentScorer(analyzer, this.Query, fieldName, searchResults));
            using (StringReader reader = new StringReader(fieldResult))
            {
                TokenStream tokenStream = analyzer.TokenStream(fieldName, reader);

                return highlighter.GetBestFragments(tokenStream, fieldResult, ZoombracoConstants.Search.HighlightFragements, "...");
            }
        }
    }
}