using System;
using System.Collections.Generic;
using System.Linq;
using FluentTc.Domain;
using FluentTc.Locators;

namespace FluentTc
{
    public class RemoteTc
    {
        private ITeamCityCaller m_Caller;
        private IBuildsRetriever m_BuildsRetriever;

        public RemoteTc Connect(Action<TeamCityConfigurationBuilder> connect)
        {
            var teamCityConfigurationBuilder = new TeamCityConfigurationBuilder();
            connect(teamCityConfigurationBuilder);
            m_Caller = new TeamCityCaller(teamCityConfigurationBuilder.GetITeamCityConnectionDetails());
            m_BuildsRetriever = new BuildsRetriever(m_Caller, new BuildHavingBuilderFactory(new BuildConfigurationHavingBuilderFactory(new BuildProjectHavingBuilderFactory()) ));
            return this;
        }

        public List<Build> GetBuilds(Action<IBuildHavingBuilder> having, Action<CountBuilder> count,
            Action<BuildIncludeBuilder> include)
        {
            return m_BuildsRetriever.GetBuilds(having, count, include);
        }

        public List<Build> GetBuilds(Action<IBuildHavingBuilder> having, Action<BuildIncludeBuilder> include)
        {
            return m_BuildsRetriever.GetBuilds(having, _ => _.All(), include);
        }

        public List<Build> GetBuilds(Action<IBuildHavingBuilder> having)
        {
            return m_BuildsRetriever.GetBuilds(having, _ => _.All(), _ => _.IncludeDefaults());
        }

        public Build GetBuild(Action<IBuildHavingBuilder> having, Action<BuildIncludeBuilder> include)
        {
            var builds = GetBuilds(having, include);
            if (!builds.Any() ) throw new BuildNotFoundException();
            if (builds.Count() > 1 ) throw new MoreThanOneBuildFoundException();
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
    }
}