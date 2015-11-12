using System;
using System.Runtime.Serialization;

namespace FluentTc.Exceptions
{
    [Serializable]
    public class BuildNotFoundException : Exception
    {
        public BuildNotFoundException()
        {
        }

        public BuildNotFoundException(string message) : base(message)
        {
        }

        public BuildNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected BuildNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}