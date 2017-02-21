// <copyright file="ZoombracoBootstrapper.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace Zoombraco
{
    using System;
    using System.IO;
    using System.Reflection;
    using Semver;
    using Umbraco.Core;
    using Umbraco.Core.Logging;
    using Zoombraco.Helpers;
    using Zoombraco.Resources;

    /// <summary>
    /// Provides installation logic for setting up the project
    /// </summary>
    public class ZoombracoBootstrapper
    {
        /// <summary>
        /// Performs installation of the Zoombraco package.
        /// Note: At this point in time it is not possible to register the package as installed to the CMS. This means that we are
        /// unable to automatically uninstall the package.
        /// </summary>
        /// <param name="umbracoApplication">
        /// The current <see cref="UmbracoApplicationBase"/>
        /// </param>
        /// <param name="applicationContext">
        /// The Umbraco <see cref="ApplicationContext"/> for the current application.
        /// </param>
        /// <returns>The <see cref="bool"/></returns>
        public static bool Install(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            if (!applicationContext.IsConfigured)
            {
                LogHelper.Info<ZoombracoBootstrapper>("Umbraco is not configured. Aborting Zoombraco setup.");
                return false;
            }

            // Check our versioning.
            Assembly assembly = typeof(ZoombracoBootstrapper).Assembly;
            SemVersion currentVersion = new SemVersion(assembly.GetName().Version);
            SemVersion installedVersion = ZoombracoConfiguration.Instance.CurrentVersion;
            if (installedVersion >= currentVersion)
            {
                LogHelper.Info<ZoombracoBootstrapper>($"Zoombraco {installedVersion} is installed and up-to-date. Aborting Zoombraco setup.");
                return true;
            }

            try
            {
                // Install our package.
                string resourceName;
                using (Stream package = EmbeddedResourceHelper.GetResource(assembly, "package.xml", out resourceName))
                {
                    PackageHelper packageHelper = new PackageHelper(package, applicationContext);
                    packageHelper.Unpack();

                    using (Stream meta = EmbeddedResourceHelper.GetResource(assembly, "_Meta.cshtml", out resourceName))
                    using (Stream sitemap = EmbeddedResourceHelper.GetResource(assembly, "XmlSitemap.cshtml", out resourceName))
                    {
                        packageHelper.UnpackFile(meta, "_Meta.cshtml");
                        packageHelper.UnpackFile(sitemap, "XmlSitemap.cshtml");
                    }
                }

                // Set default values.
                if (installedVersion == new SemVersion(0))
                {
                    ZoombracoConfiguration.SaveSetting(ZoombracoConstants.Configuration.OutputCacheDuration, "0");
                    ZoombracoConfiguration.SaveSetting(ZoombracoConstants.Configuration.ImageCdnRequestTimeout, "1000");
                    ZoombracoConfiguration.SaveSetting(ZoombracoConstants.Configuration.ModelsBuilderEnabled, "false");
                }

                // Update our version number
                ZoombracoConfiguration.SaveSetting(ZoombracoConstants.Configuration.Version, currentVersion.ToSemanticString());

                return true;
            }
            catch (Exception e)
            {
                LogHelper.Error<ZoombracoBootstrapper>("Unable to install the Zoombraco.", e);
                return false;
            }
        }
    }
}