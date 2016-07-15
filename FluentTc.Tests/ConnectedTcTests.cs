using System;
using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using FluentAssertions;
using FluentTc.Domain;
using FluentTc.Engine;
using FluentTc.Exceptions;
using FluentTc.Locators;
using FluentTc.Tests.Locators;
using NUnit.Framework;
using Ploeh.AutoFixture;

namespace FluentTc.Tests
{
    [TestFixture]
    public class ConnectedTcTests
    {
        private static Project CreateProject(string projectId, Project[] childProjects,
            BuildConfiguration[] buildConfigurations)
        {
            return new Project
            {
                Id = projectId,
                BuildTypes = new BuildTypeWrapper {BuildType = new List<BuildConfiguration>(buildConfigurations)},
                Projects = new ProjectWrapper {Project = new List<Project>(childProjects)}
            };
        }

        [Test]
        public void GetBuild_NoBuildsFound_BuildNotFoundExceptionThrown()
        {
            // Arrange
            Action<IBuildHavingBuilder> having = _ => _.Id(11);

            var fixture = Auto.Fixture();
            var buildsRetriever = fixture.Freeze<IBuildsRetriever>();
            A.CallTo(
                () => buildsRetriever.GetBuilds(having, A<Action<ICountBuilder>>._, A<Action<IBuildIncludeBuilder>>._))
                .Returns(new List<IBuild>());

            var connectedTc = fixture.Create<ConnectedTc>();

            // Act
            Action action = () => connectedTc.GetBuild(having);

            // Assert
            action.ShouldThrow<BuildNotFoundException>();
        }

        [Test]
        public void GetBuild_OneBuild_ThantBuild()
        {
            // Arrange
            Action<IBuildHavingBuilder> having = _ => _.AgentName("Bond");

            var fixture = Auto.Fixture();
            var buildsRetriever = fixture.Freeze<IBuildsRetriever>();
            var build = A.Fake<IBuild>();
            A.CallTo(() => build.Id).Returns(123);

            A.CallTo(
                () => buildsRetriever.GetBuilds(having, A<Action<ICountBuilder>>._, A<Action<IBuildIncludeBuilder>>._))
                .Returns(new List<IBuild>(new[] {build}));

            var connectedTc = fixture.Create<ConnectedTc>();

            // Act
            var actualBuild = connectedTc.GetBuild(having);

            // Assert
            actualBuild.Should().Be(build);
        }

        [Test]
        public void GetLastBuild_OneBuild_ThantBuild()
        {
            // Arrange
            Action<IBuildHavingBuilder> having = _ => _.AgentName("Bond");

            var fixture = Auto.Fixture();
            var buildsRetriever = fixture.Freeze<IBuildsRetriever>();
            var build = A.Fake<IBuild>();
            A.CallTo(() => build.Id).Returns(123);

            A.CallTo(
                () => buildsRetriever.GetBuilds(having, A<Action<ICountBuilder>>._, A<Action<IBuildIncludeBuilder>>._))
                .Returns(new List<IBuild>(new[] {build}));
            A.CallTo(() => buildsRetriever.GetBuild(123)).Returns(build);

            var connectedTc = fixture.Create<ConnectedTc>();

            // Act
            var actualBuild = connectedTc.GetLastBuild(having);

            // Assert
            actualBuild.Should().Be(build);
        }

        [Test]
        public void GetLastBuild_IncludeChangedFiles_BuildContainsChangesWithFiles()
        {
            // Arrange
            Action<IBuildHavingBuilder> having = _ => _.AgentName("Bond");

            var fixture = Auto.Fixture();
            var buildsRetriever = fixture.Freeze<IBuildsRetriever>();
            var build = A.Fake<IBuild>();
            A.CallTo(() => build.Id).Returns(123);
            A.CallTo(() => build.SetChanges(A<List<Change>>._)).Invokes(a =>
            {
                var changes = (List<Change>)a.Arguments[0];
                var fakedBuild = (IBuild)a.FakedObject;
                A.CallTo(() => fakedBuild.Changes).Returns(changes);
            });

            A.CallTo(
                () => buildsRetriever.GetBuilds(having, A<Action<ICountBuilder>>._, A<Action<IBuildIncludeBuilder>>._))
                .Returns(new List<IBuild>(new[] {build}));
            A.CallTo(() => buildsRetriever.GetBuild(123)).Returns(build);

            var changesRetriever = fixture.Freeze<IChangesRetriever>();
            A.CallTo(
                () =>
                    changesRetriever.GetChanges(A<Action<IChangesHavingBuilder>>._, A<Action<IChangesIncludeBuilder>>._))
                .Returns(new List<Change>()
                {
                    new Change {Files = new FileWrapper {File = new List<File>
                    {
                        new File{relativefile = "modifiedFile.cs"}
                    }}}
                });
            var connectedTc = fixture.Create<ConnectedTc>();

            // Act
            var actualBuild = connectedTc.GetLastBuild(having, _=>_.IncludeChanges(__=>__.IncludeFiles()));

            // Assert
            actualBuild.Should().Be(build);
            build.Changes.Single().Files.File.Single().relativefile.Should().Be("modifiedFile.cs");
        }

        [Test]
        public void GetBuild_TwoBuilds_MoreThanOneBuildFoundExceptionThrown()
        {
            // Arrange
            Action<IBuildHavingBuilder> having = _ => _.AgentName("Bond");

            var fixture = Auto.Fixture();
            var buildsRetriever = fixture.Freeze<IBuildsRetriever>();
            A.CallTo(
                () => buildsRetriever.GetBuilds(having, A<Action<ICountBuilder>>._, A<Action<IBuildIncludeBuilder>>._))
                .Returns(new List<IBuild>(new[] {A.Fake<IBuild>(), A.Fake<IBuild>()}));

            var connectedTc = fixture.Create<ConnectedTc>();

            // Act
            Action action = () => connectedTc.GetBuild(having);

            // Assert
            action.ShouldThrow<MoreThanOneBuildFoundException>();
        }

        [Test]
        public void GetBuildConfigurationsRecursively_ProjectWithChildProjectAndConfiguration_Retrieved()
        {
            // Arrange
            var childProject = CreateProject("childId", new Project[0],
                new[] {new BuildConfiguration {Id = "childConfig"}});
            var rootProject = CreateProject("rootId", new[] {childProject},
                new[] {new BuildConfiguration {Id = "rootConfig"}});

            var fixture = Auto.Fixture();
            var projectsRetriever = fixture.Freeze<IProjectsRetriever>();
            A.CallTo(() => projectsRetriever.GetProject("childId")).Returns(childProject);
            A.CallTo(() => projectsRetriever.GetProject("rootId")).Returns(rootProject);

            var connectedTc = fixture.Create<ConnectedTc>();

            // Act
            var buildConfigurations = connectedTc.GetBuildConfigurationsRecursively("rootId");

            // Assert
            buildConfigurations.Select(c => c.Id).ShouldAllBeEquivalentTo(new[] {"rootConfig", "childConfig"});
        }

        [Test]
        public void GetBuildStatistics_ProjectWithChildProjectAndConfiguration_Retrieved()
        {
            // Arrange
            var statistics = new List<IBuildStatistic>()
            {
                new BuildStatistic("TestName", "TestValue")
            };

            Action<IBuildHavingBuilder> having = _ => _.Id(123);

            var fixture = Auto.Fixture();
            var statisticsRetriever = fixture.Freeze<IBuildStatisticsRetriever>();
            A.CallTo(() => statisticsRetriever.GetBuildStatistics(having)).Returns(statistics);

            var connectedTc = fixture.Create<ConnectedTc>();

            // Act
            var buildStatistics = connectedTc.GetBuildStatistics(having);

            // Assert
            buildStatistics.Single().Name.Should().Be("TestName");
            buildStatistics.Single().Value.Should().Be("TestValue");
        }

        [Test]
        public void RunBuildConfiguration_BuildResponse()
        {
            // Arrange
            Action<IBuildConfigurationHavingBuilder> having = _ => _.Name("FluentTc");
            var fixture = Auto.Fixture();
            var connectedTc = fixture.Create<ConnectedTc>();

            // Act
            var build = connectedTc.RunBuildConfiguration(having);

            // Assert
            build.Should().NotBe(null);
        }
    }
}