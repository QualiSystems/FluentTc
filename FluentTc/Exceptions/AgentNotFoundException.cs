using System;
using System.Runtime.Serialization;

namespace FluentTc.Exceptions
{
    [Serializable]
    public class AgentNotFoundException : Exception
    {
        public AgentNotFoundException()
        {
        }

        public AgentNotFoundException(string message) : base(message)
        {
        }

        public AgentNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected AgentNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}