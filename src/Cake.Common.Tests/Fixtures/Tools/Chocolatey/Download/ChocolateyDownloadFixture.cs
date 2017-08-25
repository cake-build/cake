// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tools.Chocolatey.Download;

namespace Cake.Common.Tests.Fixtures.Tools.Chocolatey.Download
{
    internal sealed class ChocolateyDownloadFixture : ChocolateyFixture<ChocolateyDownloadSettings>
    {
        public string PackageId { get; set; }

        public ChocolateyDownloadFixture()
        {
            PackageId = "MyPackage";
        }

        protected override void RunTool()
        {
            var tool = new ChocolateyDownloader(FileSystem, Environment, ProcessRunner, Tools, Resolver);
            tool.Download(PackageId, Settings);
        }
    }
}