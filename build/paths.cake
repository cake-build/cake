public record BuildPaths(
    BuildDirectories Directories,
    FilePath SignClientPath,
    FilePath SignFilterPath
)
{
    public static BuildPaths GetPaths(
        ICakeContext context,
        string configuration,
        string semVersion
        )
    {
        if (context == null)
        {
            throw new ArgumentNullException("context");
        }
        if (string.IsNullOrEmpty(configuration))
        {
            throw new ArgumentNullException("configuration");
        }
        if (string.IsNullOrEmpty(semVersion))
        {
            throw new ArgumentNullException("semVersion");
        }

        var artifactsDir = (DirectoryPath)(context.Directory("./artifacts") + context.Directory("v" + semVersion));
        var testResultsDir = artifactsDir.Combine("test-results");
        var nugetRoot = artifactsDir.Combine("nuget");

        var testCoverageOutputFilePath = testResultsDir.CombineWithFilePath("OpenCover.xml");

        var integrationTestsBin = context.MakeAbsolute(context.Directory("./tests/integration/tools"));
        var integrationTestsBinTool = integrationTestsBin.Combine("Cake.Tool");

        // Directories
        var buildDirectories = new BuildDirectories(
            artifactsDir,
            testResultsDir,
            nugetRoot,
            integrationTestsBinTool);

        FilePath signClientPath = null;
        if (context.IsRunningOnWindows() && context.GitHubActions().IsRunningOnGitHubActions)
        {
            signClientPath = context.Tools.Resolve("sign.exe");

            if (signClientPath == null)
            {
                context.Information("Installing sign tool...");
                context.CakeExecuteScript(
                    "./build/signtool.cake",
                    new CakeSettings {
                        Verbosity = Verbosity.Quiet,
                        EnvironmentVariables = {
                            ["CAKE_PATHS_TOOLS"] = context.MakeAbsolute(context.Directory("./tools")).FullPath
                        }
                    }
                );
                
                signClientPath = context.Tools.Resolve("sign.exe")
                                    ?? throw new Exception("Failed to locate sign tool");

            }
        }

        var signFilterPath = context.MakeAbsolute(context.File("./build/signclient.filter"));

        return new BuildPaths(
            Directories: buildDirectories,
            SignClientPath: signClientPath,
            SignFilterPath: signFilterPath
        );
    }
}

public record BuildDirectories(
        DirectoryPath Artifacts,
        DirectoryPath TestResults,
        DirectoryPath NuGetRoot,
        DirectoryPath IntegrationTestsBinTool
        )
{
    public ICollection<DirectoryPath> ToClean { get; } = new[] {
            Artifacts,
            TestResults,
            NuGetRoot,
            IntegrationTestsBinTool
        };
}