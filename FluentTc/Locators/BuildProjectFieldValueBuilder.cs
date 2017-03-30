using FluentTc.Domain;
using System.Collections.Generic;
using System.Globalization;

namespace FluentTc.Locators
{
    public interface IBuildProjectFieldValueBuilder
    {
        IBuildProjectFieldValueBuilder Name(string value);
        IBuildProjectFieldValueBuilder Description(string value);
        IBuildProjectFieldValueBuilder Archived(bool value);
    }

    public class BuildProjectFieldValueBuilder: IBuildProjectFieldValueBuilder
    {
        readonly List<Property> m_Properties = new List<Property>();

        public IBuildProjectFieldValueBuilder Description(string value)
        {
            m_Properties.Add(new Property { Name = nameof(Description).ToLower(), Value = value, Type = null });
            return this;
        }

        public IBuildProjectFieldValueBuilder Archived(bool value)
        {
            m_Properties.Add(new Property
                {
                Name = nameof(Name).ToLower(),
                Value = value.ToString(CultureInfo.InvariantCulture).ToLowerInvariant(),
                Type = null });
            return this;
        }

        public IBuildProjectFieldValueBuilder Name(string value)
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
