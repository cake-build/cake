using System.Collections.Generic;
using Cake.Common.Tools.XUnit;
using Cake.Core.IO;
using Cake.Testing.Fixtures;

namespace Cake.Common.Tests.Fixtures.Tools
{
    internal sealed class XUnit2RunnerFixture : ToolFixture<XUnit2Settings>
    {
        public IEnumerable<FilePath> AssemblyPaths { get; set; }

        public XUnit2RunnerFixture()
            : base("xunit.console.exe")
        {
            AssemblyPaths = new FilePath[] { "./Test1.dll" };
        }

        protected override void RunTool()
        {
            var runner = new XUnit2Runner(FileSystem, Environment, Globber, ProcessRunner);
            runner.Run(AssemblyPaths, Settings);
        }
    }
}