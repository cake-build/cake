// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Cake.Common.Tools.DotNet.Package.Download;

namespace Cake.Common.Tests.Fixtures.Tools.DotNet.Package.Download;

internal sealed class DotNetPackageDownloaderFixture : DotNetFixture<DotNetPackageDownloadSettings>
{
    public IEnumerable<string> Packages { get; set; } = ["Newtonsoft.Json"];

    protected override void RunTool()
    {
        var tool = new DotNetPackageDownloader(FileSystem, Environment, ProcessRunner, Tools);
        tool.Download(Packages, Settings);
    }
}
