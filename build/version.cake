public class BuildVersion
{
    public string Version { get; private set; }
    public string SemVersion { get; private set; }
    public string Milestone { get; private set; }
    public string CakeVersion { get; private set; }
    public string SolutionVersion { get; private set; }
    public string SolutionSemVer { get ; private set; }

    public static BuildVersion CalculateSemanticVersion(
        ICakeContext context,
        bool isLocalBuild,
        bool isPublishBuild,
        bool isReleaseBuild
        )
    {
        if (context == null)
        {
            throw new ArgumentNullException("context");
        }

        string version = null;
        string semVersion = null;
        string milestone = null;

        var cakeVersion = typeof(ICakeContext).Assembly.GetName().Version.ToString();

        if (context.IsRunningOnWindows())
        {
            context.Information("Calculating Semantic Version...");
            if (!isLocalBuild)
            {
                context.GitVersion(new GitVersionSettings {
                    UpdateAssemblyInfoFilePath = "./src/SolutionInfo.cs",
                    UpdateAssemblyInfo = true,
                    OutputType = GitVersionOutput.BuildServer
                });

                version = context.EnvironmentVariable("GitVersion_MajorMinorPatch");
                semVersion = context.EnvironmentVariable("GitVersion_LegacySemVerPadded");
                milestone = string.Concat("v", version);
            }

            if (string.IsNullOrEmpty(version) || string.IsNullOrEmpty(semVersion))
            {
                GitVersionSettings gitVersionSettings = (isPublishBuild || isReleaseBuild)
                    ? new GitVersionSettings {
                        UpdateAssemblyInfoFilePath = "./src/SolutionInfo.cs",
                        UpdateAssemblyInfo = true,
                        OutputType = GitVersionOutput.Json
                        }
                    : new GitVersionSettings { OutputType = GitVersionOutput.Json };

                GitVersion assertedVersions = context.GitVersion(gitVersionSettings);

                version = assertedVersions.MajorMinorPatch;
                semVersion = assertedVersions.LegacySemVerPadded;
                milestone = string.Concat("v", version);
            }

            context.Information("Calculated Semantic Version: {0}", semVersion);
        }

        context.Information("Fetching version from SolutionInfo...");
        var assemblyInfo = context.ParseAssemblyInfo("./src/SolutionInfo.cs");
        var solutionVersion = assemblyInfo.AssemblyVersion;
        var solutionSemVer = assemblyInfo.AssemblyInformationalVersion;
        context.Information("SolutionInfo Version: {0}", solutionVersion);

        if (string.IsNullOrEmpty(version) || string.IsNullOrEmpty(semVersion))
        {
            context.Information("No GitVersion found using solution version...");
            version = solutionVersion;
            semVersion = solutionSemVer;
            milestone = string.Concat("v", solutionVersion);
        }

        return new BuildVersion {
            Version = version,
            SemVersion = semVersion,
            Milestone = milestone,
            CakeVersion = cakeVersion,
            SolutionVersion = solutionVersion,
            SolutionSemVer = solutionSemVer
        };
    }
}