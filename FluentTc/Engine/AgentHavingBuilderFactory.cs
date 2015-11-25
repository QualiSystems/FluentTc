using FluentTc.Locators;

namespace FluentTc.Engine
{
    internal interface IAgentHavingBuilderFactory
    {
        AgentHavingBuilder CreateAgentHavingBuilder();
    }

    internal class AgentHavingBuilderFactory : IAgentHavingBuilderFactory
    {
        public AgentHavingBuilder CreateAgentHavingBuilder()
        {
            return new AgentHavingBuilder();
        }
    }
}