using FluentAssertions;
using FluentTc.Locators;
using NUnit.Framework;

namespace FluentTc.Tests.Locators
{
    [TestFixture]
    public class BuildQueueIdHavingBuilderTests
    {
        [Test]
        public void GetLocator_Id()
        {
            var buildQueueIdHavingBuilder = new BuildQueueIdHavingBuilder();
            var queueIdHavingBuilder = buildQueueIdHavingBuilder.Id(123);

            var locator = queueIdHavingBuilder.GetLocator();

            locator.Should().Be("id:123");
        }
    }
}