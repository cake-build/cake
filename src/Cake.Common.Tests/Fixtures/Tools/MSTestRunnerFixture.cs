// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Cake.Common.Tools.MSTest;
using Cake.Core.IO;
using Cake.Testing.Fixtures;
using System.Collections.Generic;

namespace Cake.Common.Tests.Fixtures.Tools
{
    internal sealed class MSTestRunnerFixture : ToolFixture<MSTestSettings>
    {
        public IEnumerable<FilePath> AssemblyPaths { get; set; }

        public MSTestRunnerFixture()
            : base("mstest.exe")
        {
            AssemblyPaths = new[] { new FilePath("./Test1.dll") };
            Environment.SetSpecialPath(SpecialPath.ProgramFilesX86, "/ProgramFilesX86");
        }

        protected override FilePath GetDefaultToolPath(string toolFilename)
        {
            return new FilePath("/ProgramFilesX86/Microsoft Visual Studio 12.0/Common7/IDE/mstest.exe");
        }

        protected override void RunTool()
        {
            var tool = new MSTestRunner(FileSystem, Environment, ProcessRunner, Tools);
            tool.Run(AssemblyPaths, Settings);
        }
    }
}
