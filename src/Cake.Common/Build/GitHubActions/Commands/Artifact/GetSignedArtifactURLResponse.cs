// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Text.Json.Serialization;
namespace Cake.Common.Build.GitHubActions.Commands.Artifact;

internal record GetSignedArtifactURLResponse(
    [property: JsonPropertyName("name")]
    string Name,
    [property: JsonPropertyName("signed_url")]
    string SignedUrl);
