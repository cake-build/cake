using Cake.Common.Tools.XUnit;
using Cake.Core.IO;
using Cake.Testing.Shared;

namespace Cake.Common.Tests.Fixtures.Tools
{
    internal sealed class XUnit2RunnerFixture : ToolFixture<XUnit2Settings>
    {
        public FilePath AssemblyPath { get; set; }

        public XUnit2RunnerFixture()
            : base("xunit.console.exe")
        {
            AssemblyPath = "./Test1.dll";
        }

        protected override void RunTool()
        {
            var runner = new XUnit2Runner(FileSystem, Environment, Globber, ProcessRunner);
            runner.Run(AssemblyPath, Settings);
        }
    }
}