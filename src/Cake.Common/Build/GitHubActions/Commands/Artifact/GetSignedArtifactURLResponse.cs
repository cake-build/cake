// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Text.Json.Serialization;
namespace Cake.Common.Build.GitHubActions.Commands.Artifact
{
#pragma warning disable SA1313 // Parameter names should begin with lower-case letter
    internal record GetSignedArtifactURLResponse(
        [property: JsonPropertyName("name")]
        string Name,
        [property: JsonPropertyName("signed_url")]
        string SignedUrl);
#pragma warning restore SA1313 // Parameter names should begin with lower-case letter
}