// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Xunit;

namespace Cake.Core.Tests.Unit.Extensions;

public sealed class StringExtensionsTests
{
    public sealed class TheQuoteMethod
    {
        [Theory]
        [InlineData("Hello World", "\"Hello World\"")]
        [InlineData("", "\"\"")]
        [InlineData("123", "\"123\"")]
        [InlineData("123\\", "\"123\\\\\"")]
        [InlineData(" 1 2 3 \\", "\" 1 2 3 \\\\\"")]
        [InlineData("1\"2\"3", "\"1\\\"2\\\"3\"")]
        [InlineData("\"already quoted\"", "\"already quoted\"")]
        [InlineData("\"foo\\\"", "\"\\\"foo\\\\\\\"\"")]
        public void Should_Quote_Literal_Values(string value, string expected)
        {
            // Given, When
            var result = value.Quote();

            // Then
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Should_Quote_Null_As_Empty()
        {
            // Given
            string value = null;

            // When
            var result = value.Quote();

            // Then
            Assert.Equal("\"\"", result);
        }
    }

    public sealed class TheUnQuoteMethod
    {
        [Theory]
        [InlineData("hello", "hello")]
        [InlineData("\"hello\"", "hello")]
        [InlineData("\"Hello World\"", "Hello World")]
        [InlineData("\"123\\\\\"", "123\\")]
        [InlineData("\"\\\"1\\\"2\\\"3\\\"\"", "\"1\"2\"3\"")]
        [InlineData("[REDACTED]", "[REDACTED]")]
        [InlineData("", "")]
        [InlineData(null, "")]
        public void Should_Unescape_Quoted_Process_Arguments(string value, string expected)
        {
            // Given, When
            var result = value.UnQuote();

            // Then
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Should_Roundtrip_Quote()
        {
            // Given
            const string literal = "C:\\My Folder\\";

            // When
            var result = literal.Quote().UnQuote();

            // Then
            Assert.Equal(literal, result);
        }
    }
}
