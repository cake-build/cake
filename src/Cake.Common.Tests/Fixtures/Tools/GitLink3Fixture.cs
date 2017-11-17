// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tools.GitLink;
using Cake.Core.IO;
using Cake.Testing.Fixtures;

namespace Cake.Common.Tests.Fixtures.Tools
{
    internal sealed class GitLink3Fixture : ToolFixture<GitLink3Settings>
    {
        public FilePath PdbFilePath { get; set; }

        public GitLink3Fixture()
            : base("gitlink.exe")
        {
            PdbFilePath = new FilePath("c:/temp/my.pdb");
        }

        protected override void RunTool()
        {
            var tool = new GitLink3Runner(FileSystem, Environment, ProcessRunner, Tools);
            tool.Run(PdbFilePath, Settings);
        }
    }
}