using System;
using System.IO.Abstractions;
using FluentTc.Engine;

namespace FluentTc
{
    public interface ILocalTc
    {
        IBuildParameters BuildParameters { get; }
    }

    public class LocalTc : ILocalTc
    {
        private readonly IBuildParameters m_BuildParameters;

        public LocalTc()
        {
            m_BuildParameters = new BuildParameters(
                Environment.GetEnvironmentVariable("TEAMCITY_BUILD_PROPERTIES_FILE"), new FileSystem());
        }

        public IBuildParameters BuildParameters
        {
            get { return m_BuildParameters; }
        }
    }
}