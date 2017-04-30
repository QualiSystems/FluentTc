using FluentTc.Domain;
using FluentTc.Extensions;
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

        private void AppendProperty(string name, string value)
        {
            m_VCSRoot.Properties.Property.Add(
                new Property()
                {
                    Name = name,
                    Value = value
                }
            );
        }

        private void AppendProperty<T>(T value)
        {
            m_VCSRoot.Properties.Property.Add(
                new Property()
                {
                    Name = typeof(T).Name.FromPascalToCamelCase(),
                    Value = value.ToString().FromPascalToCapitalizedCase()
                }
            );
        }

        public IGitVCSRootBuilder AgentCleanFilePolicy(AgentCleanFilePolicy value)
        {
            AppendProperty(value);
            return this;
        }

        public IGitVCSRootBuilder AgentCleanPolicy(AgentCleanPolicy value)
        {
            AppendProperty(value);
            return this;
        }

        public IGitVCSRootBuilder AuthMethod(AuthMethod value)
        {
            AppendProperty(value);
            return this;
        }

        public IGitVCSRootBuilder Branch(string value)
        {
            AppendProperty("branch", value);
            return this;
        }

        public IGitVCSRootBuilder IgnoreKnownHosts(bool value)
        {
            AppendProperty("ignoreKnownHosts", value.ToString(CultureInfo.InvariantCulture).ToLower());
            return this;
        }

        public IGitVCSRootBuilder UserNameStyle(UserNameStyle value)
        {
            AppendProperty(value);
            return this;
        }

        public IGitVCSRootBuilder Username(string value)
        {
            AppendProperty("username", value);
            return this;
        }

        public IGitVCSRootBuilder Password(string value)
        {
            AppendProperty("secure:password", value);
            return this;
        }

        public IGitVCSRootBuilder UseAlternates(bool value)
        {
            AppendProperty("useAlternates", value.ToString(CultureInfo.InvariantCulture).ToLower());
            return this;
        }

        public IGitVCSRootBuilder Url(Uri value)
        {
            AppendProperty("url", value.ToString());
            return this;
        }

        public IGitVCSRootBuilder BranchSpec(string value)
        {
            AppendProperty("teamcity:branchSpec", value);
            return this;
        }

        public IGitVCSRootBuilder SubModuleCheckout(bool value)
        {
            AppendProperty("submoduleCheckout", (value ? "CHECKOUT" : "IGNORE"));
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
    }
}
