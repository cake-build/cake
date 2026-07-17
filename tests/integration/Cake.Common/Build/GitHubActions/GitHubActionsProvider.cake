#load "./../../../utilities/xunit.cake"
#load "./../../../utilities/paths.cake"
#load "./../../../utilities/io.cake"

using System.Net;
using System.Net.Http;
using System.Text.Json;

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
                                GitHubActions.Environment.Runner.OS,
                                GitHubActions.Environment.Runner.Architecture)
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
{GitHubActions.Environment.Runner.OS}

## Runner Environment
{GitHubActions.Environment.Runner.Environment}

## Runner Architecture
{GitHubActions.Environment.Runner.Architecture}

## Runner Debug
{GitHubActions.Environment.Runner.IsDebug}

## Action Repository
{GitHubActions.Environment.Workflow.ActionRepository}

## Actor / Triggering Actor
{GitHubActions.Environment.Workflow.Actor} / {GitHubActions.Environment.Workflow.TriggeringActor}

## Repository
{GitHubActions.Environment.Workflow.Repository} ({GitHubActions.Environment.Workflow.RepositoryId})

## Workflow Ref / SHA
{GitHubActions.Environment.Workflow.WorkflowRef}

{GitHubActions.Environment.Workflow.WorkflowSha}

## Retention Days
{GitHubActions.Environment.Workflow.RetentionDays}";

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

Task("Cake.Common.Build.GitHubActionsProvider.Commands.DownloadArtifact.PreviousJob")
    .Does(async () => {
        // Given
        var targetPath = Paths.Temp.Combine("./Cake.Common.Build.GitHubActionsProvider.Commands.DownloadArtifact.PreviousJob");
        EnsureDirectoryExists(targetPath);
        var targetArtifactPath = targetPath.CombineWithFilePath("cake-integration-tests.txt");

        // When
        await GitHubActions.Commands.DownloadArtifact("cake-integration-tests", targetPath);

        // Then
        Assert.True(System.IO.File.Exists(targetArtifactPath.FullPath), $"{targetArtifactPath.FullPath} Missing");
        Assert.Equal("Cake Integration Tests\n", System.IO.File.ReadAllText(targetArtifactPath.FullPath));
});

Task("Cake.Common.Build.GitHubActionsProvider.Commands.NuGetLogin")
    .WithCriteria<GitHubActionsData>((context, data) => !string.IsNullOrWhiteSpace(data.NuGetUserName))
    .Does<GitHubActionsData>(async data => {
        // When (trusted publishing: OIDC from ACTIONS_ID_TOKEN_REQUEST_* → NuGet API key)
        var apiKey = await GitHubActions.Commands.NuGetLogin(data.NuGetUserName);

        // Then
        Assert.True(!string.IsNullOrWhiteSpace(apiKey));

        if (!string.IsNullOrWhiteSpace(data.NuGetPackageId))
        {
            // Validate the API key against nuget.org using the verify-scope protocol:
            // https://learn.microsoft.com/en-us/nuget/api/nuget-protocols#api-to-request-a-verify-scope-key
            var expires = await NuGetVerifyScopeValidator.ValidateApiKeyAsync(apiKey, data.NuGetPackageId, data.NuGetPackageVersion);
            Information("NuGet verify-scope key expires: {0}", expires);
        }
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
    Setup<GitHubActionsData>(context => new GitHubActionsData {
        AssemblyPath = typeof(ICakeContext).GetTypeInfo().Assembly.Location,
        FileArtifactName = $"File_{GitHubActions.Environment.Runner.ImageOS ?? GitHubActions.Environment.Runner.OS}_{GitHubActions.Environment.Runner.Architecture}_{Context.Environment.Runtime.BuiltFramework.Identifier}_{Context.Environment.Runtime.BuiltFramework.Version}",
        DirectoryArtifactName = $"Directory_{GitHubActions.Environment.Runner.ImageOS ?? GitHubActions.Environment.Runner.OS}_{GitHubActions.Environment.Runner.Architecture}_{Context.Environment.Runtime.BuiltFramework.Identifier}_{Context.Environment.Runtime.BuiltFramework.Version}",
        NuGetUserName = EnvironmentVariable("CAKE_INTEGRATIONTEST_NUGET_USERNAME") ?? string.Empty,
        NuGetPackageId = EnvironmentVariable("CAKE_INTEGRATIONTEST_NUGET_PACKAGE_ID") ?? string.Empty,
        NuGetPackageVersion = EnvironmentVariable("CAKE_INTEGRATIONTEST_NUGET_PACKAGE_VERSION") ?? string.Empty
    });

    gitHubActionsProviderTask
        .IsDependentOn("Cake.Common.Build.GitHubActionsProvider.Commands.UploadArtifact.File")
        .IsDependentOn("Cake.Common.Build.GitHubActionsProvider.Commands.UploadArtifact.Directory")
        .IsDependentOn("Cake.Common.Build.GitHubActionsProvider.Commands.DownloadArtifact")
        .IsDependentOn("Cake.Common.Build.GitHubActionsProvider.Commands.DownloadArtifact.PreviousJob")
        .IsDependentOn("Cake.Common.Build.GitHubActionsProvider.Commands.NuGetLogin");
}

public class GitHubActionsData
{
    public FilePath AssemblyPath { get; set; }
    public string FileArtifactName { get; set; }
    public string DirectoryArtifactName { get; set; }
    public string NuGetUserName { get; set; }
    public string NuGetPackageId { get; set; }
    public string NuGetPackageVersion { get; set; }
}

/// <summary>
/// Validates a NuGet.org API key using the verify-scope protocol (nuget.org protocol 4.1.0).
/// </summary>
public static class NuGetVerifyScopeValidator
{
    private const string NuGetOrgBaseUrl = "https://www.nuget.org";

    /// <summary>
    /// Requests a verify-scope key with the API key, then verifies it, proving the API key is accepted by nuget.org.
    /// </summary>
    /// <param name="apiKey">The NuGet API key to validate.</param>
    /// <param name="packageId">A package ID owned by the NuGet account.</param>
    /// <param name="packageVersion">An optional package version.</param>
    /// <returns>The verify-scope key expiration from the create-verification-key response.</returns>
    public static async System.Threading.Tasks.Task<string> ValidateApiKeyAsync(string apiKey, string packageId, string packageVersion = null)
    {
        using var client = new HttpClient();

        var createVerificationKeyUrl = BuildVerifyScopeUrl(
            "/api/v2/package/create-verification-key",
            packageId,
            packageVersion);

        using var createRequest = new HttpRequestMessage(HttpMethod.Post, createVerificationKeyUrl);
        createRequest.Headers.Add("X-NuGet-ApiKey", apiKey);

        using var createResponse = await client.SendAsync(createRequest);
        var createBody = await createResponse.Content.ReadAsStringAsync();

        Assert.True(
            createResponse.IsSuccessStatusCode,
            $"create-verification-key failed ({(int)createResponse.StatusCode} {createResponse.StatusCode}): {createBody}");

        using var createJson = JsonDocument.Parse(createBody);
        var verifyScopeKey = createJson.RootElement.GetProperty("Key").GetString();
        var expires = createJson.RootElement.GetProperty("Expires").GetString();

        Assert.False(string.IsNullOrWhiteSpace(verifyScopeKey), "create-verification-key response did not contain \"Key\".");
        Assert.False(string.IsNullOrWhiteSpace(expires), "create-verification-key response did not contain \"Expires\".");

        var verifyKeyUrl = BuildVerifyScopeUrl("/api/v2/verifykey", packageId, packageVersion);

        using var verifyRequest = new HttpRequestMessage(HttpMethod.Get, verifyKeyUrl);
        verifyRequest.Headers.Add("X-NuGet-ApiKey", verifyScopeKey);

        using var verifyResponse = await client.SendAsync(verifyRequest);

        Assert.Equal(HttpStatusCode.OK, verifyResponse.StatusCode);

        return expires;
    }

    private static string BuildVerifyScopeUrl(string pathPrefix, string packageId, string packageVersion)
    {
        var encodedId = Uri.EscapeDataString(packageId);

        if (string.IsNullOrWhiteSpace(packageVersion))
        {
            return $"{NuGetOrgBaseUrl}{pathPrefix}/{encodedId}";
        }

        return $"{NuGetOrgBaseUrl}{pathPrefix}/{encodedId}/{Uri.EscapeDataString(packageVersion)}";
    }
}

