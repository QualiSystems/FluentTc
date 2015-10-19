using FluentAssertions;
using FluentTc.Locators;
using NUnit.Framework;

namespace FluentTc.Tests.Locators
{
    [TestFixture]
    public class BranchHavingBuilderTests
    {
        [Test]
        public void Name()
        {
            var branchHavingBuilder = new BranchHavingBuilder();
            var havingBuilder = branchHavingBuilder.Name("Branch1");

            var locator = havingBuilder.GetLocator();

            locator.Should().Be("name:Branch1");
        }

        [Test]
        public void Branched()
        {
            var branchHavingBuilder = new BranchHavingBuilder();
            var havingBuilder = branchHavingBuilder.Branched();

            var locator = havingBuilder.GetLocator();

            locator.Should().Be("branched:True");
        }

        [Test]
        public void NotBranched()
        {
            var branchHavingBuilder = new BranchHavingBuilder();
            var havingBuilder = branchHavingBuilder.NotBranched();

            var locator = havingBuilder.GetLocator();

            locator.Should().Be("branched:False");
        }

        [Test]
        public void Default()
        {
            var branchHavingBuilder = new BranchHavingBuilder();
            var havingBuilder = branchHavingBuilder.Default();

            var locator = havingBuilder.GetLocator();

            locator.Should().Be("default:True");
        }

        [Test]
        public void DefaultFalse()
        {
            var branchHavingBuilder = new BranchHavingBuilder();
            var havingBuilder = branchHavingBuilder.NotDefault();

            var locator = havingBuilder.GetLocator();

            locator.Should().Be("default:False");
        }

        [Test]
        public void Unspecified()
        {
            var branchHavingBuilder = new BranchHavingBuilder();
            var havingBuilder = branchHavingBuilder.Unspecified();

            var locator = havingBuilder.GetLocator();

            locator.Should().Be("unspecified:True");
        }

        [Test]
        public void NotUnspecified()
        {
            var branchHavingBuilder = new BranchHavingBuilder();
            var havingBuilder = branchHavingBuilder.NotUnspecified();

            var locator = havingBuilder.GetLocator();

            locator.Should().Be("unspecified:False");
        }
    }
}