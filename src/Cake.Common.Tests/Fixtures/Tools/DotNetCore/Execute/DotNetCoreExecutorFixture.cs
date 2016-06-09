// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Cake.Common.Tools.DotNetCore.Execute;
using Cake.Core.IO;

namespace Cake.Common.Tests.Fixtures.Tools.DotNetCore.Execute
{
    internal sealed class DotNetCoreExecutorFixture : DotNetCoreFixture<DotNetCoreExecuteSettings>
    {
        public FilePath AssemblyPath { get; set; }

        public string Arguments { get; set; }

        protected override void RunTool()
        {
            var tool = new DotNetCoreExecutor(FileSystem, Environment, ProcessRunner, Tools);
            tool.Execute(AssemblyPath, Arguments, Settings);
        }
    }
}
