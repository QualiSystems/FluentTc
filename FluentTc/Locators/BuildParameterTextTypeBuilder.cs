using System.Text;

namespace FluentTc.Locators
{
    public interface IBuildParameterTextTypeBuilder
    {
        IBuildParameterTextTypeBuilder AsAny();
        IBuildParameterTextTypeBuilder AsNotEmpty();
        IBuildParameterTextTypeBuilder AsRegex(string regexp, string validationMessage);
    }

    internal class BuildParameterTextTypeBuilder : IBuildParameterTextTypeBuilder
    {
        private const string AnyValidation = "any";
        private const string RegexValidation = "regex";
        private const string NotEmptyValidation = "not_empty";

        private const string DefaultValidation = AnyValidation;

        private string m_mode = DefaultValidation;
        private string m_regexp;
        private string m_message;

        public IBuildParameterTextTypeBuilder AsAny()
        {
            m_mode = AnyValidation;
            m_regexp = null;
            m_message = null;
            return this;
        }

        public IBuildParameterTextTypeBuilder AsNotEmpty()
        {
            m_mode = NotEmptyValidation;
            m_regexp = null;
            m_message = null;
            return this;
        }

        public IBuildParameterTextTypeBuilder AsRegex(string regexp, string validationMessage)
        {
            m_mode = RegexValidation;
            m_regexp = regexp;
            m_message = validationMessage;
            return this;
        }

        public string Build()
        {
            var builder = new StringBuilder();
            builder.Append("text");
            builder.Append(" validationMode='").Append(m_mode).Append("'");
            if (!string.IsNullOrEmpty(m_regexp))
                builder.Append(" regexp='").Append(m_regexp).Append("'");
            if (!string.IsNullOrEmpty(m_message))
                builder.Append(" validationMessage='").Append(m_message).Append("'");
            return builder.ToString();
        }
    }
}
