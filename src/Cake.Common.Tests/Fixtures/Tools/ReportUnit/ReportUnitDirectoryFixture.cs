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
            InputFolder = "/temp/input";
            OutputFolder = "/temp/output";
        }

        protected override void RunTool()
        {
            var tool = new ReportUnitRunner(FileSystem, Environment, ProcessRunner, Globber);
            tool.Run(InputFolder, OutputFolder, Settings);
        }
    }
}