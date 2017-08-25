// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Scripting;
using NSubstitute;
using Xunit;

namespace Cake.Tests.Unit.Scripting
{
    public sealed class DryRunScriptHostTests
    {
        public sealed class TheConstructor
        {
            [Fact]
            public void Should_Throw_If_Engine_Is_Null()
            {
                // Given
                var context = Substitute.For<ICakeContext>();
                var log = Substitute.For<ICakeLog>();

                // When
                var result = Record.Exception(() => new DryRunScriptHost(null, context, log));

                // Then
                AssertEx.IsArgumentNullException(result, "engine");
            }

            [Fact]
            public void Should_Throw_If_Context_Is_Null()
            {
                // Given
                var engine = Substitute.For<ICakeEngine>();
                var log = Substitute.For<ICakeLog>();

                // When
                var result = Record.Exception(() => new DryRunScriptHost(engine, null, log));

                // Then
                AssertEx.IsArgumentNullException(result, "context");
            }

            [Fact]
            public void Should_Throw_If_Log_Is_Null()
            {
                // Given
                var engine = Substitute.For<ICakeEngine>();
                var context = Substitute.For<ICakeContext>();

                // When
                var result = Record.Exception(() => new DryRunScriptHost(engine, context, null));

                // Then
                AssertEx.IsArgumentNullException(result, "log");
            }
        }

        public sealed class TheRunTargetMethod
        {
            [Fact]
            public void Should_Invoke_The_Engine_With_Correct_Execution_Strategy()
            {
                // Given
                var engine = Substitute.For<ICakeEngine>();
                var context = Substitute.For<ICakeContext>();
                var log = Substitute.For<ICakeLog>();
                var host = new DryRunScriptHost(engine, context, log);

                // When
                host.RunTarget("TheTarget");

                // Then
                engine.Received(1).RunTarget(context, Arg.Any<DryRunExecutionStrategy>(), "TheTarget");
            }
        }
    }
}