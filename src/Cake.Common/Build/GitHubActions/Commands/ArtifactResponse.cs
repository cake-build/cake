// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Text.Json.Serialization;

namespace Cake.Common.Build.GitHubActions.Commands
{
    internal sealed class ArtifactResponse
    {
        [JsonPropertyName("containerId")]
        public long ContainerId { get; set; }

        [JsonPropertyName("size")]
        public long Size { get; set; }

        [JsonPropertyName("signedContent")]
        public string SignedContent { get; set; }

        [JsonPropertyName("fileContainerResourceUrl")]
        public string FileContainerResourceUrl { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; }
    }
}
