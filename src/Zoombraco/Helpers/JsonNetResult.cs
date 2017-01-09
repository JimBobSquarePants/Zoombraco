// <copyright file="JsonNetResult.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace Zoombraco.Helpers
{
    using System;
    using System.IO;
    using System.Web;
    using System.Web.Mvc;

    using Newtonsoft.Json;

    /// <summary>
    /// Represents a class that is used to send JSON-formatted content to the response.
    /// <para>
    /// This version uses JSON.NET to do the serialization.
    /// Both MaxJsonLength and RecursionLimit properties are not used in this implementation since there are
    /// no equivalent properties in JSON.NET.
    /// </para>
    /// </summary>
    public class JsonNetResult : JsonResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JsonNetResult"/> class.
        /// </summary>
        public JsonNetResult()
        {
            this.Settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
        }

        /// <summary>
        /// Gets the settings.
        /// </summary>
        public JsonSerializerSettings Settings { get; }

        /// <summary>
        /// Executes the result.
        /// </summary>
        /// <param name="context">The context.</param>
        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if ((this.JsonRequestBehavior == JsonRequestBehavior.DenyGet)
                && string.Equals(context.HttpContext.Request.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException("This request has been blocked because sensitive information could "
                                                    + "be disclosed to third party web sites when this is used in a GET request. "
                                                    + "To allow GET requests, set JsonRequestBehavior to AllowGet.");
            }

            HttpResponseBase response = context.HttpContext.Response;

            response.ContentType = !string.IsNullOrEmpty(this.ContentType)
                ? this.ContentType
                : "application/json";

            if (this.ContentEncoding != null)
            {
                response.ContentEncoding = this.ContentEncoding;
            }

            if (this.Data != null)
            {
                JsonSerializer scriptSerializer = JsonSerializer.Create(this.Settings);

                using (StringWriter sw = new StringWriter())
                {
                    scriptSerializer.Serialize(sw, this.Data);
                    response.Write(sw.ToString());
                }
            }
        }
    }
}
