using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FluentTc.Locators
{
    public interface IBuildParameterTypeBuilder
    {
        IBuildParameterTypeBuilder WithDisplayNormal();
        IBuildParameterTypeBuilder WithDisplayHidden();
        IBuildParameterTypeBuilder WithDisplayPrompt();
        IBuildParameterTypeBuilder WithDescription(string description);
        IBuildParameterTypeBuilder WithLabel(string label);
        IBuildParameterTypeBuilder AsSelectList(Action<IBuildParameterSelectListTypeBuilder> selectListBuilder);
        IBuildParameterTypeBuilder AsCheckbox(Action<IBuildParameterCheckboxTypeBuilder> checkboxBuilder);
        IBuildParameterTypeBuilder AsPassword();
        IBuildParameterTypeBuilder AsText(Action<IBuildParameterTextTypeBuilder> textBuilder);
    }

    internal class BuildParameterTypeBuilder : IBuildParameterTypeBuilder
    {
        private const string DisplayNormal = "normal";
        private const string DisplayHidden = "hidden";
        private const string DisplayPrompt = "prompt";

        private const string DefaultDisplay = DisplayNormal;

        private string m_display = DefaultDisplay;
        private string m_description;
        private string m_label;
        private string m_special;

        public IBuildParameterTypeBuilder WithDisplayNormal()
        {
            m_display = DisplayNormal;
            return this;
        }

        public IBuildParameterTypeBuilder WithDisplayHidden()
        {
            m_display = DisplayHidden;
            return this;
        }

        public IBuildParameterTypeBuilder WithDisplayPrompt()
        {
            m_display = DisplayPrompt;
            return this;
        }

        public IBuildParameterTypeBuilder WithDescription(string description)
        {
            if (!string.IsNullOrEmpty(description))
                m_description = description;
            return this;
        }

        public IBuildParameterTypeBuilder WithLabel(string label)
        {
            if (!string.IsNullOrEmpty(label))
                m_label = label;
            return this;
        }

        public IBuildParameterTypeBuilder AsSelectList(Action<IBuildParameterSelectListTypeBuilder> selectListBuilder)
        {
            var builder = new BuildParameterSelectListTypeBuilder();
            selectListBuilder(builder);
            var special = builder.Build();
            if (!string.IsNullOrEmpty(special))
                m_special = special;
            return this;
        }

        public IBuildParameterTypeBuilder AsCheckbox(Action<IBuildParameterCheckboxTypeBuilder> checkboxBuilder)
        {
            var builder = new BuildParameterCheckboxTypeBuilder();
            checkboxBuilder(builder);
            var special = builder.Build();
            if (!string.IsNullOrEmpty(special))
                m_special = special;
            return this;
        }

        public IBuildParameterTypeBuilder AsText(Action<IBuildParameterTextTypeBuilder> textBuilder)
        {
            var builder = new BuildParameterTextTypeBuilder();
            textBuilder(builder);
            var special = builder.Build();
            if (!string.IsNullOrEmpty(special))
                m_special = special;
            return this;
        }

        public IBuildParameterTypeBuilder AsPassword()
        {
            m_special = "password";
            return this;
        }

        public string Build()
        {
            var builder = new StringBuilder();
            if (string.IsNullOrEmpty(m_special))
                return string.Empty;
            builder.Append(m_special);
            if (!string.IsNullOrEmpty(m_label))
                builder.Append($" label='{m_label}'");
            if (!string.IsNullOrEmpty(m_description))
                builder.Append($" description='{m_description}'");
            builder.Append($" display='{m_display}'");
            return builder.ToString();
        }
    }
}
