// <copyright file="ZoombracoConstants.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace Zoombraco
{
    /// <summary>
    /// Identifies framework-wide application constants.
    /// </summary>
    public static class ZoombracoConstants
    {
        /// <summary>
        /// Constant values related to content.
        /// </summary>
        public static class Content
        {
            /// <summary>
            /// The property alias for the content's "Name" property.
            /// </summary>
            public const string Name = "name";

            /// <summary>
            /// The property alias for the content's "Url" property.
            /// </summary>
            public const string Url = "url";

            /// <summary>
            /// The property alias for the content's "ExcludeFromSearchResults" property.
            /// </summary>
            public const string ExcludeFromSearchResults = "excludeFromSearchResults";
        }

        /// <summary>
        /// Constant values related to configuration.
        /// </summary>
        public static class Configuration
        {
            /// <summary>
            /// The configuration key for setting the duration in seconds to cache action results for using donut caching.
            /// </summary>
            public const string OutputCacheDuration = "Zoombraco:OutputCacheDuration";

            /// <summary>
            /// The configuration key for setting the default timeout duration in milliseconds to wait for requests to images stored on CDN before
            /// reverting to default cached storage locations.
            /// </summary>
            public const string ImageCdnRequestTimeout = "Zoombraco:ImageCdnRequestTimeout";
        }
    }
}