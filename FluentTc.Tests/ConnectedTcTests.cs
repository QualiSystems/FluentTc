using System;
using System.Collections.Generic;
using FakeItEasy;
using FluentAssertions;
using FluentTc.Domain;
using FluentTc.Locators;
using FluentTc.Tests.Locators;
using NUnit.Framework;
using Ploeh.AutoFixture;

namespace FluentTc.Tests
{
    [TestFixture]
    public class ConnectedTcTests
    {
        [Test]
        public void GetBuild_NoBuildsFound_BuildNotFoundExceptionThrown()
        {
            // Arrange
            Action<IBuildHavingBuilder> having = _ => _.Id(11);

            var fixture = Auto.Fixture();
            var buildsRetriever = fixture.Freeze<IBuildsRetriever>();
            A.CallTo(() => buildsRetriever.GetBuilds(having, A<Action<ICountBuilder>>._, A<Action<IBuildIncludeBuilder>>._))
                .Returns(new List<Build>());

            var connectedTc = fixture.Create<ConnectedTc>();

            // Act
            Action action = () => connectedTc.GetBuild(having);

            // Assert
            action.ShouldThrow<BuildNotFoundException>();
        }

        [Test]
        public void GetBuild_TwoBuilds_MoreThanOneBuildFoundExceptionThrown()
        {
            // Arrange
            Action<IBuildHavingBuilder> having = _ => _.AgentName("Bond");

            var fixture = Auto.Fixture();
            var buildsRetriever = fixture.Freeze<IBuildsRetriever>();
            A.CallTo(() => buildsRetriever.GetBuilds(having, A<Action<ICountBuilder>>._, A<Action<IBuildIncludeBuilder>>._))
                .Returns(new List<Build>(new [] { new Build(), new Build() }));

            var connectedTc = fixture.Create<ConnectedTc>();

            // Act
            Action action = () => connectedTc.GetBuild(having);

            // Assert
            action.ShouldThrow<MoreThanOneBuildFoundException>();
        }
    }
}