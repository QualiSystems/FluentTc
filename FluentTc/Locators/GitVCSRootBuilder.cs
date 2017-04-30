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
        IGitVCSRootBuilder IgnoreKnownHosts();
        IGitVCSRootBuilder DontIgnoreKnownHosts();
        IGitVCSRootBuilder UserNameStyle(UserNameStyle value);
        IGitVCSRootBuilder Username(string value);
        IGitVCSRootBuilder Password(string value);
        IGitVCSRootBuilder UseAlternates();
        IGitVCSRootBuilder DontUseAlternates();
        IGitVCSRootBuilder Url(Uri value);
        IGitVCSRootBuilder BranchSpec(string value);
        IGitVCSRootBuilder CheckoutSubModule();
        IGitVCSRootBuilder DontCheckoutSubModule();

        IGitVCSRootBuilder Project(Project value);

    }

    internal class GitVCSRootBuilder: IGitVCSRootBuilder
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

        public IGitVCSRootBuilder IgnoreKnownHosts()
        {
            AppendProperty("ignoreKnownHosts", "true");
            return this;
        }

        public IGitVCSRootBuilder DontIgnoreKnownHosts()
        {
            AppendProperty("ignoreKnownHosts", "false");
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

        public IGitVCSRootBuilder DontUseAlternates()
        {
            AppendProperty("useAlternates", "false");
            return this;
        }

        public IGitVCSRootBuilder UseAlternates()
        {
            AppendProperty("useAlternates", "true");
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

        public IGitVCSRootBuilder CheckoutSubModule()
        {
            AppendProperty("submoduleCheckout", "CHECKOUT");
            return this;
        }

        public IGitVCSRootBuilder DontCheckoutSubModule()
        {
            AppendProperty("submoduleCheckout", "IGNORE");
            return this;
        }

        public IGitVCSRootBuilder Project(Project value)
        {
            m_VCSRoot.Project = value;
            return this;
        }

        internal VcsRoot GetVCSRoot()
        {
            return m_VCSRoot;
        }
    }
}
