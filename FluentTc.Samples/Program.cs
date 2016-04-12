using System;
using System.Linq;
using FluentTc.Locators;
using MoreLinq;

namespace FluentTc.Samples
{
    internal static class Program
    {
        private const string TeamCityHost = "tc";
        private static readonly string Username = "buser";
        private static readonly string Password = "qaz$9512";

        private static void Main(string[] args)
        {
            #region RemoteTc

            //GetBuildConfigurationParameters();
            GetLastSuccessfulBuildsForEachConfigurationWithChanges("Trunk_Ci_FastCi");

            PrintEnabledAuthorizedDisconnectedAgents();
            PrintAllUsers();
            PrintAllUserEmails();
            PrintUserDetails();
            GetBuildsTriggeredByUser();
            DeleteBuildConfigurationParameter();
            DeleteProjectParameter();

            #endregion

            #region LocalTc

            var localTc = new LocalTc();

            var buildId = localTc.GetBuildParameter<long>("build.id");
            localTc.SetBuildParameter("parameter.name", "value1");
            localTc.ChangeBuildStatus(BuildStatus.Success);

            #endregion

            Console.ReadKey();
        }

        private static void DeleteBuildConfigurationParameter()
        {
            new RemoteTc()
                .Connect(_ => _.ToHost(TeamCityHost).AsUser(Username, Password))
                .DeleteBuildConfigurationParameter(_ => _.Id("buildConfigId"), __ => __.ParameterName("parameter.name"));
        }

        private static void GetBuildConfigurationParameters()
        {
            var buildConfiguration = new RemoteTc()
                .Connect(_ => _.ToHost(TeamCityHost).AsUser(Username, Password))
                .GetBuildConfiguration(_ => _.Id("Trunk_Green_Ci_Compile"));
        }

        private static void DeleteProjectParameter()
        {
            new RemoteTc()
                .Connect(_ => _.ToHost(TeamCityHost).AsUser(Username, Password))
                .DeleteProjectParameter(_ => _.Id("projectId"), __ => __.ParameterName("parameter.name"));
        }

        private static void GetBuildsTriggeredByUser()
        {
            new RemoteTc()
                .Connect(_ => _.ToHost(TeamCityHost).AsUser(Username, Password))
                .GetBuilds(_ => _.TriggeredBy(u => u.Username(Username)))
                .ForEach(b => Console.WriteLine("BuildTypeId: {0}", b.BuildConfiguration.Id));
        }

        private static void GetLastSuccessfulBuildsForEachConfigurationWithChanges(string projectName)
        {
            var builds =
                new RemoteTc()
                    .Connect(_ => _.ToHost(TeamCityHost).AsUser(Username, Password))
                    .GetLastBuild(_ => _.BuildConfiguration(__ => __.Id("Trunk_Green_Ci_Compile")).Status(BuildStatus.Success),
                        __ => __.IncludeChanges(c => c.IncludeComment().IncludeFiles().IncludeVcsRootInstance()));

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

        public static void Sample_Usage()
        {
            // Agents
            new RemoteTc().Connect(a => a.ToHost("tc").AsGuest())
                .DisableAgent(_ => _.Ip("127.0.0.1"));

            new RemoteTc().Connect(a => a.ToHost("tc").AsGuest())
                .EnableAgent(_ => _.Name("agent1"));

            // Get project by Id
            var project = new RemoteTc().Connect(a => a.ToHost("tc").AsGuest())
                .GetProjectById("FluentTc");

            // Create sub project with Id and Name
            project = new RemoteTc().Connect(a => a.ToHost("tc").AsGuest())
                .CreateProject(_ =>
                    _.Name("New Project Name")
                    .Id("newProjectId")
                    .ParentProject(a => a.Name("FluentTc")));

            // Create project by Name in Root
            project = new RemoteTc().Connect(a => a.ToHost("tc").AsGuest())
                .CreateProject(_ => _.Name("New Project Name"));

            // Agents
            var agents = new RemoteTc().Connect(a => a.ToHost("tc").AsGuest())
                .GetAgents(h => h.Connected());

            var enabledAuthorizedButDisconnectedAgents = new RemoteTc().Connect(a => a.ToHost("tc").AsGuest())
                .GetAgents(h => h.Disconnected().Enabled().Authorized());

            // Build queue  
            var buildQueue = new RemoteTc().Connect(_ => _.ToHost("tc"))
                .GetBuildsQueue(_ => _.Project(__ => __.Id("Branch6_4_Red_NightlyCi_RedWebTests")));

            var buildQueue2 = new RemoteTc().Connect(_ => _.ToHost("tc"))
                .GetBuildsQueue(
                    __ =>
                        __.Project(___ => ___.Id("Branch6_4_Red_NightlyCi_RedWebTests"))
                            .BuildConfiguration(b => b.Name("Trunk")));

            // Remove builds from queue by project Id recursively 
            var connectedTc = new RemoteTc().Connect(_ => _.ToHost("tc"));
            connectedTc.GetBuildConfigurationsRecursively("ProjectId")
                .ForEach(c => connectedTc.RemoveBuildFromQueue(__ => __.BuildConfiguration(___ => ___.Id(c.Id))));

            // Builds
            var builds = new RemoteTc().Connect(a => a.ToHost("tc").AsGuest())
                .GetBuilds(
                    h =>
                        h.BuildConfiguration(r => r.Id("bt2"))
                            .NotPersonal()
                            .Project(r => r.Name("Trunk"))
                            .AgentName("BUILDS11")
                            .Branch(b => b.Name("aa")));

            builds = new RemoteTc().Connect(_ => _.ToHost("tc"))
                .GetBuilds(_ => _.Personal(),
                    _ => _.IncludeStartDate().IncludeFinishDate().IncludeStatusText(), _ => _.DefaultCount());

            builds = new RemoteTc().Connect(_ => _.ToHost("tc"))
                .GetBuilds(_ => _.Personal(), _ => _.IncludeDefaults(), _ => _.Count(5));

            builds = new RemoteTc().Connect(_ => _.ToHost("tc"))
                .GetBuilds(_ => _.BuildConfiguration(x => x.Id("bt2")).NotPersonal().NotRunning(),
                    _ => _.IncludeDefaults(), _ => _.Count(5));

            var build = new RemoteTc().Connect(_ => _.ToHost("tc"))
                .GetBuild(_ => _.Id(123456), _ => _.IncludeDefaults());

            build = new RemoteTc().Connect(_ => _.ToHost("tc"))
                .GetBuild(_ => _.Id(123456));


            // Build configurations
            var buildConfiguration = new RemoteTc().Connect(_ => _.ToHost("tc"))
                .GetBuildConfiguration(_ => _.Id("bt2"));

            // Retrieves all the build configuration under a project
            var buildConfigurations = new RemoteTc().Connect(_ => _.ToHost("tc").AsGuest())
                .GetBuildConfigurations(_ => _.Project(__ => __.Id("Trunk")));

            // Retrieves all the build configuration under a project recursively
            buildConfigurations = new RemoteTc().Connect(_ => _.ToHost("tc").AsGuest())
                .GetBuildConfigurations(_ => _.ProjectRecursively(__ => __.Id("Trunk")));


            new RemoteTc().Connect(_ => _.ToHost("tc"))
                .SetBuildConfigurationParameters(_ => _.Id("bt2"),
                    _ => _.Parameter("name", "value").Parameter("name2", "value"));

            new RemoteTc().Connect(_ => _.ToHost("tc"))
                .RunBuildConfiguration(_ => _.Id("bt2"));

            new RemoteTc().Connect(_ => _.ToHost("tc"))
                .RunBuildConfiguration(_ => _.Id("bt2"),
                    _ => _.Parameter("name", "value").Parameter("name2", "value"));

            new RemoteTc().Connect(_ => _.ToHost("tc"))
                .RunBuildConfiguration(having => having.Id("bt2"), onAgent => onAgent.Name("agent1"));

            new RemoteTc().Connect(_ => _.ToHost("tc"))
                .RunBuildConfiguration(_ => _.Id("bt2"), _ => _.Name("agent1"),
                    _ => _.Parameter("name", "value").Parameter("name2", "value"));

            buildConfiguration = new RemoteTc().Connect(_ => _.ToHost("tc"))
                .CreateBuildConfiguration(_ => _.Id("Trunk"), "config name");

            // Retrieves all the projects
            var allProjects = new RemoteTc().Connect(_ => _.ToHost("tc").AsGuest())
                .GetAllProjects();

            var downloadedFiles = new RemoteTc().Connect(a => a.ToHost("tc").AsGuest())
                .DownloadArtifacts(123, @"C:\DownloadedArtifacts");

            string downloadedFile = new RemoteTc().Connect(a => a.ToHost("tc").AsGuest())
                .DownloadArtifact(759688, @"C:\DownloadedArtifacts", "Logs.zip");

            var testInvestigation = new RemoteTc().Connect(_ => _.ToHost("tc"))
                    .GetTestinvestigationByTestNameId("-1884830467297296372");

            var investigation = new RemoteTc().Connect(_ => _.ToHost("tc"))
                .GetInvestigation(_ => _.Id("fluentTc"));
        }

    }
}