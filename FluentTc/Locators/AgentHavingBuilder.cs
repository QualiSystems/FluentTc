using System.Collections.Generic;

namespace FluentTc.Locators
{
    public interface IAgentHavingBuilder : ILocator
    {
        IAgentHavingBuilder Id(int agentId);
        IAgentHavingBuilder Name(string agentName);
        AgentHavingBuilder Connected();
        AgentHavingBuilder Disconnected();
        AgentHavingBuilder Enabled();
        AgentHavingBuilder Disabled();
        AgentHavingBuilder Authorized();
        AgentHavingBuilder NotAuthorized();
        AgentHavingBuilder Ip(string ip);
    }

    public class AgentHavingBuilder : IAgentHavingBuilder
    {
        private readonly List<string> m_Having = new List<string>();

        public IAgentHavingBuilder Name(string agentName)
        {
            m_Having.Add("name:" + agentName);
            return this;
        }

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

        public AgentHavingBuilder Disabled()
        {
            throw new System.NotImplementedException();
        }

        public AgentHavingBuilder Authorized()
        {
            m_Having.Add("authorized:" + bool.TrueString);
            return this;
        }

        public AgentHavingBuilder NotAuthorized()
        {
            m_Having.Add("authorized:" + bool.FalseString);
            return this;
        }

        public AgentHavingBuilder Ip(string ip)
        {
            m_Having.Add("ip:" + ip);
            return this;
        }

        public AgentHavingBuilder Enabled()
        {
            m_Having.Add("enabled:" + bool.TrueString);
            return this;
        }

        public IAgentHavingBuilder Id(int agentId)
        {
            m_Having.Add("id:" + agentId);
            return this;
        }

        public string GetLocator()
        {
            return string.Join(",", m_Having);
        }
    }
}