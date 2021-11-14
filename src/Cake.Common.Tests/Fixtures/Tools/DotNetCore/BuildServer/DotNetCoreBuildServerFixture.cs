// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tools.DotNet.BuildServer;
using Cake.Common.Tools.DotNetCore.BuildServer;

namespace Cake.Common.Tests.Fixtures.Tools.DotNetCore.Build
{
    internal sealed class DotNetCoreBuildServerFixture : DotNetCoreFixture<DotNetBuildServerShutdownSettings>
    {
        protected override void RunTool()
        {
            var tool = new DotNetCoreBuildServer(FileSystem, Environment, ProcessRunner, Tools);
            tool.Shutdown(Settings);
        }
    }
}