using Cake.Common.Tools.DotNetCore.Pack;

namespace Cake.Common.Tests.Fixtures.Tools.DotNetCore.Pack
{
    internal sealed class DotNetCorePackFixture : DotNetCoreFixture<DotNetCorePackSettings>
    {
        public string Project { get; set; }

        protected override void RunTool()
        {
            var tool = new DotNetCorePacker(FileSystem, Environment, ProcessRunner, Tools);
            tool.Pack(Project, Settings);
        }
    }
}
