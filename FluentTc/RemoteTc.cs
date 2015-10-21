using System;
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
            var teamCityConfigurationBuilder = new TeamCityConfigurationBuilder();
            connect(teamCityConfigurationBuilder);
            var bootstrapper = new Bootstrapper(teamCityConfigurationBuilder.GetITeamCityConnectionDetails());
            return bootstrapper.GetConnectedTc();
        }
    }
}