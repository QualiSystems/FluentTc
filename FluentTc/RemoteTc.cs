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
            var buildHavingBuilderFactory = new BuildHavingBuilderFactory(new BuildConfigurationHavingBuilderFactory(buildProjectHavingBuilderFactory), new UserHavingBuilderFactory(), new BranchHavingBuilderFactory(), buildProjectHavingBuilderFactory);
            m_BuildsRetriever = new BuildsRetriever(m_Caller, buildHavingBuilderFactory, new CountBuilderFactory(), new BuildIncludeBuilderFactory(), buildProjectHavingBuilderFactory);
            var buildsRetriever = new BuildsRetriever(teamCityCaller, buildHavingBuilderFactory, new CountBuilderFactory(), new BuildIncludeBuilderFactory());
            var projectsRetriever = new ProjectsRetriever(new BuildProjectHavingBuilderFactory(), teamCityCaller);
            return new ConnectedTc(buildsRetriever, new AgentsRetriever(teamCityCaller, new AgentHavingBuilderFactory()), projectsRetriever, new BuildConfigurationRetriever(new BuildConfigurationHavingBuilderFactory(new BuildProjectHavingBuilderFactory()), teamCityCaller));
        }
    }
}