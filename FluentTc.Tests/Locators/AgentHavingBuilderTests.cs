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
            agentHavingBuilder.Connected();

            // Assert
            agentHavingBuilder.GetLocator().Should().Be("connected:True");
        }

        [Test]
        public void Disabled()
        {
            // Arrange
            var fixture = Auto.Fixture();
            var agentHavingBuilder = fixture.Create<AgentHavingBuilder>();

            // Act
            agentHavingBuilder.Disabled();

            // Assert
            agentHavingBuilder.GetLocator().Should().Be("enabled:False");
        }

        [Test]
        public void Authorized()
        {
            // Arrange
            var fixture = Auto.Fixture();
            var agentHavingBuilder = fixture.Create<AgentHavingBuilder>();

            // Act
            agentHavingBuilder.Authorized();

            // Assert
            agentHavingBuilder.GetLocator().Should().Be("authorized:True");
        }

        [Test]
        public void Name()
        {
            // Arrange
            var fixture = Auto.Fixture();
            var agentHavingBuilder = fixture.Create<AgentHavingBuilder>();

            // Act
            agentHavingBuilder.Name("agent1");

            // Assert
            agentHavingBuilder.GetLocator().Should().Be("name:agent1");
        }

        [Test]
        public void Disconnected()
        {
            // Arrange
            var fixture = Auto.Fixture();
            var agentHavingBuilder = fixture.Create<AgentHavingBuilder>();

            // Act
            agentHavingBuilder.Disconnected();

            // Assert
            agentHavingBuilder.GetLocator().Should().Be("connected:False");

        
        }
        [Test]
        public void NotAuthorized()
        {
            // Arrange
            var fixture = Auto.Fixture();
            var agentHavingBuilder = fixture.Create<AgentHavingBuilder>();

            // Act
            agentHavingBuilder.NotAuthorized();

            // Assert
            agentHavingBuilder.GetLocator().Should().Be("authorized:False");
        }

        [Test]
        public void Ip()
        {
            // Arrange
            var fixture = Auto.Fixture();
            var agentHavingBuilder = fixture.Create<AgentHavingBuilder>();

            // Act
            agentHavingBuilder.Ip("127.0.0.1");

            // Assert
            agentHavingBuilder.GetLocator().Should().Be("ip:127.0.0.1");
        }
    }
}