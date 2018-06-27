using System;
using System.Collections.Generic;
using FluentAssertions;
using FluentTc.Domain;
using FluentTc.Locators;
using NUnit.Framework;

namespace FluentTc.Tests.Domain
{
    [TestFixture]
    public class BuildTests
    {
        [Test]
        public void SetBuildConfiguration()
        {
            // Arrange
            var build = new Build(1, "2", BuildStatus.Success, new DateTime(), new DateTime(), new DateTime(), null,
                null,
                new List<Change>(), "",
                null, new TestOccurrences { Count = 0 }, BuildState.Finished, new RevisionsWrapper());

            // Act
            build.SetBuildConfiguration(new BuildConfiguration {Id = "ConfigId"});

            // Assert
            build.BuildConfiguration.Id.Should().Be("ConfigId");
        }
    }
}