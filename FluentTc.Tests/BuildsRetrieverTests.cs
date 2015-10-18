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
                        "/app/rest/builds?locator={0},count:{1},&fields=count,build({2})",
                        A<object[]>._))
                .Returns(new BuildWrapper() {Count = "0"});

            var buildHavingBuilderFactory = A.Fake<IBuildHavingBuilderFactory>();
            A.CallTo(() => buildHavingBuilderFactory.CreateBuildHavingBuilder()).Returns(new BuildHavingBuilder());

            var buildsRetriever = new BuildsRetriever(teamCityCaller, buildHavingBuilderFactory);

            // Act
            var builds = buildsRetriever.GetBuilds(_ => _.Id(123), _ => _.All(), _ => _.IncludeDefaults());

            // Assert
            A.CallTo(
                () =>
                    teamCityCaller.GetFormat<BuildWrapper>(
                        "/app/rest/builds?locator={0},count:{1},&fields=count,build({2})",
                        A<object[]>.That.IsSameSequenceAs(new object[] { "id:123", -1, "buildTypeId,href,id,number,state,status,webUrl" })))
                .MustHaveHappened(Repeated.Exactly.Once);
        }
    }
}