// <copyright file="XmlViewPage{T}.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace Zoombraco.Views
{
    using System.Web.Mvc;
    using System.Web.WebPages;

    /// <summary>
    /// Represents the properties and methods that are needed in order to render an XML view.
    /// <remarks>
    /// Using this type allows us to always avoid the error where a line of space is inserted before
    /// the opening XML declaration.
    /// </remarks>
    /// </summary>
    /// <typeparam name="T">The type of object to create the view for.</typeparam>
    public class XmlViewPage<T> : WebViewPage<T>
    {
        /// <summary>
        /// Runs the page hierarchy for the ASP.NET Razor execution pipeline.
        /// </summary>
        public override void ExecutePageHierarchy()
        {
            this.Response.ContentType = "application/xml; charset=UTF-8";
            this.Layout = null;
            this.PushContext(new WebPageContext(), this.Response.Output);
            base.ExecutePageHierarchy();
        }

        /// <summary>
        /// Executes the server code in the current web page that is marked using Razor syntax.
        /// </summary>
        public override void Execute()
        {
        }
    }
}