// <copyright file="ZoombracoDemoLogicEvents.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace ZoombracoDemo.Logic
{
    using Umbraco.Core;

    /// <summary>
    /// Contains code for responding to application-level events raised by ASP.NET or by HttpModules.
    /// </summary>
    public class ZoombracoDemoLogicEvents : ApplicationEventHandler
    {
        /// <summary>
        /// Overridable method to execute when all resolvers have been initialized but resolution is not frozen so they
        /// can be modified in this method.
        /// </summary>
        /// <param name="umbracoApplication">
        /// The current <see cref="UmbracoApplicationBase"/>
        /// </param>
        /// <param name="applicationContext">cal
        /// The Umbraco <see cref="ApplicationContext"/> for the current application.
        /// </param>
        protected override void ApplicationStarting(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            NinjectWebCommon.UmbracoStart();
        }
    }
}