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

        [Test]
        public void ConvertToBuilds_StatusIsNull_StatusIsNull()
        {
            var buildModelToBuildConverter = new BuildModelToBuildConverter();
            var buildWrapper = new BuildWrapper
            {
                Build = new List<BuildModel> {new BuildModel
                {
                    Status = null,
                    BuildType = new BuildConfiguration { Id = "bt2"},
                    BuildTypeId = "WRONG"
                }},
                Count = "1"
            };
            var builds = buildModelToBuildConverter.ConvertToBuilds(buildWrapper);

            // Assert
            builds.Single().Status.HasValue.Should().BeFalse();
            builds.Single().BuildConfiguration.Id.Should().Be("bt2");
        }

        [Test]
        public void ConvertToBuild()
        {
            var buildModelToBuildConverter = new BuildModelToBuildConverter();
            var buildModel = new BuildModel
            {
                Status = "FAILURE",
                BuildType = new BuildConfiguration {Id = "bt2"},
                BuildTypeId = "bt2"
            };
            var build = buildModelToBuildConverter.ConvertToBuild(buildModel);

            // Assert
            build.Status.Should().Be(BuildStatus.Failure);
            build.BuildConfiguration.Id.Should().Be("bt2");
        }

        [Test]
        public void ConvertToBuilds_BuildProperties()
        {
            var buildModelToBuildConverter = new BuildModelToBuildConverter();
            var buildWrapper = new BuildWrapper
            {
                Build = new List<BuildModel> {new BuildModel
                {
                    Status = "SUCCESS",
                    Properties = new Properties
                    {
                        Property = new List<Property> { new Property { Name = "Property1", Value = "Value1"} }
                    }
                }},
                Count = "1"
            };
            var builds = buildModelToBuildConverter.ConvertToBuilds(buildWrapper);

            // Assert
            builds.Single().Properties.Property.Single().Name.Should().Be("Property1");
            builds.Single().Properties.Property.Single().Value.Should().Be("Value1");
        }
    }
}