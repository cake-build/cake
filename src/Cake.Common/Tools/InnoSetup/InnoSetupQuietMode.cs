// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Cake.Common.Tools.InnoSetup
{
    /// <summary>
    /// Represents the possible quiet modes when compiling an Inno Setup script.
    /// </summary>
    public enum InnoSetupQuietMode
    {
        /// <summary>
        /// Quiet mode disabled. This is the default value.
        /// </summary>
        Off,

        /// <summary>
        /// Quiet mode. Only error messages are printed.
        /// </summary>
        Quiet,

        /// <summary>
        /// Quiet mode with progress. Same as quiet mode, but also displays progress.
        /// </summary>
        QuietWithProgress
    }
}