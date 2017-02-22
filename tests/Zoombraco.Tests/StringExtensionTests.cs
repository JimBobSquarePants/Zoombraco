namespace Zoombraco.Tests
{
    using Xunit;
    using Zoombraco.Extensions;

    public class StringExtensionTests
    {
        public static readonly TheoryData<string, bool> UrlData = new TheoryData<string, bool>
        {
            {"http://umbraco.com", true },
            {"https://umbraco.com", true },
            {"https://umbraco.com/test.html?w=10", true },
            {"ftp://umbraco.com/test.html?w=10", true },
            {"umbraco.com", false },
            {"/test.html", false },
            {"/test.html?w=10", false },
        };

        [Theory]
        [MemberData(nameof(UrlData))]
        public void StringCanCorrectlyDetermineAbsoluteUrl(string url, bool expected)
        {
            Assert.Equal(url.IsAbsoluteUrl(), expected);
        }
    }
}