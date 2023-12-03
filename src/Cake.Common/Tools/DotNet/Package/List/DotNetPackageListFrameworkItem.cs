// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Cake.Common.Tools.DotNet.Package.List
{
    /// <summary>
    /// The framework information.
    /// </summary>
    public sealed class DotNetPackageListFrameworkItem
    {
        /// <summary>
        /// Gets the framework name.
        /// </summary>
        [JsonInclude]
        public string Framework { get; private set; }

        /// <summary>
        /// Gets the top-level packages.
        /// </summary>
        [JsonInclude]
        public IEnumerable<DotNetPackageListPackageItem> TopLevelPackages { get; private set; }

        /// <summary>
        /// Gets transitive packages.
        /// </summary>
        [JsonInclude]
        public IEnumerable<DotNetPackageListPackageItem> TransitivePackages { get; private set; }
    }
}
