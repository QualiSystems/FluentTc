using System.Collections.Generic;
using FluentTc.Exceptions;
using JetBrains.TeamCity.ServiceMessages.Write.Special;

namespace FluentTc.Engine
{
    public interface IBuildParameters
    {
        string GetBuildParameter(string parameterName);
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
        void SetBuildParameter(string parameterName, string parameterValue);
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

        public string GetBuildParameter(string parameterName)
        {
            string parameterValue;
            if (!m_Parameters.TryGetValue(parameterName, out parameterValue))
            {
                throw new MissingBuildParameterException(parameterName);
            }
            return parameterValue;
        }

        public string AgentHomeDir
        {
            get { return GetBuildParameter("agent.home.dir"); }
        }

        public string AgentName
        {
            get { return GetBuildParameter("agent.name"); }
        }

        public string AgentOwnPort
        {
            get { return GetBuildParameter("agent.ownPort"); }
        }

        public string AgentWorkDir
        {
            get { return GetBuildParameter("agent.work.dir"); }
        }

        public string BuildNumber
        {
            get { return GetBuildParameter("build.number"); }
        }

        public string TeamcityAgentCpuBenchmark
        {
            get { return GetBuildParameter("teamcity.agent.cpuBenchmark"); }
        }

        public string TeamcityBuildChangedFilesFile
        {
            get { return GetBuildParameter("teamcity.build.changedFiles.file"); }
        }

        public string TeamcityBuildCheckoutDir
        {
            get { return GetBuildParameter("teamcity.build.checkoutDir"); }
        }

        public string TeamcityBuildId
        {
            get { return GetBuildParameter("teamcity.build.id"); }
        }

        public string TeamcityBuildTempDir
        {
            get { return GetBuildParameter("teamcity.build.tempDir"); }
        }

        public string TeamcityBuildWorkingDir
        {
            get { return GetBuildParameter("teamcity.build.workingDir"); }
        }

        public string TeamcityBuildConfName
        {
            get { return GetBuildParameter("teamcity.buildConfName"); }
        }

        public string TeamcityBuildTypeId
        {
            get { return GetBuildParameter("teamcity.buildType.id"); }
        }

        public string TeamcityProjectName
        {
            get { return GetBuildParameter("teamcity.projectName"); }
        }

        public string TeamCityVersion
        {
            get { return GetBuildParameter("teamcity.version"); }
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
    }
}