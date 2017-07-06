using System.Collections.Generic;
using FakeItEasy;
using FluentTc.Domain;
using FluentTc.Engine;
using NUnit.Framework;

namespace FluentTc.Tests
{
    [TestFixture]
    public class ProjectRetrievalTests
    {
        [TestCase("FluentTC")]
        [TestCase("Test 1")]
        [TestCase("Test%20C")]
        public void GetProject_ByName(string projectName)
        {
            // Arrange
            var teamCityCaller = CreateTeamCityCaller();
            new RemoteTc().Connect(_ => _.AsGuest(), teamCityCaller)
                          .GetProject(project => project.Name(projectName));

            //Expectations
            var expectedCall = string.Format(@"/app/rest/projects/name:{0}", projectName);
            A.CallTo(() => teamCityCaller.Get<Project>(expectedCall)).MustHaveHappened();
        }

        [TestCase("1234")]
        [TestCase("ABcd")]
        [TestCase("129@$$jd")]
        public void GetProject_ById(string projectId)
        {
            // Arrange
            var teamCityCaller = CreateTeamCityCaller();
            new RemoteTc().Connect(_ => _.AsGuest(), teamCityCaller)
                          .GetProject(project => project.Id(projectId));

            //Expectations
            var expectedCall = string.Format(@"/app/rest/projects/id:{0}", projectId);
            A.CallTo(() => teamCityCaller.Get<Project>(expectedCall)).MustHaveHappened();
        }

        private static TeamCityCaller CreateTeamCityCaller()
        {
            var teamCityCaller = A.Fake<TeamCityCaller>();
            var projectWrapper = new ProjectWrapper
            {
                Project = new List<Project>
                {
                    new Project
                    {
                        Id = "1234",
                        Name = "Test 1"
                    },
                    new Project
                    {
                        Id = "ABcd",
                        Name = "Test%20C"
                    },
                    new Project
                    {
                        Id = "129@$$jd",
                        Name = "FluentTC"
                    }
                }
            };
            A.CallTo(() => teamCityCaller.GetFormat<User>(A<string>._, A<object[]>._)).CallsBaseMethod();
            A.CallTo(() => teamCityCaller.GetFormat<UserWrapper>(A<string>._, A<object[]>._)).CallsBaseMethod();
            A.CallTo(() => teamCityCaller.GetFormat<InvestigationWrapper>(A<string>._, A<object[]>._)).CallsBaseMethod();
            A.CallTo(() => teamCityCaller.GetFormat<BuildModel>(A<string>._, A<object[]>._)).CallsBaseMethod();
            A.CallTo(() => teamCityCaller.GetFormat<BuildConfiguration>(A<string>._, A<object[]>._)).CallsBaseMethod();
            A.CallTo(() => teamCityCaller.GetFormat<ProjectWrapper>(A<string>._, A<object[]>._)).Returns(projectWrapper);
            A.CallTo(() => teamCityCaller.GetFormat<Project>(A<string>._, A<object[]>._)).CallsBaseMethod();
            A.CallTo(() => teamCityCaller.GetFormat<BuildTypeWrapper>(A<string>._, A<object[]>._)).CallsBaseMethod();
            A.CallTo(() => teamCityCaller.GetFormat<BuildWrapper>(A<string>._, A<object[]>._)).CallsBaseMethod();
            A.CallTo(() => teamCityCaller.GetFormat<BuildWrapper>(A<string>._)).CallsBaseMethod();
            A.CallTo(() => teamCityCaller.PostFormat(A<object>._, A<string>._, A<string>._, A<object[]>._)).CallsBaseMethod();
            A.CallTo(() => teamCityCaller.PostFormat<BuildConfiguration>(A<object>._, A<string>._, A<string>._, A<string>._, A<object[]>._)).CallsBaseMethod();
            A.CallTo(() => teamCityCaller.PostFormat<Project>(A<object>._, A<string>._, A<string>._, A<string>._, A<object[]>._)).CallsBaseMethod();
            A.CallTo(() => teamCityCaller.PutFormat(A<object>._, A<string>._, A<string>._, A<object[]>._)).CallsBaseMethod();
            A.CallTo(() => teamCityCaller.DeleteFormat(A<string>._, A<object[]>._)).CallsBaseMethod();
            return teamCityCaller;
        }
    }
}
