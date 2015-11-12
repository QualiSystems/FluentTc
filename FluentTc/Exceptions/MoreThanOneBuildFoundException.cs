using System;
using System.Runtime.Serialization;

namespace FluentTc.Exceptions
{
    [Serializable]
    public class MoreThanOneBuildFoundException : Exception
    {
        public MoreThanOneBuildFoundException()
        {
        }

        public MoreThanOneBuildFoundException(string message) : base(message)
        {
        }

        public MoreThanOneBuildFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected MoreThanOneBuildFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}