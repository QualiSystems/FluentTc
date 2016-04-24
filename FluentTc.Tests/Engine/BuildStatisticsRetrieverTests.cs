using FakeItEasy;
using FluentTc.Domain;
using FluentTc.Engine;
using FluentTc.Locators;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;

namespace FluentTc.Tests.Engine
{
    [TestFixture]
    public class BuildStatisticsRetrieverTests
    {
        [Test]
        public void GetStatistics_ByBuild_ShouldReturnZeroResults()
        {
            // Arrange
            var teamCityCaller = A.Fake<ITeamCityCaller>();
            var buildStatisticsModel = new BuildStatisticsModel { Count = "0", Property = { } };
            A.CallTo(
               () =>
                   teamCityCaller.GetFormat<BuildStatisticsModel>(
                       "/app/rest/builds/{0}/statistics",
                       A<object[]>._))
               .Returns(buildStatisticsModel);

            var buildHavingBuilder = A.Fake<BuildHavingBuilder>();
            A.CallTo(() => buildHavingBuilder.GetLocator()).Returns("buildId:123");
            var buildHavingBuilderFactory = A.Fake<IBuildHavingBuilderFactory>();
            A.CallTo(() => buildHavingBuilderFactory.CreateBuildHavingBuilder()).Returns(buildHavingBuilder);

            var buildStatisticConverter = A.Fake<IBuildStatisticConverter>();
            A.CallTo(() => buildStatisticConverter.Convert(buildStatisticsModel)).Returns(new List<IBuildStatistic>());

            var statisticsRetriever = new BuildStatisticsRetriever(teamCityCaller, buildHavingBuilderFactory, buildStatisticConverter);

            // Act
            var statistics = statisticsRetriever.GetBuildStatistics(_ => _.Id(123));

            // Assert
            A.CallTo(
                () =>
                    teamCityCaller.GetFormat<BuildStatisticsModel>(
                       "/app/rest/builds/{0}/statistics",
                        A<object[]>.That.IsSameSequenceAs(new object[] { "buildId:123" })))
                .MustHaveHappened(Repeated.Exactly.Once);

            statistics.Should().BeEmpty();
        }

        [Test]
        public void GetStatistics_ByBuild_ShouldReturnResultsWithProperties()
        {
            // Arrange
            var mockPropertyList = new List<Property> { 
                        new Property { Name = "MockProperty1", Value = "MockValue1" }, 
                        new Property { Name = "MockProperty2", Value = "MockValue2" } 
            };
            var buildStatisticsModel = new BuildStatisticsModel { Count = "2", Property = mockPropertyList };
            
            var teamCityCaller = A.Fake<ITeamCityCaller>();
            A.CallTo(
               () =>
                   teamCityCaller.GetFormat<BuildStatisticsModel>(
                       "/app/rest/builds/{0}/statistics",
                       A<object[]>._))
               .Returns(buildStatisticsModel);

            var buildHavingBuilder = A.Fake<BuildHavingBuilder>();
            A.CallTo(() => buildHavingBuilder.GetLocator()).Returns("buildId:123");
            var buildHavingBuilderFactory = A.Fake<IBuildHavingBuilderFactory>();
            A.CallTo(() => buildHavingBuilderFactory.CreateBuildHavingBuilder()).Returns(buildHavingBuilder);

            var buildStatisticConverter = A.Fake<IBuildStatisticConverter>();
            A.CallTo(() => buildStatisticConverter.Convert(buildStatisticsModel)).Returns(new List<IBuildStatistic>()
            {
                new BuildStatistic("MockProperty1", "MockValue1"),
                new BuildStatistic("MockProperty2", "MockValue2")
            });

            var statisticsRetriever = new BuildStatisticsRetriever(teamCityCaller, buildHavingBuilderFactory, buildStatisticConverter);

            // Act
            var statistics = statisticsRetriever.GetBuildStatistics(_ => _.Id(1));

            // Assert
            A.CallTo(
                () =>
                    teamCityCaller.GetFormat<BuildStatisticsModel>(
                       "/app/rest/builds/{0}/statistics",
                        A<object[]>.That.IsSameSequenceAs(new object[] { "buildId:123" })))
                .MustHaveHappened(Repeated.Exactly.Once);
            statistics.Count.Should().Be(2);
            statistics.First().Name.Should().Be("MockProperty1");
            statistics.First().Value.Should().Be("MockValue1");
            statistics.Last().Name.Should().Be("MockProperty2");
            statistics.Last().Value.Should().Be("MockValue2");
        }
    }
}
