// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core.IO;

namespace Cake.Common.Tools.MSBuild;

/// <summary>
/// Locates Visual Studio installations containing MSBuild
/// when they're not found in any of the well-known locations.
/// </summary>
internal interface IMSBuildInstallationLocator
{
    /// <summary>
    /// Finds the root path of the newest Visual Studio installation containing MSBuild.
    /// </summary>
    /// <param name="versionRange">The version range to look for, e.g. <c>[17.0,18.0)</c>, or <c>null</c> for any version.</param>
    /// <param name="includePrerelease">Whether to include prerelease installations.</param>
    /// <returns>The installation root path, or <c>null</c> if no installation was found.</returns>
    DirectoryPath FindInstallation(string versionRange, bool includePrerelease);
}
