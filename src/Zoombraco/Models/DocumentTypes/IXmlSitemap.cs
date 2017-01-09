// <copyright file="IXmlSitemap.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace Zoombraco.Models
{
    /// <summary>
    /// Encapsulates properties required to render a page correctly in an XML sitemap.
    /// </summary>
    public interface IXmlSitemap
    {
        /// <summary>
        /// Gets or sets a value indicating whether the page should be excluded from xml sitemap.
        /// </summary>
        bool ExcludeFromXmlSitemap { get; set; }

        /// <summary>
        /// Gets or sets the frequently the page is likely to change.
        /// This value provides general information to search engines and may not correlate exactly to how often they crawl the page.
        /// </summary>
        ChangeFrequency ChangeFrequency { get; set; }

        /// <summary>
        /// Gets or sets the priority of this URL relative to other URLs on your site.
        /// Valid values range from 0.0 to 1.0. This value does not affect how your pages are compared to pages
        /// on other sites—it only lets the search engines know which pages you deem most important for the crawlers.
        /// </summary>
        decimal Priority { get; set; }
    }
}