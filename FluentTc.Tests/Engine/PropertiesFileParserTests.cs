using System.IO.Abstractions.TestingHelpers;
using FluentAssertions;
using FluentTc.Engine;
using FluentTc.Tests.TestingTools;
using NUnit.Framework;

namespace FluentTc.Tests.Engine
{
    [TestFixture]
    public class PropertiesFileParserTests
    {
        [Test]
        public void ParsePropertiesFile_File_Parsed()
        {
            // Arrange
            var mockFileSystem = new MockFileSystem();
            var propertiesFile = @"C:\BuildAgent\temp\buildTmp\teamcity.build322130465402584030.properties";
            mockFileSystem.AddFile(propertiesFile, @"#TeamCity build properties without 'system.' prefix
#Sun Nov 01 14:40:00 IST 2015
agent.home.dir=C\:\\BuildAgent
");

            var fileParser = new PropertiesFileParser(mockFileSystem);

            // Act
            var dictionary = fileParser.ParsePropertiesFile(propertiesFile);

            // Assert
            dictionary["agent.home.dir"].Should().Be(@"C:\BuildAgent");
        }

        [Test]
        public void ParsePropertiesFile_RealFile_Parsed()
        {
            var resource = new EmbeddedResourceReader().GetResource("PropertiesFile.txt");

            // Arrange
            var mockFileSystem = new MockFileSystem();
            var propertiesFile = @"C:\BuildAgent\temp\buildTmp\teamcity.build322130465402584030.properties";
            mockFileSystem.AddFile(propertiesFile, resource);

            var propertiesFileParser = new PropertiesFileParser(mockFileSystem);

            // Act
            var dictionary = propertiesFileParser.ParsePropertiesFile(propertiesFile);

            // Assert
            dictionary["agent.home.dir"].Should().Be(@"C:\BuildAgent");
            dictionary["agent.name"].Should().Be(@"BUILDS8");
            dictionary["agent.ownPort"].Should().Be(@"9090");
            dictionary["agent.work.dir"].Should().Be(@"C:\BuildAgent\work");
            dictionary["build.number"].Should().Be(@"4");
            dictionary["FxCopRoot"].Should()
                .Be(@"C:\Program Files (x86)\Microsoft Visual Studio 12.0\Team Tools\Static Analysis Tools\FxCop");
            dictionary["teamcity.agent.dotnet.agent_url"].Should().Be(@"http://localhost:9090/RPC2");
            dictionary["teamcity.auth.userId"].Should().Be(@"TeamCityBuildId=781682");
        }
    }
}