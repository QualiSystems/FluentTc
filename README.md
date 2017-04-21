﻿[![build status](http://teamcity.codebetter.com/app/rest/builds/buildType:id:FluentTc/statusIcon)](http://teamcity.codebetter.com/viewType.html?buildTypeId=FluentTc&guest=1) [![code coverage](https://img.shields.io/teamcity/coverage/FluentTc.svg)](http://teamcity.codebetter.com/viewType.html?buildTypeId=FluentTc&guest=1) [![NuGet version](https://badge.fury.io/nu/FluentTc.svg)](https://badge.fury.io/nu/FluentTc)  [![Stories in Ready](https://badge.waffle.io/QualiSystems/FluentTc.png?label=ready&title=Ready)](https://waffle.io/QualiSystems/FluentTc)

# FluentTc 
Integrate with TeamCity fluently

# Getting Started
Get TeamCity builds with branches, status, start and finish date and paging
```C#
IList<IBuild> builds =
    new RemoteTc()
        .Connect(connect => connect
            .ToHost("teamcity.jetbrains.com")
            .AsGuest())
        .GetBuilds(
            having => having
                .BuildConfiguration(
                    buildConfiguration => buildConfiguration
                        .Id("FluentTc"))
                .Branch(branch => branch.Branched()),
            include => include
                .IncludeStatusText()
                .IncludeStartDate()
                .IncludeFinishDate(), 
            paging => paging
                .Start(30)
                .Count(10));
```

Run custom build
```C#
IBuild build = new RemoteTc()
    .Connect(connect => connect
        .ToHost("teamcity.jetbrains.com")
        .AsGuest())
    .RunBuildConfiguration(
         buildConfiguration => buildConfiguration.Id("FluentTc"), 
         agent => agent.Name("Agent1"),
         parameters => parameters
                     .Parameter("param1", "value1")
                     .Parameter("param2", "value2"),
         options => options.OnBranch("develop")
                    .WithComment("personal build on develop")
                    .AsPersonal()
                    .QueueAtTop()
                    .RebuildAllDependencies()
                    .WithCleanSources()
                    .OnChange(change => change.Id(123456)));
```

Interact with TeamCity from within a build step without authentication
```C#
ILocalTc localTc = new LocalTc();

// Gets the current checkout directory
string agentWorkDir = localTc.TeamcityBuildCheckoutDir;

// Gets the current parameter value 
int param1 = localTc.GetBuildParameter<int>("param1");

// Sets parameter value of the current build
localTc.SetBuildParameter("parameter.name", "value1");

// Gets list of files changed in the current build
IList<IChangedFile> changedFiles = localTc.ChangedFiles;

// Determines whether the build is personal
bool isPersonal = localTc.IsPersonal;

// Change status of the current build 
localTc.ChangeBuildStatus(BuildStatus.Success);
```

# Dive In

* For more examples and documentation read the [Wiki](https://github.com/QualiSystems/FluentTc/wiki)

* For questions and discussions chat on Gitter:  [![Chat on gitter](https://img.shields.io/gitter/room/QualiSystems/FluentTc.svg)](https://gitter.im/QualiSystems/FluentTc)


# Download

The easiest way to get __FluentTc__ is through Nuget: Visual Studio -> Manage Nuget packages or from Nuget Package Manager Console:


# Get Involved
* For reporting bugs, create an issue on [Github issues](https://github.com/QualiSystems/FluentTc/issues)
* For contribution fork and submit a change via Pull Request 

## Versioning
FluentTc adheres to [Semantic Versioning 2.0.0](http://semver.org/), basically means that there are no breaking changes unless the version is 0.x or major version is promoted. 

## License
FluentTc is released under [Apache License 2.0](https://github.com/QualiSystems/FluentTc/blob/master/LICENSE)

## Credits
FluentTc project keeps growing with the support of growing comminity of [contributors](https://github.com/QualiSystems/FluentTc/graphs/contributors)