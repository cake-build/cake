#Cake

Cake (C# Make) is a build automation system inspired by [Fake](http://fsharp.github.io/FAKE/).

##Example

###1. Download Cake

```
C:\Project> NuGet.exe install Cake -OutputDirectory Tools -ExcludeVersion
```

###2. Create build script

```CSharp
var isTeamCityBuild = HasArgument("teamCity");
var configuration = Argument("configuration", defaultValue: "Debug");

// Access the log via script host and print some debug info.
Log.Debug("teamCity={0}", isTeamCityBuild);
Log.Debug("configuration={0}", configuration);

Task("Hello")
    .WithCriteria(isTeamCityBuild)
    .Does(c =>
{
    // Access log via context.
    c.Log.Information("Hello TeamCity!");
});

Task("Build")
    .IsDependentOn("Hello")
    .Does(c =>
{
    // Build project using MSBuild
    c.MSBuild("./src/Cake.sln", s => 
        s.WithProperty("Magic","1")
         .WithTarget("Build")
         .SetConfiguration(configuration));
});

Task("Run-Tests")
    .IsDependentOn("Build")
    .Does(c =>
{
    // Run unit tests.
    c.XUnit("./src/**/bin/" + configuration + "/*.Tests.dll");
});

// Run the script.
Run("Run-Tests");
```

###3. Run build script

```
C:\Project\Tools\Cake> Cake.exe ../../build.csx -verbosity=diagnostic -teamCity