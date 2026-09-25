// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Cake.Core;
using Cake.Core.IO;

namespace Cake.Common.Build.GitHubActions.Commands.Azure;

/// <summary>
/// Requests a GitHub Actions OIDC JWT and exchanges it for an Azure AD access token,
/// or writes the OIDC JWT to a file for Azure.Identity <c>WorkloadIdentityCredential</c>.
/// </summary>
internal sealed class GitHubAzureLoginService
{
    private const string OidcRequestTokenVariable = "ACTIONS_ID_TOKEN_REQUEST_TOKEN";
    private const string OidcRequestUrlVariable = "ACTIONS_ID_TOKEN_REQUEST_URL";
    private const string UserAgent = "Cake";
    private const string GrantType = "client_credentials";
    private const string ClientAssertionType = "urn:ietf:params:oauth:client-assertion-type:jwt-bearer";
    private const string DefaultTokenAuthority = "https://login.microsoftonline.com";

    private readonly ICakeEnvironment _environment;
    private readonly IFileSystem _fileSystem;
    private readonly Func<string, HttpClient> _createHttpClient;
    private readonly Action<string> _maskSecret;

    /// <summary>
    /// Initializes a new instance of the <see cref="GitHubAzureLoginService"/> class.
    /// </summary>
    /// <param name="environment">The Cake environment.</param>
    /// <param name="fileSystem">The file system.</param>
    /// <param name="createHttpClient">A factory that creates an <see cref="HttpClient"/> for the given logical operation name.</param>
    /// <param name="maskSecret">A callback that masks sensitive values in the build log.</param>
    public GitHubAzureLoginService(
        ICakeEnvironment environment,
        IFileSystem fileSystem,
        Func<string, HttpClient> createHttpClient,
        Action<string> maskSecret)
    {
        _environment = environment ?? throw new ArgumentNullException(nameof(environment));
        _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
        _createHttpClient = createHttpClient ?? throw new ArgumentNullException(nameof(createHttpClient));
        _maskSecret = maskSecret ?? throw new ArgumentNullException(nameof(maskSecret));
    }

    /// <summary>
    /// Obtains an Azure AD access token using the current GitHub Actions OIDC environment and the given settings.
    /// </summary>
    /// <param name="settings">Tenant, client, scope, audience, and optional authority.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The access token string (Bearer).</returns>
    public async Task<string> LoginAsync(GitHubAzureLoginSettings settings, CancellationToken cancellationToken)
    {
        ValidateSettings(settings, requireScope: true);

        var oidcJwt = await GetGitHubOidcJwtAsync(settings, cancellationToken).ConfigureAwait(false);

        var authority = string.IsNullOrWhiteSpace(settings.TokenAuthority)
            ? DefaultTokenAuthority
            : settings.TokenAuthority.TrimEnd('/');
        var tokenEndpoint = $"{authority}/{settings.TenantId}/oauth2/v2.0/token";

        var accessToken = await ExchangeForAzureAccessTokenAsync(
            tokenEndpoint,
            settings.ClientId,
            settings.Scope,
            oidcJwt,
            cancellationToken).ConfigureAwait(false);

        MaskSecret(accessToken);

        return accessToken;
    }

    /// <summary>
    /// Fetches the GitHub OIDC JWT and writes it to a file so child processes can use
    /// <c>WorkloadIdentityCredential</c> (with <c>AZURE_FEDERATED_TOKEN_FILE</c> and related env vars).
    /// </summary>
    /// <param name="settings">Tenant, client, and OIDC audience; scope and token authority are ignored.</param>
    /// <param name="federatedTokenFile">
    /// Destination file path. If <see langword="null"/>, a unique file name under
    /// <see cref="ICakeEnvironment.GetSpecialPath(SpecialPath)"/> with <see cref="SpecialPath.LocalTemp"/> is used.
    /// </param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>Tenant ID, client ID, and path to the federated token file.</returns>
    public async Task<GitHubAzureWorkloadIdentityInfo> PrepareWorkloadIdentityAsync(
        GitHubAzureLoginSettings settings,
        FilePath federatedTokenFile,
        CancellationToken cancellationToken)
    {
        ValidateSettings(settings, requireScope: false);

        var oidcJwt = await GetGitHubOidcJwtAsync(settings, cancellationToken).ConfigureAwait(false);

        var outputPath = federatedTokenFile ?? GetDefaultFederatedTokenFilePath();

        EnsureParentDirectoryExists(outputPath);
        await WriteFederatedTokenFileAsync(outputPath, oidcJwt, cancellationToken).ConfigureAwait(false);

        return new GitHubAzureWorkloadIdentityInfo(settings.TenantId, settings.ClientId, outputPath);
    }

    private static void ValidateSettings(GitHubAzureLoginSettings settings, bool requireScope)
    {
        ArgumentNullException.ThrowIfNull(settings);
        ArgumentException.ThrowIfNullOrWhiteSpace(settings.TenantId, "tenantId");
        ArgumentException.ThrowIfNullOrWhiteSpace(settings.ClientId, "clientId");
        ArgumentException.ThrowIfNullOrWhiteSpace(settings.Audience, "audience");

        if (requireScope)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(settings.Scope, "scope");
        }
    }

    private FilePath GetDefaultFederatedTokenFilePath()
    {
        var baseDirectory = _environment.GetSpecialPath(SpecialPath.LocalTemp);
        var id = Guid.CreateVersion7().ToString("n");
        return baseDirectory.CombineWithFilePath(new FilePath($"cake-azure-fed-{id}.jwt"));
    }

    private async Task<string> GetGitHubOidcJwtAsync(GitHubAzureLoginSettings settings, CancellationToken cancellationToken)
    {
        var oidcRequestToken = _environment.GetEnvironmentVariable(OidcRequestTokenVariable);
        var oidcRequestUrl = _environment.GetEnvironmentVariable(OidcRequestUrlVariable);

        if (string.IsNullOrWhiteSpace(oidcRequestToken) || string.IsNullOrWhiteSpace(oidcRequestUrl))
        {
            throw new CakeException("Missing GitHub OIDC request environment variables.");
        }

        MaskSecret(oidcRequestToken);

        var tokenRequestUri = BuildOidcTokenRequestUri(oidcRequestUrl, settings.Audience);
        var oidcJwt = await RequestGitHubOidcTokenAsync(oidcRequestToken, tokenRequestUri, cancellationToken).ConfigureAwait(false);

        MaskSecret(oidcJwt);

        return oidcJwt;
    }

    private void EnsureParentDirectoryExists(FilePath filePath)
    {
        var parent = filePath.GetDirectory();
        if (parent is null)
        {
            return;
        }

        _fileSystem.GetDirectory(parent).Create();
    }

    private async Task WriteFederatedTokenFileAsync(FilePath path, string oidcJwt, CancellationToken cancellationToken)
    {
        var file = _fileSystem.GetFile(path);
        await using var stream = file.Open(FileMode.Create, FileAccess.Write, FileShare.None);
        var bytes = Encoding.UTF8.GetBytes(oidcJwt);
        await stream.WriteAsync(bytes.AsMemory(0, bytes.Length), cancellationToken).ConfigureAwait(false);
    }

    private static Uri BuildOidcTokenRequestUri(string oidcRequestUrl, string audience)
    {
        var separator = oidcRequestUrl.Contains('?', StringComparison.Ordinal) ? '&' : '?';
        var raw = $"{oidcRequestUrl}{separator}audience={Uri.EscapeDataString(audience)}";
        return new Uri(raw, UriKind.Absolute);
    }

    private async Task<string> RequestGitHubOidcTokenAsync(string oidcRequestToken, Uri tokenUrl, CancellationToken cancellationToken)
    {
        using var httpClient = _createHttpClient(nameof(RequestGitHubOidcTokenAsync));
        using var request = new HttpRequestMessage(HttpMethod.Get, tokenUrl);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", oidcRequestToken);

        using var response = await httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);

        if (!response.IsSuccessStatusCode)
        {
            var body = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
            throw new CakeException(
                $"Failed to retrieve OIDC token from GitHub ({(int)response.StatusCode} {response.StatusCode}): {body}");
        }

        await using var stream = await response.Content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false);
        var tokenResponse = await JsonSerializer.DeserializeAsync(
            stream,
            GitHubAzureLoginJsonContext.Default.GitHubOidcTokenResponse,
            cancellationToken).ConfigureAwait(false);

        if (tokenResponse is null || string.IsNullOrEmpty(tokenResponse.Value))
        {
            throw new CakeException("Failed to retrieve OIDC token from GitHub.");
        }

        return tokenResponse.Value;
    }

    private async Task<string> ExchangeForAzureAccessTokenAsync(
        string tokenEndpoint,
        string clientId,
        string scope,
        string oidcJwt,
        CancellationToken cancellationToken)
    {
        using var content = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["client_id"] = clientId,
            ["scope"] = scope,
            ["grant_type"] = GrantType,
            ["client_assertion_type"] = ClientAssertionType,
            ["client_assertion"] = oidcJwt
        });

        using var httpClient = _createHttpClient(nameof(ExchangeForAzureAccessTokenAsync));
        using var request = new HttpRequestMessage(HttpMethod.Post, tokenEndpoint) { Content = content };
        request.Headers.UserAgent.ParseAdd(UserAgent);

        using var response = await httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);

        if (!response.IsSuccessStatusCode)
        {
            var errorBody = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
            var message = BuildAzureTokenErrorMessage(response.StatusCode, errorBody);
            throw new CakeException(message);
        }

        await using var stream = await response.Content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false);
        var tokenPayload = await JsonSerializer.DeserializeAsync(
            stream,
            GitHubAzureLoginJsonContext.Default.AzureOAuthTokenResponse,
            cancellationToken).ConfigureAwait(false);

        if (string.IsNullOrWhiteSpace(tokenPayload?.AccessToken))
        {
            throw new CakeException("Azure token response did not contain \"access_token\".");
        }

        return tokenPayload.AccessToken;
    }

    private static string BuildAzureTokenErrorMessage(HttpStatusCode statusCode, string errorBody)
    {
        var errorMessage = $"Azure token request failed ({(int)statusCode} {statusCode})";

        try
        {
            var errorJson = JsonSerializer.Deserialize(
                errorBody,
                GitHubAzureLoginJsonContext.Default.AzureOAuthErrorResponse);

            if (!string.IsNullOrEmpty(errorJson?.Error))
            {
                var detail = string.IsNullOrEmpty(errorJson.ErrorDescription)
                    ? errorJson.Error
                    : $"{errorJson.Error}: {errorJson.ErrorDescription}";
                return $"{errorMessage}: {detail}";
            }
        }
        catch (JsonException)
        {
            // Fall through to raw body
        }

        return $"{errorMessage}: {errorBody}";
    }

    private void MaskSecret(string secret)
    {
        if (!string.IsNullOrEmpty(secret))
        {
            _maskSecret(secret);
        }
    }
}
