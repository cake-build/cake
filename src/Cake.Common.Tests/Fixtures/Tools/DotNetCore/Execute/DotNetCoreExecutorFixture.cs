using Cake.Common.Tools.DotNetCore.Execute;
using Cake.Core.IO;

namespace Cake.Common.Tests.Fixtures.Tools.DotNetCore.Execute
{
    internal sealed class DotNetCoreExecutorFixture : DotNetCoreFixture<DotNetCoreExecuteSettings>
    {
        public FilePath AssemblyPath { get; set; }

        public string Arguments { get; set; }

        protected override void RunTool()
        {
            var tool = new DotNetCoreExecutor(FileSystem, Environment, ProcessRunner, Tools);
            tool.Execute(AssemblyPath, Arguments, Settings);
        }
    }
}
