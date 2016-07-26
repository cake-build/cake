// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Cake.Common.Tools.WiX.Heat
{
    /// <summary>
    /// The type of object file to harvest from.
    /// </summary>
    public enum WiXHarvestType
    {
        /// <summary>
        /// Harvest a directory.
        /// </summary>
        Dir,

        /// <summary>
        /// Harvest a file
        /// </summary>
        File,

        /// <summary>
        /// Harvest outputs of a Visual Studio project.
        /// </summary>
        Project,

        /// <summary>
        /// Harvest an IIS web site.
        /// </summary>
        Website,

        /// <summary>
        /// Harvest performance counters from a category.
        /// </summary>
        Perf,

        /// <summary>
        /// Harvest registry information from a reg file.
        /// </summary>
        Reg
    }
}