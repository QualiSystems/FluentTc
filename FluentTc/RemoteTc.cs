using System;
using FluentTc.Locators;

namespace FluentTc
{
    public class RemoteTc
    {
        private ITeamCityCaller m_Caller;
        private IBuildsRetriever m_BuildsRetriever;
        private IAgentsRetriever m_AgentsRetriever;

        public IConnectedTc Connect(Action<TeamCityConfigurationBuilder> connect)
        {
            var teamCityConfigurationBuilder = new TeamCityConfigurationBuilder();
            connect(teamCityConfigurationBuilder);
            m_Caller = new TeamCityCaller(teamCityConfigurationBuilder.GetITeamCityConnectionDetails());
            var buildHavingBuilderFactory = new BuildHavingBuilderFactory(new BuildConfigurationHavingBuilderFactory(new BuildProjectHavingBuilderFactory()), new UserHavingBuilderFactory(), new BranchHavingBuilderFactory());
            m_BuildsRetriever = new BuildsRetriever(m_Caller, buildHavingBuilderFactory, new CountBuilderFactory(), new BuildIncludeBuilderFactory());
            m_AgentsRetriever = new AgentsRetriever(m_Caller, new AgentHavingBuilderFactory());
            return new ConnectedTc(m_Caller, m_BuildsRetriever, m_AgentsRetriever);
        }
    }
}