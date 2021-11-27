// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tools.DotNet.MSBuild;
using Cake.Common.Tools.DotNetCore.Clean;
using Cake.Core.IO;

namespace Cake.Common.Tools.DotNet.Clean
{
    /// <summary>
    /// Contains settings used by <see cref="DotNetCoreCleaner" />.
    /// </summary>
    public class DotNetCleanSettings : DotNetSettings
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
        /// Gets or sets the specific framework to compile.
        /// </summary>
        public string Framework { get; set; }

        /// <summary>
        /// Gets or sets the target runtime.
        /// </summary>
        public string Runtime { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to display the startup banner or the copyright message.
        /// </summary>
        /// <remarks>
        /// Available since .NET Core 3.0 SDK.
        /// </remarks>
        public bool NoLogo { get; set; }

        /// <summary>
        /// Gets or sets additional arguments to be passed to MSBuild.
        /// </summary>
        public DotNetMSBuildSettings MSBuildSettings { get; set; }
    }
}
