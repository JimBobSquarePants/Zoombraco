// <copyright file="ZoombracoApiController.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace Zoombraco.Controllers
{
    using Umbraco.Web;
    using Umbraco.Web.WebApi;

    using Zoombraco.Helpers;

    /// <summary>
    /// The base class for autorouted API controllers.
    /// </summary>
    public abstract class ZoombracoApiController : UmbracoApiController
    {
        /// <summary>
        /// The content helper.
        /// </summary>
        private ContentHelper contentHelper;

        /// <summary>
        /// Initializes a new instance of the <see cref="ZoombracoApiController"/> class.
        /// Empty constructor, uses Singleton to resolve the UmbracoContext
        /// </summary>
        protected ZoombracoApiController()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ZoombracoApiController"/> class.
        /// </summary>
        /// <param name="umbracoContext">The Umbraco context</param>
        protected ZoombracoApiController(UmbracoContext umbracoContext)
          : base(umbracoContext)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ZoombracoApiController"/> class.
        /// </summary>
        /// <param name="umbracoContext">The Umbraco context.</param>
        /// <param name="umbracoHelper">The Umbraco helper.</param>
        protected ZoombracoApiController(UmbracoContext umbracoContext, UmbracoHelper umbracoHelper)
          : base(umbracoContext, umbracoHelper)
        {
        }

        /// <summary>
        /// Gets the content helper.
        /// </summary>
        public virtual ContentHelper ContentHelper => this.contentHelper ?? (this.contentHelper = new ContentHelper(this.Umbraco));
    }
}