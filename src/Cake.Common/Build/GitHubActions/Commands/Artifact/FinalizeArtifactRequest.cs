// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Text.Json.Serialization;

namespace Cake.Common.Build.GitHubActions.Commands.Artifact;

internal record FinalizeArtifactRequest(
    [property: JsonPropertyName("name")]
    string Name,
    [property: JsonPropertyName("hash")]
    string Hash,
    [property: JsonPropertyName("size")]
    long Size,
    [property: JsonPropertyName("workflow_run_backend_id")]
    string WorkflowRunBackendId,
    [property: JsonPropertyName("workflow_job_run_backend_id")]
    string WorkflowJobRunBackendId);
