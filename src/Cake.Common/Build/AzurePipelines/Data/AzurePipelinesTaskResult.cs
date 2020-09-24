// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Cake.Common.Build.AzurePipelines.Data
{
    /// <summary>
    /// Provides the result of an Azure Pipelines task record.
    /// </summary>
    public enum AzurePipelinesTaskResult
    {
        /// <summary>
        /// Succeeded status.
        /// </summary>
        Succeeded,

        /// <summary>
        /// Succeeded with issues status.
        /// </summary>
        SucceededWithIssues,

        /// <summary>
        /// Failed status.
        /// </summary>
        Failed,

        /// <summary>
        /// Cancelled status.
        /// </summary>
        Cancelled,

        /// <summary>
        /// Skipped status.
        /// </summary>
        Skipped
    }
}
