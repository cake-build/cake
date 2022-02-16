// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Cake.DotNetTool.Module
{
    /// <summary>
    /// Represents a dotnet tool package.
    /// </summary>
    public sealed class DotNetToolPackage
    {
        /// <summary>
        /// Gets or sets the tool package ID.
        /// </summary>
        /// <value>The tool package ID.</value>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the tool package version.
        /// </summary>
        /// <value>The tool package version.</value>
        public string Version { get; set; }

        /// <summary>
        /// Gets or sets the tool package short code.
        /// </summary>
        /// <value>The tool package short code.</value>
        public string ShortCode { get; set; }
    }
}