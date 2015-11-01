﻿using System;
using System.Collections.Generic;
using System.Linq;
using EasyHttp.Http;
using FakeItEasy;
using FluentAssertions;
using FluentTc.Domain;
using FluentTc.Locators;
using NUnit.Framework;

namespace FluentTc.Tests
{
    [TestFixture]
    public class AcceptanceTests
    {
        [Test]
        public void DisableAgent_Id()
        {
            // Arrange
            var teamCityCaller = A.Fake<TeamCityCaller>();

            var connectedTc = new RemoteTc().Connect(_ => _.AsGuest(), teamCityCaller);

            // Act
            connectedTc.DisableAgent(_ => _.Id(123));

            // Assert
            A.CallTo(
                () =>
                    teamCityCaller.PutFormat("False", "text/plain", "/app/rest/agents/{0}/enabled",
                        A<object[]>.That.IsSameSequenceAs(new[] {"id:123"}))).MustHaveHappened();
        }

        [Test]
        public void EnableAgent_Id()
        {
            // Arrange
            var teamCityCaller = A.Fake<TeamCityCaller>();

            var connectedTc = new RemoteTc().Connect(_ => _.AsGuest(), teamCityCaller);

            // Act
            connectedTc.EnableAgent(_ => _.Id(123));

            // Assert
            A.CallTo(
                () =>
                    teamCityCaller.PutFormat("True", "text/plain", "/app/rest/agents/{0}/enabled",
                        A<object[]>.That.IsSameSequenceAs(new[] {"id:123"}))).MustHaveHappened();
        }

        [Test]
        public void GetBuild_Id_Build()
        {
            // Arrange
            var teamCityCaller = CreateTeamCityCaller();
            A.CallTo(
                () =>
                    teamCityCaller.Get<BuildWrapper>(
                        "/app/rest/builds?locator=id:123,count:-1,&fields=count,build(buildTypeId,href,id,number,state,status,webUrl)"))
                .Returns(new BuildWrapper {Count = "1", Build = new List<Build>(new[] {new Build {Id = 987}})});

            var connectedTc = new RemoteTc().Connect(_ => _.AsGuest(), teamCityCaller);

            // Act
            var build = connectedTc.GetBuild(_ => _.Id(123));

            // Assert
            build.Id.Should().Be(987);
        }        
        
        [Test]
        public void GetBuilds_BuildConfigurationName()
        {
            // Arrange
            var teamCityCaller = CreateTeamCityCaller();
            var build = new Build {Id = 987};
            A.CallTo(
                () =>
                    teamCityCaller.Get<BuildWrapper>(
                        "/app/rest/builds?locator=buildType:name:FluentTc,count:-1,&fields=count,build(buildTypeId,href,id,number,state,status,webUrl)"))
                .Returns(new BuildWrapper {Count = "1", Build = new List<Build>(new[] {build})});

            var connectedTc = new RemoteTc().Connect(_ => _.AsGuest(), teamCityCaller);

            // Act
            var builds = connectedTc.GetBuilds(_ => _.BuildConfiguration(__ => __.Name("FluentTc")));

            // Assert
            builds.ShouldAllBeEquivalentTo(new [] { build });
        }

        [Test]
        public void GetBuilds_SinceDate()
        {
            // Arrange
            var teamCityCaller = CreateTeamCityCaller();
            var build = new Build {Id = 987};

            A.CallTo(
                () =>
                    teamCityCaller.Get<BuildWrapper>(
                        "/app/rest/builds?locator=sinceDate:20151026T142200%2b0000,count:-1,&fields=count,build(buildTypeId,href,id,number,state,status,webUrl)"))
                .Returns(new BuildWrapper {Count = "1", Build = new List<Build>(new[] {build})});

            var connectedTc = new RemoteTc().Connect(_ => _.AsGuest(), teamCityCaller);

            // Act
            var builds = connectedTc.GetBuilds(_ => _.SinceDate(new DateTime(2015,10,26,16,22,0)));

            // Assert
            builds.ShouldAllBeEquivalentTo(new [] { build });
        }

        [Test]
        public void SetParameters_ConfigurationName()
        {
            // Arrange
            var teamCityCaller = A.Fake<TeamCityCaller>();

            var connectedTc = new RemoteTc().Connect(_ => _.AsGuest(), teamCityCaller);

            // Act
            connectedTc.SetParameters(_ => _.Name("FluentTc"), p=>p.Parameter("name","newVal"));

            // Assert
            A.CallTo(
                () =>
                    teamCityCaller.PutFormat("newVal", HttpContentTypes.TextPlain, "/app/rest/buildTypes/{0}/parameters/{1}", A<object[]>.That.IsSameSequenceAs(new[] {"name:FluentTc", "name"})))
                        .MustHaveHappened(Repeated.Exactly.Once);
        }

        [Test]
        [Ignore]
        public void RunBuildConfiguration_ConfigurationName()
        {
            // Arrange
            Action<IBuildConfigurationHavingBuilder> having = _ => _.Name("FluentTc");
            var teamCityCaller = CreateTeamCityCaller();
            var buildConfigurationRetriever = A.Fake<IBuildConfigurationRetriever>();
            A.CallTo(() => buildConfigurationRetriever.GetSingleBuildConfiguration(having))
                .Returns(new BuildConfiguration {Id = "bt2"});

            var connectedTc = new RemoteTc().Connect(_ => _.AsGuest(), teamCityCaller, buildConfigurationRetriever);

            // Act
            connectedTc.RunBuildConfiguration(having);

            // Assert
            A.CallTo(
                () =>
                    teamCityCaller.Post(@"<build>
<buildType id=""bt2""/>
</build>
", HttpContentTypes.ApplicationXml, "/app/rest/buildQueue", string.Empty))
                        .MustHaveHappened(Repeated.Exactly.Once);
        }

        [Test]
        [Ignore]
        public void RunBuildConfiguration_ConfigurationNameWithParameters()
        {
            // Arrange
            Action<IBuildConfigurationHavingBuilder> having = _ => _.Name("FluentTc");
            var teamCityCaller = CreateTeamCityCaller();
            var buildConfigurationRetriever = A.Fake<IBuildConfigurationRetriever>();
            A.CallTo(() => buildConfigurationRetriever.GetSingleBuildConfiguration(having))
                .Returns(new BuildConfiguration {Id = "bt2"});

            var connectedTc = new RemoteTc().Connect(_ => _.AsGuest(), teamCityCaller, buildConfigurationRetriever);

            // Act
            connectedTc.RunBuildConfiguration(having, p => p.Parameter("param1","value1"));

            // Assert
            A.CallTo(
                () =>
                    teamCityCaller.Post(@"<build>
<buildType id=""bt2""/>
<properties>
<property name=""param1"" value=""value1""/>
</properties>
</build>
", HttpContentTypes.ApplicationXml, "/app/rest/buildQueue", string.Empty))
                        .MustHaveHappened(Repeated.Exactly.Once);
        }

        [Test]
        [Ignore]
        public void RunBuildConfiguration_OnAgentName()
        {
            // Arrange
            Action<IBuildConfigurationHavingBuilder> having = _ => _.Name("FluentTc");
            var teamCityCaller = CreateTeamCityCaller();

            var buildConfigurationRetriever = A.Fake<IBuildConfigurationRetriever>();
            A.CallTo(() => buildConfigurationRetriever.GetSingleBuildConfiguration(having))
                .Returns(new BuildConfiguration {Id = "bt2"});

            Action<IAgentHavingBuilder> onAgent = p => p.Name("agent1");
            var agentsRetriever = A.Fake<IAgentsRetriever>();
            A.CallTo(() => agentsRetriever.GetAgent(onAgent)).Returns(new Agent() {Id = 9});

            var connectedTc = new RemoteTc().Connect(_ => _.AsGuest(), teamCityCaller, buildConfigurationRetriever, agentsRetriever);

            // Act
            connectedTc.RunBuildConfiguration(having, onAgent);

            // Assert
            A.CallTo(
                () =>
                    teamCityCaller.Post(@"<build>
<buildType id=""bt2""/>
<agent id=""9""/>
</build>
", HttpContentTypes.ApplicationXml, "/app/rest/buildQueue", string.Empty))
                        .MustHaveHappened(Repeated.Exactly.Once);
        }

        [Test]
        [Ignore]
        public void RunBuildConfiguration_OnAgentNameWithParameters()
        {
            // Arrange
            Action<IBuildConfigurationHavingBuilder> having = _ => _.Name("FluentTc");
            var teamCityCaller = CreateTeamCityCaller();
            var buildConfigurationRetriever = A.Fake<IBuildConfigurationRetriever>();
            A.CallTo(() => buildConfigurationRetriever.GetSingleBuildConfiguration(having))
                .Returns(new BuildConfiguration {Id = "bt2"});

            Action<IAgentHavingBuilder> onAgent = p => p.Name("agent1");
            var agentsRetriever = A.Fake<IAgentsRetriever>();
            A.CallTo(() => agentsRetriever.GetAgent(onAgent)).Returns(new Agent() {Id = 9});

            var connectedTc = new RemoteTc().Connect(_ => _.AsGuest(), teamCityCaller, buildConfigurationRetriever, agentsRetriever);

            // Act
            connectedTc.RunBuildConfiguration(having, onAgent, p => p.Parameter("param1", "value1"));

            // Assert
            A.CallTo(
                () =>
                    teamCityCaller.Post(@"<build>
<buildType id=""bt2""/>
<agent id=""9""/>
<properties>
<property name=""param1"" value=""value1""/>
</properties>
</build>
", HttpContentTypes.ApplicationXml, "/app/rest/buildQueue", string.Empty))
                        .MustHaveHappened(Repeated.Exactly.Once);
        }

        [Test]
        public void GetBuildConfigurations_ByName()
        {
            // Arrange
            Action<IBuildConfigurationHavingBuilder> having = _ => _.Name("FluentTc");
            var teamCityCaller = CreateTeamCityCaller();
            A.CallTo(
                () =>
                    teamCityCaller.Get<BuildTypeWrapper>("/app/rest/buildTypes?locator=name:FluentTc"))
                .Returns(new BuildTypeWrapper { BuildType = new List<BuildConfiguration>(new[] { new BuildConfiguration { Id = "bt987" } }) });

            var connectedTc = new RemoteTc().Connect(_ => _.AsGuest(), teamCityCaller);

            // Act
            var buildConfigurations = connectedTc.GetBuildConfigurations(having);

            // Assert
            buildConfigurations.Single().Id.Should().Be("bt987");
        }

        [Test]
        public void CreateBuildConfiguration_ByName()
        {
            // Arrange
            Action<IBuildProjectHavingBuilder> having = _ => _.Name("OpenSource");
            var teamCityCaller = CreateTeamCityCaller();

            var connectedTc = new RemoteTc().Connect(_ => _.AsGuest(), teamCityCaller);

            // Act
            BuildConfiguration buildConfiguration = connectedTc.CreateBuildConfiguration(having, "NewConfig");

            // Assert
            A.CallTo(() => teamCityCaller.Post("NewConfig", HttpContentTypes.TextPlain, "/app/rest/projects/name:OpenSource/buildTypes", HttpContentTypes.ApplicationJson)).MustHaveHappened();
        }

        [Test]
        public void AttachBuildConfigurationToTemplate()
        {
            // Arrange
            Action<IBuildConfigurationHavingBuilder> having = _ => _.Name("FluentTc");
            var teamCityCaller = CreateTeamCityCaller();

            var connectedTc = new RemoteTc().Connect(_ => _.AsGuest(), teamCityCaller);

            // Act
            connectedTc.AttachBuildConfigurationToTemplate(having, "BuildTemplateId");

            // Assert
            A.CallTo(
                () =>
                    teamCityCaller.Put("BuildTemplateId", HttpContentTypes.TextPlain, "/app/rest/buildTypes/name:FluentTc/template", string.Empty)).MustHaveHappened();
        }

        [Test]
        public void RemoveBuildFromQueue_ProjectName()
        {
            // Arrange
            var teamCityCaller = CreateTeamCityCaller();

            var connectedTc = new RemoteTc().Connect(_ => _.AsGuest(), teamCityCaller);

            // Act
            connectedTc.RemoveBuildFromQueue(_ => _.Project(__ => __.Id("FluentTc")));

            // Assert
            A.CallTo(
                () =>
                    teamCityCaller.Delete("/app/rest/buildQueue/?locator=project:id:FluentTc")).MustHaveHappened();
        }          
        
        [Test]
        public void RemoveBuildFromQueue_BuildId()
        {
            // Arrange
            var teamCityCaller = CreateTeamCityCaller();

            var connectedTc = new RemoteTc().Connect(_ => _.AsGuest(), teamCityCaller);

            // Act
            connectedTc.RemoveBuildFromQueue(_ => _.Id(123));

            // Assert
            A.CallTo(() => teamCityCaller.Delete("/app/rest/buildQueue/id:123")).MustHaveHappened();
        }   

        [Test]
        public void GetProjectById()
        {
            // Arrange
            var teamCityCaller = CreateTeamCityCaller();

            var connectedTc = new RemoteTc().Connect(_ => _.AsGuest(), teamCityCaller);

            // Act
            connectedTc.GetProjectById("FluentTc");

            // Assert
            A.CallTo(() => teamCityCaller.Get<Project>(@"/app/rest/projects/id:FluentTc")).MustHaveHappened();
        }    

        [Test]
        public void GetAllProjects()
        {
            // Arrange
            var teamCityCaller = CreateTeamCityCaller();

            var connectedTc = new RemoteTc().Connect(_ => _.AsGuest(), teamCityCaller);

            // Act
            connectedTc.GetAllProjects();

            // Assert
            A.CallTo(() => teamCityCaller.Get<ProjectWrapper>(@"/app/rest/projects/")).MustHaveHappened();
        }    

        [Test]
        public void GetBuildsQueue_All()
        {
            // Arrange
            var teamCityCaller = CreateTeamCityCaller();

            var connectedTc = new RemoteTc().Connect(_ => _.AsGuest(), teamCityCaller);
            A.CallTo(() => teamCityCaller.Get<BuildWrapper>(@"/app/rest/buildQueue"))
                .Returns(new BuildWrapper {Count = "0"});

            // Act
            connectedTc.GetBuildsQueue();

            // Assert
            A.CallTo(() => teamCityCaller.Get<BuildWrapper>(@"/app/rest/buildQueue")).MustHaveHappened();
        }    

        [Test]
        public void GetBuildsQueue_ByProjectId()
        {
            // Arrange
            var teamCityCaller = CreateTeamCityCaller();

            var connectedTc = new RemoteTc().Connect(_ => _.AsGuest(), teamCityCaller);
            A.CallTo(() => teamCityCaller.Get<BuildWrapper>(@"/app/rest/buildQueue?locator=project:id:Trunk"))
                .Returns(new BuildWrapper {Count = "0"});

            // Act
            connectedTc.GetBuildsQueue(_ => _.Project(p => p.Id("Trunk")));

            // Assert
            A.CallTo(() => teamCityCaller.Get<BuildWrapper>(@"/app/rest/buildQueue?locator=project:id:Trunk")).MustHaveHappened();
        }

        [Test]
        public void LocalTc_AgentName()
        {
            new LocalTc().BuildParameters.AgentName.Should().NotBeEmpty();
        }

        private static ITeamCityCaller CreateTeamCityCaller()
        {
            var teamCityCaller = A.Fake<TeamCityCaller>();
            A.CallTo(() => teamCityCaller.GetFormat<ProjectWrapper>(A<string>._, A<object[]>._)).CallsBaseMethod();
            A.CallTo(() => teamCityCaller.GetFormat<Project>(A<string>._, A<object[]>._)).CallsBaseMethod();
            A.CallTo(() => teamCityCaller.GetFormat<BuildTypeWrapper>(A<string>._, A<object[]>._)).CallsBaseMethod();
            A.CallTo(() => teamCityCaller.GetFormat<BuildWrapper>(A<string>._, A<object[]>._)).CallsBaseMethod();
            A.CallTo(() => teamCityCaller.PostFormat(A<object>._, A<string>._, A<string>._, A<object[]>._)).CallsBaseMethod();
            A.CallTo(() => teamCityCaller.PostFormat<string>(A<string>._, A<string>._, A<string>._, A<string>._, A<object[]>._)).CallsBaseMethod();
            A.CallTo(() => teamCityCaller.PostFormat<BuildConfiguration>(A<object>._, A<string>._, A<string>._, A<string>._, A<object[]>._)).CallsBaseMethod();
            A.CallTo(() => teamCityCaller.PutFormat(A<object>._, A<string>._, A<string>._, A<object[]>._)).CallsBaseMethod();
            A.CallTo(() => teamCityCaller.DeleteFormat(A<string>._, A<object[]>._)).CallsBaseMethod();
            return teamCityCaller;
        }
    }
}