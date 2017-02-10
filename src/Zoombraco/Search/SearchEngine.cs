// <copyright file="SearchEngine.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace Zoombraco.Search
{
    using System.Collections.Generic;
    using System.Globalization;
    using Umbraco.Web;

    /// <summary>
    /// Provides methods allowing searching for objects within the application.
    /// </summary>
    public static class SearchEngine
    {
        /// <summary>
        /// Searches within the current site for the given query.
        /// </summary>
        /// <param name="query">The query containing information to search for.</param>
        /// <param name="cultures">The collection of <see cref="CultureInfo"/>, if any, to restrict the search to.</param>
        /// <param name="useWildcards">Whether to use wildcards when searching. Wildcards are only supported using the languages using the standard analyzer.</param>
        /// <param name="categories">The categories, if any, to restrict a search to.</param>
        /// <param name="skip">The number of matches to skip.</param>
        /// <param name="take">The number of matches to take.</param>
        /// <returns>
        /// The <see cref="IEnumerable{SearchMatch}"/>.
        /// </returns>
        public static SearchResponse SearchSite(string query, CultureInfo[] cultures = null, bool useWildcards = false, string[] categories = null, int skip = 0, int take = int.MaxValue)
        {
            SearchRequest request = new SearchRequest(new UmbracoHelper(UmbracoContext.Current))
            {
                Query = query,
                Skip = skip,
                Take = take,
                UseWildcards = useWildcards
            };

            if (categories != null)
            {
                request.Categories = categories;
            }

            if (cultures != null)
            {
                request.Cultures = cultures;
            }

            // We want to constrain searches to the current site only.
            string rootId = UmbracoContext.Current.PublishedContentRequest.PublishedContent.Path.Split(',')[1];

            // Search results should match the current culture.
            return request.Execute(rootId);
        }

        /// <summary>
        /// Searches across multiple sites for the given query.
        /// </summary>
        /// <param name="query">The query containing information to search for.</param>
        /// <param name="cultures">The collection of <see cref="CultureInfo"/>, if any, to restrict the search to.</param>
        /// <param name="useWildcards">Whether to use wildcards when searching. Wildcards are only supported using the languages using the standard analyzer.</param>
        /// <param name="categories">The categories, if any, to restrict a search to.</param>
        /// <param name="skip">The number of matches to skip.</param>
        /// <param name="take">The number of matches to take.</param>
        /// <returns>
        /// The <see cref="IEnumerable{SearchMatch}"/>.
        /// </returns>
        public static SearchResponse SearchMultipleSites(string query, CultureInfo[] cultures = null, bool useWildcards = false, string[] categories = null, int skip = 0, int take = int.MaxValue)
        {
            SearchRequest request = new SearchRequest(new UmbracoHelper(UmbracoContext.Current))
            {
                Query = query,
                Skip = skip,
                Take = take,
                UseWildcards = useWildcards
            };

            if (categories != null)
            {
                request.Categories = categories;
            }

            if (cultures != null)
            {
                request.Cultures = cultures;
            }

            return request.Execute();
        }
    }
}