// <copyright file="IUrl.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace Zoombraco.Models
{
    /// <summary>
    /// Defines an element that has a navigable url.
    /// </summary>
    public interface IUrl
    {
        /// <summary>
        /// Gets or sets the url.
        /// </summary>
        string Url { get; set; }

        /// <summary>
        /// Gets the absolute url.
        /// </summary>
        /// <returns><see cref="string"/>.</returns>
        string UrlAbsolute();
    }
}