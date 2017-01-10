// <copyright file="Home.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace ZoombracoDemo.Logic.Models
{
    using System.Collections.Generic;

    using Zoombraco.Models;

    /// <summary>
    /// Represents the home page document type
    /// </summary>
    public class Home : Page, IHeroPanel
    {
        /// <inheritdoc />
        public virtual IEnumerable<Image> HeroImages { get; set; }

        /// <inheritdoc />
        public virtual string HeroTitle { get; set; }

        /// <inheritdoc />
        public virtual RelatedLink HeroLink { get; set; }
    }
}