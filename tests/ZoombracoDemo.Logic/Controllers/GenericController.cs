// <copyright file="GenericController.cs" company="James Jackson-South">
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
    /// The Generic page controller
    /// </summary>
    [UmbracoOutputCache]
    public class GenericController : ZoombracoController
    {
        private readonly IGenericService genericService;

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericController"/> class.
        /// </summary>
        /// <param name="genericService">The generic service</param>
        public GenericController(IGenericService genericService)
        {
            this.genericService = genericService;
        }

        /// <inheritdoc />
        public override ActionResult Index(RenderModel model)
        {
            RenderPage<Generic> viewModel = new RenderPage<Generic>(this.genericService.GetById(model.Content.Id));

            return this.CurrentTemplate(viewModel);
        }
    }
}