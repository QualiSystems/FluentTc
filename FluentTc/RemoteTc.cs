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
            var buildProjectHavingBuilderFactory = new BuildProjectHavingBuilderFactory();
            var locatorBuilder = new LocatorBuilder(new BuildConfigurationHavingBuilderFactory(new BuildProjectHavingBuilderFactory()),
                new BuildProjectHavingBuilderFactory());
            var buildHavingBuilderFactory = new BuildHavingBuilderFactory(new UserHavingBuilderFactory(),
                new BranchHavingBuilderFactory(),
                buildProjectHavingBuilderFactory,
                locatorBuilder);
            var buildsRetriever = new BuildsRetriever(teamCityCaller, buildHavingBuilderFactory, new CountBuilderFactory(), new BuildIncludeBuilderFactory(), buildProjectHavingBuilderFactory, new BuildConfigurationHavingBuilderFactory(new BuildProjectHavingBuilderFactory()),new QueueHavingBuilderFactory(locatorBuilder));
            var projectsRetriever = new ProjectsRetriever(new BuildProjectHavingBuilderFactory(), teamCityCaller);
            return new ConnectedTc(buildsRetriever, new AgentsRetriever(teamCityCaller, new AgentHavingBuilderFactory()), projectsRetriever, new BuildConfigurationRetriever(new BuildConfigurationHavingBuilderFactory(new BuildProjectHavingBuilderFactory()), teamCityCaller));
        }
    }
}