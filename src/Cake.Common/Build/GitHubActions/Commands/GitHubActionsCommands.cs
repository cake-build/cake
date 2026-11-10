// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Cake.Common.Build.GitHubActions.Commands.Artifact;
using Cake.Common.Build.GitHubActions.Commands.Azure;
using Cake.Common.Build.GitHubActions.Commands.NuGet;
using Cake.Common.Build.GitHubActions.Data;
using Cake.Core;
using Cake.Core.IO;

namespace Cake.Common.Build.GitHubActions.Commands;

/// <summary>
/// Provides GitHub Actions commands for a current build.
/// </summary>
public sealed class GitHubActionsCommands
{
    private readonly ICakeEnvironment _environment;
    private readonly IFileSystem _fileSystem;
    private readonly IBuildSystemServiceMessageWriter _writer;
    private readonly GitHubActionsEnvironmentInfo _actionsEnvironment;
    private readonly GitHubActionsArtifactService _artifactsService;
    private readonly GitHubNuGetLoginService _nuGetLoginService;
    private readonly GitHubAzureLoginService _azureLoginService;

    /// <summary>
    /// Initializes a new instance of the <see cref="GitHubActionsCommands"/> class.
    /// </summary>
    /// <param name="environment">The environment.</param>
    /// <param name="fileSystem">The file system.</param>
    /// <param name="writer">The build system service message writer.</param>
    /// <param name="actionsEnvironment">The actions environment.</param>
    /// <param name="createHttpClient">The http client factory.</param>
    public GitHubActionsCommands(
        ICakeEnvironment environment,
        IFileSystem fileSystem,
        IBuildSystemServiceMessageWriter writer,
        GitHubActionsEnvironmentInfo actionsEnvironment,
        Func<string, HttpClient> createHttpClient)
    {
        _environment = environment ?? throw new ArgumentNullException(nameof(environment));
        _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
        _writer = writer ?? throw new ArgumentNullException(nameof(writer));
        _actionsEnvironment = actionsEnvironment ?? throw new ArgumentNullException(nameof(actionsEnvironment));
        // Internal service class, keeping public API unchanged,
        // introduced in pr https://github.com/cake-build/cake/pull/4350
        var createClient = createHttpClient ?? throw new ArgumentNullException(nameof(createHttpClient));
        _artifactsService = new GitHubActionsArtifactService(environment, fileSystem, actionsEnvironment, createClient);
        _nuGetLoginService = new GitHubNuGetLoginService(environment, createClient, SetSecret);
        _azureLoginService = new GitHubAzureLoginService(environment, fileSystem, createClient, SetSecret);
    }

    /// <summary>
    /// Write debug message to the build log.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <example>
    /// <code>
    /// if (GitHubActions.IsRunningOnGitHubActions)
    /// {
    ///     GitHubActions.Commands.Debug("This is a debug message");
    /// }
    /// </code>
    /// </example>
    public void Debug(string message)
    {
        WriteCommand("debug", message);
    }

    /// <summary>
    /// Write notice message to the build log.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="annotation">The annotation.</param>
    /// <example>
    /// <code>
    /// if (GitHubActions.IsRunningOnGitHubActions)
    /// {
    ///     GitHubActions.Commands.Notice("This is a notice message");
    /// }
    /// </code>
    /// </example>
    public void Notice(string message, GitHubActionsAnnotation annotation = null)
    {
        WriteCommand("notice", annotation?.GetParameters(), message);
    }

    /// <summary>
    /// Write warning message to the build log.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="annotation">The annotation.</param>
    /// <example>
    /// <code>
    /// if (GitHubActions.IsRunningOnGitHubActions)
    /// {
    ///     GitHubActions.Commands.Warning("This is a warning message");
    /// }
    /// </code>
    /// </example>
    public void Warning(string message, GitHubActionsAnnotation annotation = null)
    {
        WriteCommand("warning", annotation?.GetParameters(), message);
    }

    /// <summary>
    /// Write error message to the build log.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="annotation">The annotation.</param>
    /// <example>
    /// <code>
    /// if (GitHubActions.IsRunningOnGitHubActions)
    /// {
    ///     GitHubActions.Commands.Error("This is an error message");
    /// }
    /// </code>
    /// </example>
    public void Error(string message, GitHubActionsAnnotation annotation = null)
    {
        WriteCommand("error", annotation?.GetParameters(), message);
    }

    /// <summary>
    /// Start a group in the build log.
    /// </summary>
    /// <param name="title">The title.</param>
    /// <example>
    /// <code>
    /// if (GitHubActions.IsRunningOnGitHubActions)
    /// {
    ///     GitHubActions.Commands.StartGroup("Cake group");
    ///     Information("This is inside a group");
    ///     GitHubActions.Commands.EndGroup();
    /// }
    /// </code>
    /// </example>
    public void StartGroup(string title)
    {
        WriteCommand("group", title);
    }

    /// <summary>
    /// End a group in the build log.
    /// </summary>
    /// <example>
    /// <code>
    /// if (GitHubActions.IsRunningOnGitHubActions)
    /// {
    ///     GitHubActions.Commands.EndGroup();
    /// }
    /// </code>
    /// </example>
    public void EndGroup()
    {
        WriteCommand("endgroup");
    }

    /// <summary>
    /// Registers a secret which will get masked in the build log.
    /// </summary>
    /// <param name="secret">The secret.</param>
    /// <example>
    /// <code>
    /// if (GitHubActions.IsRunningOnGitHubActions)
    /// {
    ///     GitHubActions.Commands.SetSecret(Guid.NewGuid().ToString());
    /// }
    /// </code>
    /// </example>
    public void SetSecret(string secret)
    {
        WriteCommand("add-mask", secret);
    }

    /// <summary>
    /// Prepends a directory to the system PATH variable and automatically makes it available to all subsequent actions in the current job.
    /// </summary>
    /// <param name="path">The directory path.</param>
    /// <example>
    /// <code>
    /// if (GitHubActions.IsRunningOnGitHubActions)
    /// {
    ///     GitHubActions.Commands.AddPath(toolsPath);
    /// }
    /// </code>
    /// </example>
    public void AddPath(DirectoryPath path)
    {
        ArgumentNullException.ThrowIfNull(path);

        if (_actionsEnvironment.Runtime.SystemPath == null)
        {
            throw new CakeException("GitHub Actions Runtime SystemPath missing.");
        }

        var file = _fileSystem.GetFile(_actionsEnvironment.Runtime.SystemPath);
        using var stream = file.Open(FileMode.Append, FileAccess.Write, FileShare.None);
        using var writer = new StreamWriter(stream);
        writer.WriteLine(path.MakeAbsolute(_environment).FullPath);
    }

    /// <summary>
    /// Creates or updates an environment variable for any steps running next in a job.
    /// </summary>
    /// <param name="key">The key.</param>
    /// <param name="value">The Value.</param>
    /// <example>
    /// <code>
    /// if (GitHubActions.IsRunningOnGitHubActions)
    /// {
    ///     GitHubActions.Commands.SetEnvironmentVariable(
    ///         "CAKE_VERSION",
    ///         Context.Environment.Runtime.CakeVersion.ToString(3));
    /// }
    /// </code>
    /// </example>
    public void SetEnvironmentVariable(string key, string value)
    {
        if (string.IsNullOrEmpty(key))
        {
            throw new ArgumentNullException(nameof(key));
        }

        ArgumentNullException.ThrowIfNull(value);

        if (_actionsEnvironment.Runtime.EnvPath == null)
        {
            throw new CakeException("GitHub Actions Runtime EnvPath missing.");
        }

        var file = _fileSystem.GetFile(_actionsEnvironment.Runtime.EnvPath);
        using var stream = file.Open(FileMode.Append, FileAccess.Write, FileShare.None);
        using var writer = new StreamWriter(stream);
        writer.Write(key);
        writer.WriteLine("<<CAKEEOF");
        writer.WriteLine(value);
        writer.WriteLine("CAKEEOF");
    }

    /// <summary>
    /// Creates or updates an output parameter for any steps running next in a job.
    /// </summary>
    /// <param name="key">The key.</param>
    /// <param name="value">The Value.</param>
    /// <example>
    /// <code>
    /// if (GitHubActions.IsRunningOnGitHubActions)
    /// {
    ///     GitHubActions.Commands.SetOutputParameter(
    ///         "CAKE_VERSION_OS",
    ///         string.Join(
    ///             '_',
    ///             Context.Environment.Runtime.CakeVersion.ToString(3),
    ///             GitHubActions.Environment.Runner.OS,
    ///             GitHubActions.Environment.Runner.Architecture));
    /// }
    /// </code>
    /// </example>
    public void SetOutputParameter(string key, string value)
    {
        if (string.IsNullOrEmpty(key))
        {
            throw new ArgumentNullException(nameof(key));
        }

        ArgumentNullException.ThrowIfNull(value);

        if (_actionsEnvironment.Runtime.OutputPath == null)
        {
            throw new CakeException("GitHub Actions Runtime OutputPath missing.");
        }

        var file = _fileSystem.GetFile(_actionsEnvironment.Runtime.OutputPath);
        using var stream = file.Open(FileMode.Append, FileAccess.Write, FileShare.None);
        using var writer = new StreamWriter(stream);
        writer.Write(key);
        writer.Write('=');
        writer.WriteLine(value);
    }

    /// <summary>
    /// Creates or updates the step summary for a GitHub workflow.
    /// </summary>
    /// <param name="summary">The step summary.</param>
    /// <example>
    /// <code>
    /// if (GitHubActions.IsRunningOnGitHubActions)
    /// {
    ///     GitHubActions.Commands.SetStepSummary(
    ///         $"## Cake Version\n{Context.Environment.Runtime.CakeVersion.ToString(3)}");
    /// }
    /// </code>
    /// </example>
    public void SetStepSummary(string summary)
    {
        if (string.IsNullOrEmpty(summary))
        {
            throw new ArgumentNullException(nameof(summary));
        }

        if (_actionsEnvironment.Runtime.StepSummary == null)
        {
            throw new CakeException("GitHub Actions Runtime StepSummary missing.");
        }

        var file = _fileSystem.GetFile(_actionsEnvironment.Runtime.StepSummary);
        using var stream = file.Open(FileMode.Append, FileAccess.Write, FileShare.None);
        using var writer = new StreamWriter(stream);
        writer.WriteLine(summary);
    }

    /// <summary>
    /// Upload local file into a file container folder, and create an artifact.
    /// </summary>
    /// <param name="path">Path to the local file.</param>
    /// <param name="artifactName">The artifact name.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    /// <example>
    /// <code>
    /// if (GitHubActions.IsRunningOnGitHubActions)
    /// {
    ///     await GitHubActions.Commands.UploadArtifact(artifactPath, "my-artifact");
    /// }
    /// </code>
    /// </example>
    public async Task UploadArtifact(FilePath path, string artifactName)
    {
        var file = _fileSystem.GetFile(ValidateArtifactParameters(path, artifactName));

        if (!file.Exists)
        {
            throw new FileNotFoundException("Artifact file not found.", file.Path.FullPath);
        }

        await _artifactsService.CreateAndUploadArtifactFiles(artifactName, file.Path.GetDirectory(), file);
    }

    /// <summary>
    /// Upload local directory files into a file container folder, and create an artifact.
    /// </summary>
    /// <param name="path">Path to the local directory.</param>
    /// <param name="artifactName">The artifact name.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    /// <example>
    /// <code>
    /// if (GitHubActions.IsRunningOnGitHubActions)
    /// {
    ///     await GitHubActions.Commands.UploadArtifact(artifactDirectory, "my-artifact");
    /// }
    /// </code>
    /// </example>
    public async Task UploadArtifact(DirectoryPath path, string artifactName)
    {
        var directory = _fileSystem.GetDirectory(ValidateArtifactParameters(path, artifactName));

        if (!directory.Exists)
        {
            throw new DirectoryNotFoundException(FormattableString.Invariant($"Artifact directory {directory.Path.FullPath} not found."));
        }

        var files = directory
                        .GetFiles("*", SearchScope.Recursive)
                        .ToArray();

        await _artifactsService.CreateAndUploadArtifactFiles(artifactName, directory.Path, files);
    }

    /// <summary>
    /// Download remote artifact container into local directory.
    /// </summary>
    /// <param name="artifactName">The artifact name.</param>
    /// <param name="path">Path to the local directory.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    /// <example>
    /// <code>
    /// if (GitHubActions.IsRunningOnGitHubActions)
    /// {
    ///     await GitHubActions.Commands.DownloadArtifact("cake-integration-tests", targetPath);
    /// }
    /// </code>
    /// </example>
    public async Task DownloadArtifact(string artifactName, DirectoryPath path)
    {
        var directory = _fileSystem.GetDirectory(ValidateArtifactParameters(path, artifactName));

        if (!directory.Exists)
        {
            throw new DirectoryNotFoundException(FormattableString.Invariant($"Local directory {directory.Path.FullPath} not found."));
        }

        await _artifactsService.DownloadArtifactFiles(artifactName, directory.Path);
    }

    /// <summary>
    /// Exchanges a GitHub Actions OIDC identity token for a short-lived NuGet.org API key (trusted publishing).
    /// Default token service URL is <c>https://www.nuget.org/api/v2/token</c> and default OIDC audience is <c>https://www.nuget.org</c>.
    /// Requires <c>permissions: id-token: write</c> in the GitHub Actions workflow.
    /// OIDC tokens and the returned API key are automatically masked in the build log.
    /// </summary>
    /// <example>
    /// <code>
    /// var nuGetUserName = EnvironmentVariable("NUGET_USERNAME");
    /// var apiKey = await GitHubActions.Commands.NuGetLogin(nuGetUserName);
    ///
    /// var settings = new DotNetNuGetPushSettings
    /// {
    ///     ApiKey = apiKey,
    ///     Source = "https://api.nuget.org/v3/index.json"
    /// };
    ///
    /// foreach (var packageFilePath in GetFiles("./*.nupkg"))
    /// {
    ///     DotNetNuGetPush(packageFilePath, settings);
    /// }
    /// </code>
    /// </example>
    /// <param name="userName">The NuGet.org account user name.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The NuGet API key.</returns>
    public Task<string> NuGetLogin(string userName, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(userName);

        return NuGetLogin(new GitHubNuGetLoginSettings(userName), cancellationToken);
    }

    /// <summary>
    /// Exchanges a GitHub Actions OIDC identity token for a NuGet API key using the specified token service URL and audience.
    /// Requires <c>permissions: id-token: write</c> in the GitHub Actions workflow.
    /// OIDC tokens and the returned API key are automatically masked in the build log.
    /// </summary>
    /// <example>
    /// <code>
    /// var loginSettings = new GitHubNuGetLoginSettings(
    ///     userName: "myorg",
    ///     tokenServiceUrl: "https://www.nuget.org/api/v2/token",
    ///     audience: "https://www.nuget.org");
    ///
    /// var apiKey = await GitHubActions.Commands.NuGetLogin(loginSettings);
    ///
    /// var settings = new DotNetNuGetPushSettings
    /// {
    ///     ApiKey = apiKey,
    ///     Source = EnvironmentVariable("NUGET_API_URL")
    /// };
    ///
    /// foreach (var packageFilePath in GetFiles("./*.nupkg"))
    /// {
    ///     DotNetNuGetPush(packageFilePath, settings);
    /// }
    /// </code>
    /// </example>
    /// <param name="settings">The user name, token service URL, and OIDC audience.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The NuGet API key.</returns>
    public Task<string> NuGetLogin(GitHubNuGetLoginSettings settings, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(settings);

        return _nuGetLoginService.LoginAsync(settings, cancellationToken);
    }

    /// <summary>
    /// Exchanges a GitHub Actions OIDC identity token for an Azure AD access token (workload identity federation).
    /// Default scope is <c>https://management.azure.com/.default</c>, default OIDC audience is <c>api://AzureADTokenExchange</c>,
    /// and default token authority is <c>https://login.microsoftonline.com</c>.
    /// Requires <c>permissions: id-token: write</c> in the GitHub Actions workflow.
    /// OIDC tokens and the returned access token are automatically masked in the build log.
    /// </summary>
    /// <example>
    /// <code>
    /// var tenantId = EnvironmentVariable("AZURE_TENANT_ID");
    /// var clientId = EnvironmentVariable("AZURE_CLIENT_ID");
    /// var accessToken = await GitHubActions.Commands.AzureLogin(tenantId, clientId);
    /// </code>
    /// </example>
    /// <param name="tenantId">The Entra ID tenant ID.</param>
    /// <param name="clientId">The application (client) ID.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The Azure AD access token.</returns>
    public Task<string> AzureLogin(string tenantId, string clientId, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(tenantId);
        ArgumentException.ThrowIfNullOrWhiteSpace(clientId);

        return AzureLogin(new GitHubAzureLoginSettings(tenantId, clientId), cancellationToken);
    }

    /// <summary>
    /// Exchanges a GitHub Actions OIDC identity token for an Azure AD access token using the specified scope, audience, and authority.
    /// Requires <c>permissions: id-token: write</c> in the GitHub Actions workflow.
    /// OIDC tokens and the returned access token are automatically masked in the build log.
    /// </summary>
    /// <example>
    /// <code>
    /// var loginSettings = new GitHubAzureLoginSettings(
    ///     tenantId: EnvironmentVariable("AZURE_TENANT_ID"),
    ///     clientId: EnvironmentVariable("AZURE_CLIENT_ID"),
    ///     scope: "https://management.azure.com/.default",
    ///     audience: "api://AzureADTokenExchange",
    ///     tokenAuthority: "https://login.microsoftonline.com");
    ///
    /// var accessToken = await GitHubActions.Commands.AzureLogin(loginSettings);
    /// </code>
    /// </example>
    /// <param name="settings">The tenant, client, scope, OIDC audience, and optional token authority.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The Azure AD access token.</returns>
    public Task<string> AzureLogin(GitHubAzureLoginSettings settings, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(settings);

        return _azureLoginService.LoginAsync(settings, cancellationToken);
    }

    /// <summary>
    /// Fetches the GitHub Actions OIDC JWT and writes it to a file for Azure.Identity <c>WorkloadIdentityCredential</c>.
    /// Default OIDC audience is <c>api://AzureADTokenExchange</c>.
    /// Requires <c>permissions: id-token: write</c> in the GitHub Actions workflow.
    /// The OIDC JWT is automatically masked in the build log.
    /// </summary>
    /// <example>
    /// <code>
    /// var tenantId = EnvironmentVariable("AZURE_TENANT_ID");
    /// var clientId = EnvironmentVariable("AZURE_CLIENT_ID");
    /// var workload = await GitHubActions.Commands.AzurePrepareWorkloadIdentity(tenantId, clientId);
    ///
    /// InstallTool("dotnet:?package=ARI&amp;version=2026.9.9.1110");
    /// Command(
    ///     new CommandSettings
    ///     {
    ///         ToolName = "ari",
    ///         ToolExecutableNames = ["ari", "ari.exe"],
    ///         EnvironmentVariables =
    ///         {
    ///             { "AZURE_TENANT_ID", workload.TenantId },
    ///             { "AZURE_CLIENT_ID", workload.ClientId },
    ///             { "AZURE_FEDERATED_TOKEN_FILE", workload.FederatedTokenFile.FullPath }
    ///         }
    ///     },
    ///     new ProcessArgumentBuilder()
    ///         .Append("inventory")
    ///         .AppendQuotedSecret(workload.TenantId)
    ///         .AppendQuoted("./artifacts/inventory")
    ///         .Append("--skip-tenant-overview")
    ///         .Append("--include-site-application-settings"));
    /// </code>
    /// </example>
    /// <param name="tenantId">The Entra ID tenant ID.</param>
    /// <param name="clientId">The application (client) ID.</param>
    /// <param name="federatedTokenFile">Optional destination path; default is a unique file under the local temp directory.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>Tenant ID, client ID, and federated token file path.</returns>
    public Task<GitHubAzureWorkloadIdentityInfo> AzurePrepareWorkloadIdentity(
        string tenantId,
        string clientId,
        FilePath federatedTokenFile = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(tenantId);
        ArgumentException.ThrowIfNullOrWhiteSpace(clientId);

        return AzurePrepareWorkloadIdentity(new GitHubAzureLoginSettings(tenantId, clientId), federatedTokenFile, cancellationToken);
    }

    /// <summary>
    /// Fetches the GitHub Actions OIDC JWT and writes it to a file for Azure.Identity <c>WorkloadIdentityCredential</c>.
    /// Requires <c>permissions: id-token: write</c> in the GitHub Actions workflow.
    /// The OIDC JWT is automatically masked in the build log.
    /// </summary>
    /// <example>
    /// <code>
    /// var loginSettings = new GitHubAzureLoginSettings(
    ///     tenantId: EnvironmentVariable("AZURE_TENANT_ID"),
    ///     clientId: EnvironmentVariable("AZURE_CLIENT_ID"));
    ///
    /// var workload = await GitHubActions.Commands.AzurePrepareWorkloadIdentity(loginSettings);
    ///
    /// InstallTool("dotnet:?package=ARI&amp;version=2026.9.9.1110");
    /// Command(
    ///     new CommandSettings
    ///     {
    ///         ToolName = "ari",
    ///         ToolExecutableNames = ["ari", "ari.exe"],
    ///         EnvironmentVariables =
    ///         {
    ///             { "AZURE_TENANT_ID", workload.TenantId },
    ///             { "AZURE_CLIENT_ID", workload.ClientId },
    ///             { "AZURE_FEDERATED_TOKEN_FILE", workload.FederatedTokenFile.FullPath }
    ///         }
    ///     },
    ///     new ProcessArgumentBuilder()
    ///         .Append("inventory")
    ///         .AppendQuotedSecret(workload.TenantId)
    ///         .AppendQuoted("./artifacts/inventory")
    ///         .Append("--skip-tenant-overview")
    ///         .Append("--include-site-application-settings"));
    /// </code>
    /// </example>
    /// <param name="settings">The tenant, client, and OIDC audience; scope and token authority are ignored.</param>
    /// <param name="federatedTokenFile">Optional destination path; default is a unique file under the local temp directory.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>Tenant ID, client ID, and federated token file path.</returns>
    public Task<GitHubAzureWorkloadIdentityInfo> AzurePrepareWorkloadIdentity(
        GitHubAzureLoginSettings settings,
        FilePath federatedTokenFile = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(settings);

        return _azureLoginService.PrepareWorkloadIdentityAsync(settings, federatedTokenFile, cancellationToken);
    }

    internal void WriteCommand(string command, string message = null)
    {
        WriteCommand(command, new Dictionary<string, string>(), message);
    }

    internal void WriteCommand(string command, Dictionary<string, string> parameters, string message)
    {
        var parameterString = parameters?.Count > 0 ? string.Concat(" ", string.Join(',', parameters.Select(pair => $"{pair.Key}={EscapeCommandParameter(pair.Value)}"))) : string.Empty;

        _writer.Write("::{0}{1}::{2}", command, parameterString, EscapeCommandMessage(message));
    }

    private static string EscapeCommandMessage(string value) => (value ?? string.Empty).Replace("%", "%25").Replace("\r", "%0D").Replace("\n", "%0A");

    private static string EscapeCommandParameter(string value) => (value ?? string.Empty).Replace("%", "%25").Replace("\r", "%0D").Replace("\n", "%0A").Replace(":", "%3A").Replace(",", "%2C");

    private T ValidateArtifactParameters<T>(T path, string artifactName) where T : IPath<T>
    {
        if (path is null)
        {
            throw new ArgumentNullException(nameof(path));
        }

        if (string.IsNullOrWhiteSpace(artifactName))
        {
            throw new ArgumentNullException(nameof(artifactName));
        }

        if (string.IsNullOrWhiteSpace(_actionsEnvironment.Runtime.Token))
        {
            throw new CakeException("GitHub Actions Runtime Token missing.");
        }

        if (string.IsNullOrWhiteSpace(_actionsEnvironment.Runtime.Url))
        {
            throw new CakeException("GitHub Actions Runtime Url missing.");
        }

        if (string.IsNullOrWhiteSpace(_actionsEnvironment.Workflow.RunId))
        {
            throw new CakeException("GitHub Actions Workflow RunId missing.");
        }

        return path.MakeAbsolute(_environment);
    }
}
