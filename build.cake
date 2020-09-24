// Install modules
#module nuget:?package=Cake.DotNetTool.Module&version=0.4.0

// Install addins.
#addin "nuget:https://api.nuget.org/v3/index.json?package=Cake.Coveralls&version=0.10.1"
#addin "nuget:https://api.nuget.org/v3/index.json?package=Cake.Twitter&version=0.10.1"
#addin "nuget:https://api.nuget.org/v3/index.json?package=Cake.Gitter&version=0.11.1"

// Install tools.
#tool "nuget:https://api.nuget.org/v3/index.json?package=coveralls.io&version=1.4.2"
#tool "nuget:https://api.nuget.org/v3/index.json?package=OpenCover&version=4.7.922"
#tool "nuget:https://api.nuget.org/v3/index.json?package=ReportGenerator&version=4.5.8"
#tool "nuget:https://api.nuget.org/v3/index.json?package=nuget.commandline&version=5.5.1"

// Install .NET Core Global tools.
#tool "dotnet:https://api.nuget.org/v3/index.json?package=GitVersion.Tool&version=5.1.2"
#tool "dotnet:https://api.nuget.org/v3/index.json?package=SignClient&version=1.2.109"
#tool "dotnet:https://api.nuget.org/v3/index.json?package=GitReleaseManager.Tool&version=0.11.0"

// Load other scripts.
#load "./build/parameters.cake"

///////////////////////////////////////////////////////////////////////////////
// SETUP / TEARDOWN
///////////////////////////////////////////////////////////////////////////////
Setup<BuildParameters>(context =>
{
    var parameters = new BuildParameters(context);

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

    foreach(var assemblyInfo in GetFiles("./src/**/AssemblyInfo.cs"))
    {
        CreateAssemblyInfo(
            assemblyInfo.ChangeExtension(".Generated.cs"),
            new AssemblyInfoSettings { Description = parameters.Version.SemVersion });
    }

    if(!parameters.IsRunningOnWindows)
    {
        var frameworkPathOverride = context.Environment.Runtime.IsCoreClr
                                        ?   new []{
                                                new DirectoryPath("/Library/Frameworks/Mono.framework/Versions/Current/lib/mono"),
                                                new DirectoryPath("/usr/lib/mono"),
                                                new DirectoryPath("/usr/local/lib/mono")
                                            }
                                            .Select(directory =>directory.Combine("4.5"))
                                            .FirstOrDefault(directory => context.DirectoryExists(directory))
                                            ?.FullPath + "/"
                                        : new FilePath(typeof(object).Assembly.Location).GetDirectory().FullPath + "/";

        // Use FrameworkPathOverride when not running on Windows.
        Information("Build will use FrameworkPathOverride={0} since not building on Windows.", frameworkPathOverride);
        parameters.MSBuildSettings.WithProperty("FrameworkPathOverride", frameworkPathOverride);
    }

    return parameters;
});

Teardown<BuildParameters>((context, parameters) =>
{
    Information("Starting Teardown...");

    if(context.Successful)
    {
        if(parameters.ShouldPublish)
        {
            if(parameters.CanPostToTwitter)
            {
                var message = "Version " + parameters.Version.SemVersion + " of Cake has just been released, https://www.nuget.org/packages/Cake.";

                TwitterSendTweet(parameters.Twitter.ConsumerKey, parameters.Twitter.ConsumerSecret, parameters.Twitter.AccessToken, parameters.Twitter.AccessTokenSecret, message);
            }

            if(parameters.CanPostToGitter)
            {
                var message = "@/all Version " + parameters.Version.SemVersion + " of the Cake has just been released, https://www.nuget.org/packages/Cake.";

                var postMessageResult = Gitter.Chat.PostMessage(
                    message: message,
                    messageSettings: new GitterChatMessageSettings { Token = parameters.Gitter.Token, RoomId = parameters.Gitter.RoomId}
                );

                if (postMessageResult.Ok)
                {
                    Information("Message {0} succcessfully sent", postMessageResult.TimeStamp);
                }
                else
                {
                    Error("Failed to send message: {0}", postMessageResult.Error);
                }
            }
        }
    }

    Information("Finished running tasks.");
});

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Clean")
    .Does<BuildParameters>((context, parameters) =>
{
    CleanDirectories("./src/**/bin/" + parameters.Configuration);
    CleanDirectories("./src/**/obj");
    CleanDirectories(parameters.Paths.Directories.ToClean);
});

Task("Restore-NuGet-Packages")
    .IsDependentOn("Clean")
    .Does<BuildParameters>((context, parameters) =>
{
    DotNetCoreRestore("./src/Cake.sln", new DotNetCoreRestoreSettings
    {
        Verbosity = DotNetCoreVerbosity.Minimal,
        Sources = new [] { "https://api.nuget.org/v3/index.json" },
        MSBuildSettings = parameters.MSBuildSettings
    });
});

Task("Build")
    .IsDependentOn("Restore-NuGet-Packages")
    .Does<BuildParameters>((context, parameters) =>
{
    // Build the solution.
    var path = MakeAbsolute(new DirectoryPath("./src/Cake.sln"));
    DotNetCoreBuild(path.FullPath, new DotNetCoreBuildSettings()
    {
        Configuration = parameters.Configuration,
        NoRestore = true,
        MSBuildSettings = parameters.MSBuildSettings
    });
});

Task("Run-Unit-Tests")
    .IsDependentOn("Build")
    .DoesForEach<BuildParameters, FilePath>(
        () => GetFiles("./src/**/*.Tests.csproj"),
        (parameters, project, context) =>
{
    foreach(var framework in new[] { "netcoreapp2.0", "netcoreapp3.0", "net461", "net5.0" })
    {
        FilePath testResultsPath = MakeAbsolute(parameters.Paths.Directories.TestResults
                                    .CombineWithFilePath($"{project.GetFilenameWithoutExtension()}_{framework}_TestResults.xml"));

        DotNetCoreTest(project.FullPath, new DotNetCoreTestSettings
        {
            Framework = framework,
            NoBuild = true,
            NoRestore = true,
            Configuration = parameters.Configuration,
            ArgumentCustomization = args=>args.Append($"--logger trx;LogFileName=\"{testResultsPath}\"")
        });
    }
});

Task("Copy-Files")
    .IsDependentOn("Run-Unit-Tests")
    .Does<BuildParameters>((context, parameters) =>
{
    // .NET 4.6
    DotNetCorePublish("./src/Cake/Cake.csproj", new DotNetCorePublishSettings
    {
        Framework = "net461",
        NoRestore = true,
        VersionSuffix = parameters.Version.DotNetAsterix,
        Configuration = parameters.Configuration,
        OutputDirectory = parameters.Paths.Directories.ArtifactsBinFullFx,
        MSBuildSettings = parameters.MSBuildSettings
    });

    // .NET Core
    DotNetCorePublish("./src/Cake/Cake.csproj", new DotNetCorePublishSettings
    {
        Framework = "netcoreapp2.0",
        NoRestore = true,
        Configuration = parameters.Configuration,
        OutputDirectory = parameters.Paths.Directories.ArtifactsBinNetCore,
        MSBuildSettings = parameters.MSBuildSettings
    });

    // Copy license
    CopyFileToDirectory("./LICENSE", parameters.Paths.Directories.ArtifactsBinFullFx);
    CopyFileToDirectory("./LICENSE", parameters.Paths.Directories.ArtifactsBinNetCore);

    // Copy icon
    CopyFileToDirectory("./nuspec/cake-medium.png", parameters.Paths.Directories.ArtifactsBinFullFx);
    CopyFileToDirectory("./nuspec/cake-medium.png", parameters.Paths.Directories.ArtifactsBinNetCore);
});

Task("Validate-Version")
    .IsDependentOn("Copy-Files")
    .Does<BuildParameters>((context, parameters) =>
{
    var fullFxExe = MakeAbsolute(parameters.Paths.Directories.ArtifactsBinFullFx.CombineWithFilePath("Cake.exe"));
    var coreFxExe = MakeAbsolute(parameters.Paths.Directories.ArtifactsBinNetCore.CombineWithFilePath("Cake.dll"));

    context.Information("Testing {0} version...", fullFxExe);
    IEnumerable<string> fullFxOutput;
    var fullFxResult = StartProcess(
         fullFxExe,
         new ProcessSettings {
             Arguments = "--version",
             RedirectStandardOutput = true,
             WorkingDirectory = parameters.Paths.Directories.ArtifactsBinFullFx
         },
         out fullFxOutput
     );
    var fullFxVersion = string.Concat(fullFxOutput);

    context.Information("Testing {0} version...", coreFxExe);
    IEnumerable<string> coreFxOutput;
    var coreFxResult = StartProcess(
         context.Tools.Resolve("dotnet") ?? context.Tools.Resolve("dotnet.exe"),
         new ProcessSettings {
             Arguments = $"\"{coreFxExe}\" --version",
             RedirectStandardOutput = true,
             WorkingDirectory = parameters.Paths.Directories.ArtifactsBinNetCore
         },
         out coreFxOutput
     );
    var coreFxVersion = string.Concat(coreFxOutput);

    Information("{0}, ExitCode: {1}, Version: {2}",
        fullFxExe,
        fullFxResult,
        string.Concat(fullFxVersion)
        );

    Information("{0}, ExitCode: {1}, Version: {2}",
        coreFxExe,
        coreFxResult,
        string.Concat(coreFxVersion)
        );

    if (parameters.Version.SemVersion != fullFxVersion || parameters.Version.SemVersion != coreFxVersion)
    {
        throw new Exception(
            $"Invalid version, expected \"{parameters.Version.SemVersion}\", got .NET \"{fullFxVersion}\" and .NET Core \"{coreFxVersion}\"");
    }
});

Task("Zip-Files")
    .IsDependentOn("Validate-Version")
    .Does<BuildParameters>((context, parameters) =>
{
    // .NET 4.6
    var homebrewFiles = GetFiles( parameters.Paths.Directories.ArtifactsBinFullFx.FullPath + "/**/*");
    Zip(parameters.Paths.Directories.ArtifactsBinFullFx, parameters.Paths.Files.ZipArtifactPathDesktop, homebrewFiles);

    // .NET Core
    var coreclrFiles = GetFiles( parameters.Paths.Directories.ArtifactsBinNetCore.FullPath + "/**/*");
    Zip(parameters.Paths.Directories.ArtifactsBinNetCore, parameters.Paths.Files.ZipArtifactPathCoreClr, coreclrFiles);
});

Task("Create-Chocolatey-Packages")
    .IsDependentOn("Validate-Version")
    .IsDependentOn("Package")
    .WithCriteria<BuildParameters>((context, parameters) => parameters.IsRunningOnWindows)
    .Does<BuildParameters>((context, parameters) =>
{
    foreach(var package in parameters.Packages.Chocolatey)
    {
        var netFxFullArtifactPath = MakeAbsolute(parameters.Paths.Directories.ArtifactsBinFullFx).FullPath;
        var curDirLength =  MakeAbsolute(Directory("./")).FullPath.Length + 1;

        // Create package.
        ChocolateyPack(package.NuspecPath, new ChocolateyPackSettings {
            Version = parameters.Version.SemVersion,
            ReleaseNotes = parameters.ReleaseNotes.Notes.ToArray(),
            OutputDirectory = parameters.Paths.Directories.NuGetRoot,
            Files = (GetFiles(netFxFullArtifactPath + "/*.*") + GetFiles("./nuspec/*.txt") + GetFiles("./LICENSE"))
                                    .Select(file=>"../" + file.FullPath.Substring(curDirLength))
                                    .Select(file=> new ChocolateyNuSpecContent { Source = file })
                                    .ToArray()
        });
    }
});

Task("Create-NuGet-Packages")
    .IsDependentOn("Validate-Version")
    .Does<BuildParameters>((context, parameters) =>
{
    // Build libraries
    var projects = GetFiles("./src/**/*.csproj");
    foreach(var project in projects)
    {
        var name = project.GetDirectory().FullPath;
        if(name.EndsWith("Cake") || name.EndsWith("Tests"))
        {
            continue;
        }

        DotNetCorePack(project.FullPath, new DotNetCorePackSettings {
            Configuration = parameters.Configuration,
            OutputDirectory = parameters.Paths.Directories.NuGetRoot,
            NoBuild = true,
            NoRestore = true,
            MSBuildSettings = parameters.MSBuildSettings
        });
    }

    var netFxFullArtifactPath = MakeAbsolute(parameters.Paths.Directories.ArtifactsBinFullFx).FullPath;
    var netFxFullArtifactPathLength = netFxFullArtifactPath.Length+1;

    // Cake - .NET 4.6.1
    NuGetPack("./nuspec/Cake.nuspec", new NuGetPackSettings {
        Version = parameters.Version.SemVersion,
        ReleaseNotes = parameters.ReleaseNotes.Notes.ToArray(),
        BasePath = netFxFullArtifactPath,
        OutputDirectory = parameters.Paths.Directories.NuGetRoot,
        NoPackageAnalysis = true,
        Files = GetFiles(netFxFullArtifactPath + "/*")
                                .Select(file=>file.FullPath.Substring(netFxFullArtifactPathLength))
                                .Select(file=> new NuSpecContent { Source = file, Target = file })
                                .ToArray()
    });

    var netCoreFullArtifactPath = MakeAbsolute(parameters.Paths.Directories.ArtifactsBinNetCore).FullPath;
    var netCoreFullArtifactPathLength = netCoreFullArtifactPath.Length+1;

    // Cake - .NET Core
    NuGetPack("./nuspec/Cake.CoreCLR.nuspec", new NuGetPackSettings {
        Version = parameters.Version.SemVersion,
        ReleaseNotes = parameters.ReleaseNotes.Notes.ToArray(),
        BasePath = netCoreFullArtifactPath,
        OutputDirectory = parameters.Paths.Directories.NuGetRoot,
        NoPackageAnalysis = true,
        Files = GetFiles(netCoreFullArtifactPath + "/**/*")
                                .Select(file=>file.FullPath.Substring(netCoreFullArtifactPathLength))
                                .Select(file=> new NuSpecContent { Source = file, Target = file })
                                .ToArray()
    });

    DotNetCorePack("./src/Cake/Cake.csproj", new DotNetCorePackSettings {
        Configuration = parameters.Configuration,
        OutputDirectory = parameters.Paths.Directories.NuGetRoot,
        MSBuildSettings = parameters.MSBuildSettings,
        ArgumentCustomization = arg => arg.Append("/p:PackAsTool=true")
    });
});

Task("Sign-Binaries")
    .IsDependentOn("Zip-Files")
    .IsDependentOn("Create-Chocolatey-Packages")
    .IsDependentOn("Create-NuGet-Packages")
    .WithCriteria<BuildParameters>((context, parameters) =>
        (parameters.ShouldPublish && !parameters.SkipSigning) ||
        StringComparer.OrdinalIgnoreCase.Equals(EnvironmentVariable("SIGNING_TEST"), "True"))
    .Does<BuildParameters>((context, parameters) =>
{
    // Get the secret.
    var secret = EnvironmentVariable("SIGNING_SECRET");
    if(string.IsNullOrWhiteSpace(secret)) {
        throw new InvalidOperationException("Could not resolve signing secret.");
    }
    // Get the user.
    var user = EnvironmentVariable("SIGNING_USER");
    if(string.IsNullOrWhiteSpace(user)) {
        throw new InvalidOperationException("Could not resolve signing user.");
    }

    var settings = File("./signclient.json");
    var filter = File("./signclient.filter");

    // Get the files to sign.
    var files = GetFiles(string.Concat(parameters.Paths.Directories.NuGetRoot, "/", "*.nupkg"))
        + parameters.Paths.Files.ZipArtifactPathDesktop
        + parameters.Paths.Files.ZipArtifactPathCoreClr;

    foreach(var file in files)
    {
        Information("Signing {0}...", file.FullPath);

        // Build the argument list.
        var arguments = new ProcessArgumentBuilder()
            .Append("sign")
            .AppendSwitchQuoted("-c", MakeAbsolute(settings.Path).FullPath)
            .AppendSwitchQuoted("-i", MakeAbsolute(file).FullPath)
            .AppendSwitchQuoted("-f", MakeAbsolute(filter).FullPath)
            .AppendSwitchQuotedSecret("-s", secret)
            .AppendSwitchQuotedSecret("-r", user)
            .AppendSwitchQuoted("-n", "Cake")
            .AppendSwitchQuoted("-d", "Cake (C# Make) is a cross platform build automation system.")
            .AppendSwitchQuoted("-u", "https://cakebuild.net");

        // Sign the binary.
        var result = StartProcess(parameters.Paths.SignClientPath.FullPath, new ProcessSettings {  Arguments = arguments });
        if(result != 0)
        {
            // We should not recover from this.
            throw new InvalidOperationException("Signing failed!");
        }
    }
});

Task("Upload-AppVeyor-Artifacts")
    .IsDependentOn("Sign-Binaries")
    .IsDependentOn("Create-Chocolatey-Packages")
    .WithCriteria<BuildParameters>((context, parameters) => parameters.IsRunningOnAppVeyor)
    .Does<BuildParameters>((context, parameters) =>
{
    AppVeyor.UploadArtifact(parameters.Paths.Files.ZipArtifactPathDesktop);
    AppVeyor.UploadArtifact(parameters.Paths.Files.ZipArtifactPathCoreClr);
    foreach(var package in GetFiles(parameters.Paths.Directories.NuGetRoot + "/*"))
    {
        AppVeyor.UploadArtifact(package);
    }
});

Task("Upload-Coverage-Report")
    .WithCriteria<BuildParameters>((context, parameters) => FileExists(parameters.Paths.Files.TestCoverageOutputFilePath))
    .WithCriteria<BuildParameters>((context, parameters) => !parameters.IsLocalBuild)
    .WithCriteria<BuildParameters>((context, parameters) => !parameters.IsPullRequest)
    .WithCriteria<BuildParameters>((context, parameters) => parameters.IsMainCakeRepo)
    .IsDependentOn("Run-Unit-Tests")
    .Does<BuildParameters>((context, parameters) =>
{
    CoverallsIo(parameters.Paths.Files.TestCoverageOutputFilePath, new CoverallsIoSettings()
    {
        RepoToken = parameters.Coveralls.RepoToken
    });
});

Task("Publish-MyGet")
    .IsDependentOn("Sign-Binaries")
    .IsDependentOn("Package")
    .WithCriteria<BuildParameters>((context, parameters) => parameters.ShouldPublishToMyGet)
    .Does<BuildParameters>((context, parameters) =>
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
.OnError<BuildParameters>((exception, parameters) =>
{
    Information("Publish-MyGet Task failed, but continuing with next Task...");
    parameters.PublishingError = true;
});

Task("Publish-NuGet")
    .IsDependentOn("Sign-Binaries")
    .IsDependentOn("Create-NuGet-Packages")
    .WithCriteria<BuildParameters>((context, parameters) => parameters.ShouldPublish)
    .Does<BuildParameters>((context, parameters) =>
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

    foreach(var package in parameters.Packages.NuGet)
    {
        // Push the package.
        NuGetPush(package.PackagePath, new NuGetPushSettings {
          ApiKey = apiKey,
          Source = apiUrl
        });
    }
})
.OnError<BuildParameters>((exception, parameters) =>
{
    Information("Publish-NuGet Task failed, but continuing with next Task...");
    parameters.PublishingError = true;
});

Task("Publish-Chocolatey")
    .IsDependentOn("Sign-Binaries")
    .IsDependentOn("Create-Chocolatey-Packages")
    .WithCriteria<BuildParameters>((context, parameters) => parameters.ShouldPublish)
    .Does<BuildParameters>((context, parameters) =>
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
.OnError<BuildParameters>((exception, parameters) =>
{
    Information("Publish-Chocolatey Task failed, but continuing with next Task...");
    parameters.PublishingError = true;
});

Task("Publish-HomeBrew")
    .IsDependentOn("Sign-Binaries")
    .IsDependentOn("Zip-Files")
    .WithCriteria<BuildParameters>((context, parameters) => parameters.ShouldPublish)
    .Does<BuildParameters>((context, parameters) =>
{
    var hash = CalculateFileHash(parameters.Paths.Files.ZipArtifactPathDesktop).ToHex();
    Information("Hash for creating HomeBrew PullRequest: {0}", hash);
})
.OnError<BuildParameters>((exception, parameters) =>
{
    Information("Publish-HomeBrew Task failed, but continuing with next Task...");
    parameters.PublishingError = true;
});

Task("Publish-GitHub-Release")
    .WithCriteria<BuildParameters>((context, parameters) => parameters.ShouldPublish)
    .Does<BuildParameters>((context, parameters) =>
{
    GitReleaseManagerAddAssets(parameters.GitHub.Token, "cake-build", "cake", parameters.Version.Milestone, parameters.Paths.Files.ZipArtifactPathDesktop.ToString());
    GitReleaseManagerAddAssets(parameters.GitHub.Token, "cake-build", "cake", parameters.Version.Milestone, parameters.Paths.Files.ZipArtifactPathCoreClr.ToString());
    GitReleaseManagerClose(parameters.GitHub.Token, "cake-build", "cake", parameters.Version.Milestone);
})
.OnError<BuildParameters>((exception, parameters) =>
{
    Information("Publish-GitHub-Release Task failed, but continuing with next Task...");
    parameters.PublishingError = true;
});

Task("Create-Release-Notes")
    .Does<BuildParameters>((context, parameters) =>
{
    GitReleaseManagerCreate(parameters.GitHub.Token, "cake-build", "cake", new GitReleaseManagerCreateSettings {
        Milestone         = parameters.Version.Milestone,
        Name              = parameters.Version.Milestone,
        Prerelease        = false,
        TargetCommitish   = "main"
    });
});

Task("Prepare-Integration-Tests")
    .IsDependentOn("Create-NuGet-Packages")
    .Does<BuildParameters>((context, parameters) =>
{
   Unzip(parameters.Paths.Directories.NuGetRoot.CombineWithFilePath($"Cake.Tool.{parameters.Version.SemVersion}.nupkg"),
        parameters.Paths.Directories.IntegrationTestsBinTool);

   CopyDirectory(parameters.Paths.Directories.ArtifactsBinFullFx, parameters.Paths.Directories.IntegrationTestsBinFullFx);
   CopyDirectory(parameters.Paths.Directories.ArtifactsBinNetCore, parameters.Paths.Directories.IntegrationTestsBinNetCore);
});

Task("Run-Integration-Tests")
    .IsDependentOn("Prepare-Integration-Tests")
    .DeferOnError()
    .DoesForEach<BuildParameters, FilePath>(
        parameters => new[] {
            GetFiles($"{parameters.Paths.Directories.IntegrationTestsBinTool.FullPath}/**/netcoreapp2.1/**/Cake.dll").Single(),
            GetFiles($"{parameters.Paths.Directories.IntegrationTestsBinTool.FullPath}/**/netcoreapp3.0/**/Cake.dll").Single(),
            GetFiles($"{parameters.Paths.Directories.IntegrationTestsBinTool.FullPath}/**/net5.0/**/Cake.dll").Single(),
            parameters.Paths.Directories.IntegrationTestsBinFullFx.CombineWithFilePath("Cake.exe"),
            parameters.Paths.Directories.IntegrationTestsBinNetCore.CombineWithFilePath("Cake.dll")
        },
        (parameters, cakeAssembly, context) =>
{
    try
    {
        Information("Testing: {0}", cakeAssembly);
        CakeExecuteScript("./tests/integration/build.cake",
            new CakeSettings {
                ToolPath = cakeAssembly,
                EnvironmentVariables = {
                    ["MyEnvironmentVariable"] = "Hello World",
                    ["CAKE_INTEGRATION_TEST_ROOT"] = "../.."
                },
                ArgumentCustomization = args => args
                    .AppendSwitchQuoted("--target", " ", Argument("integration-tests-target", "Run-All-Tests"))
                    .AppendSwitchQuoted("--verbosity", " ", "quiet")
                    .AppendSwitchQuoted("--platform", " ", parameters.IsRunningOnWindows ? "windows" : "posix")
                    .AppendSwitchQuoted("--customarg", " ", "hello")
                    .AppendSwitchQuoted("--multipleargs", "=", "a")
                    .AppendSwitchQuoted("--multipleargs", "=", "b")
            });
    }
    catch(Exception ex)
    {
        Error("While testing: {0}\r\n{1}", cakeAssembly, ex);
        throw new Exception($"Exception while testing: {cakeAssembly}", ex);
    }
    finally
    {
        Information("Done testing: {0}", cakeAssembly);
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

Task("AppVeyor")
  .IsDependentOn("Upload-AppVeyor-Artifacts")
  .IsDependentOn("Upload-Coverage-Report")
  .IsDependentOn("Publish-MyGet")
  .IsDependentOn("Publish-NuGet")
  .IsDependentOn("Publish-Chocolatey")
  .IsDependentOn("Publish-HomeBrew")
  .IsDependentOn("Publish-GitHub-Release")
  .Does<BuildParameters>((context, parameters) =>
{
    if(parameters.PublishingError)
    {
        throw new Exception("An error occurred during the publishing of Cake.  All publishing tasks have been attempted.");
    }
});

Task("Travis")
  .IsDependentOn("Run-Unit-Tests");

Task("ReleaseNotes")
  .IsDependentOn("Create-Release-Notes");

Task("AzureDevOps")
  .IsDependentOn(IsRunningOnWindows() ? "Create-Chocolatey-Packages" : "Package");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(Argument("target", "Default"));
