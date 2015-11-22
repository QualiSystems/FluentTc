using FluentAssertions;
using FluentTc.Locators;
using NUnit.Framework;

namespace FluentTc.Tests.Locators
{
    [TestFixture]
    public class UserHavingBuilderTests
    {
        [Test]
        public void InternalUserId()
        {
            var userHavingBuilder = new UserHavingBuilder();
            var havingBuilder = userHavingBuilder.Id("123");

            var locator = ((IUserHavingBuilder) havingBuilder).GetLocator();

            locator.Should().Be("id:123");
        }

        [Test]
        public void Username()
        {
            var userHavingBuilder = new UserHavingBuilder();
            var havingBuilder = userHavingBuilder.Username("chuck");

            var locator = ((IUserHavingBuilder) havingBuilder).GetLocator();

            locator.Should().Be("username:chuck");
        }
    }
}