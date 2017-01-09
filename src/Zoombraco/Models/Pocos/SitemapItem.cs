// <copyright file="SitemapItem.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace Zoombraco.Models
{
    using System;

    /// <summary>
    /// Represents a single sitemap item.
    /// </summary>
    public class SitemapItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SitemapItem"/> class.
        /// </summary>
        /// <param name="url">The URL to the item.</param>
        /// <param name="lastModified">The last modified date.</param>
        /// <param name="frequency">The frequently the page is likely to change.</param>
        /// <param name="priority">The priority of this URL relative to other URLs on your site.</param>
        public SitemapItem(string url, DateTime lastModified, ChangeFrequency frequency, decimal priority)
        {
            this.Url = url;
            this.LastModified = lastModified;
            this.ChangeFrequency = frequency;
            this.Priority = priority;
        }

        /// <summary>
        /// Gets the URL to the item.
        /// </summary>
        public string Url { get; private set; }

        /// <summary>
        /// Gets the last modified date.
        /// </summary>
        public DateTime LastModified { get; private set; }

        /// <summary>
        /// Gets or sets the frequently the page is likely to change.
        /// This value provides general information to search engines and may not correlate exactly to how often they crawl the page.
        /// </summary>
        public ChangeFrequency ChangeFrequency { get; set; }

        /// <summary>
        /// Gets or sets the priority of this URL relative to other URLs on your site.
        /// Valid values range from 0.0 to 1.0. This value does not affect how your pages are compared to pages
        /// on other sites—it only lets the search engines know which pages you deem most important for the crawlers.
        /// </summary>
        public decimal Priority { get; set; }
    }
}