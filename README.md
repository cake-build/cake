#Cake

Cake (C# Make) is a build automation system inspired by [Fake](http://fsharp.github.io/FAKE/).

[![TeamCity CI Build Status](http://builds.nullreferenceexception.se/app/rest/builds/buildType:id:Cake_Continuous/statusIcon)](http://builds.nullreferenceexception.se/viewType.html?buildTypeId=Cake_Continuous&guest=1)

##Roadmap

The Cake engine is pretty much done, but there are still improvements to be made. I'm still experimenting with the script API to make it as easy and intuitive as possible, so expect changes along the road.

Currently basic MSBuild, xUnit, NuGet, compression and basic file system operations are implemented, but more features such as ILMerge, NUnit, MSTest support are planned. The full roadmap can be found [here](https://github.com/cake-build/cake/issues/milestones).

For more information and examples of how to use Cake, see the [Wiki](https://github.com/cake-build/cake/wiki).

##Example

###1. Download Cake

```
C:\Project> NuGet.exe install Cake -OutputDirectory Tools -ExcludeVersion
```

###2. Create build script

```CSharp
var isTeamCityBuild = HasArgument("teamCity");
var configuration = Argument("configuration", defaultValue: "Release");

// Access the log via script host and print some debug info.
Log.Debug("teamCity={0}", isTeamCityBuild);
Log.Debug("configuration={0}", configuration);

////////////////////////////////////////////////////////////////////////////
// All functionality is implemented as extension methods for ICakeContext.
// For convenience, all built in functionality (such as MSBuild, xUnit etc) 
// is also exposed directly on the script host for convenience.
////////////////////////////////////////////////////////////////////////////

Task("Hello")
    .WithCriteria(isTeamCityBuild)
    .Does(context =>
{
    // Access log via context.    
    context.Log.Information("Hello TeamCity!");
});

Task("Clean")
    .IsDependentOn("Hello")
    .Does(() =>
{
    // Clean directories.
    CleanDirectory("./build")
    CleanDirectories("./src/**/bin/" + configuration);
});

Task("Build")
    .IsDependentOn("Clean")
    .Does(() =>
{
    // Build project using MSBuild.
    MSBuild("./src/Cake.sln", settings => 
        settings.SetPlatformTarget(PlatformTarget.x86)
            .UseToolVersion(MSBuildToolVersion.NET45)
            .WithProperty("Magic","1")
            .WithTarget("Build")
            .SetConfiguration(configuration));         
});

Task("Unit-Tests")
    .IsDependentOn("Build")
    .Does(() =>
{
    // Run unit tests.
    XUnit("./src/**/bin/" + configuration + "/*.Tests.dll");
});

Task("Pack")
    .IsDependentOn("Unit-Tests")
    .Does(() =>
{   
    var root = "./src/Cake/bin/" + configuration;
    var output = "./build/" + configuration + ".zip";
    var files = root + "/*";

    // Package the bin folder.
    Zip(root, output, files);
});

Task("NuGet")
    .IsDependentOn("Pack")
    .Does(() =>
{
    // Create NuGet package.
    NuGetPack("./Cake.nuspec", new NuGetPackSettings
    {
        Version = "0.1.0",
        BasePath = "./src/Cake/bin/" + configuration,
        OutputDirectory = "./build",
        NoPackageAnalysis = true
    });
});

// Get the build target from the arguments.
// Default to NuGet if no target parameter was provided.
var buildTarget = Argument("target", defaultValue: "NuGet");

// Run the build target.
Run(buildTarget);
```

###3. Run build script

```
C:\Project\Tools\Cake> Cake.exe ../../build.csx -verbosity=diagnostic -teamCity -target=Pack
```
