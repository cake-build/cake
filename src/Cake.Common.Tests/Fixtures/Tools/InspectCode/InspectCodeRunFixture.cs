using Cake.Common.Tools.InspectCode;
using Cake.Core.IO;

namespace Cake.Common.Tests.Fixtures.Tools.InspectCode
{
    internal sealed class InspectCodeRunFixture : InspectCodeFixture
    {
        public FilePath Solution { get; set; }

        public InspectCodeRunFixture()
        {
            Solution = new FilePath("./Test.sln");
        }

        protected override void RunTool()
        {
            var tool = new InspectCodeRunner(FileSystem, Environment, ProcessRunner, Globber);
            tool.Run(Solution, Settings);
        }
    }
}