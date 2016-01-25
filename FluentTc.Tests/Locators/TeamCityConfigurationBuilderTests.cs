using FluentAssertions;
using FluentTc.Locators;
using NUnit.Framework;

namespace FluentTc.Tests.Locators
{
    [TestFixture]
    public class TeamCityConfigurationBuilderTests
    {
        [Test]
        public void GetTeamCityConnectionDetails_Guest_ActAsGuestTrue()
        {
            // Arrange
            var teamCityConfigurationBuilder = new TeamCityConfigurationBuilder();
            // Act
            var teamCityConnectionDetails =
                teamCityConfigurationBuilder.ToHost("teamcity").AsGuest().GetTeamCityConnectionDetails();

            // Assert
            teamCityConnectionDetails.ActAsGuest.Should().BeTrue();
            teamCityConnectionDetails.TeamCityHost.Should().Be("teamcity");
        }

        [Test]
        public void GetTeamCityConnectionDetails_AsUser_ActAsGuestFalse()
        {
            // Arrange
            var teamCityConfigurationBuilder = new TeamCityConfigurationBuilder();
            // Act
            var teamCityConnectionDetails =
                teamCityConfigurationBuilder.ToHost("teamcity").AsUser("user", "pwd").GetTeamCityConnectionDetails();

            // Assert
            teamCityConnectionDetails.TeamCityHost.Should().Be("teamcity");
            teamCityConnectionDetails.ActAsGuest.Should().BeFalse();
            teamCityConnectionDetails.Username.Should().Be("user");
            teamCityConnectionDetails.Password.Should().Be("pwd");
        }
    }
}