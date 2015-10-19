using System.Collections.Generic;

namespace FluentTc.Locators
{
    public interface IAgentHavingBuilder
    {
        AgentHavingBuilder Connected();
        AgentHavingBuilder Disconnected();
        AgentHavingBuilder Authorized();
        AgentHavingBuilder Enabled();
        string GetLocator();
    }

    public class AgentHavingBuilder : IAgentHavingBuilder
    {
        private readonly List<string> m_Having = new List<string>();

        public AgentHavingBuilder Connected()
        {
            m_Having.Add("connected:" + bool.TrueString);
            return this;
        }

        public AgentHavingBuilder Disconnected()
        {
            m_Having.Add("connected:" + bool.FalseString);
            return this;
        }

        public AgentHavingBuilder Authorized()
        {
            m_Having.Add("authorized:" + bool.TrueString);
            return this;
        }

        public AgentHavingBuilder Enabled()
        {
            m_Having.Add("enabled:" + bool.TrueString);
            return this;
        }

        public string GetLocator()
        {
            return string.Join(",", m_Having);
        }
    }
}