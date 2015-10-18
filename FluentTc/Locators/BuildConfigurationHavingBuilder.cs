using System;
using System.Collections.Generic;

namespace FluentTc.Locators
{
    public class BuildConfigurationHavingBuilder
    {
        private readonly List<string> m_Having = new List<string>();
        private readonly IBuildProjectHavingBuilderFactory m_BuildProjectHavingBuilderFactory;

        public BuildConfigurationHavingBuilder(IBuildProjectHavingBuilderFactory buildProjectHavingBuilderFactory)
        {
            m_BuildProjectHavingBuilderFactory = buildProjectHavingBuilderFactory;
        }

        public BuildConfigurationHavingBuilder Id(string buildConfigurationId)
        {
            m_Having.Add("id:" + buildConfigurationId);
            return this;
        }

        public BuildConfigurationHavingBuilder Name(string buildConfigurationName)
        {
            m_Having.Add("name:" + buildConfigurationName);
            return this;
        }

        public BuildConfigurationHavingBuilder InternalId(string internalId)
        {
            m_Having.Add("internalId:" + internalId);
            return this;
        }

        public BuildConfigurationHavingBuilder Project(Action<BuildProjectHavingBuilder> projectHavingBuilderAction)
        {
            var buildProjectHavingBuilder = m_BuildProjectHavingBuilderFactory.CreateBuildProjectHavingBuilder();
            projectHavingBuilderAction(buildProjectHavingBuilder);
            m_Having.Add("project:" + buildProjectHavingBuilder.GetLocator());
            return this;
        }

        public BuildConfigurationHavingBuilder ProjectRecursively(Action<BuildProjectHavingBuilder> projectHavingBuilderAction)
        {
            var buildProjectHavingBuilder = new BuildProjectHavingBuilder();
            projectHavingBuilderAction(buildProjectHavingBuilder);
            m_Having.Add("affectedProject:" + buildProjectHavingBuilder.GetLocator());
            return this;
        }

        public IEnumerable<string> Get()
        {
            return new string[] {};
        }
    }
}