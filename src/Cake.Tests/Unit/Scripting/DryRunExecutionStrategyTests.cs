// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core;
using Cake.Scripting;
using Cake.Testing;
using NSubstitute;
using Xunit;

namespace Cake.Tests.Unit.Scripting
{
    public sealed class DryRunExecutionStrategyTests
    {
        public sealed class TheConstructor
        {
            [Fact]
            public void Should_Throw_If_Log_Is_Null()
            {
                // Given, When
                var result = Record.Exception(() => new DryRunExecutionStrategy(null));

                // Then
                AssertEx.IsArgumentNullException(result, "log");
            }
        }

        public sealed class TheExecuteMethod
        {
            [Fact]
            public void Should_Write_Correct_Output_To_The_Log()
            {
                // Given
                var log = new FakeLog();
                var context = Substitute.For<ICakeContext>();
                var strategy = new DryRunExecutionStrategy(log);

                // When
                strategy.Execute(new ActionTask("First"), context);
                strategy.Execute(new ActionTask("Second"), context);

                // Then
                Assert.Equal(2, log.Entries.Count);
                Assert.Equal("1. First", log.Entries[0].Message);
                Assert.Equal("2. Second", log.Entries[1].Message);
            }
        }
    }
}