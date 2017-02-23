// <copyright file="ZoombracoController.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace Zoombraco.Controllers
{
    using System;
    using System.IO;
    using System.Text;
    using System.Web;
    using System.Web.Configuration;
    using System.Web.Mvc;

    using Helpers;
    using Models;

    using Our.Umbraco.Ditto;

    using Umbraco.Core.Logging;
    using Umbraco.Web;
    using Umbraco.Web.Models;
    using Umbraco.Web.Mvc;

    using Zoombraco.ComponentModel.Caching;

    /// <summary>
    /// A combined surface and render controller. This allows us to return child actions without the need to specify route values.
    /// </summary>
    [PreRenderViewActionFilter]
    public abstract class ZoombracoController : SurfaceController, IRenderMvcController
    {
        /// <summary>
        /// The content helper.
        /// </summary>
        private ContentHelper contentHelper;

        /// <summary>
        /// Initializes a new instance of the <see cref="ZoombracoController"/> class.
        /// Empty constructor, uses Singleton to resolve the UmbracoContext
        /// </summary>
        protected ZoombracoController()
            : base(UmbracoContext.Current)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ZoombracoController"/> class.
        /// </summary>
        /// <param name="umbracoContext">The Umbraco context</param>
        protected ZoombracoController(UmbracoContext umbracoContext)
          : base(umbracoContext)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ZoombracoController"/> class.
        /// </summary>
        /// <param name="umbracoContext">The Umbraco context.</param>
        /// <param name="umbracoHelper">The Umbraco helper.</param>
        protected ZoombracoController(UmbracoContext umbracoContext, UmbracoHelper umbracoHelper)
            : base(umbracoContext, umbracoHelper)
        {
        }

        /// <summary>
        /// Gets the content helper.
        /// </summary>
        public virtual ContentHelper ContentHelper => this.contentHelper ?? (this.contentHelper = new ContentHelper(this.Umbraco));

        /// <summary>
        /// Gets the view name for displaying handled errors. Defaults to "Error"
        /// </summary>
        public virtual string ErrorViewName { get; } = "Error";

        /// <summary>
        /// Returns the default result of an action method for the controller used to perform a framework-level
        /// operation on behalf of the action method.
        /// <remarks>
        /// The resultant view will always match the name of the document type.
        /// </remarks>
        /// </summary>
        /// <param name="model">The model to provide the result for.</param>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        [UmbracoOutputCache]
        public virtual ActionResult Index(RenderModel model)
        {
            Page page = model.As<Page>();
            IRenderPage<Page> render = new RenderPage<Page>(page);
            return this.CurrentTemplate(render);
        }

        /// <summary>
        /// Returns the compiled contents of a partial view as a string.
        /// </summary>
        /// <param name="partialViewName">The partial view name.</param>
        /// <param name="model">The model to pass to the partial.</param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public string RenderPartialToString(string partialViewName, object model)
        {
            this.CreateControllerContext();

            IView view = this.EnsurePhsyicalViewExists(partialViewName, true);

            return this.RenderViewToString(view, model);
        }

        /// <summary>
        /// Returns the compiled contents of a view as a string.
        /// </summary>
        /// <param name="viewName">The view name.</param>
        /// <param name="model">The model to pass to the view.</param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public string RenderViewToString(string viewName, object model)
        {
            this.CreateControllerContext();

            IView view = this.EnsurePhsyicalViewExists(viewName);

            return this.RenderViewToString(view, model);
        }

        /// <summary>
        /// Renders an <see cref="IView"/> as a string.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="model">The model.</param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public string RenderViewToString(IView view, object model)
        {
            this.CreateControllerContext();
            string result = null;
            if (view != null)
            {
                StringBuilder sb = new StringBuilder();

                using (StringWriter writer = new StringWriter(sb))
                {
                    ViewContext viewContext = new ViewContext(this.ControllerContext, view, new ViewDataDictionary(model), new TempDataDictionary(), writer);

                    view.Render(viewContext, writer);
                    writer.Flush();
                }

                result = sb.ToString();
            }

            return result;
        }

        /// <summary>
        /// Checks to see if there is a physical file saved matching the given template returning the resultant view.
        /// </summary>
        /// <param name="template">The template to match.</param>
        /// <param name="isPartial">Whether the view is a partial one.</param>
        /// <returns>
        /// The <see cref="IView"/>.
        /// </returns>
        protected IView EnsurePhsyicalViewExists(string template, bool isPartial = false)
        {
            ViewEngineResult result = !isPartial
                ? ViewEngines.Engines.FindView(this.ControllerContext, template, null)
                : ViewEngines.Engines.FindPartialView(this.ControllerContext, template);

            if (result?.View != null)
            {
                return result.View;
            }

            LogHelper.Warn<ZoombracoController>($"No physical template file was found for template {template}");
            return null;
        }

        /// <summary>
        /// Returns a <see cref="ViewResult"/> based on the template name found in the route values and the given model.
        /// </summary>
        /// <param name="model">The model to provide the result for.</param>
        /// <typeparam name="T">The type of object to return the result for.</typeparam>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown if the view cannot be found.
        /// </exception>
        protected ActionResult CurrentTemplate<T>(T model)
        {
            string template = this.ControllerContext.RouteData.Values["action"].ToString();

            if (this.EnsurePhsyicalViewExists(template) == null)
            {
                return this.Content(string.Empty);
            }

            return this.View(template, model);
        }

        /// <summary>
        /// Returns a partial <see cref="ViewResult"/> based on the template name found in the route values and the given model.
        /// </summary>
        /// <param name="model">The model to provide the result for.</param>
        /// <typeparam name="T">The type of object to return the result for.</typeparam>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown if the view cannot be found.
        /// </exception>
        protected ActionResult CurrentPartialTemplate<T>(T model)
        {
            string template = this.ControllerContext.RouteData.Values["action"].ToString();

            if (this.EnsurePhsyicalViewExists(template, true) == null)
            {
                return this.Content(string.Empty);
            }

            return this.PartialView(template, model);
        }

        /// <inheritdoc/>
        protected override void OnException(ExceptionContext filterContext)
        {
            CompilationSection compilationSection = (CompilationSection)WebConfigurationManager.GetSection(@"system.web/compilation");

            if (compilationSection.Debug)
            {
                return;
            }

            if (filterContext.ExceptionHandled)
            {
                return;
            }

            // Log the exception.
            LogHelper.Error<ZoombracoController>("Handled Exception: ", filterContext.Exception);

            // Set the correct view.
            HttpException httpException = filterContext.Exception as HttpException;
            if (httpException != null)
            {
                int statusCode = httpException.GetHttpCode();
                filterContext.HttpContext.Response.StatusDescription = httpException.Message;
                filterContext.HttpContext.Response.StatusCode = statusCode;
            }

            filterContext.Result = this.View(this.ErrorViewName);
            filterContext.ExceptionHandled = true;
        }

        /// <inheritdoc/>
        protected override ITempDataProvider CreateTempDataProvider()
        {
            // Use our cookie based provider to better suit a load balanced environment.
            return new CookieTempDataProvider();
        }

        /// <summary>
        /// Creates a JsonResult object that serializes the specified object to JavaScript Object Notation (JSON) format
        /// using JSON.NET with the given the content type, content encoding, and the JSON request behavior.
        /// </summary>
        /// <param name="data">The JavaScript object graph to serialize.</param>
        /// <param name="contentType">The content type (MIME type).</param>
        /// <param name="contentEncoding">The content encoding.</param>
        /// <param name="behavior">The JSON request behavior.</param>
        /// <returns>
        /// The result object that serializes the specified object to JSON format.
        /// </returns>
        protected override JsonResult Json(object data, string contentType, Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return new Helpers.JsonNetResult
            {
                Data = data,
                ContentType = contentType,
                RecursionLimit = int.MaxValue,
                ContentEncoding = contentEncoding,
                JsonRequestBehavior = behavior
            };
        }

        /// <summary>
        /// Creates a new controller context if none exists.
        /// </summary>
        private void CreateControllerContext()
        {
            if (this.ControllerContext == null)
            {
                ControllerContext context = new ControllerContext(System.Web.HttpContext.Current.Request.RequestContext, this);
                this.ControllerContext = context;
            }
        }
    }
}