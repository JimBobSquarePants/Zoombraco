namespace Zoombraco.Tests
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Xunit;
    using Zoombraco.Extensions;

    public class EnumExtensionTests
    {
        public static readonly TheoryData<TestEnum, string> EnumData = new TheoryData<TestEnum, string>
        {
            {TestEnum.Value1,"I am the first value." },
            {TestEnum.Value2,"I am the second value." },
            {TestEnum.Value3,"Value3" },
        };

        [Theory]
        [MemberData(nameof(EnumData))]
        public void EnumReturnsCorrectDisplayValue(TestEnum value, string expected)
        {
            Assert.Equal(value.ToDisplay(), expected, StringComparer.OrdinalIgnoreCase);
        }

    }

    public enum TestEnum
    {
        [Display(Name = "I am the first value.")]
        Value1,

        [Display(Name = "I am the second value.")]
        Value2,

        Value3
    }
}