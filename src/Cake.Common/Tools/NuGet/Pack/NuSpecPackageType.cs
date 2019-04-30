// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Cake.Common.Tools.NuGet.Pack
{
    /// <summary>
    /// Specifies the package's package type that indicates its intended use.
    /// </summary>
    public class NuSpecPackageType
    {
        /// <summary>
        /// Gets or sets the package type, e.g <c>DotnetCliTool</c>
        /// Defaults to <c>Dependency</c>.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the version of the package type.
        /// </summary>
        public string Version { get; set; }
    }
}
