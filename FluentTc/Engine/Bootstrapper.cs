using System.IO.Abstractions;
using System.Linq;
using Autofac;
using JetBrains.TeamCity.ServiceMessages.Write.Special;

namespace FluentTc.Engine
{
    internal class Bootstrapper
    {
        private readonly object[] m_Overrides;

        internal Bootstrapper(params object[] overrides)
        {
            m_Overrides = overrides;
        }

        internal IConnectedTc GetConnectedTc()
        {
            return Get<IConnectedTc>();
        }

        internal T Get<T>()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<FileSystem>().As<IFileSystem>();
            builder.RegisterType<TeamCityServiceMessages>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterAssemblyTypes(typeof(Bootstrapper).Assembly).AsImplementedInterfaces();
            OverrideRegistrations(builder, m_Overrides);
            var container = builder.Build();

            return container.Resolve<T>();
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