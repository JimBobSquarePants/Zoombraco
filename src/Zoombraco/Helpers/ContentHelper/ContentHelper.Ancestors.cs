// <copyright file="ContentHelper.Ancestors.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace Zoombraco.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Umbraco.Core.Models;
    using Umbraco.Web;
    using Zoombraco.Models;

    /// <content>
    /// Allows the querying of ancestor content
    /// </content>
    public partial class ContentHelper
    {
        /// <summary>
        /// Gets the ancestors of the current instance as an <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <param name="nodeId">The id of the current node to search from.</param>
        /// <param name="maxLevel">The maximum level to search.</param>
        /// <param name="predicate">A function to test each item for a condition.</param>
        /// <param name="skip">The number of elements to bypass in the collection</param>
        /// <param name="take">The number of contiguous elements to take from the collection</param>
        /// <returns>
        /// The <see cref="IEnumerable{Page}"/>.
        /// </returns>
        public IEnumerable<Page> GetAncestors(int nodeId, int maxLevel = int.MaxValue, Func<IPublishedContent, bool> predicate = null, int skip = 0, int take = int.MaxValue)
            => this.GetAncestors<Page>(nodeId, maxLevel, predicate, skip, take);

        /// <summary>
        /// Gets the ancestors of the current instance as the given <see cref="Type"/>.
        /// </summary>
        /// <typeparam name="T">The type to return</typeparam>
        /// <param name="nodeId">The id of the current node to search from.</param>
        /// <param name="maxLevel">The maximum level to search.</param>
        /// <param name="predicate">A function to test each item for a condition.</param>
        /// <param name="skip">The number of elements to bypass in the collection</param>
        /// <param name="take">The number of contiguous elements to take from the collection</param>
        /// <returns>
        /// The <see cref="IEnumerable{T}"/>.
        /// </returns>
        public IEnumerable<T> GetAncestors<T>(int nodeId, int maxLevel = int.MaxValue, Func<IPublishedContent, bool> predicate = null, int skip = 0, int take = int.MaxValue)
        {
            return this.FilterAndParseCollection<T>(
                predicate == null
                    ? this.UmbracoHelper.TypedContent(nodeId).Ancestors(maxLevel)
                    : this.UmbracoHelper.TypedContent(nodeId).Ancestors(maxLevel).Where(predicate),
                skip,
                take);
        }
    }
}
