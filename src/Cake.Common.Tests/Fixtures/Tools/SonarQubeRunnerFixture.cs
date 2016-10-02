// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Cake.Common.Tools.SonarQube;
using Cake.Core.IO;
using Cake.Testing.Fixtures;

namespace Cake.Common.Tests.Fixtures.Tools
{
    internal sealed class SonarQubeRunnerFixture : ToolFixture<SonarQubeSettings>
    {
        public FilePath SolutionPath { get; set; }

        public SonarQubeRunnerFixture()
            : base("MSBuild.SonarQube.Runner.exe")
        {
            SolutionPath = "./Test.sln";
        }

        protected override FilePath GetDefaultToolPath(string toolFilename)
        {
            return new FilePath(@"C:\ProgramData\chocolatey\lib\msbuild-sonarqube-runner\tools");
        }

        protected override void RunTool()
        {
            var tool = new SonarQubeRunner(FileSystem, Environment, ProcessRunner, Tools);
            tool.Run(SolutionPath, Settings);
        }
    }
}