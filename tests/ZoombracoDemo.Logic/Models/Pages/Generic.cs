﻿// <copyright file="Generic.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace ZoombracoDemo.Logic.Models
{
    using System.Collections.Generic;

    using Zoombraco.Models;

    /// <summary>
    /// The generic page document type
    /// </summary>
    public class Generic : Page, IHeroPanel, INested
    {
        /// <inheritdoc />
        public virtual IEnumerable<Image> HeroImages { get; set; }

        /// <inheritdoc />
        public virtual string HeroTitle { get; set; }

        /// <inheritdoc />
        public virtual RelatedLink HeroLink { get; set; }

        /// <inheritdoc />
        public virtual IEnumerable<NestedComponent> NestedContent { get; set; }
    }
}