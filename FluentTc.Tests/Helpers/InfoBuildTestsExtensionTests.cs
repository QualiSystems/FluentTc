using FakeItEasy;
using FluentAssertions;
using FluentTc.Domain;
using FluentTc.Helpers;
using NUnit.Framework;

namespace FluentTc.Tests.Helpers
{
    [TestFixture]
    public class InfoBuildTestsExtensionTests
    {
        [Test]
        public void Constructor_PropertiesFileIsNull_NoExceptionThrown()
        {
            // Arrange
            var build = A.Fake<IBuild>();
            A.CallTo(() => build.StatusText).Returns("Tests failed: 4 (3 new), passed: 2, muted: 1");

            // Act
            var testsInfo = build.GetTestInfo();

            // Assert
            testsInfo.Failed.Should().Be(4);
            testsInfo.FailedNew.Should().Be(3);
            testsInfo.Passed.Should().Be(2);
            testsInfo.Muted.Should().Be(1);
        }
    }
}
