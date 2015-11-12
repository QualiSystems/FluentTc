using FluentTc.Locators;

namespace FluentTc.Engine
{
    public interface IAgentHavingBuilderFactory
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