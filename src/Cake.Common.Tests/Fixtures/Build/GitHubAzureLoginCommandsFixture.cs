// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Net;
using System.Text;
using System.Text.Json;
using Cake.Common.Build.GitHubActions.Commands;
using Cake.Common.Tests.Fakes;
using Cake.Core;
using Cake.Testing;
using NSubstitute;

namespace Cake.Common.Tests.Fixtures.Build;

/// <summary>
/// Provides a fake <see cref="HttpMessageHandler"/> and GitHub Actions environment for
/// <see cref="GitHubActionsCommands.AzureLogin(string, string, CancellationToken)"/> tests.
/// </summary>
internal class GitHubAzureLoginCommandsFixture : HttpMessageHandler
{
    /// <summary>
    /// Base OIDC request URL (without <c>audience</c>); matches GitHub Actions format.
    /// </summary>
    public const string OidcRequestUrl = "https://github.example/api/v2/token?foo=1";

    /// <summary>
    /// Default OIDC audience string (matches <see cref="Cake.Common.Build.GitHubActions.Commands.Azure.GitHubAzureLoginSettings"/> default).
    /// </summary>
    public const string DefaultAudience = "api://AzureADTokenExchange";

    /// <summary>
    /// Test tenant ID used for the token endpoint path.
    /// </summary>
    public const string TenantId = "11111111-1111-1111-1111-111111111111";

    /// <summary>
    /// Test application (client) ID.
    /// </summary>
    public const string ClientId = "22222222-2222-2222-2222-222222222222";

    /// <summary>
    /// Default Entra ID token authority.
    /// </summary>
    public const string DefaultTokenAuthority = "https://login.microsoftonline.com";

    /// <summary>
    /// Custom token authority used to exercise sovereign-cloud endpoints.
    /// </summary>
    public const string CustomTokenAuthority = "https://login.microsoftonline.us";

    /// <summary>
    /// Bearer token used for the GitHub OIDC request token environment variable.
    /// </summary>
    public const string ActionsIdTokenRequestToken = "actions-id-token-request-token";

    /// <summary>
    /// Expected full OIDC GET URI including <c>audience</c> query parameter for <see cref="DefaultAudience"/>.
    /// </summary>
    public static readonly string ExpectedOidcGetUriDefaultAudience = BuildExpectedOidcUri(OidcRequestUrl, DefaultAudience);

    /// <summary>
    /// Entra ID OAuth2 token endpoint for the test tenant (public cloud).
    /// </summary>
    public static readonly string DefaultTokenEndpoint =
        $"{DefaultTokenAuthority}/{TenantId}/oauth2/v2.0/token";

    /// <summary>
    /// Entra ID OAuth2 token endpoint for the custom authority.
    /// </summary>
    public static readonly string CustomTokenEndpoint =
        $"{CustomTokenAuthority}/{TenantId}/oauth2/v2.0/token";

    private readonly GitHubActionsInfoFixture _gitHubActionsInfoFixture;

    /// <summary>
    /// Initializes a new instance of the <see cref="GitHubAzureLoginCommandsFixture"/> class.
    /// </summary>
    public GitHubAzureLoginCommandsFixture()
    {
        _gitHubActionsInfoFixture = new GitHubActionsInfoFixture();
        FileSystem = new FakeFileSystem(_gitHubActionsInfoFixture.Environment);
        FileSystem.CreateDirectory("/opt");
        FileSystem.CreateDirectory("/tmp");
        Writer = new FakeBuildSystemServiceMessageWriter();
        Environment.GetEnvironmentVariable("ACTIONS_ID_TOKEN_REQUEST_TOKEN").Returns(ActionsIdTokenRequestToken);
        Environment.GetEnvironmentVariable("ACTIONS_ID_TOKEN_REQUEST_URL").Returns(OidcRequestUrl);
    }

    /// <summary>
    /// Gets the substituted Cake environment.
    /// </summary>
    public ICakeEnvironment Environment => _gitHubActionsInfoFixture.Environment;

    /// <summary>
    /// Gets the fake file system.
    /// </summary>
    public FakeFileSystem FileSystem { get; }

    /// <summary>
    /// Gets the fake workflow command writer.
    /// </summary>
    public FakeBuildSystemServiceMessageWriter Writer { get; }

    /// <summary>
    /// Creates <see cref="GitHubActionsCommands"/> wired to this handler.
    /// </summary>
    /// <returns>A new commands instance.</returns>
    public GitHubActionsCommands CreateGitHubActionsCommands()
    {
        return new GitHubActionsCommands(Environment, FileSystem, Writer, _gitHubActionsInfoFixture.CreateEnvironmentInfo(), _ => new HttpClient(this));
    }

    /// <inheritdoc />
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        return HandleAsync(request, cancellationToken);
    }

    /// <summary>
    /// Dispatches the request; override in derived fixtures for scenario-specific behavior.
    /// </summary>
    /// <param name="request">The HTTP request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The HTTP response.</returns>
    protected virtual Task<HttpResponseMessage> HandleAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (request.RequestUri is null)
        {
            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.BadRequest));
        }

        var absolute = request.RequestUri.AbsoluteUri;

        if (request.Method == HttpMethod.Get
            && absolute == ExpectedOidcGetUriDefaultAudience
            && request.Headers.Authorization?.Scheme == "Bearer"
            && request.Headers.Authorization.Parameter == ActionsIdTokenRequestToken)
        {
            var json = JsonSerializer.Serialize(new { value = "mock-oidc-jwt" });
            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            });
        }

        if (request.Method == HttpMethod.Post
            && absolute == DefaultTokenEndpoint
            && request.Headers.UserAgent.ToString().Contains("Cake", StringComparison.Ordinal))
        {
            return HandleAzureTokenPostAsync(request, cancellationToken, "expected-access-token");
        }

        if (request.Method == HttpMethod.Post
            && absolute == CustomTokenEndpoint
            && request.Headers.UserAgent.ToString().Contains("Cake", StringComparison.Ordinal))
        {
            return HandleAzureTokenPostAsync(request, cancellationToken, "expected-access-token-custom-authority");
        }

        return Task.FromResult(new HttpResponseMessage(HttpStatusCode.NotFound));
    }

    private static async Task<HttpResponseMessage> HandleAzureTokenPostAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken,
        string accessToken)
    {
        var form = await request.Content!.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
        if (!form.Contains("grant_type=client_credentials", StringComparison.Ordinal)
            || !form.Contains("client_assertion_type=urn%3Aietf%3Aparams%3Aoauth%3Aclient-assertion-type%3Ajwt-bearer", StringComparison.Ordinal)
            || !form.Contains("client_assertion=mock-oidc-jwt", StringComparison.Ordinal)
            || !form.Contains($"client_id={ClientId}", StringComparison.Ordinal))
        {
            return new HttpResponseMessage(HttpStatusCode.BadRequest);
        }

        return new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent($"{{\"access_token\":\"{accessToken}\"}}", Encoding.UTF8, "application/json")
        };
    }

    private static string BuildExpectedOidcUri(string oidcRequestUrl, string audience)
    {
        var separator = oidcRequestUrl.Contains('?', StringComparison.Ordinal) ? '&' : '?';
        return $"{oidcRequestUrl}{separator}audience={Uri.EscapeDataString(audience)}";
    }
}
