// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System.Collections.Generic;
using Cake.Common.Tools.Fixie;
using Cake.Core.IO;
using Cake.Testing.Fixtures;

namespace Cake.Common.Tests.Fixtures.Tools
{
    internal sealed class FixieRunnerFixture : ToolFixture<FixieSettings>
    {
        public List<FilePath> AssemblyPaths { get; set; }

        public FixieRunnerFixture()
            : base("Fixie.Console.exe")
        {
            AssemblyPaths = new List<FilePath>();
            AssemblyPaths.Add(new FilePath("./Test1.dll"));
        }

        protected override void RunTool()
        {
            var tool = new FixieRunner(FileSystem, Environment, ProcessRunner, Tools);
            tool.Run(AssemblyPaths, Settings);
        }
    }
}
