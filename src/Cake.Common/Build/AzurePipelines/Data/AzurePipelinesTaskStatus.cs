// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Cake.Common.Build.AzurePipelines.Data
{
    /// <summary>
    /// Provides the status of an Azure Pipelines task record.
    /// </summary>
    public enum AzurePipelinesTaskStatus
    {
        /// <summary>
        /// Unknown status.
        /// </summary>
        Unknown,

        /// <summary>
        /// Initialized status.
        /// </summary>
        Initialized,

        /// <summary>
        /// In progress status.
        /// </summary>
        InProgress,

        /// <summary>
        /// Completed status.
        /// </summary>
        Completed
    }
}
