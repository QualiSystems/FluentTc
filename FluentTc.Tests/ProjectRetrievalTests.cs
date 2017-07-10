using System;
using System.Collections.Generic;
using FakeItEasy;
using FluentAssertions;
using FluentTc.Domain;
using FluentTc.Engine;
using FluentTc.Exceptions;
using FluentTc.Locators;
using NUnit.Framework;

namespace FluentTc.Tests
{
    [TestFixture]
    public class ProjectRetrievalTests
    {
        [TestCase("FluentTC")]
        [TestCase("Test 1")]
        [TestCase("Test!@#$%^&*()")]
        public void GetProject_ById(string projectId)
        {
            // Arrange
            var teamCityCaller = A.Fake<ITeamCityCaller>();
            A.CallTo(
                    () =>
                        teamCityCaller.GetFormat<ProjectWrapper>("/app/rest/projects/{0}",
                            string.Format("id:{0}", projectId)))
                .Returns(new ProjectWrapper
                {
                    Project = new List<Project> { new Project { Id = projectId, Name = "Test 1" } },
                    Count = "1"
                });

            var projectsRetriever = new ProjectsRetriever(new BuildProjectHavingBuilderFactory(), teamCityCaller);

            // Act
            var result = projectsRetriever.GetProject(project => project.Id(projectId));

            // Assert
            result.Should().NotBeNull();
            result.Name.Should().Be("Test 1");
            result.Id.Should().Be(projectId);
        }

        [TestCase("1234")]
        [TestCase("ABcd")]
        [TestCase("129@$$jd")]
        public void GetProject_ByName(string projectName)
        {
            // Arrange
            var teamCityCaller = A.Fake<ITeamCityCaller>();
            A.CallTo(
                    () =>
                        teamCityCaller.GetFormat<ProjectWrapper>("/app/rest/projects/{0}", 
                        string.Format("name:{0}", projectName)))
                .Returns(new ProjectWrapper
                {
                    Project = new List<Project> { new Project { Id = "Project1", Name = projectName } },
                    Count = "1"
                });
            
            var projectsRetriever = new ProjectsRetriever(new BuildProjectHavingBuilderFactory(), teamCityCaller);

            // Act
            var result = projectsRetriever.GetProject(project => project.Name(projectName));

            // Assert
            result.Should().NotBeNull();
            result.Name.Should().Be(projectName);
            result.Id.Should().Be("Project1");
        }

        [Test]
        public void GetProject_MultipleProjects_MoreThanOneProjectFoundExceptionThrown()
        {
            // Arrange
            var teamCityCaller = A.Fake<ITeamCityCaller>();
            A.CallTo(
                    () =>
                        teamCityCaller.GetFormat<ProjectWrapper>("/app/rest/projects/{0}", "name:exception"))
                .Returns(new ProjectWrapper { Project = new List<Project>{new Project(), new Project()}, Count = "2" });

            var projectsRetriever = new ProjectsRetriever(new BuildProjectHavingBuilderFactory(), teamCityCaller);

            // Act
            Action action = () => projectsRetriever.GetProject(project => project.Name("exception"));

            // Assert
            action.ShouldThrow<MoreThanOneProjectFoundException>();
        }

        [Test]
        public void GetProject_NoProjects_ProjectNotFoundException()
        {
            // Arrange
            var teamCityCaller = A.Fake<ITeamCityCaller>();
            A.CallTo(
                    () =>
                        teamCityCaller.GetFormat<ProjectWrapper>(
                            "/app/rest/projects?locator={0}",
                            A<object[]>.That.IsSameSequenceAs(new[] { "enabled:False" })))
                .Returns(new ProjectWrapper { Project = new List<Project>(), Count = "0" });

            var projectsRetriever = new ProjectsRetriever(new BuildProjectHavingBuilderFactory(), teamCityCaller);

            // Act
            Action action = () => projectsRetriever.GetProject(project => project.Name("exception"));

            // Assert
            action.ShouldThrow<ProjectNotFoundException>();
        }
    }
}
