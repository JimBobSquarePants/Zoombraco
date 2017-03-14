// <copyright file="INavigation.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace Zoombraco.Models
{
    using System;
    using System.Collections.Generic;
    using Umbraco.Core.Models;

    /// <summary>
    /// Encapsulates methods that allow the retrieval of relative nodes.
    /// </summary>
    public interface INavigation
    {
        /// <summary>
        /// Gets the parent of the current instance as the given <see cref="Type"/>.
        /// </summary>
        /// <typeparam name="T">The type to return</typeparam>
        /// <returns>
        /// The type instance.
        /// </returns>
        T Parent<T>();

        /// <summary>
        /// Gets the ancestors of the current instance as the given <see cref="Type"/>.
        /// </summary>
        /// <typeparam name="T">The type to return</typeparam>
        /// <param name="maxLevel">The maximum level to search.</param>
        /// <param name="predicate">A function to test each item for a condition.</param>
        /// <returns>
        /// The <see cref="IEnumerable{T}"/>.
        /// </returns>
        IEnumerable<T> Ancestors<T>(int maxLevel, Func<IPublishedContent, bool> predicate);

        /// <summary>
        /// Gets the children of the current instance as the given <see cref="Type"/>.
        /// </summary>
        /// <param name="predicate">A function to test each item for a condition.</param>
        /// <typeparam name="T">The type to return</typeparam>
        /// <returns>
        /// The <see cref="IEnumerable{T}"/>.
        /// </returns>
        IEnumerable<T> Children<T>(Func<IPublishedContent, bool> predicate);

        /// <summary>
        /// Gets the descendants of the current instance as the given <see cref="Type"/>.
        /// </summary>
        /// <typeparam name="T">The type to return</typeparam>
        /// <param name="level">The level to search.</param>
        /// <param name="predicate">A function to test each item for a condition.</param>
        /// <returns>
        /// The <see cref="IEnumerable{T}"/>.
        /// </returns>
        IEnumerable<T> Descendents<T>(int level, Func<IPublishedContent, bool> predicate);
    }
}