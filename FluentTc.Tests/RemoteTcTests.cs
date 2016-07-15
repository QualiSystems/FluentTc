﻿using FluentAssertions;
using MoreLinq;
using NUnit.Framework;

namespace FluentTc.Tests
{
    [TestFixture]
    public class RemoteTcTests
    {
        public void Sample_Usage()
        {
            // Agents
            new RemoteTc().Connect(a => a.ToHost("tc").AsGuest())
                .DisableAgent(_ => _.Ip("127.0.0.1"));

            new RemoteTc().Connect(a => a.ToHost("tc").AsGuest())
                .EnableAgent(_ => _.Name("agent1"));

            // Get project by Id
            var project = new RemoteTc().Connect(a => a.ToHost("tc").AsGuest())
                .GetProjectById("FluentTc");

            // Create sub project with Id and Name
            project = new RemoteTc().Connect(a => a.ToHost("tc").AsGuest())
                .CreateProject(_ => 
                    _.Name("New Project Name")
                    .Id("newProjectId")
                    .ParentProject(a => a.Name("FluentTc")));

            // Create project by Name in Root
            project = new RemoteTc().Connect(a => a.ToHost("tc").AsGuest())
                .CreateProject(_ => _.Name("New Project Name"));

            // Agents
            var agents = new RemoteTc().Connect(a => a.ToHost("tc").AsGuest())
                .GetAgents(h => h.Connected());

            var enabledAuthorizedButDisconnectedAgents = new RemoteTc().Connect(a => a.ToHost("tc").AsGuest())
                .GetAgents(h => h.Disconnected().Enabled().Authorized());

            // Build queue  
            var buildQueue = new RemoteTc().Connect(_ => _.ToHost("tc"))
                .GetBuildsQueue(_ => _.Project(__ => __.Id("Branch6_4_Red_NightlyCi_RedWebTests")));

            var buildQueue2 = new RemoteTc().Connect(_ => _.ToHost("tc"))
                .GetBuildsQueue(
                    __ =>
                        __.Project(___ => ___.Id("Branch6_4_Red_NightlyCi_RedWebTests"))
                            .BuildConfiguration(b => b.Name("Trunk")));

            // Remove builds from queue by project Id recursively 
            var connectedTc = new RemoteTc().Connect(_ => _.ToHost("tc"));
            connectedTc.GetBuildConfigurationsRecursively("ProjectId")
                .ForEach(c => connectedTc.RemoveBuildFromQueue(__ => __.BuildConfiguration(___ => ___.Id(c.Id))));

            // Builds
            var builds = new RemoteTc().Connect(a => a.ToHost("tc").AsGuest())
                .GetBuilds(
                    h =>
                        h.BuildConfiguration(r => r.Id("bt2"))
                            .NotPersonal()
                            .Project(r => r.Name("Trunk"))
                            .AgentName("BUILDS11")
                            .Branch(b => b.Name("aa")));

            builds = new RemoteTc().Connect(_ => _.ToHost("tc"))
                .GetBuilds(_ => _.Personal(),
                    _ => _.IncludeStartDate().IncludeFinishDate().IncludeStatusText(), _ => _.DefaultCount());

            builds = new RemoteTc().Connect(_ => _.ToHost("tc"))
                .GetBuilds(_ => _.Personal(), _ => _.IncludeDefaults(), _ => _.Count(5));

            builds = new RemoteTc().Connect(_ => _.ToHost("tc"))
                .GetBuilds(_ => _.BuildConfiguration(x => x.Id("bt2")).NotPersonal().NotRunning(),
                    _ => _.IncludeDefaults(), _ => _.Count(5));

            var build = new RemoteTc().Connect(_ => _.ToHost("tc"))
                .GetBuild(_ => _.Id(123456), _ => _.IncludeDefaults());

            build = new RemoteTc().Connect(_ => _.ToHost("tc"))
                .GetBuild(_ => _.Id(123456));


            // Build configurations
            var buildConfiguration = new RemoteTc().Connect(_ => _.ToHost("tc"))
                .GetBuildConfiguration(_ => _.Id("bt2"));

            // Retrieves all the build configuration under a project
            var buildConfigurations = new RemoteTc().Connect(_ => _.ToHost("tc").AsGuest())
                .GetBuildConfigurations(_ => _.Project(__ => __.Id("Trunk")));

            // Retrieves all the build configuration under a project recursively
            buildConfigurations = new RemoteTc().Connect(_ => _.ToHost("tc").AsGuest())
                .GetBuildConfigurations(_ => _.ProjectRecursively(__ => __.Id("Trunk")));


            new RemoteTc().Connect(_ => _.ToHost("tc"))
                .SetBuildConfigurationParameters(_ => _.Id("bt2"),
                    _ => _.Parameter("name", "value").Parameter("name2", "value"));

            build = new RemoteTc().Connect(_ => _.ToHost("tc"))
                .RunBuildConfiguration(_ => _.Id("bt2"));

            build = new RemoteTc().Connect(_ => _.ToHost("tc"))
                .RunBuildConfiguration(_ => _.Id("bt2"),
                    _ => _.Parameter("name", "value").Parameter("name2", "value"));

            build = new RemoteTc().Connect(_ => _.ToHost("tc"))
                .RunBuildConfiguration(having => having.Id("bt2"), onAgent => onAgent.Name("agent1"));

            build = new RemoteTc().Connect(_ => _.ToHost("tc"))
                .RunBuildConfiguration(_ => _.Id("bt2"), _ => _.Name("agent1"),
                    _ => _.Parameter("name", "value").Parameter("name2", "value"));

            buildConfiguration = new RemoteTc().Connect(_ => _.ToHost("tc"))
                .CreateBuildConfiguration(_ => _.Id("Trunk"), "config name");

            // Retrieves all the projects
            var allProjects = new RemoteTc().Connect(_ => _.ToHost("tc").AsGuest())
                .GetAllProjects();

            var downloadedFiles = new RemoteTc().Connect(a => a.ToHost("tc").AsGuest())
                .DownloadArtifacts(123, @"C:\DownloadedArtifacts");

            string downloadedFile = new RemoteTc().Connect(a => a.ToHost("tc").AsGuest())
                .DownloadArtifact(759688, @"C:\DownloadedArtifacts", "Logs.zip");

            var testInvestigation = new RemoteTc().Connect(_ => _.ToHost("tc"))
                    .GetTestinvestigationByTestNameId("-1884830467297296372");

            var investigation = new RemoteTc().Connect(_ => _.ToHost("tc"))
                .GetInvestigation(_=> _.Id("fluentTc"));
        }

        [Test]
        public void Connect_Guest_NotNull()
        {
            // Arrange
            var remoteTc = new RemoteTc();

            // Act
            var connectedTc = remoteTc.Connect(_ => _.AsGuest());

            // Assert
            connectedTc.Should().NotBeNull();
        }
    }
}