// <copyright file="ContentHelper.Examine.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace Zoombraco.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Examine;
    using Examine.SearchCriteria;
    using Our.Umbraco.Ditto;
    using Umbraco.Core;
    using Umbraco.Core.Models;
    using UmbracoExamine;

    /// <content>
    /// Allows the querying of content from the examine index
    /// </content>
    public partial class ContentHelper
    {
        /// <summary>
        /// Gets the nodes matching the given <see cref="Type"/> from the Examine index.
        /// </summary>
        /// <typeparam name="T">The type to return</typeparam>
        /// <param name="searcher">The name of the examine searcher to use. Defaults to <see cref="Constants.Examine.ExternalSearcher"/></param>
        /// <param name="predicate">A function to test each item for a condition.</param>
        /// <param name="skip">The number of elements to bypass in the collection</param>
        /// <param name="take">The number of contiguous elements to take from the collection</param>
        /// <returns>
        /// The nodes matching the given <see cref="Type"/>.
        /// </returns>
        public IEnumerable<T> GetByNodeFromExamine<T>(string searcher = Constants.Examine.ExternalSearcher, Func<IPublishedContent, bool> predicate = null, int skip = 0, int take = int.MaxValue)
            where T : class
        {
            var searchProvider = (UmbracoExamineSearcher)ExamineManager.Instance.SearchProviderCollection[searcher];
            ISearchCriteria critera = searchProvider.CreateSearchCriteria(IndexTypes.Content);
            IBooleanOperation query;

            Type returnType = typeof(T);

            // Filter the collection if necessary by our specific type.
            if (!this.IsInheritableType(returnType))
            {
                query = critera.NodeTypeAlias(returnType.Name.ToSafeAlias());
            }
            else
            {
                IEnumerable<KeyValuePair<string, Type>> assignableTypes = RegisteredTypes.Where(x => returnType.IsAssignableFrom(x.Value));
                query = critera.GroupedOr(new[] { "nodeTypeAlias" }, assignableTypes.Select(x => x.Key.ToSafeAlias()).ToArray());
            }

            IEnumerable<SearchResult> searchResults = searchProvider.Search(query.Compile());

            // Map back to IPublishedContent
            var ids = searchResults.Select(result => result.Id).ToList();
            IEnumerable<IPublishedContent> published = this.UmbracoHelper.TypedContent(ids);

            if (predicate != null)
            {
                published = published.Where(predicate);
            }

            // We skip/take here as the predicate will change the ordering
            if (skip > 0)
            {
                published = published.Skip(skip);
            }

            if (take < int.MaxValue)
            {
                published = published.Take(take);
            }

            foreach (IPublishedContent content in published)
            {
                yield return content.As<T>();
            }
        }
    }
}