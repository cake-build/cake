// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core.IO;

namespace Cake.Common.Tools.DotNet.Package.Update;

/// <summary>
/// Contains settings used by <see cref="DotNetPackageUpdater" />.
/// </summary>
public sealed class DotNetPackageUpdateSettings : DotNetSettings
{
    /// <summary>
    /// Gets or sets a value indicating whether to allow the command to stop and wait for user input or action.
    /// For example, to complete authentication.
    /// </summary>
    public bool Interactive { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to upgrade packages that restore reports as having known vulnerabilities.
    /// Packages are upgraded to the lowest version that is higher than the currently referenced version and has no known vulnerabilities.
    /// </summary>
    public bool Vulnerable { get; set; }

    /// <summary>
    /// Gets or sets the project or directory that packages should be updated in.
    /// Used when there is no project method argument.
    /// </summary>
    public FilePath Project { get; set; }
}
