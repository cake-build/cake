// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Cake.Common.Tools.OpenCover
{
    using System;

    /// <summary>
    /// Represents the hide skipped option for OpenCover.
    /// </summary>
    [Flags]
    public enum OpenCoverHideSkippedOption
    {
        /// <summary>
        /// Removes no information from output file.
        /// </summary>
        None = 0,

        /// <summary>
        /// Removes information from output files that relates to classes/modules that have been skipped (filtered) due to use of the -excludebyfile -switch.
        /// </summary>
        File = 1,

        /// <summary>
        /// Removes information from output files that relates to classes/modules that have been skipped (filtered) due to use of the -filter -switch.
        /// </summary>
        Filter = 2,

        /// <summary>
        /// Removes information from output files that relates to classes/modules that have been skipped (filtered) due to use of the -excludebyattribute -switch.
        /// </summary>
        Attribute = 4,

        /// <summary>
        /// Removes missing Pdb information from output file.
        /// </summary>
        MissingPdb = 8,

        /// <summary>
        /// Removes all information from output file.
        /// </summary>
        All = File | Filter | Attribute | MissingPdb
    }
}
