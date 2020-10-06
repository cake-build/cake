// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Cake.Frosting
{
    /// <summary>
    /// Represents a command for the <see cref="ICakeHost"/>.
    /// </summary>
    public enum CakeHostCommand
    {
        /// <summary>
        /// Runs the build script.
        /// </summary>
        Run = 0,

        /// <summary>
        /// Performs a dry run of the build script.
        /// </summary>
        DryRun = 1,

        /// <summary>
        /// Shows version information.
        /// </summary>
        Version = 2,

        /// <summary>
        /// Shows help.
        /// </summary>
        Help = 3,
    }
}
