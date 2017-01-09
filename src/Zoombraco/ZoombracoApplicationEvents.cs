// <copyright file="ZoombracoApplicationEvents.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace Zoombraco
{
    using System;
    using System.Web.Http;
    using System.Web.Routing;

    using Controllers;
    using DevTrends.MvcDonutCaching;

    using ImageProcessor.Web.HttpModules;

    using Umbraco.Core;
    using Umbraco.Core.Cache;
    using Umbraco.Core.Logging;
    using Umbraco.Web.Cache;
    using Umbraco.Web.Mvc;

    using Zoombraco.Caching;

    /// <summary>
    /// Runs the handling code for appication specific events.
    /// </summary>
    public class ZoombracoApplicationEvents : ApplicationEventHandler
    {
        /// <summary>
        /// Gets the outputcache manager.
        /// </summary>
        public OutputCacheManager OutputCacheManager => new OutputCacheManager();

        /// <summary>
        /// Overridable method to execute when all resolvers have been initialized but resolution is not frozen so they
        /// can be modified in this method.
        /// </summary>
        /// <param name="umbracoApplication">
        /// The current <see cref="UmbracoApplicationBase"/>
        /// </param>
        /// <param name="applicationContext">cal
        /// The Umbraco <see cref="ApplicationContext"/> for the current application.
        /// </param>
        protected override void ApplicationStarting(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            // By registering this here we can make sure that if route hijacking doesn't find a controller it will use this controller.
            // That way each page will always be routed through one of our controllers.
            DefaultRenderMvcControllerResolver.Current.SetDefaultControllerType(typeof(ZoombracoController));

            // Assign events for managing content caching.
            PageCacheRefresher.CacheUpdated += this.PageCacheRefresherCacheUpdated;
            MediaCacheRefresher.CacheUpdated += this.MediaCacheRefresherCacheUpdated;
            MemberCacheRefresher.CacheUpdated += this.MemberCacheRefresherCacheUpdated;
        }

        /// <summary>
        /// Boot-up is completed, this allows you to perform any other boot-up logic required for the application.
        /// Resolution is frozen so now they can be used to resolve instances.
        /// </summary>
        /// <param name="umbracoApplication">
        /// The current <see cref="UmbracoApplicationBase"/>
        /// </param>
        /// <param name="applicationContext">
        /// The Umbraco <see cref="ApplicationContext"/> for the current application.
        /// </param>
        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            // Register custom routes.
            RouteBuilder.RegisterRoutes(RouteTable.Routes);

            // Ensure that the xml formatter does not interfere with API controllers.
            GlobalConfiguration.Configuration.Formatters.XmlFormatter.SupportedMediaTypes.Clear();

            // Clear out any Cdn url requests on new image processing.
            // This ensures that when we update an image the cached CDN result for rendering is deleted.
            ImageProcessingModule.OnPostProcessing += (s, e) => { ZoombracoApplicationCache.RemoveItem(e.Context.Request.Unvalidated.RawUrl); };
        }

        /// <summary>
        /// The page cache refresher cache updated event handler.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="CacheRefresherEventArgs"/> containing information about the event.</param>
        private void PageCacheRefresherCacheUpdated(PageCacheRefresher sender, CacheRefresherEventArgs e)
        {
            try
            {
                // Remove all caches. Donut cache changes how it stores keys too often to keep up with.
                this.OutputCacheManager.RemoveItems();
                LogHelper.Info<ZoombracoApplicationEvents>($"Donut Output Cache cleared on Server:{Environment.MachineName} AppDomain:{AppDomain.CurrentDomain.FriendlyName} for all items");
            }
            catch (Exception ex)
            {
                LogHelper.Error<ZoombracoApplicationEvents>($"Donut Output Cache failed to clear on Server:{Environment.MachineName} AppDomain:{AppDomain.CurrentDomain.FriendlyName}", ex);
            }
        }

        /// <summary>
        /// The media cache refresher cache updated event handler.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="CacheRefresherEventArgs"/> containing information about the event.</param>
        private void MediaCacheRefresherCacheUpdated(MediaCacheRefresher sender, CacheRefresherEventArgs e)
        {
            try
            {
                // Remove all caches.
                this.OutputCacheManager.RemoveItems();
                LogHelper.Info<ZoombracoApplicationEvents>($"Donut Output Cache cleared on Server:{Environment.MachineName} AppDomain:{AppDomain.CurrentDomain.FriendlyName} for all items");
            }
            catch (Exception ex)
            {
                LogHelper.Error<ZoombracoApplicationEvents>($"Donut Output Cache failed to clear on Server:{Environment.MachineName} AppDomain:{AppDomain.CurrentDomain.FriendlyName}", ex);
            }
        }

        /// <summary>
        /// The media cache refresher cache updated event handler.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="CacheRefresherEventArgs"/> containing information about the event.</param>
        private void MemberCacheRefresherCacheUpdated(MemberCacheRefresher sender, CacheRefresherEventArgs e)
        {
            try
            {
                // Remove all caches.
                this.OutputCacheManager.RemoveItems();
                LogHelper.Info<ZoombracoApplicationEvents>($"Donut Output Cache cleared on Server:{Environment.MachineName} AppDomain:{AppDomain.CurrentDomain.FriendlyName} for all items");
            }
            catch (Exception ex)
            {
                LogHelper.Error<ZoombracoApplicationEvents>($"Donut Output Cache failed to clear on Server:{Environment.MachineName} AppDomain:{AppDomain.CurrentDomain.FriendlyName}", ex);
            }
        }
    }
}