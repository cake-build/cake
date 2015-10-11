//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

//////////////////////////////////////////////////////////////////////
// PREPARATION
//////////////////////////////////////////////////////////////////////

// Get whether or not this is a local build.
var local = BuildSystem.IsLocalBuild;
var isRunningOnUnix = IsRunningOnUnix();
var isRunningOnWindows = IsRunningOnWindows();
var isRunningOnAppVeyor = AppVeyor.IsRunningOnAppVeyor;
var isPullRequest = AppVeyor.Environment.PullRequest.IsPullRequest;
var isMainCakeRepo = StringComparer.OrdinalIgnoreCase.Equals("cake-build/cake", AppVeyor.Environment.Repository.Name);

// Parse release notes.
var releaseNotes = ParseReleaseNotes("./ReleaseNotes.md");

// Get version.
var buildNumber = AppVeyor.Environment.Build.Number;
var version = releaseNotes.Version.ToString();
var semVersion = local ? version : (version + string.Concat("-build-", buildNumber));

// Define directories.
var buildDir = Directory("./src/Cake/bin") + Directory(configuration);
var buildResultDir = Directory("./build") + Directory("v" + semVersion);
var testResultsDir = buildResultDir + Directory("test-results");
var nugetRoot = buildResultDir + Directory("nuget");
var binDir = buildResultDir + Directory("bin");

///////////////////////////////////////////////////////////////////////////////
// SETUP / TEARDOWN
///////////////////////////////////////////////////////////////////////////////

Setup(() =>
{
    Information("Building version {0} of Cake.", semVersion);
});

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Clean")
    .WithCriteria(() => isRunningOnWindows)
    .Does(() =>
{
    CleanDirectories(new DirectoryPath[] {
        buildResultDir, binDir, testResultsDir, nugetRoot});
});

Task("Restore-NuGet-Packages")
    .IsDependentOn("Clean")
    .Does(() =>
{
    NuGetRestore("./src/Cake.sln", new NuGetRestoreSettings {
        Source = new List<string> {
            "https://www.nuget.org/api/v2/",
            "https://www.myget.org/F/roslyn-nightly/"
        }
    });
});

Task("Patch-Assembly-Info")
    .IsDependentOn("Restore-NuGet-Packages")
    .Does(() =>
{
    var file = "./src/SolutionInfo.cs";
    CreateAssemblyInfo(file, new AssemblyInfoSettings {
        Product = "Cake",
        Version = version,
        FileVersion = version,
        InformationalVersion = semVersion,
        Copyright = "Copyright (c) Patrik Svensson, Mattias Karlsson and contributors"
    });
});

Task("Build")
    .IsDependentOn("Patch-Assembly-Info")
    .Does(() =>
{
    if(isRunningOnUnix)
    {
        XBuild("./src/Cake.sln", new XBuildSettings()
            .SetConfiguration("Debug")
            .WithProperty("TreatWarningsAsErrors", "true")
            .SetVerbosity(Verbosity.Minimal)
        );
    }
    else
    {
        MSBuild("./src/Cake.sln", new MSBuildSettings()
            .SetConfiguration(configuration)
            .WithProperty("TreatWarningsAsErrors", "true")
            .UseToolVersion(MSBuildToolVersion.NET45)
            .SetVerbosity(Verbosity.Minimal)
            .SetNodeReuse(false));
    }
});

Task("Run-Unit-Tests")
    .IsDependentOn("Build")
    .WithCriteria(() => isRunningOnWindows)
    .Does(() =>
{
    XUnit2("./src/**/bin/" + configuration + "/*.Tests.dll", new XUnit2Settings {
        OutputDirectory = testResultsDir,
        XmlReportV1 = true
    });
});


Task("Copy-Files")
    .IsDependentOn("Run-Unit-Tests")
    .WithCriteria(() => isRunningOnWindows)
    .Does(() =>
{
    CopyFileToDirectory(buildDir + File("Cake.exe"), binDir);
    CopyFileToDirectory(buildDir + File("Cake.Core.dll"), binDir);
    CopyFileToDirectory(buildDir + File("Cake.Core.pdb"), binDir);
    CopyFileToDirectory(buildDir + File("Cake.Core.xml"), binDir);
    CopyFileToDirectory(buildDir + File("Cake.Common.dll"), binDir);
    CopyFileToDirectory(buildDir + File("Cake.Common.pdb"), binDir);
    CopyFileToDirectory(buildDir + File("Cake.Common.xml"), binDir);
    CopyFileToDirectory(buildDir + File("Mono.CSharp.dll"), binDir);
    CopyFileToDirectory(buildDir + File("Autofac.dll"), binDir);
    CopyFileToDirectory(buildDir + File("Nuget.Core.dll"), binDir);

    // Copy testing assemblies.
    var testingDir = Directory("./src/Cake.Testing/bin") + Directory(configuration);
    CopyFileToDirectory(testingDir + File("Cake.Testing.dll"), binDir);
    CopyFileToDirectory(testingDir + File("Cake.Testing.pdb"), binDir);
    CopyFileToDirectory(testingDir + File("Cake.Testing.xml"), binDir);

    CopyFiles(new FilePath[] { "LICENSE", "README.md", "ReleaseNotes.md" }, binDir);
});

Task("Zip-Files")
    .IsDependentOn("Copy-Files")
    .WithCriteria(() => isRunningOnWindows)
    .Does(() =>
{
    var packageFile = File("Cake-bin-v" + semVersion + ".zip");
    var packagePath = buildResultDir + packageFile;

    var files = GetFiles(binDir.Path.FullPath + "/*")
      - GetFiles(binDir.Path.FullPath + "/*.Testing.*");

    Zip(binDir, packagePath, files);
});

Task("Create-NuGet-Packages")
    .IsDependentOn("Copy-Files")
    .WithCriteria(() => isRunningOnWindows)
    .Does(() =>
{
    // Create Cake package.
    NuGetPack("./nuspec/Cake.nuspec", new NuGetPackSettings {
        Version = semVersion,
        ReleaseNotes = releaseNotes.Notes.ToArray(),
        BasePath = binDir,
        OutputDirectory = nugetRoot,
        Symbols = false,
        NoPackageAnalysis = true
    });

    // Create Core package.
    NuGetPack("./nuspec/Cake.Core.nuspec", new NuGetPackSettings {
        Version = semVersion,
        ReleaseNotes = releaseNotes.Notes.ToArray(),
        BasePath = binDir,
        OutputDirectory = nugetRoot,
        Symbols = false
    });

    // Create Common package.
    NuGetPack("./nuspec/Cake.Common.nuspec", new NuGetPackSettings {
        Version = semVersion,
        ReleaseNotes = releaseNotes.Notes.ToArray(),
        BasePath = binDir,
        OutputDirectory = nugetRoot,
        Symbols = false
    });

    // Create Testing package.
    NuGetPack("./nuspec/Cake.Testing.nuspec", new NuGetPackSettings {
        Version = semVersion,
        ReleaseNotes = releaseNotes.Notes.ToArray(),
        BasePath = binDir,
        OutputDirectory = nugetRoot,
        Symbols = false
    });
});

Task("Update-AppVeyor-Build-Number")
    .WithCriteria(() => isRunningOnAppVeyor)
    .WithCriteria(() => isRunningOnWindows)
    .Does(() =>
{
    AppVeyor.UpdateBuildVersion(semVersion);
});

Task("Upload-AppVeyor-Artifacts")
    .IsDependentOn("Package")
    .WithCriteria(() => isRunningOnAppVeyor)
    .WithCriteria(() => isRunningOnWindows)
    .Does(() =>
{
    var artifact = buildResultDir + File("Cake-bin-v" + semVersion + ".zip");
    AppVeyor.UploadArtifact(artifact);
});

Task("Publish-MyGet")
    .WithCriteria(() => !local)
    .WithCriteria(() => !isPullRequest)
    .WithCriteria(() => isRunningOnWindows)
    .WithCriteria(() => isMainCakeRepo)
    .Does(() =>
{
    // Resolve the API key.
    var apiKey = EnvironmentVariable("MYGET_API_KEY");
    if(string.IsNullOrEmpty(apiKey)) {
        throw new InvalidOperationException("Could not resolve MyGet API key.");
    }

    foreach(var package in new[] { "Cake", "Cake.Core", "Cake.Common", "Cake.Testing" })
    {
        // Get the path to the package.
        var packagePath = nugetRoot + File(string.Concat(package, ".", semVersion, ".nupkg"));

        // Push the package.
        NuGetPush(packagePath, new NuGetPushSettings {
            Source = "https://www.myget.org/F/cake/api/v2/package",
            ApiKey = apiKey
        });
    }
});

Task("Publish-NuGet")
  .IsDependentOn("Create-NuGet-Packages")
  .WithCriteria(() => local)
  .Does(() =>
{
    // Resolve the API key.
    var apiKey = EnvironmentVariable("NUGET_API_KEY");
    if(string.IsNullOrEmpty(apiKey)) {
        throw new InvalidOperationException("Could not resolve NuGet API key.");
    }

    foreach(var package in new[] { "Cake", "Cake.Core", "Cake.Common", "Cake.Testing" })
    {
        // Get the path to the package.
        var packagePath = nugetRoot + File(string.Concat(package, ".", semVersion, ".nupkg"));

        // Push the package.
        NuGetPush(packagePath, new NuGetPushSettings {
          ApiKey = apiKey
        });
    }
});

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Package")
  .IsDependentOn("Zip-Files")
  .IsDependentOn("Create-NuGet-Packages");

Task("Default")
  .IsDependentOn("Package");

Task("Publish")
  .IsDependentOn("Publish-NuGet");

Task("AppVeyor")
  .IsDependentOn("Update-AppVeyor-Build-Number")
  .IsDependentOn("Upload-AppVeyor-Artifacts")
  .IsDependentOn("Publish-MyGet");

Task("Travis")
  .IsDependentOn("Build");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);
