using System.Net;
using EasyHttp.Infrastructure;
using FakeItEasy;
using FluentAssertions;
using FluentTc.Domain;
using FluentTc.Engine;
using FluentTc.Tests.Locators;
using NUnit.Framework;
using Ploeh.AutoFixture;

namespace FluentTc.Tests.Engine
{
    [TestFixture]
    public class BuildConfigurationRetrieverTests
    {
        [Test]
        public void RetrieveBuildConfigurations_NotFoundExceptionThrown_Empty()
        {
            // Arrange
            var fixture = Auto.Fixture();
            var teamCityCaller = fixture.Freeze<ITeamCityCaller>();
            A.CallTo(() => teamCityCaller.GetFormat<BuildTypeWrapper>("/app/rest/buildTypes/{0}", A<object[]>._))
                .Throws(new HttpException(HttpStatusCode.NotFound, ""));

            var buildConfigurationRetriever = fixture.Create<BuildConfigurationRetriever>();

            // Act
            var retrieveBuildConfigurations =
                buildConfigurationRetriever.RetrieveBuildConfigurations(h => h.Project(i => i.Id("projId")));

            // Assert
            retrieveBuildConfigurations.Should().BeEmpty();
        }
    }
}