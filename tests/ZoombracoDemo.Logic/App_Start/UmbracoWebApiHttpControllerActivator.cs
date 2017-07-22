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
    using Ninject;
    using Ninject.Web.WebApi;

    /// <summary>
    /// The Umbraco HttpController Activator based on the link below with additional constructor injection functionality
    /// <see href="https://our.umbraco.org/forum/core/general/55057-714-WebApi-and-Unity-IOC-brakes-BackOffice"/>
    /// </summary>
    public class UmbracoWebApiHttpControllerActivator : IHttpControllerActivator
    {
        private readonly DefaultHttpControllerActivator defaultHttpControllerActivator;
        private readonly IKernel kernel;

        /// <summary>
        /// Initializes a new instance of the <see cref="UmbracoWebApiHttpControllerActivator"/> class.
        /// </summary>
        /// <param name="kernel">The kernel super-factory</param>
        public UmbracoWebApiHttpControllerActivator(IKernel kernel)
        {
            this.kernel = kernel;
            this.defaultHttpControllerActivator = new DefaultHttpControllerActivator();
        }

        /// <inheritdoc/>
        public IHttpController Create(HttpRequestMessage request, HttpControllerDescriptor controllerDescriptor, Type controllerType)
        {
            IHttpController instance;
            if (this.IsUmbracoController(controllerType))
            {
                instance = this.defaultHttpControllerActivator.Create(request, controllerDescriptor, controllerType);
            }
            else
            {
                // Using the NinjectDependencyScope allows use to implement constructor injection
                using (var scope = new NinjectDependencyScope(this.kernel))
                {
                    instance = scope.GetService(controllerType) as IHttpController;
                }
            }

            return instance;
        }

        private bool IsUmbracoController(Type controllerType)
        {
            return controllerType.Namespace != null
                   && controllerType.Namespace.StartsWith("umbraco", StringComparison.InvariantCultureIgnoreCase);
        }
    }
}