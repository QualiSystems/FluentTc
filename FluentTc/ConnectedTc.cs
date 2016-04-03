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
        List<Build> GetBuilds(Action<IBuildHavingBuilder> having);
        List<Build> GetBuilds(Action<IBuildHavingBuilder> having, Action<IBuildIncludeBuilder> include);

        List<Build> GetBuilds(Action<IBuildHavingBuilder> having, Action<IBuildIncludeBuilder> include,
            Action<ICountBuilder> count);

        List<Agent> GetAgents(Action<IAgentHavingBuilder> having);

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
        /// <returns>Build</returns>
        Build GetBuild(Action<IBuildHavingBuilder> having, Action<IBuildIncludeBuilder> include);

        /// <summary>
        ///     Retrieves a build according to specified having parameter with specified columns
        /// </summary>
        /// <exception cref="FluentTc.Exceptions.BuildNotFoundException">Thrown when build not found by the specified criteria</exception>
        /// <exception cref="FluentTc.Exceptions.MoreThanOneBuildFoundException">
        ///     Thrown when more than one build found by the
        ///     specified criteria
        /// </exception>
        /// <param name="having">Retrieve build that matches the criteria</param>
        /// <returns>Build</returns>
        Build GetBuild(Action<IBuildHavingBuilder> having);

        /// <summary>
        ///     Retrieves the last build that matches having parameter with all the data.
        /// </summary>
        /// <exception cref="FluentTc.Exceptions.BuildNotFoundException">Thrown when build not found by the specified criteria</exception>
        /// <exception cref="FluentTc.Exceptions.MoreThanOneBuildFoundException">
        ///     Thrown when more than one build found by the
        ///     specified criteria
        /// </exception>
        /// <param name="having">Retrieve build that matches the criteria</param>
        /// <returns>Build</returns>
        Build GetLastBuild(Action<IBuildHavingBuilder> having);

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
        /// <returns>Build</returns>
        Build GetLastBuild(Action<IBuildHavingBuilder> having, Action<IBuildAdditionalIncludeBuilder> include);

        Build GetBuild(long buildId);

        BuildConfiguration GetBuildConfiguration(Action<IBuildConfigurationHavingBuilder> having);
        IList<BuildConfiguration> GetBuildConfigurations(Action<IBuildConfigurationHavingBuilder> having);

        void SetParameters(Action<IBuildConfigurationHavingBuilder> having,
            Action<IBuildParameterValueBuilder> parameters);

        void RunBuildConfiguration(Action<IBuildConfigurationHavingBuilder> having,
            Action<IBuildParameterValueBuilder> parameters);

        void RunBuildConfiguration(Action<IBuildConfigurationHavingBuilder> having, Action<IAgentHavingBuilder> onAgent);

        void RunBuildConfiguration(Action<IBuildConfigurationHavingBuilder> having, Action<IAgentHavingBuilder> onAgent,
            Action<IBuildParameterValueBuilder> parameters);

        void RunBuildConfiguration(Action<IBuildConfigurationHavingBuilder> having);

        BuildConfiguration CreateBuildConfiguration(Action<IBuildProjectHavingBuilder> having,
            string buildConfigurationName);

        List<Build> GetBuildsQueue(Action<IQueueHavingBuilder> having = null);
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
        private readonly IChangesRetriever m_ChangesRetriever;

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
            IInvestigationRetriever investigationRetriever, IUserRetriever userRetriever, IChangesRetriever changesRetriever)
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
            m_ChangesRetriever = changesRetriever;
        }

        public List<Build> GetBuilds(Action<IBuildHavingBuilder> having)
        {
            return m_BuildsRetriever.GetBuilds(having, _ => _.DefaultCount(), _ => _.IncludeDefaults());
        }

        public List<Build> GetBuilds(Action<IBuildHavingBuilder> having, Action<IBuildIncludeBuilder> include)
        {
            return m_BuildsRetriever.GetBuilds(having, _ => _.DefaultCount(), include);
        }

        public List<Build> GetBuilds(Action<IBuildHavingBuilder> having, Action<IBuildIncludeBuilder> include,
            Action<ICountBuilder> count)
        {
            return m_BuildsRetriever.GetBuilds(having, count, include);
        }

        public List<Agent> GetAgents(Action<IAgentHavingBuilder> having)
        {
            return m_AgentsRetriever.GetAgents(having);
        }

        public Build GetBuild(Action<IBuildHavingBuilder> having, Action<IBuildIncludeBuilder> include)
        {
            var builds = GetBuilds(having, include);
            if (!builds.Any()) throw new BuildNotFoundException();
            if (builds.Count() > 1) throw new MoreThanOneBuildFoundException();
            return builds.Single();
        }

        public Build GetBuild(Action<IBuildHavingBuilder> having)
        {
            return GetBuild(having, _ => _.IncludeDefaults());
        }

        public Build GetLastBuild(Action<IBuildHavingBuilder> having)
        {
            return GetLastBuild(having, _ => { });
        }

        public Build GetLastBuild(Action<IBuildHavingBuilder> having, Action<IBuildAdditionalIncludeBuilder> include)
        {
            var builds = GetBuilds(having, _ => _.IncludeDefaults(), __ => __.Count(1));
            if (!builds.Any()) throw new BuildNotFoundException();
            var lastBuild = GetBuild(builds.First().Id);
            var buildAdditionalIncludeBuilder = new BuildAdditionalIncludeBuilder();
            include(buildAdditionalIncludeBuilder);
            if (buildAdditionalIncludeBuilder.ShouldIncludeChanges)
            {
                lastBuild.BuildChanges = m_ChangesRetriever.GetChanges(_ => _.Build(__ => __.Id(lastBuild.Id)), 
                    buildAdditionalIncludeBuilder.ChangesInclude);
            }
            return lastBuild;
        }

        public Build GetBuild(long buildId)
        {
            return m_BuildsRetriever.GetBuild(buildId);
        }

        public BuildConfiguration GetBuildConfiguration(Action<IBuildConfigurationHavingBuilder> having)
        {
            return m_BuildConfigurationRetriever.GetSingleBuildConfiguration(having);
        }

        public IList<BuildConfiguration> GetBuildConfigurations(Action<IBuildConfigurationHavingBuilder> having)
        {
            return m_BuildConfigurationRetriever.RetrieveBuildConfigurations(having, null);
        }

        public void SetParameters(Action<IBuildConfigurationHavingBuilder> having,
            Action<IBuildParameterValueBuilder> parameters)
        {
            m_BuildConfigurationRetriever.SetParameters(having, parameters);
        }

        public void RunBuildConfiguration(Action<IBuildConfigurationHavingBuilder> having,
            Action<IBuildParameterValueBuilder> parameters)
        {
            m_BuildConfigurationRunner.Run(having, parameters: parameters);
        }

        public void RunBuildConfiguration(Action<IBuildConfigurationHavingBuilder> having,
            Action<IAgentHavingBuilder> onAgent)
        {
            m_BuildConfigurationRunner.Run(having, onAgent);
        }

        public void RunBuildConfiguration(Action<IBuildConfigurationHavingBuilder> having,
            Action<IAgentHavingBuilder> onAgent, Action<IBuildParameterValueBuilder> parameters)
        {
            m_BuildConfigurationRunner.Run(having, onAgent, parameters);
        }

        public void RunBuildConfiguration(Action<IBuildConfigurationHavingBuilder> having)
        {
            m_BuildConfigurationRunner.Run(having);
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

        public List<Build> GetBuildsQueue(Action<IQueueHavingBuilder> having = null)
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
    }
}