// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Cake.Common.Tools.GitReleaseNotes;
using Cake.Core.IO;
using Cake.Testing.Fixtures;

namespace Cake.Common.Tests.Fixtures.Tools
{
    internal sealed class GitReleaseNotesRunnerFixture : ToolFixture<GitReleaseNotesSettings>
    {
        public FilePath OutputFile;

        public GitReleaseNotesRunnerFixture()
            : base("GitReleaseNotes.exe")
        {
            OutputFile = "/temp/releasenotes.md";
        }

        protected override void RunTool()
        {
            var tool = new GitReleaseNotesRunner(FileSystem, Environment, ProcessRunner, Tools);
            tool.Run(OutputFile, Settings);
        }
    }
}
