// <copyright file="ZoombracoViewPage{T}.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace Zoombraco.Views
{
    using Umbraco.Web.Mvc;

    using Zoombraco.Helpers;

    /// <summary>
    /// The view that all Zoombraco views inherit.
    /// </summary>
    /// <typeparam name="T">The type of object to create the view for.</typeparam>
    public abstract class ZoombracoViewPage<T> : UmbracoViewPage<T>
    {
        /// <summary>
        /// The content helper
        /// </summary>
        private ContentHelper contentHelper;

        /// <summary>
        /// Gets the content helper.
        /// </summary>
        public virtual ContentHelper ContentHelper => this.contentHelper ?? (this.contentHelper = new ContentHelper(this.Umbraco));
    }
}