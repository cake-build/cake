﻿using Cake.Common.Tools.ReportUnit;
using Cake.Core.IO;

namespace Cake.Common.Tests.Fixtures.Tools.ReportUnit
{
    internal sealed class ReportUnitDirectoryFixture : ToolFixture<ReportUnitSettings>
    {
        public DirectoryPath InputFolder;
        public DirectoryPath OutputFolder;

        public ReportUnitDirectoryFixture()
            : base("ReportUnit.exe")
        {
            this.InputFolder = "c:/temp/input";
            this.OutputFolder = "c:/temp/output";
        }

        protected override void RunTool()
        {
            var tool = new ReportUnitRunner(this.FileSystem, this.Environment, this.ProcessRunner, this.Globber);
            tool.Run(this.InputFolder, this.OutputFolder, this.Settings);
        }
    }
}