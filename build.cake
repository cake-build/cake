// Install addins.
#addin "nuget:https://www.nuget.org/api/v2?package=Newtonsoft.Json&version=9.0.1"
#addin "nuget:https://www.nuget.org/api/v2?package=Cake.Coveralls&version=0.2.0"

// Install tools.
#tool "nuget:https://www.nuget.org/api/v2?package=gitreleasemanager&version=0.5.0"
#tool "nuget:https://www.nuget.org/api/v2?package=GitVersion.CommandLine&version=3.6.2"
#tool "nuget:https://www.nuget.org/api/v2?package=coveralls.io&version=1.3.4"
#tool "nuget:https://www.nuget.org/api/v2?package=OpenCover&version=4.6.519"
#tool "nuget:https://www.nuget.org/api/v2?package=ReportGenerator&version=2.4.5"

// Load other scripts.
#load "./build/parameters.cake"

//////////////////////////////////////////////////////////////////////
// PARAMETERS
//////////////////////////////////////////////////////////////////////

BuildParameters parameters = BuildParameters.GetParameters(Context);
bool publishingError = false;

///////////////////////////////////////////////////////////////////////////////
// SETUP / TEARDOWN
///////////////////////////////////////////////////////////////////////////////

Setup(context =>
{
    parameters.Initialize(context);

    // Increase verbosity?
    if(parameters.IsMainCakeBranch && (context.Log.Verbosity != Verbosity.Diagnostic)) {
        Information("Increasing verbosity to diagnostic.");
        context.Log.Verbosity = Verbosity.Diagnostic;
    }

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
    .Does(() =>
{
    CleanDirectories(parameters.Paths.Directories.ToClean);
});

Task("Patch-Project-Json")
    .IsDependentOn("Clean")
    .Does(() =>
{
    var projects = GetFiles("./src/**/project.json");
    foreach(var project in projects)
    {
        if(!parameters.Version.PatchProjectJson(project)) {
            Warning("No version specified in {0}.", project.FullPath);
        }
    }
});

Task("Restore-NuGet-Packages")
    .IsDependentOn("Clean")
    .Does(() =>
{
    DotNetCoreRestore("./", new DotNetCoreRestoreSettings
    {
        Verbose = false,
        Verbosity = DotNetCoreRestoreVerbosity.Warning,
        Sources = new [] {
            "https://www.myget.org/F/xunit/api/v3/index.json",
            "https://dotnet.myget.org/F/dotnet-core/api/v3/index.json",
            "https://dotnet.myget.org/F/cli-deps/api/v3/index.json",
            "https://api.nuget.org/v3/index.json",
        }
    });
});

Task("Build")
    .IsDependentOn("Patch-Project-Json")
    .IsDependentOn("Restore-NuGet-Packages")
    .Does(() =>
{
    var projects = GetFiles("./**/*.xproj");
    foreach(var project in projects)
    {
        DotNetCoreBuild(project.GetDirectory().FullPath, new DotNetCoreBuildSettings {
            VersionSuffix = parameters.Version.DotNetAsterix,
            Configuration = parameters.Configuration
        });
    }
});

Task("Run-Unit-Tests")
    .IsDependentOn("Build")
    .Does(() =>
{
    var projects = GetFiles("./src/**/*.Tests.xproj");
    foreach(var project in projects)
    {
        if(IsRunningOnWindows())
        {
            var apiUrl = EnvironmentVariable("APPVEYOR_API_URL");
            try
            {
                if (!string.IsNullOrEmpty(apiUrl))
                {
                    // Disable XUnit AppVeyorReporter see https://github.com/cake-build/cake/issues/1200
                    System.Environment.SetEnvironmentVariable("APPVEYOR_API_URL", null);
                }
                OpenCover(tool => {
                    tool.DotNetCoreTest(project.GetDirectory().FullPath, new DotNetCoreTestSettings {
                        Configuration = parameters.Configuration,
                        NoBuild = true,
                        Verbose = false,
                        ArgumentCustomization = args =>
                            args.Append("-xml").Append(parameters.Paths.Directories.TestResults.CombineWithFilePath(project.GetFilenameWithoutExtension()).FullPath + ".xml")
                    });
                },
                parameters.Paths.Files.TestCoverageOutputFilePath,
                new OpenCoverSettings {
                    ReturnTargetCodeOffset = 0,
                    ArgumentCustomization = args => args.Append("-mergeoutput")
                }
                .WithFilter("+[*]* -[xunit.*]* -[*.Tests]* -[Cake.Testing]* -[Cake.Testing.Xunit]* ")
                .ExcludeByAttribute("*.ExcludeFromCodeCoverage*")
                .ExcludeByFile("*/*Designer.cs;*/*.g.cs;*/*.g.i.cs"));
            }
            finally
            {
                if (!string.IsNullOrEmpty(apiUrl))
                {
                    System.Environment.SetEnvironmentVariable("APPVEYOR_API_URL", apiUrl);
                }
            }
        }
        else
        {
            var name = project.GetFilenameWithoutExtension();
            var dirPath = project.GetDirectory().FullPath;
            var config = parameters.Configuration;
            var xunit = GetFiles(dirPath + "/bin/" + config + "/net451/*/dotnet-test-xunit.exe").First().FullPath;
            var testfile = GetFiles(dirPath + "/bin/" + config + "/net451/*/" + name + ".dll").First().FullPath;

            using(var process = StartAndReturnProcess("mono", new ProcessSettings{ Arguments = xunit + " " + testfile }))
            {
                process.WaitForExit();
                if (process.GetExitCode() != 0)
                {
                    throw new Exception("Mono tests failed!");
                }
            }
        }
    }

    // Generate the HTML version of the Code Coverage report if the XML file exists
    if(FileExists(parameters.Paths.Files.TestCoverageOutputFilePath))
    {
        ReportGenerator(parameters.Paths.Files.TestCoverageOutputFilePath, parameters.Paths.Directories.TestResults);
    }
});

Task("Copy-Files")
    .IsDependentOn("Run-Unit-Tests")
    .Does(() =>
{
    // .NET 4.5
    DotNetCorePublish("./src/Cake", new DotNetCorePublishSettings
    {
        Framework = "net45",
        VersionSuffix = parameters.Version.DotNetAsterix,
        Configuration = parameters.Configuration,
        OutputDirectory = parameters.Paths.Directories.ArtifactsBinNet45,
        NoBuild = true,
        Verbose = false
    });

    // .NET Core
    DotNetCorePublish("./src/Cake", new DotNetCorePublishSettings
    {
        Framework = "netcoreapp1.0",
        Configuration = parameters.Configuration,
        VersionSuffix = "alpha",
        OutputDirectory = parameters.Paths.Directories.ArtifactsBinNetCoreApp10,
        NoBuild = true,
        Verbose = false
    });

    // Copy license
    CopyFileToDirectory("./LICENSE", parameters.Paths.Directories.ArtifactsBinNet45);
    CopyFileToDirectory("./LICENSE", parameters.Paths.Directories.ArtifactsBinNetCoreApp10);
});

Task("Zip-Files")
    .IsDependentOn("Copy-Files")
    .Does(() =>
{
    // .NET 4.5
    var homebrewFiles = GetFiles( parameters.Paths.Directories.ArtifactsBinNet45.FullPath + "/**/*");
    Zip(parameters.Paths.Directories.ArtifactsBinNet45, parameters.Paths.Files.ZipArtifactPathDesktop, homebrewFiles);

    // .NET Core
    var coreclrFiles = GetFiles( parameters.Paths.Directories.ArtifactsBinNetCoreApp10.FullPath + "/**/*");
    Zip(parameters.Paths.Directories.ArtifactsBinNetCoreApp10, parameters.Paths.Files.ZipArtifactPathCoreClr, coreclrFiles);
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
    // Build libraries
    var projects = GetFiles("./**/*.xproj");
    foreach(var project in projects)
    {
        var name = project.GetDirectory().FullPath;
        if(name.EndsWith("Cake") || name.EndsWith("Tests")
            || name.EndsWith("Xunit") || name.EndsWith("NuGet"))
        {
            continue;
        }

        DotNetCorePack(project.GetDirectory().FullPath, new DotNetCorePackSettings {
            VersionSuffix = parameters.Version.DotNetAsterix,
            Configuration = parameters.Configuration,
            OutputDirectory = parameters.Paths.Directories.NugetRoot,
            NoBuild = true,
            Verbose = false
        });
    }

    // Cake - .NET 4.5
    NuGetPack("./nuspec/Cake.nuspec", new NuGetPackSettings {
        Version = parameters.Version.SemVersion,
        ReleaseNotes = parameters.ReleaseNotes.Notes.ToArray(),
        BasePath = parameters.Paths.Directories.ArtifactsBinNet45,
        OutputDirectory = parameters.Paths.Directories.NugetRoot,
        Symbols = false,
        NoPackageAnalysis = true
    });

    // Cake - .NET Core
    NuGetPack("./nuspec/Cake.CoreCLR.nuspec", new NuGetPackSettings {
        Version = parameters.Version.SemVersion,
        ReleaseNotes = parameters.ReleaseNotes.Notes.ToArray(),
        BasePath = parameters.Paths.Directories.ArtifactsBinNetCoreApp10,
        OutputDirectory = parameters.Paths.Directories.NugetRoot,
        Symbols = false,
        NoPackageAnalysis = true
    });
});

Task("Upload-AppVeyor-Artifacts")
    .IsDependentOn("Create-Chocolatey-Packages")
    .WithCriteria(() => parameters.IsRunningOnAppVeyor)
    .Does(() =>
{
    AppVeyor.UploadArtifact(parameters.Paths.Files.ZipArtifactPathDesktop);
    AppVeyor.UploadArtifact(parameters.Paths.Files.ZipArtifactPathCoreClr);
    foreach(var package in GetFiles(parameters.Paths.Directories.NugetRoot + "/*"))
    {
        AppVeyor.UploadArtifact(package);
    }
});

Task("Upload-Coverage-Report")
    .WithCriteria(() => FileExists(parameters.Paths.Files.TestCoverageOutputFilePath))
    .WithCriteria(() => !parameters.IsLocalBuild)
    .WithCriteria(() => !parameters.IsPullRequest)
    .WithCriteria(() => parameters.IsMainCakeRepo)
    .IsDependentOn("Run-Unit-Tests")
    .Does(() =>
{
    CoverallsIo(parameters.Paths.Files.TestCoverageOutputFilePath, new CoverallsIoSettings()
    {
        RepoToken = parameters.Coveralls.RepoToken
    });
});

Task("Publish-MyGet")
    .IsDependentOn("Package")
    .WithCriteria(() => parameters.ShouldPublishToMyGet)
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
    .WithCriteria(() => parameters.ShouldPublish)
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
    .WithCriteria(() => parameters.ShouldPublish)
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
    .WithCriteria(() => parameters.ShouldPublish)
    .IsDependentOn("Zip-Files")
	.Does(() =>
{
    var hash = CalculateFileHash(parameters.Paths.Files.ZipArtifactPathDesktop).ToHex();
    Information("Hash for creating HomeBrew PullRequest: {0}", hash);
})
.OnError(exception =>
{
    Information("Publish-HomeBrew Task failed, but continuing with next Task...");
    publishingError = true;
});

Task("Publish-GitHub-Release")
    .WithCriteria(() => parameters.ShouldPublish)
    .Does(() =>
{
    GitReleaseManagerAddAssets(parameters.GitHub.UserName, parameters.GitHub.Password, "cake-build", "cake", parameters.Version.Milestone, parameters.Paths.Files.ZipArtifactPathDesktop.ToString());
    GitReleaseManagerAddAssets(parameters.GitHub.UserName, parameters.GitHub.Password, "cake-build", "cake", parameters.Version.Milestone, parameters.Paths.Files.ZipArtifactPathCoreClr.ToString());
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
  .IsDependentOn("Upload-Coverage-Report")
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
