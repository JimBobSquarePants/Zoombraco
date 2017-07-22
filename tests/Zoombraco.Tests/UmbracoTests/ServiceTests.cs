using System;
using Xunit;
using ZoombracoDemo.Logic.Models;
using ZoombracoDemo.Logic.Services;

namespace Zoombraco.Tests.UmbracoTests
{
    public class ServiceTests : IDisposable
    {
        private readonly UmbracoSupport support = new UmbracoSupport();

        public ServiceTests()
        {
            this.support.SetupUmbraco();
        }

        [Fact]
        public void GenericByIdNotNull()
        {
            IGenericService genericService = new PublishedGenericService();

            Generic generic = genericService.GetById(1071);

            // TODO: How do I get the tests to map the NestedContent property across like real usage does?
            // Also, how do we test mapping retrieval from Examine
            Assert.NotNull(generic);
        }

        public void Dispose()
        {
            this.support.DisposeUmbraco();
        }
    }
}
