using Cake.Common.Tools.DNU.Pack;

namespace Cake.Common.Tests.Fixtures.Tools.DNU.Pack
{
    internal sealed class DNUPackerFixture : DNUFixture<DNUPackSettings>
    {
        public string Path { get; set; }

        protected override void RunTool()
        {
            var tool = new DNUPacker(FileSystem, Environment, ProcessRunner, Tools);
            tool.Pack(Path, Settings);
        }
    }
}