// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using NSubstitute;
using Xunit;

namespace Cake.Core.Tests.Unit
{
    public sealed class CakeRuntimeTests
    {
        public sealed class TheConstructor
        {
            [Fact]
            public void Should_Throw_If_Arguments_Is_Null()
            {
                // Given
                // When
                var result = Record.Exception(() => new CakeRuntime(null));

                // Then
                Assert.IsArgumentNullException(result, "arguments");
            }
        }

        public sealed class TheIsDebugProperty
        {
            [Theory]
            [InlineData("d","True")]
            [InlineData("d", "")]
            [InlineData("debug", null)]
            [InlineData("debug", "true")]
            public void Should_Return_True_If_Debug_Argument_Is_True(string key, string value)
            {
                // Given
                var arguments = Substitute.For<ICakeArguments>();
                arguments.HasArgument(key).Returns(true);
                arguments.GetArgument(key).Returns(value);
                var runtime = new CakeRuntime(arguments);

                // When
                var result = runtime.IsDebug;

                // Then
                Assert.True(result);
            }

            [Theory]
            [InlineData("d", "False")]
            [InlineData("de", "")]
            [InlineData("debug", false)]
            public void Should_Return_False_If_Debug_Argument_Is_False_Or_Missing(string key, string value)
            {
                // Given
                var arguments = Substitute.For<ICakeArguments>();
                arguments.HasArgument(key).Returns(true);
                arguments.GetArgument(key).Returns(value);
                var runtime = new CakeRuntime(arguments);

                // When
                var result = runtime.IsDebug;

                // Then
                Assert.False(result);
            }
        }
    }
}
