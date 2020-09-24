// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Cake.Common.Tools.OpenCover
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Extensions class for <see cref="OpenCoverHideSkippedOption"/>.
    /// </summary>
    public static class OpenCoverHideSkippedOptionExtensions
    {
        /// <summary>
        /// Get flags.
        /// </summary>
        /// <param name="openCoverHideSkippedOption">
        /// The input.
        /// </param>
        /// <returns>
        /// The <see cref="IEnumerable"/>.
        /// </returns>
        public static IEnumerable<OpenCoverHideSkippedOption> GetFlags(this OpenCoverHideSkippedOption openCoverHideSkippedOption)
        {
            foreach (OpenCoverHideSkippedOption value in Enum.GetValues(openCoverHideSkippedOption.GetType()))
            {
                if (openCoverHideSkippedOption.HasFlag(value))
                {
                    yield return value;
                }
            }
        }
    }
}