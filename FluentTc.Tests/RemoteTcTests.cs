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
            BuildConfiguration build2 = new RemoteTc().Connect(_ => _.ToHost("tc"))
                .GetBuildConfiguration(_ => _.Id("bt2"), _ => _.IncludeDefaults());

            BuildConfiguration build3 = new RemoteTc().Connect(_ => _.ToHost("tc"))
                .SetParameters(_ => _.Id("bt2"),
                    _ => _.Parameters("name", "value").Parameters("name2", "value"));

            BuildConfiguration build4 = new RemoteTc().Connect(_ => _.ToHost("tc"))
                .RunBuildConfiguration(_ => _.Id("bt2"));

            BuildConfiguration build5 = new RemoteTc().Connect(_ => _.ToHost("tc"))
                .RunBuildConfiguration(_ => _.Id("bt2"),
                    _ => _.Parameters("name", "value").Parameters("name2", "value"));

            BuildConfiguration build6 = new RemoteTc().Connect(_ => _.ToHost("tc"))
                .RunBuildConfiguration(_ => _.Id("bt2"), _ => _.AgentName("agent1"));

            BuildConfiguration build7 = new RemoteTc().Connect(_ => _.ToHost("tc"))
                .RunBuildConfiguration(_ => _.Id("bt2"), _ => _.AgentName("agent1"),
                    _ => _.Parameters("name", "value").Parameters("name2", "value"));

            BuildConfiguration build8 = new RemoteTc().Connect(_ => _.ToHost("tc"))
                .CreateBuildConfiguration(_ => _.ProjectId("Trunk"), "config name");

            new RemoteTc().Connect(_ => _.ToHost("tc"))
                .AttachBuildConfigurationToTemplate(_ => _.Name("Trunk"), _ => _.TemplateName("config name"));

            new RemoteTc().Connect(_ => _.ToHost("tc"))
                .DeleteBuildConfiguration(_ => _.Name("Trunk"));
        }
    }
}