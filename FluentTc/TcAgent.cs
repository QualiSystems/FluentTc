using System;
using System.Collections.Generic;

namespace FluentTc
{
    public class TcAgent
    {
        
    }

    public partial class RemoteTc
    {
        public List<TcAgent> GetAgents(Action<TcAgentHavingBuilder> having)
        {
            return new List<TcAgent>();
        }
    }

    public class TcAgentHavingBuilder
    {
        private readonly List<string> m_Having = new List<string>();

        public TcAgentHavingBuilder Authorized()
        {
            m_Having.Add("authorized:" + bool.TrueString);
            return this;
        }

        public TcAgentHavingBuilder Connected()
        {
            m_Having.Add("connected:" + bool.TrueString);
            return this;
        }
        public TcAgentHavingBuilder Enabled()
        {
            m_Having.Add("enabled:" + bool.TrueString);
            return this;
        }
    }
}