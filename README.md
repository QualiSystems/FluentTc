[![Stories in Ready](https://badge.waffle.io/borismod/FluentTc.png?label=ready&title=Ready)](https://waffle.io/borismod/FluentTc)
# FluentTc
Easy-to-use, readable and comprehensive library for consuming TeamCity REST API. Written using real scenarios in mind, enables variuos range of queries and operation on TeamCity

# How to get

install-package FluentTc

# How to use
```C#
// Disable agent by IP
new RemoteTc().Connect(a => a.ToHost("tc").AsGuest())
    .DisableAgent(_ => _.Ip("127.0.0.1"));

// Enable agent by name
new RemoteTc().Connect(a => a.ToHost("tc").AsGuest())
    .EnableAgent(_ => _.Name("agent1"));

// Get project by ID
var project = new RemoteTc().Connect(a => a.ToHost("tc").AsGuest())
    .GetProjectById("FluentTc");

// Get connected Agents
var agents = new RemoteTc().Connect(a => a.ToHost("tc").AsGuest())
    .GetAgents(h => h.Connected());

// Get disconnected, enabled and authorized agents
var enabledAuthorizedButDisconnectedAgents = new RemoteTc().Connect(a => a.ToHost("tc").AsGuest())
    .GetAgents(h => h.Disconnected().Enabled().Authorized());

// Get builds queue from specific project by its ID
var buildQueue = new RemoteTc().Connect(_ => _.ToHost("tc"))
    .GetBuildsQueue(_ => _.Project(__ => __.Id("OpenSourceProject")));

// Get builds queue from specific project by its ID and build configuration name
var buildQueue2 = new RemoteTc().Connect(_ => _.ToHost("tc"))
    .GetBuildsQueue(
        __ =>
            __.Project(___ => ___.Id("OpenSourceProject"))
                .BuildConfiguration(b => b.Name("FluentTc")));

// Remove builds from queue by project Id recursively 
var connectedTc = new RemoteTc().Connect(_ => _.ToHost("tc"));
connectedTc.GetBuildConfigurationsRecursively("ProjectId")
    .ForEach(c => connectedTc.RemoveBuildFromQueue(__ => __.BuildConfiguration(___ => ___.Id(c.Id))));

// Get not personal builds, under build configuration bt2, under project OpenSourceProject, that ran on agent Agent01 on branch master
var builds = new RemoteTc().Connect(a => a.ToHost("tc").AsGuest())
    .GetBuilds(
        h =>
            h.BuildConfiguration(r => r.Id("bt2"))
                .NotPersonal()
                .Project(r => r.Name("OpenSourceProject"))
                .AgentName("Agent01")
                .Branch(b => b.Name("master")));

// Get personal builds with additional properties: StartDate, FinishDate and StatusText
builds = new RemoteTc().Connect(_ => _.ToHost("tc"))
    .GetBuilds(_ => _.Personal(), _ => _.DefaultCount(),
        _ => _.IncludeStartDate().IncludeFinishDate().IncludeStatusText());

// Get 5 personal builds with default properties
builds = new RemoteTc().Connect(_ => _.ToHost("tc"))
    .GetBuilds(_ => _.Personal(), _ => _.Count(5), _ => _.IncludeDefaults());

// Get 5 not personal builds from build configuration bt2 with default properties
builds = new RemoteTc().Connect(_ => _.ToHost("tc"))
    .GetBuilds(_ => _.BuildConfiguration(x => x.Id("bt2")).NotPersonal().NotRunning(), _ => _.Count(5),
        _ => _.IncludeDefaults());

// Get specific build by ID (with all the properties)
build = new RemoteTc().Connect(_ => _.ToHost("tc"))
    .GetBuild(_ => _.Id(123456));

// Get build configuration by ID
var buildConfiguration = new RemoteTc().Connect(_ => _.ToHost("tc"))
    .GetBuildConfiguration(_ => _.Id("bt2"));

// Retrieves all the build configuration under a project
var buildConfigurations = new RemoteTc().Connect(_ => _.ToHost("tc").AsGuest())
    .GetBuildConfigurations(_ => _.Project(__ => __.Id("Trunk")));

// Retrieves all the build configuration under a project recursively
buildConfigurations = new RemoteTc().Connect(_ => _.ToHost("tc").AsGuest())
    .GetBuildConfigurations(_ => _.ProjectRecursively(__ => __.Id("Trunk")));

// Set build parameters on build configuration bt2
new RemoteTc().Connect(_ => _.ToHost("tc"))
    .SetParameters(_ => _.Id("bt2"),
        _ => _.Parameter("name", "value").Parameter("name2", "value"));

// Run build configuration bt2
new RemoteTc().Connect(_ => _.ToHost("tc"))
    .RunBuildConfiguration(_ => _.Id("bt2"));

// Run build configuration bt2 with custom build parameters
new RemoteTc().Connect(_ => _.ToHost("tc"))
    .RunBuildConfiguration(_ => _.Id("bt2"),
        _ => _.Parameter("name", "value").Parameter("name2", "value"));

// Run build configuration bt2 on specific agent
new RemoteTc().Connect(_ => _.ToHost("tc"))
    .RunBuildConfiguration(having => having.Id("bt2"), onAgent => onAgent.Name("agent1"));

// Run build configuration bt2 on specific agent with custom parameters
new RemoteTc().Connect(_ => _.ToHost("tc"))
    .RunBuildConfiguration(_ => _.Id("bt2"), _ => _.Name("agent1"),
        _ => _.Parameter("name", "value").Parameter("name2", "value"));

// Create build configuration under a project
buildConfiguration = new RemoteTc().Connect(_ => _.ToHost("tc"))
    .CreateBuildConfiguration(_ => _.Id("Trunk"), "config name");

// Retrieves all the projects
var allProjects = new RemoteTc().Connect(_ => _.ToHost("tc").AsGuest())
    .GetAllProjects();
```
# How can I help
* Vote  http://nugetmusthaves.com/Package/FluentTc
* Contribute 

# TeamCity REST API reference
hhttps://confluence.jetbrains.com/display/TCD9/REST+API

## Dependencies
* Autofac http://autofac.org/ (≥ 3.5.2)
* System.IO.Abstractions https://github.com/tathamoddie/System.IO.Abstractions (≥ 2.0.0.116)
* EasyHttp https://github.com/hhariri/EasyHttp (== 1.6.67.0)

## Guidelines for contributors
* Fork this repository 
* Choose issue from open issues 
* Develop using (A)TDD, Clean Code principles, SOLID
* Strive to fluent, readable, easy to discover API
* Use (A)TDD, Clean Code
* Submit Pull Request


## License
Apache License 2.0



