using FluentTc.Domain;
using System.Collections.Generic;
using System.Globalization;

namespace FluentTc.Locators
{
    public interface IBuildProjectFieldValueBuilder
    {
        IBuildProjectFieldValueBuilder Name(string value);
        IBuildProjectFieldValueBuilder Description(string value);
        IBuildProjectFieldValueBuilder Archived();
        IBuildProjectFieldValueBuilder NotArchived();
    }

    public class BuildProjectFieldValueBuilder: IBuildProjectFieldValueBuilder
    {
        readonly List<Property> m_Properties = new List<Property>();

        public IBuildProjectFieldValueBuilder Description(string value)
        {
            m_Properties.Add(new Property { Name = "description", Value = value, Type = null });
            return this;
        }

        public IBuildProjectFieldValueBuilder Archived()
        {
            m_Properties.Add(new Property
                {
                Name = "archived",
                Value = "true",
                Type = null });
            return this;
        }

        public IBuildProjectFieldValueBuilder NotArchived()
        {
            m_Properties.Add(new Property
            {
                Name = "archived",
                Value = "false",
                Type = null
            });
            return this;
        }

        public IBuildProjectFieldValueBuilder Name(string value)
        {
            m_Properties.Add(new Property { Name = "name", Value = value, Type = null });
            return this;
        }

        public List<Property> GetFields()
        {
            return m_Properties;
        }
    }
}
