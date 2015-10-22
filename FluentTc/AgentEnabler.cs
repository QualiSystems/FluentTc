using System;
using EasyHttp.Http;
using FluentTc.Locators;

namespace FluentTc
{
    internal interface IAgentEnabler
    {
        void DisableAgent(Action<AgentHavingBuilder> having);
        void EnableAgent(Action<AgentHavingBuilder> having);
    }

    internal class AgentEnabler : IAgentEnabler
    {
        private readonly IAgentHavingBuilderFactory m_AgentHavingBuilderFactory;
        private readonly ITeamCityCaller m_TeamCityCaller;

        public AgentEnabler(IAgentHavingBuilderFactory agentHavingBuilderFactory, ITeamCityCaller teamCityCaller)
        {
            m_AgentHavingBuilderFactory = agentHavingBuilderFactory;
            m_TeamCityCaller = teamCityCaller;
        }

        public void DisableAgent(Action<AgentHavingBuilder> having)
        {
            ToggleAgent(having, bool.FalseString);
        }

        public void EnableAgent(Action<AgentHavingBuilder> having)
        {
            ToggleAgent(having, bool.TrueString);
        }

        private void ToggleAgent(Action<AgentHavingBuilder> having, string enabled)
        {
            var agentHavingBuilder = m_AgentHavingBuilderFactory.CreateAgentHavingBuilder();
            having(agentHavingBuilder);
            var locator = agentHavingBuilder.GetLocator();
            m_TeamCityCaller.PutFormat(enabled, HttpContentTypes.TextPlain, "/app/rest/agents/{0}/enabled",
                locator);
        }
    }
}