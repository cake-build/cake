#Cake

Cake (C# Make) is a build automation system inspired by [Fake](http://fsharp.github.io/FAKE/).

[![TeamCity CI Build Status](http://builds.nullreferenceexception.se/app/rest/builds/buildType:id:Cake_Continuous/statusIcon)](http://builds.nullreferenceexception.se/viewType.html?buildTypeId=Cake_Continuous&guest=1)

##Roadmap

The Cake engine is pretty much done, but there are still improvements to be made. I'm still experimenting with the script API to make it as easy and intuitive as possible, so expect changes along the road.

Currently only basic MSBuild and xUnit support are implemented, but more features such as NuGet, ILMerge, NUnit, MSTest and compression support are planned. The full roadmap can be found [here](https://github.com/patriksvensson/cake/issues/milestones).

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
    // Delete the bin directories (via script host).
    CleanDirectories("./src/**/bin/" + configuration);
});

Task("Build")
    .IsDependentOn("Clean")
    .Does(() =>
{
    // Build project using MSBuild (via script host)
    MSBuild("./src/Cake.sln", settings => 
        settings.SetPlatformTarget(PlatformTarget.x86)
            .UseToolVersion(MSBuildToolVersion.NET45)
            .WithProperty("Magic","1")
            .WithTarget("Build")
            .SetConfiguration(configuration));         
});

Task("Run-Tests")
    .IsDependentOn("Build")
    .Does(() =>
{
    // Run unit tests (via script host)
    XUnit("./src/**/bin/" + configuration + "/*.Tests.dll");
});

// Run the script.
Run("Run-Tests");
```

###3. Run build script

```
C:\Project\Tools\Cake> Cake.exe ../../build.csx -verbosity=diagnostic -teamCity
```
