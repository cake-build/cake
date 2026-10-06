// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tools.DotNet.Package.Update;

namespace Cake.Common.Tests.Fixtures.Tools.DotNet.Package.Update;

internal sealed class DotNetPackageUpdaterFixture : DotNetFixture<DotNetPackageUpdateSettings>
{
    public IEnumerable<string> Packages { get; set; }

    public string Project { get; set; }

    protected override void RunTool()
    {
        var tool = new DotNetPackageUpdater(FileSystem, Environment, ProcessRunner, Tools);
        tool.Update(Packages, Project, Settings);
    }
}
