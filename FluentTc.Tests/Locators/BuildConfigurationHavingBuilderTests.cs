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
            var configurationHavingBuilder = buildConfigurationHavingBuilder.Id("bt2");
            var having = configurationHavingBuilder.GetLocator();

            having.Should().Be("id:bt2");
        }

        [Test]
        public void Name()
        {
            // Arrange
            var fixture = Auto.Fixture();
            var buildConfigurationHavingBuilder = fixture.Create<BuildConfigurationHavingBuilder>();

            // Act
            var configurationHavingBuilder = buildConfigurationHavingBuilder.Name("FluentTc");
            var having = configurationHavingBuilder.GetLocator();

            having.Should().Be("name:FluentTc");
        }
    }
}