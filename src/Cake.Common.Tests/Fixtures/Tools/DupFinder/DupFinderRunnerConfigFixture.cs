using Cake.Common.Tools.DupFinder;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Testing.Fixtures;
using NSubstitute;

namespace Cake.Common.Tests.Fixtures.Tools.DupFinder
{
    internal sealed class DupFinderRunnerConfigFixture : ToolFixture<DupFinderSettings>
    {
        private readonly ICakeLog Log;

        public FilePath ConfigPath { get; set; }

        public DupFinderRunnerConfigFixture()
            : base("dupfinder.exe")
        {
            ConfigPath = new FilePath("./Config.xml");

            Log = Substitute.For<ICakeLog>();
        }

        protected override void RunTool()
        {
            var tool = new DupFinderRunner(FileSystem, Environment, ProcessRunner, Globber, Log);
            tool.RunFromConfig(ConfigPath);
        }
    }
}