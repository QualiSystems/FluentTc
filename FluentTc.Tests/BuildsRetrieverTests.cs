using FakeItEasy;
using FluentTc.Domain;
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
                        "/app/rest/builds?locator={0},{1},&fields=count,build({2})",
                        A<object[]>._))
                .Returns(new BuildWrapper() {Count = "0"});

            var buildHavingBuilder = A.Fake<IBuildHavingBuilder>();
            A.CallTo(() => buildHavingBuilder.GetLocator()).Returns("id:123");

            var buildHavingBuilderFactory = A.Fake<IBuildHavingBuilderFactory>();
            A.CallTo(() => buildHavingBuilderFactory.CreateBuildHavingBuilder()).Returns(buildHavingBuilder);

            var countBuilder = A.Fake<ICountBuilder>();
            A.CallTo(() => countBuilder.GetCount()).Returns("count:-1");

            var countBuilderFactory = A.Fake<ICountBuilderFactory>();
            A.CallTo(() => countBuilderFactory.CreateCountBuilder()).Returns(countBuilder);

            var buildIncludeBuilder = A.Fake<IBuildIncludeBuilder>();
            A.CallTo(() => buildIncludeBuilder.GetColumns()).Returns("buildTypeId,href,id,number,state,status,webUrl");

            var buildIncludeBuilderFactory = A.Fake<IBuildIncludeBuilderFactory>();
            A.CallTo(() => buildIncludeBuilderFactory.CreateBuildIncludeBuilder()).Returns(buildIncludeBuilder);

            var buildProjectHavingBuilderFactory = A.Fake<IBuildProjectHavingBuilderFactory>();

            var buildsRetriever = new BuildsRetriever(teamCityCaller, buildHavingBuilderFactory, countBuilderFactory, buildIncludeBuilderFactory, buildProjectHavingBuilderFactory);

            // Act
            var builds = buildsRetriever.GetBuilds(_ => _.Id(123), _ => _.All(), _ => _.IncludeDefaults());

            // Assert
            A.CallTo(
                () =>
                    teamCityCaller.GetFormat<BuildWrapper>(
                        "/app/rest/builds?locator={0},{1},&fields=count,build({2})",
                        A<object[]>.That.IsSameSequenceAs(new object[] { "id:123", "count:-1", "buildTypeId,href,id,number,state,status,webUrl" })))
                .MustHaveHappened(Repeated.Exactly.Once);
        }
    }
}