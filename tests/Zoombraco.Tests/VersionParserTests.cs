namespace Zoombraco.Tests
{
    using System;
    using Semver;
    using Xunit;
    using Zoombraco.Helpers;

    public class VersionParserTests
    {
        [Fact]
        public void VersionParserThrowsWhenGivenNull()
        {
            Assert.Throws(typeof(ArgumentNullException), () =>
            {
                SemVersion error = VersionParser.FromSemanticString(null);
            });
        }

        [Fact]
        public void VersionParserResolvesOrderCorrectly()
        {
            SemVersion beta1 = VersionParser.FromSemanticString("0.5.0-beta.1");
            SemVersion beta2 = VersionParser.FromSemanticString("0.5.0-beta.2");
            SemVersion release = VersionParser.ZoombracoProductVersion();

            Assert.True(release > beta1);
            Assert.True(release > beta2);
            Assert.True(beta2 > beta1);
        }
    }
}
