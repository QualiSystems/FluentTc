using FluentTc.Engine;
using FluentTc.Locators;
using JetBrains.TeamCity.ServiceMessages.Write;
using JetBrains.TeamCity.ServiceMessages.Write.Special;

namespace FluentTc
{
    public interface ILocalTc
    {
        void ChangeBuildStatus(BuildStatus buildStatus);
        string GetBuildParameter(string buildParameterName);
        void SetBuildParameter(string buildParameterName, string buildParameterValue);
    }

    public class LocalTc : ILocalTc
    {
        private readonly IBuildParameters m_BuildParameters;
        private readonly ITeamCityWriter m_TeamCityWriter;

        public LocalTc() : this(null)
        {
        }

        internal LocalTc(object[] overrides)
            : this(new Bootstrapper(overrides).Get<IBuildParameters>(), new Bootstrapper(overrides).Get<ITeamCityWriterFactory>())
        {
        }

        internal LocalTc(IBuildParameters buildParameters, ITeamCityWriterFactory teamCityWriterFactory)
        {
            m_BuildParameters = buildParameters;
            m_TeamCityWriter = teamCityWriterFactory.CreateTeamCityWriter();
        }

        public IBuildParameters BuildParameters
        {
            get { return m_BuildParameters; }
        }

        public void ChangeBuildStatus(BuildStatus buildStatus)
        {
            m_TeamCityWriter.WriteRawMessage(new ServiceMessage("buildStatus")
            {
                {"status", buildStatus.ToString().ToUpper()}
            });
        }

        public string GetBuildParameter(string buildParameterName)
        {
            return m_BuildParameters.GetParameterValue(buildParameterName);
        }

        public void SetBuildParameter(string buildParameterName, string buildParameterValue)
        {
            m_BuildParameters.SetParameterValue(buildParameterName, buildParameterValue);
        }
    }
}