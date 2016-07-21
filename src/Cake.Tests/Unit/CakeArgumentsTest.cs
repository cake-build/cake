// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Xunit;

namespace Cake.Tests.Unit
{
    public sealed class CakeArgumentsTests
    {
        public sealed class AsDictionary
        {
            [Fact]
            public void Should_Return_All_Arguments()
            {
                // Given
                var options = new CakeOptions();
                options.Arguments.Add("A", "B");
                options.Arguments.Add("C", "D");
                var arguments = new CakeArguments(options);

                // When
                var result = arguments.AsDictionary;

                // Then
                var expected = new Dictionary<string, string>
                {
                    { "A", "B" },
                    { "C", "D" }
                };
                Assert.Equal(expected, result);
            }
        }

        public sealed class DefinedArgumentNames
        {
            [Theory]
            [InlineData(new string[] { "A" }, new string[] { "A" } )]
            [InlineData(new string[] { "A", "B" }, new string[] { "A", "B" })]
            public void Should_Return_All_Defined_Arguments(string[] defined, string[] expected)
            {
                // Given
                var options = new CakeOptions();
                options.Arguments.Add("A", "B");
                options.Arguments.Add("C", "D");
                var arguments = new CakeArguments(options);

                foreach (var arg in defined)
                {
                    arguments.GetArgument(arg);
                }

                // When
                var result = arguments.DefinedArgumentNames;

                // Then
                Assert.Equal(expected, result);
            }
        }

        public sealed class UnrecognizedArgumentNames
        {
            [Theory]
            [InlineData(new string[] { "A" }, new string[] { "C" })]
            [InlineData(new string[] { "A", "B", "C" }, new string[] { })]
            public void Should_Return_All_Unrecognized_Arguments(string[] defined, string[] expected)
            {
                // Given
                var options = new CakeOptions();
                options.Arguments.Add("A", "B");
                options.Arguments.Add("C", "D");
                var arguments = new CakeArguments(options);

                foreach (var arg in defined)
                {
                    arguments.GetArgument(arg);
                }

                // When
                var result = arguments.UnrecognizedArgumentNames;

                // Then
                Assert.Equal(expected, result);
            }
        }

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

            [Theory]
            [InlineData("a", true)]
            [InlineData("  a  ", true)]
            [InlineData("b", false)]
            [InlineData("  b  ", false)]
            public void Should_Trim_Argument_Name(string key, bool expected)
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

            [Theory]
            [InlineData("a", "B")]
            [InlineData("  a  ", "B")]
            public void Should_Trim_Argument_Name(string key, string expected)
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
