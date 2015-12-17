using System;
using System.Diagnostics;
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
        public void Ctor_DefaultCtor_TakesLessThanSecond()
        {
            Time(() => new LocalTc()).Should().BeLessThan(1.Seconds());
        }

        private TimeSpan Time(Action toTime)
        {
            var timer = Stopwatch.StartNew();
            toTime();
            timer.Stop();
            return timer.Elapsed;
        }
    }
}