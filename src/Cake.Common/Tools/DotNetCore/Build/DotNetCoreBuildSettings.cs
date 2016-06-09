// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System.Collections.Generic;
using Cake.Core.IO;

namespace Cake.Common.Tools.DotNetCore.Build
{
    /// <summary>
    /// Contains settings used by <see cref="DotNetCoreBuilder" />.
    /// </summary>
    public sealed class DotNetCoreBuildSettings : DotNetCoreSettings
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
        /// Gets or sets the target runtime.
        /// </summary>
        public string Runtime { get; set; }

        /// <summary>
        /// Gets or sets the configuration under which to build.
        /// </summary>
        public string Configuration { get; set; }

        /// <summary>
        /// Gets or sets the specific framework to compile.
        /// </summary>
        public string Framework { get; set; }

        /// <summary>
        /// Gets or sets the value that defines what `*` should be replaced with in version field in project.json.
        /// </summary>
        public string VersionSuffix { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to print the incremental safety checks that prevent incremental compilation.
        /// </summary>
        public bool BuildProfile { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to mark the build as unsafe for incrementality.
        /// This turns off incremental compilation and forces a clean rebuild of the project dependency graph.
        /// </summary>
        public bool NoIncremental { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to ignore project to project references and only build the root project.
        /// </summary>
        public bool NoDependencies { get; set; }
    }
}
