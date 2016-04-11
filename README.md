[![build status](http://teamcity.codebetter.com/app/rest/builds/buildType:id:FluentTc/statusIcon)](http://teamcity.codebetter.com/viewType.html?buildTypeId=FluentTc&guest=1) [![code coverage](https://img.shields.io/teamcity/coverage/FluentTc.svg)](http://teamcity.codebetter.com/viewType.html?buildTypeId=FluentTc&guest=1) [![NuGet version](https://badge.fury.io/nu/FluentTc.svg)](https://badge.fury.io/nu/FluentTc)  [![Chat on gitter](https://img.shields.io/gitter/room/QualiSystems/FluentTc.svg)](https://gitter.im/QualiSystems/FluentTc) [![Stories in Ready](https://badge.waffle.io/QualiSystems/FluentTc.png?label=ready&title=Ready)](https://waffle.io/QualiSystems/FluentTc)

# FluentTc 
Easy-to-use, readable and comprehensive library for consuming TeamCity REST API. Written using real scenarios in mind, enables variuos range of queries and operation on TeamCity

# Getting started 

Install __FluentTc__ nuget package from from Manage Nuget packages

Or from Nuget Package Manager Console:
__install-package FluentTc__

# Example 
```C#
var builds = new RemoteTc().Connect(_ => _.ToHost("tc"))
    .GetBuilds(
        _ => _.BuildConfiguration(x => x.Id("bt2"))
              .NotPersonal()
              .NotRunning()
              .Branch(_ => _.NotBranched()), 
        _ => _.Start(10).Count(5),
        _ => _.IncludeStartDate().IncludeFinishDate());
```

For more examples and documentation read the [Wiki](https://github.com/QualiSystems/FluentTc/wiki)

## License
[Apache License 2.0](https://github.com/QualiSystems/FluentTc/blob/master/LICENSE)

## Credits
[![Continuous Integration powered by TeamCity and CodeBetter](https://resources.jetbrains.com/assets/banners/jetbrains-com/Codebetter.png)](http://codebetter.com/codebetter-ci/)
