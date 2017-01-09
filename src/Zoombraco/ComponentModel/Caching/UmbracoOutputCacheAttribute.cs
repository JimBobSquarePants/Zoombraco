// <copyright file="UmbracoOutputCacheAttribute.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace Zoombraco.ComponentModel.Caching
{
    using System.Collections.Specialized;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.UI;

    using DevTrends.MvcDonutCaching;

    using Umbraco.Web;

    /// <summary>
    /// The umbraco output cache attribute is used to declare that an action result can be cached in a manner that
    /// respects preview mode.
    /// </summary>
    /// <remarks>
    /// The cache duration is set by a <see cref="ZoombracoConstants.Configuration.OutputCacheDuration"/> property in the web configuration.
    /// </remarks>
    public class UmbracoOutputCacheAttribute : DonutOutputCacheAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UmbracoOutputCacheAttribute"/> class.
        /// </summary>
        public UmbracoOutputCacheAttribute()
        {
            if (this.Duration <= 0)
            {
                this.Duration = ZoombracoConfiguration.Instance.OutputCacheDuration;
            }

            // We handle both url and params in one. See Global.cs
            this.VaryByCustom = "url";
            this.Location = OutputCacheLocation.Server;
        }

        /// <inheritdoc/>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            NameValueCollection filtered = HttpUtility.ParseQueryString(filterContext.HttpContext.Request.QueryString.ToString());
            bool previewMode = UmbracoContext.Current != null && UmbracoContext.Current.InPreviewMode;
            bool debugMode = filterContext.HttpContext.IsDebuggingEnabled;
            bool umbDebugMode = filtered["umbDebug"] != null;

            // If not in preview mode or Umbraco debug mode then return cached output.
            if (!previewMode && !umbDebugMode)
            {
                base.OnActionExecuting(filterContext);
            }
            else if (!debugMode && umbDebugMode)
            {
                // If compilation debug mode is false but Umbraco debug mode is requested return the cached output.
                base.OnActionExecuting(filterContext);
            }
        }
    }
}