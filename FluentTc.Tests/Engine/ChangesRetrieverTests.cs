using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using FluentAssertions;
using FluentTc.Domain;
using FluentTc.Engine;
using FluentTc.Locators;
using NUnit.Framework;

namespace FluentTc.Tests.Engine
{
    [TestFixture]
    public class ChangesRetrieverTests
    {
        [Test]
        public void GetChanges_BuildIdIncludeDefaults()
        {
            // Arrange
            var teamCityCaller = A.Fake<TeamCityCaller>();
            A.CallTo(
                () =>
                    teamCityCaller.GetFormat<ChangesList>(@"/app/rest/changes?locator={0}&fields=change({1})",
                        A<object[]>.Ignored)).Returns(new ChangesList(){Change = new List<Change>(){new Change(){Id = 123}}});


            var buildHavingBuilder = A.Fake<BuildHavingBuilder>();
            A.CallTo(() => buildHavingBuilder.GetLocator()).Returns("id:123");

            var buildHavingBuilderFactory = A.Fake<IBuildHavingBuilderFactory>();
            A.CallTo(() => buildHavingBuilderFactory.CreateBuildHavingBuilder())
                .Returns(buildHavingBuilder);

            // Act
            var changesRetriever = new ChangesRetriever(teamCityCaller, buildHavingBuilderFactory);
            var changes = changesRetriever.GetChanges(_ => _.Build(__ => __.Id(123)), __ => __.IncludeDefaults());
            
            // Assert
            changes.Single().Id.Should().Be(123);
        }
    }
}