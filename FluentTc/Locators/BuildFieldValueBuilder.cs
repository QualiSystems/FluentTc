using FluentTc.Domain;
using System.Collections.Generic;
using System.Globalization;

namespace FluentTc.Locators
{
    public interface IBuildFieldValueBuilder
    {
        IBuildFieldValueBuilder Name(string value);
        IBuildFieldValueBuilder Description(string value);
        IBuildFieldValueBuilder Paused();
        IBuildFieldValueBuilder NotPaused();
    }

    public class BuildFieldValueBuilder: IBuildFieldValueBuilder
    {
        readonly List<Property> m_Properties = new List<Property>();

        public IBuildFieldValueBuilder Description(string value)
        {
            m_Properties.Add(new Property { Name = "description", Value = value, Type = null });
            return this;
        }

        public IBuildFieldValueBuilder Paused()
        {
            m_Properties.Add(new Property
                {
                Name = "paused",
                Value = "true",
                Type = null });
            return this;
        }
        public IBuildFieldValueBuilder NotPaused()
        {
            m_Properties.Add(new Property
            {
                Name = "paused",
                Value = "false",
                Type = null
            });
            return this;
        }

        public IBuildFieldValueBuilder Name(string value)
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
