// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Cake.Common.Build.AzurePipelines.Data
{
    /// <summary>
    /// Provides the known values for the Azure Pipelines Repository types.
    /// </summary>
    public enum AzurePipelinesRepositoryType
    {
        /// <summary>
        /// TFS Git repository.
        /// </summary>
        TfsGit,

        /// <summary>
        /// Team Foundation Version Control repository.
        /// </summary>
        TfsVersionControl,

        /// <summary>
        /// Git repository hosted on an external server.
        /// </summary>
        Git,

        /// <summary>
        /// GitHub repository.
        /// </summary>
        GitHub,

        /// <summary>
        /// Subversion repository.
        /// </summary>
        Svn
    }
}
