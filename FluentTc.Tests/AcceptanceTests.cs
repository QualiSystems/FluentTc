using System.Collections.Generic;
using FakeItEasy;
using FluentAssertions;
using FluentTc.Domain;
using NUnit.Framework;

namespace FluentTc.Tests
{
    [TestFixture]
    public class AcceptanceTests
    {
        [Test]
        public void DisableAgent_Id()
        {
            // Arrange
            var teamCityCaller = A.Fake<TeamCityCaller>();

            var connectedTc = new RemoteTc().Connect(_ => _.AsGuest(), teamCityCaller);

            // Act
            connectedTc.DisableAgent(_ => _.Id(123));

            // Assert
            A.CallTo(
                () =>
                    teamCityCaller.PutFormat("False", "text/plain", "/app/rest/agents/{0}/enabled",
                        A<object[]>.That.IsSameSequenceAs(new[] {"id:123"}))).MustHaveHappened();
        }

        [Test]
        public void EnableAgent_Id()
        {
            // Arrange
            var teamCityCaller = A.Fake<TeamCityCaller>();

            var connectedTc = new RemoteTc().Connect(_ => _.AsGuest(), teamCityCaller);

            // Act
            connectedTc.EnableAgent(_ => _.Id(123));

            // Assert
            A.CallTo(
                () =>
                    teamCityCaller.PutFormat("True", "text/plain", "/app/rest/agents/{0}/enabled",
                        A<object[]>.That.IsSameSequenceAs(new[] {"id:123"}))).MustHaveHappened();
        }

        [Test]
        public void GetBuild_Id_Build()
        {
            // Arrange
            var teamCityCaller = A.Fake<TeamCityCaller>();
            A.CallTo(
                () =>
                    teamCityCaller.Get<BuildWrapper>(
                        "/app/rest/builds?locator=id:123,count:-1,&fields=count,build(buildTypeId,href,id,number,state,status,webUrl)"))
                .Returns(new BuildWrapper {Count = "1", Build = new List<Build>(new[] {new Build {Id = 987}})});

            var connectedTc = new RemoteTc().Connect(_ => _.AsGuest(), teamCityCaller);

            // Act
            var build = connectedTc.GetBuild(_ => _.Id(123));

            // Assert
            build.Id.Should().Be(987);
        }
    }
}