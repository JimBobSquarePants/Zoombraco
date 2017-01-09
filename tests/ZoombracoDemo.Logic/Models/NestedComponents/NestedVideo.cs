// <copyright file="NestedVideo.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace ZoombracoDemo.Logic.Models
{
    using Zoombraco.Models;

    /// <summary>
    /// A nested video container. Youtube and Vimeo just now for brevity
    /// </summary>
    public class NestedVideo : NestedComponent
    {
        /// <summary>
        /// Gets or sets the video provider
        /// </summary>
        public virtual VideoProvider VideoProvider { get; set; }

        /// <summary>
        /// Gets or sets the video url
        /// </summary>
        public virtual string VideoUrl { get; set; }
    }
}