using System.Collections.Generic;
using Cake.Common.Tools.NUnit;
using Cake.Core.IO;
using Cake.Testing.Fixtures;

namespace Cake.Common.Tests.Fixtures.Tools
{
    internal sealed class NUnitRunnerFixture : ToolFixture<NUnitSettings>
    {
        public List<FilePath> Assemblies { get; set; }

        public NUnitRunnerFixture()
            : base("nunit-console.exe")
        {
            Assemblies = new List<FilePath>();
            Assemblies.Add(new FilePath("./Test1.dll"));
        }

        protected override void RunTool()
        {
            var tool = new NUnitRunner(FileSystem, Environment, ProcessRunner, Tools);
            tool.Run(Assemblies, Settings);
        }
    }
}