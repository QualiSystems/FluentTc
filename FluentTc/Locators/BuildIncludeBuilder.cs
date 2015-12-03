using System.Collections.Generic;
using System.Diagnostics;

namespace FluentTc.Locators
{
    public interface IBuildIncludeBuilder
    {
        IBuildIncludeBuilder IncludeStartDate();
        IBuildIncludeBuilder IncludeFinishDate();
        IBuildIncludeBuilder IncludeStatusText();
        IBuildIncludeBuilder IncludeDefaults();
    }

    public class BuildIncludeBuilder : IBuildIncludeBuilder
    {
        readonly IList<string> m_Properties = new List<string>(new[]
        {
            "buildTypeId", "href", "id", "number", "state", "status","webUrl"
        });

        public IBuildIncludeBuilder IncludeStartDate()
        {
            return IncludeProperty();
        }

        public IBuildIncludeBuilder IncludeFinishDate()
        {
            return IncludeProperty();
        }

        public IBuildIncludeBuilder IncludeStatusText()
        {
            return IncludeProperty();
        }

        private BuildIncludeBuilder IncludeProperty()
        {
            var methodName = new StackFrame(1).GetMethod().Name.Remove(0, 7);
            m_Properties.Add(methodName.FirstCharacterToLower());
            return this;
        }

        public IBuildIncludeBuilder IncludeDefaults()
        {
            return this;
        }

        public virtual string GetColumns()
        {
            return string.Join(",", m_Properties);
        }
    }
}