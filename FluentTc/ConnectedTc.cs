using System;
using System.Collections.Generic;
using System.Linq;
using FluentTc.Domain;
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
        BuildConfiguration GetBuildConfiguration(Action<IBuildConfigurationHavingBuilder> having);
        IList<BuildConfiguration> GetBuildConfigurations(Action<IBuildConfigurationHavingBuilder> having);
        void SetParameters(Action<IBuildConfigurationHavingBuilder> having, Action<IBuildParameterValueBuilder> parameters);
        void RunBuildConfiguration(Action<IBuildConfigurationHavingBuilder> having, Action<IBuildParameterValueBuilder> parameters);
        void RunBuildConfiguration(Action<IBuildConfigurationHavingBuilder> having, Action<IAgentHavingBuilder> onAgent);
        void RunBuildConfiguration(Action<IBuildConfigurationHavingBuilder> having, Action<IAgentHavingBuilder> onAgent, Action<IBuildParameterValueBuilder> parameters);
        void RunBuildConfiguration(Action<IBuildConfigurationHavingBuilder> having);
        BuildConfiguration CreateBuildConfiguration(Action<IBuildProjectHavingBuilder> having, string buildConfigurationName);
        void AttachBuildConfigurationToTemplate(Action<BuildConfigurationHavingBuilder> having, Action<BuildTemplateHavingBuilder> templateHaving);
        void DeleteBuildConfiguration(Action<BuildConfigurationHavingBuilder> having);
        List<Build> GetBuildQueue(Action<IQueueHavingBuilder> having);
        Project GetProject(Action<IBuildProjectHavingBuilder> having);
        IList<Project> GetProjects(Action<IBuildProjectHavingBuilder> having);
        void DisableAgent(Action<IAgentHavingBuilder> having);
        void EnableAgent(Action<IAgentHavingBuilder> having);
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

        public ConnectedTc(IBuildsRetriever buildsRetriever, IAgentsRetriever agentsRetriever, IProjectsRetriever projectsRetriever, IBuildConfigurationRetriever buildConfigurationRetriever, IAgentEnabler agentEnabler, IBuildConfigurationRunner buildConfigurationRunner, IBuildConfigurationCreator buildConfigurationCreator)
        {
            m_BuildsRetriever = buildsRetriever;
            m_AgentsRetriever = agentsRetriever;
            m_ProjectsRetriever = projectsRetriever;
            m_BuildConfigurationRetriever = buildConfigurationRetriever;
            m_AgentEnabler = agentEnabler;
            m_BuildConfigurationRunner = buildConfigurationRunner;
            m_BuildConfigurationCreator = buildConfigurationCreator;
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
            m_BuildConfigurationRunner.Run(having, parameters);
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

        public void AttachBuildConfigurationToTemplate(Action<BuildConfigurationHavingBuilder> having, Action<BuildTemplateHavingBuilder> templateHaving)
        {
            throw new NotImplementedException();
        }

        public void DeleteBuildConfiguration(Action<BuildConfigurationHavingBuilder> having)
        {
            throw new NotImplementedException();
        }

        public List<Build> GetBuildQueue(Action<IQueueHavingBuilder> having)
        {
            return m_BuildsRetriever.GetBuildQueues(having);
        }

        public Project GetProject(Action<IBuildProjectHavingBuilder> having)
        {
            var projects = GetProjects(having);
            if (!projects.Any()) throw new ProjectNotFoundException();
            if (projects.Count() > 1) throw new MoreThanOneProjectFoundException();
            return projects.Single();
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
    }
}