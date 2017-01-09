// <copyright file="GenericController.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace ZoombracoDemo.Logic.Controllers
{
    using System.Web.Mvc;

    using Our.Umbraco.Ditto;

    using Umbraco.Web.Models;

    using Zoombraco.ComponentModel.Caching;
    using Zoombraco.Controllers;
    using Zoombraco.Models;

    using ZoombracoDemo.Logic.Models;

    /// <summary>
    /// The Generic page controller
    /// </summary>
    [UmbracoOutputCache]
    public class GenericController : ZoombracoController
    {
        /// <inheritdoc />
        public override ActionResult Index(RenderModel model)
        {
            Generic generic = model.As<Generic>();
            RenderPage<Generic> viewModel = new RenderPage<Generic>(generic);

            return this.CurrentTemplate(viewModel);
        }
    }
}