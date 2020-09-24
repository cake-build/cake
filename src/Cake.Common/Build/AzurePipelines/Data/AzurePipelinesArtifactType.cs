// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Cake.Common.Build.AzurePipelines.Data
{
    /// <summary>
    /// Provides the type of an Azure Pipelines artifact.
    /// </summary>
    public enum AzurePipelinesArtifactType
    {
        /// <summary>
        /// The container type.
        /// </summary>
        Container,

        /// <summary>
        /// The file path type.
        /// </summary>
        FilePath,

        /// <summary>
        /// The version control path type.
        /// </summary>
        VersionControl,

        /// <summary>
        /// The Git reference type.
        /// </summary>
        GitRef,

        /// <summary>
        /// The TFVC label type.
        /// </summary>
        TFVCLabel
    }
}
