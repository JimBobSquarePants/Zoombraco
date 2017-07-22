// <copyright file="HomeController.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace ZoombracoDemo.Logic.Controllers
{
    using System.Web.Mvc;
    using Umbraco.Web.Models;
    using Zoombraco.ComponentModel.Caching;
    using Zoombraco.Controllers;
    using Zoombraco.Models;
    using ZoombracoDemo.Logic.Models;
    using ZoombracoDemo.Logic.Services;

    /// <summary>
    /// The home page controller
    /// </summary>
    [UmbracoOutputCache]
    public class HomeController : ZoombracoController
    {
        private readonly IHomeService homeService;

        /// <summary>
        /// Initializes a new instance of the <see cref="HomeController"/> class.
        /// </summary>
        /// <param name="homeService">The home service</param>
        public HomeController(IHomeService homeService)
        {
            this.homeService = homeService;
        }

        /// <inheritdoc />
        public override ActionResult Index(RenderModel model)
        {
            RenderPage<Home> viewModel = new RenderPage<Home>(this.homeService.GetById(model.Content.Id));

            return this.CurrentTemplate(viewModel);
        }
    }
}