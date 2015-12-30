using Cake.Common.Tools.DNU.Build;

namespace Cake.Common.Tests.Fixtures.Tools.DNU.Build
{
    internal sealed class DNUBuilderFixture : DNUFixture<DNUBuildSettings>
    {
        public string Path { get; set; }

        protected override void RunTool()
        {
            var tool = new DNUBuilder(FileSystem, Environment, ProcessRunner, Globber);
            tool.Build(Path, Settings);
        }
    }
}