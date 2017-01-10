// <copyright file="IHeroPanel.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace ZoombracoDemo.Logic.Models
{
    using System.Collections.Generic;

    using Zoombraco.ComponentModel.PropertyValueConverters;
    using Zoombraco.Models;

    /// <summary>
    /// Encapsulates properties allowing a hero panel on the page.
    /// </summary>
    public interface IHeroPanel
    {
        /// <summary>
        /// Gets or sets the header images.
        /// </summary>
        IEnumerable<Image> HeroImages { get; set; }

        /// <summary>
        /// Gets or sets the header title
        /// </summary>
        string HeroTitle { get; set; }

        /// <summary>
        /// Gets or sets the related link
        /// </summary>
        RelatedLink HeroLink { get; set; }
    }
}