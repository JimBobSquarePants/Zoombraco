// <copyright file="WebApiConfig.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace ZoombracoDemo.Logic
{
    using System.Web.Http;

    /// <summary>
    /// Provides configuration for the Web.Api routes
    /// </summary>
    public static class WebApiConfig
    {
        /// <summary>
        /// Registers the configuration
        /// </summary>
        /// <param name="config">The HttpServer configuration</param>
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional });
        }
    }
}
