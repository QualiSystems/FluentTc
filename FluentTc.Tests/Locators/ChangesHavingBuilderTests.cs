using FakeItEasy;
using FluentAssertions;
using FluentTc.Engine;
using FluentTc.Locators;
using NUnit.Framework;

namespace FluentTc.Tests.Locators
{
    [TestFixture]
    public class ChangesHavingBuilderTests
    {
        [Test]
        public void Build()
        {
            // Arrange
            var buildHavingBuilder = A.Fake<IBuildHavingBuilder>();
            A.CallTo(() => buildHavingBuilder.GetLocator()).Returns("id:123");

            var buildHavingBuilderFactory = A.Fake<IBuildHavingBuilderFactory>();
            A.CallTo(() => buildHavingBuilderFactory.CreateBuildHavingBuilder()).Returns(buildHavingBuilder);

            var changesHavingBuilder = new ChangesHavingBuilder(buildHavingBuilderFactory);

            // Act
            changesHavingBuilder.Build(_ => _.Id(123));
            var locator = changesHavingBuilder.GetLocator();
            
            // Assertions
            locator.Should().Be("build:id:123");
        }
    }
}