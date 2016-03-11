﻿using System.Collections.Generic;
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
            var runner = new ILRepackRunner(FileSystem, Environment, Globber, ProcessRunner);
            runner.Merge(OutputAssemblyPath, PrimaryAssemblyPath, AssemblyPaths, Settings);
        }
    }
}