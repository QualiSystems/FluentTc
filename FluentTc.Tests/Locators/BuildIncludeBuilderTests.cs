using FluentAssertions;
using FluentTc.Locators;
using NUnit.Framework;

namespace FluentTc.Tests.Locators
{
    [TestFixture]
    public class BuildIncludeBuilderTests
    {
        [Test]
        public void GetColumns_IncludeStartDate_Formatted()
        {
            // Arrange
            var buildIncludeBuilder = new BuildIncludeBuilder();
            buildIncludeBuilder.IncludeStartDate();

            // Act
            var columns = buildIncludeBuilder.GetColumns();

            // Assert
            columns.Should().Be("buildTypeId,href,id,number,state,status,webUrl,startDate");
        }

        [Test]
        public void GetColumns_IncludeFinishDate_Formatted()
        {
            // Arrange
            var buildIncludeBuilder = new BuildIncludeBuilder();
            buildIncludeBuilder.IncludeFinishDate();

            // Act
            var columns = buildIncludeBuilder.GetColumns();

            // Assert
            columns.Should().Be("buildTypeId,href,id,number,state,status,webUrl,finishDate");
        }

        [Test]
        public void GetColumns_IncludeStatusText_Formatted()
        {
            // Arrange
            var buildIncludeBuilder = new BuildIncludeBuilder();
            buildIncludeBuilder.IncludeStatusText();

            // Act
            var columns = buildIncludeBuilder.GetColumns();

            // Assert
            columns.Should().Be("buildTypeId,href,id,number,state,status,webUrl,statusText");
        }

        [Test]
        public void GetColumns_IncludeDefaults_Formatted()
        {
            // Arrange
            var buildIncludeBuilder = new BuildIncludeBuilder();
            buildIncludeBuilder.IncludeDefaults();

            // Act
            var columns = buildIncludeBuilder.GetColumns();

            // Assert
            columns.Should().Be("buildTypeId,href,id,number,state,status,webUrl");
        }

        [Test]
        public void GetColumns_IncludeStartDatefinishDateStatusText_Formatted()
        {
            // Arrange
            var buildIncludeBuilder = new BuildIncludeBuilder();
            buildIncludeBuilder.IncludeStartDate().IncludeFinishDate().IncludeStatusText();

            // Act
            var columns = buildIncludeBuilder.GetColumns();

            // Assert
            columns.Should().Be("buildTypeId,href,id,number,state,status,webUrl,startDate,finishDate,statusText");
        }
    }
}