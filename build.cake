// Install addins.
#addin "nuget:https://api.nuget.org/v3/index.json?package=Cake.Twitter&version=2.0.0"
#addin "nuget:https://api.nuget.org/v3/index.json?package=Cake.Gitter&version=2.0.0"

// Install .NET Core Global tools.
#tool "dotnet:https://api.nuget.org/v3/index.json?package=GitVersion.Tool&version=5.8.1"
#tool "dotnet:https://api.nuget.org/v3/index.json?package=SignClient&version=1.3.155"
#tool "dotnet:https://api.nuget.org/v3/index.json?package=GitReleaseManager.Tool&version=0.12.1"

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

    return parameters;
});

Teardown<BuildParameters>((context, parameters) =>
{
    Information("Starting Teardown...");

    if(context.Successful)
    {
        if(parameters.ShouldPublish)
        {
            var message = $"Version {parameters.Version.SemVersion} of Cake has just been released, https://www.nuget.org/packages/Cake.Tool/{parameters.Version.SemVersion} 🎉";

            if(parameters.CanPostToTwitter)
            {
                TwitterSendTweet(parameters.Twitter.ConsumerKey, parameters.Twitter.ConsumerSecret, parameters.Twitter.AccessToken, parameters.Twitter.AccessTokenSecret, message);
            }

            if(parameters.CanPostToGitter)
            {
                var gitterMessage = $"@/all {message}";

                var postMessageResult = Gitter.Chat.PostMessage(
                    message: gitterMessage,
                    messageSettings: new GitterChatMessageSettings { Token = parameters.Gitter.Token, RoomId = parameters.Gitter.RoomId}
                );

                if (postMessageResult.Ok)
                {
                    Information("Message {0} successfully sent", postMessageResult.TimeStamp);
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
    DotNetRestore("./src/Cake.sln", new DotNetRestoreSettings
    {
        Verbosity = DotNetVerbosity.Minimal,
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
    DotNetBuild(path.FullPath, new DotNetBuildSettings
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
    foreach(var framework in new[] { "netcoreapp3.1", "net5.0", "net6.0" })
    {
        FilePath testResultsPath = MakeAbsolute(parameters.Paths.Directories.TestResults
                                    .CombineWithFilePath($"{project.GetFilenameWithoutExtension()}_{framework}_TestResults.xml"));

        DotNetTest(project.FullPath, new DotNetTestSettings
        {
            Framework = framework,
            NoBuild = true,
            NoRestore = true,
            Configuration = parameters.Configuration,
            ArgumentCustomization = args=>args.Append($"--logger trx;LogFileName=\"{testResultsPath}\"")
        });
    }
});

Task("Create-NuGet-Packages")
    .IsDependentOn("Run-Unit-Tests")
    .Does<BuildParameters>((context, parameters) =>
{
    // Build libraries
    var projects = GetFiles("./src/*/*.csproj");
    foreach(var project in projects)
    {
        var name = project.GetDirectory().FullPath;
        if(name.EndsWith("Tests") || name.EndsWith("Example"))
        {
            continue;
        }

        DotNetPack(project.FullPath, new DotNetPackSettings {
            Configuration = parameters.Configuration,
            OutputDirectory = parameters.Paths.Directories.NuGetRoot,
            NoBuild = true,
            NoRestore = true,
            MSBuildSettings = parameters.MSBuildSettings
        });
    }
});

Task("Sign-Binaries")
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
    var files = GetFiles(string.Concat(parameters.Paths.Directories.NuGetRoot, "/", "*.nupkg"));

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
    .WithCriteria<BuildParameters>((context, parameters) => parameters.IsRunningOnAppVeyor)
    .Does<BuildParameters>((context, parameters) =>
{
    foreach(var package in GetFiles(parameters.Paths.Directories.NuGetRoot + "/*"))
    {
        AppVeyor.UploadArtifact(package);
    }
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

    foreach(var package in parameters.Packages.NuGet)
    {
        // Push the package.
        var settings = new DotNetNuGetPushSettings {
            ApiKey = apiKey,
            Source = apiUrl
        };

        DotNetNuGetPush(package.PackagePath.FullPath, settings);
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
        var settings = new DotNetNuGetPushSettings {
            ApiKey = apiKey,
            Source = apiUrl
        };

        DotNetNuGetPush(package.PackagePath.FullPath, settings);
    }
})
.OnError<BuildParameters>((exception, parameters) =>
{
    Information("Publish-NuGet Task failed, but continuing with next Task...");
    parameters.PublishingError = true;
});

Task("Publish-GitHub-Release")
    .WithCriteria<BuildParameters>((context, parameters) => parameters.ShouldPublish)
    .Does<BuildParameters>((context, parameters) =>
{
    foreach(var package in GetFiles(parameters.Paths.Directories.NuGetRoot + "/*"))
    {
        GitReleaseManagerAddAssets(parameters.GitHub.Token, "cake-build", "cake", parameters.Version.Milestone, package.FullPath);
    }

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
});

Task("Frosting-Integration-Tests")
    .DeferOnError()
    .DoesForEach<BuildParameters, (string Framework, FilePath Project)>(
        (parameters, context) => {
            var project = context.MakeAbsolute(
                new FilePath("tests/integration/Cake.Frosting/build/Build.csproj")
            );

            DotNetBuild(project.FullPath,
                new DotNetBuildSettings
                {
                    Verbosity = DotNetVerbosity.Quiet,
                    Configuration = parameters.Configuration,
                    MSBuildSettings = parameters.MSBuildSettings
                });

            context.Verbose("Peeking into {0}...", project);

            var targetFrameworks = context.XmlPeek(
                project,
                "/Project/PropertyGroup/TargetFrameworks"
            );

            return targetFrameworks?.Split(';')
                    .Select(targetFramework => (targetFramework, project))
                    .ToArray();
        },
        (parameters, test, context) =>
{
    try
    {
        Information("Testing: {0}", test.Framework);

        DotNetRun(test.Project.FullPath,
            new ProcessArgumentBuilder()
                .AppendSwitchQuoted("--verbosity", "=", "quiet"),
            new DotNetRunSettings
            {
                Configuration = parameters.Configuration,
                Framework = test.Framework,
                NoRestore = true,
                NoBuild = true
            });
    }
    catch(Exception ex)
    {
        Error("While testing: {0}\r\n{1}", test.Framework, ex);
        throw new Exception($"Exception while testing: {test.Framework}", ex);
    }
    finally
    {
        Information("Done testing: {0}", test.Framework);
    }
});
Task("Run-Integration-Tests")
    .IsDependentOn("Prepare-Integration-Tests")
    .IsDependentOn("Frosting-Integration-Tests")
    .DeferOnError()
    .DoesForEach<BuildParameters, FilePath>(
        parameters => new[] {
            GetFiles($"{parameters.Paths.Directories.IntegrationTestsBinTool.FullPath}/**/netcoreapp3.1/**/Cake.dll").Single(),
            GetFiles($"{parameters.Paths.Directories.IntegrationTestsBinTool.FullPath}/**/net5.0/**/Cake.dll").Single(),
            GetFiles($"{parameters.Paths.Directories.IntegrationTestsBinTool.FullPath}/**/net6.0/**/Cake.dll").Single()
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
                    .AppendSwitchQuoted("--testAssemblyDirectoryPath", "=", cakeAssembly.GetDirectory().FullPath)
                    .AppendSwitchQuoted("--testAssemblyFilePath", "=", cakeAssembly.FullPath)
                    .AppendSwitchQuoted("--testDotNetCoreVerbosity", "=", "Diagnostic")
                    .AppendSwitchQuoted("--testDotNetRollForward", "=", "LatestMajor")
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
  .IsDependentOn("Create-NuGet-Packages");

Task("Default")
  .IsDependentOn("Package");

Task("AppVeyor")
  .IsDependentOn("Upload-AppVeyor-Artifacts")
  .IsDependentOn("Publish-MyGet")
  .IsDependentOn("Publish-NuGet")
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
  .IsDependentOn("Package");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(Argument("target", "Default"));
