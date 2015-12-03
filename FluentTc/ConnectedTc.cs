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
        List<Build> GetBuilds(Action<IBuildHavingBuilder> having, Action<ICountBuilder> count, Action<IBuildIncludeBuilder> include);
        List<Agent> GetAgents(Action<IAgentHavingBuilder> having);
        Build GetBuild(Action<IBuildHavingBuilder> having, Action<IBuildIncludeBuilder> include);
        Build GetBuild(Action<IBuildHavingBuilder> having);
        Build GetBuild(long buildId);
        BuildConfiguration GetBuildConfiguration(Action<IBuildConfigurationHavingBuilder> having);
        IList<BuildConfiguration> GetBuildConfigurations(Action<IBuildConfigurationHavingBuilder> having);
        void SetBuildConfigurationParameters(Action<IBuildConfigurationHavingBuilder> having, Action<IBuildParameterValueBuilder> parameters);
        void SetProjectParameters(Action<IBuildProjectHavingBuilder> having, Action<IBuildParameterValueBuilder> parameters);
        void RunBuildConfiguration(Action<IBuildConfigurationHavingBuilder> having, Action<IBuildParameterValueBuilder> parameters);
        void RunBuildConfiguration(Action<IBuildConfigurationHavingBuilder> having, Action<IAgentHavingBuilder> onAgent);
        void RunBuildConfiguration(Action<IBuildConfigurationHavingBuilder> having, Action<IAgentHavingBuilder> onAgent, Action<IBuildParameterValueBuilder> parameters);
        void RunBuildConfiguration(Action<IBuildConfigurationHavingBuilder> having);
        BuildConfiguration CreateBuildConfiguration(Action<IBuildProjectHavingBuilder> having, string buildConfigurationName);
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
        Project CreateProject(Action<INewProjectDetailsBuilder> newProjectDetailsBuilderAction);
        List<BuildConfiguration> GetAllBuildConfigurationTemplates();
        BuildConfiguration GetBuildConfigurationTemplate(Action<IBuildConfigurationTemplateHavingBuilder> having);
    }

    internal class ConnectedTc : IConnectedTc
    {
        private readonly IBuildsRetriever m_BuildsRetriever;
        private readonly IAgentsRetriever m_AgentsRetriever;
        private readonly IProjectsRetriever m_ProjectsRetriever;
        private readonly IBuildConfigurationRetriever m_BuildConfigurationRetriever;
        private readonly IAgentEnabler m_AgentEnabler;
        private readonly IBuildConfigurationRunner m_BuildConfigurationRunner;
        private readonly IBuildConfigurationCreator m_BuildConfigurationCreator;
        private readonly IBuildTemplateAttacher m_BuildTemplateAttacher;
        private readonly IBuildQueueRemover m_BuildQueueRemover;
        private readonly IArtifactsDownloader m_ArtifactsDownloader;
        private readonly IInvestigationRetriever m_InvestigationRetriever;
        private readonly IUserRetriever m_UserRetriever;
        private readonly IProjectCreator m_ProjectCreator;
        private readonly IProjectPropertySetter m_ProjectPropertySetter;
        private readonly IBuildConfigurationTemplateRetriever m_BuildConfigurationTemplateRetriever;

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
            IProjectCreator projectCreator, IProjectPropertySetter projectPropertySetter, IBuildConfigurationTemplateRetriever buildConfigurationTemplateRetriever)
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
        }

        public List<Build> GetBuilds(Action<IBuildHavingBuilder> having, Action<ICountBuilder> count, Action<IBuildIncludeBuilder> include)
        {
            return m_BuildsRetriever.GetBuilds(having, count, include);
        }

        public List<Agent> GetAgents(Action<IAgentHavingBuilder> having)
        {
            return m_AgentsRetriever.GetAgents(having);
        }

        public List<Build> GetBuilds(Action<IBuildHavingBuilder> having, Action<IBuildIncludeBuilder> include)
        {
            return m_BuildsRetriever.GetBuilds(having, _ => _.DefaultCount(), include);
        }

        public List<Build> GetBuilds(Action<IBuildHavingBuilder> having)
        {
            return m_BuildsRetriever.GetBuilds(having, _ => _.DefaultCount(), _ => _.IncludeDefaults());
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

        public void SetBuildConfigurationParameters(Action<IBuildConfigurationHavingBuilder> having, Action<IBuildParameterValueBuilder> parameters)
        {
            m_BuildConfigurationRetriever.SetParameters(having, parameters);
        }

        public void SetProjectParameters(Action<IBuildProjectHavingBuilder> having, Action<IBuildParameterValueBuilder> parameters)
        {
            m_ProjectPropertySetter.SetProjectParameters(having, parameters);
        }

        public void RunBuildConfiguration(Action<IBuildConfigurationHavingBuilder> having, Action<IBuildParameterValueBuilder> parameters)
        {
            m_BuildConfigurationRunner.Run(having, parameters: parameters);
        }

        public void RunBuildConfiguration(Action<IBuildConfigurationHavingBuilder> having, Action<IAgentHavingBuilder> onAgent)
        {
            m_BuildConfigurationRunner.Run(having, onAgent);
        }

        public void RunBuildConfiguration(Action<IBuildConfigurationHavingBuilder> having, Action<IAgentHavingBuilder> onAgent, Action<IBuildParameterValueBuilder> parameters)
        {
            m_BuildConfigurationRunner.Run(having, onAgent, parameters);
        }

        public void RunBuildConfiguration(Action<IBuildConfigurationHavingBuilder> having)
        {
            m_BuildConfigurationRunner.Run(having);
        }

        public BuildConfiguration CreateBuildConfiguration(Action<IBuildProjectHavingBuilder> having, string buildConfigurationName)
        {
            return m_BuildConfigurationCreator.Create(having, buildConfigurationName);
        }

        public void AttachBuildConfigurationToTemplate(Action<IBuildConfigurationHavingBuilder> having, string buildTemplateId)
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
            if (project.Projects.Project == null || !project.Projects.Project.Any()) return project.BuildTypes.BuildType;

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
        /// gets the investigator of the test
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
    }
}