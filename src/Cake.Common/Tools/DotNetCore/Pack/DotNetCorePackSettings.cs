// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Cake.Core.IO;

namespace Cake.Common.Tools.DotNetCore.Pack
{
    /// <summary>
    /// Contains settings used by <see cref="DotNetCorePacker" />.
    /// </summary>
    public sealed class DotNetCorePackSettings : DotNetCoreSettings
    {
        /// <summary>
        /// Gets or sets the directory in which to place temporary outputs.
        /// </summary>
        public DirectoryPath BuildBasePath { get; set; }

        /// <summary>
        /// Gets or sets the output directory.
        /// </summary>
        public DirectoryPath OutputDirectory { get; set; }

        /// <summary>
        /// Gets or sets the configuration under which to build.
        /// </summary>
        public string Configuration { get; set; }

        /// <summary>
        /// Gets or sets the value that defines what `*` should be replaced with in version field in project.json.
        /// </summary>
        public string VersionSuffix { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to not build project before packing.
        /// </summary>
        public bool NoBuild { get; set; }
    }
}
