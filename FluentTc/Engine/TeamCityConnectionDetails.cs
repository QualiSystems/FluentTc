namespace FluentTc.Engine
{
    internal interface ITeamCityConnectionDetails
    {
        string TeamCityHost { get; }
        string Username { get; }
        string Password { get; }
        bool ActAsGuest { get; }
        bool UseSSL { get; }
    }

    internal class TeamCityConnectionDetails : ITeamCityConnectionDetails
    {
        private readonly string m_Password;
        private readonly string m_TeamCityHost;
        private readonly string m_Username;
        private readonly bool m_ActAsGuest;
        private readonly bool m_UseSsl;

        public TeamCityConnectionDetails(string teamCityHost, string username, string password, bool actAsGuest, bool useSsl)
        {
            m_TeamCityHost = teamCityHost;
            m_Username = username;
            m_Password = password;
            m_ActAsGuest = actAsGuest;
            m_UseSsl = useSsl;
        }

        public string TeamCityHost
        {
            get { return m_TeamCityHost; }
        }

        public string Username
        {
            get { return m_Username; }
        }

        public string Password
        {
            get { return m_Password; }
        }

        public bool ActAsGuest
        {
            get { return m_ActAsGuest; }
        }

        public bool UseSSL
        {
            get { return m_UseSsl; }
        }
    }
}