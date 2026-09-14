// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Text.Json.Serialization;

namespace Cake.Common.Build.GitHubActions.Commands.NuGet
{
    /// <summary>
    /// Source-generated JSON serialization context for GitHub NuGet login HTTP payloads.
    /// </summary>
    [JsonSerializable(typeof(GitHubOidcTokenResponse))]
    [JsonSerializable(typeof(NuGetTokenExchangeRequest))]
    [JsonSerializable(typeof(NuGetTokenExchangeResponse))]
    [JsonSerializable(typeof(NuGetTokenErrorResponse))]
    internal partial class GitHubNuGetLoginJsonContext : JsonSerializerContext
    {
    }
}
