// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Cake.Common.Tools.DotNetCore.Run;

namespace Cake.Common.Tests.Fixtures.Tools.DotNetCore.Run
{
    internal sealed class DotNetCoreRunnerFixture : DotNetCoreFixture<DotNetCoreRunSettings>
    {
        public string Project { get; set; }

        public string Arguments { get; set; }

        protected override void RunTool()
        {
            var tool = new DotNetCoreRunner(FileSystem, Environment, ProcessRunner, Tools);
            tool.Run(Project, Arguments, Settings);
        }
    }
}
