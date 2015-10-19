# FluentTc
.NET TeamCity fluent API 

## Goal
Develop easy-to-use, readable, convenient and fluent library for accessing TeamCity REST API. 
Full documentation is here: https://confluence.jetbrains.com/display/TCD9/REST+API

## Dependencies
* EasyHttp https://github.com/hhariri/EasyHttp
* JsonFx  https://github.com/jsonfx/jsonfx

## Guidelines for contributors
* Fork this repository 
* Install GitExtensions for Visual Studio
* Develop using TDD: test-first
* There should be single access point to all the methods: RemoteTc class
* Library's API should be fluent, i.e. enable chaining methods one after the other 
* Commit, push and pull-request frequently
* Take example of the current API and update usage:
https://github.com/borismod/FluentTc/blob/master/FluentTc.Tests/RemoteTcTests.cs

## License
Apache License 2.0



