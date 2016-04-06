using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using EasyHttp.Infrastructure;
using FakeItEasy;
using FluentAssertions;
using FluentTc.Domain;
using FluentTc.Engine;
using NUnit.Framework;

namespace FluentTc.Tests.Engine
{
    [TestFixture]
    public class BuildConfigurationTemplateRetrieverTests
    {
        [Test]
        public void GetAllBuildConfigurationTemplates_BuildTypeIsNull_EmptyList()
        {
            // Arrange
            var teamCityCaller = A.Fake<ITeamCityCaller>();
            A.CallTo(() => teamCityCaller.GetFormat<BuildTypeWrapper>("/app/rest/buildTypes?locator=templateFlag:true"))
                .Returns(new BuildTypeWrapper
                {
                    BuildType = null
                });

            var buildConfigurationTemplateRetriever = new BuildConfigurationTemplateRetriever(teamCityCaller);

            // Act
            var templates = buildConfigurationTemplateRetriever.GetAllBuildConfigurationTemplates();

            // Assert
            templates.Should().BeEmpty();
        }

        [Test]
        public void GetAllBuildConfigurationTemplates_BuildTypeWrapperIsNull_EmptyList()
        {
            // Arrange
            var teamCityCaller = A.Fake<ITeamCityCaller>();
            A.CallTo(() => teamCityCaller.GetFormat<BuildTypeWrapper>("/app/rest/buildTypes?locator=templateFlag:true"))
                .Returns(null);

            var buildConfigurationTemplateRetriever = new BuildConfigurationTemplateRetriever(teamCityCaller);

            // Act
            var templates = buildConfigurationTemplateRetriever.GetAllBuildConfigurationTemplates();

            // Assert
            templates.Should().BeEmpty();
        }

        [Test]
        public void GetAllBuildConfigurationTemplates_ExceptionThrown_ExceptionRethrown()
        {
            // Arrange
            var teamCityCaller = A.Fake<ITeamCityCaller>();
            A.CallTo(() => teamCityCaller.GetFormat<BuildTypeWrapper>("/app/rest/buildTypes?locator=templateFlag:true"))
                .Throws(new HttpException(HttpStatusCode.BadRequest, "BadRequest"));

            var buildConfigurationTemplateRetriever = new BuildConfigurationTemplateRetriever(teamCityCaller);

            // Act
            Action action = () => buildConfigurationTemplateRetriever.GetAllBuildConfigurationTemplates();

            // Assert
            action.ShouldThrow<HttpException>()
                .Where(_ => _.StatusCode == HttpStatusCode.BadRequest && _.StatusDescription == "BadRequest");
        }

        [Test]
        public void GetAllBuildConfigurationTemplates_NotFound_EmptyList()
        {
            // Arrange
            var teamCityCaller = A.Fake<ITeamCityCaller>();
            A.CallTo(() => teamCityCaller.GetFormat<BuildTypeWrapper>("/app/rest/buildTypes?locator=templateFlag:true"))
                .Throws(new HttpException(HttpStatusCode.NotFound, "NotFound"));

            var buildConfigurationTemplateRetriever = new BuildConfigurationTemplateRetriever(teamCityCaller);

            // Act
            var templates = buildConfigurationTemplateRetriever.GetAllBuildConfigurationTemplates();

            // Assert
            templates.Should().BeEmpty();
        }

        [Test]
        public void GetAllBuildConfigurationTemplates_OneTemplateFound_Returned()
        {
            // Arrange
            var teamCityCaller = A.Fake<ITeamCityCaller>();
            A.CallTo(() => teamCityCaller.GetFormat<BuildTypeWrapper>("/app/rest/buildTypes?locator=templateFlag:true"))
                .Returns(new BuildTypeWrapper
                {
                    BuildType = new List<BuildConfiguration>
                    {
                        new BuildConfiguration
                        {
                            Id = "bt2"
                        }
                    }
                });

            var buildConfigurationTemplateRetriever = new BuildConfigurationTemplateRetriever(teamCityCaller);

            // Act
            var templates = buildConfigurationTemplateRetriever.GetAllBuildConfigurationTemplates();

            // Assert
            templates.Single().Id.Should().Be("bt2");
        }
    }
}