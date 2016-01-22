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
        private const string EnabledPrefix = "enabled:";
        private const string AuthorizedPrefix = "authorized:";
        private const string NamePrefix = "name:";
        private const string ConnectedPrefix = "connected:";
        private const string IdPrefix = "id:";

        private readonly List<string> m_Having = new List<string>();

        public IAgentHavingBuilder Name(string agentName)
        {
            m_Having.Add(NamePrefix + agentName);
            return this;
        }

        public IAgentHavingBuilder Connected()
        {
            m_Having.Add(ConnectedPrefix + bool.TrueString);
            return this;
        }

        public IAgentHavingBuilder Disconnected()
        {
            m_Having.Add(ConnectedPrefix + bool.FalseString);
            return this;
        }

        public IAgentHavingBuilder Disabled()
        {
            m_Having.Add(EnabledPrefix + bool.FalseString);
            return this;
        }

        public IAgentHavingBuilder Authorized()
        {
            m_Having.Add(AuthorizedPrefix + bool.TrueString);
            return this;
        }

        public IAgentHavingBuilder NotAuthorized()
        {
            m_Having.Add(AuthorizedPrefix + bool.FalseString);
            return this;
        }

        public IAgentHavingBuilder Ip(string ip)
        {
            m_Having.Add("ip:" + ip);
            return this;
        }

        public IAgentHavingBuilder Enabled()
        {
            m_Having.Add(EnabledPrefix + bool.TrueString);
            return this;
        }

        public IAgentHavingBuilder Id(int agentId)
        {
            m_Having.Add(IdPrefix + agentId);
            return this;
        }

        public string GetLocator()
        {
            return string.Join(",", m_Having);
        }
    }
}