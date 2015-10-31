using Cake.Common.Tools.ReportUnit;
using Cake.Core.IO;

namespace Cake.Common.Tests.Fixtures.Tools.ReportUnit
{
    internal sealed class ReportUnitFileFixture : ToolFixture<ReportUnitSettings>
    {
        public FilePath InputFile;
        public FilePath OutputFile;

        public ReportUnitFileFixture()
            : base("ReportUnit.exe")
        {
            this.InputFile = "c:/temp/input.xml";
            this.OutputFile = "c:/temp/output.html";
        }

        protected override void RunTool()
        {
            var tool = new ReportUnitRunner(this.FileSystem, this.Environment, this.ProcessRunner, this.Globber);
            tool.Run(this.InputFile, this.OutputFile, this.Settings);
        }
    }
}