// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Cake.Common.Build.GitHubActions.Commands;
using Cake.Common.Tests.Fakes;
using Cake.Core;
using Cake.Testing;
using NSubstitute;

namespace Cake.Common.Tests.Fixtures.Build
{
    /// <summary>
    /// Provides a fake <see cref="HttpMessageHandler"/> and GitHub Actions environment for
    /// <see cref="GitHubActionsCommands.NuGetLogin(string, CancellationToken)"/> tests.
    /// </summary>
    internal class GitHubNuGetLoginCommandsFixture : HttpMessageHandler
    {
        /// <summary>
        /// Base OIDC request URL (without <c>audience</c>); matches GitHub Actions format.
        /// </summary>
        public const string OidcRequestUrl = "https://github.example/api/v2/token?foo=1";

        /// <summary>
        /// NuGet token service URL used in tests that override defaults.
        /// </summary>
        public const string TokenServiceUrl = "https://nuget.example/api/v2/token";

        /// <summary>
        /// Default OIDC audience string (matches <see cref="Cake.Common.Build.GitHubActions.Commands.NuGet.GitHubNuGetLoginSettings"/> default).
        /// </summary>
        public const string DefaultAudience = "https://www.nuget.org";

        /// <summary>
        /// Default NuGet token service URL (matches <see cref="Cake.Common.Build.GitHubActions.Commands.NuGet.GitHubNuGetLoginSettings"/> default).
        /// </summary>
        public const string DefaultTokenServiceUrl = "https://www.nuget.org/api/v2/token";

        /// <summary>
        /// Bearer token used for the GitHub OIDC request token environment variable.
        /// </summary>
        public const string ActionsIdTokenRequestToken = "actions-id-token-request-token";

        /// <summary>
        /// Expected full OIDC GET URI including <c>audience</c> query parameter for <see cref="DefaultAudience"/>.
        /// </summary>
        public static readonly string ExpectedOidcGetUriDefaultAudience = BuildExpectedOidcUri(OidcRequestUrl, DefaultAudience);

        private readonly GitHubActionsInfoFixture _gitHubActionsInfoFixture;

        /// <summary>
        /// Initializes a new instance of the <see cref="GitHubNuGetLoginCommandsFixture"/> class.
        /// </summary>
        public GitHubNuGetLoginCommandsFixture()
        {
            _gitHubActionsInfoFixture = new GitHubActionsInfoFixture();
            FileSystem = new FakeFileSystem(_gitHubActionsInfoFixture.Environment);
            FileSystem.CreateDirectory("/opt");
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
                && absolute == TokenServiceUrl
                && request.Headers.Authorization?.Scheme == "Bearer"
                && request.Headers.Authorization.Parameter == "mock-oidc-jwt"
                && request.Headers.UserAgent.ToString().Contains("nuget/login-action", StringComparison.Ordinal))
            {
                return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent("""{"apiKey":"expected-api-key"}""", Encoding.UTF8, "application/json")
                });
            }

            if (request.Method == HttpMethod.Post
                && absolute == DefaultTokenServiceUrl
                && request.Headers.Authorization?.Scheme == "Bearer"
                && request.Headers.Authorization.Parameter == "mock-oidc-jwt"
                && request.Headers.UserAgent.ToString().Contains("nuget/login-action", StringComparison.Ordinal))
            {
                return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent("""{"apiKey":"expected-api-key-default-url"}""", Encoding.UTF8, "application/json")
                });
            }

            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.NotFound));
        }

        private static string BuildExpectedOidcUri(string oidcRequestUrl, string audience)
        {
            var separator = oidcRequestUrl.Contains('?', StringComparison.Ordinal) ? '&' : '?';
            return $"{oidcRequestUrl}{separator}audience={Uri.EscapeDataString(audience)}";
        }
    }
}
