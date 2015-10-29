using FakeItEasy;
using FluentTc.Engine;
using FluentTc.Locators;
using FluentTc.Tests.Locators;
using NUnit.Framework;
using Ploeh.AutoFixture;

namespace FluentTc.Tests.Engine
{
    [TestFixture]
    public class BuildQueueRemoverTests
    {
        [Test]
        public void RemoveBuildFromQueue_Id_DeleteFormatCalled()
        {
            // Arrange
            var fixture = Auto.Fixture();

            var buildQueueIdHavingBuilder = A.Fake<IBuildQueueIdHavingBuilder>();
            A.CallTo(() => buildQueueIdHavingBuilder.GetLocator()).Returns("id:123");

            var buildQueueIdHavingBuilderFactory = fixture.Freeze<IBuildQueueIdHavingBuilderFactory>();
            A.CallTo(() => buildQueueIdHavingBuilderFactory.CreateBuildQueueIdHavingBuilder())
                .Returns(buildQueueIdHavingBuilder);

            var teamCityCaller = fixture.Freeze<ITeamCityCaller>();

            var buildQueueRemover = fixture.Create<BuildQueueRemover>();

            // Act
            buildQueueRemover.RemoveBuildFromQueue(_ => _.Id(123));

            // Assert
            A.CallTo(() => buildQueueIdHavingBuilder.Id(123)).MustHaveHappened();
            A.CallTo(
                () =>
                    teamCityCaller.DeleteFormat(@"/app/rest/buildQueue/{0}",
                        A<object[]>.That.IsSameSequenceAs(new[] {"id:123"}))).MustHaveHappened();
        }
    }
}