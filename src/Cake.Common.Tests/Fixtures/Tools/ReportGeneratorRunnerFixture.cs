using System.Collections.Generic;
using Cake.Common.Tools.ReportGenerator;
using Cake.Core.IO;
using Cake.Testing.Shared;

namespace Cake.Common.Tests.Fixtures.Tools
{
    internal sealed class ReportGeneratorRunnerFixture : ToolFixture<ReportGeneratorSettings>
    {
        public IEnumerable<FilePath> Reports { get; set; }
        public DirectoryPath TargetDir { get; set; }

        public ReportGeneratorRunnerFixture() : base("ReportGenerator.exe")
        {
            Reports = new FilePath[] { "report.xml" };
            TargetDir = "output";
        }

        protected override void RunTool()
        {
            var tool = new ReportGeneratorRunner(FileSystem, Environment, ProcessRunner, Globber);
            tool.Run(Reports, TargetDir, Settings);
        }
    }
}