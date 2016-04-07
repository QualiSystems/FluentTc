using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FluentTc.Locators
{
    public interface IBuildParameterSelectListTypeBuilder
    {
        IBuildParameterSelectListTypeBuilder Value(string value);
        IBuildParameterSelectListTypeBuilder LabeledValue(string label, string value);
        IBuildParameterSelectListTypeBuilder AllowMultiple(bool allowMultiple);
        IBuildParameterSelectListTypeBuilder WithSeparator(string separator);
    }

    internal class BuildParameterSelectListTypeBuilder : IBuildParameterSelectListTypeBuilder
    {
        private readonly List<Tuple<string, string>> m_values = new List<Tuple<string, string>>();

        private string m_separator = null;
        private bool m_multiple;

        public IBuildParameterSelectListTypeBuilder Value(string value)
        {
            if (!string.IsNullOrEmpty(value))
                m_values.Add(new Tuple<string, string>(null, value));
            return this;
        }

        public IBuildParameterSelectListTypeBuilder LabeledValue(string label, string value)
        {
            if (!string.IsNullOrEmpty(value))
                m_values.Add(new Tuple<string, string>(label, value));
            return this;
        }

        public IBuildParameterSelectListTypeBuilder AllowMultiple(bool allowMultiple)
        {
            m_multiple = allowMultiple;
            return this;
        }

        public IBuildParameterSelectListTypeBuilder WithSeparator(string separator)
        {
            if (!string.IsNullOrEmpty(separator))
                m_separator = separator;
            return this;
        }

        public string Build()
        {
            var builder = new StringBuilder();
            if (!m_values.Any())
                return string.Empty;
            builder.Append("select");
            if (m_multiple)
            {
                builder.Append(" multiple='true'");
                if (!string.IsNullOrEmpty(m_separator))
                    builder.Append($" valueSeparator='{m_separator}'");
            }
            for (var i = 0; i < m_values.Count; i++)
            {
                if (!string.IsNullOrEmpty(m_values[i].Item1))
                    builder.Append($" label_{i + 1}='{m_values[i].Item1}'");
                builder.Append($" data_{i + 1}='{m_values[i].Item2}'");
            }
            return builder.ToString();
        }
    }
}
