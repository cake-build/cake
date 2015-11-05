using Cake.Common.Tools.DupFinder;
using Cake.Core.IO;

namespace Cake.Common.Tests.Fixtures.Tools.DupFinder
{
    internal sealed class DupFinderRunnerConfigFixture : ToolFixture<DupFinderSettings>
    {
        public FilePath ConfigPath { get; set; }

        public DupFinderRunnerConfigFixture()
            : base("dupfinder.exe")
        {
            ConfigPath = new FilePath("./Config.xml");
        }

        protected override void RunTool()
        {
            var tool = new DupFinderRunner(FileSystem, Environment, ProcessRunner, Globber);
            tool.RunFromConfig(ConfigPath);
        }
    }
}