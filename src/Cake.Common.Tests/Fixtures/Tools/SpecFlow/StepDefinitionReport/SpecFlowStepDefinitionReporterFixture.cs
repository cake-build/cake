using Cake.Common.Tools.SpecFlow.StepDefinitionReport;
using Cake.Core.IO;

namespace Cake.Common.Tests.Fixtures.Tools.SpecFlow.StepDefinitionReport
{
    internal sealed class SpecFlowStepDefinitionReporterFixture : SpecFlowFixture<SpecFlowStepDefinitionReportSettings>
    {
        public FilePath ProjectFile { get; set; }

        public SpecFlowStepDefinitionReporterFixture()
        {
            // Set the project file.
            ProjectFile = new FilePath("./Tests.csproj");
        }

        protected override void RunTool()
        {
            var tool = new SpecFlowStepDefinitionReporter(FileSystem, Environment, ProcessRunner, Globber);
            tool.Run(ProjectFile, Settings);
        }
    }
}
