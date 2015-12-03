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
            countBuilder.DefaultCount();

            // Assert
            countBuilder.GetCount().Should().BeEmpty();
        }

        [Test]
        public void CountFive()
        {
            // Arrange
            var countBuilder = new CountBuilder();
            
            // Act
            countBuilder.Count(5);

            // Assert
            countBuilder.GetCount().Should().Be("count:5");
        }

        [Test]
        public void StartTwoCountFive()
        {
            // Arrange
            var countBuilder = new CountBuilder();
            
            // Act
            countBuilder.Start(2).Count(5);

            // Assert
            countBuilder.GetCount().Should().Be("start:2,count:5");
        }
    }
}