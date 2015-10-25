using System;
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
            var teamCityCaller = A.Fake<TeamCityCaller>();
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
            var teamCityCaller = A.Fake<TeamCityCaller>();
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
        public void RunBuildConfiguration_ConfigurationName()
        {
            // Arrange
            Action<IBuildConfigurationHavingBuilder> having = _ => _.Name("FluentTc");
            var teamCityCaller = A.Fake<TeamCityCaller>();
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
        public void RunBuildConfiguration_ConfigurationNameWithParameters()
        {
            // Arrange
            Action<IBuildConfigurationHavingBuilder> having = _ => _.Name("FluentTc");
            var teamCityCaller = A.Fake<TeamCityCaller>();
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
        public void RunBuildConfiguration_OnAgentName()
        {
            // Arrange
            Action<IBuildConfigurationHavingBuilder> having = _ => _.Name("FluentTc");
            var teamCityCaller = A.Fake<TeamCityCaller>();
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
        public void RunBuildConfiguration_OnAgentNameWithParameters()
        {
            // Arrange
            Action<IBuildConfigurationHavingBuilder> having = _ => _.Name("FluentTc");
            var teamCityCaller = A.Fake<TeamCityCaller>();
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
            var teamCityCaller = A.Fake<TeamCityCaller>();
            A.CallTo(
                () =>
                    teamCityCaller.Get<BuildTypeWrapper>("/app/rest/buildTypes/name:FluentTc"))
                .Returns(new BuildTypeWrapper { BuildType = new List<BuildConfiguration>(new[] { new BuildConfiguration { Id = "bt987" } }) });

            var connectedTc = new RemoteTc().Connect(_ => _.AsGuest(), teamCityCaller);

            // Act
            var buildConfigurations = connectedTc.GetBuildConfigurations(having);

            // Assert
            buildConfigurations.Single().Id.Should().Be("bt987");
        }

    }
}