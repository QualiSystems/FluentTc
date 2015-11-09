using FluentAssertions;
using FluentTc.Locators;
using NUnit.Framework;

namespace FluentTc.Tests.Locators
{
    [TestFixture]
    public class CountBuilderTests
    {
        [Test]
        public void All()
        {
            // Arrange
            var countBuilder = new CountBuilder();
            
            // Act
            var builder = countBuilder.DefaultCount();

            builder.GetCount().Should().BeEmpty();
        }

        [Test]
        public void CountFive()
        {
            // Arrange
            var countBuilder = new CountBuilder();
            
            // Act
            var builder = countBuilder.Count(5);

            builder.GetCount().Should().Be("count:5");
        }

        [Test]
        public void StartTwoCountFive()
        {
            // Arrange
            var countBuilder = new CountBuilder();
            
            // Act
            var builder = countBuilder.Start(2).Count(5);

            builder.GetCount().Should().Be("start:2,count:5");
        }
    }
}