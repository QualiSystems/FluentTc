using System.Collections.Generic;
using FluentTc.Domain;

namespace FluentTc.Locators
{
    public interface IBuildParameterValueBuilder
    {
        IBuildParameterValueBuilder Parameter(string name, string value);
        Property[] GetParameters();
    }

    public class BuildParameterValueBuilder : IBuildParameterValueBuilder
    {
        readonly List<Property> m_Properties = new List<Property>();

        public IBuildParameterValueBuilder Parameter(string name, string value)
        {
            m_Properties.Add(new Property(){ Name = name, Value = value});
            return this;
        }

        public Property[] GetParameters()
        {
            return m_Properties.ToArray();
        }
    }
}