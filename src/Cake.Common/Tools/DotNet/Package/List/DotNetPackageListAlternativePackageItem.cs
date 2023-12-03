// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Text.Json.Serialization;

namespace Cake.Common.Tools.DotNet.Package.List
{
    /// <summary>
    /// The alternative package information.
    /// </summary>
    public sealed class DotNetPackageListAlternativePackageItem
    {
        /// <summary>
        /// Gets the alternative package id.
        /// </summary>
        [JsonInclude]
        public string Id { get; private set; }

        /// <summary>
        /// Gets the alternative package versions.
        /// </summary>
        [JsonInclude]
        public string VersionRange { get; private set; }
    }
}
