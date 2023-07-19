// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core.IO;

namespace Cake.Common.Tools.DotNet.Package.Add
{
    /// <summary>
    /// Contains settings used by <see cref="DotNetPackageAdder" />.
    /// </summary>
    public sealed class DotNetPackageAddSettings : DotNetSettings
    {
        /// <summary>
        /// Gets or sets a specific framework to compile.
        /// </summary>
        public string Framework { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to allow the command to stop and wait for user input or action.
        /// For example, to complete authentication.
        /// </summary>
        public bool Interactive { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to not do implicit NuGet package restore.
        /// This makes run faster, but requires restore to be done before run is executed.
        /// </summary>
        public bool NoRestore { get; set; }

        /// <summary>
        /// Gets or sets the directory path where to restore the packages.
        /// The default package restore location is %userprofile%\.nuget\packages on Windows and ~/.nuget/packages on macOS and Linux.
        /// </summary>
        public DirectoryPath PackageDirectory { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to allow installation of prerelease packages.
        /// Available since .NET Core 5 SDK.
        /// </summary>
        public bool Prerelease { get; set; }

        /// <summary>
        /// Gets or sets the URI of the NuGet package source to use during the restore operation.
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// Gets or sets the version of the package to install.
        /// If none specified, the latest will be used.
        /// </summary>
        public string Version { get; set; }
    }
}
