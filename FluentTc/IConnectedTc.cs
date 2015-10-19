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
        BuildConfiguration GetBuildConfiguration(Action<BuildConfigurationHavingBuilder> having, Action<BuildConfigurationPropertyBuilder> include);
        IList<BuildConfiguration> GetBuildConfigurations(Action<BuildConfigurationHavingBuilder> having, Action<BuildConfigurationPropertyBuilder> include);
        BuildConfiguration SetParameters(Action<BuildConfigurationHavingBuilder> having, Action<BuildParameterValueBuilder> parameters);
        BuildConfiguration RunBuildConfiguration(Action<BuildConfigurationHavingBuilder> having, Action<BuildParameterValueBuilder> parameters);
        BuildConfiguration RunBuildConfiguration(Action<BuildConfigurationHavingBuilder> having, Action<AgentLocatorBuilder> onAgent);
        BuildConfiguration RunBuildConfiguration(Action<BuildConfigurationHavingBuilder> having, Action<AgentLocatorBuilder> onAgent, Action<BuildParameterValueBuilder> parameters);
        BuildConfiguration RunBuildConfiguration(Action<BuildConfigurationHavingBuilder> having);
        IList<BuildConfiguration> GetBuildConfigurations(Action<BuildProjectHavingBuilder> having, Action<BuildConfigurationPropertyBuilder> include);
        BuildConfiguration CreateBuildConfiguration(Action<BuildProjectHavingBuilder> having, string buildConfigurationName);
        void AttachBuildConfigurationToTemplate(Action<BuildConfigurationHavingBuilder> having, Action<BuildTemplateHavingBuilder> templateHaving);
        void DeleteBuildConfiguration(Action<BuildConfigurationHavingBuilder> having);
        List<Build> GetBuildQueue(Action<IBuildProjectHavingBuilder> having);
    }

    internal class ConnectedTc : IConnectedTc
    {
        private readonly ITeamCityCaller m_Caller;
        private readonly IBuildsRetriever m_BuildsRetriever;
        private readonly IAgentsRetriever m_AgentsRetriever;

        public ConnectedTc(ITeamCityCaller caller, IBuildsRetriever buildsRetriever, IAgentsRetriever agentsRetriever)
        {
            m_Caller = caller;
            m_BuildsRetriever = buildsRetriever;
            m_AgentsRetriever = agentsRetriever;
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

        public BuildConfiguration GetBuildConfiguration(Action<BuildConfigurationHavingBuilder> having, Action<BuildConfigurationPropertyBuilder> include)
        {
            throw new NotImplementedException();
        }

        public IList<BuildConfiguration> GetBuildConfigurations(Action<BuildConfigurationHavingBuilder> having, Action<BuildConfigurationPropertyBuilder> include)
        {
            throw new NotImplementedException();
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

        public List<Build> GetBuildQueue(Action<IBuildProjectHavingBuilder> having)
        {
            return m_BuildsRetriever.GetBuildQueues(having);
        }
    }
}