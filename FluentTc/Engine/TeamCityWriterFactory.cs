using JetBrains.TeamCity.ServiceMessages.Write.Special;

namespace FluentTc.Engine
{
    internal interface ITeamCityWriterFactory
    {
        ITeamCityWriter CreateTeamCityWriter();
    }

    internal class TeamCityWriterFactory : ITeamCityWriterFactory
    {
        private readonly ITeamCityServiceMessages m_CityServiceMessages;

        public TeamCityWriterFactory(ITeamCityServiceMessages cityServiceMessages)
        {
            m_CityServiceMessages = cityServiceMessages;
        }

        public ITeamCityWriter CreateTeamCityWriter()
        {
            return m_CityServiceMessages.CreateWriter();
        }
    }
}