using Cake.Common.Tests.Properties;
using Cake.Common.Tools.InspectCode;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Testing;
using NSubstitute;

namespace Cake.Common.Tests.Fixtures.Tools.InspectCode
{
    internal sealed class InspectCodeRunFixture : InspectCodeFixture
    {
        public ICakeLog Log { get; set; }
        public FilePath Solution { get; set; }

        public InspectCodeRunFixture()
        {
            Solution = new FilePath("./Test.sln");

            Log = Substitute.For<ICakeLog>();

            FileSystem.CreateFile("build/inspect_code.xml").SetContent(Resources.InspectCodeReportNoViolations.NormalizeLineEndings());
            FileSystem.CreateFile("build/violations.xml").SetContent(Resources.InspectCodeReportWithViolations.NormalizeLineEndings());
        }

        protected override void RunTool()
        {
            var tool = new InspectCodeRunner(FileSystem, Environment, ProcessRunner, Tools, Log);
            tool.Run(Solution, Settings);
        }
    }
}