// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;

namespace Cake.Core.IO
{
    /// <summary>
    /// Represents a file system globber.
    /// </summary>
    public interface IGlobber
    {
        /// <summary>
        /// Returns <see cref="Path" /> instances matching the specified pattern.
        /// </summary>
        /// <param name="pattern">The pattern to match.</param>
        /// <param name="settings">The globber settings.</param>
        /// <returns>
        ///   <see cref="Path" /> instances matching the specified pattern.
        /// </returns>
        IEnumerable<Path> Match(GlobPattern pattern, GlobberSettings settings);
    }
}