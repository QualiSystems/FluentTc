using System;
using System.IO.Abstractions.TestingHelpers;
using System.Linq;
using FluentAssertions;
using FluentTc.Domain;
using FluentTc.Engine;
using NUnit.Framework;

namespace FluentTc.Tests.Engine
{
    [TestFixture]
    public class ChangedFilesParserTests
    {
        [Test]
        public void ParseChangedFiles_FileWithOneChange_Parsed()
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            fileSystem.AddFile("changes.txt", new MockFileData(@"VCE Drivers/Cisco NxOS/Debug.tsdrv:CHANGED:136346"));
            var changedFilesParser = new ChangedFilesParser(fileSystem);
            // Act
            var changedFiles = changedFilesParser.ParseChangedFiles("changes.txt");

            // Assert
            var changedFile = changedFiles.Single();
            changedFile.RelativeFilePath.Should().Be(@"VCE Drivers\Cisco NxOS\Debug.tsdrv");
            changedFile.ChangeStatus.Should().Be(FileChangeStatus.Changed);
            changedFile.ChangesetNumber.Should().Be("136346");
        }

        [Test]
        public void ParseChangedFiles_FileWithUnknownStatus_ExceptionThrown()
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            fileSystem.AddFile("changes.txt", new MockFileData(@"VCE Drivers/Cisco NxOS/Debug.tsdrv:UNKNOWN:136346"));
            var changedFilesParser = new ChangedFilesParser(fileSystem);
            // Act
            Action action = () => changedFilesParser.ParseChangedFiles("changes.txt");

            // Assert
            action.ShouldThrow<ArgumentException>().WithMessage("Could not parse FileStatusChange: UNKNOWN");
        }

        [Test]
        public void ParseChangedFiles_FileWithOneFileRemoved_Parsed()
        {
            // Arrange
            var fileSystem = new MockFileSystem();
            fileSystem.AddFile("changes.txt", new MockFileData(@"VCE Drivers/Cisco NxOS/Debug.tsdrv:REMOVED:136346"));
            var changedFilesParser = new ChangedFilesParser(fileSystem);
            // Act
            var changedFiles = changedFilesParser.ParseChangedFiles("changes.txt");

            // Assert
            var changedFile = changedFiles.Single();
            changedFile.RelativeFilePath.Should().Be(@"VCE Drivers\Cisco NxOS\Debug.tsdrv");
            changedFile.ChangeStatus.Should().Be(FileChangeStatus.Removed);
            changedFile.ChangesetNumber.Should().Be("136346");
        }
    }
}