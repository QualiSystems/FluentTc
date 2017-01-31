using System;
using FakeItEasy;
using FluentAssertions;
using FluentTc.Locators;
using NUnit.Framework;
using Ploeh.AutoFixture;

namespace FluentTc.Tests.Locators
{
    [TestFixture]
    public class BuildHavingBuilderTests
    {
        [Test]
        public void AgentName()
        {
            // Arrange
            var fixture = Auto.Fixture();
            var buildHavingBuilder = fixture.Create<BuildHavingBuilder>();

            // Act
            buildHavingBuilder.AgentName("bond");

            // Assert
            buildHavingBuilder.GetLocator().Should().Be("agentName:bond");
        }

        [Test]
        public void Branch()
        {
            // Arrange
            var fixture = Auto.Fixture();
            var branchHavingBuilder = A.Fake<BranchHavingBuilder>();
            A.CallTo(() => branchHavingBuilder.GetLocator()).Returns("name:Branch1");

            var userHavingBuilderFactory = fixture.Freeze<IBranchHavingBuilderFactory>();
            A.CallTo(() => userHavingBuilderFactory.CreateBranchHavingBuilder()).Returns(branchHavingBuilder);

            var buildHavingBuilder = fixture.Create<BuildHavingBuilder>();

            // Act
            buildHavingBuilder.Branch(_ => _.Name("Branch1"));

            // Assert
            buildHavingBuilder.GetLocator().Should().Be("branch:name:Branch1");
        }

        [Test]
        public void BuildConfiguration()
        {
            // Arrange
            Action<IBuildConfigurationHavingBuilder> havingBuildConfig = _ => _.Id("bt2");

            var fixture = Auto.Fixture();
            var locatorBuilder = fixture.Freeze<ILocatorBuilder>();
            A.CallTo(() => locatorBuilder.GetBuildConfigurationLocator(havingBuildConfig)).Returns("id:bt2");

            var buildHavingBuilder = fixture.Create<BuildHavingBuilder>();

            // Act
            buildHavingBuilder.BuildConfiguration(havingBuildConfig);

            // Assert
            buildHavingBuilder.GetLocator().Should().Be("buildType:id:bt2");
        }

        [Test]
        public void Cancelled()
        {
            // Arrange
            var fixture = Auto.Fixture();
            var buildHavingBuilder = fixture.Create<BuildHavingBuilder>();

            // Act
            buildHavingBuilder.Cancelled();

            // Assert
            buildHavingBuilder.GetLocator().Should().Be("canceled:True");
        }

        [Test]
        public void Id()
        {
            // Arrange
            var fixture = Auto.Fixture();
            var buildHavingBuilder = fixture.Create<BuildHavingBuilder>();

            // Act
            buildHavingBuilder.Id(123);

            // Assert
            buildHavingBuilder.GetLocator().Should().Be("id:123");
        }

        [Test]
        public void Id_AsLong()
        {
            // Arrange
            var fixture = Auto.Fixture();
            var buildHavingBuilder = fixture.Create<BuildHavingBuilder>();

            // Act
            buildHavingBuilder.Id(123L);

            // Assert
            buildHavingBuilder.GetLocator().Should().Be("id:123");
        }

        [Test]
        public void NotCancelled()
        {
            // Arrange
            var fixture = Auto.Fixture();
            var buildHavingBuilder = fixture.Create<BuildHavingBuilder>();

            // Act
            buildHavingBuilder.NotCancelled();

            // Assert
            buildHavingBuilder.GetLocator().Should().Be("canceled:False");
        }

        [Test]
        public void NotPersonal()
        {
            // Arrange
            var fixture = Auto.Fixture();
            var buildHavingBuilder = fixture.Create<BuildHavingBuilder>();

            // Act
            buildHavingBuilder.NotPersonal();

            // Assert
            buildHavingBuilder.GetLocator().Should().Be("personal:False");
        }

        [Test]
        public void NotPinned()
        {
            // Arrange
            var fixture = Auto.Fixture();
            var buildHavingBuilder = fixture.Create<BuildHavingBuilder>();

            // Act
            buildHavingBuilder.NotPinned();

            // Assert
            buildHavingBuilder.GetLocator().Should().Be("pinned:False");
        }

        [Test]
        public void NotRunning()
        {
            // Arrange
            var fixture = Auto.Fixture();
            var buildHavingBuilder = fixture.Create<BuildHavingBuilder>();

            // Act
            buildHavingBuilder.NotRunning();

            // Assert
            buildHavingBuilder.GetLocator().Should().Be("running:False");
        }

        [Test]
        public void Personal()
        {
            // Arrange
            var fixture = Auto.Fixture();
            var buildHavingBuilder = fixture.Create<BuildHavingBuilder>();

            // Act
            buildHavingBuilder.Personal();

            // Assert
            buildHavingBuilder.GetLocator().Should().Be("personal:True");
        }

        [Test]
        public void Pinned()
        {
            // Arrange
            var fixture = Auto.Fixture();
            var buildHavingBuilder = fixture.Create<BuildHavingBuilder>();

            // Act
            buildHavingBuilder.Pinned();

            // Assert
            buildHavingBuilder.GetLocator().Should().Be("pinned:True");
        }

        [Test]
        public void Running()
        {
            // Arrange
            var fixture = Auto.Fixture();
            var buildHavingBuilder = fixture.Create<BuildHavingBuilder>();

            // Act
            buildHavingBuilder.Running();

            // Assert
            buildHavingBuilder.GetLocator().Should().Be("running:True");
        }

        [Test]
        [Ignore("Ignore")]
        public void SinceDate()
        {
            // Arrange
            var fixture = Auto.Fixture();
            var buildHavingBuilder = fixture.Create<BuildHavingBuilder>();

            // Act
            buildHavingBuilder.SinceDate(new DateTime(2015, 10, 18, 16, 56, 0));

            // Assert
            buildHavingBuilder.GetLocator().Should().StartWith("sinceDate:20151018T135600%2b0000");
        }

        [Test]
        public void StatusError()
        {
            // Arrange
            var fixture = Auto.Fixture();
            var buildHavingBuilder = fixture.Create<BuildHavingBuilder>();

            // Act
            buildHavingBuilder.Status(BuildStatus.Error);

            // Assert
            buildHavingBuilder.GetLocator().Should().Be("status:ERROR");
        }

        [Test]
        public void StatusFailure()
        {
            // Arrange
            var fixture = Auto.Fixture();
            var buildHavingBuilder = fixture.Create<BuildHavingBuilder>();

            // Act
            buildHavingBuilder.Status(BuildStatus.Failure);

            // Assert
            buildHavingBuilder.GetLocator().Should().Be("status:FAILURE");
        }

        [Test]
        public void StatusSuccess()
        {
            // Arrange
            var fixture = Auto.Fixture();
            var buildHavingBuilder = fixture.Create<BuildHavingBuilder>();

            // Act
            buildHavingBuilder.Status(BuildStatus.Success);

            // Assert
            buildHavingBuilder.GetLocator().Should().Be("status:SUCCESS");
        }

        [Test]
        public void Tags_OneTag()
        {
            // Arrange
            var fixture = Auto.Fixture();
            var buildHavingBuilder = fixture.Create<BuildHavingBuilder>();

            // Act
            buildHavingBuilder.Tags("tag1");

            // Assert
            buildHavingBuilder.GetLocator().Should().Be("tags:tag1");
        }

        [Test]
        public void Tags_TwoTags()
        {
            // Arrange
            var fixture = Auto.Fixture();
            var buildHavingBuilder = fixture.Create<BuildHavingBuilder>();

            // Act
            buildHavingBuilder.Tags("tag1", "tag2");

            // Assert
            buildHavingBuilder.GetLocator().Should().Be("tags:tag1,tag2");
        }

        [Test]
        public void TriggeredBy()
        {
            // Arrange
            var fixture = Auto.Fixture();
            var userHavingBuilder = A.Fake<IUserHavingBuilder>();
            A.CallTo(() => userHavingBuilder.GetLocator()).Returns("id:123");

            var userHavingBuilderFactory = fixture.Freeze<IUserHavingBuilderFactory>();
            A.CallTo(() => userHavingBuilderFactory.CreateUserHavingBuilder()).Returns(userHavingBuilder);

            var buildHavingBuilder = fixture.Create<BuildHavingBuilder>();

            // Act
            buildHavingBuilder.TriggeredBy(_ => _.Id("123"));

            // Assert
            buildHavingBuilder.GetLocator().Should().Be("user:id:123");
        }
    }
}