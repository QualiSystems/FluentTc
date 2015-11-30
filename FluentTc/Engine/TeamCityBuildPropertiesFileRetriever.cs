using System;

namespace FluentTc.Engine
{
    internal interface ITeamCityBuildPropertiesFileRetriever
    {
        string GetTeamCityBuildPropertiesFilePath();
    }

    internal class TeamCityBuildPropertiesFileRetriever : ITeamCityBuildPropertiesFileRetriever
    {
        private const string TeamcityBuildPropertiesFile = "TEAMCITY_BUILD_PROPERTIES_FILE";

        public string GetTeamCityBuildPropertiesFilePath()
        {
            return Environment.GetEnvironmentVariable(TeamcityBuildPropertiesFile);
        }
    }
}