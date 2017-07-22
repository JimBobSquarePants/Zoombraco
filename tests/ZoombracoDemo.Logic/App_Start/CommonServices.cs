// <copyright file="CommonServices.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace ZoombracoDemo.Logic
{
    using Ninject;
    using Ninject.Web.Common;
    using ZoombracoDemo.Logic.Services;

    /// <summary>
    /// The CommonServices are services that are a 1:1 pair with an implementing class.
    /// </summary>
    public class CommonServices
    {
        /// <summary>
        /// Registers the common services
        /// </summary>
        /// <param name="kernel">The super-factory</param>
        public static void Register(IKernel kernel)
        {
            kernel.Bind<IGenericService>().To<PublishedGenericService>().InRequestScope();
            kernel.Bind<IHomeService>().To<PublishedHomeService>().InRequestScope();
        }
    }
}
