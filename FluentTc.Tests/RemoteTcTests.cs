using System.Collections.Generic;
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
            // Builds
            List<Build> list = new RemoteTc().Connect(a => a.ToHost("tc").AsGuest())
                .GetBuilds(h => h.BuildConfiguration(r => r.ConfigurationId(123456)));

            List<Build> builds = new RemoteTc().Connect(_ => _.ToHost("tc"))
                .GetBuilds(_ => _.Personal(), _ => _.Top(5), _ => _.IncludeDefaults());

            Build build1 = new RemoteTc().Connect(_ => _.ToHost("tc"))
                .GetBuild(_ => _.HavingId(123456), _ => _.IncludeDefaults());            
            
            Build build11 = new RemoteTc().Connect(_ => _.ToHost("tc"))
                .GetBuild(_ => _.HavingId(123456));

            // Build configurations
            BuildConfiguration build2 = new RemoteTc().Connect(_ => _.ToHost("tc"))
                .GetBuildConfiguration(_ => _.ConfigurationId(123456), _ => _.IncludeDefaults());

            BuildConfiguration build3 = new RemoteTc().Connect(_ => _.ToHost("tc"))
                .SetParameters(_ => _.ConfigurationId(123456),
                    _ => _.Parameters("name", "value").Parameters("name2", "value"));

            BuildConfiguration build4 = new RemoteTc().Connect(_ => _.ToHost("tc"))
                .Run(_ => _.ConfigurationId(123456));

            BuildConfiguration build5 = new RemoteTc().Connect(_ => _.ToHost("tc"))
                .Run(_ => _.ConfigurationId(123456),
                    _ => _.Parameters("name", "value").Parameters("name2", "value"));

            BuildConfiguration build6 = new RemoteTc().Connect(_ => _.ToHost("tc"))
                .Run(_ => _.ConfigurationId(123456), _ => _.AgentName("agent1"));

            BuildConfiguration build7 = new RemoteTc().Connect(_ => _.ToHost("tc"))
                .Run(_ => _.ConfigurationId(123456), _ => _.AgentName("agent1"),
                    _ => _.Parameters("name", "value").Parameters("name2", "value"));

            BuildConfiguration build8 = new RemoteTc().Connect(_ => _.ToHost("tc"))
                .CreateBuildConfiguration(_ => _.ProjectId("Trunk"), "config name");

            new RemoteTc().Connect(_ => _.ToHost("tc"))
                .AttachToTemplate(_ => _.ConfigurationName("Trunk"), _ => _.TemplateName("config name"));

            new RemoteTc().Connect(_ => _.ToHost("tc"))
                .DeleteBuildConfiguration(_ => _.ConfigurationName("Trunk"));
        }
    }
}