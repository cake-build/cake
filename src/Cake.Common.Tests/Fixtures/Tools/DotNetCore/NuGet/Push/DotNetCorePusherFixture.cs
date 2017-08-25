// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tools.DotNetCore.NuGet.Push;

namespace Cake.Common.Tests.Fixtures.Tools.DotNetCore.NuGet.Push
{
    internal sealed class DotNetCorePusherFixture : DotNetCoreFixture<DotNetCoreNuGetPushSettings>
    {
        public string PackageName { get; set; }

        protected override void RunTool()
        {
            var tool = new DotNetCoreNuGetPusher(FileSystem, Environment, ProcessRunner, Tools);
            tool.Push(PackageName, Settings);
        }
    }
}