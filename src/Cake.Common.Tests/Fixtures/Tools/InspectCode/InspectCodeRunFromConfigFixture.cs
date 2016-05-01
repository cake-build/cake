using Cake.Common.Tools.InspectCode;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using NSubstitute;

namespace Cake.Common.Tests.Fixtures.Tools.InspectCode
{
    internal sealed class InspectCodeRunFromConfigFixture : InspectCodeFixture
    {
        public ICakeLog Log { get; set; }
        public FilePath Config { get; set; }

        public InspectCodeRunFromConfigFixture()
        {
            Log = Substitute.For<ICakeLog>();
        }

        protected override void RunTool()
        {
            var tool = new InspectCodeRunner(FileSystem, Environment, ProcessRunner, Globber, Log);
            tool.RunFromConfig(Config);
        }
    }
}