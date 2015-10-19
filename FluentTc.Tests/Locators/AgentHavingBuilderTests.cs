using FluentAssertions;
using FluentTc.Locators;
using NUnit.Framework;
using Ploeh.AutoFixture;

namespace FluentTc.Tests.Locators
{
    [TestFixture]
    public class AgentHavingBuilderTests
    {
        [Test]
        public void Connected()
        {
            // Arrange
            var fixture = Auto.Fixture();
            var agentHavingBuilder = fixture.Create<AgentHavingBuilder>();

            // Act
            var havingBuilder = agentHavingBuilder.Connected();

            // Assert
            ((IAgentHavingBuilder)havingBuilder).GetLocator().Should().Be("connected:True");
        }

        [Test]
        public void Authorized()
        {
            // Arrange
            var fixture = Auto.Fixture();
            var agentHavingBuilder = fixture.Create<AgentHavingBuilder>();

            // Act
            var havingBuilder = agentHavingBuilder.Authorized();

            // Assert
            ((IAgentHavingBuilder)havingBuilder).GetLocator().Should().Be("authorized:True");
        }
    }
}