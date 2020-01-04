using System;
using System.Linq;
using FakeItEasy;
using FluentAssertions;
using FluentTc.Engine;
using FluentTc.Locators;
using JetBrains.TeamCity.ServiceMessages;
using JetBrains.TeamCity.ServiceMessages.Write.Special;
using NUnit.Framework;

namespace FluentTc.Tests
{
    [TestFixture]
    public class LocalTcTests
    {
        [Test]
        public void Test_LocalTc_CreatedWithNoErrors()
        {
            Action action = () => new LocalTc();

            action.ShouldNotThrow<Exception>();
        }

        [Test]
        public void ChangeBuildStatus_Success_MessageWritten()
        {
            // Arrange
            var teamCityWriter = A.Fake<ITeamCityWriter>();

            var teamCityWriterFactory = A.Fake<ITeamCityWriterFactory>();
            A.CallTo(() => teamCityWriterFactory.CreateTeamCityWriter()).Returns(teamCityWriter);

            var localTc = new LocalTc(A.Fake<IBuildParameters>(), teamCityWriterFactory);

            // Act
            localTc.ChangeBuildStatus(BuildStatus.Success);

            // Assert
            A.CallTo(() => teamCityWriter.WriteRawMessage(A<IServiceMessage>.That.Matches(
                m => m.Name == "buildStatus" && 
                m.Keys.Single() == "status" &&
                m.GetValue(m.Keys.Single()) == "SUCCESS") ))
                .MustHaveHappened();
        }

        [Test]
        public void ChangeBuildStatus_Failure_MessageWritten()
        {
            // Arrange
            var teamCityWriter = A.Fake<ITeamCityWriter>();

            var teamCityWriterFactory = A.Fake<ITeamCityWriterFactory>();
            A.CallTo(() => teamCityWriterFactory.CreateTeamCityWriter()).Returns(teamCityWriter);

            var localTc = new LocalTc(A.Fake<IBuildParameters>(), teamCityWriterFactory);

            // Act
            localTc.ChangeBuildStatus(BuildStatus.Failure);

            // Assert
            A.CallTo(() => teamCityWriter.WriteRawMessage(A<IServiceMessage>.That.Matches(
                m => m.Name == "buildStatus" && 
                m.Keys.Single() == "status" &&
                m.GetValue(m.Keys.Single()) == "FAILURE") ))
                .MustHaveHappened();
        }

        [Test]
        public void SetBuildParameter_ParameterNameValue_WriteBuildParameterCalled()
        {
            // Arrange
            var buildParameters = A.Fake<IBuildParameters>();

            var localTc = new LocalTc(buildParameters, A.Fake<ITeamCityWriterFactory>());

            // Act
            localTc.SetBuildParameter("parameter.name", "value1");

            // Assert
            A.CallTo(() => buildParameters.SetBuildParameter("parameter.name", "value1")).MustHaveHappened();
        }

        [Test]
        public void IsTeamCityMode_False()
        {
            // Arrange
            var buildParameters = A.Fake<IBuildParameters>();
            A.CallTo(() => buildParameters.IsTeamCityMode).Returns(false);

            var localTc = new LocalTc(buildParameters, A.Fake<ITeamCityWriterFactory>());

            // Act
            var isTeamCityMode = localTc.IsTeamCityMode;

            // Assert
            isTeamCityMode.Should().BeFalse();
        }

        [Test]
        public void IsTeamCityMode_True()
        {
            // Arrange
            var buildParameters = A.Fake<IBuildParameters>();
            A.CallTo(() => buildParameters.IsTeamCityMode).Returns(true);

            var localTc = new LocalTc(buildParameters, A.Fake<ITeamCityWriterFactory>());

            // Act
            var isTeamCityMode = localTc.IsTeamCityMode;

            // Assert
            isTeamCityMode.Should().BeTrue();
        }

        [Test]
        public void TryGetBuildParameter_ParameterExists_True()
        {
            // Arrange
            var buildParameters = A.Fake<IBuildParameters>();
            string paramValue;
            A.CallTo(() => buildParameters.TryGetBuildParameter("param1", out paramValue)).Returns(true)
                .AssignsOutAndRefParameters("VALUE");

            var localTc = new LocalTc(buildParameters, A.Fake<ITeamCityWriterFactory>());

            // Act
            string actualValue;
            var paramExists = localTc.TryGetBuildParameter("param1", out actualValue);

            // Assert
            actualValue.Should().Be("VALUE");
            paramExists.Should().BeTrue();
        }

        [Test]
        public void TryGetBuildParameter_ParameterDoesNotExist_False()
        {
            // Arrange
            var buildParameters = A.Fake<IBuildParameters>();
            string paramValue;
            A.CallTo(() => buildParameters.TryGetBuildParameter("param1", out paramValue)).Returns(false);

            var localTc = new LocalTc(buildParameters, A.Fake<ITeamCityWriterFactory>());

            // Act
            string actualValue;
            var paramExists = localTc.TryGetBuildParameter("param1", out actualValue);

            // Assert
            paramExists.Should().BeFalse();
        }

        [Test]
        public void IsPersonal_True_True()
        {
            // Arrange
            var buildParameters = A.Fake<IBuildParameters>();
            A.CallTo(() => buildParameters.IsPersonal)
                .Returns(true);

            var localTc = new LocalTc(buildParameters, A.Fake<ITeamCityWriterFactory>());

            // Act
            bool isPersonal = localTc.IsPersonal;

            // Assert
            isPersonal.Should().BeTrue();
        }

        [Test]
        public void IsPersonal_False_False()
        {
            // Arrange
            var buildParameters = A.Fake<IBuildParameters>();
            A.CallTo(() => buildParameters.IsPersonal)
                .Returns(false);

            var localTc = new LocalTc(buildParameters, A.Fake<ITeamCityWriterFactory>());

            // Act
            bool isPersonal = localTc.IsPersonal;

            // Assert
            isPersonal.Should().BeFalse();
        }

        [Test]
        public void PublishArtifact_FileName_Published()
        {
            // Arrange
            var teamCityWriterFactory = A.Fake<ITeamCityWriterFactory>();
            var teamCityWriter = A.Fake<ITeamCityWriter>();
            A.CallTo(() => teamCityWriterFactory.CreateTeamCityWriter()).Returns(teamCityWriter);

            var localTc = new LocalTc(A.Fake<IBuildParameters>(), teamCityWriterFactory);

            // Act
            localTc.PublishArtifact("file.txt");

            // Assert
            A.CallTo(()=>teamCityWriter.PublishArtifact("file.txt")).MustHaveHappened();
        }

        [Test]
        public void PublishArtifact_FileNameAndTarget_Published()
        {
            // Arrange
            var teamCityWriterFactory = A.Fake<ITeamCityWriterFactory>();
            var teamCityWriter = A.Fake<ITeamCityWriter>();
            A.CallTo(() => teamCityWriterFactory.CreateTeamCityWriter()).Returns(teamCityWriter);

            var localTc = new LocalTc(A.Fake<IBuildParameters>(), teamCityWriterFactory);

            // Act
            localTc.PublishArtifact("file.txt", "dir");

            // Assert
            A.CallTo(()=>teamCityWriter.PublishArtifact("file.txt => dir")).MustHaveHappened();
        }
    }
}