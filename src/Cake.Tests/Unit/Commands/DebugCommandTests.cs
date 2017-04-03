// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core.Diagnostics;
using Cake.Scripting;
using Cake.Tests.Fixtures;
using NSubstitute;
using Xunit;

namespace Cake.Tests.Unit.Commands
{
    public sealed class DebugCommandTests
    {
        public sealed class TheExecuteMethod
        {
            [Fact]
            public void Should_Throw_If_Options_Is_Null()
            {
                // Given
                var fixture = new DebugCommandFixture();
                fixture.Options = null;

                // When
                var result = Record.Exception(() => fixture.Execute());

                // Then
                AssertEx.IsArgumentNullException(result, "options");
            }

            [Fact]
            public void Should_Proxy_Call_To_ScriptRunner()
            {
                // Given
                var fixture = new DebugCommandFixture();
                var runner = fixture.ScriptRunner;
                var options = fixture.Options;

                // When
                fixture.Execute();

                // Then
                runner.Received(1).Run(Arg.Any<BuildScriptHost>(), options.Script, options.Arguments);
            }

            [Fact]
            public void Should_Report_Correct_Process_Id()
            {
                // Given
                var fixture = new DebugCommandFixture();
                var debugger = fixture.Debugger;
                var log = fixture.Log;
                var pid = 1234567;

                debugger.GetProcessId().Returns(pid);

                // When
                fixture.Execute();

                // Then
                log.Received(1).Information(Verbosity.Quiet, Arg.Any<string>(), pid);
            }
        }
    }
}