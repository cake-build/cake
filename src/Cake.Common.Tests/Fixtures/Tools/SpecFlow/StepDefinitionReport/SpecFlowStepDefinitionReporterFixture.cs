// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
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
            var tool = new SpecFlowStepDefinitionReporter(FileSystem, Environment, ProcessRunner, Tools);
            tool.Run(ProjectFile, Settings);
        }
    }
}
