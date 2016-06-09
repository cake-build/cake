// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
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
            var tool = new ReportUnitRunner(FileSystem, Environment, ProcessRunner, Tools);
            tool.Run(InputFolder, OutputFolder, Settings);
        }
    }
}
