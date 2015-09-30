#Cake [![NuGet](https://img.shields.io/nuget/v/Cake.svg)](https://www.nuget.org/packages/Cake)

Cake (C# Make) is a build automation system with a C# DSL to do things like compiling code, copy files/folders, running unit tests, compress files and build NuGet packages.

| Platform    | Status                                                                                                                    |
|-------------|---------------------------------------------------------------------------------------------------------------------------|
| Windows     | [![AppVeyor](https://img.shields.io/appveyor/ci/cakebuild/cake.svg)](https://ci.appveyor.com/project/cakebuild/cake)      |
| Linux / OSX | [![Travis build status](https://travis-ci.org/cake-build/cake.svg?branch=develop)](https://travis-ci.org/cake-build/cake) |

## Table of contents

1. [Implemented functionality](https://github.com/cake-build/cake#implemented-functionality)
2. [Example](https://github.com/cake-build/cake#example)
    - [Download Cake](https://github.com/cake-build/cake#1-download-cake)
    - [Create build script](https://github.com/cake-build/cake#2-create-build-script)
    - [Run build script](https://github.com/cake-build/cake#3-run-build-script)
3. [Documentation](https://github.com/cake-build/cake#documentation)
4. [Bootstrapper](https://github.com/cake-build/cake#bootstrapper)
5. [Contributing](https://github.com/cake-build/cake#contributing)
6. [External add-ons](https://github.com/cake-build/cake#external-add-ons)
7. [Get in touch](https://github.com/cake-build/cake#get-in-touch)
7. [License](https://github.com/cake-build/cake#license)

## Implemented functionality

This is a list of some the currently implemented functionality.   
For a full list of supported tools, see the [DSL reference](http://cakebuild.net/dsl/).

* [MSBuild](http://cakebuild.net/dsl/msbuild)
* [MSTest](http://cakebuild.net/dsl/mstest)
* [xUnit (v1)](http://cakebuild.net/dsl/xunit)
* [xUnit (v2)](http://cakebuild.net/dsl/xunit-v2)
* [NUnit](http://cakebuild.net/dsl/nunit)
* [NuGet](http://cakebuild.net/dsl/nuget)
  * Pack
  * Push
  * Restore
  * Sources
* [ILMerge](http://cakebuild.net/dsl/ilmerge)
* [WiX](http://cakebuild.net/dsl/wix)
  * Candle
  * Light
* [SignTool](http://cakebuild.net/dsl/signing)
* [File operations](http://cakebuild.net/dsl/file-operations)
  * Copying
  * Moving
  * Deleting
* [Directory operations](http://cakebuild.net/dsl/directory-operations)
  * Creation
  * Cleaning
  * Deleting
* [File/Directory globbing](http://cakebuild.net/dsl/globbing)
* [Compression (zip)](http://cakebuild.net/dsl/compression)
* [AssemblyInfo patching](http://cakebuild.net/dsl/assembly-info)
* [Release notes parser](http://cakebuild.net/dsl/release-notes)
* [AppVeyor](http://cakebuild.net/dsl/build-system)
* [MSBuild Resource](http://cakebuild.net/dsl/msbuild-resource)
  * Solution file parsing
  * Project file parsing
* [Octopus deploy](http://cakebuild.net/dsl/octopus-deploy)
  * Create release

For more information and examples of how to use Cake, see the [Documentation](http://cakebuild.net/docs).

## Example

### 1. Download Cake

```batchfile
C:\Project> NuGet.exe install Cake -OutputDirectory Tools -ExcludeVersion
```

### 2. Create build script

```csharp
var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

// Define directories.
var buildDir = Directory("./src/Example/bin") + Directory(configuration);

Task("Clean")
    .Does(() =>
{
    CleanDirectory(buildDir);
});

Task("Restore-NuGet-Packages")
    .IsDependentOn("Clean")
    .Does(() =>
{
    NuGetRestore("./src/Example.sln");
});

Task("Build")
    .IsDependentOn("Restore-NuGet-Packages")
    .Does(() =>
{
    MSBuild("./src/Example.sln", new MSBuildSettings()
        .UseToolVersion(MSBuildToolVersion.NET45)
        .SetVerbosity(Verbosity.Minimal)
        .SetConfiguration(configuration));
});

Task("Run-Unit-Tests")
    .IsDependentOn("Build")
    .Does(() =>
{
    XUnit2("./src/**/bin/" + configuration + "/*.Tests.dll");
});

Task("Default")
    .IsDependentOn("Run-Unit-Tests");

RunTarget(target);
```

### 3. Run build script

```
C:\Project\Tools\Cake> Cake.exe ../../build.cake -verbosity=verbose -target=Build
```

You could of course use our bootstrapper script if you want to. More information can be found in the [tutorial](http://cakebuild.net/docs/tutorials/setting-up-a-new-project)

## Documentation

You can read the latest documentation at [http://cakebuild.net/](http://cakebuild.net/).

## Bootstrapper

The Cake [Bootstrapper](https://github.com/cake-build/bootstrapper) is a Powershell cmdlet that helps you set up a new Cake build by downloading dependencies, setting up the bootstrapper script and creating a Cake build script.

## Contributing

So you’re thinking about contributing to Cake? Great! It’s **really** appreciated.   

Make sure you've read the [contribution guidelines](http://cakebuild.net/contribute/contribution-guidelines/) before sending that epic pull request.

* Fork the repository.
* Make your feature addition or bug fix.
* Don't forget the unit tests.
* Send a pull request.

## Get in touch

[![Follow @cakebuildnet](https://img.shields.io/badge/Twitter-Follow%20%40cakebuildnet-blue.svg)](https://twitter.com/intent/follow?screen_name=cakebuildnet)

[![Join the chat at https://gitter.im/cake-build/cake](https://badges.gitter.im/Join%20Chat.svg)](https://gitter.im/cake-build/cake?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)

## License

Copyright © 2014 - 2015, Patrik Svensson, Mattias Karlsson and contributors.
Cake is provided as-is under the MIT license. For more information see [LICENSE](https://github.com/cake-build/cake/blob/develop/LICENSE).

* For Roslyn, see https://github.com/dotnet/roslyn/blob/master/License.txt
* For Mono.CSharp, see https://github.com/mono/mono/blob/master/mcs/LICENSE
* For Autofac, see https://github.com/autofac/Autofac/blob/master/LICENSE
* For NuGet.Core, see https://nuget.codeplex.com/license
