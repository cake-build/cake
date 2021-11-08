#load "./../../../utilities/xunit.cake"

Task("Cake.Common.Build.GitHubActionsProvider.Provider")
    .Does(() => {
        Assert.Equal(BuildProvider.GitHubActions, BuildSystem.Provider);
});

Task("Cake.Common.Build.GitHubActionsProvider.Commands.AddPath")
    .Does(() => {
        // Given
        FilePath path = typeof(ICakeContext).GetTypeInfo().Assembly.Location;

        // When
        GitHubActions.Commands.AddPath(path.GetDirectory());
});


Task("Cake.Common.Build.GitHubActionsProvider.Commands.SetEnvironmentVariable")
    .Does(() => {
        // Given
        string key = $"CAKE_{GitHubActions.Environment.Runner.OS}_{Context.Environment.Runtime.BuiltFramework.Identifier}_{Context.Environment.Runtime.BuiltFramework.Version}_VERSION"
                        .Replace(".", "_")
                        .Replace("__", "_")
                        .ToUpper(),
                value = Context.Environment.Runtime.CakeVersion.ToString(3);

        // When
        GitHubActions.Commands.SetEnvironmentVariable(key, value);
});

Task("Cake.Common.Build.GitHubActionsProvider.Commands.UploadArtifact.File")
    .Does(async () => {
        // Given
        FilePath path = typeof(ICakeContext).GetTypeInfo().Assembly.Location;
        string artifactName = $"File_{GitHubActions.Environment.Runner.OS}_{Context.Environment.Runtime.BuiltFramework.Identifier}_{Context.Environment.Runtime.BuiltFramework.Version}";

        // When
        await GitHubActions.Commands.UploadArtifact(path, artifactName);
});

Task("Cake.Common.Build.GitHubActionsProvider.Commands.UploadArtifact.Directory")
    .Does(async () => {
        // Given
        FilePath path = typeof(ICakeContext).GetTypeInfo().Assembly.Location;
        string artifactName = $"Directory_{GitHubActions.Environment.Runner.OS}_{Context.Environment.Runtime.BuiltFramework.Identifier}_{Context.Environment.Runtime.BuiltFramework.Version}";

        // When
        await GitHubActions.Commands.UploadArtifact(path.GetDirectory(), artifactName);
});

var gitHubActionsProviderTask = Task("Cake.Common.Build.GitHubActionsProvider");

if (GitHubActions.IsRunningOnGitHubActions)
{
    gitHubActionsProviderTask
        .IsDependentOn("Cake.Common.Build.GitHubActionsProvider.Provider")
        .IsDependentOn("Cake.Common.Build.GitHubActionsProvider.Commands.AddPath")
        .IsDependentOn("Cake.Common.Build.GitHubActionsProvider.Commands.SetEnvironmentVariable");

}

if (GitHubActions.Environment.Runtime.IsRuntimeAvailable)
{
    gitHubActionsProviderTask
        .IsDependentOn("Cake.Common.Build.GitHubActionsProvider.Commands.UploadArtifact.File");
    gitHubActionsProviderTask
        .IsDependentOn("Cake.Common.Build.GitHubActionsProvider.Commands.UploadArtifact.Directory");
}