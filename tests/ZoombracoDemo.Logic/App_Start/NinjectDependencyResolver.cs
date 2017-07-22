// <copyright file="NinjectDependencyResolver.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace ZoombracoDemo.Logic
{
    using System.Web.Http.Dependencies;
    using Ninject;
    using Ninject.Web.WebApi;

    /// <summary>
    /// The Ninject dependency resolver
    /// </summary>
    public class NinjectDependencyResolver : NinjectDependencyScope, IDependencyResolver, System.Web.Mvc.IDependencyResolver
    {
        private readonly IKernel kernel;

        /// <summary>
        /// Initializes a new instance of the <see cref="NinjectDependencyResolver"/> class.
        /// </summary>
        /// <param name="kernel">The kernel super-factory</param>
        public NinjectDependencyResolver(IKernel kernel)
            : base(kernel)
        {
            this.kernel = kernel;
        }

        /// <summary>
        /// Begins the dependency scope
        /// </summary>
        /// <returns>The <see cref="IDependencyScope"/></returns>
        public IDependencyScope BeginScope()
        {
            return new NinjectDependencyScope(this.kernel);
        }
    }
}