using System.Collections.Generic;
using Cake.Common.Tools.Fixie;
using Cake.Core.IO;
using Cake.Testing.Shared;

namespace Cake.Common.Tests.Fixtures.Tools
{
    internal sealed class FixieRunnerFixture : ToolFixture<FixieSettings>
    {
        public List<FilePath> AssemblyPaths { get; set; }

        public FixieRunnerFixture()
            : base("Fixie.Console.exe")
        {
            AssemblyPaths = new List<FilePath>();
            AssemblyPaths.Add(new FilePath("./Test1.dll"));
        }

        protected override void RunTool()
        {
            var tool = new FixieRunner(FileSystem, Environment, ProcessRunner, Globber);
            tool.Run(AssemblyPaths, Settings);
        }
    }
}