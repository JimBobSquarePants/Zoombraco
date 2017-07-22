// <copyright file="GenericApiController.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace ZoombracoDemo.Logic.Controllers
{
    using System.Web.Http;
    using Umbraco.Web.WebApi;
    using ZoombracoDemo.Logic.Services;

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

        [Route("api/generic/{id}")]
        [HttpGet]
        public IHttpActionResult GetById(int id)
        {
            return this.Ok(this.genericService.GetById(id));
        }
    }
}