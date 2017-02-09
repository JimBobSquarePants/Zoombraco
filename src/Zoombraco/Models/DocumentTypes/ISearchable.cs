// <copyright file="ISearchable.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace Zoombraco.Models
{
    /// <summary>
    /// Encapsulates properties required to determine whether a page or component should indexed by the search engine.
    /// </summary>
    public interface ISearchable
    {
        /// <summary>
        /// Gets or sets a value indicating whether the page should be excluded from search results.
        /// </summary>
        bool ExcludeFromSearchResults { get; set; }
    }
}
