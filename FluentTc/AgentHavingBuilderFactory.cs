using FluentTc.Locators;

namespace FluentTc
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