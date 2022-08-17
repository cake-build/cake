// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Cake.Common.Tools.Command;
using Cake.Core.IO;
using Cake.Testing.Fixtures;

namespace Cake.Common.Tests.Fixtures.Tools.Command
{
    internal class CommandRunnerFixture : ToolFixture<CommandSettings>
    {
        public ProcessArgumentBuilder Arguments { get; set; }

        public string ToolName
        {
            get => Settings.ToolName;
            set => Settings.ToolName = value;
        }

        public ICollection<string> ToolExecutableNames
        {
            get => Settings.ToolExecutableNames;
            set => Settings.ToolExecutableNames = value;
        }

        public CommandRunnerFixture()
           : base("dotnet.exe")
        {
            Arguments = new ProcessArgumentBuilder();
            Settings.ToolName = "dotnet";
            Settings.ToolExecutableNames = new[] { "dotnet.exe", "dotnet" };
        }

        protected override void RunTool()
        {
            GetRunner().RunCommand(Arguments);
        }

        protected CommandRunner GetRunner()
            => new CommandRunner(
                            Settings,
                            FileSystem,
                            Environment,
                            ProcessRunner,
                            Tools);
    }
}
