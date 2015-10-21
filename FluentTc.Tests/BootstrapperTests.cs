using FakeItEasy;
using FluentAssertions;
using NUnit.Framework;

namespace FluentTc.Tests
{
    [TestFixture]
    public class BootstrapperTests
    {
        [Test]
        public void GetConnectedTc_FakeConnection_NotNull()
        {
            // Arrange
            var bootstrapper = new Bootstrapper(A.Fake<ITeamCityConnectionDetails>());

            // Act
            var connectedTc = bootstrapper.GetConnectedTc();

            // Assert
            connectedTc.Should().NotBeNull();
        }
    }
}