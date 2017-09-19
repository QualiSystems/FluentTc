using FluentAssertions;
using FluentTc.Locators;
using NUnit.Framework;
using System;

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
            properties.Should().ContainSingle(
                p => p.Name == "agentCleanPolicy" &&
                     p.Value == "ALWAYS");
            properties.Should().ContainSingle(
                p => p.Name == "authMethod" &&
                     p.Value == "ANONYMOUS");
            properties.Should().ContainSingle(
                p => p.Name == "branch" &&
                     p.Value == "branch");
            properties.Should().ContainSingle(
                p => p.Name == "teamcity:branchSpec" &&
                     p.Value == "branchSpec");
            properties.Should().ContainSingle(
                p => p.Name == "submoduleCheckout" &&
                     p.Value == "CHECKOUT");
            properties.Should().ContainSingle(
                p => p.Name == "ignoreKnownHosts" &&
                     p.Value == "true");
            properties.Should().ContainSingle(
                p => p.Name == "secure:password" &&
                     p.Value == "password");
            properties.Should().ContainSingle(
                p => p.Name == "url" &&
                     p.Value == url.ToString());
            properties.Should().ContainSingle(
                p => p.Name == "useAlternates" &&
                     p.Value == "true");
            properties.Should().ContainSingle(
                p => p.Name == "username" &&
                     p.Value == "username");
            properties.Should().ContainSingle(
                p => p.Name == "userNameStyle" &&
                     p.Value == "AUTHOR_NAME");
        }

        [Test]
        public void GetVCSRoot_SshKey_SshKeyCreated()
        {
            // Act
            var vcsRootBuilder = new GitVCSRootBuilder();
            vcsRootBuilder
                .AuthMethod(AuthMethod.TeamcitySshKey)
                .UploadedKey("keyName");
            var vcsRoot = vcsRootBuilder.GetVCSRoot();

            // Assert
            vcsRoot.Properties.Property.Should()
                .ContainSingle(p => p.Name == "authMethod" && p.Value == "TEAMCITY_SSH_KEY");
            vcsRoot.Properties.Property.Should().ContainSingle(p => p.Name == "teamcitySshKey" && p.Value == "keyName");
        }
    }
}
