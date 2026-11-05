// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core;
using NSubstitute;
using Spectre.Console.Testing;

namespace Cake.Cli.Tests.Unit;

public class DescriptionScriptHostTests
{
    public sealed class TheConstructor
    {
        [Fact]
        public void Should_Throw_If_Console_Is_Null()
        {
            // Given
            var engine = Substitute.For<ICakeEngine>();
            var context = Substitute.For<ICakeContext>();

            // When
            var result = Record.Exception(() => new DescriptionScriptHost(engine, context, null));

            // Then
            AssertEx.IsArgumentNullException(result, "console");
        }
    }

    [Fact]
    public async Task RunTargetAsync_Prints_Task_Descriptions()
    {
        // Given
        var console = new TestConsole();
        var engine = Substitute.For<ICakeEngine>();
        IReadOnlyList<ICakeTaskInfo> tasks =
        [
            new CakeTask("Build") { Description = "Builds the solution" },
            new CakeTask("Clean") { Description = null },
            new CakeTask("Task[with]markup") { Description = "Handles [brackets]" },
        ];
        engine.Tasks.Returns(tasks);
        var host = new DescriptionScriptHost(engine, Substitute.For<ICakeContext>(), console);

        // When
        await host.RunTargetAsync("Build");

        // Then
        await Verify(console.Output);
    }
}
