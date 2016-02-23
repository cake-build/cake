using System.Collections.Generic;
using Cake.Common.Tools.XUnit;
using Cake.Core.IO;
using Cake.Testing.Fixtures;

namespace Cake.Common.Tests.Fixtures.Tools
{
    internal sealed class XUnitRunnerFixture : ToolFixture<XUnitSettings>
    {
        public IEnumerable<FilePath> AssemblyPaths { get; set; }

        public XUnitRunnerFixture()
            : base("xunit.console.clr4.exe")
        {
            AssemblyPaths = new FilePath[] { "./Test1.dll" };
        }

        protected override void RunTool()
        {
            var runner = new XUnitRunner(FileSystem, Environment, Globber, ProcessRunner);
            runner.Run(AssemblyPaths, Settings);
        }
    }
}