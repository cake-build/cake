// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tools.DotNet.Project.Convert;
using Cake.Core.IO;

namespace Cake.Common.Tests.Fixtures.Tools.DotNet.Project.Convert;

internal sealed class DotNetProjectConverterFixture : DotNetFixture<DotNetProjectConvertSettings>
{
    public FilePath File { get; set; } = "cake.cs";

    protected override void RunTool()
    {
        var tool = new DotNetProjectConverter(FileSystem, Environment, ProcessRunner, Tools);
        tool.Convert(File, Settings);
    }
}
