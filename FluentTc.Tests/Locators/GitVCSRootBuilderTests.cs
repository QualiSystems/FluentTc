using FluentAssertions;
using FluentTc.Locators;
using NUnit.Framework;
using System;
using System.Linq;

namespace FluentTc.Tests.Locators
{
    [TestFixture]
    public class GitVCSRootBuilderTests
    {
        [Test]
        public void SimpleCreation()
        {
            var vcsRootBuilder = new GitVCSRootBuilder();
            var url = new Uri("http://www.google.com");

            vcsRootBuilder
                .AgentCleanFilePolicy(AgentCleanFilePolicy.AllIgnoredUntrackedFiles)
                .AgentCleanPolicy(AgentCleanPolicy.Always)
                .AuthMethod(AuthMethod.Anonymous)
                .Branch("branch")
                .BranchSpec("branchSpec")
                .CheckoutSubModule()
                .Id("id")
                .IgnoreKnownHosts()
                .Name("name")
                .Password("password")
                .ProjectId("projectId")
                .Url(url)
                .UseAlternates()
                .Username("username")
                .UserNameStyle(UserNameStyle.AuthorName);

            var vcsRoot = vcsRootBuilder.GetVCSRoot();
            vcsRoot.Id.Should().Be("id");
            vcsRoot.Name.Should().Be("name");
            vcsRoot.Project.Id.Should().Be("projectId");
            var properties = vcsRoot.Properties.Property;
            properties.FirstOrDefault(
                p => p.Name == "agentCleanFilePolicy" && 
                p.Value == "ALL_IGNORED_UNTRACKED_FILES").Should().NotBeNull();
            properties.FirstOrDefault(
                p => p.Name == "agentCleanPolicy" &&
                p.Value == "ALWAYS").Should().NotBeNull();
            properties.FirstOrDefault(
                p => p.Name == "authMethod" &&
                p.Value == "ANONYMOUS").Should().NotBeNull();
            properties.FirstOrDefault(
                p => p.Name == "branch" &&
                p.Value == "branch").Should().NotBeNull();
            properties.FirstOrDefault(
                p => p.Name == "teamcity:branchSpec" &&
                p.Value == "branchSpec").Should().NotBeNull();
            properties.FirstOrDefault(
                p => p.Name == "submoduleCheckout" &&
                p.Value == "CHECKOUT").Should().NotBeNull();
            properties.FirstOrDefault(
                p => p.Name == "ignoreKnownHosts" &&
                p.Value == "true").Should().NotBeNull();
            properties.FirstOrDefault(
                p => p.Name == "secure:password" &&
                p.Value == "password").Should().NotBeNull();
            properties.FirstOrDefault(
                p => p.Name == "url" &&
                p.Value == url.ToString()).Should().NotBeNull();
            properties.FirstOrDefault(
                p => p.Name == "useAlternates" &&
                p.Value == "true").Should().NotBeNull();
            properties.FirstOrDefault(
                p => p.Name == "username" &&
                p.Value == "username").Should().NotBeNull();
            properties.FirstOrDefault(
                p => p.Name == "userNameStyle" &&
                p.Value == "AUTHOR_NAME").Should().NotBeNull();
        }
    }
}
