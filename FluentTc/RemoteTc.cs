using System;
using FluentTc.Locators;

namespace FluentTc
{
    public class RemoteTc
    {
        public IConnectedTc Connect(Action<TeamCityConfigurationBuilder> connect)
        {
            var teamCityConfigurationBuilder = new TeamCityConfigurationBuilder();
            connect(teamCityConfigurationBuilder);
            ITeamCityCaller caller = new TeamCityCaller(teamCityConfigurationBuilder.GetITeamCityConnectionDetails());
            var buildHavingBuilderFactory = new BuildHavingBuilderFactory(new BuildConfigurationHavingBuilderFactory(new BuildProjectHavingBuilderFactory()), new UserHavingBuilderFactory(), new BranchHavingBuilderFactory(), new BuildProjectHavingBuilderFactory());
            return new ConnectedTc(new BuildsRetriever(caller, buildHavingBuilderFactory, new CountBuilderFactory(), new BuildIncludeBuilderFactory()), new AgentsRetriever(caller, new AgentHavingBuilderFactory()), new ProjectsRetriever(new BuildProjectHavingBuilderFactory(), caller));
        }
    }
}