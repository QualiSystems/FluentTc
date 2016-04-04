using System.Collections.Generic;

namespace FluentTc.Locators
{
    public interface IBuildProjectHavingBuilder
    {
        void Id(string projectId);
        void Name(string projectName);
    }

    public class BuildProjectHavingBuilder : IBuildProjectHavingBuilder
    {
        readonly List<string> m_Having = new List<string>();

        public void Id(string projectId)
        {
            m_Having.Add("id:" + projectId);
        }

        public void Name(string projectName)
        {
            m_Having.Add("name:" + projectName);
        }

        public string GetLocator()
        {
            return string.Join(",", m_Having);
        }
    }
}