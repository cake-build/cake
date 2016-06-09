// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
namespace Cake.Core.Diagnostics
{
    /// <summary>
    /// Represents verbosity.
    /// </summary>
    public enum Verbosity
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
        /// Normal verbosity.
        /// </summary>
        Normal = 2,

        /// <summary>
        /// Verbose verbosity.
        /// </summary>
        Verbose = 3,

        /// <summary>
        /// Diagnostic verbosity.
        /// </summary>
        Diagnostic = 4
    }
}
