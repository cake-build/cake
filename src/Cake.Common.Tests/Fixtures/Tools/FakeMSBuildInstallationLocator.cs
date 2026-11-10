// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tools.MSBuild;
using Cake.Core.IO;

namespace Cake.Common.Tests.Fixtures.Tools;

internal sealed class FakeMSBuildInstallationLocator : IMSBuildInstallationLocator
{
    public DirectoryPath InstallationPath { get; set; }
    public List<(string VersionRange, bool IncludePrerelease)> Calls { get; } = new List<(string, bool)>();

    public DirectoryPath FindInstallation(string versionRange, bool includePrerelease)
    {
        Calls.Add((versionRange, includePrerelease));
        return InstallationPath;
    }
}
