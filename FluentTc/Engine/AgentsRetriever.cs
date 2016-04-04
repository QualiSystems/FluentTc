using System;
using System.Collections.Generic;
using System.Linq;
using FluentTc.Domain;
using FluentTc.Exceptions;
using FluentTc.Locators;

namespace FluentTc.Engine
{
    internal interface IAgentsRetriever
    {
        List<Agent> GetAgents(Action<IAgentHavingBuilder> having);
        Agent GetAgent(Action<IAgentHavingBuilder> having);
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

        public List<Agent> GetAgents(Action<IAgentHavingBuilder> having)
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

        public Agent GetAgent(Action<IAgentHavingBuilder> having)
        {
            var buildConfigurations = GetAgents(having);
            if (!buildConfigurations.Any()) throw new AgentNotFoundException();
            if (buildConfigurations.Count() > 1) throw new MoreThanOneAgentFoundException();
            return buildConfigurations.Single();
        }
    }
}