using System;
using System.Collections.Generic;
using System.Linq;
using FluentTc.Domain;
using FluentTc.Engine;
using FluentTc.Exceptions;
using FluentTc.Locators;

namespace FluentTc
{
    public interface IConnectedTc
    {
        /// <summary>
        /// Retrieves builds matching the criteria
        /// </summary>
        /// <param name="having">Criteria to retrieve builds</param>
        /// <returns>Builds matching the criteria</returns>
        IList<IBuild> GetBuilds(Action<IBuildHavingBuilder> having);

        /// <summary>
        /// Retrieves builds matching the criteria
        /// </summary>
        /// <param name="having">Criteria to retrieve builds</param>
        /// <param name="include">Specifies which additional properties to retrieve</param>
        /// <returns>Builds matching the criteria</returns>
        IList<IBuild> GetBuilds(Action<IBuildHavingBuilder> having, Action<IBuildIncludeBuilder> include);

        /// <summary>
        /// Retrieves builds matching the criteria
        /// </summary>
        /// <param name="having">Criteria to retrieve builds</param>
        /// <param name="include">Specifies which additional properties to retrieve</param>
        /// <param name="count">Allow retrieving specific amount of results with paging</param>
        /// <returns>Builds matching the criteria</returns>
        IList<IBuild> GetBuilds(Action<IBuildHavingBuilder> having, Action<IBuildIncludeBuilder> include, Action<ICountBuilder> count);

        /// <summary>
        ///     Retrieves a build according to specified having parameter with specified columns
        /// </summary>
        /// <exception cref="FluentTc.Exceptions.BuildNotFoundException">Thrown when build not found by the specified criteria</exception>
        /// <exception cref="FluentTc.Exceptions.MoreThanOneBuildFoundException">
        ///     Thrown when more than one build found by the
        ///     specified criteria
        /// </exception>
        /// <param name="having">Retrieve build that matches the criteria</param>
        /// <param name="include">Include these columns in retrieved build</param>
        /// <returns>IBuild</returns>
        IBuild GetBuild(Action<IBuildHavingBuilder> having, Action<IBuildIncludeBuilder> include);

        /// <summary>
        ///     Retrieves a build according to specified having parameter with specified columns
        /// </summary>
        /// <exception cref="FluentTc.Exceptions.BuildNotFoundException">Thrown when build not found by the specified criteria</exception>
        /// <exception cref="FluentTc.Exceptions.MoreThanOneBuildFoundException">
        ///     Thrown when more than one build found by the
        ///     specified criteria
        /// </exception>
        /// <param name="having">Retrieve build that matches the criteria</param>
        /// <returns>IBuild</returns>
        IBuild GetBuild(Action<IBuildHavingBuilder> having);

        /// <summary>
        ///     Retrieves the last build that matches having parameter with all the data.
        /// </summary>
        /// <exception cref="FluentTc.Exceptions.BuildNotFoundException">Thrown when build not found by the specified criteria</exception>
        /// <exception cref="FluentTc.Exceptions.MoreThanOneBuildFoundException">
        ///     Thrown when more than one build found by the
        ///     specified criteria
        /// </exception>
        /// <param name="having">Retrieve build that matches the criteria</param>
        /// <returns>IBuild</returns>
        IBuild GetLastBuild(Action<IBuildHavingBuilder> having);

        List<Agent> GetAgents(Action<IAgentHavingBuilder> having);

        /// <summary>
        ///     Retrieves the last build that matches having parameter with all the data.
        /// </summary>
        /// <exception cref="FluentTc.Exceptions.BuildNotFoundException">Thrown when build not found by the specified criteria</exception>
        /// <exception cref="FluentTc.Exceptions.MoreThanOneBuildFoundException">
        ///     Thrown when more than one build found by the
        ///     specified criteria
        /// </exception>
        /// <param name="having">Retrieve build that matches the criteria</param>
        /// <param name="include">Include additional data, such as Changes</param>
        /// <returns>IBuild</returns>
        IBuild GetLastBuild(Action<IBuildHavingBuilder> having, Action<IBuildAdditionalIncludeBuilder> include);

        IBuild GetBuild(long buildId);

        BuildConfiguration GetBuildConfiguration(Action<IBuildConfigurationHavingBuilder> having);
        IList<BuildConfiguration> GetBuildConfigurations(Action<IBuildConfigurationHavingBuilder> having);

        void SetBuildConfigurationParameters(Action<IBuildConfigurationHavingBuilder> having,
            Action<IBuildParameterValueBuilder> parameters);
        void SetBuildConfigurationField(Action<IBuildConfigurationHavingBuilder> having,
            Action<IBuildFieldValueBuilder> parameters);

        void SetProjectConfigurationField(Action<IBuildProjectHavingBuilder> having,
            Action<IBuildProjectFieldValueBuilder> fields);

        void SetProjectParameters(Action<IBuildProjectHavingBuilder> having, Action<IBuildParameterValueBuilder> parameters);

        IBuild RunBuildConfiguration(Action<IBuildConfigurationHavingBuilder> having,
            Action<IBuildParameterValueBuilder> parameters);

        IBuild RunBuildConfiguration(Action<IBuildConfigurationHavingBuilder> having, Action<IAgentHavingBuilder> onAgent);

        IBuild RunBuildConfiguration(Action<IBuildConfigurationHavingBuilder> having, Action<IAgentHavingBuilder> onAgent,
            Action<IBuildParameterValueBuilder> parameters);

        IBuild RunBuildConfiguration(Action<IBuildConfigurationHavingBuilder> having);

        IBuild RunBuildConfiguration(Action<IBuildConfigurationHavingBuilder> having, Action<IMoreOptionsHavingBuilder> moreOptions);

        IBuild RunBuildConfiguration(Action<IBuildConfigurationHavingBuilder> having, 
            Action<IBuildParameterValueBuilder> parameters, Action<IMoreOptionsHavingBuilder> moreOptions);

        BuildConfiguration CreateBuildConfiguration(Action<IBuildProjectHavingBuilder> having,
            string buildConfigurationName);

        IList<IBuild> GetBuildsQueue(Action<IQueueHavingBuilder> having = null);
        void RemoveBuildFromQueue(Action<IQueueHavingBuilder> having);
        void RemoveBuildFromQueue(Action<IBuildQueueIdHavingBuilder> having);
        IList<Project> GetProjects(Action<IBuildProjectHavingBuilder> having);
        void DisableAgent(Action<IAgentHavingBuilder> having);
        void EnableAgent(Action<IAgentHavingBuilder> having);
        void AttachBuildConfigurationToTemplate(Action<IBuildConfigurationHavingBuilder> having, string buildTemplateId);
        Project GetProjectById(string projectId);
        IList<BuildConfiguration> GetBuildConfigurationsRecursively(string projectId);
        IList<Project> GetAllProjects();
        IList<string> DownloadArtifacts(int buildId, string destinationPath);
        string DownloadArtifact(int buildId, string destinationPath, string fileToDownload);
        Investigation GetInvestigation(Action<IBuildConfigurationHavingBuilder> havingBuildConfig);
        Investigation GetTestinvestigationByTestNameId(string testNameId);
        List<User> GetAllUsers();
        User GetUser(Action<IUserHavingBuilder> having);
        Project CreateProject(Action<INewProjectDetailsBuilder> newProjectDetailsBuilderAction);
        List<BuildConfiguration> GetAllBuildConfigurationTemplates();
        BuildConfiguration GetBuildConfigurationTemplate(Action<IBuildConfigurationTemplateHavingBuilder> having);

        /// <summary>
        /// Deletes build parameter from build configuration or build configuration template
        /// </summary>
        /// <param name="project">Project to delete parameter from</param>
        /// <param name="parameterName">Parameter name to be deleted</param>
        void DeleteProjectParameter(Action<IBuildProjectHavingBuilder> project, Action<IBuildParameterHavingBuilder> parameterName);

        /// <summary>
        /// Deletes build parameter from build configuration or build configuration template
        /// </summary>
        /// <param name="buildConfigurationOrTemplate">IBuild configuration or template to delete parameter from</param>
        /// <param name="parameterName">Parameter name to be deleted</param>
        void DeleteBuildConfigurationParameter(Action<IBuildConfigurationHavingBuilder> buildConfigurationOrTemplate, Action<IBuildParameterHavingBuilder> parameterName);

        /// <summary>
        /// Retrieves build statistics according to the provided build criteria
        /// </summary>
        /// <param name="having">Build criteria</param>
        /// <returns>List of build statistics</returns>
        IList<IBuildStatistic> GetBuildStatistics(Action<IBuildHavingBuilder> having);

        /// <summary>
        /// Creates a VCS root.
        /// </summary>
        /// <param name="project">The project that will contain the VCS Root.</param>
        /// <param name="vcsRoot">The VCS root data.</param>
        /// <returns></returns>
        VcsRoot CreateVcsRoot(Project project, Action<IGitVCSRootBuilder> vcsRoot);

        /// <summary>
        /// Attaches the VCS root to a build configuration.
        /// </summary>
        /// <param name="having">The having.</param>
        /// <param name="vcsRootEntry">The VCS root entry.</param>
        void AttachVcsRootToBuildConfiguration(Action<IBuildConfigurationHavingBuilder> having, VcsRoot vcsRoot);
    }

    internal class ConnectedTc : IConnectedTc
    {
        private readonly IAgentEnabler m_AgentEnabler;
        private readonly IAgentsRetriever m_AgentsRetriever;
        private readonly IArtifactsDownloader m_ArtifactsDownloader;
        private readonly IBuildConfigurationCreator m_BuildConfigurationCreator;
        private readonly IBuildConfigurationRetriever m_BuildConfigurationRetriever;
        private readonly IBuildConfigurationRunner m_BuildConfigurationRunner;
        private readonly IBuildQueueRemover m_BuildQueueRemover;
        private readonly IBuildsRetriever m_BuildsRetriever;
        private readonly IBuildTemplateAttacher m_BuildTemplateAttacher;
        private readonly IInvestigationRetriever m_InvestigationRetriever;
        private readonly IProjectsRetriever m_ProjectsRetriever;
        private readonly IUserRetriever m_UserRetriever;
        private readonly IProjectCreator m_ProjectCreator;
        private readonly IChangesRetriever m_ChangesRetriever;
        private readonly IBuildConfigurationTemplateRetriever m_BuildConfigurationTemplateRetriever;
        private readonly IProjectPropertySetter m_ProjectPropertySetter;
        private readonly IBuildStatisticsRetriever m_StatisticsRetriever;
        private readonly IVCSRootCreator m_VcsRootCreator;
        private readonly IVCSRootAttacher m_VcsRootAttacher;

        public ConnectedTc(IBuildsRetriever buildsRetriever,
            IAgentsRetriever agentsRetriever,
            IProjectsRetriever projectsRetriever,
            IBuildConfigurationRetriever buildConfigurationRetriever,
            IAgentEnabler agentEnabler,
            IBuildConfigurationRunner buildConfigurationRunner,
            IBuildConfigurationCreator buildConfigurationCreator,
            IBuildTemplateAttacher buildTemplateAttacher,
            IBuildQueueRemover buildQueueRemover,
            IArtifactsDownloader artifactsDownloader,
            IInvestigationRetriever investigationRetriever, 
            IUserRetriever userRetriever, 
            IProjectCreator projectCreator, 
            IProjectPropertySetter projectPropertySetter, 
            IBuildConfigurationTemplateRetriever buildConfigurationTemplateRetriever,
            IChangesRetriever changesRetriever,
            IBuildStatisticsRetriever statisticsRetriever,
            IVCSRootCreator vcsRootCreator,
            IVCSRootAttacher vcsRootAttacher)
        {
            m_BuildsRetriever = buildsRetriever;
            m_AgentsRetriever = agentsRetriever;
            m_ProjectsRetriever = projectsRetriever;
            m_BuildConfigurationRetriever = buildConfigurationRetriever;
            m_AgentEnabler = agentEnabler;
            m_BuildConfigurationRunner = buildConfigurationRunner;
            m_BuildConfigurationCreator = buildConfigurationCreator;
            m_BuildTemplateAttacher = buildTemplateAttacher;
            m_BuildQueueRemover = buildQueueRemover;
            m_ArtifactsDownloader = artifactsDownloader;
            m_InvestigationRetriever = investigationRetriever;
            m_UserRetriever = userRetriever;
            m_ProjectCreator = projectCreator;
            m_ProjectPropertySetter = projectPropertySetter;
            m_BuildConfigurationTemplateRetriever = buildConfigurationTemplateRetriever;
            m_ChangesRetriever = changesRetriever;
            m_StatisticsRetriever = statisticsRetriever;
            m_VcsRootCreator = vcsRootCreator;
            m_VcsRootAttacher = vcsRootAttacher; 
        }

        public IList<IBuild> GetBuilds(Action<IBuildHavingBuilder> having)
        {
            return m_BuildsRetriever.GetBuilds(having, _ => _.DefaultCount(), _ => _.IncludeDefaults());
        }

        public IList<IBuild> GetBuilds(Action<IBuildHavingBuilder> having, Action<IBuildIncludeBuilder> include)
        {
            return m_BuildsRetriever.GetBuilds(having, _ => _.DefaultCount(), include);
        }

        public IList<IBuild> GetBuilds(Action<IBuildHavingBuilder> having, Action<IBuildIncludeBuilder> include, Action<ICountBuilder> count)
        {
            return m_BuildsRetriever.GetBuilds(having, count, include);
        }

        public List<Agent> GetAgents(Action<IAgentHavingBuilder> having)
        {
            return m_AgentsRetriever.GetAgents(having);
        }

        public IBuild GetBuild(Action<IBuildHavingBuilder> having, Action<IBuildIncludeBuilder> include)
        {
            var builds = GetBuilds(having, include);
            if (!builds.Any()) throw new BuildNotFoundException();
            if (builds.Count() > 1) throw new MoreThanOneBuildFoundException();
            return builds.Single();
        }

        public IBuild GetBuild(Action<IBuildHavingBuilder> having)
        {
            return GetBuild(having, _ => _.IncludeDefaults());
        }

        public IBuild GetLastBuild(Action<IBuildHavingBuilder> having)
        {
            return GetLastBuild(having, _ => { });
        }

        public IBuild GetLastBuild(Action<IBuildHavingBuilder> having, Action<IBuildAdditionalIncludeBuilder> include)
        {
            var builds = GetBuilds(having, _ => _.IncludeDefaults(), __ => __.Count(1));
            if (!builds.Any()) throw new BuildNotFoundException();
            var lastBuild = GetBuild(builds.First().Id);
            var buildAdditionalIncludeBuilder = new BuildAdditionalIncludeBuilder();
            include(buildAdditionalIncludeBuilder);
            if (buildAdditionalIncludeBuilder.ShouldIncludeChanges)
            {
                var changes = m_ChangesRetriever.GetChanges(_ => _.Build(__ => __.Id(lastBuild.Id)), 
                    buildAdditionalIncludeBuilder.ChangesInclude);
                lastBuild.SetChanges(changes);
            }
            return lastBuild;
        }

        public IBuild GetBuild(long buildId)
        {
            return m_BuildsRetriever.GetBuild(buildId);
        }

        public BuildConfiguration GetBuildConfiguration(Action<IBuildConfigurationHavingBuilder> having)
        {
            return m_BuildConfigurationRetriever.GetSingleBuildConfiguration(having);
        }

        public IList<BuildConfiguration> GetBuildConfigurations(Action<IBuildConfigurationHavingBuilder> having)
        {
            return m_BuildConfigurationRetriever.RetrieveBuildConfigurations(having);
        }

        public void SetBuildConfigurationParameters(Action<IBuildConfigurationHavingBuilder> having,
            Action<IBuildParameterValueBuilder> parameters)
        {
            m_BuildConfigurationRetriever.SetParameters(having, parameters);
        }

        public void SetBuildConfigurationField(Action<IBuildConfigurationHavingBuilder> having,
            Action<IBuildFieldValueBuilder> fields)
        {
            m_BuildConfigurationRetriever.SetFields(having, fields);
        }

        public void SetProjectConfigurationField(Action<IBuildProjectHavingBuilder> having,
            Action<IBuildProjectFieldValueBuilder> fields)
        {
            m_ProjectsRetriever.SetFields(having, fields);
        }


        public void SetProjectParameters(Action<IBuildProjectHavingBuilder> having, Action<IBuildParameterValueBuilder> parameters)
        {
            m_ProjectPropertySetter.SetProjectParameters(having, parameters);
        }

        public IBuild RunBuildConfiguration(Action<IBuildConfigurationHavingBuilder> having,
            Action<IBuildParameterValueBuilder> parameters)
        {
            return m_BuildConfigurationRunner.Run(having, parameters: parameters);
        }

        public IBuild RunBuildConfiguration(Action<IBuildConfigurationHavingBuilder> having,
            Action<IAgentHavingBuilder> onAgent)
        {
            return m_BuildConfigurationRunner.Run(having, onAgent);
        }

        public IBuild RunBuildConfiguration(Action<IBuildConfigurationHavingBuilder> having, Action<IMoreOptionsHavingBuilder> moreOptions)
        {
            return m_BuildConfigurationRunner.Run(having, moreOptions: moreOptions);
        }

        public IBuild RunBuildConfiguration(Action<IBuildConfigurationHavingBuilder> having, Action<IBuildParameterValueBuilder> parameters,
            Action<IMoreOptionsHavingBuilder> moreOptions)
        {
            return m_BuildConfigurationRunner.Run(having, parameters: parameters, moreOptions: moreOptions);
        }

        public IBuild RunBuildConfiguration(Action<IBuildConfigurationHavingBuilder> having,
            Action<IAgentHavingBuilder> onAgent, Action<IBuildParameterValueBuilder> parameters)
        {
            return m_BuildConfigurationRunner.Run(having, onAgent, parameters);
        }

        public IBuild RunBuildConfiguration(Action<IBuildConfigurationHavingBuilder> having)
        {
            return m_BuildConfigurationRunner.Run(having);
        }

        public BuildConfiguration CreateBuildConfiguration(Action<IBuildProjectHavingBuilder> having,
            string buildConfigurationName)
        {
            return m_BuildConfigurationCreator.Create(having, buildConfigurationName);
        }

        public void AttachBuildConfigurationToTemplate(Action<IBuildConfigurationHavingBuilder> having,
            string buildTemplateId)
        {
            m_BuildTemplateAttacher.Attach(having, buildTemplateId);
        }

        public IList<IBuild> GetBuildsQueue(Action<IQueueHavingBuilder> having = null)
        {
            return m_BuildsRetriever.GetBuildsQueue(having);
        }

        public void RemoveBuildFromQueue(Action<IQueueHavingBuilder> having)
        {
            m_BuildQueueRemover.RemoveBuildFromQueue(having);
        }

        public void RemoveBuildFromQueue(Action<IBuildQueueIdHavingBuilder> having)
        {
            m_BuildQueueRemover.RemoveBuildFromQueue(having);
        }

        public Project GetProjectById(string projectId)
        {
            return m_ProjectsRetriever.GetProject(projectId);
        }

        public IList<Project> GetProjects(Action<IBuildProjectHavingBuilder> having)
        {
            return m_ProjectsRetriever.GetProjects(having);
        }

        public void DisableAgent(Action<IAgentHavingBuilder> having)
        {
            m_AgentEnabler.DisableAgent(having);
        }

        public void EnableAgent(Action<IAgentHavingBuilder> having)
        {
            m_AgentEnabler.EnableAgent(having);
        }

        public IList<BuildConfiguration> GetBuildConfigurationsRecursively(string projectId)
        {
            var project = GetProjectById(projectId);
            if (project.Projects.Project == null || !project.Projects.Project.Any())
                return project.BuildTypes.BuildType;

            return project.BuildTypes.BuildType.Concat(
                project.Projects.Project.SelectMany(p => GetBuildConfigurationsRecursively(p.Id)))
                .ToList();
        }

        public IList<Project> GetAllProjects()
        {
            return m_ProjectsRetriever.GetProjects();
        }

        public IList<string> DownloadArtifacts(int buildId, string destinationPath)
        {
            return m_ArtifactsDownloader.DownloadArtifacts(buildId, destinationPath);
        }

        public string DownloadArtifact(int buildId, string destinationPath, string fileToDownload)
        {
            return m_ArtifactsDownloader.DownloadArtifact(buildId, destinationPath, fileToDownload);
        }

        public Investigation GetInvestigation(Action<IBuildConfigurationHavingBuilder> havingBuildConfig)
        {
            return m_InvestigationRetriever.GetInvestigation(havingBuildConfig);
        }

        /// <summary>
        ///     gets the investigator of the test
        /// </summary>
        /// <param name="testNameId">testNameId </param>
        /// <returns></returns>
        public Investigation GetTestinvestigationByTestNameId(string testNameId)
        {
            return m_InvestigationRetriever.GetTestInvestigationByTestNameId(testNameId);
        }

        public List<User> GetAllUsers()
        {
            return m_UserRetriever.GetAllUsers();
        }

        public User GetUser(Action<IUserHavingBuilder> having)
        {
            return m_UserRetriever.GetUser(having);
        }

        public Project CreateProject(Action<INewProjectDetailsBuilder> newProjectDetailsBuilderAction)
        {
            return m_ProjectCreator.CreateProject(newProjectDetailsBuilderAction);
        }

        public List<BuildConfiguration> GetAllBuildConfigurationTemplates()
        {
            return m_BuildConfigurationTemplateRetriever.GetAllBuildConfigurationTemplates();
        }

        public BuildConfiguration GetBuildConfigurationTemplate(Action<IBuildConfigurationTemplateHavingBuilder> having)
        {
            return m_BuildConfigurationTemplateRetriever.GetBuildConfigurationTemplate(having);
        }

        /// <summary>
        /// Deletes build parameter from build configuration or build configuration template
        /// </summary>
        /// <param name="project">Project to delete parameter from</param>
        /// <param name="parameterName">Parameter name to be deleted</param>
        public void DeleteProjectParameter(Action<IBuildProjectHavingBuilder> project, Action<IBuildParameterHavingBuilder> parameterName)
        {
            m_ProjectPropertySetter.DeleteProjectParameter(project, parameterName);
        }

        /// <summary>
        /// Deletes build parameter from build configuration or build configuration template
        /// </summary>
        /// <param name="buildConfigurationOrTemplate">IBuild configuration or template to delete parameter from</param>
        /// <param name="parameterName">Parameter name to be deleted</param>
        public void DeleteBuildConfigurationParameter(Action<IBuildConfigurationHavingBuilder> buildConfigurationOrTemplate, Action<IBuildParameterHavingBuilder> parameterName)
        {
            m_BuildConfigurationRetriever.DeleteBuildConfigurationParameter(buildConfigurationOrTemplate, parameterName);
        }

        /// <summary>
        /// Retrieves build statistics according to the provided build criteria
        /// </summary>
        /// <param name="having">Build criteria</param>
        /// <returns>List of build statistics</returns>
        public IList<IBuildStatistic> GetBuildStatistics(Action<IBuildHavingBuilder> having)
        {
            return m_StatisticsRetriever.GetBuildStatistics(having);
        }

        /// <summary>
        /// Creates a VCS root.
        /// </summary>
        /// <param name="project">The project that will contain the VCS Root.</param>
        /// <param name="vcsRoot">The VCS root data.</param>
        /// <returns></returns>
        public VcsRoot CreateVcsRoot(Project project, Action<IGitVCSRootBuilder> vcsRoot)
        {
            return m_VcsRootCreator.Create(project, vcsRoot);
        }

        /// <summary>
        /// Attaches the VCS root to a build configuration.
        /// </summary>
        /// <param name="having">The having.</param>
        /// <param name="vcsRootEntry">The VCS root entry.</param>
        public void AttachVcsRootToBuildConfiguration(Action<IBuildConfigurationHavingBuilder> having, VcsRoot vcsRoot)
        {
            m_VcsRootAttacher.Attach(having, vcsRoot);
        }

    }
}
