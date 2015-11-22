using System;
using System.IO.Abstractions.TestingHelpers;
using FluentAssertions;
using FluentTc.Engine;
using FluentTc.Tests.TestingTools;
using NUnit.Framework;

namespace FluentTc.Tests.Engine
{
    [TestFixture]
    public class BuildParametersTests
    {
        [Test]
        public void GetParameter_File_Parsed()
        {
            // Arrange
            var mockFileSystem = new MockFileSystem();
            var propertiesFile = @"C:\BuildAgent\temp\buildTmp\teamcity.build322130465402584030.properties";
            mockFileSystem.AddFile(propertiesFile, @"#TeamCity build properties without 'system.' prefix
#Sun Nov 01 14:40:00 IST 2015
agent.home.dir=C\:\\BuildAgent
");

            var teamCityContext = new BuildParameters(propertiesFile, mockFileSystem);

            // Act + Assert
            teamCityContext.GetParameterValue("agent.home.dir").Should().Be(@"C:\BuildAgent");
            teamCityContext.AgentHomeDir.Should().Be(@"C:\BuildAgent");
        }

        [Test]
        public void Constructor_EnvironmentVariableIsNull_NoExceptionThrown()
        {
            // Arrange
            var fileSystem = new MockFileSystem();

            // Act
            Action action = () => new BuildParameters(null, fileSystem);

            // Assert
            action.ShouldNotThrow<Exception>();
        }

        [Test]
        public void GetParameter_RealFile_Parsed()
        {
            string resource = new EmbeddedResourceReader().GetResource("PropertiesFile.txt");

            // Arrange
            var mockFileSystem = new MockFileSystem();
            var propertiesFile = @"C:\BuildAgent\temp\buildTmp\teamcity.build322130465402584030.properties";
            mockFileSystem.AddFile(propertiesFile, resource);

            var teamCityContext = new BuildParameters(propertiesFile, mockFileSystem);

            // Act 
            var teamCityVersion = teamCityContext.TeamCityVersion;

            // Assert
            teamCityVersion.Should().Be(@"9.1.2 (build 37168)");
        }
    }
}