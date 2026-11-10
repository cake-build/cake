// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tests.Fixtures.Tools.VSWhere;
using Cake.Common.Tools.MSBuild;
using Cake.Common.Tools.VSWhere.Latest;
using Cake.Core.IO;

namespace Cake.Common.Tests.Fixtures.Tools.MSBuild;

internal sealed class VSWhereMSBuildInstallationLocatorFixture : VSWhereFixture<VSWhereLatestSettings>
{
    public string VersionRange { get; set; }
    public bool IncludePrerelease { get; set; }
    public DirectoryPath InstallationPath { get; private set; }

    public void GivenStandardOutput(params string[] output)
    {
        ProcessRunner.Process.SetStandardOutput(output);
    }

    protected override void RunTool()
    {
        var locator = new VSWhereMSBuildInstallationLocator(FileSystem, Environment, ProcessRunner, Tools);
        InstallationPath = locator.FindInstallation(VersionRange, IncludePrerelease);
    }
}
