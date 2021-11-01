public record BuildVersion(
    string Version,
    string SemVersion,
    string DotNetAsterix,
    string Milestone,
    string CakeVersion
)
{
    public static BuildVersion Calculate(ICakeContext context, BuildParameters parameters)
    {
        if (context == null)
        {
            throw new ArgumentNullException("context");
        }

        string version = null;
        string semVersion = null;
        string milestone = null;

        if (!parameters.SkipGitVersion)
        {
            // Temp Workaround GitVersion Azure Pipelines
            var azurePipelines = context.AzurePipelines();
            string sourceBranch = string.Empty;
            if (azurePipelines.IsRunningOnAzurePipelines && azurePipelines.Environment.PullRequest.Number > 0)
            {
                 sourceBranch = $"PullRequest{azurePipelines.Environment.PullRequest.Number}";
                 context.Information("Overriding Azure Pipelines branch name with: {0}", sourceBranch);
            }

            context.Information("Calculating Semantic Version");
            if (!parameters.IsLocalBuild || parameters.IsPublishBuild || parameters.IsReleaseBuild)
            {
                context.GitVersion(new GitVersionSettings{
                    UpdateAssemblyInfoFilePath = "./src/SolutionInfo.cs",
                    UpdateAssemblyInfo = true,
                    OutputType = GitVersionOutput.BuildServer,
                    EnvironmentVariables = {
                                                { "BUILD_SOURCEBRANCH", sourceBranch },
                                                { "SYSTEM_PULLREQUEST_SOURCEBRANCH", sourceBranch }
                                           }
                });

                version = context.EnvironmentVariable("GitVersion_MajorMinorPatch");
                semVersion = context.EnvironmentVariable("GitVersion_LegacySemVerPadded");
                milestone = string.Concat("v", version);
            }

            GitVersion assertedVersions = context.GitVersion(new GitVersionSettings
            {
                OutputType = GitVersionOutput.Json,
                EnvironmentVariables = {
                                            { "BUILD_SOURCEBRANCH", sourceBranch },
                                            { "SYSTEM_PULLREQUEST_SOURCEBRANCH", sourceBranch }
                                       }
            });

            version = assertedVersions.MajorMinorPatch;
            semVersion = assertedVersions.LegacySemVerPadded;
            milestone = string.Concat("v", version);

            context.Information("Calculated Semantic Version: {0} (Version: {1}, Milestone: {2})", semVersion, version, milestone);
        }

        if (string.IsNullOrEmpty(version) || string.IsNullOrEmpty(semVersion))
        {
            context.Information("Fetching version from first SolutionInfo...");
            version = ReadSolutionInfoVersion(context);
            semVersion = version;
            milestone = string.Concat("v", version);

            context.Information("Fetched Semantic Version: {0} (Version: {1}, Milestone: {2})", semVersion, version, milestone);
        }

        var cakeVersion = typeof(ICakeContext).Assembly.GetName().Version.ToString();

        return new BuildVersion(
            Version: version,
            SemVersion: semVersion,
            DotNetAsterix: semVersion.Substring(version.Length).TrimStart('-'),
            Milestone: milestone,
            CakeVersion: cakeVersion
        );
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
