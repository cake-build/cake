#load "./../../../utilities/xunit.cake"

Task("Cake.Common.Build.RwxProvider.Provider")
    .Does(() => {
        Assert.Equal(BuildProvider.Rwx, BuildSystem.Provider);
});

Task("Cake.Common.Build.RwxProvider.Environment")
    .Does(() => {
        Information("RWX Provider:");
        Information("  IsRunningOnRwx: {0}", BuildSystem.Rwx.IsRunningOnRwx);

        Information("  Environment:");
        Information("    CI: {0}", BuildSystem.Rwx.Environment.CI);
        Information("    Rwx: {0}", BuildSystem.Rwx.Environment.Rwx);

        Information("  Run:");
        Information("    Id: {0}", BuildSystem.Rwx.Environment.Run.Id);
        Information("    Title: {0}", BuildSystem.Rwx.Environment.Run.Title);
        Information("    Url: {0}", BuildSystem.Rwx.Environment.Run.Url);

        Information("  Task:");
        Information("    Id: {0}", BuildSystem.Rwx.Environment.Task.Id);
        Information("    Url: {0}", BuildSystem.Rwx.Environment.Task.Url);
        Information("    AttemptNumber: {0}", BuildSystem.Rwx.Environment.Task.AttemptNumber);

        Information("  Actor:");
        Information("    Id: {0}", BuildSystem.Rwx.Environment.Actor.Id);
        Information("    Name: {0}", BuildSystem.Rwx.Environment.Actor.Name);

        Information("  Git:");
        Information("    RepositoryUrl: {0}", BuildSystem.Rwx.Environment.Git.RepositoryUrl);
        Information("    RepositoryName: {0}", BuildSystem.Rwx.Environment.Git.RepositoryName);
        Information("    CommitSha: {0}", BuildSystem.Rwx.Environment.Git.CommitSha);
        Information("    CommitSummary: {0}", BuildSystem.Rwx.Environment.Git.CommitSummary);
        Information("    CommitterName: {0}", BuildSystem.Rwx.Environment.Git.CommitterName);
        Information("    CommitterEmail: {0}", BuildSystem.Rwx.Environment.Git.CommitterEmail);
        Information("    Ref: {0}", BuildSystem.Rwx.Environment.Git.Ref);
        Information("    RefName: {0}", BuildSystem.Rwx.Environment.Git.RefName);

        Information("  Runtime:");
        Information("    ValuesPath: {0}", BuildSystem.Rwx.Environment.Runtime.ValuesPath);
        Information("    ArtifactsPath: {0}", BuildSystem.Rwx.Environment.Runtime.ArtifactsPath);
        Information("    EnvPath: {0}", BuildSystem.Rwx.Environment.Runtime.EnvPath);
        Information("    IsRuntimeAvailable: {0}", BuildSystem.Rwx.Environment.Runtime.IsRuntimeAvailable);
});

Task("Cake.Common.Build.RwxProvider.Commands")
    .Does(() => {
        BuildSystem.Rwx.Commands.SetValue("cake_integration_test", "ok");
        BuildSystem.Rwx.Commands.SetEnvironmentVariable("CAKE_INTEGRATION_TEST", "ok");

        var artifactSource = Paths.Temp.CombineWithFilePath("cake-rwx-artifact.txt");
        EnsureDirectoryExist(artifactSource.GetDirectory());
        System.IO.File.WriteAllText(artifactSource.FullPath, "cake integration test");
        BuildSystem.Rwx.Commands.UploadArtifact(artifactSource);
});

var rwxProviderTask = Task("Cake.Common.Build.RwxProvider")
                            .IsDependentOn("Cake.Common.Build.RwxProvider.Environment");

if (BuildSystem.Rwx.IsRunningOnRwx)
{
    rwxProviderTask
        .IsDependentOn("Cake.Common.Build.RwxProvider.Provider")
        .IsDependentOn("Cake.Common.Build.RwxProvider.Commands");
}
