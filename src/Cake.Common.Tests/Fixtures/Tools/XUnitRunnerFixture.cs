// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Cake.Common.Tools.XUnit;
using Cake.Core.IO;
using Cake.Testing.Fixtures;

namespace Cake.Common.Tests.Fixtures.Tools
{
    internal sealed class XUnitRunnerFixture : ToolFixture<XUnitSettings>
    {
        public FilePath AssemblyPath { get; set; }

        public XUnitRunnerFixture()
            : base("xunit.console.clr4.exe")
        {
            AssemblyPath = "./Test1.dll";
        }

        protected override void RunTool()
        {
            var runner = new XUnitRunner(FileSystem, Environment, ProcessRunner, Tools);
            runner.Run(AssemblyPath, Settings);
        }
    }
}
