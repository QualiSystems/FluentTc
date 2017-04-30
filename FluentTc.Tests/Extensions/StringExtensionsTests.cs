using NUnit.Framework;
using FluentTc.Extensions;
using FluentAssertions;

namespace FluentTc.Tests.Extensions
{
    [TestFixture]
    public class StringExtensionsTests
    {
        [Test]
        [TestCase("ThisIsAPascalCaseExample", "THIS_IS_A_PASCAL_CASE_EXAMPLE")]
        [TestCase("", "")]
        [TestCase("A", "A")]
        [TestCase("Test", "TEST")]
        public void FromPascalToCapitalizedCase(string input, string output)
        {
            input.FromPascalToCapitalizedCase().Should().Be(output);
        }

        [Test]
        [TestCase("ThisIsAPascalCaseExample", "thisIsAPascalCaseExample")]
        [TestCase("", "")]
        [TestCase("A", "a")]
        [TestCase("Test", "test")]
        public void FromPascalToCamelCase(string input, string output)
        {
            input.FromPascalToCamelCase().Should().Be(output);
        }
    }
}
