// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Cake.Common.Tools.DotNet.Package.List
{
    /// <summary>
    /// An result as returned by <see cref="DotNetPackageLister"/>.
    /// </summary>
    public sealed class DotNetPackageList
    {
        /// <summary>
        /// Gets the output version.
        /// </summary>
        [JsonInclude]
        public int Version { get; private set; }

        /// <summary>
        /// Gets the specified parameters.
        /// </summary>
        [JsonInclude]
        public string Parameters { get; private set; }

        /// <summary>
        /// Gets the problems.
        /// </summary>
        [JsonInclude]
        public IEnumerable<DotNetPackageListProblemItem> Problems { get; private set; }

        /// <summary>
        /// Gets the used sources.
        /// </summary>
        [JsonInclude]
        public IEnumerable<string> Sources { get; private set; }

        /// <summary>
        /// Gets the projects.
        /// </summary>
        [JsonInclude]
        public IEnumerable<DotNetPackageListProjectItem> Projects { get; private set; }
    }
}
