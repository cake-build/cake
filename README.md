#Cake

Cake (C# Make) is a build automation system with a C# DSL to do things like compiling code, copy files/folders, running unit tests, compress files and build NuGet packages.

[![Build status](https://ci.appveyor.com/api/projects/status/s9oscm9t7ase6h6d?svg=true)](https://ci.appveyor.com/project/cakebuild/cake)
[![Coverity Scan](https://scan.coverity.com/projects/4147/badge.svg)](https://scan.coverity.com/projects/4147) 

[![Follow @cakebuildnet](https://img.shields.io/badge/Twitter-Follow%20%40cakebuildnet-blue.svg)](https://twitter.com/intent/follow?screen_name=cakebuildnet)

[![cakebuild.net](https://img.shields.io/badge/WWW-cakebuild.net-blue.svg)](http://cakebuild.net/)

[![Join the chat at https://gitter.im/cake-build/cake](https://badges.gitter.im/Join%20Chat.svg)](https://gitter.im/cake-build/cake?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)

##Table of contents

1. [Roadmap](https://github.com/cake-build/cake#roadmap)
2. [Implemented functionality](https://github.com/cake-build/cake#implemented-functionality)
3. [Example](https://github.com/cake-build/cake#example)
    - [Download Cake](https://github.com/cake-build/cake#1-download-cake)
    - [Create build script](https://github.com/cake-build/cake#2-create-build-script)
    - [Run build script](https://github.com/cake-build/cake#3-run-build-script)
4. [Documentation](https://github.com/cake-build/cake#documentation)
5. [Bootstrapper](https://github.com/cake-build/cake#bootstrapper)
6. [Contributing](https://github.com/cake-build/cake#contributing)
7. [Contributors](https://github.com/cake-build/cake#contributors)
8. [External add-ons](https://github.com/cake-build/cake#external-add-ons)
9. [License](https://github.com/cake-build/cake#license)

##Roadmap

The Cake engine is pretty much done, but there are still improvements to be made. I'm still experimenting with the script API to make it as easy and intuitive as possible, so expect changes along the road.

A roadmap can be found [here](https://github.com/cake-build/cake/milestones).

##Implemented functionality

This is a list of some the currently implemented functionality.   
For a full list of supported tools, see the [DSL reference](http://cakebuild.net/dsl/).

* [MSBuild](http://cakebuild.net/dsl/#msbuild) 
* [MSTest](http://cakebuild.net/dsl/#mstest)
* [xUnit (v1 and v2)](http://cakebuild.net/dsl/#xunit)
* [NUnit](http://cakebuild.net/dsl/#nunit)
* [NuGet](http://cakebuild.net/dsl/#nuget)
  * Pack
  * Push
  * Restore
  * Sources
* [ILMerge](http://cakebuild.net/dsl/#ilmerge)
* [WiX](http://cakebuild.net/dsl/#wix)
  * Candle
  * Light
* [SignTool](http://cakebuild.net/dsl/#signing)
* [File operations](http://cakebuild.net/dsl/#fileoperations)
  * Copying
  * Moving
  * Deleting
* [Directory operations](http://cakebuild.net/dsl/#directoryoperations)
  * Creation
  * Cleaning
  * Deleting
* [File/Directory globbing](http://cakebuild.net/dsl/#globbing)
* [Compression (zip)](http://cakebuild.net/dsl/#compression)
* [AssemblyInfo patching](http://cakebuild.net/dsl/#assemblyinfo)
* [Release notes parser](http://cakebuild.net/dsl/#releasenotes)
* [AppVeyor](http://cakebuild.net/dsl/#buildsystem)
* [MSBuild Resource](http://cakebuild.net/dsl/#msbuildresource)
  * Solution file parsing
  * Project file parsing
* [Octopus deploy](http://cakebuild.net/dsl/#octopusdeploy)
  * Create release

For more information and examples of how to use Cake, see the [Documentation](http://cakebuild.net/). 

##Example

###1. Download Cake

```Batchfile
C:\Project> NuGet.exe install Cake -OutputDirectory Tools -ExcludeVersion
```

###2. Create build script

```CSharp
var target = Argument("target", "NuGet");
var configuration = Argument("configuration", "Release");

Task("Clean")
    .Does(() =>
{
    // Clean directories.
    CleanDirectory("./build");
    CleanDirectory("./build/bin");
    CleanDirectories("./src/**/bin/" + configuration);
});

Task("Restore-NuGet-Packages")
    .IsDependentOn("Clean")
    .Does(context =>
{
    // Restore NuGet packages.
    NuGetRestore("./src/Cake.sln");    
});

Task("Build")
    .IsDependentOn("Restore-NuGet-Packages")
    .Does(() =>
{
    MSBuild("./src/Cake.sln", s => 
        s.SetConfiguration(configuration));
});

Task("Unit-Tests")
    .IsDependentOn("Build")
    .Does(() =>
{
    XUnit("./src/**/bin/" + configuration + "/*.Tests.dll");
});

Task("Copy-Files")
    .IsDependentOn("Unit-Tests")
    .Does(() =>
{
    var sourcePath = "./src/Cake/bin/" + configuration;    
    var files = GetFiles(sourcePath + "/**/*.dll") + GetFiles(sourcePath + "/**/*.exe");
    var destinationPath = "./build/bin";

    CopyFiles(files, destinationPath);
});

Task("Pack")
    .IsDependentOn("Copy-Files")
    .Does(() =>
{   
    var root = "./build/bin";
    var output = "./build/" + configuration + ".zip";

    Zip(root, output);
});

Task("NuGet")
    .Description("Create NuGet package")
    .IsDependentOn("Pack")
    .Does(() =>
{
    // Create NuGet package.
    NuGetPack("./Cake.nuspec", new NuGetPackSettings {
        Version = "0.1.0",
        BasePath = "./build/bin",
        OutputDirectory = "./build",
        NoPackageAnalysis = true
    });
});

RunTarget(target);
```

###3. Run build script

```
C:\Project\Tools\Cake> Cake.exe ../../build.csx -verbosity=verbose -target=Pack
```

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
* Send a pull request. Bonus for topic branches. *Funny .gif will be your reward.*  

## Contributors

The full list of contributors can be found at [http://cakebuild.net/contribute/list-of-contributors/](http://cakebuild.net/contribute/list-of-contributors/).

## External add-ons

Cake.AliaSql: [https://www.nuget.org/packages/Cake.AliaSql](https://www.nuget.org/packages/Cake.AliaSql)  
Cake.Unity: [https://github.com/patriksvensson/Cake.Unity](https://github.com/patriksvensson/Cake.Unity)  
Cake.Slack: [https://github.com/WCOMAB/Cake.Slack](https://github.com/WCOMAB/Cake.Slack)

## License

Copyright (c) 2014, Patrik Svensson and contributors.   
Cake is provided as-is under the MIT license. For more information see `LICENSE`.

* For Roslyn, see https://github.com/dotnet/roslyn/blob/master/License.txt
* For Autofac, see https://github.com/autofac/Autofac/blob/master/LICENSE
* For NuGet.Core, see https://nuget.codeplex.com/license
