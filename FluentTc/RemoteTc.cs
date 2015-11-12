using System;
using FluentTc.Engine;
using FluentTc.Locators;

namespace FluentTc
{
    public interface IRemoteTc
    {
        IConnectedTc Connect(Action<TeamCityConfigurationBuilder> connect);
    }

    public class RemoteTc : IRemoteTc
    {
        public IConnectedTc Connect(Action<TeamCityConfigurationBuilder> connect)
        {
            return Connect(connect, null);
        }

        internal IConnectedTc Connect(Action<TeamCityConfigurationBuilder> connect, params object[] overrides)
        {
            var teamCityConfigurationBuilder = new TeamCityConfigurationBuilder();
            connect(teamCityConfigurationBuilder);
            var bootstrapper = new Bootstrapper(teamCityConfigurationBuilder.GetITeamCityConnectionDetails(), overrides);
            return bootstrapper.GetConnectedTc();
        }
    }
}