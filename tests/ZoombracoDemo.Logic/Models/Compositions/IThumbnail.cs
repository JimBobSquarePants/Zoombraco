// <copyright file="IThumbnail.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace ZoombracoDemo.Logic.Models
{
    using System.Web;

    using Zoombraco.Models;

    /// <summary>
    /// Encapsulates properties that allow the display of the page as a thumbnail in landing pages.
    /// </summary>
    public interface IThumbnail : IEntity, IUrl
    {
        /// <summary>
        /// Gets or sets the thumnail image.
        /// </summary>
        Image ThumbnailImage { get; set; }

        /// <summary>
        /// Gets or sets the thumbnail title.
        /// </summary>
        string ThumbnailTitle { get; set; }

        /// <summary>
        /// Gets or sets the thumbnail subtitle.
        /// </summary>
        HtmlString ThumbnailSubTitle { get; set; }
    }
}