using System.Collections.Generic;

namespace FluentTc.Locators
{
    public interface IChangesIncludeBuilder
    {
        IChangesIncludeBuilder IncludeComment();
        IChangesIncludeBuilder IncludeFiles();
        IChangesIncludeBuilder IncludeVcsRootInstance();
        IChangesIncludeBuilder IncludeDefaults();
    }

    internal class ChangesIncludeBuilder : IChangesIncludeBuilder
    {
        readonly IList<string> m_Properties = new List<string>(new[]
        {
            "id", "version", "href", "username", "date", "webUrl"
        });

        public IChangesIncludeBuilder IncludeDefaults()
        {
            return this;
        }

        public IChangesIncludeBuilder IncludeComment()
        {
            m_Properties.Add("comment");
            return this;
        }

        public IChangesIncludeBuilder IncludeFiles()
        {
            m_Properties.Add("files");
            return this;
        }

        public IChangesIncludeBuilder IncludeVcsRootInstance()
        {
            m_Properties.Add("vcsRootInstance");
            return this;
        }

        internal string GetColumns()
        {
            return string.Join(",", m_Properties);
        }
    }
}