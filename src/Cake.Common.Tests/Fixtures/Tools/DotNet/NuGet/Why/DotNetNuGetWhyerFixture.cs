// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tools.DotNet.NuGet.Why;

namespace Cake.Common.Tests.Fixtures.Tools.DotNet.NuGet.Why;

internal sealed class DotNetNuGetWhyerFixture : DotNetFixture<DotNetNuGetWhySettings>
{
    public string Project { get; set; } = "./src/App/App.csproj";

    public string Package { get; set; } = "Newtonsoft.Json";

    public DotNetNuGetWhyResult Output { get; private set; }

    protected override void RunTool()
    {
        var tool = new DotNetNuGetWhyer(FileSystem, Environment, ProcessRunner, Tools);
        Output = tool.Why(Project, Package, Settings);
    }
}
