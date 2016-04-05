using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using FluentTc.Domain;
using FluentTc.Engine;
using FluentTc.Locators;
using NUnit.Framework;

namespace FluentTc.Tests.Engine
{
    [TestFixture]
    public class BuildModelToBuildConverterTests
    {
        [Test]
        public void ConvertToBuilds_BuildStatusSuccess()
        {
            var buildModelToBuildConverter = new BuildModelToBuildConverter();
            var buildWrapper = new BuildWrapper
            {
                Build = new List<BuildModel> {new BuildModel
                {
                    Status = "SUCCESS",
                    WebUrl = @"http://teamcity/buildid"
                }},
                Count = "1"
            };
            var builds = buildModelToBuildConverter.ConvertToBuilds(buildWrapper);

            // Assert
            builds.Single().Status.Should().Be(BuildStatus.Success);
            builds.Single().WebUrl.Should().Be(@"http://teamcity/buildid");
        }

        [Test]
        public void ConvertToBuilds_BuildTypeNull_BuildConfigurationHasId()
        {
            var buildModelToBuildConverter = new BuildModelToBuildConverter();
            var buildWrapper = new BuildWrapper
            {
                Build = new List<BuildModel> {new BuildModel
                {
                    Status = "SUCCESS",
                    BuildType = null,
                    BuildTypeId = "bt2"
                }},
                Count = "1"
            };
            var builds = buildModelToBuildConverter.ConvertToBuilds(buildWrapper);

            // Assert
            builds.Single().Status.Should().Be(BuildStatus.Success);
            builds.Single().BuildConfiguration.Id.Should().Be("bt2");
        }

        [Test]
        public void ConvertToBuilds_BuildTypeNotNull_BuildConfigurationInitialized()
        {
            var buildModelToBuildConverter = new BuildModelToBuildConverter();
            var buildWrapper = new BuildWrapper
            {
                Build = new List<BuildModel> {new BuildModel
                {
                    Status = "SUCCESS",
                    BuildType = new BuildConfiguration { Id = "bt2"},
                    BuildTypeId = "WRONG"
                }},
                Count = "1"
            };
            var builds = buildModelToBuildConverter.ConvertToBuilds(buildWrapper);

            // Assert
            builds.Single().Status.Should().Be(BuildStatus.Success);
            builds.Single().BuildConfiguration.Id.Should().Be("bt2");
        }
    }
}