// <copyright file="ZoombracoApplicationEvents.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace Zoombraco
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Web.Http;
    using System.Web.Routing;
    using DevTrends.MvcDonutCaching;
    using Examine;
    using ImageProcessor.Web.HttpModules;
    using Our.Umbraco.Ditto;
    using Umbraco.Core;
    using Umbraco.Core.Cache;
    using Umbraco.Core.Logging;
    using Umbraco.Core.Models;
    using Umbraco.Web;
    using Umbraco.Web.Cache;
    using Umbraco.Web.Mvc;
    using UmbracoExamine;
    using Zoombraco.Caching;
    using Zoombraco.ComponentModel.Processors;
    using Zoombraco.ComponentModel.Search;
    using Zoombraco.Controllers;
    using Zoombraco.Helpers;
    using Zoombraco.Models;

    /// <summary>
    /// Runs the handling code for appication specific events.
    /// </summary>
    public class ZoombracoApplicationEvents : ApplicationEventHandler
    {
        /// <summary>
        /// The cache for storing type property information.
        /// </summary>
        private static readonly ConcurrentDictionary<Type, PropertyInfo[]> PropertyCache
            = new ConcurrentDictionary<Type, PropertyInfo[]>();

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
            // Try and install the package.
            if (!ZoombracoBootstrapper.Install(umbracoApplication, applicationContext))
            {
                return;
            }

            // Register custom routes.
            RouteBuilder.RegisterRoutes(RouteTable.Routes);

            // Ensure that the xml formatter does not interfere with API controllers.
            GlobalConfiguration.Configuration.Formatters.XmlFormatter.SupportedMediaTypes.Clear();

            // Clear out any CDN url requests on new image processing.
            // This ensures that when we update an image the cached CDN result for rendering is deleted.
            ImageProcessingModule.OnPostProcessing += (s, e) => { ZoombracoApplicationCache.RemoveItem(e.Context.Request.Unvalidated.RawUrl); };

            // Assign indexer for full text searching.
            UmbracoContentIndexer indexProvider = ExamineManager.Instance.IndexProviderCollection[ZoombracoConstants.Search.IndexerName] as UmbracoContentIndexer;
            UmbracoExamineSearcher searchProvider = ExamineManager.Instance.SearchProviderCollection[ZoombracoConstants.Search.SearcherName] as UmbracoExamineSearcher;

            if (indexProvider == null)
            {
                throw new ArgumentOutOfRangeException($"{ZoombracoConstants.Search.IndexerName} is missing. Please check the ExamineSettings.config file.");
            }

            if (searchProvider == null)
            {
                throw new ArgumentOutOfRangeException($"{ZoombracoConstants.Search.SearcherName} is missing. Please check the ExamineSettings.config file.");
            }

            UmbracoHelper helper = new UmbracoHelper(UmbracoContext.Current);
            ContentHelper contentHelper = new ContentHelper(helper);
            indexProvider.GatheringNodeData += (sender, e) => this.GatheringNodeData(sender, e, helper, contentHelper, applicationContext);

            // Register the VortoPropertyAttribute as the default property processor so that any property can be made multilingual.
            Ditto.RegisterDefaultProcessorType<VortoPropertyAttribute>();
        }

        /// <summary>
        /// Gathers the information from each node to add to the Examine index.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arguments containing information about the nodes to be gathered.</param>
        /// <param name="helper">The <see cref="UmbracoHelper"/> to help gather node data.</param>
        /// <param name="contentHelper">The <see cref="ContentHelper"/> to help gather stong typed data.</param>
        /// <param name="applicationContext">
        /// The Umbraco <see cref="ApplicationContext"/> for the current application.
        /// </param>
        // ReSharper disable once UnusedParameter.Local
        private void GatheringNodeData(object sender, IndexingNodeDataEventArgs e, UmbracoHelper helper, ContentHelper contentHelper, ApplicationContext applicationContext)
        {
            IPublishedContent content = null;
            switch (e.IndexType)
            {
                case IndexTypes.Content:
                    content = helper.TypedContent(e.NodeId);
                    break;

                case IndexTypes.Media:
                    content = helper.TypedMedia(e.NodeId);
                    break;
            }

            if (content != null)
            {
                // Convert the property and use reflection to grab the output property value adding it to the merged property
                // collection for each configured language.
                Type doctype = contentHelper.GetRegisteredType(content.DocumentTypeAlias);

                if (doctype != null)
                {
                    // Check to see if the type is searchable. If not, we want to break out.
                    // If the interface is not implemented then we add that to the index since it is not explicitly ommited.
                    if (typeof(ISearchable).IsAssignableFrom(doctype))
                    {
                        // TODO: Ask Shannon about this.
                        // Depending on timing using IPublishedContent and searching the parent can fail.
                        // This sucks obviously since the ContentService is slow.
                        IContent c = applicationContext.Services.ContentService.GetById(e.NodeId);
                        if (c.Published
                            && c.HasProperty(ZoombracoConstants.Content.ExcludeFromSearchResults)
                            && c.GetValue<bool>(ZoombracoConstants.Content.ExcludeFromSearchResults))
                        {
                            return;
                        }

                        if (c.Ancestors().Any(i => i.Published
                            && i.HasProperty(ZoombracoConstants.Content.ExcludeFromSearchResults)
                            && i.GetValue<bool>(ZoombracoConstants.Content.ExcludeFromSearchResults)))
                        {
                            return;
                        }
                    }

                    // TODO: How much does calling this service cost?
                    ILanguage[] languages = applicationContext.Services.LocalizationService.GetAllLanguages().ToArray();
                    StringBuilder categoryStringBuilder = new StringBuilder();
                    StringBuilder[] mergedDataStringBuilders = new StringBuilder[languages.Length];
                    for (int i = 0; i < mergedDataStringBuilders.Length; i++)
                    {
                        mergedDataStringBuilders[i] = new StringBuilder();
                    }

                    // We cache the properties for to improve performance.
                    PropertyInfo[] properties;
                    PropertyCache.TryGetValue(doctype, out properties);

                    if (properties == null)
                    {
                        properties = doctype.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                            .Where(x => x.CanWrite && x.GetSetMethod() != null)
                                            .ToArray();

                        PropertyCache.TryAdd(doctype, properties);
                    }

                    // Loop through each peoperty and check if we should add it to the fields
                    foreach (PropertyInfo property in properties)
                    {
                        UmbracoSearchMergedFieldAttribute attr = property.GetCustomAttribute<UmbracoSearchMergedFieldAttribute>();

                        if (attr == null)
                        {
                            continue;
                        }

                        // Get the correct identifier for the property.
                        string key = !string.IsNullOrWhiteSpace(attr.ExamineKey) ? attr.ExamineKey : property.Name;

                        // Look for any custom search resolvers to convert the information to a useful search result.
                        ZoombracoSearchResolverAttribute[] resolverAttributes = property.GetCustomAttributes<ZoombracoSearchResolverAttribute>().ToArray();

                        foreach (KeyValuePair<string, string> field in e.Fields)
                        {
                            if (!key.InvariantEquals(field.Key))
                            {
                                continue;
                            }

                            // Combine property values for each language.
                            for (int i = 0; i < languages.Length; i++)
                            {
                                if (resolverAttributes.Any())
                                {
                                    // Resolve each value and append to the language stringbuilder.
                                    foreach (ZoombracoSearchResolverAttribute attribute in resolverAttributes)
                                    {
                                        string value = attribute.ResolveValue(content, property, field.Key, field.Value, languages[i].CultureInfo);
                                        if (!string.IsNullOrWhiteSpace(value))
                                        {
                                            mergedDataStringBuilders[i].AppendFormat(" {0} ", helper.StripHtml(value, null));
                                        }
                                    }
                                }
                                else
                                {
                                    if (!string.IsNullOrWhiteSpace(field.Value))
                                    {
                                        mergedDataStringBuilders[i].AppendFormat("{0} ", helper.StripHtml(field.Value, null));
                                    }
                                }
                            }

                            // Jump out early
                            break;
                        }
                    }

                    // Combine categories.
                    UmbracoSearchCategoryAttribute categoryAttribute = doctype.GetCustomAttribute<UmbracoSearchCategoryAttribute>();

                    if (categoryAttribute != null)
                    {
                        if (categoryAttribute.Categories.Any())
                        {
                            foreach (string category in categoryAttribute.Categories)
                            {
                                categoryStringBuilder.AppendFormat("{0} ", category);
                            }
                        }
                    }

                    // Now add the site, categories, and merged data.
                    e.Fields.Add(ZoombracoConstants.Search.SiteField, e.Fields["path"].Replace(",", " "));
                    e.Fields[ZoombracoConstants.Search.CategoryField] = categoryStringBuilder.ToString().Trim();
                    for (int i = 0; i < languages.Length; i++)
                    {
                        e.Fields[string.Format(ZoombracoConstants.Search.MergedDataFieldTemplate, languages[i].CultureInfo.Name)] = mergedDataStringBuilders[i].ToString().Trim();
                    }
                }
            }
        }

        /// <summary>
        /// The page cache refresher cache updated event handler.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="CacheRefresherEventArgs"/> containing information about the event.</param>
        private void PageCacheRefresherCacheUpdated(PageCacheRefresher sender, CacheRefresherEventArgs e)
        {
            this.ClearCache();
        }

        /// <summary>
        /// The media cache refresher cache updated event handler.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="CacheRefresherEventArgs"/> containing information about the event.</param>
        private void MediaCacheRefresherCacheUpdated(MediaCacheRefresher sender, CacheRefresherEventArgs e)
        {
            this.ClearCache();
        }

        /// <summary>
        /// The media cache refresher cache updated event handler.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="CacheRefresherEventArgs"/> containing information about the event.</param>
        private void MemberCacheRefresherCacheUpdated(MemberCacheRefresher sender, CacheRefresherEventArgs e)
        {
            this.ClearCache();
        }

        /// <summary>
        /// Clears the output cache of all items.
        /// </summary>
        private void ClearCache()
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