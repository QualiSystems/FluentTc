using System;
using System.Collections.Generic;
using System.Linq;
using FluentTc.Domain;
using FluentTc.Engine;
using FluentTc.Locators;

namespace FluentTc
{
    public interface IConnectedTc
    {
        List<Build> GetBuilds(Action<IBuildHavingBuilder> having, Action<ICountBuilder> count, Action<IBuildIncludeBuilder> include);
        List<Agent> GetAgents(Action<IAgentHavingBuilder> having);
        List<Build> GetBuilds(Action<IBuildHavingBuilder> having, Action<IBuildIncludeBuilder> include);
        List<Build> GetBuilds(Action<IBuildHavingBuilder> having);
        Build GetBuild(Action<IBuildHavingBuilder> having, Action<IBuildIncludeBuilder> include);
        Build GetBuild(Action<IBuildHavingBuilder> having);
        Build GetBuild(long buildId);
        BuildConfiguration GetBuildConfiguration(Action<IBuildConfigurationHavingBuilder> having);
        IList<BuildConfiguration> GetBuildConfigurations(Action<IBuildConfigurationHavingBuilder> having);
        void SetParameters(Action<IBuildConfigurationHavingBuilder> having, Action<IBuildParameterValueBuilder> parameters);
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

        public ConnectedTc(IBuildsRetriever buildsRetriever, IAgentsRetriever agentsRetriever, IProjectsRetriever projectsRetriever, IBuildConfigurationRetriever buildConfigurationRetriever, IAgentEnabler agentEnabler, IBuildConfigurationRunner buildConfigurationRunner, IBuildConfigurationCreator buildConfigurationCreator, IBuildTemplateAttacher buildTemplateAttacher, IBuildQueueRemover buildQueueRemover)
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
            return m_BuildsRetriever.GetBuilds(having, _ => _.All(), include);
        }

        public List<Build> GetBuilds(Action<IBuildHavingBuilder> having)
        {
            return m_BuildsRetriever.GetBuilds(having, _ => _.All(), _ => _.IncludeDefaults());
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

        public void SetParameters(Action<IBuildConfigurationHavingBuilder> having, Action<IBuildParameterValueBuilder> parameters)
        {
            m_BuildConfigurationRetriever.SetParameters(having, parameters);
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
    }
}