#Cake

Cake (C# Make) is a build automation system using C#.

[![TeamCity CI Build Status](http://builds.nullreferenceexception.se/app/rest/builds/buildType:id:Cake_Continuous/statusIcon)](http://builds.nullreferenceexception.se/viewType.html?buildTypeId=Cake_Continuous&guest=1)

##Table of contents

1. [Roadmap](https://github.com/cake-build/cake#roadmap)
2. [Implemented functionality](https://github.com/cake-build/cake#implemented-functionality)
3. [Example](https://github.com/cake-build/cake#example)
    - [Download Cake](https://github.com/cake-build/cake#1-download-cake)
    - [Create build script](https://github.com/cake-build/cake#2-create-build-script)
    - [Run build script](https://github.com/cake-build/cake#3-run-build-script)
4. [Documentation](https://github.com/cake-build/cake#documentation)
5. [Contributors](https://github.com/cake-build/cake#contributors)
6. [Contributing](https://github.com/cake-build/cake#contributing)
7. [External add-ons](https://github.com/cake-build/cake#external-add-ons)
8. [License](https://github.com/cake-build/cake#license)

##Roadmap

The Cake engine is pretty much done, but there are still improvements to be made. I'm still experimenting with the script API to make it as easy and intuitive as possible, so expect changes along the road.

A roadmap can be found [here](https://github.com/cake-build/cake/milestones).

##Implemented functionality

This is a list of the currently implemented functionality.

* MSBuild
* MSTest
* xUnit
* NUnit
* NuGet pack
* NuGet push
* NuGet restore
* ILMerge
* WiX (Candle and Light)
* File copying/moving/deleting
* Directory creation/cleaning/deleting
* File/Directory globbing
* Compression (zip)
* AssemblyInfo patching
* Release notes parser

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

## Contributors

Contributions have been accepted from:

Patrik Svensson ([@patriksvensson](https://github.com/patriksvensson))   
Mattias Karlsson ([@devlead](https://github.com/devlead))   
Fredrik Leijon ([@FredrikL](https://github.com/FredrikL))   
Viktor Elofsson ([@vktr](https://github.com/vktr))   
Jeff Doolittle ([@jeffdoolittle](https://github.com/jeffdoolittle))   
Richard Simpson ([@RichiCoder1](https://github.com/RichiCoder1))   

Thank you very much!

## Contributing

So you’re thinking about contributing to Cake? Great! It’s **really** appreciated.   

Make sure you've read the [contribution guidelines](http://cake.readthedocs.org/en/latest/contribution-guidelines.html) before sending that epic pull request.

* Fork the repository.
* Make your feature addition or bug fix.
* Don't forget the unit tests.
* Send a pull request. Bonus for topic branches. *Funny .gif will be your reward.*  

## External add-ons

AliaSql: [https://www.nuget.org/packages/Cake.AliaSql](https://www.nuget.org/packages/Cake.AliaSql)

## License

Copyright (c) 2014, Patrik Svensson and contributors.   
Cake is provided as-is under the MIT license. For more information see `LICENSE`.

* For Roslyn, see https://roslyn.codeplex.com/license
* For Autofac, see https://github.com/autofac/Autofac/blob/master/LICENSE
* For NuGet.Core, see https://nuget.codeplex.com/license