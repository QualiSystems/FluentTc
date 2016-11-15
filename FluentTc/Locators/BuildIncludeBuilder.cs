using System.Collections.Generic;

namespace FluentTc.Locators
{
    public interface IBuildIncludeBuilder
    {
        IBuildIncludeBuilder IncludeStartDate();
        IBuildIncludeBuilder IncludeFinishDate();
        IBuildIncludeBuilder IncludeStatusText();
        IBuildIncludeBuilder IncludeDefaults();
        IBuildIncludeBuilder IncludeRevisions();
    }

    public class BuildIncludeBuilder : IBuildIncludeBuilder
    {
        readonly IList<string> m_Properties = new List<string>(new[]
        {
            "buildTypeId", "href", "id", "number", "state", "status","webUrl"
        });

        public IBuildIncludeBuilder IncludeStartDate()
        {
            m_Properties.Add("startDate");
            return this;
        }

        public IBuildIncludeBuilder IncludeFinishDate()
        {
            m_Properties.Add("finishDate");
            return this;
        }

        public IBuildIncludeBuilder IncludeStatusText()
        {
            m_Properties.Add("statusText");
            return this;
        }

        public IBuildIncludeBuilder IncludeDefaults()
        {
            return this;
        }

        public IBuildIncludeBuilder IncludeRevisions()
        {
            m_Properties.Add("revisions");
            return this;
        }

        public virtual string GetColumns()
        {
            return string.Join(",", m_Properties);
        }
    }
}