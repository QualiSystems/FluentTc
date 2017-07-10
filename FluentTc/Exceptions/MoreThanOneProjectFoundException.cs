using System;
using System.Runtime.Serialization;

namespace FluentTc.Exceptions
{
    [Serializable]
    public class MoreThanOneProjectFoundException : Exception
    {
        public MoreThanOneProjectFoundException()
        {
        }

        public MoreThanOneProjectFoundException(string message) : base(message)
        {
        }

        public MoreThanOneProjectFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected MoreThanOneProjectFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
