// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core;
using Cake.Scripting;
using NSubstitute;
using Xunit;

namespace Cake.Tests.Unit.Scripting
{
    public sealed class DescriptionScriptHostTests
    {
        public sealed class TheConstructor
        {
            [Fact]
            public void Should_Throw_If_Engine_Is_Null()
            {
                // Given
                var context = Substitute.For<ICakeContext>();
                var console = Substitute.For<IConsole>();

                // When
                var result = Record.Exception(() => new DescriptionScriptHost(null, context, console));

                // Then
                AssertEx.IsArgumentNullException(result, "engine");
            }

            [Fact]
            public void Should_Throw_If_Context_Is_Null()
            {
                // Given
                var engine = Substitute.For<ICakeEngine>();
                var console = Substitute.For<IConsole>();

                // When
                var result = Record.Exception(() => new DescriptionScriptHost(engine, null, console));

                // Then
                AssertEx.IsArgumentNullException(result, "context");
            }

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

        public sealed class TheRunTargetMethod
        {
            [Fact]
            public void Should_Not_Call_To_Engine()
            {
                // Given
                var engine = Substitute.For<ICakeEngine>();
                var context = Substitute.For<ICakeContext>();
                var console = Substitute.For<IConsole>();
                var host = new DescriptionScriptHost(engine, context, console);

                // When
                host.RunTarget("Target");

                // Then
                engine.Received(0).RunTarget(
                    context,
                    Arg.Any<DefaultExecutionStrategy>(),
                    "Target");
            }
        }
    }
}