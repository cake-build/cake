// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Cake.Common.Tools.GitVersion;
using Cake.Core.Diagnostics;
using Cake.Testing.Fixtures;
using NSubstitute;

namespace Cake.Common.Tests.Fixtures.Tools
{
    internal sealed class GitVersionRunnerFixture : ToolFixture<GitVersionSettings>
    {
        public ICakeLog Log { get; set; }

        public GitVersionRunnerFixture(ICollection<string> standardOutput = null)
             : base("GitVersion.exe")
        {
            if (standardOutput == null)
            {
                // Minimal GitVersion-style JSON (string values match GitVersion CLI output).
                standardOutput =
                    [
                        """
                        {
                            "Major":"1",
                            "Minor":"0",
                            "Patch":"0"
                        }
                        """
                    ];
            }

            ProcessRunner.Process.SetStandardOutput(standardOutput);
            Log = Substitute.For<ICakeLog>();
            Log.Verbosity = Verbosity.Normal;
        }

        public void SetLogVerbosity(Verbosity verbosity)
        {
            Log.Verbosity = verbosity;
        }

        protected override void RunTool()
        {
            RunGitVersion();
        }

        public GitVersion RunGitVersion()
        {
            var tool = new GitVersionRunner(FileSystem, Environment, ProcessRunner, Tools, Log);
            return tool.Run(Settings);
        }
    }
}