// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Cake.Common.Tools.DotNet.Package.List
{
    /// <summary>
    /// The project information.
    /// </summary>
    public sealed class DotNetPackageListProjectItem
    {
        /// <summary>
        /// Gets the project path.
        /// </summary>
        [JsonInclude]
        public string Path { get; private set; }

        /// <summary>
        /// Gets the list of frameworks.
        /// </summary>
        [JsonInclude]
        public IEnumerable<DotNetPackageListFrameworkItem> Frameworks { get; private set; }
    }
}
