using Cake.Common.Tools.Roundhouse;

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
            var tool = new RoundhouseRunner(FileSystem, Environment, ProcessRunner, Globber);
            tool.Run(Settings, Drop);
        }
    }
}