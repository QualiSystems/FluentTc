using System.IO.Abstractions;
using System.Linq;
using Autofac;

namespace FluentTc.Engine
{
    internal class Bootstrapper
    {
        private readonly ITeamCityConnectionDetails m_TeamCityConnectionDetails;
        private readonly object[] m_Overrides;

        public Bootstrapper(ITeamCityConnectionDetails teamCityConnectionDetails, params object[] overrides)
        {
            m_TeamCityConnectionDetails = teamCityConnectionDetails;
            m_Overrides = overrides;
        }

        public IConnectedTc GetConnectedTc()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<FileSystem>().As<IFileSystem>();
            builder.RegisterAssemblyTypes(typeof(Bootstrapper).Assembly).AsImplementedInterfaces();
            builder.RegisterInstance(m_TeamCityConnectionDetails).AsImplementedInterfaces();
            OverrideRegistrations(builder, m_Overrides);
            var container = builder.Build();

            return container.Resolve<IConnectedTc>();
        }

        private static void OverrideRegistrations(ContainerBuilder builder, params object[] overrides)
        {
            if (overrides == null || !overrides.Any()) return;
            foreach (var instance in overrides)
            {
                builder.RegisterInstance(instance).AsImplementedInterfaces();
            }
        }
    }
}