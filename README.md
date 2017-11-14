# FluentTc 
Easy-to-use, readable and comprehensive library for consuming TeamCity REST API. Written using real scenarios in mind, enables variuos range of queries and operation on TeamCity

## Installation
Run from NuGet Package Manager console: 
```PowerShell
PM > Install-Package FluentTc
```

## Usage
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

Run build on build configuration FluentTc on agent Agent1 with parameters on branch develop, with comment 'personal build on develop', as a personal build, queue on top, rebuild all dependencies on change 123456
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

## Quick Links
* Documentation: [https://github.com/QualiSystems/FluentTc/wiki](https://github.com/QualiSystems/FluentTc/wiki)
* Questions: [https://stackoverflow.com/questions/tagged/fluenttc](https://stackoverflow.com/questions/tagged/fluenttc)
* Bug/Feature Tracking: [https://github.com/QualiSystems/FluentTc/issues](https://github.com/QualiSystems/FluentTc/issues)

## Project status
* Continuous Integration: [![build status](http://teamcity.codebetter.com/app/rest/builds/buildType:id:FluentTc/statusIcon)](http://teamcity.codebetter.com/viewType.html?buildTypeId=FluentTc&guest=1) 
* Code Coverage: [![code coverage](https://img.shields.io/teamcity/coverage/FluentTc.svg)](http://teamcity.codebetter.com/viewType.html?buildTypeId=FluentTc&guest=1)  
* Downloads: [![NuGet](https://img.shields.io/nuget/dt/fluenttc.svg)]()

## Contribute
The best way to contribute is by **spreading the word** about the library:

 - Blog it
 - Comment it
 - Fork it
 - Star it
 - Share it
 
A **HUGE THANKS** for your help.

## Contributors
FluentTc keeps growing with the support of of [contributors](https://github.com/QualiSystems/FluentTc/graphs/contributors)

## License
[Apache License 2.0](https://github.com/QualiSystems/FluentTc/blob/master/LICENSE)

