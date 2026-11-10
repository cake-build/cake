// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tools.DotNet.NuGet.Locals;

namespace Cake.Common.Tests.Fixtures.Tools.DotNet.NuGet.Locals;

internal sealed class DotNetNuGetLocalserFixture : DotNetFixture<DotNetNuGetLocalsSettings>
{
    public DotNetNuGetLocalsFolder Folder { get; set; } = DotNetNuGetLocalsFolder.All;

    public bool Clear { get; set; }

    public DotNetNuGetLocalsListResult Output { get; private set; }

    protected override void RunTool()
    {
        var tool = new DotNetNuGetLocalser(FileSystem, Environment, ProcessRunner, Tools);
        if (Clear)
        {
            tool.Clear(Folder, Settings);
            return;
        }

        Output = tool.List(Folder, Settings);
    }
}
