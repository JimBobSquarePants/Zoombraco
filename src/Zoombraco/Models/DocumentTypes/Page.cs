// <copyright file="Page.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace Zoombraco.Models
{
    using System;
    using System.Collections.Generic;
    using System.Web;
    using Our.Umbraco.Ditto;
    using Our.Umbraco.Ditto.ComponentModel.Attributes;
    using Umbraco.Core;
    using Umbraco.Web;
    using Zoombraco.ComponentModel.Search;
    using Zoombraco.Extensions;
    using Zoombraco.Helpers;

    /// <summary>
    /// Represents a single published page in the content section.
    /// </summary>
    [DittoLazy]
    [UmbracoPicker]
    public class Page : IEntity, IMeta, IXmlSitemap, ISearchable, IUrl, INavigation
    {
        /// <summary>
        /// The content helper
        /// </summary>
        private ContentHelper contentHelper;

        /// <inheritdoc/>
        public virtual int Id { get; set; }

        /// <inheritdoc/>
        [UmbracoSearchMergedField(ZoombracoConstants.SearchConstants.NodeName)]
        public virtual string Name { get; set; }

        /// <inheritdoc/>
        public virtual string DocumentTypeAlias { get; set; }

        /// <inheritdoc/>
        public virtual int Level { get; set; }

        /// <inheritdoc/>
        public virtual DateTime CreateDate { get; set; }

        /// <inheritdoc/>
        public virtual DateTime UpdateDate { get; set; }

        /// <inheritdoc/>
        [UmbracoProperty(AltPropertyName = ZoombracoConstants.Content.Name)]
        public virtual string BrowserTitle { get; set; }

        /// <inheritdoc/>
        public virtual string BrowserDescription { get; set; }

        /// <inheritdoc/>
        [UmbracoProperty(AltPropertyName = ZoombracoConstants.Content.Name)]
        public virtual string OpenGraphTitle { get; set; }

        /// <inheritdoc/>
        public virtual OpenGraphType OpenGraphType { get; set; }

        /// <inheritdoc/>
        [UmbracoProperty(Recursive = true)]
        public virtual Image OpenGraphImage { get; set; }

        /// <inheritdoc/>
        [DittoIgnore]
        public string OpenGraphUrl => this.UrlAbsolute();

        /// <inheritdoc/>
        public virtual ChangeFrequency ChangeFrequency { get; set; }

        /// <inheritdoc/>
        public virtual decimal Priority { get; set; } = 0.5M;

        /// <inheritdoc/>
        [UmbracoProperty(Recursive = true)]
        public virtual bool ExcludeFromXmlSitemap { get; set; }

        /// <inheritdoc/>
        [UmbracoProperty(Recursive = true)]
        public bool ExcludeFromSearchResults { get; set; }

        /// <inheritdoc/>
        [UmbracoProperty(Constants.Conventions.Content.UrlName, ZoombracoConstants.Content.Url)]
        public virtual string Url { get; set; }

        /// <inheritdoc/>
        public virtual string UrlAbsolute()
        {
            string url = this.ContentHelper().UmbracoHelper.UrlAbsolute(this.Id);

            // Certain virtual pages such as Articulate blog pages only return a relative url.
            if (!url.IsAbsoluteUrl())
            {
                string root = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority);
                url = new Uri(new Uri(root, UriKind.Absolute), url).ToString();
            }

            return url;
        }

        /// <inheritdoc/>
        public T Parent<T>()
        {
            return this.ContentHelper().GetParent<T>(this.Id);
        }

        /// <inheritdoc/>
        public IEnumerable<T> Ancestors<T>(int maxLevel = int.MaxValue)
        {
            return this.ContentHelper().GetAncestors<T>(this.Id, maxLevel);
        }

        /// <inheritdoc/>
        public IEnumerable<T> Children<T>()
        {
            return this.ContentHelper().GetChildren<T>(this.Id);
        }

        /// <inheritdoc/>
        public IEnumerable<T> Descendents<T>(int level = int.MaxValue)
        {
            return this.ContentHelper().GetDescendants<T>(this.Id, level);
        }

        /// <summary>
        /// Gets the content helper bound to the current Umbraco context
        /// </summary>
        /// <returns>The <see cref="ContentHelper"/></returns>
        protected virtual ContentHelper ContentHelper()
        {
            return this.contentHelper ?? (this.contentHelper = new ContentHelper(new UmbracoHelper(UmbracoContext.Current)));
        }
    }
}