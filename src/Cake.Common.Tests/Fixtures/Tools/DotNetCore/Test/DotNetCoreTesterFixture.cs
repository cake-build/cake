using Cake.Common.Tools.DotNetCore.Test;

namespace Cake.Common.Tests.Fixtures.Tools.DotNetCore.Test
{
    internal sealed class DotNetCoreTesterFixture : DotNetCoreFixture<DotNetCoreTestSettings>
    {
        public string Project { get; set; }

        protected override void RunTool()
        {
            var tool = new DotNetCoreTester(FileSystem, Environment, ProcessRunner, Tools);
            tool.Test(Project, Settings);
        }
    }
}
