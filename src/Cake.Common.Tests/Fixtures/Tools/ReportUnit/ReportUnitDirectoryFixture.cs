using Cake.Common.Tools.ReportUnit;
using Cake.Core.IO;
using Cake.Testing.Fixtures;

namespace Cake.Common.Tests.Fixtures.Tools.ReportUnit
{
    internal sealed class ReportUnitDirectoryFixture : ToolFixture<ReportUnitSettings>
    {
        public DirectoryPath InputFolder;
        public DirectoryPath OutputFolder;

        public ReportUnitDirectoryFixture()
            : base("ReportUnit.exe")
        {
            this.InputFolder = "/temp/input";
            this.OutputFolder = "/temp/output";
        }

        protected override void RunTool()
        {
            var tool = new ReportUnitRunner(this.FileSystem, this.Environment, this.ProcessRunner, this.Globber);
            tool.Run(this.InputFolder, this.OutputFolder, this.Settings);
        }
    }
}