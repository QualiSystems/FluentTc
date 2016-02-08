﻿using System.Collections.Generic;
using FluentTc.Domain;
using FluentTc.Engine;
using FluentTc.Locators;
using JetBrains.TeamCity.ServiceMessages.Write;
using JetBrains.TeamCity.ServiceMessages.Write.Special;

namespace FluentTc
{
    public interface ILocalTc
    {
        void ChangeBuildStatus(BuildStatus buildStatus);
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
        IList<IChangedFile> ChangedFiles { get; }
        bool IsTeamCityMode { get; }
        void SetBuildParameter(string parameterName, string parameterValue);
    }

    public class LocalTc : ILocalTc
    {
        private readonly IBuildParameters m_BuildParameters;
        private readonly ITeamCityWriter m_TeamCityWriter;
        private readonly IList<IChangedFile> m_ChangedFiles;

        public LocalTc() : this(null)
        {
        }

        internal LocalTc(IBuildParameters buildParameters = null, ITeamCityWriterFactory teamCityWriterFactory = null, params object[] overrides)
        {
            var bootstrapper = new Bootstrapper(overrides);
            m_BuildParameters = buildParameters ?? bootstrapper.Get<IBuildParameters>();
            teamCityWriterFactory = teamCityWriterFactory ?? bootstrapper.Get<ITeamCityWriterFactory>();
            m_TeamCityWriter = teamCityWriterFactory.CreateTeamCityWriter();

            var changedFilesPath = GetBuildParameter("build.changedFiles.file");
            if (!string.IsNullOrEmpty(changedFilesPath))
            {
                m_ChangedFiles = bootstrapper.Get<IChangedFilesParser>().ParseChangedFiles(changedFilesPath);
            }
            else
            {
                m_ChangedFiles = new List<IChangedFile>();
            }
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
            return m_BuildParameters.GetBuildParameter(buildParameterName);
        }

        public void SetBuildParameter(string buildParameterName, string buildParameterValue)
        {
            m_BuildParameters.SetBuildParameter(buildParameterName, buildParameterValue);
        }

        public string AgentHomeDir
        {
            get { return m_BuildParameters.AgentHomeDir; }
        }

        public string AgentName
        {
            get { return m_BuildParameters.AgentName; }
        }

        public string AgentOwnPort
        {
            get { return m_BuildParameters.AgentOwnPort; }
        }

        public string AgentWorkDir
        {
            get { return m_BuildParameters.AgentWorkDir; }
        }

        public string BuildNumber
        {
            get { return m_BuildParameters.BuildNumber; }
        }

        public string TeamcityAgentCpuBenchmark
        {
            get { return m_BuildParameters.TeamcityAgentCpuBenchmark; }
        }

        public string TeamcityBuildChangedFilesFile
        {
            get { return m_BuildParameters.TeamcityBuildChangedFilesFile; }
        }

        public string TeamcityBuildCheckoutDir
        {
            get { return m_BuildParameters.TeamcityBuildCheckoutDir; }
        }

        public string TeamcityBuildId
        {
            get { return m_BuildParameters.TeamcityBuildId; }
        }

        public string TeamcityBuildTempDir
        {
            get { return m_BuildParameters.TeamcityBuildTempDir; }
        }

        public string TeamcityBuildWorkingDir
        {
            get { return m_BuildParameters.TeamcityBuildWorkingDir; }
        }

        public string TeamcityBuildConfName
        {
            get { return m_BuildParameters.TeamcityBuildConfName; }
        }

        public string TeamcityBuildTypeId
        {
            get { return m_BuildParameters.TeamcityBuildTypeId; }
        }

        public string TeamcityProjectName
        {
            get { return m_BuildParameters.TeamcityProjectName; }
        }

        public string TeamCityVersion
        {
            get { return m_BuildParameters.TeamCityVersion; }
        }

        public IList<IChangedFile> ChangedFiles
        {
            get { return m_ChangedFiles; }
        }

        public bool IsTeamCityMode
        {
            get { return m_BuildParameters.IsTeamCityMode; }
        }
    }
}