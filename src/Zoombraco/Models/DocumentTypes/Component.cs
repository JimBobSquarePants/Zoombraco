// <copyright file="Component.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace Zoombraco.Models
{
    using System;
    using System.Collections.Generic;
    using Our.Umbraco.Ditto;
    using Umbraco.Core.Models;
    using Umbraco.Web;
    using Zoombraco.Helpers;

    /// <summary>
    /// Represents a single published component in the content section.
    /// </summary>
    [DittoLazy]
    [UmbracoPicker]
    public class Component : IEntity, INavigation
    {
        /// <summary>
        /// The content helper
        /// </summary>
        private ContentHelper contentHelper;

        /// <inheritdoc/>
        public virtual int Id { get; set; }

        /// <inheritdoc/>
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
        public T Parent<T>()
        {
            return this.ContentHelper().GetParent<T>(this.Id);
        }

        /// <inheritdoc/>
        public IEnumerable<T> Ancestors<T>(int maxLevel = int.MaxValue, Func<IPublishedContent, bool> predicate = null)
        {
            return this.ContentHelper().GetAncestors<T>(this.Id, maxLevel, predicate);
        }

        /// <inheritdoc/>
        public IEnumerable<T> Children<T>(Func<IPublishedContent, bool> predicate = null)
        {
            return this.ContentHelper().GetChildren<T>(this.Id, predicate);
        }

        /// <inheritdoc/>
        public IEnumerable<T> Descendents<T>(int level = 0, Func<IPublishedContent, bool> predicate = null)
        {
            return this.ContentHelper().GetDescendants<T>(this.Id, level, predicate);
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