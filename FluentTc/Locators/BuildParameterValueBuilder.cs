using System.Collections.Generic;
using FluentTc.Domain;

namespace FluentTc.Locators
{
    public interface IBuildParameterValueBuilder
    {
        IBuildParameterValueBuilder Parameter(string name, string value);
        IBuildParameterValueBuilder Parameter(string name, string value, string rawValue);
    }

    public class BuildParameterValueBuilder : IBuildParameterValueBuilder
    {
        readonly List<Property> m_Properties = new List<Property>();

        public IBuildParameterValueBuilder Parameter(string name, string value)
        {
            m_Properties.Add(new Property { Name = name, Value = value, Type = null });
            return this;
        }

        public IBuildParameterValueBuilder Parameter(string name, string value, string rawValue)
        {
            m_Properties.Add(new Property { Name = name, Value = value, Type = new PropertyType { RawValue = rawValue } });
            return this;
        }

        public List<Property> GetParameters()
        {
            return m_Properties;
        }
    }
}