// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Cake.Common.Build.GitHubActions.Data
{
    /// <summary>
    /// The GitHub Actions Ref Type.
    /// </summary>
    public enum GitHubActionsRefType
    {
        /// <summary>
        /// Unknown.
        /// </summary>
        Unknown,

        /// <summary>
        /// Branch.
        /// </summary>
        Branch,

        /// <summary>
        /// Tag.
        /// </summary>
        Tag
    }
}
