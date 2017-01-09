// <copyright file="HomeController.cs" company="James Jackson-South">
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
    /// The home page controller
    /// </summary>
    [UmbracoOutputCache]
    public class HomeController : ZoombracoController
    {
        /// <inheritdoc />
        public override ActionResult Index(RenderModel model)
        {
            Home home = model.As<Home>();
            RenderPage<Home> viewModel = new RenderPage<Home>(home);

            return this.CurrentTemplate(viewModel);
        }
    }
}