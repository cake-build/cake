using Cake.Common.Tools.XUnit;
using Cake.Core.IO;
using Cake.Testing.Shared;

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
            var runner = new XUnitRunner(FileSystem, Environment, Globber, ProcessRunner);
            runner.Run(AssemblyPath, Settings);
        }
    }
}