public class BuildVersion
{
    public string Version { get; private set; }
    public string SemVersion { get; private set; }
    public string DotNetAsterix { get; private set; }
    public string Milestone { get; private set; }
    public string CakeVersion { get; private set; }

    public static BuildVersion Calculate(ICakeContext context, BuildParameters parameters)
    {
        if (context == null)
        {
            throw new ArgumentNullException("context");
        }

        string version = null;
        string semVersion = null;
        string milestone = null;

        if (context.IsRunningOnWindows() && !parameters.SkipGitVersion)
        {
            context.Information("Calculating Semantic Version");
            if (!parameters.IsLocalBuild || parameters.IsPublishBuild || parameters.IsReleaseBuild)
            {
                context.GitVersion(new GitVersionSettings{
                    UpdateAssemblyInfoFilePath = "./src/SolutionInfo.cs",
                    UpdateAssemblyInfo = true,
                    OutputType = GitVersionOutput.BuildServer
                });

                version = context.EnvironmentVariable("GitVersion_MajorMinorPatch");
                semVersion = context.EnvironmentVariable("GitVersion_LegacySemVerPadded");
                milestone = string.Concat("v", version);
            }

            GitVersion assertedVersions = context.GitVersion(new GitVersionSettings
            {
                OutputType = GitVersionOutput.Json,
            });

            version = assertedVersions.MajorMinorPatch;
            semVersion = assertedVersions.LegacySemVerPadded;
            milestone = string.Concat("v", version);

            context.Information("Calculated Semantic Version: {0}", semVersion);
        }

        if (string.IsNullOrEmpty(version) || string.IsNullOrEmpty(semVersion))
        {
            context.Information("Fetching verson from first SolutionInfo...");
            version = ReadSolutionInfoVersion(context);
            semVersion = version;
            milestone = string.Concat("v", version);
        }

        var cakeVersion = typeof(ICakeContext).Assembly.GetName().Version.ToString();

        return new BuildVersion
        {
            Version = version,
            SemVersion = semVersion,
            DotNetAsterix = semVersion.Substring(version.Length).TrimStart('-'),
            Milestone = milestone,
            CakeVersion = cakeVersion
        };
    }

    public static string ReadSolutionInfoVersion(ICakeContext context)
    {
        var solutionInfo = context.ParseAssemblyInfo("./src/SolutionInfo.cs");
        if (!string.IsNullOrEmpty(solutionInfo.AssemblyVersion))
        {
            return solutionInfo.AssemblyVersion;
        }
        throw new CakeException("Could not parse version.");
    }
}