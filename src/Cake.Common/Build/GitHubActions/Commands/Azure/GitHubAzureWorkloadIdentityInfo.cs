// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core.IO;

namespace Cake.Common.Build.GitHubActions.Commands.Azure;

/// <summary>
/// Identifiers and federated token file path for Azure.Identity <c>WorkloadIdentityCredential</c>:
/// set environment variables <c>AZURE_CLIENT_ID</c>, <c>AZURE_TENANT_ID</c>, and <c>AZURE_FEDERATED_TOKEN_FILE</c> to these values.
/// </summary>
/// <param name="TenantId">The Entra ID tenant ID.</param>
/// <param name="ClientId">The application (client) ID.</param>
/// <param name="FederatedTokenFile">The path to the file containing the GitHub OIDC JWT (client assertion).</param>
public sealed record GitHubAzureWorkloadIdentityInfo(
    string TenantId,
    string ClientId,
    FilePath FederatedTokenFile);
