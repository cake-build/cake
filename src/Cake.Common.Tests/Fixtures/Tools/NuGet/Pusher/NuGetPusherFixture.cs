// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Cake.Common.Tools.NuGet.Push;
using Cake.Core.IO;

namespace Cake.Common.Tests.Fixtures.Tools.NuGet.Pusher
{
    internal sealed class NuGetPusherFixture : NuGetFixture<NuGetPushSettings>
    {
        public FilePath PackageFilePath { get; set; }

        public NuGetPusherFixture()
        {
            PackageFilePath = "./existing.nupkg";
        }

        protected override void RunTool()
        {
            var tool = new NuGetPusher(FileSystem, Environment, ProcessRunner, Tools, Resolver);
            tool.Push(PackageFilePath, Settings);
        }
    }
}
