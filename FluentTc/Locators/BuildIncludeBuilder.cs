using System.Collections.Generic;
using System.Diagnostics;

namespace FluentTc.Locators
{
    public interface IBuildIncludeBuilder
    {
        BuildIncludeBuilder IncludeStartDate();
        BuildIncludeBuilder IncludeFinishDate();
        BuildIncludeBuilder IncludeStatusText();
        BuildIncludeBuilder IncludeDefaults();
        string GetColumns();
    }

    public class BuildIncludeBuilder : IBuildIncludeBuilder
    {
        readonly IList<string> m_Properties = new List<string>(new[]
        {
            "buildTypeId", "href", "id", "number", "state", "status","webUrl"
        });

        public BuildIncludeBuilder IncludeStartDate()
        {
            return IncludeProperty();
        }

        public BuildIncludeBuilder IncludeFinishDate()
        {
            return IncludeProperty();
        }

        public BuildIncludeBuilder IncludeStatusText()
        {
            return IncludeProperty();
        }

        private BuildIncludeBuilder IncludeProperty()
        {
            var methodName = new StackFrame(1).GetMethod().Name.Remove(0, 7);
            m_Properties.Add(methodName.FirstCharacterToLower());
            return this;
        }

        public BuildIncludeBuilder IncludeDefaults()
        {
            return this;
        }

        string IBuildIncludeBuilder.GetColumns()
        {
            return string.Join(",", m_Properties);
        }
    }
}