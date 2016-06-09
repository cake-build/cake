// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System.Collections.Generic;
using Cake.Common.Tools.ReportGenerator;
using Cake.Core.IO;
using Cake.Testing.Fixtures;

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
            var tool = new ReportGeneratorRunner(FileSystem, Environment, ProcessRunner, Tools);
            tool.Run(Reports, TargetDir, Settings);
        }
    }
}
