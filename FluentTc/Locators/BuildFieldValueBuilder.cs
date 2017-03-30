using FluentTc.Domain;
using System.Collections.Generic;
using System.Globalization;

namespace FluentTc.Locators
{
    public interface IBuildFieldValueBuilder
    {
        IBuildFieldValueBuilder Name(string value);
        IBuildFieldValueBuilder Description(string value);
        IBuildFieldValueBuilder Paused(bool value);
    }

    public class BuildFieldValueBuilder: IBuildFieldValueBuilder
    {
        readonly List<Property> m_Properties = new List<Property>();

        public IBuildFieldValueBuilder Description(string value)
        {
            m_Properties.Add(new Property { Name = nameof(Description).ToLower(), Value = value, Type = null });
            return this;
        }

        public IBuildFieldValueBuilder Paused(bool value)
        {
            m_Properties.Add(new Property
                {
                Name = nameof(Name).ToLower(),
                Value = value.ToString(CultureInfo.InvariantCulture).ToLowerInvariant(),
                Type = null });
            return this;
        }

        public IBuildFieldValueBuilder Name(string value)
        {
            m_Properties.Add(new Property { Name = nameof(Name).ToLower(), Value = value, Type = null });
            return this;
        }

        public List<Property> GetFields()
        {
            return m_Properties;
        }
    }
}
