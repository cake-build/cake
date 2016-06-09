// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System.Collections.Generic;
using Cake.Common.Tools.ILRepack;
using Cake.Core.IO;
using Cake.Testing.Fixtures;

namespace Cake.Common.Tests.Fixtures.Tools
{
    internal sealed class ILRepackRunnerFixture : ToolFixture<ILRepackSettings>
    {
        public FilePath OutputAssemblyPath { get; set; }
        public FilePath PrimaryAssemblyPath { get; set; }
        public List<FilePath> AssemblyPaths { get; set; }

        public ILRepackRunnerFixture()
            : base("ILRepack.exe")
        {
            OutputAssemblyPath = "output.exe";
            PrimaryAssemblyPath = "input.exe";
            AssemblyPaths = new List<FilePath>();
        }

        protected override void RunTool()
        {
            var runner = new ILRepackRunner(FileSystem, Environment, ProcessRunner, Tools);
            runner.Merge(OutputAssemblyPath, PrimaryAssemblyPath, AssemblyPaths, Settings);
        }
    }
}
