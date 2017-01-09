// <copyright file="VideoProvider.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace ZoombracoDemo.Logic.Models
{
    using System.ComponentModel.DataAnnotations;

    using Zoombraco.ComponentModel.Processors;

    /// <summary>
    /// Enumerates the potential video providers
    /// </summary>
    [NuPickerEnum]
    public enum VideoProvider
    {
        /// <summary>
        /// The video is hosted on YouTube
        /// </summary>
        [Display(Name = "https://www.youtube.com/embed/")]
        YouTube,

        /// <summary>
        /// The video is hosted on Vimeo
        /// </summary>
        [Display(Name = "https://player.vimeo.com/video/")]
        Vimeo
    }
}
