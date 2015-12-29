using Cake.Core.Tests.Stubs;
using Cake.Testing.Fixtures;

namespace Cake.Core.Tests.Fixtures
{
    public sealed class DummyToolFixture : ToolFixture<DummySettings>
    {
        public DummyToolFixture()
            : base("dummy.exe")
        {
        }

        protected override void RunTool()
        {
            var tool = new DummyTool(FileSystem, Environment, ProcessRunner, Globber);
            tool.Run(Settings);
        }
    }
}
