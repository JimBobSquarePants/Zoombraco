// <copyright file="ZoombracoDemoLogicEvents.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace ZoombracoDemo.Logic
{
    using System.Web.Http;
    using Umbraco.Core;

    /// <summary>
    /// Contains code for responding to application-level events raised by ASP.NET or by HttpModules.
    /// </summary>
    public class ZoombracoDemoLogicEvents : ApplicationEventHandler
    {
        /// <summary>
        /// Boot-up is completed, this allows you to perform any other boot-up logic required for the application.
        /// Resolution is frozen so now they can be used to resolve instances.
        /// </summary>
        /// <param name="umbracoApplication">
        /// The current <see cref="UmbracoApplicationBase"/>
        /// </param>
        /// <param name="applicationContext">
        /// The Umbraco <see cref="ApplicationContext"/> for the current application.
        /// </param>
        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);

            // IOC bindings need to be set once resolution is complete
            // https://our.umbraco.org/documentation/reference/using-ioc
            NinjectWeb.UmbracoStart();
        }
    }
}