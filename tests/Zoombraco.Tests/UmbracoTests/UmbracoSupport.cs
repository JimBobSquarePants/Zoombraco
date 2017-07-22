using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;
using Moq;
using Umbraco.Core;
using Umbraco.Core.Configuration.UmbracoSettings;
using Umbraco.Core.Models;
using Umbraco.Core.Persistence;
using Umbraco.Core.Services;
using Umbraco.Tests.TestHelpers;
using Umbraco.Web;
using Umbraco.Web.Mvc;
using Umbraco.Web.Routing;
using System.Web;
using Zoombraco.Resources;

namespace Zoombraco.Tests.UmbracoTests
{
    /// <summary>
    /// Provides common stupped Umbraco properties required for testing.
    /// <see href="http://blog.aabech.no/archive/the-basics-of-unit-testing-umbraco-just-got-simpler/"/>
    /// </summary>
    public class UmbracoSupport : BaseRoutingTest
    {
        private UmbracoContext umbracoContext;
        private ServiceContext serviceContext;
        private IUmbracoSettingsSection settings;
        private RoutingContext routingContext;
        private IPublishedContent currentPage;
        private RouteData routeData;
        private UmbracoHelper umbracoHelper;
        private PublishedContentRequest publishedContentRequest;

        public UmbracoContext UmbracoContext => this.umbracoContext;

        public new ServiceContext ServiceContext => this.serviceContext;

        public IPublishedContent CurrentPage => this.currentPage;

        public RouteData RouteData => this.routeData;

        public UmbracoHelper UmbracoHelper => this.umbracoHelper;

        public HttpContextBase HttpContext => this.umbracoContext.HttpContext;

        public string ContentCacheXml { get; set; }

        /// <summary>
        /// Initializes a stubbed Umbraco request context. Generally called from [SetUp] methods.
        /// Remember to call UmbracoSupport.DisposeUmbraco from your [TearDown].
        /// </summary>
        public void SetupUmbraco()
        {
            this.InitializeFixture();
            this.TryBaseInitialize();

            this.InitializeSettings();

            this.CreateCurrentPage();
            this.CreateRouteData();
            this.CreateContexts();
            this.CreateHelper();

            this.InitializePublishedContentRequest();
        }

        /// <summary>
        /// Cleans up the stubbed Umbraco request context. Generally called from [TearDown] methods.
        /// Must be called before another UmbracoSupport.SetupUmbraco.
        /// </summary>
        public void DisposeUmbraco()
        {
            this.TearDown();
        }

        /// <summary>
        /// Attaches the stubbed UmbracoContext et. al. to the Controller.
        /// </summary>
        /// <param name="controller"></param>
        public void PrepareController(Controller controller)
        {
            var controllerContext = new ControllerContext(this.HttpContext, this.RouteData, controller);
            controller.ControllerContext = controllerContext;

            this.routeData.Values.Add("controller", controller.GetType().Name.Replace("Controller", string.Empty));
            this.routeData.Values.Add("action", "Dummy");
        }

        protected override string GetXmlContent(int templateId)
        {
            if (this.ContentCacheXml != null)
            {
                return this.ContentCacheXml;
            }

            try
            {
                return this.GetEmbeddedXml();
            }
            catch
            {
                return base.GetXmlContent(templateId);
            }
        }

        protected override ApplicationContext CreateApplicationContext()
        {
            // Overrides the base CreateApplicationContext to inject a completely stubbed servicecontext
            this.serviceContext = MockHelper.GetMockedServiceContext();
            var appContext = new ApplicationContext(
                new DatabaseContext(Mock.Of<IDatabaseFactory2>(), this.Logger, this.SqlSyntax, this.GetDbProviderName()), this.serviceContext, this.CacheHelper, this.ProfilingLogger);
            return appContext;
        }

        private void TryBaseInitialize()
        {
            // Delegates to Umbraco.Tests initialization. Gives a nice hint about disposing the support class for each test.
            try
            {
                this.Initialize();
            }
            catch (InvalidOperationException ex)
            {
                if (ex.Message.StartsWith("Resolution is frozen"))
                {
                    throw new Exception(
                        "Resolution is frozen. This is probably because UmbracoSupport.DisposeUmbraco wasn't called before another UmbracoSupport.SetupUmbraco call.");
                }
            }
        }

        private void InitializeSettings()
        {
            // Stub up all the settings in Umbraco so we don't need a big app.config file.
            this.settings = SettingsForTests.GenerateMockSettings();
            SettingsForTests.ConfigureSettings(this.settings);
        }

        private void CreateCurrentPage()
        {
            // Stubs up the content used as current page in all contexts
            this.currentPage = Mock.Of<IPublishedContent>();
        }

        private void CreateRouteData()
        {
            // Route data is used in many of the contexts, and might need more data throughout your tests.
            this.routeData = new RouteData();
        }

        private void CreateContexts()
        {
            // Surface- and RenderMvcControllers need a routing context to fint the current content.
            // Umbraco.Tests creates one and whips up the UmbracoContext in the process.
            this.routingContext = this.GetRoutingContext("http://localhost", -1, this.routeData, true, this.settings);
            this.umbracoContext = this.routingContext.UmbracoContext;
        }

        private void CreateHelper()
        {
            this.umbracoHelper = new UmbracoHelper(this.umbracoContext, this.currentPage);
        }

        private void InitializePublishedContentRequest()
        {
            // Some deep core methods fetch the published content request from routedata
            // others access it through the context
            // in any case, this is the one telling everyone which content is the current content.
            this.publishedContentRequest = new PublishedContentRequest(new Uri("http://localhost"), this.routingContext, this.settings.WebRouting, s => new string[0])
            {
                PublishedContent = this.currentPage,
                Culture = CultureInfo.CurrentCulture
            };

            this.umbracoContext.PublishedContentRequest = this.publishedContentRequest;

            var routeDefinition = new RouteDefinition
            {
                PublishedContentRequest = this.publishedContentRequest
            };

            this.routeData.DataTokens.Add("umbraco-route-def", routeDefinition);
        }

        private string GetEmbeddedXml()
        {
            var assembly = typeof(UmbracoSupport).Assembly;
            string resourceName;
            using (var stream = EmbeddedResourceHelper.GetResource(assembly, "umbraco.config", out resourceName))
            using (var reader = new StreamReader(stream, Encoding.UTF8))
            {
                return reader.ReadToEnd();
            }
        }
    }
}