// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Text.Json.Serialization;

namespace Cake.Common.Build.GitHubActions.Commands.Azure;

/// <summary>
/// JSON payload returned by the GitHub Actions OIDC token endpoint.
/// </summary>
/// <param name="Value">The OIDC JWT string.</param>
internal sealed record GitHubOidcTokenResponse(
    [property: JsonPropertyName("value")] string Value);

/// <summary>
/// JSON payload returned by the Entra ID OAuth2 token endpoint on success.
/// </summary>
/// <param name="AccessToken">The Azure AD access token.</param>
internal sealed record AzureOAuthTokenResponse(
    [property: JsonPropertyName("access_token")] string AccessToken);

/// <summary>
/// JSON payload returned by the Entra ID OAuth2 token endpoint on error.
/// </summary>
/// <param name="Error">The OAuth error code.</param>
/// <param name="ErrorDescription">Human-readable error description.</param>
internal sealed record AzureOAuthErrorResponse(
    [property: JsonPropertyName("error")] string Error,
    [property: JsonPropertyName("error_description")] string ErrorDescription);
