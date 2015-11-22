using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;

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
    }

    internal class BuildParameters : IBuildParameters
    {
        private const string TeamcityBuildPropertiesFile = "TEAMCITY_BUILD_PROPERTIES_FILE";

        private readonly Dictionary<string, string> m_Parameters;

        public BuildParameters()
            : this(Environment.GetEnvironmentVariable(TeamcityBuildPropertiesFile), new FileSystem())
        {
        }

        internal BuildParameters(string teamCityBuildPropertiesFile, IFileSystem fileSystem)
        {
            m_Parameters = teamCityBuildPropertiesFile == null ? new Dictionary<string, string>() : 
                fileSystem.File.ReadAllLines(teamCityBuildPropertiesFile)
                    .Select(ParseLine)
                    .Where(t => t != null)
                    .ToDictionary(t => t.Item1, t => t.Item2);
        }

        private static Tuple<string, string> ParseLine(string line)
        {
            if (string.IsNullOrEmpty(line)) return null;
            if (line.StartsWith("#")) return null;

            var indexOf = line.IndexOf("=", StringComparison.InvariantCultureIgnoreCase);
            if (indexOf < 0) return null;
            var parameterName = line.Substring(0, indexOf);
            var parameterValue = line.Substring(indexOf + 1, line.Length - indexOf - 1);
            return new Tuple<string, string>(parameterName, parameterValue.Replace(@"\:",@":").Replace(@"\\", @"\"));
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
            get { throw new NotImplementedException(); }
        }
    }
}