//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");
var output = Argument("output", string.Empty);

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

// Credentials
var userName = EnvironmentVariable("CAKE_GITHUB_USERNAME");
var password = EnvironmentVariable("CAKE_GITHUB_PASSWORD");

// Get version.
var buildNumber = AppVeyor.Environment.Build.Number;
GitVersion assertedVersions        = null;
var version = string.Empty;
var semVersion = string.Empty;
var milestone = string.Empty;

// Define directories.
var buildDir = Directory("./src/Cake/bin") + Directory(configuration);
var buildResultDir = Directory("./build") + Directory("v" + semVersion);

if(!string.IsNullOrWhiteSpace(output)) {
  buildResultDir = Directory(output);
}

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

Task("Run-GitVersion")
    .IsDependentOn("Restore-NuGet-Packages")
    .IsDependentOn("Run-GitVersion-AppVeyor")
    .IsDependentOn("Run-GitVersion-Local");

Task("Run-GitVersion-AppVeyor")
    .WithCriteria(AppVeyor.IsRunningOnAppVeyor)
    .Does(() =>
{
    GitVersion(new GitVersionSettings {
        UpdateAssemblyInfoFilePath = "./src/SolutionInfo.cs",
        UpdateAssemblyInfo = true,
        OutputType = GitVersionOutput.BuildServer
    });

    version = EnvironmentVariable("GitVersion_MajorMinorPatch");
    semVersion = EnvironmentVariable("GitVersion_LegacySemVerPadded");
    milestone = string.Concat("v", version);

    // Due to the way that GitVersion is executed on AppVeyor, the Environment Variables although populated
    // are not accessible yet, so have to run GitVersion again, using OutputType of JSON
    if(string.IsNullOrEmpty(semVersion))
    {
        assertedVersions = GitVersion(new GitVersionSettings {
            OutputType = GitVersionOutput.Json,
        });

        version = assertedVersions.MajorMinorPatch;
        semVersion = assertedVersions.LegacySemVerPadded;
        milestone = string.Concat("v", version);
    }

    Information("Calculated Semantic Version: {0}", semVersion);
});

Task("Run-GitVersion-Local")
    .WithCriteria(!AppVeyor.IsRunningOnAppVeyor)
    .WithCriteria(isRunningOnWindows)
    .Does(() =>
{
    assertedVersions = GitVersion(new GitVersionSettings {
        OutputType = GitVersionOutput.Json,
    });

    version = assertedVersions.MajorMinorPatch;
    semVersion = assertedVersions.LegacySemVerPadded;
    milestone = string.Concat("v", version);

    Information("Calculated Semantic Version: {0}", semVersion);
});

Task("Build")
    .IsDependentOn("Run-GitVersion")
    .Does(() =>
{
    if(isRunningOnUnix)
    {
        XBuild("./src/Cake.sln", new XBuildSettings()
            .SetConfiguration(configuration)
            .WithProperty("POSIX", "True")
            .WithProperty("TreatWarningsAsErrors", "True")
            .SetVerbosity(Verbosity.Minimal)
        );
    }
    else
    {
        MSBuild("./src/Cake.sln", new MSBuildSettings()
            .SetConfiguration(configuration)
            .WithProperty("Windows", "True")
            .WithProperty("TreatWarningsAsErrors", "True")
            .UseToolVersion(MSBuildToolVersion.NET45)
            .SetVerbosity(Verbosity.Minimal)
            .SetNodeReuse(false));
    }
});

Task("Copy-Files")
    .IsDependentOn("Build")
    .Does(() =>
{
    CopyFileToDirectory(buildDir + File("Cake.exe"), binDir);
    CopyFileToDirectory(buildDir + File("Cake.Core.dll"), binDir);
    CopyFileToDirectory(buildDir + File("Cake.Core.pdb"), binDir);
    CopyFileToDirectory(buildDir + File("Cake.Core.xml"), binDir);
    CopyFileToDirectory(buildDir + File("Cake.NuGet.dll"), binDir);
    CopyFileToDirectory(buildDir + File("Cake.NuGet.pdb"), binDir);
    CopyFileToDirectory(buildDir + File("Cake.NuGet.xml"), binDir);
    CopyFileToDirectory(buildDir + File("Cake.Common.dll"), binDir);
    CopyFileToDirectory(buildDir + File("Cake.Common.pdb"), binDir);
    CopyFileToDirectory(buildDir + File("Cake.Common.xml"), binDir);
    CopyFileToDirectory(buildDir + File("Mono.CSharp.dll"), binDir);
    CopyFileToDirectory(buildDir + File("Autofac.dll"), binDir);
    CopyFileToDirectory(buildDir + File("NuGet.Core.dll"), binDir);

    // Copy testing assemblies.
    var testingDir = Directory("./src/Cake.Testing/bin") + Directory(configuration);
    CopyFileToDirectory(testingDir + File("Cake.Testing.dll"), binDir);
    CopyFileToDirectory(testingDir + File("Cake.Testing.pdb"), binDir);
    CopyFileToDirectory(testingDir + File("Cake.Testing.xml"), binDir);

    CopyFiles(new FilePath[] { "LICENSE", "README.md", "ReleaseNotes.md" }, binDir);
});

Task("Run-Unit-Tests")
    .IsDependentOn("Build")
    .Does(() =>
{
    XUnit2("./src/**/bin/" + configuration + "/*.Tests.dll", new XUnit2Settings {
        OutputDirectory = testResultsDir,
        XmlReportV1 = true,
        NoAppDomain = true
    });
});


Task("Zip-Files")
    .IsDependentOn("Copy-Files")
    .IsDependentOn("Run-GitVersion")
    .Does(() =>
{
    var packageFile = File("Cake-bin-v" + semVersion + ".zip");
    var packagePath = buildResultDir + packageFile;

    var files = GetFiles(binDir.Path.FullPath + "/*")
      - GetFiles(binDir.Path.FullPath + "/*.Testing.*");

    Zip(binDir, packagePath, files);
});

Task("Create-Chocolatey-Packages")
    .IsDependentOn("Copy-Files")
    .IsDependentOn("Run-GitVersion")
    .IsDependentOn("Package")
    .WithCriteria(() => isRunningOnWindows)
    .Does(() =>
{
    // Create Cake package.
    ChocolateyPack("./nuspec/Cake.Portable.nuspec", new ChocolateyPackSettings {
        Version = semVersion,
        ReleaseNotes = releaseNotes.Notes.ToArray(),
        OutputDirectory = nugetRoot,
        Files = new [] {
            new ChocolateyNuSpecContent {Source = string.Concat("./../", binDir.Path.FullPath, "/Cake.exe")},
            new ChocolateyNuSpecContent {Source = string.Concat("./../", binDir.Path.FullPath, "/Cake.Core.dll")},
            new ChocolateyNuSpecContent {Source = string.Concat("./../", binDir.Path.FullPath, "/Cake.Core.xml")},
            new ChocolateyNuSpecContent {Source = string.Concat("./../", binDir.Path.FullPath, "/Cake.NuGet.dll")},
            new ChocolateyNuSpecContent {Source = string.Concat("./../", binDir.Path.FullPath, "/Cake.NuGet.xml")},
            new ChocolateyNuSpecContent {Source = string.Concat("./../", binDir.Path.FullPath, "/Cake.Common.dll")},
            new ChocolateyNuSpecContent {Source = string.Concat("./../", binDir.Path.FullPath, "/Cake.Common.xml")},
            new ChocolateyNuSpecContent {Source = string.Concat("./../", binDir.Path.FullPath, "/NuGet.Core.dll")},
            new ChocolateyNuSpecContent {Source = string.Concat("./../", binDir.Path.FullPath, "/Mono.CSharp.dll")},
            new ChocolateyNuSpecContent {Source = string.Concat("./../", binDir.Path.FullPath, "/Autofac.dll")},
            new ChocolateyNuSpecContent {Source = string.Concat("./../", binDir.Path.FullPath, "/LICENSE")}
        }
    });
});

Task("Create-NuGet-Packages")
    .IsDependentOn("Copy-Files")
    .IsDependentOn("Run-GitVersion")
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
    .IsDependentOn("Run-GitVersion")
    .Does(() =>
{
    AppVeyor.UpdateBuildVersion(semVersion);
});

Task("Upload-AppVeyor-Artifacts")
    .IsDependentOn("Create-Chocolatey-Packages")
    .IsDependentOn("Run-GitVersion")
    .WithCriteria(() => isRunningOnAppVeyor)
    .Does(() =>
{
    var artifact = buildResultDir + File("Cake-bin-v" + semVersion + ".zip");
    AppVeyor.UploadArtifact(artifact);
});

Task("Publish-MyGet")
    .IsDependentOn("Run-GitVersion")
    .WithCriteria(() => !local)
    .WithCriteria(() => !isPullRequest)
    .WithCriteria(() => isMainCakeRepo)
    .Does(() =>
{
    // Resolve the API key.
    var apiKey = EnvironmentVariable("MYGET_API_KEY");
    if(string.IsNullOrEmpty(apiKey)) {
        throw new InvalidOperationException("Could not resolve MyGet API key.");
    }

    foreach(var package in new[] { "Cake", "Cake.Core", "Cake.Common", "Cake.Testing", "cake.portable" })
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
    .IsDependentOn("Run-GitVersion")
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

Task("Publish-Chocolatey")
    .IsDependentOn("Run-GitVersion")
    .IsDependentOn("Create-Chocolatey-Packages")
    .WithCriteria(() => local)
    .Does(() =>
{
    // Resolve the API key.
    var apiKey = EnvironmentVariable("CHOCOLATEY_API_KEY");
    if(string.IsNullOrEmpty(apiKey)) {
        throw new InvalidOperationException("Could not resolve Chocolatey API key.");
    }

    foreach(var package in new[] { "cake.portable" })
    {
        // Get the path to the package.
        var packagePath = nugetRoot + File(string.Concat(package, ".", semVersion, ".nupkg"));

        // Push the package.
        ChocolateyPush(packagePath, new ChocolateyPushSettings {
          ApiKey = apiKey
        });
    }
});

Task("Publish-HomeBrew")
    .IsDependentOn("Run-GitVersion")
    .IsDependentOn("Zip-Files")
	.Does(() =>
{
    var packageFile = File("Cake-bin-v" + semVersion + ".zip");
    var packagePath = buildResultDir + packageFile;

    var hash = CalculateFileHash(packagePath).ToHex();

    Information("Hash for creating HomeBrew PullRequest: {0}", hash);
});

Task("Publish-GitHub-Release")
    .IsDependentOn("Run-GitVersion")
    .Does(() =>
{
    var packageFile = File("Cake-bin-v" + semVersion + ".zip");
    var packagePath = buildResultDir + packageFile;

    GitReleaseManagerAddAssets(userName, password, "cake-build", "cake", milestone, packagePath.ToString());

    GitReleaseManagerPublish(userName, password, "cake-build", "cake", milestone);

    GitReleaseManagerClose(userName, password, "cake-build", "cake", milestone);
});

Task("Create-Release-Notes")
    .IsDependentOn("Run-GitVersion")
    .Does(() =>
{
    GitReleaseManagerCreate(userName, password, "cake-build", "cake", new GitReleaseManagerCreateSettings {
        Milestone         = milestone,
        Name              = milestone,
        Prerelease        = true,
        TargetCommitish   = "main"
    });
});

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Package")
  .IsDependentOn("Zip-Files")
  .IsDependentOn("Create-NuGet-Packages");

Task("Default")
  .IsDependentOn("Run-Unit-Tests")
  .IsDependentOn("Package");

Task("Publish")
  .IsDependentOn("Run-Unit-Tests")
  .IsDependentOn("Publish-NuGet")
  .IsDependentOn("Publish-Chocolatey")
  .IsDependentOn("Publish-HomeBrew")
  .IsDependentOn("Publish-GitHub-Release");

Task("AppVeyor")
  .IsDependentOn("Run-Unit-Tests")
  .IsDependentOn("Update-AppVeyor-Build-Number")
  .IsDependentOn("Upload-AppVeyor-Artifacts")
  .IsDependentOn("Publish-MyGet");

Task("Travis")
  .IsDependentOn("Run-Unit-Tests");

Task("ReleaseNotes")
  .IsDependentOn("Create-Release-Notes");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);
