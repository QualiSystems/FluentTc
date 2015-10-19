using FluentTc.Domain;
using NUnit.Framework;

namespace FluentTc.Tests
{
    [TestFixture]
    public class RemoteTcTests
    {
        [Test]
        [Ignore]
        public void Sample_Usage()
        {
            // Project
            var project = new RemoteTc().Connect(a => a.ToHost("tc").AsGuest())
                .GetProject(_ => _.Name("FluentTc"));

            // Agents
            var agents = new RemoteTc().Connect(a => a.ToHost("tc").AsGuest())
                .GetAgents(h => h.Connected());
            
            var enabledAuthorizedButDisconnectedAgents = new RemoteTc().Connect(a => a.ToHost("tc").AsGuest())
                .GetAgents(h => h.Disconnected().Enabled().Authorized());

            // Build queue

            var buildQueue = new RemoteTc().Connect(_ => _.ToHost("tc"))
               .GetBuildQueue(_ => _.Id("Branch6_4_Green_NightlyCi_TestegatorIntegrationTests"));

            // Builds
            var builds = new RemoteTc().Connect(a => a.ToHost("tc").AsGuest())
                .GetBuilds(h => h.BuildConfiguration(r => r.Id("bt2")));

            builds = new RemoteTc().Connect(_ => _.ToHost("tc"))
                .GetBuilds(_ => _.Personal(), _ => _.Count(5), _ => _.IncludeDefaults());

            builds = new RemoteTc().Connect(_ => _.ToHost("tc"))
                .GetBuilds(_ => _.Personal(), _ => _.Count(5), _ => _.IncludeDefaults());

            builds = new RemoteTc().Connect(_ => _.ToHost("tc"))
                .GetBuilds(_ => _.BuildConfiguration(x => x.Id("bt2")).NotPersonal().NotRunning(), _ => _.Count(5), _ => _.IncludeDefaults());

            var build = new RemoteTc().Connect(_ => _.ToHost("tc"))
                .GetBuild(_ => _.Id(123456), _ => _.IncludeDefaults());

            build = new RemoteTc().Connect(_ => _.ToHost("tc"))
                .GetBuild(_ => _.Id(123456));   


            // Build configurations
            BuildConfiguration buildConfiguration = new RemoteTc().Connect(_ => _.ToHost("tc"))
                .GetBuildConfiguration(_ => _.Id("bt2"), _ => _.IncludeDefaults());

            buildConfiguration = new RemoteTc().Connect(_ => _.ToHost("tc"))
                .SetParameters(_ => _.Id("bt2"),
                    _ => _.Parameters("name", "value").Parameters("name2", "value"));

            buildConfiguration = new RemoteTc().Connect(_ => _.ToHost("tc"))
                .RunBuildConfiguration(_ => _.Id("bt2"));

            buildConfiguration = new RemoteTc().Connect(_ => _.ToHost("tc"))
                .RunBuildConfiguration(_ => _.Id("bt2"),
                    _ => _.Parameters("name", "value").Parameters("name2", "value"));

            buildConfiguration = new RemoteTc().Connect(_ => _.ToHost("tc"))
                .RunBuildConfiguration(_ => _.Id("bt2"), _ => _.AgentName("agent1"));

            buildConfiguration = new RemoteTc().Connect(_ => _.ToHost("tc"))
                .RunBuildConfiguration(_ => _.Id("bt2"), _ => _.AgentName("agent1"),
                    _ => _.Parameters("name", "value").Parameters("name2", "value"));

            buildConfiguration = new RemoteTc().Connect(_ => _.ToHost("tc"))
                .CreateBuildConfiguration(_ => _.Id("Trunk"), "config name");

            new RemoteTc().Connect(_ => _.ToHost("tc"))
                .AttachBuildConfigurationToTemplate(_ => _.Name("Trunk"), _ => _.TemplateName("config name"));

            new RemoteTc().Connect(_ => _.ToHost("tc"))
                .DeleteBuildConfiguration(_ => _.Name("Trunk"));
        }
    }
}