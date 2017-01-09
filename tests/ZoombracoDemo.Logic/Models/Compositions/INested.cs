// <copyright file="INested.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace ZoombracoDemo.Logic.Models
{
    using System.Collections.Generic;
    using Zoombraco.Models;

    /// <summary>
    /// Encapsulates properties that allow the addition of oderable nested content on the page
    /// </summary>
    public interface INested
    {
        /// <summary>
        /// Gets or sets the nested content
        /// </summary>
        IEnumerable<NestedComponent> NestedContent { get; set; }
    }
}
