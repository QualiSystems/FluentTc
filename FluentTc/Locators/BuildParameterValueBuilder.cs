using System;
using System.Collections.Generic;
using FluentTc.Domain;

namespace FluentTc.Locators
{
    public interface IBuildParameterValueBuilder
    {
        IBuildParameterValueBuilder Parameter(string name, string value);
        IBuildParameterValueBuilder Parameter(string name, string value, Action<IBuildParameterTypeBuilder> type);
    }

    public class BuildParameterValueBuilder : IBuildParameterValueBuilder
    {
        readonly List<Property> m_Properties = new List<Property>();

        public IBuildParameterValueBuilder Parameter(string name, string value)
        {
            m_Properties.Add(new Property { Name = name, Value = value, Type = null });
            return this;
        }

        public IBuildParameterValueBuilder Parameter(string name, string value, Action<IBuildParameterTypeBuilder> typeBuilder)
        {
            var builder = new BuildParameterTypeBuilder();
            typeBuilder(builder);
            var rawValue = builder.Build();
            if (string.IsNullOrEmpty(rawValue))
                return Parameter(name, value);
            m_Properties.Add(new Property { Name = name, Value = value, Type = new PropertyType { RawValue = rawValue } });
            return this;
        }

        public List<Property> GetParameters()
        {
            return m_Properties;
        }
    }
}