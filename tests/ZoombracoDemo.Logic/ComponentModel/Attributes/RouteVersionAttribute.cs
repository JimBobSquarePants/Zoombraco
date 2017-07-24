// <copyright file="RouteVersionAttribute.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace ZoombracoDemo.Logic.ComponentModel.Attributes
{
    using System.Collections.Generic;
    using System.Web.Http.Routing;

    /// <summary>
    /// Versioning support for the WebAPI controllers
    /// Adapted from <see href="https://stackoverflow.com/questions/25299889/customize-maphttpattributeroutes-for-web-api-versioning"/>
    /// </summary>
    public sealed class RouteVersionAttribute : RouteFactoryAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RouteVersionAttribute"/> class.
        /// </summary>
        /// <param name="template">The route template</param>
        public RouteVersionAttribute(string template)
            : this(template, 1)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RouteVersionAttribute"/> class.
        /// </summary>
        /// <param name="template">The route template</param>
        /// <param name="version">The api version</param>
        public RouteVersionAttribute(string template, int version)
            : base(template)
        {
            this.Version = version;
        }

        /// <summary>
        /// Gets the version
        /// </summary>
        public int Version { get; }

        /// <inheritdoc/>
        public override IDictionary<string, object> Constraints => new HttpRouteValueDictionary { { "version", new RouteVersionHttpConstraint(this.Version) } };

        /// <inheritdoc/>
        public override IDictionary<string, object> Defaults => new HttpRouteValueDictionary { { "version", 1 } };
    }
}