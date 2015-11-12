using FakeItEasy;
using FluentTc.Domain;
using FluentTc.Engine;
using FluentTc.Locators;
using NUnit.Framework;

namespace FluentTc.Tests
{
    [TestFixture]
    public class BuildsRetrieverTests
    {
        [Test]
        public void GetBuilds_ByIdAllDefaultColumns_GeFotmatCalled()
        {
            // Arrange
            var teamCityCaller = A.Fake<ITeamCityCaller>();
            A.CallTo(
                () =>
                    teamCityCaller.GetFormat<BuildWrapper>(
                        "/app/rest/builds?locator={0},&fields=count,build({1})",
                        A<object[]>._))
                .Returns(new BuildWrapper() {Count = "0"});

            var buildHavingBuilder = A.Fake<IBuildHavingBuilder>();
            A.CallTo(() => buildHavingBuilder.GetLocator()).Returns("id:123");

            var buildHavingBuilderFactory = A.Fake<IBuildHavingBuilderFactory>();
            A.CallTo(() => buildHavingBuilderFactory.CreateBuildHavingBuilder()).Returns(buildHavingBuilder);

            var countBuilder = A.Fake<ICountBuilder>();
            A.CallTo(() => countBuilder.GetCount()).Returns(string.Empty);

            var countBuilderFactory = A.Fake<ICountBuilderFactory>();
            A.CallTo(() => countBuilderFactory.CreateCountBuilder()).Returns(countBuilder);

            var buildIncludeBuilder = A.Fake<IBuildIncludeBuilder>();
            A.CallTo(() => buildIncludeBuilder.GetColumns()).Returns("buildTypeId,href,id,number,state,status,webUrl");

            var buildIncludeBuilderFactory = A.Fake<IBuildIncludeBuilderFactory>();
            A.CallTo(() => buildIncludeBuilderFactory.CreateBuildIncludeBuilder()).Returns(buildIncludeBuilder);

            var buildsRetriever = new BuildsRetriever(teamCityCaller, buildHavingBuilderFactory, countBuilderFactory, buildIncludeBuilderFactory,A.Fake<IQueueHavingBuilderFactory>());

            // Act
            var builds = buildsRetriever.GetBuilds(_ => _.Id(123), _ => _.DefaultCount(), _ => _.IncludeDefaults());

            // Assert
            A.CallTo(
                () =>
                    teamCityCaller.GetFormat<BuildWrapper>(
                        "/app/rest/builds?locator={0},&fields=count,build({1})",
                        A<object[]>.That.IsSameSequenceAs(new object[] { "id:123", "buildTypeId,href,id,number,state,status,webUrl" })))
                .MustHaveHappened(Repeated.Exactly.Once);
        }

        [Test]
        public void GetBuilds_ByBuildConfigurationTake5DefaultColumns_GetFotmatCalled()
        {
            // Arrange
            var teamCityCaller = A.Fake<ITeamCityCaller>();
            A.CallTo(
                () =>
                    teamCityCaller.GetFormat<BuildWrapper>(
                        "/app/rest/builds?locator={0},{1},&fields=count,build({2})",
                        A<object[]>._))
                .Returns(new BuildWrapper() {Count = "0"});

            var buildHavingBuilder = A.Fake<IBuildHavingBuilder>();
            A.CallTo(() => buildHavingBuilder.GetLocator()).Returns("buildType:name:FluentTc");

            var buildHavingBuilderFactory = A.Fake<IBuildHavingBuilderFactory>();
            A.CallTo(() => buildHavingBuilderFactory.CreateBuildHavingBuilder()).Returns(buildHavingBuilder);

            var countBuilder = A.Fake<ICountBuilder>();
            A.CallTo(() => countBuilder.GetCount()).Returns("count:5");

            var countBuilderFactory = A.Fake<ICountBuilderFactory>();
            A.CallTo(() => countBuilderFactory.CreateCountBuilder()).Returns(countBuilder);

            var buildIncludeBuilder = A.Fake<IBuildIncludeBuilder>();
            A.CallTo(() => buildIncludeBuilder.GetColumns()).Returns("buildTypeId,href,id,number,state,status,webUrl");

            var buildIncludeBuilderFactory = A.Fake<IBuildIncludeBuilderFactory>();
            A.CallTo(() => buildIncludeBuilderFactory.CreateBuildIncludeBuilder()).Returns(buildIncludeBuilder);

            var buildsRetriever = new BuildsRetriever(teamCityCaller, buildHavingBuilderFactory, countBuilderFactory, buildIncludeBuilderFactory,A.Fake<IQueueHavingBuilderFactory>());

            // Act
            var builds = buildsRetriever.GetBuilds(_ => _.BuildConfiguration(b=>b.Name("FluentTc")), _ => _.Count(5), _ => _.IncludeDefaults());

            // Assert
            A.CallTo(
                () =>
                    teamCityCaller.GetFormat<BuildWrapper>(
                        "/app/rest/builds?locator={0},{1},&fields=count,build({2})",
                        A<object[]>.That.IsSameSequenceAs(new object[] { "buildType:name:FluentTc", "count:5", "buildTypeId,href,id,number,state,status,webUrl" })))
                .MustHaveHappened(Repeated.Exactly.Once);
        }
    }
}