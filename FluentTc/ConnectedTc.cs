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
        BuildConfiguration GetBuildConfiguration(Action<IBuildConfigurationHavingBuilder> having, Action<BuildConfigurationPropertyBuilder> include);
        IList<BuildConfiguration> GetBuildConfigurations(Action<IBuildConfigurationHavingBuilder> having, Action<BuildConfigurationPropertyBuilder> include);
        BuildConfiguration SetParameters(Action<BuildConfigurationHavingBuilder> having, Action<BuildParameterValueBuilder> parameters);
        BuildConfiguration RunBuildConfiguration(Action<BuildConfigurationHavingBuilder> having, Action<BuildParameterValueBuilder> parameters);
        BuildConfiguration RunBuildConfiguration(Action<BuildConfigurationHavingBuilder> having, Action<AgentLocatorBuilder> onAgent);
        BuildConfiguration RunBuildConfiguration(Action<BuildConfigurationHavingBuilder> having, Action<AgentLocatorBuilder> onAgent, Action<BuildParameterValueBuilder> parameters);
        BuildConfiguration RunBuildConfiguration(Action<BuildConfigurationHavingBuilder> having);
        IList<BuildConfiguration> GetBuildConfigurations(Action<BuildProjectHavingBuilder> having, Action<BuildConfigurationPropertyBuilder> include);
        BuildConfiguration CreateBuildConfiguration(Action<BuildProjectHavingBuilder> having, string buildConfigurationName);
        void AttachBuildConfigurationToTemplate(Action<BuildConfigurationHavingBuilder> having, Action<BuildTemplateHavingBuilder> templateHaving);
        void DeleteBuildConfiguration(Action<BuildConfigurationHavingBuilder> having);
        Project GetProject(Action<IBuildProjectHavingBuilder> having);
        IList<Project> GetProjects(Action<IBuildProjectHavingBuilder> having);
    }

    internal class ConnectedTc : IConnectedTc
    {
        private readonly IBuildsRetriever m_BuildsRetriever;
        private readonly IAgentsRetriever m_AgentsRetriever;
        private readonly IProjectsRetriever m_ProjectsRetriever;
        private readonly IBuildConfigurationRetriever m_BuildConfigurationRetriever;

        public ConnectedTc(IBuildsRetriever buildsRetriever, IAgentsRetriever agentsRetriever, IProjectsRetriever projectsRetriever, IBuildConfigurationRetriever buildConfigurationRetriever)
        {
            m_BuildsRetriever = buildsRetriever;
            m_AgentsRetriever = agentsRetriever;
            m_ProjectsRetriever = projectsRetriever;
            m_BuildConfigurationRetriever = buildConfigurationRetriever;
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

        public BuildConfiguration GetBuildConfiguration(Action<IBuildConfigurationHavingBuilder> having, Action<BuildConfigurationPropertyBuilder> include)
        {
            var buildConfigurations = GetBuildConfigurations(having, include);
            if (!buildConfigurations.Any()) throw new BuildConfigurationNotFoundException();
            if (buildConfigurations.Count() > 1) throw new MoreThanOneBuildConfigurationFoundException();
            return buildConfigurations.Single();
        }

        public IList<BuildConfiguration> GetBuildConfigurations(Action<IBuildConfigurationHavingBuilder> having, Action<BuildConfigurationPropertyBuilder> include)
        {
            return m_BuildConfigurationRetriever.RetrieveBuildConfigurations(having, include);
        }

        public BuildConfiguration SetParameters(Action<BuildConfigurationHavingBuilder> having, Action<BuildParameterValueBuilder> parameters)
        {
            throw new NotImplementedException();
        }

        public BuildConfiguration RunBuildConfiguration(Action<BuildConfigurationHavingBuilder> having, Action<BuildParameterValueBuilder> parameters)
        {
            throw new NotImplementedException();
        }

        public BuildConfiguration RunBuildConfiguration(Action<BuildConfigurationHavingBuilder> having, Action<AgentLocatorBuilder> onAgent)
        {
            throw new NotImplementedException();
        }

        public BuildConfiguration RunBuildConfiguration(Action<BuildConfigurationHavingBuilder> having, Action<AgentLocatorBuilder> onAgent, Action<BuildParameterValueBuilder> parameters)
        {
            throw new NotImplementedException();
        }

        public BuildConfiguration RunBuildConfiguration(Action<BuildConfigurationHavingBuilder> having)
        {
            throw new NotImplementedException();
        }

        public IList<BuildConfiguration> GetBuildConfigurations(Action<BuildProjectHavingBuilder> having, Action<BuildConfigurationPropertyBuilder> include)
        {
            throw new NotImplementedException();
        }

        public BuildConfiguration CreateBuildConfiguration(Action<BuildProjectHavingBuilder> having, string buildConfigurationName)
        {
            throw new NotImplementedException();
        }

        public void AttachBuildConfigurationToTemplate(Action<BuildConfigurationHavingBuilder> having, Action<BuildTemplateHavingBuilder> templateHaving)
        {
            throw new NotImplementedException();
        }

        public void DeleteBuildConfiguration(Action<BuildConfigurationHavingBuilder> having)
        {
            throw new NotImplementedException();
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
    }
}