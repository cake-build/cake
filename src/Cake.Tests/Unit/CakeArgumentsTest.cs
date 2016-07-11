// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Xunit;

namespace Cake.Tests.Unit
{
    public sealed class CakeArgumentsTests
    {
        public sealed class HasArguments
        {
            [Theory]
            [InlineData("A", true)]
            [InlineData("B", false)]
            public void Should_Return_Whether_Or_Not_An_Argument_Exist(string key, bool expected)
            {
                // Given
                var options = new CakeOptions();
                options.Arguments.Add("A", "B");
                var arguments = new CakeArguments(options);

                // When
                var result = arguments.HasArgument(key);

                // Then
                Assert.Equal(expected, result);
            }

            [Theory]
            [InlineData("a", true)]
            [InlineData("b", false)]
            public void Should_Be_Case_Insensitive(string key, bool expected)
            {
                // Given
                var options = new CakeOptions();
                options.Arguments.Add("A", "B");
                var arguments = new CakeArguments(options);

                // When
                var result = arguments.HasArgument(key);

                // Then
                Assert.Equal(expected, result);
            }
        }

        public sealed class GetArguments
        {
            [Theory]
            [InlineData("A", "B")]
            [InlineData("B", null)]
            public void Should_Return_Argument_Exist(string key, string expected)
            {
                // Given
                var options = new CakeOptions();
                options.Arguments.Add("A", "B");
                var arguments = new CakeArguments(options);

                // When
                var result = arguments.GetArgument(key);

                // Then
                Assert.Equal(expected, result);
            }

            [Theory]
            [InlineData("a", "B")]
            [InlineData("b", null)]
            public void Should_Be_Case_Insensitive(string key, string expected)
            {
                // Given
                var options = new CakeOptions();
                options.Arguments.Add("A", "B");
                var arguments = new CakeArguments(options);

                // When
                var result = arguments.GetArgument(key);

                // Then
                Assert.Equal(expected, result);
            }
        }
    }
}
