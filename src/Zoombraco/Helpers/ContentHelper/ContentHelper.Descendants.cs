// <copyright file="ContentHelper.Descendants.cs" company="James Jackson-South">
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
    /// Allows the querying of descendant content
    /// </content>
    public partial class ContentHelper
    {
        /// <summary>
        /// Gets the descendants of the current instance as an <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <param name="nodeId">The id of the current node to search from.</param>
        /// <param name="level">The level to search.</param>
        /// <param name="predicate">A function to test each item for a condition.</param>
        /// <param name="skip">The number of elements to bypass in the collection</param>
        /// <param name="take">The number of contiguous elements to take from the collection</param>
        /// <returns>
        /// The <see cref="IEnumerable{Page}"/>.
        /// </returns>
        public IEnumerable<Page> GetDescendants(int nodeId, int level = 0, Func<IPublishedContent, bool> predicate = null, int skip = 0, int take = int.MaxValue)
            => this.GetDescendants<Page>(nodeId, level, predicate, skip, take);

        /// <summary>
        /// Gets the descendants of the current instance as the given <see cref="Type"/>.
        /// </summary>
        /// <typeparam name="T">The type to return</typeparam>
        /// <param name="nodeId">The id of the current node to search from.</param>
        /// <param name="level">The level to search.</param>
        /// <param name="predicate">A function to test each item for a condition.</param>
        /// <param name="skip">The number of elements to bypass in the collection</param>
        /// <param name="take">The number of contiguous elements to take from the collection</param>
        /// <returns>
        /// The <see cref="IEnumerable{T}"/>.
        /// </returns>
        public IEnumerable<T> GetDescendants<T>(int nodeId, int level = 0, Func<IPublishedContent, bool> predicate = null, int skip = 0, int take = int.MaxValue)
        {
            if (level == 0)
            {
                return this.FilterAndParseCollection<T>(
                    predicate == null
                        ? this.UmbracoHelper.TypedContent(nodeId).Descendants()
                        : this.UmbracoHelper.TypedContent(nodeId).Descendants().Where(predicate),
                    skip,
                    take);
            }

            return this.FilterAndParseCollection<T>(
                predicate == null
                    ? this.UmbracoHelper.TypedContent(nodeId).Descendants(level)
                    : this.UmbracoHelper.TypedContent(nodeId).Descendants(level).Where(predicate),
                skip,
                take);
        }
    }
}