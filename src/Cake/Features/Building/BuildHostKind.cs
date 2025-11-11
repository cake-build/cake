// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Cake.Features.Building
{
    /// <summary>
    /// Represents the kind of build host.
    /// </summary>
    public enum BuildHostKind
    {
        /// <summary>
        /// Standard build host.
        /// </summary>
        Build,

        /// <summary>
        /// Dry run host.
        /// </summary>
        DryRun,

        /// <summary>
        /// Description host.
        /// </summary>
        Description,

        /// <summary>
        /// Tree host.
        /// </summary>
        Tree
    }
}
