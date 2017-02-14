// <copyright file="ZoombracoXmlSitemapController.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace Zoombraco.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Web;
    using System.Web.Mvc;

    using Umbraco.Core;
    using Umbraco.Core.Configuration;
    using Umbraco.Core.Configuration.UmbracoSettings;
    using Umbraco.Core.Logging;
    using Umbraco.Web;
    using Umbraco.Web.Routing;
    using Umbraco.Web.Security;

    using Zoombraco.ComponentModel.Caching;
    using Zoombraco.Helpers;
    using Zoombraco.Models;

    /// <summary>
    /// The xml sitemap controller for rendering the XML sitemap.
    /// </summary>
    public class ZoombracoXmlSitemapController : Controller
    {
        /// <summary>
        /// The content helper for content traversal.
        /// </summary>
        private ContentHelper contentHelper;

        /// <summary>
        /// The xml sitemap event handler
        /// </summary>
        /// <param name="sender">The sender <see cref="ZoombracoXmlSitemapController"/></param>
        /// <param name="xmlSitemap">The <see cref="XmlSitemap"/></param>
        public delegate void ProcessXmlSitemapEventHandler(object sender, XmlSitemap xmlSitemap);

        /// <summary>
        /// The event that is called when a xmlsitemap is processed.
        /// </summary>
        public static event ProcessXmlSitemapEventHandler OnProcessXmlSitemap;

        /// <summary>
        /// Gets the collection of doctype aliases representing the doctypes to omit from the XML sitemap.
        /// </summary>
        public static ICollection<string> ExcludedDoctypeAliases { get; } = new List<string>();

        /// <summary>
        /// Represents the result of the default action method for the given path.
        /// </summary>
        /// <returns>
        /// The <see cref="ActionResult"/> representing the result of the action.
        /// </returns>
        /// <exception cref="NullReferenceException">
        /// Returned if no home page is found.
        /// </exception>
        [UmbracoOutputCache]
        public ActionResult XmlSitemap()
        {
            ContentHelper helper = this.GetContentHelper();
            Page home = helper.GetRootNode();

            if (home == null)
            {
                throw new NullReferenceException("Home page not found in content tree.");
            }

            XmlSitemap viewModel = new XmlSitemap();

            try
            {
                viewModel.Items.Add(new SitemapItem(home.UrlAbsolute(), home.UpdateDate, home.ChangeFrequency, home.Priority));
            }
            catch (Exception ex)
            {
                LogHelper.Error<ZoombracoXmlSitemapController>($"Cannot add node with id {home.Id} to the XML sitemap.", ex);
            }

            this.AddChildren(helper, home, viewModel);

            // If any handlers have been added for things like virtual nodes process them.
            ProcessXmlSitemapEventHandler handler = OnProcessXmlSitemap;
            handler?.Invoke(this, viewModel);

            return this.View("XmlSitemap", viewModel);
        }

        /// <summary>
        /// Adds any child items that have the "ExcludeFromXmlSitemap" and "UmbracoNaviHide" properties set
        /// set to false.
        /// </summary>
        /// <param name="helper">The content helper.</param>
        /// <param name="parent">The parent content.</param>
        /// <param name="viewModel">The <see cref="XmlSitemap"/> to add to.</param>
        /// <param name="depth">
        /// The recursion depth.
        /// </param>
        private void AddChildren(ContentHelper helper, Page parent, XmlSitemap viewModel, int depth = 0)
        {
            // Our recursion limit
            if (depth > 10)
            {
                return;
            }

            IEnumerable<Page> children = helper.GetChildren(parent.Id);

            foreach (Page child in children)
            {
                try
                {
                    if (this.IncludeInSiteMap(child))
                    {
                        viewModel.Items.Add(new SitemapItem(child.UrlAbsolute(), child.UpdateDate, child.ChangeFrequency, child.Priority));
                        this.AddChildren(helper, child, viewModel, depth + 1);
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.Error<ZoombracoXmlSitemapController>($"Cannot add node with id {child.Id} to the XML sitemap.", ex);
                }
            }
        }

        /// <summary>
        /// Returns a value indicating whether to include the content in the site map.
        /// </summary>
        /// <param name="content">The content to check against.</param>
        /// <returns>
        /// The <see cref="bool"/> indicating whether to include the content.
        /// </returns>
        private bool IncludeInSiteMap(Page content)
        {
            if (content.ExcludeFromXmlSitemap)
            {
                return false;
            }

            if (ExcludedDoctypeAliases.InvariantContains(content.DocumentTypeAlias))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Gets the content helper for the current request.
        /// </summary>
        /// <returns>The <see cref="ContentHelper"/></returns>
        private ContentHelper GetContentHelper()
        {
            return this.contentHelper ?? (this.contentHelper = new ContentHelper(new UmbracoHelper(this.EnsureUmbracoContext())));
        }

        /// <summary>
        /// Gets an Umbraco Context instance for non pipeline requiests.
        /// </summary>
        /// <returns>The <see cref="UmbracoContext"/></returns>
        private UmbracoContext EnsureUmbracoContext()
        {
            HttpContextBase context = this.HttpContext;
            ApplicationContext application = ApplicationContext.Current;
            WebSecurity security = new WebSecurity(context, application);
            IUmbracoSettingsSection umbracoSettings = UmbracoConfig.For.UmbracoSettings();
            IEnumerable<IUrlProvider> providers = UrlProviderResolver.Current.Providers;
            return UmbracoContext.EnsureContext(context, application, security, umbracoSettings, providers, false);
        }
    }
}