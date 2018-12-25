using System.Collections.Generic;
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
        T GetBuildParameter<T>(string parameterName);
        bool TryGetBuildParameter(string parameterName, out string parameterValue);
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
        IList<IChangedFile> ChangedFiles { get; }
        bool IsTeamCityMode { get; }
        bool IsPersonal { get; }
        void SetBuildParameter(string parameterName, string parameterValue);

        /// <summary>
        /// Attaches new artifact publishing rules as described in
        /// http://confluence.jetbrains.net/display/TCD7/Build+Artifact
        /// </summary>
        /// <param name="fileDirectoryName">
        /// Filename to publish. The file name should be relative to the build checkout directory.
        /// Directory name to publish all the files and subdirectories within the directory specified. The directory name should be a path relative to the build checkout directory. The files will be published preserving the directories structure under the directory specified (the directory itself will not be included).
        /// </param>
        void PublishArtifact(string fileDirectoryName);

        /// <summary>
        /// Attaches new artifact publishing rules as described in
        /// http://confluence.jetbrains.net/display/TCD7/Build+Artifact
        /// </summary>
        /// <param name="fileDirectoryName">
        /// Filename to publish. The file name should be relative to the build checkout directory.
        /// Directory name to publish all the files and subdirectories within the directory specified. The directory name should be a path relative to the build checkout directory. The files will be published preserving the directories structure under the directory specified (the directory itself will not be included).
        /// </param>
        /// <param name="targetDirectoryArchive">
        /// Target directory - the directory in the resulting build's artifacts that will contain the files determined by the left part of the pattern.
        /// Target archive - the path to the archive to be created by TeamCity by packing build artifacts.
        /// TeamCity treats <paramref name="targetDirectoryArchive"/> as archive whenever it ends with a supported archive extension, i.e. .zip, .jar, .tar.gz, or .tgz.
        /// </param>
        void PublishArtifact(string fileDirectoryName, string targetDirectoryArchive);
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

            string changedFilesPath;
            if (m_BuildParameters.TryGetBuildParameter("build.changedFiles.file", out changedFilesPath))
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

        public T GetBuildParameter<T>(string buildParameterName)
        {
            return m_BuildParameters.GetBuildParameter<T>(buildParameterName);
        }

        public bool TryGetBuildParameter(string parameterName, out string parameterValue)
        {
            return m_BuildParameters.TryGetBuildParameter(parameterName, out parameterValue);
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

        public long BuildNumber
        {
            get { return m_BuildParameters.BuildNumber; }
        }

        public int TeamcityAgentCpuBenchmark
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

        public long TeamcityBuildId
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

        public bool IsPersonal
        {
            get { return m_BuildParameters.IsPersonal; }
        }

        /// <summary>
        /// Attaches new artifact publishing rules as described in
        /// http://confluence.jetbrains.net/display/TCD7/Build+Artifact
        /// </summary>
        /// <param name="fileDirectoryName">
        /// Filename to publish. The file name should be relative to the build checkout directory.
        /// Directory name to publish all the files and subdirectories within the directory specified. The directory name should be a path relative to the build checkout directory. The files will be published preserving the directories structure under the directory specified (the directory itself will not be included).
        /// </param>
        public void PublishArtifact(string fileDirectoryName)
        {
            m_TeamCityWriter.PublishArtifact(fileDirectoryName);
        }

        /// <summary>
        /// Attaches new artifact publishing rules as described in
        /// http://confluence.jetbrains.net/display/TCD7/Build+Artifact
        /// </summary>
        /// <param name="fileDirectoryName">
        /// Filename to publish. The file name should be relative to the build checkout directory.
        /// Directory name to publish all the files and subdirectories within the directory specified. The directory name should be a path relative to the build checkout directory. The files will be published preserving the directories structure under the directory specified (the directory itself will not be included).
        /// </param>
        /// <param name="targetDirectoryArchive">
        /// Target directory - the directory in the resulting build's artifacts that will contain the files determined by the left part of the pattern.
        /// Target archive - the path to the archive to be created by TeamCity by packing build artifacts.
        /// TeamCity treats <paramref name="targetDirectoryArchive"/> as archive whenever it ends with a supported archive extension, i.e. .zip, .jar, .tar.gz, or .tgz.
        /// </param>
        public void PublishArtifact(string fileDirectoryName, string targetDirectoryArchive)
        {
            m_TeamCityWriter.PublishArtifact($"{fileDirectoryName} => {targetDirectoryArchive}");
        }
    }
}