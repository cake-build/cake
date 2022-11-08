#load "./../../../utilities/xunit.cake"
#load "./../../../utilities/paths.cake"
#load "./../../../utilities/io.cake"

Task("Cake.Common.Build.GitHubActionsProvider.Provider")
    .Does(() => {
        Assert.Equal(BuildProvider.GitHubActions, BuildSystem.Provider);
});

Task("Cake.Common.Build.GitHubActionsProvider.Commands.Debug")
    .Does(() => {
        // When
        GitHubActions.Commands.Debug("This is a debug message");
});

Task("Cake.Common.Build.GitHubActionsProvider.Commands.Notice")
    .Does(() => {
        // When
        GitHubActions.Commands.Notice("This is a notice message");
        GitHubActions.Commands.Notice("This is a notice message with annotation", new GitHubActionsAnnotation { File = "tests/integration/Cake.Common/Build/GitHubActions/GitHubActionsProvider.cake", StartLine = 20, StartColumn = 40, EndColumn = 80 });
});

Task("Cake.Common.Build.GitHubActionsProvider.Commands.Warning")
    .Does(() => {
        // When
        GitHubActions.Commands.Warning("This is a warning message");
        GitHubActions.Commands.Warning("This is a warning message with annotation", new GitHubActionsAnnotation { File = "tests/integration/Cake.Common/Build/GitHubActions/GitHubActionsProvider.cake", StartLine = 27, StartColumn = 41, EndColumn = 82 });
});

Task("Cake.Common.Build.GitHubActionsProvider.Commands.Error")
    .Does(() => {
        // When
        GitHubActions.Commands.Error("This is an error message");
        GitHubActions.Commands.Error("This is an error message with annotation", new GitHubActionsAnnotation { File = "tests/integration/Cake.Common/Build/GitHubActions/GitHubActionsProvider.cake", StartLine = 34, StartColumn = 39, EndColumn = 79 });
});

Task("Cake.Common.Build.GitHubActionsProvider.Commands.Group")
    .Does(() => {
        // When
        GitHubActions.Commands.StartGroup("Cake group");
        System.Console.WriteLine("This is inside a group");
        GitHubActions.Commands.EndGroup();
});

Task("Cake.Common.Build.GitHubActionsProvider.Commands.SetSecret")
    .Does(() => {
        // Given
        var secret = Guid.NewGuid().ToString();

        // When
        GitHubActions.Commands.SetSecret(secret);
        Information("This is secret: {0}", secret);
});

Task("Cake.Common.Build.GitHubActionsProvider.Commands.AddPath")
    .Does<GitHubActionsData>(data => {
        // When
        GitHubActions.Commands.AddPath(data.AssemblyPath.GetDirectory());
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

Task("Cake.Common.Build.GitHubActionsProvider.Commands.SetOutputParameter")
    .Does(() => {
        // Given
        string key = $"CAKE_{Context.Environment.Runtime.BuiltFramework.Identifier}_{Context.Environment.Runtime.BuiltFramework.Version}_VERSION_OS"
                        .Replace(".", "_")
                        .Replace("__", "_")
                        .ToUpper(),
                value = string.Join(
                                '_',
                                Context.Environment.Runtime.CakeVersion.ToString(3),
                                GitHubActions.Environment.Runner.OS)
                            .ToUpper();

        // When
        GitHubActions.Commands.SetOutputParameter(key, value);
});

Task("Cake.Common.Build.GitHubActionsProvider.Commands.SetStepSummary")
    .Does(() => {
        // Given
        string summary = $@"## Identifier
{Context.Environment.Runtime.BuiltFramework.Identifier}

## Built Framework Version
{Context.Environment.Runtime.BuiltFramework.Version}

## Cake Version
{Context.Environment.Runtime.CakeVersion.ToString(3)}

## Runner OS
{GitHubActions.Environment.Runner.OS}";

        // When
        GitHubActions.Commands.SetStepSummary(summary);
});

Task("Cake.Common.Build.GitHubActionsProvider.Commands.UploadArtifact.File")
    .Does<GitHubActionsData>(async data => {
        // When
        await GitHubActions.Commands.UploadArtifact(data.AssemblyPath, data.FileArtifactName);
});

Task("Cake.Common.Build.GitHubActionsProvider.Commands.UploadArtifact.Directory")
    .Does<GitHubActionsData>(async data => {
        // When
        await GitHubActions.Commands.UploadArtifact(data.AssemblyPath.GetDirectory(), data.DirectoryArtifactName);
});

Task("Cake.Common.Build.GitHubActionsProvider.Commands.DownloadArtifact")
    .IsDependentOn("Cake.Common.Build.GitHubActionsProvider.Commands.UploadArtifact.File")
    .IsDependentOn("Cake.Common.Build.GitHubActionsProvider.Commands.UploadArtifact.Directory")
    .Does<GitHubActionsData>(async data => {
        // Given
        var targetPath = Paths.Temp.Combine("./Cake.Common.Build.GitHubActionsProvider.Commands.DownloadArtifact");
        EnsureDirectoryExists(targetPath);
        var targetArtifactPath = targetPath.CombineWithFilePath(data.AssemblyPath.GetFilename());

        // When
        await GitHubActions.Commands.DownloadArtifact(data.FileArtifactName, targetPath);

        // Then
        Assert.True(System.IO.File.Exists(targetArtifactPath.FullPath), $"{targetArtifactPath.FullPath} Missing");
        Assert.True(FileHashEquals(data.AssemblyPath, targetArtifactPath), $"{data.AssemblyPath.FullPath}=={targetArtifactPath.FullPath}");
});

Task("Cake.Common.Build.GitHubActionsProvider.Environment.Runner.Architecture")
    .Does(() => {
        // Given / When
        var result = GitHubActions.Environment.Runner.Architecture switch {
            GitHubActionsArchitecture.Unknown => !GitHubActions.IsRunningOnGitHubActions,
            _=> GitHubActions.IsRunningOnGitHubActions
        };

        // Then
        Assert.True(result);
});

Task("Cake.Common.Build.GitHubActionsProvider.Environment.Workflow.RefType")
    .Does(() => {
        // Given / When
        var result = GitHubActions.Environment.Workflow.RefType switch {
            GitHubActionsRefType.Unknown => !GitHubActions.IsRunningOnGitHubActions,
            _=> GitHubActions.IsRunningOnGitHubActions
        };

        // Then
        Assert.True(result);
});


var gitHubActionsProviderTask = Task("Cake.Common.Build.GitHubActionsProvider")
                                    .IsDependentOn("Cake.Common.Build.GitHubActionsProvider.Environment.Runner.Architecture")
                                    .IsDependentOn("Cake.Common.Build.GitHubActionsProvider.Environment.Workflow.RefType");

if (GitHubActions.IsRunningOnGitHubActions)
{
    gitHubActionsProviderTask
        .IsDependentOn("Cake.Common.Build.GitHubActionsProvider.Provider")
        .IsDependentOn("Cake.Common.Build.GitHubActionsProvider.Commands.Debug")
        .IsDependentOn("Cake.Common.Build.GitHubActionsProvider.Commands.Notice")
        .IsDependentOn("Cake.Common.Build.GitHubActionsProvider.Commands.Warning")
        .IsDependentOn("Cake.Common.Build.GitHubActionsProvider.Commands.Error")
        .IsDependentOn("Cake.Common.Build.GitHubActionsProvider.Commands.Group")
        .IsDependentOn("Cake.Common.Build.GitHubActionsProvider.Commands.SetSecret")
        .IsDependentOn("Cake.Common.Build.GitHubActionsProvider.Commands.AddPath")
        .IsDependentOn("Cake.Common.Build.GitHubActionsProvider.Commands.SetEnvironmentVariable")
        .IsDependentOn("Cake.Common.Build.GitHubActionsProvider.Commands.SetOutputParameter")
        .IsDependentOn("Cake.Common.Build.GitHubActionsProvider.Commands.SetStepSummary");
}

if (GitHubActions.Environment.Runtime.IsRuntimeAvailable)
{
    Setup(context => new GitHubActionsData {
        AssemblyPath = typeof(ICakeContext).GetTypeInfo().Assembly.Location,
        FileArtifactName = $"File_{GitHubActions.Environment.Runner.ImageOS ?? GitHubActions.Environment.Runner.OS}_{Context.Environment.Runtime.BuiltFramework.Identifier}_{Context.Environment.Runtime.BuiltFramework.Version}",
        DirectoryArtifactName = $"Directory_{GitHubActions.Environment.Runner.ImageOS ?? GitHubActions.Environment.Runner.OS}_{Context.Environment.Runtime.BuiltFramework.Identifier}_{Context.Environment.Runtime.BuiltFramework.Version}"
    });

    gitHubActionsProviderTask
        .IsDependentOn("Cake.Common.Build.GitHubActionsProvider.Commands.UploadArtifact.File")
        .IsDependentOn("Cake.Common.Build.GitHubActionsProvider.Commands.UploadArtifact.Directory")
        .IsDependentOn("Cake.Common.Build.GitHubActionsProvider.Commands.DownloadArtifact");
}

public class GitHubActionsData
{
    public FilePath AssemblyPath { get; set; }
    public string FileArtifactName { get; set; }
    public string DirectoryArtifactName { get; set; }
}