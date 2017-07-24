// <copyright file="NinjectWeb.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(ZoombracoDemo.Logic.NinjectWeb), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethod(typeof(ZoombracoDemo.Logic.NinjectWeb), "Stop")]

namespace ZoombracoDemo.Logic
{
    using System;
    using System.Web;
    using System.Web.Http;
    using System.Web.Http.Dispatcher;
    using System.Web.Mvc;
    using Microsoft.Web.Infrastructure.DynamicModuleHelper;
    using Ninject;
    using Ninject.Web.Common;

    /// <summary>
    /// Initializes common dependency bootstrapping
    /// See <see href="https://gist.github.com/deMD/c818f694c678af8ae3476b630ca6cbb0"/>
    /// </summary>
    public static class NinjectWeb
    {
        private static readonly Bootstrapper Bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start()
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
        }

        /// <summary>
        /// Starts the Umbraco application
        /// </summary>
        /// <returns>The created kernel</returns>
        public static IKernel UmbracoStart()
        {
            Bootstrapper.Initialize(CreateKernel);
            return Bootstrapper.Kernel;
        }

        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            Bootstrapper.ShutDown();
        }

        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                RegisterServices(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            // Bind the common services.
            CommonServices.Register(kernel);

            // Set the resolvers for both the MVC architecture and the Web API architecture in that order
            DependencyResolver.SetResolver(new NinjectDependencyResolver(kernel));

            // Don't use the default dependency resolver as it causes the Umbraco backoffice to immediately log out. Instead use a
            // custom activator based on the code descriped at: https://our.umbraco.org/forum/core/general/55057-714-WebApi-and-Unity-IOC-brakes-BackOffice
            // GlobalConfiguration.Configuration.DependencyResolver = new NinjectDependencyResolver(kernel);
            GlobalConfiguration.Configuration.Services.Replace(typeof(IHttpControllerActivator), new UmbracoWebApiHttpControllerActivator(kernel));
        }
    }
}