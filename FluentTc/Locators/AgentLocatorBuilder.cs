namespace FluentTc.Locators
{
    public interface IAgentLocatorBuilder
    {
        IAgentLocatorBuilder AgentName(string agentName);
        IAgentLocatorBuilder AgentId(string agentId);
    }

    public class AgentLocatorBuilder : IAgentLocatorBuilder
    {
        public IAgentLocatorBuilder AgentName(string agentName)
        {
            return this;
        }

        public IAgentLocatorBuilder AgentId(string agentId)
        {
            return this;
        }
    }
}