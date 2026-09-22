// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Testing;
using Xunit;

namespace Cake.Core.Tests.Unit
{
    public sealed class FakeConsoleTests
    {
        public sealed class TheWriteMethod
        {
            [Fact]
            public void Should_Write_Literal_String_Without_Formatting()
            {
                // Given
                var console = new FakeConsole();

                // When
                console.Write("{0}");
                console.WriteLine();

                // Then
                Assert.Equal(new[] { "{0}" }, console.Messages);
            }

            [Fact]
            public void Should_Format_When_Arguments_Are_Provided()
            {
                // Given
                var console = new FakeConsole();

                // When
                console.Write("{0}", "x");
                console.WriteLine();

                // Then
                Assert.Equal(new[] { "x" }, console.Messages);
            }
        }

        public sealed class TheWriteLineMethod
        {
            [Fact]
            public void Should_Write_Literal_String_Without_Formatting()
            {
                // Given
                var console = new FakeConsole();

                // When
                console.WriteLine("{0}");

                // Then
                Assert.Equal(new[] { "{0}" }, console.Messages);
            }

            [Fact]
            public void Should_Format_When_Arguments_Are_Provided()
            {
                // Given
                var console = new FakeConsole();

                // When
                console.WriteLine("{0}", "x");

                // Then
                Assert.Equal(new[] { "x" }, console.Messages);
            }
        }

        public sealed class TheWriteErrorMethod
        {
            [Fact]
            public void Should_Write_Literal_String_Without_Formatting()
            {
                // Given
                var console = new FakeConsole();

                // When
                console.WriteError("{0}");
                console.WriteErrorLine();

                // Then
                Assert.Equal(new[] { "{0}" }, console.ErrorMessages);
            }

            [Fact]
            public void Should_Format_When_Arguments_Are_Provided()
            {
                // Given
                var console = new FakeConsole();

                // When
                console.WriteError("{0}", "x");
                console.WriteErrorLine();

                // Then
                Assert.Equal(new[] { "x" }, console.ErrorMessages);
            }
        }

        public sealed class TheWriteErrorLineMethod
        {
            [Fact]
            public void Should_Write_Literal_String_Without_Formatting()
            {
                // Given
                var console = new FakeConsole();

                // When
                console.WriteErrorLine("{0}");

                // Then
                Assert.Equal(new[] { "{0}" }, console.ErrorMessages);
            }

            [Fact]
            public void Should_Format_When_Arguments_Are_Provided()
            {
                // Given
                var console = new FakeConsole();

                // When
                console.WriteErrorLine("{0}", "x");

                // Then
                Assert.Equal(new[] { "x" }, console.ErrorMessages);
            }
        }
    }
}
