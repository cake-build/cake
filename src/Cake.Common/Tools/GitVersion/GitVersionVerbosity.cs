// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Cake.Common.Tools.GitVersion
{
    /// <summary>
    /// The GitVersion verbosity. Default is <see cref="Normal"/>.
    /// </summary>
    public enum GitVersionVerbosity
    {
        /// <summary>
        /// Quiet verbosity.
        /// </summary>
        Quiet = 0,

        /// <summary>
        /// Minimal verbosity.
        /// </summary>
        Minimal = 1,

        /// <summary>
        /// Normal verbosity (Default).
        /// </summary>
        Normal = 2,

        /// <summary>
        /// Verbose verbosity.
        /// </summary>
        Verbose = 3,

        /// <summary>
        /// Diagnostic verbosity.
        /// </summary>
        Diagnostic = 4,
    }
}
