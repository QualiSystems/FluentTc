namespace FluentTc.Locators
{
    public class TeamCityConfigurationBuilder
    {
        private string m_Password;
        private string m_TeamCityHost;
        private string m_Username;
        private bool m_UseSsl;

        public TeamCityConfigurationBuilder()
        {
            m_TeamCityHost = "localhost";
            m_Username = "guest";
            m_Password = string.Empty;
        }

        public TeamCityConfigurationBuilder ToHost(string teamCityHost)
        {
            m_TeamCityHost = teamCityHost;
            return this;
        }

        public TeamCityConfigurationBuilder AsGuest()
        {
            m_Username = "guest";
            return this;
        }

        public TeamCityConfigurationBuilder AsUser(string username, string password)
        {
            m_Username = username;
            m_Password = password;
            return this;
        }

        public TeamCityConfigurationBuilder UseSsl()
        {
            m_UseSsl = true;
            return this;
        }

        public TeamCityConfigurationBuilder DoNotUseSsl()
        {
            m_UseSsl = false;
            return this;
        }

        internal ITeamCityConnectionDetails GetTeamCityConnectionDetails()
        {
            return new TeamCityConnectionDetails(m_TeamCityHost, m_Username, m_Password, m_Username.Equals("guest"), m_UseSsl);
        }
    }
}