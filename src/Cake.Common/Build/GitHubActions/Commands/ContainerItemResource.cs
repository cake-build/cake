// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Text.Json.Serialization;

namespace Cake.Common.Build.GitHubActions.Commands
{
    internal class ContainerItemResource
    {
        [JsonPropertyName("containerId")]
        public long ContainerId { get; set; }

        [JsonPropertyName("scopeIdentifier")]
        public Guid ScopeIdentifier { get; set; }

        [JsonPropertyName("path")]
        public string Path { get; set; }

        [JsonPropertyName("itemType")]
        public string ItemType { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("dateCreated")]
        public DateTimeOffset DateCreated { get; set; }

        [JsonPropertyName("dateLastModified")]
        public DateTimeOffset DateLastModified { get; set; }

        [JsonPropertyName("createdBy")]
        public Guid CreatedBy { get; set; }

        [JsonPropertyName("lastModifiedBy")]
        public Guid LastModifiedBy { get; set; }

        [JsonPropertyName("itemLocation")]
        public string ItemLocation { get; set; }

        [JsonPropertyName("contentLocation")]
        public string ContentLocation { get; set; }

        [JsonPropertyName("contentId")]
        public string ContentId { get; set; }

        [JsonPropertyName("fileLength")]
        public long? FileLength { get; set; }

        [JsonPropertyName("fileEncoding")]
        public long? FileEncoding { get; set; }

        [JsonPropertyName("fileType")]
        public long? FileType { get; set; }

        [JsonPropertyName("fileId")]
        public long? FileId { get; set; }
    }
}
