using Cake.Common.Tools.DotNetCore.Restore;

namespace Cake.Common.Tests.Fixtures.Tools.DotNetCore.Restore
{
    internal sealed class DotNetCoreRestorerFixture : DotNetCoreFixture<DotNetCoreRestoreSettings>
    {
        public string Root { get; set; }

        protected override void RunTool()
        {
            var tool = new DotNetCoreRestorer(FileSystem, Environment, ProcessRunner, Tools);
            tool.Restore(Root, Settings);
        }
    }
}
