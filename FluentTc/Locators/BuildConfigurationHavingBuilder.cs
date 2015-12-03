using System;
using System.Collections.Generic;

namespace FluentTc.Locators
{
    public interface IBuildConfigurationHavingBuilder 
    {
        IBuildConfigurationHavingBuilder Id(string buildConfigurationId);
        IBuildConfigurationHavingBuilder Name(string buildConfigurationName);
        IBuildConfigurationHavingBuilder InternalId(string internalId);
        IBuildConfigurationHavingBuilder Project(Action<IBuildProjectHavingBuilder> projectHavingBuilderAction);
        IBuildConfigurationHavingBuilder ProjectRecursively(Action<IBuildProjectHavingBuilder> projectHavingBuilderAction);
    }

    public class BuildConfigurationHavingBuilder : IBuildConfigurationHavingBuilder
    {
        private readonly List<string> m_Having = new List<string>();
        private readonly IBuildProjectHavingBuilderFactory m_BuildProjectHavingBuilderFactory;

        public BuildConfigurationHavingBuilder(IBuildProjectHavingBuilderFactory buildProjectHavingBuilderFactory)
        {
            m_BuildProjectHavingBuilderFactory = buildProjectHavingBuilderFactory;
        }

        public IBuildConfigurationHavingBuilder Id(string buildConfigurationId)
        {
            m_Having.Add("id:" + buildConfigurationId);
            return this;
        }

        public IBuildConfigurationHavingBuilder Name(string buildConfigurationName)
        {
            m_Having.Add("name:" + buildConfigurationName);
            return this;
        }

        public IBuildConfigurationHavingBuilder InternalId(string internalId)
        {
            m_Having.Add("internalId:" + internalId);
            return this;
        }

        public IBuildConfigurationHavingBuilder Project(Action<IBuildProjectHavingBuilder> projectHavingBuilderAction)
        {
            var buildProjectHavingBuilder = m_BuildProjectHavingBuilderFactory.CreateBuildProjectHavingBuilder();
            projectHavingBuilderAction(buildProjectHavingBuilder);
            m_Having.Add("project:" + buildProjectHavingBuilder.GetLocator());
            return this;
        }

        public IBuildConfigurationHavingBuilder ProjectRecursively(Action<IBuildProjectHavingBuilder> projectHavingBuilderAction)
        {
            var buildProjectHavingBuilder = m_BuildProjectHavingBuilderFactory.CreateBuildProjectHavingBuilder();
            projectHavingBuilderAction(buildProjectHavingBuilder);
            m_Having.Add("affectedProject:" + buildProjectHavingBuilder.GetLocator());
            return this;
        }

        public string GetLocator()
        {
            return string.Join(",", m_Having);
        }
    }
}