// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Text.Json.Serialization;

namespace Cake.Common.Build.GitHubActions.Commands.NuGet
{
#pragma warning disable SA1313 // Parameter names should begin with lower-case letter
    /// <summary>
    /// JSON payload returned by the GitHub Actions OIDC token endpoint.
    /// </summary>
    /// <param name="Value">The OIDC JWT value.</param>
    internal sealed record GitHubOidcTokenResponse(
        [property: JsonPropertyName("value")] string Value);

    /// <summary>
    /// JSON body sent to the NuGet token exchange endpoint.
    /// </summary>
    /// <param name="Username">The NuGet user name.</param>
    /// <param name="TokenType">The token type (always <c>ApiKey</c> for this flow).</param>
    internal sealed record NuGetTokenExchangeRequest(
        [property: JsonPropertyName("username")] string Username,
        [property: JsonPropertyName("tokenType")] string TokenType);

    /// <summary>
    /// JSON payload returned on successful NuGet token exchange.
    /// </summary>
    /// <param name="ApiKey">The NuGet API key.</param>
    internal sealed record NuGetTokenExchangeResponse(
        [property: JsonPropertyName("apiKey")] string ApiKey);

    /// <summary>
    /// JSON payload returned when NuGet token exchange fails.
    /// </summary>
    /// <param name="Error">The error message from the service.</param>
    internal sealed record NuGetTokenErrorResponse(
        [property: JsonPropertyName("error")] string Error);
#pragma warning restore SA1313 // Parameter names should begin with lower-case letter
}
