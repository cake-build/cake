// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Cake.Common.Tools.MSpec;
using Cake.Core.IO;
using Cake.Testing.Fixtures;

namespace Cake.Common.Tests.Fixtures.Tools
{
    internal sealed class MSpecRunnerFixture : ToolFixture<MSpecSettings>
    {
        public List<FilePath> Assemblies { get; set; }

        public MSpecRunnerFixture()
            : base("mspec-clr4.exe")
        {
            Assemblies = new List<FilePath>();
            Assemblies.Add(new FilePath("./Test1.dll"));
        }

        public MSpecRunnerFixture(string toolFileName) : base(toolFileName)
        {
            Assemblies = new List<FilePath>();
            Assemblies.Add(new FilePath("./Test1.dll"));
        }

        protected override void RunTool()
        {
            var tool = new MSpecRunner(FileSystem, Environment, ProcessRunner, Tools);
            tool.Run(Assemblies, Settings);
        }
    }
}