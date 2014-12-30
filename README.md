#Cake

Cake (C# Make) is a build automation system using C#.

[![Build status](https://ci.appveyor.com/api/projects/status/c6lw0vvj1mf4395a/branch/develop)](https://ci.appveyor.com/project/patriksvensson/cake/branch/develop)

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

This is a list of the currently implemented functionality.

* [MSBuild](http://cake.readthedocs.org/en/latest/api-documentation.html#msbuild) 
* [MSTest](http://cake.readthedocs.org/en/latest/api-documentation.html#mstest)
* [xUnit (v1 and v2)](http://cake.readthedocs.org/en/latest/api-documentation.html#xunit)
* [NUnit](http://cake.readthedocs.org/en/latest/api-documentation.html#nunit)
* [NuGet](http://cake.readthedocs.org/en/latest/api-documentation.html#nuget)
  * Pack
  * Push
  * Restore
  * Sources
* [ILMerge](http://cake.readthedocs.org/en/latest/api-documentation.html#ilmerge)
* [WiX](http://cake.readthedocs.org/en/latest/api-documentation.html#wix)
  * Candle
  * Light
* [SignTool](http://cake.readthedocs.org/en/latest/api-documentation.html#signing)
* [File operations](http://cake.readthedocs.org/en/latest/api-documentation.html#file-operations)
  * Copying
  * Moving
  * Deleting
* [Directory operations](http://cake.readthedocs.org/en/latest/api-documentation.html#directory-operations)
  * Creation
  * Cleaning
  * Deleting
* [File/Directory globbing](http://cake.readthedocs.org/en/latest/api-documentation.html#globbing)
* [Compression (zip)](http://cake.readthedocs.org/en/latest/api-documentation.html#compression)
* [AssemblyInfo patching](http://cake.readthedocs.org/en/latest/api-documentation.html#assembly-info)
* [Release notes parser](http://cake.readthedocs.org/en/latest/api-documentation.html#release-notes)
* [MSBuild Resource](http://cake.readthedocs.org/en/latest/api-documentation.html#msbuild-resource)
  * Solution file parsing
  * Project file parsing

For more information and examples of how to use Cake, see the [Documentation](http://cake.readthedocs.org/). 

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

You can read the latest documentation at [http://cake.readthedocs.org/](http://cake.readthedocs.org/).

## Bootstrapper

The Cake [Bootstrapper](https://github.com/cake-build/bootstrapper) is a Powershell cmdlet that helps you set up a new Cake build by downloading dependencies, setting up the bootstrapper script and creating a Cake build script.

## Contributing

So you’re thinking about contributing to Cake? Great! It’s **really** appreciated.   

Make sure you've read the [contribution guidelines](http://cake.readthedocs.org/en/latest/contribution-guidelines.html) before sending that epic pull request.

* Fork the repository.
* Make your feature addition or bug fix.
* Don't forget the unit tests.
* Send a pull request. Bonus for topic branches. *Funny .gif will be your reward.*  

## Contributors

The full list of contributors can be found at [http://cake.readthedocs.org/en/latest/contributors.html](http://cake.readthedocs.org/en/latest/contributors.html).

## External add-ons

AliaSql: [https://www.nuget.org/packages/Cake.AliaSql](https://www.nuget.org/packages/Cake.AliaSql)  
Unity: [https://github.com/patriksvensson/Cake.Unity](https://github.com/patriksvensson/Cake.Unity)

## License

Copyright (c) 2014, Patrik Svensson and contributors.   
Cake is provided as-is under the MIT license. For more information see `LICENSE`.

* For Roslyn, see https://roslyn.codeplex.com/license
* For Autofac, see https://github.com/autofac/Autofac/blob/master/LICENSE
* For NuGet.Core, see https://nuget.codeplex.com/license
