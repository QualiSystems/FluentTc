using FluentAssertions;
using FluentTc.Locators;
using NUnit.Framework;

namespace FluentTc.Tests.Locators
{
    [TestFixture]
    public class BuildAdditionalIncludeBuilderTests
    {
        [Test]
        public void ShouldIncludeChanges_False()
        {
            // Arrange
            var buildAdditionalIncludeBuilder = new BuildAdditionalIncludeBuilder();

            // Act
            var shouldIncludeChanges = buildAdditionalIncludeBuilder.ShouldIncludeChanges;

            // Assert
            shouldIncludeChanges.Should().BeFalse();
        }

        [Test]
        public void ShouldIncludeChanges_True()
        {
            // Arrange
            var buildAdditionalIncludeBuilder = new BuildAdditionalIncludeBuilder();

            // Act
            buildAdditionalIncludeBuilder.IncludeChanges(_ => _.IncludeComment());
            var shouldIncludeChanges = buildAdditionalIncludeBuilder.ShouldIncludeChanges;

            // Assert
            shouldIncludeChanges.Should().BeTrue();
            var changesIncludeBuilder = new ChangesIncludeBuilder();
            buildAdditionalIncludeBuilder.ChangesInclude(changesIncludeBuilder);
            changesIncludeBuilder.GetColumns().Should().Be("id,version,href,username,date,webUrl,comment");
        }
    }
}

