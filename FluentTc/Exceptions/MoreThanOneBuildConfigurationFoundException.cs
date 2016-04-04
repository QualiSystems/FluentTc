using System;
using System.Runtime.Serialization;

namespace FluentTc.Exceptions
{
    [Serializable]
    public class MoreThanOneBuildConfigurationFoundException : Exception
    {
        public MoreThanOneBuildConfigurationFoundException()
        {
        }

        public MoreThanOneBuildConfigurationFoundException(string message) : base(message)
        {
        }

        public MoreThanOneBuildConfigurationFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected MoreThanOneBuildConfigurationFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}