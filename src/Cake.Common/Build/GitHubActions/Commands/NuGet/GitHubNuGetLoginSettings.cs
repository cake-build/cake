// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Cake.Common.Build.GitHubActions.Commands.NuGet
{
    /// <summary>
    /// Configuration for exchanging a GitHub Actions OIDC token for a NuGet API key via the NuGet token service.
    /// </summary>
    /// <param name="UserName">The NuGet.org account user name.</param>
    /// <param name="TokenServiceUrl">The NuGet token service URL.</param>
    /// <param name="Audience">The OIDC audience passed to the GitHub Actions ID token request.</param>
#pragma warning disable SA1313 // Parameter names should begin with lower-case letter
    public record GitHubNuGetLoginSettings(
        string UserName,
        string TokenServiceUrl = "https://www.nuget.org/api/v2/token",
        string Audience = "https://www.nuget.org");
#pragma warning restore SA1313 // Parameter names should begin with lower-case letter
}
