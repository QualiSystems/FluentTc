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
            var buildProjectHavingBuilder = new BuildProjectHavingBuilder();
            var projectHavingBuilder = buildProjectHavingBuilder.Id("ProjectId1");

            // Act
            var locator = projectHavingBuilder.GetLocator();

            // Assert
            locator.Should().Be("id:ProjectId1");
        }

        [Test]
        public void Name()
        {
            var buildProjectHavingBuilder = new BuildProjectHavingBuilder();
            var projectHavingBuilder = buildProjectHavingBuilder.Name("ProjectName1");

            // Act
            var locator = projectHavingBuilder.GetLocator();

            // Assert
            locator.Should().Be("name:ProjectName1");
        }
    }
}