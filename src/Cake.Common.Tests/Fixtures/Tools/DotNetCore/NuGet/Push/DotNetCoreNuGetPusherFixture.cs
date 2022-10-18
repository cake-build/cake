// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tools.DotNet.NuGet.Push;

namespace Cake.Common.Tests.Fixtures.Tools.DotNet.NuGet.Push
{
    internal sealed class DotNetNuGetPusherFixture : DotNetFixture<DotNetNuGetPushSettings>
    {
        public string PackageName { get; set; }

        protected override void RunTool()
        {
            var tool = new DotNetNuGetPusher(FileSystem, Environment, ProcessRunner, Tools);
            tool.Push(PackageName, Settings);
        }
    }
}