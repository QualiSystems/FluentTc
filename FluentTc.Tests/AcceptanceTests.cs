﻿using System;
using System.Collections.Generic;
using System.Linq;
using EasyHttp.Http;
using FakeItEasy;
using FluentAssertions;
using FluentTc.Domain;
using FluentTc.Engine;
using FluentTc.Locators;
using NUnit.Framework;
using System.Security;

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
                        "/app/rest/builds?locator=id:123,&fields=count,build(buildTypeId,href,id,number,state,status,webUrl)"))
                .Returns(new BuildWrapper {Count = "1", Build = new List<BuildModel>(new[] {new BuildModel {Id = 987, Status = "SUCCESS"}})});

            var connectedTc = new RemoteTc().Connect(_ => _.AsGuest(), teamCityCaller);

            // Act
            var build = connectedTc.GetBuild(_ => _.Id(123));

            // Assert
            build.Id.Should().Be(987);
        }        

        [Test]
        public void GetLastBuild_BuildConfigurationStatus_BuildWithWithAllDetails()
        {
            // Arrange
            var teamCityCaller = CreateTeamCityCaller();
            A.CallTo(
                () =>
                    teamCityCaller.Get<BuildWrapper>(
                        "/app/rest/builds?locator=buildType:id:bt2,status:SUCCESS,count:1,&fields=count,build(buildTypeId,href,id,number,state,status,webUrl)"))
                .Returns(new BuildWrapper {Count = "1", Build = new List<BuildModel>(new[] {new BuildModel {Id = 987, Status = "FAILURE"}})});
            A.CallTo(() => teamCityCaller.Get<BuildModel>("/app/rest/builds/id:987"))
                .Returns(new BuildModel { Id = 987, Status = "SUCCESS" });

            var connectedTc = new RemoteTc().Connect(_ => _.AsGuest(), teamCityCaller);

            // Act
            var build =
                connectedTc.GetLastBuild(_ => _.BuildConfiguration(__ => __.Id("bt2")).Status(BuildStatus.Success));

            // Assert
            build.Id.Should().Be(987);
            build.Status.Should().Be(BuildStatus.Success);
        }        

        [Test]
        public void GetBuildFullResponse_Id_Build()
        {
            // Arrange
            var teamCityCaller = CreateTeamCityCaller();
            A.CallTo(() => teamCityCaller.Get<BuildModel>("/app/rest/builds/id:123")).Returns(new BuildModel { Id = 123, Status = "SUCCESS"});

            var connectedTc = new RemoteTc().Connect(_ => _.AsGuest(), teamCityCaller);

            // Act
            var build = connectedTc.GetBuild(123);

            // Assert
            build.Id.Should().Be(123);
        }

        [Test]
        public void GetBuildWithRevisions()
        {
            // Arrange
            var teamCityCaller = CreateTeamCityCaller();
            A.CallTo(() => teamCityCaller.Get<BuildModel>("/app/rest/builds/id:123")).Returns(new BuildModel
            {
                Id = 123,
                Status = "SUCCESS",
                Revisions = new RevisionsWrapper()
                {
                    Revision = new List<Change>
                    {
                        new Change
                        {
                            VcsBranchName = "refs/head/master"
                        }
                    }
                }
            });

            var connectedTc = new RemoteTc().Connect(_ => _.AsGuest(), teamCityCaller);

            // Act
            var build = connectedTc.GetBuild(123);

            // Assert
            build.Id.Should().Be(123);
            build.Revisions.Should().NotBeNull();
            build.Revisions.Revision.Count.Should().Be(1);
            build.Revisions.Revision.First().VcsBranchName.Should().Be("refs/head/master");
        }

        [Test]
        public void GetBuildFullResponse_TestOccurrences_Build()
        {
            // Arrange
            var teamCityCaller = CreateTeamCityCaller();
            A.CallTo(() => teamCityCaller.Get<BuildModel>("/app/rest/builds/id:123")).Returns(
                new BuildModel
                {
                    Id = 123,
                    Status = "SUCCESS",
                    TestOccurrences = new TestOccurrences
                    {
                        Count = 13,
                        Href = "/app/rest/testOccurrences?locator=build:(id:123)",
                        Passed = 1,
                        NewFailed = 2,
                        Failed = 3,
                        Muted = 4,
                        Ignored = 5,
                    }
                });

            var connectedTc = new RemoteTc().Connect(_ => _.AsGuest(), teamCityCaller);

            // Act
            var build = connectedTc.GetBuild(123);

            // Assert
            build.TestOccurrences.Should().NotBeNull();
            build.TestOccurrences.Count.Should().Be(13);
            build.TestOccurrences.Href.Should().Be("/app/rest/testOccurrences?locator=build:(id:123)");
            build.TestOccurrences.Passed.Should().Be(1);
            build.TestOccurrences.NewFailed.Should().Be(2);
            build.TestOccurrences.Failed.Should().Be(3);
            build.TestOccurrences.Muted.Should().Be(4);
            build.TestOccurrences.Ignored.Should().Be(5);
        }

        [Test]
        public void GetBuilds_BuildConfigurationName()
        {
            // Arrange
            var teamCityCaller = CreateTeamCityCaller();
            var build = new BuildModel {Id = 987, Status = "SUCCESS"};

            A.CallTo(() => teamCityCaller.Get<BuildWrapper>("/app/rest/builds?locator=buildType:name:FluentTc,&fields=count,build(buildTypeId,href,id,number,state,status,webUrl)"))
                .Returns(new BuildWrapper {Count = "1", Build = new List<BuildModel>(new[] {build})});

            var connectedTc = new RemoteTc().Connect(_ => _.AsGuest(), teamCityCaller);

            // Act
            var builds = connectedTc.GetBuilds(_ => _.BuildConfiguration(__ => __.Name("FluentTc")));

            // Assert
            builds.Single().Id.Should().Be(987);
        }

        public void GetBuilds_SinceDate()
        {
            // Arrange
            var teamCityCaller = CreateTeamCityCaller();
            var build = new BuildModel {Id = 987};

            A.CallTo(
                () =>
                    teamCityCaller.Get<BuildWrapper>(
                        "/app/rest/builds?locator=sinceDate:20151026T142200%2b0000,&fields=count,build(buildTypeId,href,id,number,state,status,webUrl)"))
                .Returns(new BuildWrapper {Count = "1", Build = new List<BuildModel>(new[] {build})});

            var connectedTc = new RemoteTc().Connect(_ => _.AsGuest(), teamCityCaller);

            // Act
            var builds = connectedTc.GetBuilds(_ => _.SinceDate(new DateTime(2015,10,26,16,22,0)));

            // Assert
            builds.ShouldAllBeEquivalentTo(new [] { build });
        }

        [Test]
        public void SetBuildConfigurationParameters_GivenParameterWithoutRawType_ConfigurationName()
        {
            // Arrange
            var teamCityCaller = A.Fake<TeamCityCaller>();

            var connectedTc = new RemoteTc().Connect(_ => _.AsGuest(), teamCityCaller);

            // Act
            connectedTc.SetBuildConfigurationParameters(_ => _.Name("FluentTc"), p=>p.Parameter("name","newVal"));

            // Assert
            A.CallTo(
                () =>
                    teamCityCaller.PutFormat("{\"name\":\"name\",\"value\":\"newVal\",\"type\":null}",
                    HttpContentTypes.ApplicationJson, "/app/rest/buildTypes/{0}/parameters/{1}", A<object[]>.That.IsSameSequenceAs(new[] {"name:FluentTc", "name"})))
                        .MustHaveHappened(Repeated.Exactly.Once);
        }

        [Test]
        public void SetBuildConfigurationParameters_GivenParameterWithRawType_ConfigurationName()
        {
            // Arrange
            var teamCityCaller = A.Fake<TeamCityCaller>();

            var connectedTc = new RemoteTc().Connect(_ => _.AsGuest(), teamCityCaller);

            // Act
            connectedTc.SetBuildConfigurationParameters(_ => _.Name("FluentTc"), 
                p => p.Parameter("name", "newVal", t => t.AsSelectList(s => s.Value("lol"))));

            // Assert
            A.CallTo(
                () =>
                    teamCityCaller.PutFormat("{\"name\":\"name\",\"value\":\"newVal\",\"type\":{\"rawValue\":\"select data_1='lol' display='normal'\"}}",
                    HttpContentTypes.ApplicationJson, "/app/rest/buildTypes/{0}/parameters/{1}", A<object[]>.That.IsSameSequenceAs(new[] { "name:FluentTc", "name" })))
                        .MustHaveHappened(Repeated.Exactly.Once);
        }

        [Test]
        public void DeleteBuildConfigurationParameter_ConfigurationName()
        {
            // Arrange
            var teamCityCaller = CreateTeamCityCaller();

            var connectedTc = new RemoteTc().Connect(_ => _.AsGuest(), teamCityCaller);

            // Act
            connectedTc.DeleteBuildConfigurationParameter(_ => _.Name("FluentTc"), p => p.ParameterName("paramName"));

            // Assert
            A.CallTo(
                () =>
                    teamCityCaller.Delete("/app/rest/buildTypes/name:FluentTc/parameters/paramName"))
                        .MustHaveHappened(Repeated.Exactly.Once);
        }

        [Test]
        public void RunBuildConfiguration_WithComment()
        {
            // Arrange
            Action<IBuildConfigurationHavingBuilder> having = _ => _.Name("FluentTc");
            var teamCityCaller = CreateTeamCityCaller();
            var buildConfigurationRetriever = A.Fake<IBuildConfigurationRetriever>();

            A.CallTo(() => buildConfigurationRetriever.GetSingleBuildConfiguration(having))
                .Returns(new BuildConfiguration { Id = "bt2" });
            A.CallTo(() =>
                teamCityCaller.PostFormat<BuildModel>(
                    "<build>\r\n<buildType id=\"bt2\"/>\r\n<comment><text>comment!</text></comment>\r\n</build>\r\n",
                    HttpContentTypes.ApplicationXml, HttpContentTypes.ApplicationJson, "/app/rest/buildQueue"))
                .Returns(new BuildModel {Id = 123, Status = "SUCCESS"});

            var connectedTc = new RemoteTc().Connect(_ => _.AsGuest(), teamCityCaller, buildConfigurationRetriever);

            // Act
            var build = connectedTc.RunBuildConfiguration(having, options => options.WithComment("comment!"));

            // Assert
            A.CallTo(() =>
                teamCityCaller.PostFormat<BuildModel>(
                    "<build>\r\n<buildType id=\"bt2\"/>\r\n<comment><text>comment!</text></comment>\r\n</build>\r\n",
                    HttpContentTypes.ApplicationXml, HttpContentTypes.ApplicationJson, "/app/rest/buildQueue"))
                .MustHaveHappened(Repeated.Exactly.Once);
            build.Id.ShouldBeEquivalentTo(123);
            build.Status.ShouldBeEquivalentTo(BuildStatus.Success);
        }
        
        [Test]
        public void RunBuildConfiguration_WithComment_SpecialCharacters()
        {
            // Arrange
            Action<IBuildConfigurationHavingBuilder> having = _ => _.Name("FluentTc");
            var teamCityCaller = CreateTeamCityCaller();
            var buildConfigurationRetriever = A.Fake<IBuildConfigurationRetriever>();

            A.CallTo(() => buildConfigurationRetriever.GetSingleBuildConfiguration(having))
                .Returns(new BuildConfiguration { Id = "bt2" });
            A.CallTo(() =>
                teamCityCaller.PostFormat<BuildModel>(
                    "<build>\r\n<buildType id=\"bt2\"/>\r\n<comment><text>comment&lt;HAHA&gt;</text></comment>\r\n</build>\r\n",
                    HttpContentTypes.ApplicationXml, HttpContentTypes.ApplicationJson, "/app/rest/buildQueue"))
                .Returns(new BuildModel {Id = 123, Status = "SUCCESS"});

            var connectedTc = new RemoteTc().Connect(_ => _.AsGuest(), teamCityCaller, buildConfigurationRetriever);

            // Act
            var build = connectedTc.RunBuildConfiguration(having, options => options.WithComment("comment<HAHA>"));

            // Assert
            A.CallTo(() =>
                teamCityCaller.PostFormat<BuildModel>(
                    "<build>\r\n<buildType id=\"bt2\"/>\r\n<comment><text>comment&lt;HAHA&gt;</text></comment>\r\n</build>\r\n",
                    HttpContentTypes.ApplicationXml, HttpContentTypes.ApplicationJson, "/app/rest/buildQueue"))
                .MustHaveHappened(Repeated.Exactly.Once);
            build.Id.ShouldBeEquivalentTo(123);
            build.Status.ShouldBeEquivalentTo(BuildStatus.Success);
        }

        [Test]
        public void RunBuildConfiguration_WithCommentAndPersonal()
        {
            // Arrange
            Action<IBuildConfigurationHavingBuilder> having = _ => _.Name("FluentTc");
            var teamCityCaller = CreateTeamCityCaller();
            var buildConfigurationRetriever = A.Fake<IBuildConfigurationRetriever>();

            A.CallTo(() => buildConfigurationRetriever.GetSingleBuildConfiguration(having))
                .Returns(new BuildConfiguration { Id = "bt2" });
            A.CallTo(() =>
                teamCityCaller.PostFormat<BuildModel>(
                    "<build personal=\"true\">\r\n<buildType id=\"bt2\"/>\r\n<comment><text>comment!</text></comment>\r\n</build>\r\n",
                    HttpContentTypes.ApplicationXml, HttpContentTypes.ApplicationJson, "/app/rest/buildQueue"))
                .Returns(new BuildModel {Id = 123, Status = "SUCCESS"});

            var connectedTc = new RemoteTc().Connect(_ => _.AsGuest(), teamCityCaller, buildConfigurationRetriever);

            // Act
            var build = connectedTc.RunBuildConfiguration(having, options => options.WithComment("comment!").AsPersonal());

            // Assert
            A.CallTo(() =>
                teamCityCaller.PostFormat<BuildModel>(
                    "<build personal=\"true\">\r\n<buildType id=\"bt2\"/>\r\n<comment><text>comment!</text></comment>\r\n</build>\r\n",
                    HttpContentTypes.ApplicationXml, HttpContentTypes.ApplicationJson, "/app/rest/buildQueue"))
                .MustHaveHappened(Repeated.Exactly.Once);
            build.Id.ShouldBeEquivalentTo(123);
            build.Status.ShouldBeEquivalentTo(BuildStatus.Success);
        }

        [Test]
        public void RunBuildConfiguration_WithCommentAndPersonalAndCleanSources()
        {
            // Arrange
            Action<IBuildConfigurationHavingBuilder> having = _ => _.Name("FluentTc");
            var teamCityCaller = CreateTeamCityCaller();
            var buildConfigurationRetriever = A.Fake<IBuildConfigurationRetriever>();

            A.CallTo(() => buildConfigurationRetriever.GetSingleBuildConfiguration(having))
                .Returns(new BuildConfiguration { Id = "bt2" });
            A.CallTo(() =>
                teamCityCaller.PostFormat<BuildModel>(
                    "<build personal=\"true\">\r\n<buildType id=\"bt2\"/>\r\n<comment><text>comment!</text></comment>\r\n<triggeringOptions cleanSources=\"true\" />\r\n</build>\r\n",
                    HttpContentTypes.ApplicationXml, HttpContentTypes.ApplicationJson, "/app/rest/buildQueue"))
                .Returns(new BuildModel {Id = 123, Status = "SUCCESS"});

            var connectedTc = new RemoteTc().Connect(_ => _.AsGuest(), teamCityCaller, buildConfigurationRetriever);

            // Act
            var build = connectedTc.RunBuildConfiguration(having, options => options.WithComment("comment!").AsPersonal().WithCleanSources());

            // Assert
            A.CallTo(() =>
                teamCityCaller.PostFormat<BuildModel>(
                    "<build personal=\"true\">\r\n<buildType id=\"bt2\"/>\r\n<comment><text>comment!</text></comment>\r\n<triggeringOptions cleanSources=\"true\" />\r\n</build>\r\n",
                    HttpContentTypes.ApplicationXml, HttpContentTypes.ApplicationJson, "/app/rest/buildQueue"))
                .MustHaveHappened(Repeated.Exactly.Once);
            build.Id.ShouldBeEquivalentTo(123);
            build.Status.ShouldBeEquivalentTo(BuildStatus.Success);
        }

        [Test]
        public void RunBuildConfiguration_Personal_CleanSources_QueueTop()
        {
            // Arrange
            Action<IBuildConfigurationHavingBuilder> having = _ => _.Name("FluentTc");
            var teamCityCaller = CreateTeamCityCaller();
            var buildConfigurationRetriever = A.Fake<IBuildConfigurationRetriever>();

            A.CallTo(() => buildConfigurationRetriever.GetSingleBuildConfiguration(having))
                .Returns(new BuildConfiguration { Id = "bt2" });
            A.CallTo(() =>
                teamCityCaller.PostFormat<BuildModel>(
                    "<build>\r\n<buildType id=\"bt2\"/>\r\n<triggeringOptions cleanSources=\"true\" queueAtTop=\"true\" />\r\n</build>\r\n",
                    HttpContentTypes.ApplicationXml, HttpContentTypes.ApplicationJson, "/app/rest/buildQueue"))
                .Returns(new BuildModel {Id = 123, Status = "SUCCESS"});

            var connectedTc = new RemoteTc().Connect(_ => _.AsGuest(), teamCityCaller, buildConfigurationRetriever);

            // Act
            var build = connectedTc.RunBuildConfiguration(having, options => options.WithCleanSources().QueueAtTop());

            // Assert
            A.CallTo(() =>
                teamCityCaller.PostFormat<BuildModel>(
                    "<build>\r\n<buildType id=\"bt2\"/>\r\n<triggeringOptions cleanSources=\"true\" queueAtTop=\"true\" />\r\n</build>\r\n",
                    HttpContentTypes.ApplicationXml, HttpContentTypes.ApplicationJson, "/app/rest/buildQueue"))
                .MustHaveHappened(Repeated.Exactly.Once);
            build.Id.ShouldBeEquivalentTo(123);
            build.Status.ShouldBeEquivalentTo(BuildStatus.Success);
        }

        [Test]
        [TestCase(1234567891023)]
        public void RunBuildConfiguration_OnChange(long changeId)
        {
            // Arrange
            Action<IBuildConfigurationHavingBuilder> having = _ => _.Name("FluentTc");
            var teamCityCaller = CreateTeamCityCaller();
            var buildConfigurationRetriever = A.Fake<IBuildConfigurationRetriever>();

            A.CallTo(() => buildConfigurationRetriever.GetSingleBuildConfiguration(having))
                .Returns(new BuildConfiguration { Id = "bt2" });
            A.CallTo(
                () =>
                teamCityCaller.PostFormat<BuildModel>(
                    string.Format(
                        "<build>\r\n<buildType id=\"bt2\"/>\r\n<lastChanges>\r\n<change id=\"{0}\"/>\r\n</lastChanges>\r\n</build>\r\n",
                        changeId),
                    HttpContentTypes.ApplicationXml,
                    HttpContentTypes.ApplicationJson,
                    "/app/rest/buildQueue"))
                .Returns(new BuildModel { Id = 123, Status = "SUCCESS" });

            var connectedTc = new RemoteTc().Connect(_ => _.AsGuest(), teamCityCaller, buildConfigurationRetriever);

            // Act
            var build = connectedTc.RunBuildConfiguration(having, options => options.OnChange(change => change.Id(changeId)));

            // Assert
            A.CallTo(
                () =>
                teamCityCaller.PostFormat<BuildModel>(
                    string.Format(
                        "<build>\r\n<buildType id=\"bt2\"/>\r\n<lastChanges>\r\n<change id=\"{0}\"/>\r\n</lastChanges>\r\n</build>\r\n",
                        changeId),
                    HttpContentTypes.ApplicationXml,
                    HttpContentTypes.ApplicationJson,
                    "/app/rest/buildQueue"))
                .MustHaveHappened(Repeated.Exactly.Once);
            build.Id.ShouldBeEquivalentTo(123);
            build.Status.ShouldBeEquivalentTo(BuildStatus.Success);
        }

        [Test]
        public void RunBuildConfiguration()
        {
            // Arrange
            Action<IBuildConfigurationHavingBuilder> having = _ => _.Name("FluentTc");
            var teamCityCaller = CreateTeamCityCaller();
            var buildConfigurationRetriever = A.Fake<IBuildConfigurationRetriever>();

            A.CallTo(() => buildConfigurationRetriever.GetSingleBuildConfiguration(having))
                .Returns(new BuildConfiguration { Id = "bt2" });
            A.CallTo(() =>
                teamCityCaller.PostFormat<BuildModel>(
                    "<build>\r\n<buildType id=\"bt2\"/>\r\n</build>\r\n",
                    HttpContentTypes.ApplicationXml, HttpContentTypes.ApplicationJson, "/app/rest/buildQueue"))
                .Returns(new BuildModel {Id = 123, Status = "SUCCESS"});

            var connectedTc = new RemoteTc().Connect(_ => _.AsGuest(), teamCityCaller, buildConfigurationRetriever);

            // Act
            var build = connectedTc.RunBuildConfiguration(having);

            // Assert
            A.CallTo(() =>
                teamCityCaller.PostFormat<BuildModel>(
                    "<build>\r\n<buildType id=\"bt2\"/>\r\n</build>\r\n",
                    HttpContentTypes.ApplicationXml, HttpContentTypes.ApplicationJson, A<string>.Ignored, A<object[]>.Ignored))
                .MustHaveHappened(Repeated.Exactly.Once);
            build.Id.ShouldBeEquivalentTo(123);
            build.Status.ShouldBeEquivalentTo(BuildStatus.Success);
        }

        [Test]
        public void RunBuildConfiguration_ConfigurationName()
        {
            // Arrange
            Action<IBuildConfigurationHavingBuilder> having = _ => _.Name("FluentTc");
            var teamCityCaller = CreateTeamCityCaller();
            var buildConfigurationRetriever = A.Fake<IBuildConfigurationRetriever>();
            A.CallTo(() => buildConfigurationRetriever.GetSingleBuildConfiguration(having))
                .Returns(new BuildConfiguration {Id = "bt2"});
            A.CallTo(() =>
                teamCityCaller.PostFormat<BuildModel>(
                    "<build>\r\n<buildType id=\"bt2\"/>\r\n</build>\r\n",
                    HttpContentTypes.ApplicationXml, HttpContentTypes.ApplicationJson, "/app/rest/buildQueue"))
                .Returns(new BuildModel {Id = 123, Status = "SUCCESS"});

            var connectedTc = new RemoteTc().Connect(_ => _.AsGuest(), teamCityCaller, buildConfigurationRetriever);

            // Act
            var build = connectedTc.RunBuildConfiguration(having);

            // Assert
            A.CallTo(() =>
                teamCityCaller.PostFormat<BuildModel>(
                    "<build>\r\n<buildType id=\"bt2\"/>\r\n</build>\r\n",
                    HttpContentTypes.ApplicationXml, HttpContentTypes.ApplicationJson, "/app/rest/buildQueue", A<object[]>.Ignored))
                .MustHaveHappened(Repeated.Exactly.Once);
            build.Id.ShouldBeEquivalentTo(123);
            build.Status.ShouldBeEquivalentTo(BuildStatus.Success);
        }

        [Test]
        public void RunBuildConfiguration_ConfigurationNameWithParameters()
        {
            // Arrange
            Action<IBuildConfigurationHavingBuilder> having = _ => _.Name("FluentTc");
            var teamCityCaller = CreateTeamCityCaller();
            var buildConfigurationRetriever = A.Fake<IBuildConfigurationRetriever>();
            A.CallTo(() => buildConfigurationRetriever.GetSingleBuildConfiguration(having))
                .Returns(new BuildConfiguration {Id = "bt2"});
            A.CallTo(() =>
                teamCityCaller.PostFormat<BuildModel>(
                    "<build>\r\n<buildType id=\"bt2\"/>\r\n<properties>\r\n<property name=\"param1\" value=\"value1\"/>\r\n</properties>\r\n</build>\r\n",
                    HttpContentTypes.ApplicationXml, HttpContentTypes.ApplicationJson, "/app/rest/buildQueue"))
                .Returns(new BuildModel {Id = 123, Status = "SUCCESS"});

            var connectedTc = new RemoteTc().Connect(_ => _.AsGuest(), teamCityCaller, buildConfigurationRetriever);

            // Act
            var build = connectedTc.RunBuildConfiguration(having, p => p.Parameter("param1","value1"));

            // Assert
            A.CallTo(() =>
                teamCityCaller.PostFormat<BuildModel>(
                    "<build>\r\n<buildType id=\"bt2\"/>\r\n<properties>\r\n<property name=\"param1\" value=\"value1\"/>\r\n</properties>\r\n</build>\r\n",
                    HttpContentTypes.ApplicationXml, HttpContentTypes.ApplicationJson, "/app/rest/buildQueue", A<object[]>.Ignored))
                .MustHaveHappened(Repeated.Exactly.Once);
            build.Id.ShouldBeEquivalentTo(123);
            build.Status.ShouldBeEquivalentTo(BuildStatus.Success);
        }

        [Test]
        public void RunBuildConfiguration_ConfigurationNameWithParametersAndOptions()
        {
            // Arrange
            Action<IBuildConfigurationHavingBuilder> having = _ => _.Name("FluentTc");
            var teamCityCaller = CreateTeamCityCaller();
            var buildConfigurationRetriever = A.Fake<IBuildConfigurationRetriever>();
            A.CallTo(() => buildConfigurationRetriever.GetSingleBuildConfiguration(having))
                .Returns(new BuildConfiguration { Id = "bt2" });
            A.CallTo(() =>
                teamCityCaller.PostFormat<BuildModel>(
                    "<build>\r\n<buildType id=\"bt2\"/>\r\n<triggeringOptions queueAtTop=\"true\" />\r\n<properties>\r\n<property name=\"param1\" value=\"value1\"/>\r\n</properties>\r\n</build>\r\n",
                    HttpContentTypes.ApplicationXml, HttpContentTypes.ApplicationJson, "/app/rest/buildQueue"))
                .Returns(new BuildModel { Id = 123, Status = "SUCCESS" });

            var connectedTc = new RemoteTc().Connect(_ => _.AsGuest(), teamCityCaller, buildConfigurationRetriever);

            // Act
            var build = connectedTc.RunBuildConfiguration(having, p => p.Parameter("param1", "value1"), o => o.QueueAtTop());

            // Assert
            A.CallTo(() =>
                teamCityCaller.PostFormat<BuildModel>(
                    "<build>\r\n<buildType id=\"bt2\"/>\r\n<triggeringOptions queueAtTop=\"true\" />\r\n<properties>\r\n<property name=\"param1\" value=\"value1\"/>\r\n</properties>\r\n</build>\r\n",
                    HttpContentTypes.ApplicationXml, HttpContentTypes.ApplicationJson, "/app/rest/buildQueue", A<object[]>.Ignored))
                .MustHaveHappened(Repeated.Exactly.Once);
            build.Id.ShouldBeEquivalentTo(123);
            build.Status.ShouldBeEquivalentTo(BuildStatus.Success);
        }

        [Test]
        public void RunBuildConfiguration_OnAgentName()
        {
            // Arrange
            Action<IBuildConfigurationHavingBuilder> having = _ => _.Name("FluentTc");
            var teamCityCaller = CreateTeamCityCaller();

            var buildConfigurationRetriever = A.Fake<IBuildConfigurationRetriever>();
            A.CallTo(() => buildConfigurationRetriever.GetSingleBuildConfiguration(having))
                .Returns(new BuildConfiguration {Id = "bt2"});
            A.CallTo(() =>
                teamCityCaller.PostFormat<BuildModel>(
                    "<build>\r\n<buildType id=\"bt2\"/>\r\n<agent id=\"9\"/>\r\n</build>\r\n",
                    HttpContentTypes.ApplicationXml, HttpContentTypes.ApplicationJson, "/app/rest/buildQueue"))
                .Returns(new BuildModel {Id = 123, Status = "SUCCESS"});

            Action<IAgentHavingBuilder> onAgent = p => p.Name("agent1");
            var agentsRetriever = A.Fake<IAgentsRetriever>();
            A.CallTo(() => agentsRetriever.GetAgent(onAgent)).Returns(new Agent() {Id = 9});

            var connectedTc = new RemoteTc().Connect(_ => _.AsGuest(), teamCityCaller, buildConfigurationRetriever, agentsRetriever);

            // Act
            var build = connectedTc.RunBuildConfiguration(having, onAgent);

            // Assert
            A.CallTo(() =>
                teamCityCaller.PostFormat<BuildModel>(
                    "<build>\r\n<buildType id=\"bt2\"/>\r\n<agent id=\"9\"/>\r\n</build>\r\n",
                    HttpContentTypes.ApplicationXml, HttpContentTypes.ApplicationJson, "/app/rest/buildQueue", A<object[]>.Ignored))
                .MustHaveHappened(Repeated.Exactly.Once);
            build.Id.ShouldBeEquivalentTo(123);
            build.Status.ShouldBeEquivalentTo(BuildStatus.Success);
        }

        [Test]
        public void RunBuildConfiguration_OnAgentNameWithParameters()
        {
            // Arrange
            Action<IBuildConfigurationHavingBuilder> having = _ => _.Name("FluentTc");
            var teamCityCaller = CreateTeamCityCaller();
            var buildConfigurationRetriever = A.Fake<IBuildConfigurationRetriever>();
            A.CallTo(() => buildConfigurationRetriever.GetSingleBuildConfiguration(having))
                .Returns(new BuildConfiguration {Id = "bt2"});
            A.CallTo(() =>
                teamCityCaller.PostFormat<BuildModel>(
                    "<build>\r\n<buildType id=\"bt2\"/>\r\n<agent id=\"9\"/>\r\n<properties>\r\n<property name=\"param1\" value=\"value1\"/>\r\n</properties>\r\n</build>\r\n",
                    HttpContentTypes.ApplicationXml, HttpContentTypes.ApplicationJson, "/app/rest/buildQueue"))
                .Returns(new BuildModel {Id = 123, Status = "SUCCESS"});

            Action<IAgentHavingBuilder> onAgent = p => p.Name("agent1");
            var agentsRetriever = A.Fake<IAgentsRetriever>();
            A.CallTo(() => agentsRetriever.GetAgent(onAgent)).Returns(new Agent() {Id = 9});

            var connectedTc = new RemoteTc().Connect(_ => _.AsGuest(), teamCityCaller, buildConfigurationRetriever, agentsRetriever);

            // Act
            var build = connectedTc.RunBuildConfiguration(having, onAgent, p => p.Parameter("param1", "value1"));

            // Assert
            A.CallTo(() =>
                teamCityCaller.PostFormat<BuildModel>(
                    "<build>\r\n<buildType id=\"bt2\"/>\r\n<agent id=\"9\"/>\r\n<properties>\r\n<property name=\"param1\" value=\"value1\"/>\r\n</properties>\r\n</build>\r\n",
                    HttpContentTypes.ApplicationXml, HttpContentTypes.ApplicationJson, "/app/rest/buildQueue", A<object[]>.Ignored))
                .MustHaveHappened(Repeated.Exactly.Once);
            build.Id.ShouldBeEquivalentTo(123);
            build.Status.ShouldBeEquivalentTo(BuildStatus.Success);
        }

        [Test]
        public void RunBuildConfiguration_OnAgentNameWithParametersBranchName()
        {
            // Arrange
            Action<IBuildConfigurationHavingBuilder> having = _ => _.Name("FluentTc");
            var teamCityCaller = CreateTeamCityCaller();
            var buildConfigurationRetriever = A.Fake<IBuildConfigurationRetriever>();

            A.CallTo(() => buildConfigurationRetriever.GetSingleBuildConfiguration(having))
                .Returns(new BuildConfiguration { Id = "bt2" });
            A.CallTo(() =>
                teamCityCaller.PostFormat<BuildModel>(
                    "<build branchName=\"develop\">\r\n<buildType id=\"bt2\"/>\r\n<agent id=\"9\"/>\r\n<properties>\r\n<property name=\"param1\" value=\"value1\"/>\r\n</properties>\r\n</build>\r\n",
                    HttpContentTypes.ApplicationXml, HttpContentTypes.ApplicationJson, "/app/rest/buildQueue"))
                .Returns(new BuildModel { Id = 123, Status = "SUCCESS" });

            Action<IAgentHavingBuilder> onAgent = p => p.Name("agent1");
            var agentsRetriever = A.Fake<IAgentsRetriever>();
            A.CallTo(() => agentsRetriever.GetAgent(onAgent)).Returns(new Agent() { Id = 9 });

            var connectedTc = new RemoteTc().Connect(_ => _.AsGuest(), teamCityCaller, buildConfigurationRetriever, agentsRetriever);

            // Act
            var build = connectedTc.RunBuildConfiguration(having, onAgent, p => p.Parameter("param1", "value1"), o => o.OnBranch("develop"));

            // Assert
            A.CallTo(() =>
                teamCityCaller.PostFormat<BuildModel>(
                    "<build branchName=\"develop\">\r\n<buildType id=\"bt2\"/>\r\n<agent id=\"9\"/>\r\n<properties>\r\n<property name=\"param1\" value=\"value1\"/>\r\n</properties>\r\n</build>\r\n",
                    HttpContentTypes.ApplicationXml, HttpContentTypes.ApplicationJson, "/app/rest/buildQueue", A<object[]>.Ignored))
                .MustHaveHappened(Repeated.Exactly.Once);
            build.Id.ShouldBeEquivalentTo(123);
            build.Status.ShouldBeEquivalentTo(BuildStatus.Success);
        }

        [Test]
        public void RunBuildConfiguration_BranchName()
        {
            // Arrange
            Action<IBuildConfigurationHavingBuilder> having = _ => _.Name("FluentTc");
            var teamCityCaller = CreateTeamCityCaller();
            var buildConfigurationRetriever = A.Fake<IBuildConfigurationRetriever>();

            A.CallTo(() => buildConfigurationRetriever.GetSingleBuildConfiguration(having))
                .Returns(new BuildConfiguration { Id = "bt2" });
            A.CallTo(() =>
                teamCityCaller.PostFormat<BuildModel>(
                    "<build branchName=\"develop\">\r\n<buildType id=\"bt2\"/>\r\n</build>\r\n",
                    HttpContentTypes.ApplicationXml, HttpContentTypes.ApplicationJson, "/app/rest/buildQueue"))
                .Returns(new BuildModel { Id = 123, Status = "SUCCESS" });

            var connectedTc = new RemoteTc().Connect(_ => _.AsGuest(), teamCityCaller, buildConfigurationRetriever);

            // Act
            var build = connectedTc.RunBuildConfiguration(having, o => o.OnBranch("develop"));

            // Assert
            A.CallTo(() =>
                teamCityCaller.PostFormat<BuildModel>(
                    "<build branchName=\"develop\">\r\n<buildType id=\"bt2\"/>\r\n</build>\r\n",
                    HttpContentTypes.ApplicationXml, HttpContentTypes.ApplicationJson, A<string>.Ignored, A<object[]>.Ignored))
                .MustHaveHappened(Repeated.Exactly.Once);
            build.Id.ShouldBeEquivalentTo(123);
            build.Status.ShouldBeEquivalentTo(BuildStatus.Success);
        }

        [Test]
        public void RunBuildConfiguration_BranchNameAndPersonal()
        {
            // Arrange
            Action<IBuildConfigurationHavingBuilder> having = _ => _.Name("FluentTc");
            var teamCityCaller = CreateTeamCityCaller();
            var buildConfigurationRetriever = A.Fake<IBuildConfigurationRetriever>();

            A.CallTo(() => buildConfigurationRetriever.GetSingleBuildConfiguration(having))
                .Returns(new BuildConfiguration { Id = "bt2" });
            A.CallTo(() =>
                teamCityCaller.PostFormat<BuildModel>(
                    "<build personal=\"true\" branchName=\"develop\">\r\n<buildType id=\"bt2\"/>\r\n</build>\r\n",
                    HttpContentTypes.ApplicationXml, HttpContentTypes.ApplicationJson, "/app/rest/buildQueue"))
                .Returns(new BuildModel { Id = 123, Status = "SUCCESS" });

            var connectedTc = new RemoteTc().Connect(_ => _.AsGuest(), teamCityCaller, buildConfigurationRetriever);

            // Act
            var build = connectedTc.RunBuildConfiguration(having, o => o.OnBranch("develop").AsPersonal());

            // Assert
            A.CallTo(() =>
                teamCityCaller.PostFormat<BuildModel>(
                    "<build personal=\"true\" branchName=\"develop\">\r\n<buildType id=\"bt2\"/>\r\n</build>\r\n",
                    HttpContentTypes.ApplicationXml, HttpContentTypes.ApplicationJson, A<string>.Ignored, A<object[]>.Ignored))
                .MustHaveHappened(Repeated.Exactly.Once);
            build.Id.ShouldBeEquivalentTo(123);
            build.Status.ShouldBeEquivalentTo(BuildStatus.Success);
        }

        [Test]
        public void RunBuildConfiguration_BranchNameWithCharactersRequiringEscaping()
        {
            // Arrange
            Action<IBuildConfigurationHavingBuilder> having = _ => _.Name("FluentTc");
            var teamCityCaller = CreateTeamCityCaller();
            var buildConfigurationRetriever = A.Fake<IBuildConfigurationRetriever>();

            A.CallTo(() => buildConfigurationRetriever.GetSingleBuildConfiguration(having))
                .Returns(new BuildConfiguration { Id = "bt2" });
            A.CallTo(() =>
                teamCityCaller.PostFormat<BuildModel>(
                    "<build branchName=\"r&amp;d\">\r\n<buildType id=\"bt2\"/>\r\n</build>\r\n",
                    HttpContentTypes.ApplicationXml, HttpContentTypes.ApplicationJson, "/app/rest/buildQueue"))
                .Returns(new BuildModel { Id = 123, Status = "SUCCESS" });

            var connectedTc = new RemoteTc().Connect(_ => _.AsGuest(), teamCityCaller, buildConfigurationRetriever);

            // Act
            var build = connectedTc.RunBuildConfiguration(having, o => o.OnBranch("r&d"));

            // Assert
            A.CallTo(() =>
                teamCityCaller.PostFormat<BuildModel>(
                    "<build branchName=\"r&amp;d\">\r\n<buildType id=\"bt2\"/>\r\n</build>\r\n",
                    HttpContentTypes.ApplicationXml, HttpContentTypes.ApplicationJson, A<string>.Ignored, A<object[]>.Ignored))
                .MustHaveHappened(Repeated.Exactly.Once);
            build.Id.ShouldBeEquivalentTo(123);
            build.Status.ShouldBeEquivalentTo(BuildStatus.Success);
        }

        [Test]
        [TestCase("param1", "value1", "release", 1234567891023)]
        public void RunBuildConfiguration_WithParametersBranchNameOnChange(string paramName, string paramValue, string branchName, long changeId)
        {
            // Arrange
            Action<IBuildConfigurationHavingBuilder> having = _ => _.Name("FluentTc");
            var teamCityCaller = CreateTeamCityCaller();
            var buildConfigurationRetriever = A.Fake<IBuildConfigurationRetriever>();

            A.CallTo(() => buildConfigurationRetriever.GetSingleBuildConfiguration(having))
                .Returns(new BuildConfiguration { Id = "bt2" });
            A.CallTo(
                () =>
                teamCityCaller.PostFormat<BuildModel>(
                    string.Format(
                        "<build branchName=\"{0}\">\r\n<buildType id=\"bt2\"/>\r\n<properties>\r\n<property name=\"{1}\" value=\"{2}\"/>\r\n</properties>\r\n<lastChanges>\r\n<change id=\"{3}\"/>\r\n</lastChanges>\r\n</build>\r\n",
                        branchName,
                        paramName,
                        paramValue,
                        changeId),
                    HttpContentTypes.ApplicationXml,
                    HttpContentTypes.ApplicationJson,
                    "/app/rest/buildQueue")).Returns(new BuildModel { Id = 123, Status = "SUCCESS" });

            var connectedTc = new RemoteTc().Connect(_ => _.AsGuest(), teamCityCaller, buildConfigurationRetriever);

            // Act
            var build = connectedTc.RunBuildConfiguration(
                having,
                parameters => parameters.Parameter(paramName, paramValue),
                options => options.OnBranch(branchName).OnChange(change => change.Id(changeId)));

            // Assert
            A.CallTo(
                () =>
                teamCityCaller.PostFormat<BuildModel>(
                    string.Format(
                        "<build branchName=\"{0}\">\r\n<buildType id=\"bt2\"/>\r\n<properties>\r\n<property name=\"{1}\" value=\"{2}\"/>\r\n</properties>\r\n<lastChanges>\r\n<change id=\"{3}\"/>\r\n</lastChanges>\r\n</build>\r\n",
                        branchName,
                        paramName,
                        paramValue,
                        changeId),
                    HttpContentTypes.ApplicationXml,
                    HttpContentTypes.ApplicationJson,
                    "/app/rest/buildQueue",
                    A<object[]>.Ignored))
                .MustHaveHappened(Repeated.Exactly.Once);
            build.Id.ShouldBeEquivalentTo(123);
            build.Status.ShouldBeEquivalentTo(BuildStatus.Success);
        }

        [Test]
        [TestCase("agent1", "param1", "value1", "release", 1234567891023)]
        public void RunBuildConfiguration_OnAgentNameWithParametersBranchNameOnChange(string agentName, string paramName, string paramValue, string branchName, long changeId)
        {
            // Arrange
            Action<IBuildConfigurationHavingBuilder> having = _ => _.Name("FluentTc");
            var teamCityCaller = CreateTeamCityCaller();
            var buildConfigurationRetriever = A.Fake<IBuildConfigurationRetriever>();

            const int AgentId = 46;
            Action<IAgentHavingBuilder> onAgent = p => p.Name(agentName);
            var agentsRetriever = A.Fake<IAgentsRetriever>();
            A.CallTo(() => agentsRetriever.GetAgent(onAgent)).Returns(new Agent { Id = AgentId });

            A.CallTo(() => buildConfigurationRetriever.GetSingleBuildConfiguration(having))
                .Returns(new BuildConfiguration { Id = "bt2" });
            A.CallTo(
                () =>
                teamCityCaller.PostFormat<BuildModel>(
                    string.Format(
                        "<build branchName=\"{0}\">\r\n<buildType id=\"bt2\"/>\r\n<agent id=\"{1}\"/>\r\n<properties>\r\n<property name=\"{2}\" value=\"{3}\"/>\r\n</properties>\r\n<lastChanges>\r\n<change id=\"{4}\"/>\r\n</lastChanges>\r\n</build>\r\n",
                        branchName,
                        AgentId,
                        paramName,
                        paramValue,
                        changeId),
                    HttpContentTypes.ApplicationXml,
                    HttpContentTypes.ApplicationJson,
                    "/app/rest/buildQueue")).Returns(new BuildModel { Id = 123, Status = "SUCCESS" });
            
            var connectedTc = new RemoteTc().Connect(_ => _.AsGuest(), teamCityCaller, buildConfigurationRetriever, agentsRetriever);

            // Act
            var build = connectedTc.RunBuildConfiguration(
                having,
                onAgent,
                parameters => parameters.Parameter(paramName, paramValue),
                options => options.OnBranch(branchName).OnChange(change => change.Id(changeId)));

            // Assert
            A.CallTo(
                () =>
                teamCityCaller.PostFormat<BuildModel>(
                    string.Format(
                        "<build branchName=\"{0}\">\r\n<buildType id=\"bt2\"/>\r\n<agent id=\"{1}\"/>\r\n<properties>\r\n<property name=\"{2}\" value=\"{3}\"/>\r\n</properties>\r\n<lastChanges>\r\n<change id=\"{4}\"/>\r\n</lastChanges>\r\n</build>\r\n",
                        branchName,
                        AgentId,
                        paramName,
                        paramValue,
                        changeId),
                    HttpContentTypes.ApplicationXml,
                    HttpContentTypes.ApplicationJson,
                    "/app/rest/buildQueue",
                    A<object[]>.Ignored))
                .MustHaveHappened(Repeated.Exactly.Once);
            build.Id.ShouldBeEquivalentTo(123);
            build.Status.ShouldBeEquivalentTo(BuildStatus.Success);
        }

        [Test]
        public void RunBuildConfiguration_BuildResponse()
        {
            // Arrange
            Action<IBuildConfigurationHavingBuilder> having = _ => _.Name("FluentTc");
            var teamCityCaller = CreateTeamCityCaller();
            var buildConfigurationRetriever = A.Fake<IBuildConfigurationRetriever>();

            A.CallTo(() => buildConfigurationRetriever.GetSingleBuildConfiguration(having))
                .Returns(new BuildConfiguration { Id = "bt2" });
            A.CallTo(() =>
                teamCityCaller.PostFormat<BuildModel>("<build>\r\n<buildType id=\"bt2\"/>\r\n</build>\r\n",
                    HttpContentTypes.ApplicationXml, HttpContentTypes.ApplicationJson, "/app/rest/buildQueue", A<object[]>.Ignored))
                .Returns(new BuildModel {Id = 123, Status = "SUCCESS"});

            var connectedTc = new RemoteTc().Connect(_ => _.AsGuest(), teamCityCaller, buildConfigurationRetriever);

            // Act
            var build = connectedTc.RunBuildConfiguration(having);

            // Assert
            A.CallTo(() =>
                teamCityCaller.PostFormat<BuildModel>("<build>\r\n<buildType id=\"bt2\"/>\r\n</build>\r\n",
                    HttpContentTypes.ApplicationXml, HttpContentTypes.ApplicationJson, "/app/rest/buildQueue", A<object[]>.Ignored))
                .MustHaveHappened(Repeated.Exactly.Once);
            build.Should().NotBeNull();
            build.Id.ShouldBeEquivalentTo(123);
            build.Status.ShouldBeEquivalentTo(BuildStatus.Success);
        }

        [Test]
        public void RunBuildConfiguration_BuildResponse_WithParameters()
        {
            // Arrange
            Action<IBuildConfigurationHavingBuilder> having = _ => _.Id("FluentTc");
            var teamCityCaller = CreateTeamCityCaller();
            var buildConfigurationRetriever = A.Fake<IBuildConfigurationRetriever>();

            A.CallTo(() => buildConfigurationRetriever.GetSingleBuildConfiguration(having))
                .Returns(new BuildConfiguration { Id = "bt2" });
            A.CallTo(() =>
                teamCityCaller.PostFormat<BuildModel>(
                    "<build>\r\n<buildType id=\"bt2\"/>\r\n<properties>\r\n<property name=\"param1\" value=\"value1\"/>\r\n</properties>\r\n</build>\r\n",
                    HttpContentTypes.ApplicationXml, HttpContentTypes.ApplicationJson, "/app/rest/buildQueue", A<object[]>.Ignored))
                .Returns(new BuildModel {Id = 123, Status = "SUCCESS"});

            var connectedTc = new RemoteTc().Connect(_ => _.AsGuest(), teamCityCaller, buildConfigurationRetriever);

            // Act
            var build = connectedTc.RunBuildConfiguration(having, p => p.Parameter("param1", "value1"));

            // Assert
            build.Should().NotBeNull();
            build.Id.ShouldBeEquivalentTo(123);
            build.Status.ShouldBeEquivalentTo(BuildStatus.Success);
        }

        [Test]
        public void RunBuildConfiguration_BuildResponse_StateQueued()
        {
            // Arrange
            Action<IBuildConfigurationHavingBuilder> having = _ => _.Id("FluentTc");
            var teamCityCaller = CreateTeamCityCaller();
            var buildConfigurationRetriever = A.Fake<IBuildConfigurationRetriever>();

            A.CallTo(() => buildConfigurationRetriever.GetSingleBuildConfiguration(having))
                .Returns(new BuildConfiguration { Id = "bt2" });
            A.CallTo(() =>
                teamCityCaller.PostFormat<BuildModel>(
                    "<build>\r\n<buildType id=\"bt2\"/>\r\n</build>\r\n",
                    HttpContentTypes.ApplicationXml, HttpContentTypes.ApplicationJson, "/app/rest/buildQueue", A<object[]>.Ignored))
                .Returns(new BuildModel { Id = 123, State = "queued" });

            var connectedTc = new RemoteTc().Connect(_ => _.AsGuest(), teamCityCaller, buildConfigurationRetriever);

            // Act
            var build = connectedTc.RunBuildConfiguration(having);

            // Assert
            build.Should().NotBeNull();
            build.State.ShouldBeEquivalentTo(BuildState.Queued);
        }

        [Test]
        public void GetBuildConfigurations_ByName()
        {
            // Arrange
            Action<IBuildConfigurationHavingBuilder> having = _ => _.Name("FluentTc");
            var teamCityCaller = CreateTeamCityCaller();
            A.CallTo(() =>
                    teamCityCaller.Get<BuildTypeWrapper>("/app/rest/buildTypes?locator=name:FluentTc"))
                .Returns(new BuildTypeWrapper { BuildType = new List<BuildConfiguration>(new[] { new BuildConfiguration { Id = "bt987" } }) });

            var connectedTc = new RemoteTc().Connect(_ => _.AsGuest(), teamCityCaller);

            // Act
            var buildConfigurations = connectedTc.GetBuildConfigurations(having);

            // Assert
            buildConfigurations.Single().Id.Should().Be("bt987");
        }

        [Test]
        public void GetBuildConfiguration_Id()
        {
            // Arrange
            var teamCityCaller = CreateTeamCityCaller();
            A.CallTo(() => teamCityCaller.Get<BuildTypeWrapper>("/app/rest/buildTypes?locator=id:bt123"))
                .Returns(new BuildTypeWrapper { BuildType = new List<BuildConfiguration>(new[] { new BuildConfiguration { Id = "bt123" } }) });
            var buildConfigMock = new BuildConfiguration {
                Id = "bt123",
                SnapshotDependencies = new SnapshotDependencies
                {
                    SnapshotDependency = new List<SnapshotDependency>
                    {
                        new SnapshotDependency
                        {
                            Id = "SpanshotDepId",
                            SourceBuildType = new SourceBuildType()
                            {
                                Id = "SnapshotDepBuildConfig",
                                Name = "Snapshot Dep Build Config 123",
                                Href = "/httpAuth/app/rest/buildTypes/id:SnapshotDepBuildConfig",
                                WebUrl = "http://teamcity/viewType.html?buildTypeId=SnapshotDepBuildConfig",
                                ProjectId = "SnapshotDepProj123",
                                ProjectName = "SnapshotDep Project 123"
                            }
                        }
                    }
                },
                ArtifactDependencies = new ArtifactDependencies
                {
                    ArtifactDependency = new List<ArtifactDependency>
                    {
                        new ArtifactDependency()
                        {
                            Id = "ARTIFACT_DEPENDENCY_1",
                            SourceBuildType = new SourceBuildType()
                            {
                                Id = "ArtifactDepBuildConfig123",
                                Name = "Artifact Dep Build Config 123",
                                Href = "/httpAuth/app/rest/buildTypes/id:ArtifactDepBuildConfig123",
                                WebUrl = "http://teamcity/viewType.html?buildTypeId=ArtifactDepBuildConfig123",
                                ProjectId = "ArtifactDepProj123",
                                ProjectName = "ArtifactDep Project 123"
                            }    
                        }
                    }
                }
            };
            A.CallTo(() => teamCityCaller.Get<BuildConfiguration>("/app/rest/buildTypes/id:bt123"))
                .Returns(buildConfigMock);

            var connectedTc = new RemoteTc().Connect(_ => _.AsGuest(), teamCityCaller);

            // Act
            var buildConfiguration = connectedTc.GetBuildConfiguration(_ => _.Id("bt123"));

            // Assert
            var snapshotDependency = buildConfiguration.SnapshotDependencies.SnapshotDependency.Single();
            snapshotDependency.Id.Should().Be("SpanshotDepId");
            snapshotDependency.SourceBuildType.Id.Should().Be("SnapshotDepBuildConfig");
            snapshotDependency.SourceBuildType.Name.Should().Be("Snapshot Dep Build Config 123");
            snapshotDependency.SourceBuildType.Href.Should().Be("/httpAuth/app/rest/buildTypes/id:SnapshotDepBuildConfig");
            snapshotDependency.SourceBuildType.WebUrl.Should().Be("http://teamcity/viewType.html?buildTypeId=SnapshotDepBuildConfig");
            snapshotDependency.SourceBuildType.ProjectId.Should().Be("SnapshotDepProj123");
            snapshotDependency.SourceBuildType.ProjectName.Should().Be("SnapshotDep Project 123");

            var artifactDependency = buildConfiguration.ArtifactDependencies.ArtifactDependency.Single();
            artifactDependency.Id.Should().Be("ARTIFACT_DEPENDENCY_1");
            artifactDependency.SourceBuildType.Id.Should().Be("ArtifactDepBuildConfig123");
            artifactDependency.SourceBuildType.Name.Should().Be("Artifact Dep Build Config 123");
            artifactDependency.SourceBuildType.Href.Should().Be("/httpAuth/app/rest/buildTypes/id:ArtifactDepBuildConfig123");
            artifactDependency.SourceBuildType.WebUrl.Should().Be("http://teamcity/viewType.html?buildTypeId=ArtifactDepBuildConfig123");
            artifactDependency.SourceBuildType.ProjectId.Should().Be("ArtifactDepProj123");
            artifactDependency.SourceBuildType.ProjectName.Should().Be("ArtifactDep Project 123");
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

        //[Test]
        //public void CreateBuildProject_ByName()
        //{
        //    // Arrange
        //    var teamCityCaller = CreateTeamCityCaller();

        //    var connectedTc = new RemoteTc().Connect(_ => _.AsGuest(), teamCityCaller);

        //    // Act
        //    Project project = connectedTc.CreateProject("NewProject");

        //    // Assert
        //    A.CallTo(() => teamCityCaller.Post("NewProject", HttpContentTypes.TextPlain, "/app/rest/projects/", HttpContentTypes.ApplicationJson)).MustHaveHappened();
        //}

        [Test]
        public void CreateProject_NameIdParent()
        {
            // Arrange
            var teamCityCaller = CreateTeamCityCaller();

            var connectedTc = new RemoteTc().Connect(_ => _.AsGuest(), teamCityCaller);

            // Act
            Project project = connectedTc.CreateProject(with => with.Name("New Project Name")
                .Id("newProjectId")
                .ParentProject(parent => parent.Id("parentProjectId")));

            var data = @"<newProjectDescription name='New Project Name' id='newProjectId'><parentProject locator='id:parentProjectId'/></newProjectDescription>";

            // Assert
            A.CallTo(() => teamCityCaller.Post(data, HttpContentTypes.ApplicationXml, "/app/rest/projects/", HttpContentTypes.ApplicationJson)).MustHaveHappened();
        }

        [Test]
        public void CreateProject_Name()
        {
            // Arrange
            var teamCityCaller = CreateTeamCityCaller();

            var connectedTc = new RemoteTc().Connect(_ => _.AsGuest(), teamCityCaller);

            // Act
            Project project = connectedTc.CreateProject(with => with.Name("New Project Name"));

            var data = @"<newProjectDescription name='New Project Name'></newProjectDescription>";

            // Assert
            A.CallTo(() => teamCityCaller.Post(data, HttpContentTypes.ApplicationXml, "/app/rest/projects/", HttpContentTypes.ApplicationJson)).MustHaveHappened();
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
        public void GetAllBuildConfigurationTemplates()
        {
            // Arrange
            var teamCityCaller = CreateTeamCityCaller();

            var connectedTc = new RemoteTc().Connect(_ => _.AsGuest(), teamCityCaller);

            // Act
            connectedTc.GetAllBuildConfigurationTemplates();

            // Assert
            A.CallTo(() => teamCityCaller.Get<BuildTypeWrapper>(@"/app/rest/buildTypes?locator=templateFlag:true")).MustHaveHappened();
        }    

        [Test]
        public void GetBuildConfigurationTemplate_ById()
        {
            // Arrange
            var teamCityCaller = CreateTeamCityCaller();
            A.CallTo(
                () =>
                    teamCityCaller.Get<BuildConfiguration>("/app/rest/buildTypes/id:TemplateId"))
                .Returns(new BuildConfiguration { Id = "TemplateId" });

            var connectedTc = new RemoteTc().Connect(_ => _.AsGuest(), teamCityCaller);

            // Act
            connectedTc.GetBuildConfigurationTemplate(_ => _.Id("TemplateId"));

            // Assert
            A.CallTo(() => teamCityCaller.Get<BuildConfiguration>(@"/app/rest/buildTypes/id:TemplateId")).MustHaveHappened();
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
        public void DownloadArtifacts_ByBuildId()
        {
            // Arrange
            var teamCityCaller = CreateTeamCityCaller();

            var connectedTc = new RemoteTc().Connect(_ => _.AsGuest(), teamCityCaller);
            A.CallTo(() => teamCityCaller.Get<BuildWrapper>(@"/app/rest/buildQueue?locator=project:id:Trunk"))
                .Returns(new BuildWrapper {Count = "0"});

            // Act
            connectedTc.DownloadArtifacts(123, @"C:\DownloadArtifacts_ByBuildId");

            // Assert
            A.CallTo(() => teamCityCaller.GetDownloadFormat(A<Action<string>>.Ignored,"/downloadArtifacts.html?buildId={0}", 123)).MustHaveHappened();
        }        
        
        [Test]
        public void DownloadArtifacts_SpecificFileByBuildId()
        {
            // Arrange
            var teamCityCaller = CreateTeamCityCaller();

            var connectedTc = new RemoteTc().Connect(_ => _.AsGuest(), teamCityCaller);
            A.CallTo(() => teamCityCaller.Get<BuildWrapper>(@"/app/rest/buildQueue?locator=project:id:Trunk"))
                .Returns(new BuildWrapper {Count = "0"});

            // Act
            connectedTc.DownloadArtifact(123, @"C:\DownloadArtifacts_ByBuildId", "Logs.zip");

            // Assert
            A.CallTo(() => teamCityCaller.GetDownloadFormat(A<Action<string>>.Ignored, "/app/rest/builds/id:{0}/artifacts/content/{1}", 123, "Logs.zip")).MustHaveHappened();
        }

        private static TeamCityCaller CreateTeamCityCaller()
        {
            var teamCityCaller = A.Fake<TeamCityCaller>();
            A.CallTo(() => teamCityCaller.GetFormat<User>(A<string>._, A<object[]>._)).CallsBaseMethod();
            A.CallTo(() => teamCityCaller.GetFormat<UserWrapper>(A<string>._, A<object[]>._)).CallsBaseMethod();
            A.CallTo(() => teamCityCaller.GetFormat<InvestigationWrapper>(A<string>._, A<object[]>._)).CallsBaseMethod();
            A.CallTo(() => teamCityCaller.GetFormat<BuildModel>(A<string>._, A<object[]>._)).CallsBaseMethod();
            A.CallTo(() => teamCityCaller.GetFormat<BuildConfiguration>(A<string>._, A<object[]>._)).CallsBaseMethod();
            A.CallTo(() => teamCityCaller.GetFormat<ProjectWrapper>(A<string>._, A<object[]>._)).CallsBaseMethod();
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

        [Test]
        public void GetAssignedResponsibilityFromBuildConfiguration()
        {
            // Arrange
            var teamCityCaller = CreateTeamCityCaller();
            var buildConfigurationRetriever = A.Fake<IBuildConfigurationRetriever>();
            A.CallTo(
                () =>
                    buildConfigurationRetriever.GetSingleBuildConfiguration(
                        A<Action<IBuildConfigurationHavingBuilder>>.Ignored))
                .Returns(new BuildConfiguration {Id = "bt2"});

            var connectedTc = new RemoteTc().Connect(_ => _.AsGuest(), teamCityCaller, buildConfigurationRetriever);

            // Act
            Investigation investigation = connectedTc.GetInvestigation(_ => _.Id("bt2"));

            // Assert
            A.CallTo(() => teamCityCaller.Get<InvestigationWrapper>(@"/app/rest/investigations?locator=buildType:(id:bt2)")).MustHaveHappened();
        }

        [Test]
        public void GetAssignedResponsibilityFromTestNameId()
        {
            ///app/rest/investigations?locator=test:(id:-1884830467297296372)

            // Arrange
            var teamCityCaller = CreateTeamCityCaller();

            var connectedTc = new RemoteTc().Connect(_ => _.AsGuest(), teamCityCaller);

            // Act
            Investigation investigation = connectedTc.GetTestinvestigationByTestNameId("reallyLongNumberHere");

            // Assert
            A.CallTo(() => teamCityCaller.Get<InvestigationWrapper>(@"/app/rest/investigations?locator=test:(id:reallyLongNumberHere)")).MustHaveHappened();
        }

        [Test]
        public void GetAllUsers()
        {
            // Arrange
            var teamCityCaller = CreateTeamCityCaller();

            var connectedTc = new RemoteTc().Connect(_ => _.AsGuest(), teamCityCaller);

            // Act
            List<User> users = connectedTc.GetAllUsers();

            // Assert
            A.CallTo(() => teamCityCaller.Get<UserWrapper>(@"/app/rest/users/")).MustHaveHappened();
        }

        [Test]
        public void GetUser_ByUsername()
        {
            // Arrange
            var teamCityCaller = CreateTeamCityCaller();

            var connectedTc = new RemoteTc().Connect(_ => _.AsGuest(), teamCityCaller);

            // Act
            User user = connectedTc.GetUser(_ => _.Username("boris.m"));

            // Assert
            A.CallTo(() => teamCityCaller.Get<User>(@"/app/rest/users/username:boris.m")).MustHaveHappened();
        }

        [Test]
        public void SetProjectParameters_GivenParameterWithoutRawType_ById()
        {
            // Arrange
            var teamCityCaller = CreateTeamCityCaller();

            var connectedTc = new RemoteTc().Connect(_ => _.AsGuest(), teamCityCaller);

            // Act
            connectedTc.SetProjectParameters(_ => _.Id("ProjectId"), __ => __.Parameter("param1", "value1"));

            // Assert
            A.CallTo(
                () =>
                    teamCityCaller.Put("{\"name\":\"param1\",\"value\":\"value1\",\"type\":null}",
                    HttpContentTypes.ApplicationJson, "/app/rest/projects/id:ProjectId/parameters/param1", string.Empty))
                        .MustHaveHappened(Repeated.Exactly.Once);
        }

        [Test]
        public void SetProjectParameters_GivenParameterWithRawType_ById()
        {
            // Arrange
            var teamCityCaller = CreateTeamCityCaller();

            var connectedTc = new RemoteTc().Connect(_ => _.AsGuest(), teamCityCaller);

            // Act
            connectedTc.SetProjectParameters(_ => _.Id("ProjectId"), __ => __.Parameter("param1", "value1", t => t.AsSelectList(s => s.Value("item1"))));

            // Assert
            A.CallTo(
                () =>
                    teamCityCaller.Put("{\"name\":\"param1\",\"value\":\"value1\",\"type\":{\"rawValue\":\"select data_1='item1' display='normal'\"}}",
                    HttpContentTypes.ApplicationJson, "/app/rest/projects/id:ProjectId/parameters/param1", string.Empty))
                        .MustHaveHappened(Repeated.Exactly.Once);
        }

        [Test]
        public void DeleteProjectParameter_ById()
        {
            // Arrange
            var teamCityCaller = CreateTeamCityCaller();

            var connectedTc = new RemoteTc().Connect(_ => _.AsGuest(), teamCityCaller);

            // Act
            connectedTc.DeleteProjectParameter(_ => _.Id("ProjectId"), __ => __.ParameterName("param1"));

            // Assert
            A.CallTo(
                () =>
                    teamCityCaller.Delete("/app/rest/projects/id:ProjectId/parameters/param1"))
                        .MustHaveHappened(Repeated.Exactly.Once);
        }

        [Test]
        public void GetSingleSuccessfulFinishedBuild()
        {
            // Arrange
            var teamCityCaller = CreateTeamCityCaller();
            A.CallTo(
                () =>
                    teamCityCaller.Get<BuildWrapper>(
                        @"/app/rest/builds?locator=buildType:id:FluentTc,running:False,status:SUCCESS,count:1,&fields=count,build(buildTypeId,href,id,number,state,status,webUrl,finishDate,startDate)"))
                .Returns(new BuildWrapper {Build = new List<BuildModel>(new [] {new BuildModel {Id = 123, Status = "SUCCESS"}}),Count = "1"});

            var connectedTc = new RemoteTc().Connect(_ => _.AsGuest(), teamCityCaller);

            // Act
            var builds = connectedTc.GetBuilds(which => which
                .BuildConfiguration(b => b.Id("FluentTc"))
                .NotRunning()
                .Status(BuildStatus.Success), 
                                    with => with.IncludeFinishDate()
                                        .IncludeStartDate(), 
                                    count => count.Count(1));

            // Assert
            builds.Single().Id.Should().Be(123);
        }

        [Test]
        [TestCase("A simple description")]
        [TestCase("")]
        public void SetProjectDescription(string description)
        {
            // Arrange
            var teamCityCaller = CreateTeamCityCaller();

            var connectedTc = new RemoteTc().Connect(_ => _.AsGuest(), teamCityCaller);

            // Act
            connectedTc.SetProjectConfigurationField(_ => _.Id("ProjectId"), __ => __.Description(description));

            // Assert
            A.CallTo(
                () =>
                    teamCityCaller.Put(description,
                    HttpContentTypes.TextPlain, "/app/rest/projects/id:ProjectId/description", string.Empty))
                        .MustHaveHappened(Repeated.Exactly.Once);
        }

        [Test]
        [TestCase("A simple description")]
        [TestCase("")]
        public void SetBuildDescription(string description)
        {
            // Arrange
            var teamCityCaller = CreateTeamCityCaller();

            var connectedTc = new RemoteTc().Connect(_ => _.AsGuest(), teamCityCaller);

            // Act
            connectedTc.SetBuildConfigurationField(_ => _.Id("BuildId"), __ => __.Description(description));

            // Assert
            A.CallTo(
                () =>
                    teamCityCaller.Put(description,
                    HttpContentTypes.TextPlain, "/app/rest/buildTypes/id:BuildId/description", string.Empty))
                        .MustHaveHappened(Repeated.Exactly.Once);
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void SetProjectArchived(bool archived)
        {
            // Arrange
            var teamCityCaller = CreateTeamCityCaller();

            var connectedTc = new RemoteTc().Connect(_ => _.AsGuest(), teamCityCaller);

            // Act
            if (archived)
                connectedTc.SetProjectConfigurationField(_ => _.Id("ProjectId"), __ => __.Archived());
            else
                connectedTc.SetProjectConfigurationField(_ => _.Id("ProjectId"), __ => __.NotArchived());

            // Assert
            A.CallTo(
                () =>
                    teamCityCaller.Put(archived.ToString(System.Globalization.CultureInfo.InvariantCulture).ToLower(),
                    HttpContentTypes.TextPlain, "/app/rest/projects/id:ProjectId/archived", string.Empty))
                        .MustHaveHappened(Repeated.Exactly.Once);
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void SetBuildDescription(bool paused)
        {
            // Arrange
            var teamCityCaller = CreateTeamCityCaller();

            var connectedTc = new RemoteTc().Connect(_ => _.AsGuest(), teamCityCaller);

            // Act
            if (paused)
                connectedTc.SetBuildConfigurationField(_ => _.Id("BuildId"), __ => __.Paused());
            else
                connectedTc.SetBuildConfigurationField(_ => _.Id("BuildId"), __ => __.NotPaused());

            // Assert
            A.CallTo(
                () =>
                    teamCityCaller.Put(paused.ToString(System.Globalization.CultureInfo.InvariantCulture).ToLower(),
                    HttpContentTypes.TextPlain, "/app/rest/buildTypes/id:BuildId/paused", string.Empty))
                        .MustHaveHappened(Repeated.Exactly.Once);
        }

        [Test]
        [TestCase("A simple name")]
        [TestCase("")]
        public void SetProjectName(string name)
        {
            // Arrange
            var teamCityCaller = CreateTeamCityCaller();

            var connectedTc = new RemoteTc().Connect(_ => _.AsGuest(), teamCityCaller);

            // Act
            connectedTc.SetProjectConfigurationField(_ => _.Id("ProjectId"), __ => __.Name(name));

            // Assert
            A.CallTo(
                () =>
                    teamCityCaller.Put(name,
                    HttpContentTypes.TextPlain, "/app/rest/projects/id:ProjectId/name", string.Empty))
                        .MustHaveHappened(Repeated.Exactly.Once);
        }

        [Test]
        [TestCase("A simple name")]
        [TestCase("")]
        public void SetBuildName(string name)
        {
            // Arrange
            var teamCityCaller = CreateTeamCityCaller();

            var connectedTc = new RemoteTc().Connect(_ => _.AsGuest(), teamCityCaller);

            // Act
            connectedTc.SetBuildConfigurationField(_ => _.Id("BuildId"), __ => __.Name(name));

            // Assert
            A.CallTo(
                () =>
                    teamCityCaller.Put(name,
                    HttpContentTypes.TextPlain, "/app/rest/buildTypes/id:BuildId/name", string.Empty))
                        .MustHaveHappened(Repeated.Exactly.Once);
        }




        [Test]
        public void CreateVcsRoot()
        {
            // Arrange
            var teamCityCaller = CreateTeamCityCaller();
            var connectedTc = new RemoteTc().Connect(_ => _.AsGuest(), teamCityCaller);

            // Act
            connectedTc.CreateVcsRoot(__ => __
                .AgentCleanFilePolicy(AgentCleanFilePolicy.AllIgnoredUntrackedFiles)
                .AgentCleanPolicy(AgentCleanPolicy.Always)
                .AuthMethod(AuthMethod.Anonymous)
                .Branch("refs/head/develop")
                .BranchSpec("+:refs/head/feature/*")
                .Id("VcsRootId")
                .IgnoreKnownHosts()
                .Name("VcsRootName")
                .Password("Password")
                .ProjectId("ProjectId")
                .CheckoutSubModule()
                .Url(new Uri("http://www.gooogle.com"))
                .UseAlternates()
                .Username("Username")
                .UserNameStyle(UserNameStyle.AuthorName));

            // Assert
            string xmlData =
                "<vcs-root id=\"VcsRootId\" name=\"VcsRootName\" vcsName=\"jetbrains.git\"> <project id=\"ProjectId\"/> <properties count =\"12\"><property name=\"agentCleanFilePolicy\" value=\"ALL_IGNORED_UNTRACKED_FILES\"/><property name=\"agentCleanPolicy\" value=\"ALWAYS\"/><property name=\"authMethod\" value=\"ANONYMOUS\"/><property name=\"branch\" value=\"refs/head/develop\"/><property name=\"teamcity:branchSpec\" value=\"+:refs/head/feature/*\"/><property name=\"ignoreKnownHosts\" value=\"true\"/><property name=\"secure:password\" value=\"Password\"/><property name=\"submoduleCheckout\" value=\"CHECKOUT\"/><property name=\"url\" value=\"http://www.gooogle.com/\"/><property name=\"useAlternates\" value=\"true\"/><property name=\"username\" value=\"Username\"/><property name=\"userNameStyle\" value=\"AUTHOR_NAME\"/></properties></vcs-root>";

            A.CallTo(
                    () =>
                        teamCityCaller.Post(xmlData,
                            HttpContentTypes.ApplicationXml, "/app/rest/vcs-roots", HttpContentTypes.ApplicationJson))
                .MustHaveHappened(Repeated.Exactly.Once);
        }

        [Test]
        public void CreateVcsRootSsh()
        {
            // Arrange
            var teamCityCaller = CreateTeamCityCaller();
            var connectedTc = new RemoteTc().Connect(_ => _.AsGuest(), teamCityCaller);

            // Act
            connectedTc.CreateVcsRoot(__ => __
                .AgentCleanFilePolicy(AgentCleanFilePolicy.AllIgnoredUntrackedFiles)
                .AgentCleanPolicy(AgentCleanPolicy.Always)
                .AuthMethod(AuthMethod.TeamcitySshKey)
                .Branch("refs/head/develop")
                .BranchSpec("+:refs/head/feature/*")
                .Id("VcsRootId")
                .IgnoreKnownHosts()
                .Name("VcsRootName")
                .ProjectId("ProjectId")
                .CheckoutSubModule()
                .TeamcitySshKey("keyName")
                .Url(new Uri("http://www.gooogle.com"))
                .UseAlternates()
                .UserNameStyle(UserNameStyle.AuthorName));

            // Assert
            var xmlData =
                "<vcs-root id=\"VcsRootId\" name=\"VcsRootName\" vcsName=\"jetbrains.git\"> <project id=\"ProjectId\"/> <properties count =\"11\"><property name=\"agentCleanFilePolicy\" value=\"ALL_IGNORED_UNTRACKED_FILES\"/><property name=\"agentCleanPolicy\" value=\"ALWAYS\"/><property name=\"authMethod\" value=\"TEAMCITY_SSH_KEY\"/><property name=\"branch\" value=\"refs/head/develop\"/><property name=\"teamcity:branchSpec\" value=\"+:refs/head/feature/*\"/><property name=\"ignoreKnownHosts\" value=\"true\"/><property name=\"submoduleCheckout\" value=\"CHECKOUT\"/><property name=\"teamcitySshKey\" value=\"keyName\"/><property name=\"url\" value=\"http://www.gooogle.com/\"/><property name=\"useAlternates\" value=\"true\"/><property name=\"userNameStyle\" value=\"AUTHOR_NAME\"/></properties></vcs-root>";

            A.CallTo(
                    () =>
                        teamCityCaller.Post(xmlData,
                            HttpContentTypes.ApplicationXml, "/app/rest/vcs-roots", HttpContentTypes.ApplicationJson))
                .MustHaveHappened(Repeated.Exactly.Once);
        }

        [Test]
        public void AttachVcsRoot()
        {
            // Arrange
            var teamCityCaller = CreateTeamCityCaller();

            var connectedTc = new RemoteTc().Connect(_ => _.AsGuest(), teamCityCaller);

            var vcsRoot = connectedTc.CreateVcsRoot(__ => __
                .AgentCleanFilePolicy(AgentCleanFilePolicy.AllIgnoredUntrackedFiles)
                .AgentCleanPolicy(AgentCleanPolicy.Always)
                .AuthMethod(AuthMethod.Anonymous)
                .Branch("refs/head/develop")
                .BranchSpec("+:refs/head/feature/*")
                .Id("VcsRootId")
                .Name("VcsRootName")
                .Password("Password")
                .ProjectId("ProjectId")
                .CheckoutSubModule()
                .Url(new Uri("http://www.gooogle.com"))
                .UseAlternates()
                .Username("Username")
                .UserNameStyle(UserNameStyle.AuthorName));

            // Act
            connectedTc.AttachVcsRootToBuildConfiguration(
                _ => _.Id("BuildId"), 
                _ => _.Id(vcsRoot.Id)
                      .CheckoutRules("CheckoutRules"));

            // Assert
            string xmlData = string.Format(
                @"<vcs-root-entry id=""{0}"">
                    <vcs-root id=""{0}""/>                    
                    <checkout-rules>{1}</checkout-rules>
                </vcs-root-entry>", vcsRoot.Id, "CheckoutRules");
            A.CallTo(
                () =>
                    teamCityCaller.Post(xmlData,
                    HttpContentTypes.ApplicationXml, "/app/rest/buildTypes/id:BuildId/vcs-root-entries", string.Empty))
                        .MustHaveHappened(Repeated.Exactly.Once);
        }
    }
}
