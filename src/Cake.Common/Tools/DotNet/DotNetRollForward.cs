// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tools.DotNetCore;

namespace Cake.Common.Tools.DotNet
{
    /// <summary>
    /// Contains the roll forward policy to be used.
    /// </summary>
    public sealed class DotNetRollForward
    {
        /// <summary>
        /// Roll forward to the lowest higher minor version, if requested minor version is missing.
        /// </summary>
        public const DotNetCoreRollForward Minor = DotNetCoreRollForward.Minor;

        /// <summary>
        /// Roll forward to the highest patch version. This disables minor version roll forward.
        /// </summary>
        public const DotNetCoreRollForward LatestPatch = DotNetCoreRollForward.LatestPatch;

        /// <summary>
        /// Roll forward to lowest higher major version, and lowest minor version, if requested major version is missing.
        /// </summary>
        public const DotNetCoreRollForward Major = DotNetCoreRollForward.Major;

        /// <summary>
        /// Roll forward to highest minor version, even if requested minor version is present.
        /// </summary>
        public const DotNetCoreRollForward LatestMinor = DotNetCoreRollForward.LatestMinor;

        /// <summary>
        /// Roll forward to highest major and highest minor version, even if requested major is present.
        /// </summary>
        public const DotNetCoreRollForward LatestMajor = DotNetCoreRollForward.LatestMajor;

        /// <summary>
        /// Don't roll forward. Only bind to specified version.
        /// </summary>
        public const DotNetCoreRollForward Disable = DotNetCoreRollForward.Disable;
    }
}
