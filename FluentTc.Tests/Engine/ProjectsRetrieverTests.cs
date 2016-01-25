using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using FluentAssertions;
using FluentTc.Domain;
using FluentTc.Engine;
using FluentTc.Locators;
using NUnit.Framework;

namespace FluentTc.Tests.Engine
{
    [TestFixture]
    public class ProjectsRetrieverTests
    {
        [Test]
        public void GetProjects_NoLocator_AllProjects()
        {
            // Arrange
            var teamCityCaller = A.Fake<ITeamCityCaller>();
            A.CallTo(() => teamCityCaller.GetFormat<ProjectWrapper>("/app/rest/projects/{0}", ""))
                .Returns(new ProjectWrapper {Project = new List<Project>(new[] {new Project {Id = "123"}})});
            var projectsRetriever = new ProjectsRetriever(A.Fake<IBuildProjectHavingBuilderFactory>(), teamCityCaller);

            // Act
            var projects = projectsRetriever.GetProjects();

            // Assert
            projects.Single().Id.Should().Be("123");
        }

        [Test]
        public void GetProjects_Name_ProjectWithName()
        {
            // Arrange
            var teamCityCaller = A.Fake<ITeamCityCaller>();
            A.CallTo(() => teamCityCaller.GetFormat<ProjectWrapper>("/app/rest/projects/{0}", "name:Proj1"))
                .Returns(new ProjectWrapper {Project = new List<Project>(new[] {new Project {Id = "123"}})});
            
            var buildProjectHavingBuilderFactory = A.Fake<IBuildProjectHavingBuilderFactory>();
            A.CallTo(() => buildProjectHavingBuilderFactory.CreateBuildProjectHavingBuilder())
                .Returns(new BuildProjectHavingBuilder());

            var projectsRetriever = new ProjectsRetriever(buildProjectHavingBuilderFactory, teamCityCaller);

            // Act
            var projects = projectsRetriever.GetProjects(a=>a.Name("Proj1"));

            // Assert
            projects.Single().Id.Should().Be("123");
        }

        [Test]
        public void GetProject_Id_Project()
        {
            // Arrange
            var teamCityCaller = A.Fake<ITeamCityCaller>();
            A.CallTo(() => teamCityCaller.GetFormat<Project>("/app/rest/projects/id:{0}", "ProjId1"))
                .Returns(new Project { Id = "ProjId1" });
            
            var buildProjectHavingBuilderFactory = A.Fake<IBuildProjectHavingBuilderFactory>();
            A.CallTo(() => buildProjectHavingBuilderFactory.CreateBuildProjectHavingBuilder())
                .Returns(new BuildProjectHavingBuilder());

            var projectsRetriever = new ProjectsRetriever(buildProjectHavingBuilderFactory, teamCityCaller);

            // Act
            var project = projectsRetriever.GetProject("ProjId1");

            // Assert
            project.Id.Should().Be("ProjId1");
        }
    }
}