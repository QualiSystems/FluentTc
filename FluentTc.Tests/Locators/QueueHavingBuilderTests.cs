using System;
using FakeItEasy;
using FluentAssertions;
using FluentTc.Locators;
using NUnit.Framework;
using Ploeh.AutoFixture;

namespace FluentTc.Tests.Locators
{
    [TestFixture]
    public class QueueHavingBuilderTests
    {
        [Test]
        public void QueueHavingBuilder_BuildType_LocatorString()
        {
            // Arrange
            var fixture = Auto.Fixture();
            var locatorBuilder = fixture.Freeze<ILocatorBuilder>();
            Action<IBuildConfigurationHavingBuilder> havingBuildConfig = _ => _.Id("bt2");
            A.CallTo(() => locatorBuilder.GetBuildConfigurationLocator(havingBuildConfig)).Returns("id:bt2");
            QueueHavingBuilder queueHavingBuilder = fixture.Create<QueueHavingBuilder>();

            // Act
            queueHavingBuilder.BuildConfiguration(havingBuildConfig);
            
            // Assert
            queueHavingBuilder.GetLocator().Should().Be("buildType:id:bt2");
        }

        [Test]
        public void QueueHavingBuilder_Project_LocatorString()
        {
            // Arrange
            var fixture = Auto.Fixture();
            var locatorBuilder = fixture.Freeze<ILocatorBuilder>();
            Action<IBuildProjectHavingBuilder> havingBuildConfig = _ => _.Id("bt2");
            A.CallTo(() => locatorBuilder.GetProjectLocator(havingBuildConfig)).Returns("id:bt2");
            QueueHavingBuilder queueHavingBuilder = fixture.Create<QueueHavingBuilder>();

            // Act
            queueHavingBuilder.Project(havingBuildConfig);
            
            // Assert
            queueHavingBuilder.GetLocator().Should().Be("project:id:bt2");
        }
    }
}