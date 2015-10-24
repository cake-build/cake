using System.Collections.Generic;
using Cake.Common.Tools.DupFinder;
using Cake.Core.IO;

namespace Cake.Common.Tests.Fixtures.Tools.DupFinder
{
    internal sealed class DupFinderRunnerFixture : ToolFixture<DupFinderSettings>
    {
        public List<FilePath> FilePaths { get; set; }

        public DupFinderRunnerFixture()
            : base("dupfinder.exe")
        {
            FilePaths = new List<FilePath>();
            FilePaths.Add(new FilePath("./Test.sln"));
        }

        protected override void RunTool()
        {
            var tool = new DupFinderRunner(FileSystem, Environment, ProcessRunner, Globber);
            tool.Run(FilePaths, Settings);
        }
    }
}