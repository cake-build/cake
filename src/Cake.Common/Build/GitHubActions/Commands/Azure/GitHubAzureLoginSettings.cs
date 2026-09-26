// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Cake.Common.Build.GitHubActions.Commands.Azure;

/// <summary>
/// Configuration for exchanging a GitHub Actions OIDC token for an Azure AD access token
/// (workload identity federation, same trust model as <c>azure/login</c> with OIDC).
/// </summary>
/// <param name="TenantId">The Entra ID (Azure AD) tenant ID (directory ID).</param>
/// <param name="ClientId">The application (client) ID of the app registration linked to the federated credential.</param>
/// <param name="Scope">The OAuth scope for the access token (default ARM: <c>https://management.azure.com/.default</c>).</param>
/// <param name="Audience">
/// The OIDC audience requested from GitHub; must match the federated credential (commonly <c>api://AzureADTokenExchange</c>).
/// </param>
/// <param name="TokenAuthority">
/// Optional authority host (e.g. <c>https://login.microsoftonline.com</c>). Defaults to the public Azure cloud login endpoint.
/// </param>
public record GitHubAzureLoginSettings(
    string TenantId,
    string ClientId,
    string Scope = "https://management.azure.com/.default",
    string Audience = "api://AzureADTokenExchange",
    string TokenAuthority = null);
