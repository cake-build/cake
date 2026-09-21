static async Task UploadVerifyReceivedFiles(ICakeContext context, BuildParameters parameters)
{
    var receivedFiles = context.GetFiles("./src/**/*.received.*");
    if (receivedFiles.Count == 0)
    {
        return;
    }

    var zipPath = parameters.Paths.Directories.Artifacts.CombineWithFilePath("verify-received.zip");
    context.EnsureDirectoryExists(zipPath.GetDirectory());
    context.Zip("./", zipPath, receivedFiles);

    var buildSystem = context.BuildSystem();
    switch (buildSystem.Provider)
    {
        case BuildProvider.GitHubActions:
            var gh = buildSystem.GitHubActions;
            var artifactName = $"verify-received_{gh.Environment.Runner.ImageOS ?? gh.Environment.Runner.OS}_{gh.Environment.Runner.Architecture}";
            await gh.Commands.UploadArtifact(zipPath, artifactName);
            break;
        case BuildProvider.AppVeyor:
            buildSystem.AppVeyor.UploadArtifact(zipPath);
            break;
        case BuildProvider.AzurePipelines:
            buildSystem.AzurePipelines.Commands.UploadArtifact("Verify", zipPath, "Verify");
            break;
        default:
            context.Verbose("Verify received zip created, but {0} does not support Cake artifact upload.", buildSystem.Provider);
            break;
    }
}
