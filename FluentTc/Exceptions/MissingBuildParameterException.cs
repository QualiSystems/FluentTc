using System;
using System.Runtime.Serialization;

namespace FluentTc.Exceptions
{
    [Serializable]
    public class MissingBuildParameterException : Exception
    {
        private readonly string m_ParameterName;

        public MissingBuildParameterException()
        {
        }

        public MissingBuildParameterException(string parameterName) : base(string.Format("Build parameter {0} is missing. It needs to be added from TeamCity", parameterName))
        {
            m_ParameterName = parameterName;
        }

        public MissingBuildParameterException(string parameterName, Exception innerException)
            : base(string.Format("Build parameter {0} is missing. It needs to be added from TeamCity", parameterName), innerException)
        {
            m_ParameterName = parameterName;
        }

        protected MissingBuildParameterException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public string ParameterName
        {
            get { return m_ParameterName; }
        }
    }
}