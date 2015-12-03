using FluentAssertions;
using FluentTc.Locators;
using NUnit.Framework;

namespace FluentTc.Tests.Locators
{
    [TestFixture]
    public class BuildProjectHavingBuilderTests
    {
        [Test]
        public void Id()
        {
            // Arrange
            var buildProjectHavingBuilder = new BuildProjectHavingBuilder();

            // Act
            buildProjectHavingBuilder.Id("ProjectId1");

            // Assert
            buildProjectHavingBuilder.GetLocator().Should().Be("id:ProjectId1");
        }

        [Test]
        public void Name()
        {
            // Arrange
            var buildProjectHavingBuilder = new BuildProjectHavingBuilder();

            // Act
            buildProjectHavingBuilder.Name("ProjectName1");

            // Assert
            buildProjectHavingBuilder.GetLocator().Should().Be("name:ProjectName1");
        }
    }
}