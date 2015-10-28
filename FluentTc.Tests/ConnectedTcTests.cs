using System;
using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using FluentAssertions;
using FluentTc.Domain;
using FluentTc.Locators;
using FluentTc.Tests.Locators;
using NUnit.Framework;
using Ploeh.AutoFixture;

namespace FluentTc.Tests
{
    [TestFixture]
    public class ConnectedTcTests
    {
        [Test]
        public void GetBuild_NoBuildsFound_BuildNotFoundExceptionThrown()
        {
            // Arrange
            Action<IBuildHavingBuilder> having = _ => _.Id(11);

            var fixture = Auto.Fixture();
            var buildsRetriever = fixture.Freeze<IBuildsRetriever>();
            A.CallTo(() => buildsRetriever.GetBuilds(having, A<Action<ICountBuilder>>._, A<Action<IBuildIncludeBuilder>>._))
                .Returns(new List<Build>());

            var connectedTc = fixture.Create<ConnectedTc>();

            // Act
            Action action = () => connectedTc.GetBuild(having);

            // Assert
            action.ShouldThrow<BuildNotFoundException>();
        }

        [Test]
        public void GetBuild_TwoBuilds_MoreThanOneBuildFoundExceptionThrown()
        {
            // Arrange
            Action<IBuildHavingBuilder> having = _ => _.AgentName("Bond");

            var fixture = Auto.Fixture();
            var buildsRetriever = fixture.Freeze<IBuildsRetriever>();
            A.CallTo(() => buildsRetriever.GetBuilds(having, A<Action<ICountBuilder>>._, A<Action<IBuildIncludeBuilder>>._))
                .Returns(new List<Build>(new [] { new Build(), new Build() }));

            var connectedTc = fixture.Create<ConnectedTc>();

            // Act
            Action action = () => connectedTc.GetBuild(having);

            // Assert
            action.ShouldThrow<MoreThanOneBuildFoundException>();
        }

        [Test]
        public void GetBuild_OneBuild_ThantBuild()
        {
            // Arrange
            Action<IBuildHavingBuilder> having = _ => _.AgentName("Bond");

            var fixture = Auto.Fixture();
            var buildsRetriever = fixture.Freeze<IBuildsRetriever>();
            var build = new Build { Id = 123};
            A.CallTo(() => buildsRetriever.GetBuilds(having, A<Action<ICountBuilder>>._, A<Action<IBuildIncludeBuilder>>._))
                .Returns(new List<Build>(new [] { build }));

            var connectedTc = fixture.Create<ConnectedTc>();

            // Act
            var actualBuild = connectedTc.GetBuild(having);

            // Assert
            actualBuild.Should().Be(build);
        }

        [Test]
        public void GetBuildConfigurationsRecursively_ProjectWithChildProjectAndConfiguration_Retrieved()
        {
            // Arrange
            var childProject = CreateProject("childId", new Project[0], new[]{ new BuildConfiguration { Id = "childConfig"}, });
            var rootProject = CreateProject("rootId", new[] { childProject, }, new [] { new BuildConfiguration() { Id = "rootConfig"} });

            var fixture = Auto.Fixture();
            var projectsRetriever = fixture.Freeze<IProjectsRetriever>();
            A.CallTo(() => projectsRetriever.GetProject("childId")).Returns(childProject);
            A.CallTo(() => projectsRetriever.GetProject("rootId")).Returns(rootProject);

            var connectedTc = fixture.Create<ConnectedTc>();

            // Act
            var buildConfigurations = connectedTc.GetBuildConfigurationsRecursively("rootId");

            // Assert
            buildConfigurations.Select(c => c.Id).ShouldAllBeEquivalentTo(new[] { "rootConfig", "childConfig" });
        }

        private static Project CreateProject(string projectId, Project[] childProjects, BuildConfiguration[] buildConfigurations)
        {
            return new Project { 
                Id = projectId, 
                BuildTypes = new BuildTypeWrapper { BuildType = new List<BuildConfiguration>(buildConfigurations) }, 
                Projects = new ProjectWrapper { Project = new List<Project>(childProjects )}};
        }
    }
}