using FluentAssertions;
using FluentTc.Locators;
using NUnit.Framework;

namespace FluentTc.Tests.Locators
{
    [TestFixture]
    public class VCSRootEntryBuilderTests
    {
        [Test]
        [TestCase("id")]
        public void Id(string id)
        {
            var vcsHavingBuilder = new VCSRootEntryBuilder();

            vcsHavingBuilder
                .Id(id);

            vcsHavingBuilder.GetVCSRootEntry().VcsRoot.Should().NotBeNull();
            vcsHavingBuilder.GetVCSRootEntry().VcsRoot.Id.Should().Be(id);
        }

        [Test]
        [TestCase("id", "checkoutRules")]
        public void IdAndCheckoutRules(string id, string checkoutRules)
        {
            var vcsHavingBuilder = new VCSRootEntryBuilder();

            vcsHavingBuilder
                .Id(id)
                .CheckoutRules(checkoutRules);

            vcsHavingBuilder.GetVCSRootEntry().VcsRoot.Should().NotBeNull();
            vcsHavingBuilder.GetVCSRootEntry().VcsRoot.Id.Should().Be(id);
            vcsHavingBuilder.GetVCSRootEntry().CheckoutRules.Should().Be(checkoutRules);
        }

        [Test]
        [TestCase("checkoutRules")]
        public void CheckoutRules(string checkoutRules)
        {
            var vcsHavingBuilder = new VCSRootEntryBuilder();

            vcsHavingBuilder
                .CheckoutRules(checkoutRules);

            vcsHavingBuilder.GetVCSRootEntry().VcsRoot.Should().NotBeNull();
            vcsHavingBuilder.GetVCSRootEntry().VcsRoot.Id.Should().BeNull();
            vcsHavingBuilder.GetVCSRootEntry().CheckoutRules.Should().Be(checkoutRules);
        }
    }
}
