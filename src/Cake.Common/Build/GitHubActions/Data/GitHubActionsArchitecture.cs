// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Cake.Common.Build.GitHubActions.Data
{
    /// <summary>
    /// The GitHub Actions Architecture.
    /// </summary>
    public enum GitHubActionsArchitecture
    {
        /// <summary>
        /// Unknown.
        /// </summary>
        Unknown,

        /// <summary>
        /// X86.
        /// </summary>
        X86,

        /// <summary>
        /// X64.
        /// </summary>
        X64,

        /// <summary>
        /// ARM.
        /// </summary>
        ARM,

        /// <summary>
        /// ARM64.
        /// </summary>
        ARM64
    }
}
