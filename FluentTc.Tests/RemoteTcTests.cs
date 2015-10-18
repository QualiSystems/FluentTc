using System.Collections.Generic;
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
            // Agents
            var tcAgents = new RemoteTc().Connect(_ => _.ToHost("tc").AsGuest())
                                    .GetAgents(h => h.Connected().Authorized().Enabled());

            // Builds
            new RemoteTc().Connect(a => a.ToHost("tc").AsGuest())
                .GetBuilds(h => h.BelongingToBuildConfiguration(r => r.ConfigurationId(123456)));

            var builds = new RemoteTc().Connect(_ => _.ToHost("tc"))
                .GetBuilds(_ => _.Personal(), _ => _.Top(5), _ => _.IncludeDefaults());

            var build1 = new RemoteTc().Connect(_ => _.ToHost("tc"))
                .GetBuild(_ => _.HavingId(123456), _ => _.IncludeDefaults());

            // Build configurations
            var build2 = new RemoteTc().Connect(_ => _.ToHost("tc"))
                .GetBuildConfiguration(_ => _.ConfigurationId(123456), _ => _.IncludeDefaults());

            var build3 = new RemoteTc().Connect(_ => _.ToHost("tc"))
                .SetParameters(_ => _.ConfigurationId(123456),
                    _ => _.Parameters("name", "value").Parameters("name2", "value"));

            var build4 = new RemoteTc().Connect(_ => _.ToHost("tc"))
                .Run(_ => _.ConfigurationId(123456));

            var build5 = new RemoteTc().Connect(_ => _.ToHost("tc"))
                .Run(_ => _.ConfigurationId(123456),
                    _ => _.Parameters("name", "value").Parameters("name2", "value"));

            var build6 = new RemoteTc().Connect(_ => _.ToHost("tc"))
                .Run(_ => _.ConfigurationId(123456), _ => _.AgentName("agent1"));

            var build7 = new RemoteTc().Connect(_ => _.ToHost("tc"))
                .Run(_ => _.ConfigurationId(123456), _ => _.AgentName("agent1"),
                    _ => _.Parameters("name", "value").Parameters("name2", "value"));

            var build8 = new RemoteTc().Connect(_ => _.ToHost("tc"))
                .CreateBuildConfiguration(_ => _.ProjectId("Trunk"), "config name");

            new RemoteTc().Connect(_ => _.ToHost("tc"))
                .AttachToTemplate(_ => _.ConfigurationName("Trunk"), _ => _.TemplateName("config name"));

            new RemoteTc().Connect(_ => _.ToHost("tc"))
                .DeleteBuildConfiguration(_ => _.ConfigurationName("Trunk"));
        }
    }
}