using System;
using System.Collections;

namespace FluentTc.Samples
{
    internal class Program
    {
        private static string Username = "";
        private static string Password = "";
        private const string TeamCityHost = "";

        private static void Main(string[] args)
        {
            PrintEnabledAuthorizedDisconnectedAgents();
            PrintAllUsers();
            Console.ReadKey();
        }

        private static void PrintEnabledAuthorizedDisconnectedAgents()
        {
            new RemoteTc()
                .Connect(_ => _.ToHost(TeamCityHost).AsUser(Username, Password))
                .GetAgents(_ => _.Enabled().Authorized().Disconnected())
                .ForEach(a => Console.WriteLine("{0}", a.Name));
        }

        private static void PrintAllUsers()
        {
            new RemoteTc()
                .Connect(_ => _.ToHost(TeamCityHost).AsUser(Username, Password))
                .GetAllUsers()
                .ForEach(a => Console.WriteLine("{0}", a.Name));
        }
    }
}