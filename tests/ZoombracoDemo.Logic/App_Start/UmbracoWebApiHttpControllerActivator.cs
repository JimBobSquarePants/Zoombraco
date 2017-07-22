// <copyright file="UmbracoWebApiHttpControllerActivator.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace ZoombracoDemo.Logic
{
    using System;
    using System.Net.Http;
    using System.Web.Http.Controllers;
    using System.Web.Http.Dispatcher;

    /// <summary>
    /// The Umbraco HttpController Activator
    /// <see href="https://our.umbraco.org/forum/core/general/55057-714-WebApi-and-Unity-IOC-brakes-BackOffice"/>
    /// </summary>
    public class UmbracoWebApiHttpControllerActivator : IHttpControllerActivator
    {
        private readonly DefaultHttpControllerActivator defaultHttpControllerActivator;

        /// <summary>
        /// Initializes a new instance of the <see cref="UmbracoWebApiHttpControllerActivator"/> class.
        /// </summary>
        public UmbracoWebApiHttpControllerActivator()
        {
            this.defaultHttpControllerActivator = new DefaultHttpControllerActivator();
        }

        /// <inheritdoc/>
        public IHttpController Create(HttpRequestMessage request, HttpControllerDescriptor controllerDescriptor, Type controllerType)
        {
            IHttpController instance =
                this.IsUmbracoController(controllerType)
                    ? Activator.CreateInstance(controllerType) as IHttpController
                    : this.defaultHttpControllerActivator.Create(request, controllerDescriptor, controllerType);

            return instance;
        }

        private bool IsUmbracoController(Type controllerType)
        {
            return controllerType.Namespace != null
                   && controllerType.Namespace.StartsWith("umbraco", StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
