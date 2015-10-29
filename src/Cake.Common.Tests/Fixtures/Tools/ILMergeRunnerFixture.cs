using System.Collections.Generic;
using Cake.Common.Tools.ILMerge;
using Cake.Core.IO;

namespace Cake.Common.Tests.Fixtures.Tools
{
    internal sealed class ILMergeRunnerFixture : ToolFixture<ILMergeSettings>
    {
        public FilePath OutputAssemblyPath { get; set; }
        public FilePath PrimaryAssemblyPath { get; set; }
        public List<FilePath> AssemblyPaths { get; set; }

        public ILMergeRunnerFixture()
            : base("ILMerge.exe")
        {
            OutputAssemblyPath = "output.exe";
            PrimaryAssemblyPath = "input.exe";
            AssemblyPaths = new List<FilePath>();
        }

        protected override void RunTool()
        {
            var runner = new ILMergeRunner(FileSystem, Environment, Globber, ProcessRunner);
            runner.Merge(OutputAssemblyPath, PrimaryAssemblyPath, AssemblyPaths, Settings);
        }
    }
}