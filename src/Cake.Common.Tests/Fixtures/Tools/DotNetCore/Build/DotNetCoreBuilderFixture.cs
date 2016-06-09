// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Cake.Common.Tools.DotNetCore.Build;

namespace Cake.Common.Tests.Fixtures.Tools.DotNetCore.Build
{
    internal sealed class DotNetCoreBuilderFixture : DotNetCoreFixture<DotNetCoreBuildSettings>
    {
        public string Project { get; set; }

        protected override void RunTool()
        {
            var tool = new DotNetCoreBuilder(FileSystem, Environment, ProcessRunner, Tools);
            tool.Build(Project, Settings);
        }
    }
}
