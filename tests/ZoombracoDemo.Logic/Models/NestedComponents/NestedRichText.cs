// <copyright file="NestedRichText.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace ZoombracoDemo.Logic.Models
{
    using System.Web;
    using Zoombraco.Models;

    /// <summary>
    /// A nested rich text editor for adding block of copy to pages
    /// </summary>
    public class NestedRichText : NestedComponent
    {
        /// <summary>
        /// Gets or sets the body text
        /// </summary>
        public virtual HtmlString BodyText { get; set; }
    }
}
