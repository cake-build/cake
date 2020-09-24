﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tools.DotNetCore.MSBuild;
using Cake.Core.IO;

namespace Cake.Common.Tools.DotNetCore.Pack
{
    /// <summary>
    /// Contains settings used by <see cref="DotNetCorePacker" />.
    /// </summary>
    public sealed class DotNetCorePackSettings : DotNetCoreSettings
    {
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

        /// <summary>
        /// Gets or sets a value indicating whether to ignore project to project references and only build the root project.
        /// </summary>
        /// <remarks>
        /// Requires .NET Core 2.x or newer.
        /// </remarks>
        public bool NoDependencies { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to not do implicit NuGet package restore.
        /// This makes build faster, but requires restore to be done before build is executed.
        /// </summary>
        /// <remarks>
        /// Requires .NET Core 2.x or newer.
        /// </remarks>
        public bool NoRestore { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to display the startup banner or the copyright message.
        /// </summary>
        /// <remarks>
        /// Available since .NET Core 3.0 SDK.
        /// </remarks>
        public bool NoLogo { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to generate the symbols nupkg.
        /// </summary>
        public bool IncludeSymbols { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to includes the source files in the NuGet package.
        /// The sources files are included in the src folder within the nupkg.
        /// </summary>
        public bool IncludeSource { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to set the serviceable flag in the package.
        /// </summary>
        /// <remarks>
        /// For more information, see https://aka.ms/nupkgservicing.
        /// </remarks>
        public bool Serviceable { get; set; }

        /// <summary>
        /// Gets or sets the target runtime.
        /// </summary>
        public string Runtime { get; set; }

        /// <summary>
        /// Gets or sets additional arguments to be passed to MSBuild.
        /// </summary>
        public DotNetCoreMSBuildSettings MSBuildSettings { get; set; }
    }
}
