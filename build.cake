// Install addins.
#addin "nuget:?package=Polly&version=4.2.0"

// Install tools.
#tool "nuget:?package=xunit.runner.console&version=2.1.0"
#tool "nuget:?package=gitreleasemanager&version=0.4.0"
#tool "nuget:?package=GitVersion.CommandLine&version=3.4.1"

// Load other scripts.
#load "./build/parameters.cake"

// Using statements
using Polly;

//////////////////////////////////////////////////////////////////////
// PARAMETERS
//////////////////////////////////////////////////////////////////////

BuildParameters parameters = BuildParameters.GetParameters(Context, BuildSystem);
bool publishingError = false;

///////////////////////////////////////////////////////////////////////////////
// SETUP / TEARDOWN
///////////////////////////////////////////////////////////////////////////////

Setup(context =>
{
    parameters.SetBuildVersion(
        BuildVersion.CalculatingSemanticVersion(
            context: Context,
            parameters: parameters
        )
    );

    parameters.SetBuildPaths(
        BuildPaths.GetPaths(
            context: Context,
            configuration: parameters.Configuration,
            semVersion: parameters.Version.SemVersion
        )
    );

    parameters.SetBuildPackages(
        BuildPackages.GetPackages(
        nugetRooPath: parameters.Paths.Directories.NugetRoot,
        semVersion: parameters.Version.SemVersion,
        packageIds: new [] { "Cake", "Cake.Core", "Cake.Common", "Cake.Testing" },
        chocolateyPackageIds: new [] { "cake.portable" }
        )
    );

    Information("Building version {0} of Cake ({1}, {2}) using version {3} of Cake. (IsTagged: {4})",
        parameters.Version.SemVersion,
        parameters.Configuration,
        parameters.Target,
        parameters.Version.CakeVersion,
        parameters.IsTagged);
});

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Clean")
    .Does(() => CleanDirectories(parameters.Paths.Directories.ToClean));

Task("Restore-NuGet-Packages")
    .IsDependentOn("Clean")
    .Does(() =>
{
    var maxRetryCount = 5;
    var toolTimeout = 1d;
    Policy
        .Handle<Exception>()
        .Retry(maxRetryCount, (exception, retryCount, context) => {
            if (retryCount == maxRetryCount)
            {
                throw exception;
            }
            else
            {
                Verbose("{0}", exception);
                toolTimeout+=0.5;
            }})
        .Execute(()=> {
            NuGetRestore("./src/Cake.sln", new NuGetRestoreSettings {
                Source = new List<string> {
                    "https://api.nuget.org/v3/index.json",
                    "https://www.myget.org/F/roslyn-nightly/api/v3/index.json"
                },
                ToolTimeout = TimeSpan.FromMinutes(toolTimeout)
            });
        });
});

Task("Build")
    .IsDependentOn("Restore-NuGet-Packages")
    .Does(() =>
{
    if(parameters.IsRunningOnUnix)
    {
        XBuild("./src/Cake.sln", new XBuildSettings()
            .SetConfiguration(parameters.Configuration)
            .WithProperty("POSIX", "True")
            .WithProperty("TreatWarningsAsErrors", "True")
            .SetVerbosity(Verbosity.Minimal)
        );
    }
    else
    {
        MSBuild("./src/Cake.sln", new MSBuildSettings()
            .SetConfiguration(parameters.Configuration)
            .WithProperty("Windows", "True")
            .WithProperty("TreatWarningsAsErrors", "True")
            .UseToolVersion(MSBuildToolVersion.NET45)
            .SetVerbosity(Verbosity.Minimal)
            .SetNodeReuse(false));
    }
});

Task("Run-Unit-Tests")
    .IsDependentOn("Build")
    .Does(() =>
{
    XUnit2("./src/**/bin/" + parameters.Configuration + "/*.Tests.dll", new XUnit2Settings {
        OutputDirectory = parameters.Paths.Directories.TestResults,
        XmlReportV1 = true,
        NoAppDomain = true
    });
});


Task("Copy-Files")
    .IsDependentOn("Run-Unit-Tests")
    .Does(() =>
{
    CopyFiles(
        parameters.Paths.Files.ArtifactsSourcePaths,
        parameters.Paths.Directories.ArtifactsBin
    );
});

Task("Zip-Files")
    .IsDependentOn("Copy-Files")
    .Does(() =>
{
    var files = GetFiles( parameters.Paths.Directories.ArtifactsBin.FullPath + "/*")
      - GetFiles(parameters.Paths.Directories.ArtifactsBin.FullPath + "/*.Testing.*");

    Zip(parameters.Paths.Directories.ArtifactsBin, parameters.Paths.Files.ZipArtifactPath, files);
});

Task("Create-Chocolatey-Packages")
    .IsDependentOn("Copy-Files")
    .IsDependentOn("Package")
    .WithCriteria(() => parameters.IsRunningOnWindows)
    .Does(() =>
{
    foreach(var package in parameters.Packages.Chocolatey)
    {
        // Create package.
        ChocolateyPack(package.NuspecPath, new ChocolateyPackSettings {
            Version = parameters.Version.SemVersion,
            ReleaseNotes = parameters.ReleaseNotes.Notes.ToArray(),
            OutputDirectory = parameters.Paths.Directories.NugetRoot,
            Files = parameters.Paths.ChocolateyFiles
        });
    }
});

Task("Create-NuGet-Packages")
    .IsDependentOn("Copy-Files")
    .Does(() =>
{
    foreach(var package in parameters.Packages.Nuget)
    {
        // Create package.
        NuGetPack(package.NuspecPath, new NuGetPackSettings {
            Version = parameters.Version.SemVersion,
            ReleaseNotes = parameters.ReleaseNotes.Notes.ToArray(),
            BasePath = parameters.Paths.Directories.ArtifactsBin,
            OutputDirectory = parameters.Paths.Directories.NugetRoot,
            Symbols = false,
            NoPackageAnalysis = true
        });
    }
});

Task("Upload-AppVeyor-Artifacts")
    .IsDependentOn("Create-Chocolatey-Packages")
    .WithCriteria(() => parameters.IsRunningOnAppVeyor)
    .Does(() =>
{
    AppVeyor.UploadArtifact(parameters.Paths.Files.ZipArtifactPath);

    foreach(var package in GetFiles(parameters.Paths.Directories.NugetRoot + "/*"))
    {
        AppVeyor.UploadArtifact(package);
    }
});

Task("Publish-MyGet")
    .IsDependentOn("Package")
    .WithCriteria(() => !parameters.IsLocalBuild)
    .WithCriteria(() => !parameters.IsPullRequest)
    .WithCriteria(() => parameters.IsMainCakeRepo)
    .WithCriteria(() => parameters.IsTagged || !parameters.IsMainCakeBranch)
    .Does(() =>
{
    // Resolve the API key.
    var apiKey = EnvironmentVariable("MYGET_API_KEY");
    if(string.IsNullOrEmpty(apiKey)) {
        throw new InvalidOperationException("Could not resolve MyGet API key.");
    }

    // Resolve the API url.
    var apiUrl = EnvironmentVariable("MYGET_API_URL");
    if(string.IsNullOrEmpty(apiUrl)) {
        throw new InvalidOperationException("Could not resolve MyGet API url.");
    }

    foreach(var package in parameters.Packages.All)
    {
        // Push the package.
        NuGetPush(package.PackagePath, new NuGetPushSettings {
            Source = apiUrl,
            ApiKey = apiKey
        });
    }
})
.OnError(exception =>
{
    Information("Publish-MyGet Task failed, but continuing with next Task...");
    publishingError = true;
});

Task("Publish-NuGet")
    .IsDependentOn("Create-NuGet-Packages")
    .WithCriteria(() => !parameters.IsLocalBuild)
    .WithCriteria(() => !parameters.IsPullRequest)
    .WithCriteria(() => parameters.IsMainCakeRepo)
    .WithCriteria(() => parameters.IsMainCakeBranch)
    .WithCriteria(() => parameters.IsTagged)
    .Does(() =>
{
    // Resolve the API key.
    var apiKey = EnvironmentVariable("NUGET_API_KEY");
    if(string.IsNullOrEmpty(apiKey)) {
        throw new InvalidOperationException("Could not resolve NuGet API key.");
    }

    // Resolve the API url.
    var apiUrl = EnvironmentVariable("NUGET_API_URL");
    if(string.IsNullOrEmpty(apiUrl)) {
        throw new InvalidOperationException("Could not resolve NuGet API url.");
    }

    foreach(var package in parameters.Packages.Nuget)
    {
        // Push the package.
        NuGetPush(package.PackagePath, new NuGetPushSettings {
          ApiKey = apiKey,
          Source = apiUrl
        });
    }
})
.OnError(exception =>
{
    Information("Publish-NuGet Task failed, but continuing with next Task...");
    publishingError = true;
});

Task("Publish-Chocolatey")
    .IsDependentOn("Create-Chocolatey-Packages")
    .WithCriteria(() => !parameters.IsLocalBuild)
    .WithCriteria(() => !parameters.IsPullRequest)
    .WithCriteria(() => parameters.IsMainCakeRepo)
    .WithCriteria(() => parameters.IsMainCakeBranch)
    .WithCriteria(() => parameters.IsTagged)
    .Does(() =>
{
    // Resolve the API key.
    var apiKey = EnvironmentVariable("CHOCOLATEY_API_KEY");
    if(string.IsNullOrEmpty(apiKey)) {
        throw new InvalidOperationException("Could not resolve Chocolatey API key.");
    }

    // Resolve the API url.
    var apiUrl = EnvironmentVariable("CHOCOLATEY_API_URL");
    if(string.IsNullOrEmpty(apiUrl)) {
        throw new InvalidOperationException("Could not resolve Chocolatey API url.");
    }

    foreach(var package in parameters.Packages.Chocolatey)
    {
        // Push the package.
        ChocolateyPush(package.PackagePath, new ChocolateyPushSettings {
          ApiKey = apiKey,
          Source = apiUrl
        });
    }
})
.OnError(exception =>
{
    Information("Publish-Chocolatey Task failed, but continuing with next Task...");
    publishingError = true;
});

Task("Publish-HomeBrew")
    .WithCriteria(() => !parameters.IsLocalBuild)
    .WithCriteria(() => !parameters.IsPullRequest)
    .WithCriteria(() => parameters.IsMainCakeRepo)
    .WithCriteria(() => parameters.IsMainCakeBranch)
    .WithCriteria(() => parameters.IsTagged)
    .IsDependentOn("Zip-Files")
	.Does(() =>
{
    var hash = CalculateFileHash(parameters.Paths.Files.ZipArtifactPath).ToHex();

    Information("Hash for creating HomeBrew PullRequest: {0}", hash);
})
.OnError(exception =>
{
    Information("Publish-HomeBrew Task failed, but continuing with next Task...");
    publishingError = true;
});

Task("Publish-GitHub-Release")
    .WithCriteria(() => !parameters.IsLocalBuild)
    .WithCriteria(() => !parameters.IsPullRequest)
    .WithCriteria(() => parameters.IsMainCakeRepo)
    .WithCriteria(() => parameters.IsMainCakeBranch)
    .WithCriteria(() => parameters.IsTagged)
    .Does(() =>
{
    GitReleaseManagerAddAssets(parameters.GitHub.UserName, parameters.GitHub.Password, "cake-build", "cake", parameters.Version.Milestone, parameters.Paths.Files.ZipArtifactPath.ToString());

    GitReleaseManagerClose(parameters.GitHub.UserName, parameters.GitHub.Password, "cake-build", "cake", parameters.Version.Milestone);
})
.OnError(exception =>
{
    Information("Publish-GitHub-Release Task failed, but continuing with next Task...");
    publishingError = true;
});

Task("Create-Release-Notes")
    .Does(() =>
{
    GitReleaseManagerCreate(parameters.GitHub.UserName, parameters.GitHub.Password, "cake-build", "cake", new GitReleaseManagerCreateSettings {
        Milestone         = parameters.Version.Milestone,
        Name              = parameters.Version.Milestone,
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
  .IsDependentOn("Package");

Task("AppVeyor")
  .IsDependentOn("Upload-AppVeyor-Artifacts")
  .IsDependentOn("Publish-MyGet")
  .IsDependentOn("Publish-NuGet")
  .IsDependentOn("Publish-Chocolatey")
  .IsDependentOn("Publish-HomeBrew")
  .IsDependentOn("Publish-GitHub-Release")
  .Finally(() =>
{
    if(publishingError)
    {
        throw new Exception("An error occurred during the publishing of Cake.  All publishing tasks have been attempted.");
    }
});

Task("Travis")
  .IsDependentOn("Run-Unit-Tests");

Task("ReleaseNotes")
  .IsDependentOn("Create-Release-Notes");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(parameters.Target);