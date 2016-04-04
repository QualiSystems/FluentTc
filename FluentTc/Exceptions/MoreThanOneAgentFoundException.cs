using System;
using System.Runtime.Serialization;

namespace FluentTc.Exceptions
{
    [Serializable]
    public class MoreThanOneAgentFoundException : Exception
    {
        public MoreThanOneAgentFoundException()
        {
        }

        public MoreThanOneAgentFoundException(string message) : base(message)
        {
        }

        public MoreThanOneAgentFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected MoreThanOneAgentFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}