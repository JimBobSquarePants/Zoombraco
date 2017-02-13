// <copyright file="RouteBuilder.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace Zoombraco
{
    using System.Web.Mvc;
    using System.Web.Routing;

    /// <summary>
    /// Handles the registration of custom routes.
    /// </summary>
    internal static class RouteBuilder
    {
        /// <summary>
        /// The object to lock against.
        /// </summary>
        private static readonly object Locker = new object();

        /// <summary>
        /// Registers a collection of routes.
        /// </summary>
        /// <param name="routes">The collection of routes for ASP.NET routing.</param>
        public static void RegisterRoutes(RouteCollection routes)
        {
            lock (Locker)
            {
                if (routes["XmlSitemapRAMMFAR"] == null)
                {
                    // Requires RAMMFAR to be enabled for the application.
                    routes.MapRoute(
                        "XmlSitemapRAMMFAR",
                        "sitemap.xml",
                        new { controller = "ZoombracoXmlSitemap", action = "XmlSitemap" });
                }

                if (routes["XmlSitemap"] == null)
                {
                    // Non RAMMFAR version.
                    routes.MapRoute(
                        "XmlSitemap",
                        "sitemap-xml",
                        new { controller = "ZoombracoXmlSitemap", action = "XmlSitemap" });
                }

                if (routes[ZoombracoConstants.EmbeddedResources.EmbeddedResourcesRoute] == null)
                {
                    // Embedded resources
                    routes.MapRoute(
                        ZoombracoConstants.EmbeddedResources.EmbeddedResourcesRoute,
                        "App_Plugins/Zoombraco/GetResource/{fileName}",
                        new { controller = "ZoombracoEmbeddedResource", action = "GetResource" },
                        new[] { "Zoombraco" });
                }
            }
        }
    }
}