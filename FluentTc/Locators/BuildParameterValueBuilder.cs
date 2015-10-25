using System.Collections.Generic;
using FluentTc.Domain;

namespace FluentTc.Locators
{
    public interface IBuildParameterValueBuilder
    {
        IBuildParameterValueBuilder Parameter(string name, string value);
        List<Property> GetParameters();
    }

    public class BuildParameterValueBuilder : IBuildParameterValueBuilder
    {
        readonly List<Property> m_Properties = new List<Property>();

        public IBuildParameterValueBuilder Parameter(string name, string value)
        {
            m_Properties.Add(new Property(){ Name = name, Value = value});
            return this;
        }

        List<Property> IBuildParameterValueBuilder.GetParameters()
        {
            return m_Properties;
        }
    }
}