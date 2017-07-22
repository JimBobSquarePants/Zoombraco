// <copyright file="GenericApiController.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace ZoombracoDemo.Logic.Controllers
{
    using System.Web.Http;
    using ComponentModel.Attributes;
    using Services;
    using Umbraco.Web.WebApi;

    /// <summary>
    /// The Generic api controller
    /// </summary>
    public class GenericApiController : UmbracoApiController
    {
        private readonly IGenericService genericService;

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericApiController"/> class.
        /// </summary>
        /// <param name="genericService">The generic service</param>
        public GenericApiController(IGenericService genericService)
        {
            this.genericService = genericService;
        }

        /// <summary>
        /// Returns the generic page that matches the given id
        /// </summary>
        /// <param name="id">The page id</param>
        /// <returns>The <see cref="IHttpActionResult"/></returns>
        [RouteVersion("api/generic/{id}", 1)]
        [HttpGet]
        public IHttpActionResult GetById(int id)
        {
            return this.Ok(this.genericService.GetById(id));
        }
    }
}