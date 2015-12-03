using System.Collections.Generic;

namespace FluentTc.Locators
{
    public interface IBuildConfigurationTemplateHavingBuilder
    {
        IBuildConfigurationTemplateHavingBuilder Id(string buildConfigurationId);
        IBuildConfigurationTemplateHavingBuilder Name(string buildConfigurationName);
    }

    internal class BuildConfigurationTemplateHavingBuilder : IBuildConfigurationTemplateHavingBuilder
    {
        private readonly List<string> m_Having = new List<string>();

        public IBuildConfigurationTemplateHavingBuilder Id(string buildConfigurationId)
        {
            m_Having.Add("id:" + buildConfigurationId);
            return this;
        }

        public IBuildConfigurationTemplateHavingBuilder Name(string buildConfigurationName)
        {
            m_Having.Add("name:" + buildConfigurationName);
            return this;
        }

        public string GetLocator()
        {
            return string.Join(",", m_Having);
        }
    }
}