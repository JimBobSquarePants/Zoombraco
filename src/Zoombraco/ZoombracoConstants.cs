// <copyright file="ZoombracoConstants.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace Zoombraco
{
    using System.Reflection;

    /// <summary>
    /// Identifies framework-wide application constants.
    /// </summary>
    public static class ZoombracoConstants
    {
        /// <summary>
        /// The property bindings for mappable properties
        /// </summary>
        internal const BindingFlags MappablePropertiesBindingFlags = BindingFlags.Public | BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Static;

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

            /// <summary>
            /// Gets or sets the name of the culture cookie.
            /// </summary>
            public const string CultureCookieName = "zoombraco.culture";
        }

        /// <summary>
        /// Constant values related to configuration.
        /// </summary>
        public static class Configuration
        {
            /// <summary>
            /// The current Zoombraco version;
            /// </summary>
            public const string Version = "Zoombraco:Version";

            /// <summary>
            /// The configuration key for setting the duration in seconds to cache action results for using donut caching.
            /// </summary>
            public const string OutputCacheDuration = "Zoombraco:OutputCacheDuration";

            /// <summary>
            /// The configuration key for setting the default timeout duration in milliseconds to wait for requests to images stored on CDN before
            /// reverting to default cached storage locations.
            /// </summary>
            public const string ImageCdnRequestTimeout = "Zoombraco:ImageCdnRequestTimeout";

            /// <summary>
            /// The configuration key for setting whether the Umbraco ModelsBuilder package is enabled.
            /// </summary>
            public const string ModelsBuilderEnabled = "Umbraco.ModelsBuilder.Enable";
        }

        /// <summary>
        /// Constants for ensuring the correct values are used across search functionality.
        /// </summary>
        public static class Search
        {
            /// <summary>
            /// The name of the Examine index to search within.
            /// </summary>
            public const string IndexerName = "ExternalIndexer";

            /// <summary>
            /// The name of the Examine searcher to use to perform searches.
            /// </summary>
            public const string SearcherName = "ExternalSearcher";

            /// <summary>
            /// The formatting template of the field that will contain merged data.
            /// </summary>
            public const string MergedDataFieldTemplate = "ZoombracoMergedData-{0}";

            /// <summary>
            /// The name of the field that will contain category specific information.
            /// </summary>
            public const string CategoryField = "ZoombracoCategories";

            /// <summary>
            /// The name of the field that will contain site specific information.
            /// </summary>
            public const string SiteField = "ZoombracoSite";

            /// <summary>
            /// The name of the field used by Examine to represent the "Name" property on a node.
            /// </summary>
            public const string NodeName = "nodeName";

            /// <summary>
            /// The maximum number of highlight fragments to display in a search result.
            /// </summary>
            public const int HighlightFragements = 3;
        }

        /// <summary>
        /// Constant values related to handling embedded resources.
        /// </summary>
        public static class EmbeddedResources
        {
            /// <summary>
            /// The embedded resources route identifier.
            /// </summary>
            public const string EmbeddedResourcesRoute = "ZoombracoEmbeddedResources";

            /// <summary>
            /// The root location for all embedded resources.
            /// </summary>
            public const string ResourceRoot = "/App_Plugins/Zoombraco/GetResource/";

            /// <summary>
            /// The root namespace for embedded resources.
            /// </summary>
            public const string ResourceRootNameSpace = "Zoombraco.Umbraco.Resources.";

            /// <summary>
            /// The extension to add to embedded resources in order to ensure that the resource is not blocked
            /// by client dependency security constraints.
            /// </summary>
            public const string ResourceExtension = ".umb";
        }
    }
}