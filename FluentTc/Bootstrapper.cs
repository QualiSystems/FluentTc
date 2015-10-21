using Autofac;

namespace FluentTc
{
    internal class Bootstrapper
    {
        private readonly ITeamCityConnectionDetails m_TeamCityConnectionDetails;

        public Bootstrapper(ITeamCityConnectionDetails teamCityConnectionDetails)
        {
            m_TeamCityConnectionDetails = teamCityConnectionDetails;
        }

        public IConnectedTc GetConnectedTc()
        {
            var builder = new ContainerBuilder();
            builder.RegisterAssemblyTypes(GetType().Assembly).AsImplementedInterfaces();
            builder.RegisterInstance(m_TeamCityConnectionDetails).AsImplementedInterfaces();
            var container = builder.Build();

            return container.Resolve<IConnectedTc>();
        }
    }
}