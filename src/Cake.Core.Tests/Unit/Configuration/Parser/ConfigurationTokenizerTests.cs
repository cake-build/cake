// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Core.Configuration.Parser;
using Cake.Core.Tests.Properties;
using Xunit;

namespace Cake.Core.Tests.Unit.Configuration.Parser
{
    public sealed class ConfigurationTokenizerTests
    {
        public sealed class TheTokenizeMethod
        {
            [Fact]
            public void Should_Tokenize_Comment()
            {
                // Given, When
                var result = ConfigurationTokenizer.Tokenize("; Hello World");

                // Then
                Assert.Equal(0, result.Count);
            }

            [Fact]
            public void Should_Tokenize_Section()
            {
                // Given, When
                var result = ConfigurationTokenizer.Tokenize("[TheSection]");

                // Then
                Assert.Equal(1, result.Count);
                Assert.Equal(ConfigurationTokenKind.Section, result[0].Kind);
                Assert.Equal("TheSection", result[0].Value);
            }

            [Theory]
            [InlineData("[TheSection")]
            [InlineData("[TheSection\nHello=World")]
            public void Should_Throw_If_Section_Is_Malformed(string section)
            {
                // Given, When
                var result = Record.Exception(() => ConfigurationTokenizer.Tokenize(section));

                // Then
                AssertEx.IsExceptionWithMessage<InvalidOperationException>(result, "Encountered malformed section.");
            }

            [Fact]
            public void Should_Tokenize_Key_And_Value_Pair()
            {
                // Given, When
                var result = ConfigurationTokenizer.Tokenize("Hello=World");

                // Then
                Assert.Equal(3, result.Count);
                Assert.Equal("Hello", result[0].Value);
                Assert.Equal(ConfigurationTokenKind.Equals, result[1].Kind);
                Assert.Equal("World", result[2].Value);
            }

            [Fact]
            public void Should_Set_Missing_Value_As_Empty_String()
            {
                // Given, When
                var result = ConfigurationTokenizer.Tokenize("Hello=");

                // Then
                Assert.Equal(2, result.Count);
                Assert.Equal("Hello", result[0].Value);
                Assert.Equal(ConfigurationTokenKind.Equals, result[1].Kind);
            }

            [Fact]
            public void Should_Tokenize_Ini_File()
            {
                // Given, When
                var result = ConfigurationTokenizer.Tokenize(Resources.Ini_Configuration);

                // Then
                Assert.Equal(8, result.Count);
                Assert.Equal(ConfigurationTokenKind.Section, result[0].Kind);
                Assert.Equal("Section1", result[0].Value);
                Assert.Equal("Foo", result[1].Value);
                Assert.Equal(ConfigurationTokenKind.Equals, result[2].Kind);
                Assert.Equal("Bar", result[3].Value);
                Assert.Equal(ConfigurationTokenKind.Section, result[4].Kind);
                Assert.Equal("Section2", result[4].Value);
                Assert.Equal("Baz", result[5].Value);
                Assert.Equal(ConfigurationTokenKind.Equals, result[6].Kind);
                Assert.Equal("Qux", result[7].Value);
            }
        }
    }
}