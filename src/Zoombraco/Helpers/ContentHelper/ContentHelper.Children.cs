// <copyright file="ContentHelper.Children.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace Zoombraco.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Umbraco.Core.Models;
    using Zoombraco.Models;

    /// <content>
    /// Allows the querying of child content
    /// </content>
    public partial class ContentHelper
    {
        /// <summary>
        /// Gets the children of the current instance as an <see cref="IEnumerable{Page}"/>.
        /// </summary>
        /// <param name="nodeId">The id of the current node to search from.</param>
        /// <param name="predicate">A function to test each item for a condition.</param>
        /// <param name="skip">The number of elements to bypass in the collection</param>
        /// <param name="take">The number of contiguous elements to take from the collection</param>
        /// <returns>
        /// The <see cref="IEnumerable{Page}"/>.
        /// </returns>
        public IEnumerable<Page> GetChildren(int nodeId, Func<IPublishedContent, bool> predicate = null, int skip = 0, int take = int.MaxValue)
            => this.GetChildren<Page>(nodeId, predicate, skip, take);

        /// <summary>
        /// Gets the children of the current instance as the given <see cref="Type"/>.
        /// </summary>
        /// <typeparam name="T">The type to return</typeparam>
        /// <param name="nodeId">The id of the current node to search from.</param>
        /// <param name="predicate">A function to test each item for a condition.</param>
        /// <param name="skip">The number of elements to bypass in the collection</param>
        /// <param name="take">The number of contiguous elements to take from the collection</param>
        /// <returns>
        /// The <see cref="IEnumerable{T}"/>.
        /// </returns>
        public IEnumerable<T> GetChildren<T>(int nodeId, Func<IPublishedContent, bool> predicate = null, int skip = 0, int take = int.MaxValue)
        {
            return this.FilterAndParseCollection<T>(
                predicate == null
                    ? this.UmbracoHelper.TypedContent(nodeId).Children
                    : this.UmbracoHelper.TypedContent(nodeId).Children.Where(predicate),
                skip,
                take);
        }
    }
}