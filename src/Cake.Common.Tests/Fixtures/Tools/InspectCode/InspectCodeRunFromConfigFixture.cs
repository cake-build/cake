using Cake.Common.Tools.InspectCode;
using Cake.Core.IO;

namespace Cake.Common.Tests.Fixtures.Tools.InspectCode
{
    internal sealed class InspectCodeRunFromConfigFixture : InspectCodeFixture
    {
        public FilePath Config { get; set; }

        protected override void RunTool()
        {
            var tool = new InspectCodeRunner(FileSystem, Environment, ProcessRunner, Globber);
            tool.RunFromConfig(Config);
        }
    }
}