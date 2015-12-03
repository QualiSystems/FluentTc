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
            // Arrange
            var branchHavingBuilder = new BranchHavingBuilder();

            // Act
            branchHavingBuilder.Name("Branch1");

            // Assert
            branchHavingBuilder.GetLocator().Should().Be("name:Branch1");
        }

        [Test]
        public void Branched()
        {
            var branchHavingBuilder = new BranchHavingBuilder();

            branchHavingBuilder.Branched();

            branchHavingBuilder.GetLocator().Should().Be("branched:True");
        }

        [Test]
        public void NotBranched()
        {
            var branchHavingBuilder = new BranchHavingBuilder();

            branchHavingBuilder.NotBranched();

            branchHavingBuilder.GetLocator().Should().Be("branched:False");
        }

        [Test]
        public void Default()
        {
            var branchHavingBuilder = new BranchHavingBuilder();

            branchHavingBuilder.Default();

            branchHavingBuilder.GetLocator().Should().Be("default:True");
        }

        [Test]
        public void DefaultFalse()
        {
            var branchHavingBuilder = new BranchHavingBuilder();

            branchHavingBuilder.NotDefault();

            branchHavingBuilder.GetLocator().Should().Be("default:False");
        }

        [Test]
        public void Unspecified()
        {
            var branchHavingBuilder = new BranchHavingBuilder();

            branchHavingBuilder.Unspecified();

            branchHavingBuilder.GetLocator().Should().Be("unspecified:True");
        }

        [Test]
        public void NotUnspecified()
        {
            var branchHavingBuilder = new BranchHavingBuilder();

            branchHavingBuilder.NotUnspecified();

            branchHavingBuilder.GetLocator().Should().Be("unspecified:False");
        }
    }
}