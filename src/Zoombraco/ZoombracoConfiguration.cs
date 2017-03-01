// <copyright file="ZoombracoConfiguration.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace Zoombraco
{
    using System;
    using System.Configuration;
    using System.Linq;
    using System.Web.Configuration;
    using System.Xml.Linq;
    using Semver;
    using Umbraco.Core.IO;
    using Umbraco.Core.Logging;
    using Zoombraco.Helpers;

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
        /// Gets the currently runnning Zoombraco version.
        /// </summary>
        public SemVersion ProductVersion { get; private set; }

        /// <summary>
        /// Gets the amount of time in minutes to cache content for.
        /// </summary>
        public int OutputCacheDuration { get; private set; }

        /// <summary>
        /// Gets the timeout duration in milliseconds to wait for requests to images stored on CDN before
        /// reverting to default cached storage locations. By default this is 2000ms.
        /// </summary>
        public int ImageCdnRequestTimeout { get; private set; } = 2000;

        /// <summary>
        /// Saves a setting into the configuration file.
        /// </summary>
        /// <param name="key">Key of the setting to be saved.</param>
        /// <param name="value">Value of the setting to be saved.</param>
        internal static void SaveSetting(string key, string value)
        {
            string fileName = Umbraco.Core.IO.IOHelper.MapPath($"{SystemDirectories.Root}/web.config");
            XDocument xml = XDocument.Load(fileName, LoadOptions.PreserveWhitespace);

            if (xml.Root != null)
            {
                XElement appSettings = xml.Root.DescendantsAndSelf("appSettings").Single();

                // Update appSetting if it exists, else create a new appSetting for the given key and value.
                // ReSharper disable once PossibleNullReferenceException
                XElement setting = appSettings.Descendants("add").FirstOrDefault(s => s.Attribute("key").Value == key);
                if (setting == null)
                {
                    appSettings.Add(new XElement("add", new XAttribute("key", key), new XAttribute("value", value)));
                }
                else
                {
                    // ReSharper disable once PossibleNullReferenceException
                    setting.Attribute("value").Value = value;
                }
            }

            xml.Save(fileName, SaveOptions.DisableFormatting);
            ConfigurationManager.RefreshSection("appSettings");
        }

        /// <summary>
        /// Initializes all the settings values from the config.
        /// </summary>
        private void Initialize()
        {
            try
            {
                this.ProductVersion = VersionParser.FromSemanticString(WebConfigurationManager.AppSettings[ZoombracoConstants.Configuration.Version]);
            }
            catch
            {
                LogHelper.Info<ZoombracoConfiguration>($"No {ZoombracoConstants.Configuration.Version} appsetting found.");
                this.ProductVersion = new SemVersion(0);
            }

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
