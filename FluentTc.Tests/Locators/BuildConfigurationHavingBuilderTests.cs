using FluentAssertions;
using FluentTc.Locators;
using NUnit.Framework;
using Ploeh.AutoFixture;

namespace FluentTc.Tests.Locators
{
    [TestFixture]
    public class BuildConfigurationHavingBuilderTests
    {
        [Test]
        public void Id()
        {
            // Arrange
            var fixture = Auto.Fixture();
            var buildConfigurationHavingBuilder = fixture.Create<BuildConfigurationHavingBuilder>();

            // Act
            buildConfigurationHavingBuilder.Id("bt2");

            // Assert
            buildConfigurationHavingBuilder.GetLocator().Should().Be("id:bt2");
        }

        [Test]
        public void Name()
        {
            // Arrange
            var fixture = Auto.Fixture();
            var buildConfigurationHavingBuilder = fixture.Create<BuildConfigurationHavingBuilder>();

            // Act
            buildConfigurationHavingBuilder.Name("FluentTc");

            // Assert
            buildConfigurationHavingBuilder.GetLocator().Should().Be("name:FluentTc");
        }
    }
}