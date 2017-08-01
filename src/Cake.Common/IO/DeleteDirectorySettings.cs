// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Cake.Common.IO
{
    /// <summary>
    /// Contains settings used by <c>DeleteDirectory</c>.
    /// </summary>
    public class DeleteDirectorySettings
    {
        /// <summary>
        /// Gets or sets a value indicating whether to perform a recursive delete if set to <c>true</c>.
        /// <remarks>
        /// It is set to <c>false</c> by default.
        /// </remarks>
        /// </summary>
        public bool Recursive { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to delete read-only files if set to <c>true</c>.
        /// <remarks>
        /// It is set to <c>false</c> by default.
        /// </remarks>
        /// </summary>
        public bool Force { get; set; }
    }
}