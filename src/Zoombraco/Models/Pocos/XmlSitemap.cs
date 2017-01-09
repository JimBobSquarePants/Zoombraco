// <copyright file="XmlSitemap.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace Zoombraco.Models
{
    using System.Collections.Generic;

    /// <summary>
    /// The render sitemap view model.
    /// </summary>
    public class XmlSitemap
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="XmlSitemap"/> class.
        /// </summary>
        public XmlSitemap()
        {
            this.Items = new HashSet<SitemapItem>();
        }

        /// <summary>
        /// Gets the sitemap items.
        /// </summary>
        public ICollection<SitemapItem> Items { get; private set; }
    }
}
