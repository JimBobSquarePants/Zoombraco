using System;
using Xunit;
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
            Assert.NotNull(genericService.GetById(1071));
        }

        public void Dispose()
        {
            this.support.DisposeUmbraco();
        }
    }
}
