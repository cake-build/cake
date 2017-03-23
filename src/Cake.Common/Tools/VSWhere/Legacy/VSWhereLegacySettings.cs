// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Cake.Common.Tools.VSWhere.Legacy
{
    /// <summary>
    /// Contains settings used by <see cref="VSWhereLegacy"/>.
    /// </summary>
    public class VSWhereLegacySettings : VSWhereSettings
    {
        /// <summary>
        /// Gets or sets a value indicating whether to return only the newest version and last installed.
        /// </summary>
        /// <value><c>true</c> to find the newest version or last installed; otherwise, <c>false</c>.</value>
        public bool Latest { get; set; }

        /// <summary>
        /// Gets the workload(s) or component(s) required when finding instances, immutable to always be empty per documentation.
        /// </summary>
        public new string Requires => string.Empty;
    }
}
