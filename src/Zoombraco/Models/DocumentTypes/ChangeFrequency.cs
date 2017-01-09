// <copyright file="ChangeFrequency.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace Zoombraco.Models
{
    using Zoombraco.ComponentModel.Processors;

    /// <summary>
    /// How frequently the page is likely to change.
    /// This value provides general information to search engines and may not correlate exactly to how often they crawl the page.
    /// </summary>
    [NuPickerEnum]
    public enum ChangeFrequency
    {
        /// <summary>
        /// The page always changes. Used to describe documents that change each time they are accessed.
        /// </summary>
        Always = 1,

        /// <summary>
        /// The page changes hourly.
        /// </summary>
        Hourly = 2,

        /// <summary>
        /// The page changes daily.
        /// </summary>
        Daily = 3,

        /// <summary>
        /// The page changes weekly.
        /// </summary>
        Weekly = 0,

        /// <summary>
        /// The page changes monthly.
        /// </summary>
        Monthly = 4,

        /// <summary>
        /// The page changes yearly.
        /// </summary>
        Yearly = 5,

        /// <summary>
        /// The page changes never. Used to describe archived URLs.
        /// </summary>
        Never = 6
    }
}