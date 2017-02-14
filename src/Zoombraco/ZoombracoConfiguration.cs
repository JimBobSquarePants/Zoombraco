// <copyright file="ZoombracoConfiguration.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace Zoombraco
{
    using System;
    using System.Web.Configuration;

    /// <summary>
    /// Provides access to site wide configuration values.
    /// </summary>
    public class ZoombracoConfiguration
    {
        /// <summary>
        /// A new instance Initializes a new instance of the <see cref="ZoombracoConfiguration"/> class.
        /// with lazy initialization.
        /// </summary>
        private static readonly Lazy<ZoombracoConfiguration> LazyInstance = new Lazy<ZoombracoConfiguration>(() => new ZoombracoConfiguration());

        /// <summary>
        /// Prevents a default instance of the <see cref="ZoombracoConfiguration"/> class from being created.
        /// </summary>
        private ZoombracoConfiguration()
        {
            this.Initialize();
        }

        /// <summary>
        /// Gets the current instance of the <see cref="ZoombracoConfiguration"/> class.
        /// </summary>
        public static ZoombracoConfiguration Instance => LazyInstance.Value;

        /// <summary>
        /// Gets the amount of time in minutes to cache content for.
        /// </summary>
        public int OutputCacheDuration { get; private set; }

        /// <summary>
        /// Gets the timeout duration in milliseconds to wait for requests to images stored on CDN before
        /// reverting to default cached storage locations. By default this is 1000ms.
        /// </summary>
        public int ImageCdnRequestTimeout { get; private set; } = 1000;

        /// <summary>
        /// Initializes all the settings values from the config.
        /// </summary>
        private void Initialize()
        {
            int duration;
            if (int.TryParse(WebConfigurationManager.AppSettings[ZoombracoConstants.Configuration.OutputCacheDuration], out duration))
            {
                if (duration > 0)
                {
                    this.OutputCacheDuration = duration;
                }
            }

            int timeout;
            if (int.TryParse(WebConfigurationManager.AppSettings[ZoombracoConstants.Configuration.ImageCdnRequestTimeout], out timeout))
            {
                if (timeout > 0)
                {
                    this.ImageCdnRequestTimeout = timeout;
                }
            }
        }
    }
}
