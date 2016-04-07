using FakeItEasy;
using FluentTc.Domain;
using FluentTc.Engine;
using FluentTc.Locators;
using NUnit.Framework;
using System.Collections.Generic;

namespace FluentTc.Tests
{
    [TestFixture]
    public class StatisticsRetrieverTests
    {
        [Test]
        public void GetStatistics_ByBuild_ShouldReturnZeroResults()
        {
            // Arrange
            var teamCityCaller = A.Fake<ITeamCityCaller>();
            A.CallTo(
               () =>
                   teamCityCaller.GetFormat<Statistics>(
                       "/app/rest/builds/{0}/statistics",
                       A<object[]>._))
               .Returns(new Statistics { Count = "0", Property = { } });

            var buildHavingBuilder = A.Fake<BuildHavingBuilder>();
            A.CallTo(() => buildHavingBuilder.GetLocator()).Returns("buildId:123");
            var buildHavingBuilderFactory = A.Fake<IBuildHavingBuilderFactory>();
            A.CallTo(() => buildHavingBuilderFactory.CreateBuildHavingBuilder()).Returns(buildHavingBuilder);

            var statisticsRetriever = new StatisticsRetriever(teamCityCaller, buildHavingBuilderFactory);

            // Act
            var statistics = statisticsRetriever.GetStatistics(_ => _.Id(123));

            // Assert
            A.CallTo(
                () =>
                    teamCityCaller.GetFormat<Statistics>(
                       "/app/rest/builds/{0}/statistics",
                        A<object[]>.That.IsSameSequenceAs(new object[] { "buildId:123" })))
                .MustHaveHappened(Repeated.Exactly.Once);
        }

        [Test]
        public void GetStatistics_ByBuild_ShouldReturnResultsWithProperties()
        {
            // Arrange
            var mockPropertyList = new List<Property> { 
                        new Property { Name = "MockProperty1", Value = "MockValue1" }, 
                        new Property { Name = "MockProperty2", Value = "MockValue2" } 
            };
            var mockStatistics = new Statistics { Count = "2", Property = mockPropertyList };
            
            var teamCityCaller = A.Fake<ITeamCityCaller>();
            A.CallTo(
               () =>
                   teamCityCaller.GetFormat<Statistics>(
                       "/app/rest/builds/{0}/statistics",
                       A<object[]>._))
               .Returns(mockStatistics);

            var buildHavingBuilder = A.Fake<BuildHavingBuilder>();
            A.CallTo(() => buildHavingBuilder.GetLocator()).Returns("buildId:123");
            var buildHavingBuilderFactory = A.Fake<IBuildHavingBuilderFactory>();
            A.CallTo(() => buildHavingBuilderFactory.CreateBuildHavingBuilder()).Returns(buildHavingBuilder);

            var statisticsRetriever = new StatisticsRetriever(teamCityCaller, buildHavingBuilderFactory);

            // Act
            var statistics = statisticsRetriever.GetStatistics(_ => _.Id(1));

            // Assert
            A.CallTo(
                () =>
                    teamCityCaller.GetFormat<Statistics>(
                       "/app/rest/builds/{0}/statistics",
                        A<object[]>.That.IsSameSequenceAs(new object[] { "buildId:123" })))
                .MustHaveHappened(Repeated.Exactly.Once);
            Assert.AreEqual("MockValue1", statistics.Property[0].Value);
            Assert.AreEqual("MockValue2", statistics.Property[1].Value);
        }
    }
}
