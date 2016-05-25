using Cake.Common.Tools.Roundhouse;
using Cake.Testing.Fixtures;

namespace Cake.Common.Tests.Fixtures.Tools
{
    internal sealed class RoundhouseRunnerFixture : ToolFixture<RoundhouseSettings>
    {
        public bool Drop { get; set; }

        public RoundhouseRunnerFixture()
            : base("rh.exe")
        {
            Drop = false;
        }

        protected override void RunTool()
        {
            var tool = new RoundhouseRunner(FileSystem, Environment, ProcessRunner, Tools);
            tool.Run(Settings, Drop);
        }
    }
}