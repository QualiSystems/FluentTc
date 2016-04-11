using System.Collections.Generic;
using FluentTc.Exceptions;
using JetBrains.TeamCity.ServiceMessages.Write.Special;

namespace FluentTc.Engine
{
    public interface IBuildParameters
    {
        T GetBuildParameter<T>(string parameterName);
        string AgentHomeDir { get; }
        string AgentName { get; }
        string AgentOwnPort { get; }
        string AgentWorkDir { get; }
        long BuildNumber { get; }
        int TeamcityAgentCpuBenchmark { get; }
        string TeamcityBuildChangedFilesFile { get; }
        string TeamcityBuildCheckoutDir { get; }
        long TeamcityBuildId { get; }
        string TeamcityBuildTempDir { get; }
        string TeamcityBuildWorkingDir { get; }
        string TeamcityBuildConfName { get; }
        string TeamcityBuildTypeId { get; }
        string TeamcityProjectName { get; }
        string TeamCityVersion { get; }
        bool IsTeamCityMode { get; }
        bool IsPersonal { get; }
        void SetBuildParameter(string parameterName, string parameterValue);
        bool TryGetBuildParameter<T>(string parameterName, out T parameterValue);
    }

    internal class BuildParameters : IBuildParameters
    {
        private readonly bool m_IsTeamCityMode = true;

        private readonly Dictionary<string, string> m_Parameters = new Dictionary<string, string>();
        private readonly ITeamCityWriter m_TeamCityWriter;

        public BuildParameters(ITeamCityBuildPropertiesFileRetriever teamCityBuildPropertiesFileRetriever, ITeamCityWriterFactory teamCityWriterFactory, IPropertiesFileParser propertiesFileParser)
        {
            m_TeamCityWriter = teamCityWriterFactory.CreateTeamCityWriter();

            string teamCityBuildPropertiesFile = teamCityBuildPropertiesFileRetriever.GetTeamCityBuildPropertiesFilePath();

            if (teamCityBuildPropertiesFile == null)
            {
                m_IsTeamCityMode = false;
                return;
            }

            m_Parameters = propertiesFileParser.ParsePropertiesFile(teamCityBuildPropertiesFile);
        }

        public T GetBuildParameter<T>(string parameterName)
        {
            string parameterValue;
            if (!m_Parameters.TryGetValue(parameterName, out parameterValue))
            {
                throw new MissingBuildParameterException(parameterName);
            }
            return UniversalTypeConverter.StringToType<T>(parameterValue);
        }

        public string AgentHomeDir
        {
            get { return GetBuildParameter<string>("agent.home.dir"); }
        }

        public string AgentName
        {
            get { return GetBuildParameter<string>("agent.name"); }
        }

        public string AgentOwnPort
        {
            get { return GetBuildParameter<string>("agent.ownPort"); }
        }

        public string AgentWorkDir
        {
            get { return GetBuildParameter<string>("agent.work.dir"); }
        }

        public long BuildNumber
        {
            get { return GetBuildParameter<long>("build.number"); }
        }

        public int TeamcityAgentCpuBenchmark
        {
            get { return GetBuildParameter<int>("teamcity.agent.cpuBenchmark"); }
        }

        public string TeamcityBuildChangedFilesFile
        {
            get { return GetBuildParameter<string>("teamcity.build.changedFiles.file"); }
        }

        public string TeamcityBuildCheckoutDir
        {
            get { return GetBuildParameter<string>("teamcity.build.checkoutDir"); }
        }

        public long TeamcityBuildId
        {
            get { return GetBuildParameter<long>("teamcity.build.id"); }
        }

        public string TeamcityBuildTempDir
        {
            get { return GetBuildParameter<string>("teamcity.build.tempDir"); }
        }

        public string TeamcityBuildWorkingDir
        {
            get { return GetBuildParameter<string>("teamcity.build.workingDir"); }
        }

        public string TeamcityBuildConfName
        {
            get { return GetBuildParameter<string>("teamcity.buildConfName"); }
        }

        public string TeamcityBuildTypeId
        {
            get { return GetBuildParameter<string>("teamcity.buildType.id"); }
        }

        public string TeamcityProjectName
        {
            get { return GetBuildParameter<string>("teamcity.projectName"); }
        }

        public string TeamCityVersion
        {
            get { return GetBuildParameter<string>("teamcity.version"); }
        }

        public bool IsTeamCityMode
        {
            get { return m_IsTeamCityMode; }
        }

        public bool IsPersonal
        {
            get
            {
                bool isPersonal;
                return TryGetBuildParameter("build.is.personal", out isPersonal) && isPersonal;
            }
        }

        public void SetBuildParameter(string parameterName, string parameterValue)
        {
            if (!m_Parameters.ContainsKey(parameterName) && m_IsTeamCityMode)
            {
                throw new MissingBuildParameterException(parameterName);
            }

            m_Parameters[parameterName] = parameterValue;
            m_TeamCityWriter.WriteBuildParameter(parameterName, parameterValue);
        }

        public bool TryGetBuildParameter<T>(string parameterName, out T parameterValue)
        {
            string stringValue;
            var parameterFound = m_Parameters.TryGetValue(parameterName, out stringValue);
            if (!parameterFound)
            {
                parameterValue = default(T);
                return false;
            }
            parameterValue = UniversalTypeConverter.StringToType<T>(stringValue);
            return true;
        }
    }
}