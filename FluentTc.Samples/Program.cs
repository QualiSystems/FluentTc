using System;
using System.Linq;
using FluentTc.Locators;

namespace FluentTc.Samples
{
    internal static class Program
    {
        private const string TeamCityHost = "";
        private static readonly string Username = "";
        private static readonly string Password = "";

        private static void Main(string[] args)
        {
            #region RemoteTc

            PrintEnabledAuthorizedDisconnectedAgents();
            PrintAllUsers();
            PrintAllUserEmails();
            PrintUserDetails();
            GetBuildsTriggeredByUser();

            #endregion

            #region LocalTc

var localTc = new LocalTc();

var buildId = localTc.GetBuildParameter("build.id");
localTc.SetBuildParameter("parameter.name", "value1");
localTc.ChangeBuildStatus(BuildStatus.Success);

            #endregion

            Console.ReadKey();
        }

        private static void GetBuildsTriggeredByUser()
        {
            new RemoteTc()
                .Connect(_ => _.ToHost(TeamCityHost).AsUser(Username, Password))
                .GetBuilds(_ => _.TriggeredBy(u => u.Username(Username)))
                .ForEach(b => Console.WriteLine("BuildTypeId: {0}", b.BuildTypeId));
        }

        private static void PrintUserDetails()
        {
            var user = new RemoteTc()
                .Connect(_ => _.ToHost(TeamCityHost).AsUser(Username, Password))
                .GetUser(_ => _.Username(Username));

            Console.WriteLine("Name: {0}", user.Name);
            Console.WriteLine("Email: {0}", user.Email);
            Console.WriteLine("Last Login: {0}", user.LastLogin);
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

        private static void PrintAllUserEmails()
        {
            var connectedTc = new RemoteTc()
                .Connect(_ => _.ToHost(TeamCityHost).AsUser(Username, Password));

            connectedTc
                .GetAllUsers()
                .Select(u => connectedTc.GetUser(_ => _.Id(u.Id)))
                .ToList()
                .ForEach(u => Console.WriteLine(u.Email));
        }
    }
}