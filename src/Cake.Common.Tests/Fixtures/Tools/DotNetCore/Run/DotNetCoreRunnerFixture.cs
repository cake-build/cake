// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tools.DotNet.Run;
using Cake.Core.IO;

namespace Cake.Common.Tests.Fixtures.Tools.DotNet.Run
{
    internal sealed class DotNetRunnerFixture : DotNetFixture<DotNetRunSettings>
    {
        public string Project { get; set; }

        public ProcessArgumentBuilder Arguments { get; set; }

        protected override void RunTool()
        {
            var tool = new DotNetRunner(FileSystem, Environment, ProcessRunner, Tools);
            tool.Run(Project, Arguments, Settings);
        }
    }
}