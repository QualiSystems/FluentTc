using System.Collections.Generic;

namespace FluentTc.Locators
{
    public interface IBuildProjectHavingBuilder
    {
        IBuildProjectHavingBuilder Id(string projectId);
        IBuildProjectHavingBuilder Name(string projectName);
    }

    public class BuildProjectHavingBuilder : IBuildProjectHavingBuilder
    {
        readonly List<string> m_Having = new List<string>();

        public IBuildProjectHavingBuilder Id(string projectId)
        {
            m_Having.Add("id:" + projectId);
            return this;
        }

        public IBuildProjectHavingBuilder Name(string projectName)
        {
            m_Having.Add("name:" + projectName);
            return this;
        }

        public string GetLocator()
        {
            return string.Join(",", m_Having);
        }
    }
}