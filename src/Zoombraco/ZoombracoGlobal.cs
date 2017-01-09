// <copyright file="ZoombracoGlobal.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace Zoombraco
{
    using System.Collections.Specialized;
    using System.Web;

    using Umbraco.Core;

    /// <summary>
    /// Contains code for responding to application-level events raised by ASP.NET or by HttpModules.
    /// </summary>
    public class ZoombracoGlobal : Umbraco.Web.UmbracoApplication
    {
        /// <inheritdoc/>
        public override string GetVaryByCustomString(HttpContext context, string custom)
        {
            if (custom.InvariantEquals("url"))
            {
                // Vary by url.
                NameValueCollection filtered = HttpUtility.ParseQueryString(context.Request.QueryString.ToString());
                string absoluteUri = context.Request.Url.AbsoluteUri;

                // Exclude urls that have umbDebug in the querystring.
                if (filtered["umbDebug"] != null)
                {
                    filtered.Remove("umbDebug");
                    absoluteUri = context.Request.Url.AbsoluteUri.Split('?')[0];

                    if (filtered.Count > 0)
                    {
                        absoluteUri += $"?{filtered}";
                    }
                }

                return absoluteUri;
            }

            return base.GetVaryByCustomString(context, custom);
        }
    }
}