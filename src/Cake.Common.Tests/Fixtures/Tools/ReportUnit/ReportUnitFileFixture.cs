using Cake.Common.Tools.ReportUnit;
using Cake.Core.IO;
using Cake.Testing.Shared;

namespace Cake.Common.Tests.Fixtures.Tools.ReportUnit
{
    internal sealed class ReportUnitFileFixture : ToolFixture<ReportUnitSettings>
    {
        public FilePath InputFile;
        public FilePath OutputFile;

        public ReportUnitFileFixture()
            : base("ReportUnit.exe")
        {
            InputFile = "/temp/input.xml";
            OutputFile = "/temp/output.html";
        }

        protected override void RunTool()
        {
            var tool = new ReportUnitRunner(FileSystem, Environment, ProcessRunner, Globber);
            tool.Run(InputFile, OutputFile, Settings);
        }
    }
}