using System.Collections.Generic;

namespace FluentTc.Locators
{
    public interface IAgentHavingBuilder
    {
        AgentHavingBuilder OnlyConnected();
        AgentHavingBuilder OnlyAuthorized();
        string GetLocator();
    }

    public class AgentHavingBuilder : IAgentHavingBuilder
    {
        private readonly List<string> m_Having = new List<string>();

        public AgentHavingBuilder OnlyConnected()
        {
            m_Having.Add("connected:" + bool.TrueString);
            return this;
        }

        public AgentHavingBuilder OnlyAuthorized()
        {
            m_Having.Add("authorized:" + bool.TrueString);
            return this;
        }

        public string GetLocator()
        {
            return string.Join(",", m_Having);
        }
    }
}