// <copyright file="RouteVersionHttpConstraint.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace ZoombracoDemo.Logic.ComponentModel.Attributes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Web;
    using System.Web.Http.Routing;

    /// <summary>
    /// Add a route constraint to detect version header or by query string
    /// Adapted from <see href="https://stackoverflow.com/questions/25299889/customize-maphttpattributeroutes-for-web-api-versioning"/>
    /// </summary>
    public class RouteVersionHttpConstraint : IHttpRouteConstraint
    {
        /// <summary>
        /// The version request header key
        /// </summary>
        public const string VersionHeaderName = "api-version";
        private const int DefaultVersion = 1;

        /// <summary>
        /// Initializes a new instance of the <see cref="RouteVersionHttpConstraint"/> class.
        /// </summary>
        /// <param name="allowedVersion">The allowed version number</param>
        public RouteVersionHttpConstraint(int allowedVersion)
        {
            this.AllowedVersion = allowedVersion;
        }

        /// <summary>
        /// Gets the allowed version
        /// </summary>
        public int AllowedVersion
        {
            get;
        }

        /// <inheritdoc/>
        public bool Match(HttpRequestMessage request, IHttpRoute route, string parameterName, IDictionary<string, object> values, HttpRouteDirection routeDirection)
        {
            if (routeDirection == HttpRouteDirection.UriResolution)
            {
                int version = this.GetVersionHeaderOrQuery(request) ?? DefaultVersion;
                if (version == this.AllowedVersion)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Checks the request header, and the query string to determine if a version number has been provided
        /// </summary>
        /// <param name="request">The HTTP request message</param>
        /// <returns>The <see cref="Nullable{T}"/></returns>
        private int? GetVersionHeaderOrQuery(HttpRequestMessage request)
        {
            IEnumerable<string> headerValues;
            if (request.Headers.TryGetValues(VersionHeaderName, out headerValues) && headerValues.Count() == 1)
            {
                var versionAsString = headerValues.First();
                int version;
                if (versionAsString != null && int.TryParse(versionAsString, out version))
                {
                    return version;
                }
            }
            else
            {
                var query = HttpUtility.ParseQueryString(request.RequestUri.Query);
                string versionParam = query[VersionHeaderName];
                int version;
                int.TryParse(versionParam, out version);

                if (version > 0)
                {
                    return version;
                }
            }

            return null;
        }
    }
}