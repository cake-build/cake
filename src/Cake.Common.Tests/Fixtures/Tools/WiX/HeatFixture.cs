// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System.Collections.Generic;
using Cake.Common.Tools.WiX.Heat;
using Cake.Core.IO;
using Cake.Testing.Fixtures;

namespace Cake.Common.Tests.Fixtures.Tools.WiX
{
    internal sealed class HeatFixture : ToolFixture<HeatSettings>
    {
        public DirectoryPath DirectoryPath { get; set; }

        public List<FilePath> ObjectFiles { get; set; }

        public FilePath OutputFile { get; set; }

        public string HarvestType { get; set; }

        public HeatFixture()
            : base("heat.exe")
        {
            DirectoryPath = new DirectoryPath("./src/Cake");
            ObjectFiles = new List<FilePath>();
            ObjectFiles.Add(new FilePath("Cake.dll"));
            OutputFile = new FilePath("cake.wxs");
            Settings = new HeatSettings();
            Settings.HarvestType = WiXHarvestType.Dir;
            HarvestType = "Default Web Site";
        }

        protected override void RunTool()
        {
            var tool = new HeatRunner(FileSystem, Environment, ProcessRunner, Tools);
            switch(Settings.HarvestType)
            {
                case WiXHarvestType.Dir:
                    tool.Run(DirectoryPath, OutputFile, Settings);
                    break;
                case WiXHarvestType.File:
                case WiXHarvestType.Project:
                case WiXHarvestType.Reg:
                    tool.Run(ObjectFiles, OutputFile, Settings);
                    break;
                case WiXHarvestType.Website:
                case WiXHarvestType.Perf:
                    tool.Run(HarvestType, OutputFile, Settings);
                    break;
            }
        }
    }
}