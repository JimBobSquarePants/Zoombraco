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
            SemVersion beta7 = VersionParser.FromSemanticString("0.5.0-beta+7");
            SemVersion beta9 = VersionParser.FromSemanticString("0.5.0-beta+9");
            SemVersion release = VersionParser.ZoombracoProductVersion();

            Assert.True(release > beta7);
            Assert.True(release > beta9);
            Assert.True(beta9 > beta7);
        }
    }
}
