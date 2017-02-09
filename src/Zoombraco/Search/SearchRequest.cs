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
    using Examine.Providers;
    using Examine.SearchCriteria;
    using Lucene.Net.Analysis;
    using Lucene.Net.Analysis.AR;
    using Lucene.Net.Analysis.BR;
    using Lucene.Net.Analysis.CJK;
    using Lucene.Net.Analysis.Cn;
    using Lucene.Net.Analysis.Cz;
    using Lucene.Net.Analysis.De;
    using Lucene.Net.Analysis.Fr;
    using Lucene.Net.Analysis.Nl;
    using Lucene.Net.Analysis.Ru;
    using Lucene.Net.Analysis.Standard;
    using Lucene.Net.Highlight;
    using Lucene.Net.QueryParsers;
    using Lucene.Net.Search;
    using Umbraco.Web;

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
            BaseSearchProvider searchProvider = ExamineManager.Instance.SearchProviderCollection[ZoombracoConstants.SearchConstants.SearcherName];

            IBooleanOperation searchCriteria = searchProvider.CreateSearchCriteria().OrderBy(string.Empty);

            if (!string.IsNullOrWhiteSpace(this.Query))
            {
                string[] mergedFields = new string[this.Cultures.Length];
                for (int i = 0; i < this.Cultures.Length; i++)
                {
                    mergedFields[i] = string.Format(ZoombracoConstants.SearchConstants.MergedDataFieldTemplate, this.Cultures[i].Name);
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
                searchCriteria.And().Field(ZoombracoConstants.SearchConstants.CategoryField, string.Join(" ", this.Categories));
            }

            if (!string.IsNullOrWhiteSpace(rootId))
            {
                searchCriteria.And().Field(ZoombracoConstants.SearchConstants.SiteField, rootId);
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
                            Analyzer analyzer = GetAnalyserForCulture(culture);
                            string fieldName = string.Format(ZoombracoConstants.SearchConstants.MergedDataFieldTemplate, culture.Name);
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
        /// <param name="query">The query search term.</param>
        /// <param name="highlightField">The highlight field from which to create highlights.</param>
        /// <param name="searchResults">The search results.</param>
        /// <returns>The <see cref="QueryScorer"/></returns>
        private static QueryScorer FragmentScorer(string query, string highlightField, SearchResults searchResults)
        {
            return new QueryScorer(GetLuceneQueryObject(query, highlightField).Rewrite(((IndexSearcher)searchResults.LuceneSearcher).GetIndexReader()));
        }

        /// <summary>
        /// Gets the Lucene query for creating highlight from.
        /// </summary>
        /// <param name="query">The query search term.</param>
        /// <param name="highlightField">The highlight field from which to create highlights.</param>
        /// <returns>The <see cref="Query"/></returns>
        private static Query GetLuceneQueryObject(string query, string highlightField)
        {
            QueryParser parser = new QueryParser(Lucene.Net.Util.Version.LUCENE_29, highlightField, new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_29));

            // Allow for wildcard fragments.
            parser.SetMultiTermRewriteMethod(MultiTermQuery.SCORING_BOOLEAN_QUERY_REWRITE);

            return parser.Parse($"{highlightField}:{string.Join(" ", query.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries).Select(w => w.Trim().MultipleCharacterWildcard().Value))}");
        }

        /// <summary>
        /// Returns the correct analyser for the given culture or the <see cref="StandardAnalyzer"/> if no match is found.
        /// </summary>
        /// <param name="culture">The culture</param>
        /// <returns>The <see cref="Analyzer"/></returns>
        private static Analyzer GetAnalyserForCulture(CultureInfo culture)
        {
            // TODO: Some of these might be incorrect. I'm making a best guess based on the list
            // here https://msdn.microsoft.com/en-us/library/ee825488(v=cs.20).aspx
            switch (culture.Name)
            {
                case "ar-DZ":
                case "ar-BH":
                case "ar-EG":
                case "ar-IQ":
                case "ar-JO":
                case "ar-KW":
                case "ar-LB":
                case "ar-LY":
                case "ar-MA":
                case "ar-OM":
                case "ar-QA":
                case "ar-SA":
                case "ar-SY":
                case "ar-TN":
                case "ar-AE":
                case "ar-YE":
                    return new ArabicAnalyzer(Lucene.Net.Util.Version.LUCENE_29);

                case "cs-CZ":
                    return new CzechAnalyzer();

                case "de-AT":
                case "de-DE":
                case "de-LI":
                case "de-LU":
                case "de-CH":
                    return new GermanAnalyzer();

                case "fr-CA":
                case "fr-FR":
                case "fr-LU":
                case "fr-MC":
                case "fr-CH":
                    return new FrenchAnalyzer();

                case "ja-JP":
                case "ko-KR":
                    return new CJKAnalyzer();

                case "nl-BE":
                case "nl-NL":
                    return new DutchAnalyzer();

                case "pt-BR":
                    return new BrazilianAnalyzer();

                case "ru-RU":
                    return new RussianAnalyzer();

                case "zh-CN":
                case "zh-HK":
                case "zh-MO":
                case "zh-SG":
                case "zh-TW":
                case "zh-CHS":
                case "zh-CHT":
                    return new ChineseAnalyzer();
            }

            return new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_29);
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

            string[] categories = searchResult.Fields[ZoombracoConstants.SearchConstants.CategoryField]?.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries) ?? new string[0];

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
            Highlighter highlighter = new Highlighter(formatter, FragmentScorer(this.Query, fieldName, searchResults));
            using (StringReader reader = new StringReader(fieldResult))
            {
                TokenStream tokenStream = analyzer.TokenStream(fieldName, reader);

                return highlighter.GetBestFragments(tokenStream, fieldResult, ZoombracoConstants.SearchConstants.HighlightFragements, "...");
            }
        }
    }
}