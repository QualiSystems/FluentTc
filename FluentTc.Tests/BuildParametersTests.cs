using System.IO.Abstractions.TestingHelpers;
using FluentAssertions;
using NUnit.Framework;

namespace FluentTc.Tests
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
    }
}