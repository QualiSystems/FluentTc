using System.Collections.Generic;
using FluentTc.Exceptions;
using JetBrains.TeamCity.ServiceMessages.Write.Special;

namespace FluentTc.Engine
{
    public interface IBuildParameters
    {
        string GetParameterValue(string parameterName);
        string AgentHomeDir { get; }
        string AgentName { get; }
        string AgentOwnPort { get; }
        string AgentWorkDir { get; }
        string BuildNumber { get; }
        string TeamcityAgentCpuBenchmark { get; }
        string TeamcityBuildChangedFilesFile { get; }
        string TeamcityBuildCheckoutDir { get; }
        string TeamcityBuildId { get; }
        string TeamcityBuildTempDir { get; }
        string TeamcityBuildWorkingDir { get; }
        string TeamcityBuildConfName { get; }
        string TeamcityBuildTypeId { get; }
        string TeamcityProjectName { get; }
        string TeamCityVersion { get; }
        void SetParameterValue(string parameterName, string parameterValue);
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

        public string GetParameterValue(string parameterName)
        {
            return m_Parameters[parameterName];
        }

        public string AgentHomeDir
        {
            get { return GetParameterValue("agent.home.dir"); }
        }

        public string AgentName
        {
            get { return GetParameterValue("agent.name"); }
        }

        public string AgentOwnPort
        {
            get { return GetParameterValue("agent.ownPort"); }
        }

        public string AgentWorkDir
        {
            get { return GetParameterValue("agent.work.dir"); }
        }

        public string BuildNumber
        {
            get { return GetParameterValue("build.number"); }
        }

        public string TeamcityAgentCpuBenchmark
        {
            get { return GetParameterValue("teamcity.agent.cpuBenchmark"); }
        }

        public string TeamcityBuildChangedFilesFile
        {
            get { return GetParameterValue("teamcity.build.changedFiles.file"); }
        }

        public string TeamcityBuildCheckoutDir
        {
            get { return GetParameterValue("teamcity.build.checkoutDir"); }
        }

        public string TeamcityBuildId
        {
            get { return GetParameterValue("teamcity.build.id"); }
        }

        public string TeamcityBuildTempDir
        {
            get { return GetParameterValue("teamcity.build.tempDir"); }
        }

        public string TeamcityBuildWorkingDir
        {
            get { return GetParameterValue("teamcity.build.workingDir"); }
        }

        public string TeamcityBuildConfName
        {
            get { return GetParameterValue("teamcity.buildConfNam"); }
        }

        public string TeamcityBuildTypeId
        {
            get { return GetParameterValue("teamcity.buildType.id"); }
        }

        public string TeamcityProjectName
        {
            get { return GetParameterValue("teamcity.projectName"); }
        }

        public string TeamCityVersion
        {
            get { return GetParameterValue("teamcity.version"); }
        }

        public void SetParameterValue(string parameterName, string parameterValue)
        {
            if (!m_Parameters.ContainsKey(parameterName) && m_IsTeamCityMode)
            {
                throw new MissingBuildParameterException(parameterName);
            }

            m_Parameters[parameterName] = parameterValue;
            m_TeamCityWriter.WriteBuildParameter(parameterName, parameterValue);
        }
    }
}