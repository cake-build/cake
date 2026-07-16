// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Cake.Core;

namespace Cake.Common.Build.GitHubActions.Commands.NuGet
{
    /// <summary>
    /// Exchanges a GitHub Actions OIDC token for a NuGet API key via the NuGet token service.
    /// </summary>
    internal sealed class GitHubNuGetLoginService
    {
        private const string OidcRequestTokenVariable = "ACTIONS_ID_TOKEN_REQUEST_TOKEN";
        private const string OidcRequestUrlVariable = "ACTIONS_ID_TOKEN_REQUEST_URL";
        private const string UserAgent = "nuget/login-action";

        private readonly ICakeEnvironment _environment;
        private readonly Func<string, HttpClient> _createHttpClient;
        private readonly Action<string> _maskSecret;

        /// <summary>
        /// Initializes a new instance of the <see cref="GitHubNuGetLoginService"/> class.
        /// </summary>
        /// <param name="environment">The Cake environment.</param>
        /// <param name="createHttpClient">A factory that creates an <see cref="HttpClient"/> for the given logical operation name.</param>
        /// <param name="maskSecret">A callback that masks sensitive values in the build log.</param>
        public GitHubNuGetLoginService(
            ICakeEnvironment environment,
            Func<string, HttpClient> createHttpClient,
            Action<string> maskSecret)
        {
            _environment = environment ?? throw new ArgumentNullException(nameof(environment));
            _createHttpClient = createHttpClient ?? throw new ArgumentNullException(nameof(createHttpClient));
            _maskSecret = maskSecret ?? throw new ArgumentNullException(nameof(maskSecret));
        }

        /// <summary>
        /// Requests an OIDC token from GitHub and exchanges it for a NuGet API key.
        /// </summary>
        /// <param name="settings">User name, token service URL, and OIDC audience.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The NuGet API key.</returns>
        public async Task<string> LoginAsync(GitHubNuGetLoginSettings settings, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(settings);
            ArgumentException.ThrowIfNullOrWhiteSpace(settings.UserName, "userName");
            ArgumentException.ThrowIfNullOrWhiteSpace(settings.TokenServiceUrl, "tokenServiceUrl");
            ArgumentException.ThrowIfNullOrWhiteSpace(settings.Audience, "audience");

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

            var apiKey = await ExchangeForNuGetApiKeyAsync(settings.UserName, settings.TokenServiceUrl, oidcJwt, cancellationToken).ConfigureAwait(false);

            MaskSecret(apiKey);

            return apiKey;
        }

        private static Uri BuildOidcTokenRequestUri(string oidcRequestUrl, string nugetAudience)
        {
            var separator = oidcRequestUrl.Contains('?', StringComparison.Ordinal) ? '&' : '?';
            var raw = $"{oidcRequestUrl}{separator}audience={Uri.EscapeDataString(nugetAudience)}";
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
                GitHubNuGetLoginJsonContext.Default.GitHubOidcTokenResponse,
                cancellationToken).ConfigureAwait(false);

            if (tokenResponse is null || string.IsNullOrEmpty(tokenResponse.Value))
            {
                throw new CakeException("Failed to retrieve OIDC token from GitHub.");
            }

            return tokenResponse.Value;
        }

        private async Task<string> ExchangeForNuGetApiKeyAsync(
            string userName,
            string tokenServiceUrl,
            string oidcJwt,
            CancellationToken cancellationToken)
        {
            var body = new NuGetTokenExchangeRequest(userName, "ApiKey");

            var json = JsonSerializer.SerializeToUtf8Bytes(body, GitHubNuGetLoginJsonContext.Default.NuGetTokenExchangeRequest);

            using var httpClient = _createHttpClient(nameof(ExchangeForNuGetApiKeyAsync));
            using var request = new HttpRequestMessage(HttpMethod.Post, tokenServiceUrl)
            {
                Content = new ByteArrayContent(json)
            };

            request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", oidcJwt);
            request.Headers.UserAgent.ParseAdd(UserAgent);

            using var response = await httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                var errorBody = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
                var message = BuildTokenExchangeErrorMessage(response.StatusCode, errorBody);
                throw new CakeException(message);
            }

            await using var stream = await response.Content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false);
            var exchange = await JsonSerializer.DeserializeAsync(
                stream,
                GitHubNuGetLoginJsonContext.Default.NuGetTokenExchangeResponse,
                cancellationToken).ConfigureAwait(false);

            if (string.IsNullOrWhiteSpace(exchange?.ApiKey))
            {
                throw new CakeException("Response did not contain \"apiKey\".");
            }

            return exchange.ApiKey;
        }

        private static string BuildTokenExchangeErrorMessage(HttpStatusCode statusCode, string errorBody)
        {
            var errorMessage = $"Token exchange failed ({(int)statusCode} {statusCode})";

            try
            {
                var errorJson = JsonSerializer.Deserialize(
                    errorBody,
                    GitHubNuGetLoginJsonContext.Default.NuGetTokenErrorResponse);

                if (!string.IsNullOrEmpty(errorJson?.Error))
                {
                    return $"{errorMessage}: {errorJson.Error}";
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
}
