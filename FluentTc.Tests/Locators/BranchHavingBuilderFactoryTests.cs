using FluentAssertions;
using FluentTc.Locators;
using NUnit.Framework;

namespace FluentTc.Tests.Locators
{
    [TestFixture]
    public class BranchHavingBuilderFactoryTests
    {
        [Test]
        public void CreateBranchHavingBuilder_None_NotNull()
        {
            // Arrange
            var branchHavingBuilderFactory = new BranchHavingBuilderFactory();

            // Act
            var branchHavingBuilder = branchHavingBuilderFactory.CreateBranchHavingBuilder();

            // Assert
            branchHavingBuilder.Should().NotBeNull();
        }
    }
}

