// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tools.DotNetCore.Tool;
using Cake.Core.IO;
using Cake.Testing;

namespace Cake.Common.Tests.Fixtures.Tools.DotNetCore.Tool
{
    internal sealed class DotNetCoreToolFixture : DotNetCoreFixture<DotNetCoreToolSettings>
    {
        public FilePath ProjectPath { get; set; }

        public string Command { get; set; }

        public ProcessArgumentBuilder Arguments { get; set; }

        protected override void RunTool()
        {
            var tool = new DotNetCoreToolRunner(FileSystem, Environment, ProcessRunner, Tools);

            tool.Execute(ProjectPath, Command, Arguments, Settings);
        }
    }
}