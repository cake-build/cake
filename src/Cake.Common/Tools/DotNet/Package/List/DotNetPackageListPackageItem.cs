// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Cake.Common.Tools.DotNet.Package.List
{
    /// <summary>
    /// The package information.
    /// </summary>
    public sealed class DotNetPackageListPackageItem
    {
        /// <summary>
        /// Gets the package id.
        /// </summary>
        [JsonInclude]
        public string Id { get; private set; }

        /// <summary>
        /// Gets the requested version.
        /// </summary>
        [JsonInclude]
        public string RequestedVersion { get; private set; }

        /// <summary>
        /// Gets the resolved version.
        /// </summary>
        [JsonInclude]
        public string ResolvedVersion { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the package is auto-referenced.
        /// </summary>
        [JsonInclude]
        public string AutoReferenced { get; private set; }

        /// <summary>
        /// Gets the latest version.
        /// </summary>
        [JsonInclude]
        public string LatestVersion { get; private set; }

        /// <summary>
        /// Gets the deprecation reasons.
        /// </summary>
        [JsonInclude]
        public IEnumerable<string> DeprecationReasons { get; private set; }

        /// <summary>
        /// Gets the alternative package.
        /// </summary>
        [JsonInclude]
        public DotNetPackageListAlternativePackageItem AlternativePackage { get; private set; }

        /// <summary>
        /// Gets the vulnerabilities list.
        /// </summary>
        [JsonInclude]
        public IEnumerable<DotNetPackageListVulnerabilityItem> Vulnerabilities { get; private set; }
    }
}
