// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;

namespace Cake.Common.Tools.NuGet.Pack
{
    /// <summary>
    /// Represents a NuGet nuspec dependency.
    /// </summary>
    public class NuSpecDependency
    {
        /// <summary>
        /// Gets or sets the dependency's package ID.
        /// </summary>
        /// <value>The dependency's package ID.</value>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the dependency's version.
        /// </summary>
        /// <value>The dependency's version.</value>
        public string Version { get; set; }

        /// <summary>
        /// Gets or sets a list of include/exclude tags indicating of the dependency to
        /// include in the final package. The default value is <c>all</c>.
        /// </summary>
        public ICollection<string> Include { get; set; } = new List<string>();

        /// <summary>
        /// Gets or sets a list of include/exclude tags indicating of the dependency to
        /// exclude in the final package. The default value is <c>build,analyzers</c> which can
        /// be over-written.
        /// </summary>
        public ICollection<string> Exclude { get; set; } = new List<string>();

        /// <summary>
        /// Gets or sets the target framework for the dependency.
        /// </summary>
        /// <value>The target framework for the dependency.</value>
        public string TargetFramework { get; set; }
    }
}