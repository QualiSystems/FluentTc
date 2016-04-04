using FluentAssertions;
using FluentTc.Locators;
using NUnit.Framework;

namespace FluentTc.Tests.Locators
{
    [TestFixture]
    public class BuildQueueIdHavingBuilderTests
    {
        [Test]
        public void GetLocator_Id()
        {
            // Arrange
            var buildQueueIdHavingBuilder = new BuildQueueIdHavingBuilder();

            // Act
            buildQueueIdHavingBuilder.Id(123);

            // Assert
            buildQueueIdHavingBuilder.GetLocator().Should().Be("id:123");
        }
    }
}