// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Cake.Common.Tools.Chocolatey.Push;
using Cake.Core.IO;

namespace Cake.Common.Tests.Fixtures.Tools.Chocolatey.Push
{
    internal sealed class ChocolateyPusherFixture : ChocolateyFixture<ChocolateyPushSettings>
    {
        public FilePath PackageFilePath { get; set; }

        public ChocolateyPusherFixture()
        {
            PackageFilePath = "./existing.nupkg";
        }

        protected override void RunTool()
        {
            var tool = new ChocolateyPusher(FileSystem, Environment, ProcessRunner, Tools, Resolver);
            tool.Push(PackageFilePath, Settings);
        }
    }
}
