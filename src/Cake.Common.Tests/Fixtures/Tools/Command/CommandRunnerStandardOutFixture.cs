// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Cake.Common.Tests.Fixtures.Tools.Command
{
    internal class CommandRunnerStandardOutputFixture : CommandRunnerFixture
    {
        public int ExitCode { get; protected set; }
        public string StandardOutput { get; protected set; }

        protected override void RunTool()
        {
            ExitCode = GetRunner().RunCommand(Arguments, out var standardOutput);
            StandardOutput = standardOutput;
        }
    }
}
