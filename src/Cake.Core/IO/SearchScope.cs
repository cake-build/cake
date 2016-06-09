// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
namespace Cake.Core.IO
{
    /// <summary>
    /// Represents a search scope.
    /// </summary>
    public enum SearchScope
    {
        /// <summary>
        /// The current directory.
        /// </summary>
        Current,

        /// <summary>
        /// The current directory and child directories.
        /// </summary>
        Recursive
    }
}
