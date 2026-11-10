// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Cake.Core.IO;

namespace Cake.Common.Tools.DotNet.Package.Download;

/// <summary>
/// Contains settings used by <see cref="DotNetPackageDownloader" />.
/// </summary>
public sealed class DotNetPackageDownloadSettings : DotNetSettings
{
    /// <summary>
    /// Gets or sets the directory to download packages to.
    /// </summary>
    public DirectoryPath Output { get; set; }

    /// <summary>
    /// Gets or sets the NuGet package sources to use.
    /// </summary>
    public ICollection<string> Sources { get; set; } = new List<string>();

    /// <summary>
    /// Gets or sets a value indicating whether to allow prerelease packages.
    /// </summary>
    public bool Prerelease { get; set; }

    /// <summary>
    /// Gets or sets the NuGet configuration file.
    /// </summary>
    public FilePath ConfigFile { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to allow the command to stop and wait for user input or action.
    /// For example, to complete authentication.
    /// </summary>
    public bool Interactive { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to allow insecure HTTP connections to package sources.
    /// </summary>
    public bool AllowInsecureConnections { get; set; }
}
