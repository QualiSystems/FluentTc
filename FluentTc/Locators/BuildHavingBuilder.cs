using System;
using System.Collections.Generic;

namespace FluentTc.Locators
{
    public class BuildHavingBuilder
    {
        private readonly List<string> m_Having = new List<string>();

        public BuildHavingBuilder Personal()
        {
            m_Having.Add("personal:" + bool.TrueString);
            return this;
        }

        public BuildHavingBuilder BuildConfiguration(
            Action<BuildConfigurationHavingBuilder> havingBuildConfig)
        {
            var buildConfigurationHavingBuilder = new BuildConfigurationHavingBuilder();
            havingBuildConfig.Invoke(buildConfigurationHavingBuilder);
            m_Having.AddRange(buildConfigurationHavingBuilder.Get());
            return this;
        }

        public BuildHavingBuilder HavingId(int buildId)
        {
            m_Having.Add("id:" + buildId);
            return this;
        }

        internal string GetLocator()
        {
            return string.Join(",", m_Having);
        }
    }
}