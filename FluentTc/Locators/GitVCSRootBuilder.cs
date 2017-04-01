using FluentTc.Domain;
using System;
using System.Globalization;
using System.Text;

namespace FluentTc.Locators
{
    public enum AgentCleanPolicy
    {
        OnBranchChange,
        Always,
        Never
    }

    public enum AgentCleanFilePolicy
    {
        AllUntracked,
        AllIgnoredUntrackedFiles,
        AllNonIgnoredUntrackedFiles
    }

    public enum AuthMethod
    {
        Anonymous,
        Password
    }

    public enum UserNameStyle
    {
        UserId,
        AuthorName,
        AuthorNameAndEmail,
        AutorhEmail
    }

    public interface IGitVCSRootBuilder
    {
        IGitVCSRootBuilder Name(string value);
        IGitVCSRootBuilder Id(string value);
        IGitVCSRootBuilder AgentCleanFilePolicy(AgentCleanFilePolicy value);

        IGitVCSRootBuilder AgentCleanPolicy(AgentCleanPolicy value);

        IGitVCSRootBuilder AuthMethod(AuthMethod value);

        IGitVCSRootBuilder Branch(string value);
        IGitVCSRootBuilder IgnoreKnownHosts(bool value);
        IGitVCSRootBuilder UserNameStyle(UserNameStyle value);
        IGitVCSRootBuilder Username(string value);
        IGitVCSRootBuilder Password(string value);
        IGitVCSRootBuilder UseAlternates(bool value);
        IGitVCSRootBuilder Url(Uri value);
        IGitVCSRootBuilder BranchSpec(string value);
        IGitVCSRootBuilder SubModuleCheckout(bool value);

        IGitVCSRootBuilder Project(Project value);

    }

    public class GitVCSRootBuilder: IGitVCSRootBuilder
    {
        private VcsRoot m_VCSRoot = new VcsRoot();

        public GitVCSRootBuilder()
        {
            m_VCSRoot.Properties = new Properties();
            m_VCSRoot.Properties.Property = new System.Collections.Generic.List<Property>();
            m_VCSRoot.vcsName = "jetbrains.git";
        }

        public IGitVCSRootBuilder Id(string value)
        {
            m_VCSRoot.Id = value;
            return this;
        }

        public IGitVCSRootBuilder Name(string value)
        {
            m_VCSRoot.Name = value;
            return this;
        }

        public IGitVCSRootBuilder AgentCleanFilePolicy(AgentCleanFilePolicy value)
        {
            m_VCSRoot.Properties.Property.Add(
                new Property()
                {
                    Name = FromPascalToCamelCase(typeof(AgentCleanFilePolicy).Name),
                    Value = FromPascalToCapitalizedCase(value.ToString())
                }
            );
            return this;
        }

        public IGitVCSRootBuilder AgentCleanPolicy(AgentCleanPolicy value)
        {
            m_VCSRoot.Properties.Property.Add(
                new Property()
                {
                    Name = FromPascalToCamelCase(typeof(AgentCleanPolicy).Name),
                    Value = FromPascalToCapitalizedCase(value.ToString())
                }
            );
            return this;
        }

        public IGitVCSRootBuilder AuthMethod(AuthMethod value)
        {
            m_VCSRoot.Properties.Property.Add(
                new Property()
                {
                    Name = FromPascalToCamelCase(typeof(AuthMethod).Name),
                    Value = FromPascalToCapitalizedCase(value.ToString())
                }
            );
            return this;
        }

        public IGitVCSRootBuilder Branch(string value)
        {
            m_VCSRoot.Properties.Property.Add(
                new Property()
                {
                    Name = "branch",
                    Value = value
                }
            );
            return this;
        }

        public IGitVCSRootBuilder IgnoreKnownHosts(bool value)
        {
            m_VCSRoot.Properties.Property.Add(
                new Property()
                {
                    Name = "ignoreKnownHosts",
                    Value = value.ToString(CultureInfo.InvariantCulture).ToLower()
                }
            );
            return this;
        }

        public IGitVCSRootBuilder UserNameStyle(UserNameStyle value)
        {
            m_VCSRoot.Properties.Property.Add(
                new Property()
                {
                    Name = FromPascalToCamelCase(typeof(UserNameStyle).Name),
                    Value = FromPascalToCapitalizedCase(value.ToString())
                }
            );
            return this;
        }

        public IGitVCSRootBuilder Username(string value)
        {
            m_VCSRoot.Properties.Property.Add(
                new Property()
                {
                    Name = "username",
                    Value = value
                }
            );
            return this;
        }

        public IGitVCSRootBuilder Password(string value)
        {
            m_VCSRoot.Properties.Property.Add(
                new Property()
                {
                    Name = "secure:password",
                    Value = value
                }
            );
            return this;
        }

        public IGitVCSRootBuilder UseAlternates(bool value)
        {
            m_VCSRoot.Properties.Property.Add(
                new Property()
                {
                    Name = "useAlternates",
                    Value = value.ToString(CultureInfo.InvariantCulture).ToLower()
                }
            );
            return this;
        }

        public IGitVCSRootBuilder Url(Uri value)
        {
            m_VCSRoot.Properties.Property.Add(
                new Property()
                {
                    Name = "url",
                    Value = value.ToString()
                }
            );
            return this;
        }

        public IGitVCSRootBuilder BranchSpec(string value)
        {
            m_VCSRoot.Properties.Property.Add(
                new Property()
                {
                    Name = "teamcity:branchSpec",
                    Value = value
                }
            );
            return this;
        }

        public IGitVCSRootBuilder SubModuleCheckout(bool value)
        {
            m_VCSRoot.Properties.Property.Add(
                new Property()
                {
                    Name = "teamcity:branchSpec",
                    Value = (value ? "true" : "false")
                }
            );
            return this;
        }

        public IGitVCSRootBuilder Project(Project value)
        {
            m_VCSRoot.Project = value;
            return this;
        }

        public VcsRoot GetVCSRoot()
        {
            return m_VCSRoot;
        }

        private string FromPascalToCapitalizedCase(string value)
        {
            StringBuilder sb = new StringBuilder();

            int charCount = 0;
            foreach (char c in value)
            {
                charCount++;
                if (char.IsUpper(c) && charCount != value.Length && charCount != 1)
                {
                    sb.Append("_");
                }
                sb.Append(char.ToUpper(c));
            }
            return sb.ToString();
        }

        private string FromPascalToCamelCase(string value)
        {
            var len = value.Length;
            if (len > 0)
            {
                var sb = new StringBuilder();
                sb.Append(char.ToLower(value[0]));
                if (len > 1)
                    sb.Append(value.Substring(1, len - 1));
                return sb.ToString();
            }
            else
                return "";
        }
    }
}
