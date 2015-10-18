using System;
using FluentAssertions;
using FluentTc.Locators;
using NUnit.Framework;

namespace FluentTc.Tests.Locators
{
    [TestFixture]
    public class BuildHavingBuilderTests
    {
        [Test]
        public void Id()
        {
            // Arrange
            var buildHavingBuilder = new BuildHavingBuilder();

            // Act
            var havingBuilder = buildHavingBuilder.Id(123);

            // Assert
            havingBuilder.GetLocator().Should().Be("id:123");
        }

        [Test]
        public void Personal()
        {
            // Arrange
            var buildHavingBuilder = new BuildHavingBuilder();

            // Act
            var havingBuilder = buildHavingBuilder.Personal();

            // Assert
            havingBuilder.GetLocator().Should().Be("personal:True");
        }

        [Test]
        public void NotPersonal()
        {
            // Arrange
            var buildHavingBuilder = new BuildHavingBuilder();

            // Act
            var havingBuilder = buildHavingBuilder.NotPersonal();

            // Assert
            havingBuilder.GetLocator().Should().Be("personal:False");
        }

        [Test]
        public void AgentName()
        {
            // Arrange
            var buildHavingBuilder = new BuildHavingBuilder();

            // Act
            var havingBuilder = buildHavingBuilder.AgentName("bond");

            // Assert
            havingBuilder.GetLocator().Should().Be("agentName:bond");
        }

        [Test]
        public void Cancelled()
        {
            // Arrange
            var buildHavingBuilder = new BuildHavingBuilder();

            // Act
            var havingBuilder = buildHavingBuilder.Cancelled();

            // Assert
            havingBuilder.GetLocator().Should().Be("cancelled:True");
        }

        [Test]
        public void NotCancelled()
        {
            // Arrange
            var buildHavingBuilder = new BuildHavingBuilder();

            // Act
            var havingBuilder = buildHavingBuilder.NotCancelled();

            // Assert
            havingBuilder.GetLocator().Should().Be("cancelled:False");
        }

        [Test]
        public void Pinned()
        {
            // Arrange
            var buildHavingBuilder = new BuildHavingBuilder();

            // Act
            var havingBuilder = buildHavingBuilder.Pinned();

            // Assert
            havingBuilder.GetLocator().Should().Be("pinned:True");
        }

        [Test]
        public void NotPinned()
        {
            // Arrange
            var buildHavingBuilder = new BuildHavingBuilder();

            // Act
            var havingBuilder = buildHavingBuilder.NotPinned();

            // Assert
            havingBuilder.GetLocator().Should().Be("pinned:False");
        }

        [Test]
        public void Running()
        {
            // Arrange
            var buildHavingBuilder = new BuildHavingBuilder();

            // Act
            var havingBuilder = buildHavingBuilder.Running();

            // Assert
            havingBuilder.GetLocator().Should().Be("running:True");
        }

        [Test]
        public void NotRunning()
        {
            // Arrange
            var buildHavingBuilder = new BuildHavingBuilder();

            // Act
            var havingBuilder = buildHavingBuilder.NotRunning();

            // Assert
            havingBuilder.GetLocator().Should().Be("running:False");
        }

        [Test]
        public void SinceDate()
        {
            // Arrange
            var buildHavingBuilder = new BuildHavingBuilder();

            // Act
            var havingBuilder = buildHavingBuilder.SinceDate(new DateTime(2015,10,18,16,56,0));

            // Assert
            havingBuilder.GetLocator().Should().Be("sinceDate:20151018T165600+0300");
        }

        [Test]
        public void StatusSuccess()
        {
            // Arrange
            var buildHavingBuilder = new BuildHavingBuilder();

            // Act
            var havingBuilder = buildHavingBuilder.Status(BuildStatus.Success);

            // Assert
            havingBuilder.GetLocator().Should().Be("status:SUCCESS");
        }

        [Test]
        public void StatusFailure()
        {
            // Arrange
            var buildHavingBuilder = new BuildHavingBuilder();

            // Act
            var havingBuilder = buildHavingBuilder.Status(BuildStatus.Failure);

            // Assert
            havingBuilder.GetLocator().Should().Be("status:FAILURE");
        }

        [Test]
        public void StatusError()
        {
            // Arrange
            var buildHavingBuilder = new BuildHavingBuilder();

            // Act
            var havingBuilder = buildHavingBuilder.Status(BuildStatus.Error);

            // Assert
            havingBuilder.GetLocator().Should().Be("status:ERROR");
        }

        [Test]
        public void Tags_OneTag()
        {
            // Arrange
            var buildHavingBuilder = new BuildHavingBuilder();

            // Act
            var havingBuilder = buildHavingBuilder.Tags("tag1");

            // Assert
            havingBuilder.GetLocator().Should().Be("tags:tag1");
        }

        [Test]
        public void Tags_TwoTags()
        {
            // Arrange
            var buildHavingBuilder = new BuildHavingBuilder();

            // Act
            var havingBuilder = buildHavingBuilder.Tags("tag1", "tag2");

            // Assert
            havingBuilder.GetLocator().Should().Be("tags:tag1,tag2");
        }
    }
}