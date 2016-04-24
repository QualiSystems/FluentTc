using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using FluentTc.Domain;
using FluentTc.Engine;
using NUnit.Framework;

namespace FluentTc.Tests.Engine
{
    [TestFixture]
    public class BuildStatisticConverterTests
    {
        [Test]
        public void Convert_EmptyModel_EmptyList()
        {
            // Arrange
            var buildStatisticConverter = new BuildStatisticConverter();

            // Act
            var buildStatistics = buildStatisticConverter.Convert(new BuildStatisticsModel());

            // Assert
            buildStatistics.Should().BeEmpty();
        }

        [Test]
        public void Convert_ModelWithOneProperty_OneStatistic()
        {
            // Arrange
            var buildStatisticConverter = new BuildStatisticConverter();

            // Act
            var buildStatistics = buildStatisticConverter.Convert(new BuildStatisticsModel
            {
                Count = "1",
                Property = new List<Property> {new Property {Name = "PropName", Value = "PropVal"}}
            });

            // Assert
            buildStatistics.Count.Should().Be(1);
            buildStatistics.Single().Name.Should().Be("PropName");
            buildStatistics.Single().Value.Should().Be("PropVal");
        }
    }
}