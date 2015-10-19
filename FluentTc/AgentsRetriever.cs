using System;
using System.Collections.Generic;
using FluentTc.Domain;
using FluentTc.Locators;

namespace FluentTc
{
    internal interface IAgentsRetriever
    {
        List<Agent> GetAgents(Action<AgentHavingBuilder> having);
    }

    internal class AgentsRetriever : IAgentsRetriever
    {
        private readonly ITeamCityCaller m_Caller;
        private readonly IAgentHavingBuilderFactory m_AgentHavingBuilderFactory;

        public AgentsRetriever(ITeamCityCaller caller, IAgentHavingBuilderFactory agentHavingBuilderFactory)
        {
            m_Caller = caller;
            m_AgentHavingBuilderFactory = agentHavingBuilderFactory;
        }

        public List<Agent> GetAgents(Action<AgentHavingBuilder> having)
        {
            var agentHavingBuilder = m_AgentHavingBuilderFactory.CreateAgentHavingBuilder();
            having(agentHavingBuilder);

            var locator = agentHavingBuilder.GetLocator();
            var agentWrapper = m_Caller.GetFormat<AgentWrapper>("/app/rest/agents?locator={0}", locator);
            if (int.Parse(agentWrapper.Count) > 0)
            {
                return agentWrapper.Agent;
            }
            return new List<Agent>();
        }
    }
}