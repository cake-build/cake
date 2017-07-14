// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tools.DotNetCore.MSBuild;

namespace Cake.Common.Tests.Fixtures.Tools.DotNetCore.MSBuild
{
    internal sealed class DotNetCoreMSBuildBuilderFixture : DotNetCoreFixture<DotNetCoreMSBuildSettings>
    {
        public string Project { get; set; }

        protected override void RunTool()
        {
            var tool = new DotNetCoreMSBuildBuilder(FileSystem, Environment, ProcessRunner, Tools);
            tool.Build(Project, Settings);
        }
    }
}