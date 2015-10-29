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
            var havingBuilder = buildHavingBuilder.AgentName("bond");

            // Assert
            havingBuilder.GetLocator().Should().Be("agentName:bond");
        }

        [Test]
        public void Branch()
        {
            // Arrange
            var fixture = Auto.Fixture();
            var branchHavingBuilder = A.Fake<IBranchHavingBuilder>();
            A.CallTo(() => branchHavingBuilder.GetLocator()).Returns("name:Branch1");

            var userHavingBuilderFactory = fixture.Freeze<IBranchHavingBuilderFactory>();
            A.CallTo(() => userHavingBuilderFactory.CreateBranchHavingBuilder()).Returns(branchHavingBuilder);

            var buildHavingBuilder = fixture.Create<BuildHavingBuilder>();

            // Act
            var havingBuilder = buildHavingBuilder.Branch(_ => _.Name("Branch1"));

            // Assert
            havingBuilder.GetLocator().Should().Be("branch:name:Branch1");
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
            var havingBuilder = buildHavingBuilder.BuildConfiguration(havingBuildConfig);

            // Assert
            havingBuilder.GetLocator().Should().Be("buildType:id:bt2");
        }

        [Test]
        public void Cancelled()
        {
            // Arrange
            var fixture = Auto.Fixture();
            var buildHavingBuilder = fixture.Create<BuildHavingBuilder>();

            // Act
            var havingBuilder = buildHavingBuilder.Cancelled();

            // Assert
            havingBuilder.GetLocator().Should().Be("cancelled:True");
        }

        [Test]
        public void Id()
        {
            // Arrange
            var fixture = Auto.Fixture();
            var buildHavingBuilder = fixture.Create<BuildHavingBuilder>();

            // Act
            var havingBuilder = buildHavingBuilder.Id(123);

            // Assert
            havingBuilder.GetLocator().Should().Be("id:123");
        }

        [Test]
        public void Id_AsLong()
        {
            // Arrange
            var fixture = Auto.Fixture();
            var buildHavingBuilder = fixture.Create<BuildHavingBuilder>();

            // Act
            var havingBuilder = buildHavingBuilder.Id(123L);

            // Assert
            havingBuilder.GetLocator().Should().Be("id:123");
        }

        [Test]
        public void NotCancelled()
        {
            // Arrange
            var fixture = Auto.Fixture();
            var buildHavingBuilder = fixture.Create<BuildHavingBuilder>();

            // Act
            var havingBuilder = buildHavingBuilder.NotCancelled();

            // Assert
            havingBuilder.GetLocator().Should().Be("cancelled:False");
        }

        [Test]
        public void NotPersonal()
        {
            // Arrange
            var fixture = Auto.Fixture();
            var buildHavingBuilder = fixture.Create<BuildHavingBuilder>();

            // Act
            var havingBuilder = buildHavingBuilder.NotPersonal();

            // Assert
            havingBuilder.GetLocator().Should().Be("personal:False");
        }

        [Test]
        public void NotPinned()
        {
            // Arrange
            var fixture = Auto.Fixture();
            var buildHavingBuilder = fixture.Create<BuildHavingBuilder>();

            // Act
            var havingBuilder = buildHavingBuilder.NotPinned();

            // Assert
            havingBuilder.GetLocator().Should().Be("pinned:False");
        }

        [Test]
        public void NotRunning()
        {
            // Arrange
            var fixture = Auto.Fixture();
            var buildHavingBuilder = fixture.Create<BuildHavingBuilder>();

            // Act
            var havingBuilder = buildHavingBuilder.NotRunning();

            // Assert
            havingBuilder.GetLocator().Should().Be("running:False");
        }

        [Test]
        public void Personal()
        {
            // Arrange
            var fixture = Auto.Fixture();
            var buildHavingBuilder = fixture.Create<BuildHavingBuilder>();

            // Act
            var havingBuilder = buildHavingBuilder.Personal();

            // Assert
            havingBuilder.GetLocator().Should().Be("personal:True");
        }

        [Test]
        public void Pinned()
        {
            // Arrange
            var fixture = Auto.Fixture();
            var buildHavingBuilder = fixture.Create<BuildHavingBuilder>();

            // Act
            var havingBuilder = buildHavingBuilder.Pinned();

            // Assert
            havingBuilder.GetLocator().Should().Be("pinned:True");
        }

        [Test]
        public void Running()
        {
            // Arrange
            var fixture = Auto.Fixture();
            var buildHavingBuilder = fixture.Create<BuildHavingBuilder>();

            // Act
            var havingBuilder = buildHavingBuilder.Running();

            // Assert
            havingBuilder.GetLocator().Should().Be("running:True");
        }

        [Test]
        public void SinceDate()
        {
            // Arrange
            var fixture = Auto.Fixture();
            var buildHavingBuilder = fixture.Create<BuildHavingBuilder>();

            // Act
            var havingBuilder = buildHavingBuilder.SinceDate(new DateTime(2015, 10, 18, 16, 56, 0));

            // Assert
            havingBuilder.GetLocator().Should().StartWith("sinceDate:20151018T135600%2b0000");
        }

        [Test]
        public void StatusError()
        {
            // Arrange
            var fixture = Auto.Fixture();
            var buildHavingBuilder = fixture.Create<BuildHavingBuilder>();

            // Act
            var havingBuilder = buildHavingBuilder.Status(BuildStatus.Error);

            // Assert
            havingBuilder.GetLocator().Should().Be("status:ERROR");
        }

        [Test]
        public void StatusFailure()
        {
            // Arrange
            var fixture = Auto.Fixture();
            var buildHavingBuilder = fixture.Create<BuildHavingBuilder>();

            // Act
            var havingBuilder = buildHavingBuilder.Status(BuildStatus.Failure);

            // Assert
            havingBuilder.GetLocator().Should().Be("status:FAILURE");
        }

        [Test]
        public void StatusSuccess()
        {
            // Arrange
            var fixture = Auto.Fixture();
            var buildHavingBuilder = fixture.Create<BuildHavingBuilder>();

            // Act
            var havingBuilder = buildHavingBuilder.Status(BuildStatus.Success);

            // Assert
            havingBuilder.GetLocator().Should().Be("status:SUCCESS");
        }

        [Test]
        public void Tags_OneTag()
        {
            // Arrange
            var fixture = Auto.Fixture();
            var buildHavingBuilder = fixture.Create<BuildHavingBuilder>();

            // Act
            var havingBuilder = buildHavingBuilder.Tags("tag1");

            // Assert
            havingBuilder.GetLocator().Should().Be("tags:tag1");
        }

        [Test]
        public void Tags_TwoTags()
        {
            // Arrange
            var fixture = Auto.Fixture();
            var buildHavingBuilder = fixture.Create<BuildHavingBuilder>();

            // Act
            var havingBuilder = buildHavingBuilder.Tags("tag1", "tag2");

            // Assert
            havingBuilder.GetLocator().Should().Be("tags:tag1,tag2");
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
            var havingBuilder = buildHavingBuilder.TriggeredBy(_ => _.InternalUserId("123"));

            // Assert
            havingBuilder.GetLocator().Should().Be("user:id:123");
        }
    }
}