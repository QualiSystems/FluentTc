using System.Collections.Generic;

namespace FluentTc.Locators
{
    public interface IAgentHavingBuilder
    {
        IAgentHavingBuilder Id(int agentId);
        IAgentHavingBuilder Name(string agentName);
        IAgentHavingBuilder Connected();
        IAgentHavingBuilder Disconnected();
        IAgentHavingBuilder Enabled();
        IAgentHavingBuilder Disabled();
        IAgentHavingBuilder Authorized();
        IAgentHavingBuilder NotAuthorized();
        IAgentHavingBuilder Ip(string ip);
    }

    public class AgentHavingBuilder : IAgentHavingBuilder
    {
        private readonly List<string> m_Having = new List<string>();

        public IAgentHavingBuilder Name(string agentName)
        {
            m_Having.Add("name:" + agentName);
            return this;
        }

        public IAgentHavingBuilder Connected()
        {
            m_Having.Add("connected:" + bool.TrueString);
            return this;
        }

        public IAgentHavingBuilder Disconnected()
        {
            m_Having.Add("connected:" + bool.FalseString);
            return this;
        }

        public IAgentHavingBuilder Disabled()
        {
            throw new System.NotImplementedException();
        }

        public IAgentHavingBuilder Authorized()
        {
            m_Having.Add("authorized:" + bool.TrueString);
            return this;
        }

        public IAgentHavingBuilder NotAuthorized()
        {
            m_Having.Add("authorized:" + bool.FalseString);
            return this;
        }

        public IAgentHavingBuilder Ip(string ip)
        {
            m_Having.Add("ip:" + ip);
            return this;
        }

        public IAgentHavingBuilder Enabled()
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