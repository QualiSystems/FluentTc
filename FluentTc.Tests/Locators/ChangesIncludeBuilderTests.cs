using FluentAssertions;
using FluentTc.Locators;
using NUnit.Framework;

namespace FluentTc.Tests.Locators
{
    [TestFixture]
    public class ChangesIncludeBuilderTests
    {
        [Test]
        public void IncludeDefaults()
        {
            // Arrange + Act
            var changesIncludeBuilder = new ChangesIncludeBuilder();
            changesIncludeBuilder.IncludeDefaults();

            var columns = changesIncludeBuilder.GetColumns();

            // Assert
            columns.Should().Be("id,version,href,username,date,webUrl");
        }

        [Test]
        public void IncludeFilesDefaults()
        {
            // Arrange + Act
            var changesIncludeBuilder = new ChangesIncludeBuilder();
            changesIncludeBuilder.IncludeFiles();

            var columns = changesIncludeBuilder.GetColumns();

            // Assert
            columns.Should().Be("id,version,href,username,date,webUrl,files");
        }

        [Test]
        public void IncludeComment()
        {
            // Arrange + Act
            var changesIncludeBuilder = new ChangesIncludeBuilder();
            changesIncludeBuilder.IncludeComment();

            var columns = changesIncludeBuilder.GetColumns();

            // Assert
            columns.Should().Be("id,version,href,username,date,webUrl,comment");
        }

        [Test]
        public void IncludeVcsRootInstance()
        {
            // Arrange + Act
            var changesIncludeBuilder = new ChangesIncludeBuilder();
            changesIncludeBuilder.IncludeVcsRootInstance();

            var columns = changesIncludeBuilder.GetColumns();

            // Assert
            columns.Should().Be("id,version,href,username,date,webUrl,vcsRootInstance");
        }

        [Test]
        public void IncludeAll()
        {
            // Arrange + Act
            var changesIncludeBuilder = new ChangesIncludeBuilder();
            changesIncludeBuilder.IncludeComment().IncludeFiles().IncludeVcsRootInstance();

            var columns = changesIncludeBuilder.GetColumns();

            // Assert
            columns.Should().Be("id,version,href,username,date,webUrl,comment,files,vcsRootInstance");
        }
    }
}

