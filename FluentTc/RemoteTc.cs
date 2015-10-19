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
            ITeamCityCaller teamCityCaller = new TeamCityCaller(teamCityConfigurationBuilder.GetITeamCityConnectionDetails());
            var buildHavingBuilderFactory = new BuildHavingBuilderFactory(new BuildConfigurationHavingBuilderFactory(new BuildProjectHavingBuilderFactory()), new UserHavingBuilderFactory(), new BranchHavingBuilderFactory(), new BuildProjectHavingBuilderFactory());
            var buildsRetriever = new BuildsRetriever(teamCityCaller, buildHavingBuilderFactory, new CountBuilderFactory(), new BuildIncludeBuilderFactory());
            var projectsRetriever = new ProjectsRetriever(new BuildProjectHavingBuilderFactory(), teamCityCaller);
            return new ConnectedTc(buildsRetriever, new AgentsRetriever(teamCityCaller, new AgentHavingBuilderFactory()), projectsRetriever, new BuildConfigurationRetriever(new BuildConfigurationHavingBuilderFactory(new BuildProjectHavingBuilderFactory()), teamCityCaller));
        }
    }
}