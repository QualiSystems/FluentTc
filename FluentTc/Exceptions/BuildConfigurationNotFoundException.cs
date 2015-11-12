using System;
using System.Runtime.Serialization;

namespace FluentTc.Exceptions
{
    [Serializable]
    public class BuildConfigurationNotFoundException : Exception
    {
        public BuildConfigurationNotFoundException()
        {
        }

        public BuildConfigurationNotFoundException(string message) : base(message)
        {
        }

        public BuildConfigurationNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected BuildConfigurationNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}