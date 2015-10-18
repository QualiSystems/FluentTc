using System.Collections.Generic;
using System.Diagnostics;

namespace FluentTc.Locators
{
    public class BuildIncludeBuilder
    {
        readonly IList<string> m_Properties = new List<string>(new[]
        {
            "buildTypeId", "href", "id", "number", "state", "status","webUrl"
        });

        internal string GetColumns()
        {
            return string.Join(",", m_Properties);
        }

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
    }
}