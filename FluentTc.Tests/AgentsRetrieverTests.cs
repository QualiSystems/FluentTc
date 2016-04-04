using System;
using System.Collections.Generic;
using FakeItEasy;
using FluentAssertions;
using FluentTc.Domain;
using FluentTc.Engine;
using FluentTc.Exceptions;
using FluentTc.Locators;
using NUnit.Framework;

namespace FluentTc.Tests
{
    [TestFixture]
    public class AgentsRetrieverTests
    {
        [Test]
        public void GetAgents_AllAuthorized_GetFormatCalled()
        {
            // Arrange
            var teamCityCaller = A.Fake<ITeamCityCaller>();
            A.CallTo(
                () =>
                    teamCityCaller.GetFormat<AgentWrapper>(
                        "/app/rest/agents?locator={0}",
                        A<object[]>._))
                .Returns(new AgentWrapper() { Count = "0" });

            var agentHavingBuilderFactory = A.Fake<IAgentHavingBuilderFactory>();
            A.CallTo(() => agentHavingBuilderFactory.CreateAgentHavingBuilder()).Returns(new AgentHavingBuilder());

            var agentsRetriever = new AgentsRetriever(teamCityCaller, agentHavingBuilderFactory);

            // Act
            var agents = agentsRetriever.GetAgents(_ => _.Authorized());

            // Assert
            A.CallTo(
                () =>
                    teamCityCaller.GetFormat<AgentWrapper>(
                        "/app/rest/agents?locator={0}",
                        A<object[]>.That.IsSameSequenceAs(new object[] { "authorized:True" })))
                .MustHaveHappened(Repeated.Exactly.Once);
        }

        [Test]
        public void GetAgent_EnabledTwoAgents_MoreThanOneAgentFoundExceptionThrown()
        {
            // Arrange
            var teamCityCaller = A.Fake<ITeamCityCaller>();
            A.CallTo(
                () =>
                    teamCityCaller.GetFormat<AgentWrapper>(
                        "/app/rest/agents?locator={0}",
                        A<object[]>.That.IsSameSequenceAs(new[] {"enabled:True"})))
                .Returns(new AgentWrapper { Agent = new List<Agent>(new[]{new Agent(), new Agent()}) ,Count = "2" });

            var agentHavingBuilderFactory = A.Fake<IAgentHavingBuilderFactory>();
            A.CallTo(() => agentHavingBuilderFactory.CreateAgentHavingBuilder()).Returns(new AgentHavingBuilder());

            var agentsRetriever = new AgentsRetriever(teamCityCaller, agentHavingBuilderFactory);

            // Act
            Action action = () => agentsRetriever.GetAgent(_ => _.Enabled());

            // Assert
            action.ShouldThrow<MoreThanOneAgentFoundException>();
        }

        [Test]
        public void GetAgent_DisabledNoAgents_MoreThanOneAgentFoundExceptionThrown()
        {
            // Arrange
            var teamCityCaller = A.Fake<ITeamCityCaller>();
            A.CallTo(
                () =>
                    teamCityCaller.GetFormat<AgentWrapper>(
                        "/app/rest/agents?locator={0}",
                        A<object[]>.That.IsSameSequenceAs(new[] {"enabled:False"})))
                .Returns(new AgentWrapper { Agent = new List<Agent>() ,Count = "0" });

            var agentHavingBuilderFactory = A.Fake<IAgentHavingBuilderFactory>();
            A.CallTo(() => agentHavingBuilderFactory.CreateAgentHavingBuilder()).Returns(new AgentHavingBuilder());

            var agentsRetriever = new AgentsRetriever(teamCityCaller, agentHavingBuilderFactory);

            // Act
            Action action = () => agentsRetriever.GetAgent(_ => _.Disabled());

            // Assert
            action.ShouldThrow<AgentNotFoundException>();
        }
    }
}