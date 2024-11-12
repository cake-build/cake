// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Text.Json.Serialization;

namespace Cake.Common.Build.GitHubActions.Commands.Artifact
{
#pragma warning disable SA1313 // Parameter names should begin with lower-case letter
    internal record ListArtifactsResponse(
        [property: JsonPropertyName("artifacts")]
        ListArtifactsResponse.MonolithArtifact[] Artifacts)
    {
        internal record MonolithArtifact(
            [property: JsonPropertyName("workflow_run_backend_id")]
            string WorkflowRunBackendId,
            [property: JsonPropertyName("workflow_job_run_backend_id")]
            string WorkflowJobRunBackendId,
            [property: JsonPropertyName("database_id")]
            string DatabaseId,
            [property: JsonPropertyName("name")]
            string Name,
            [property: JsonPropertyName("created_at")]
            DateTimeOffset CreatedAt);
    }
}