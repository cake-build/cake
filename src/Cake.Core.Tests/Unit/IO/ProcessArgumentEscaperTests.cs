// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core.IO;
using Xunit;

namespace Cake.Core.Tests.Unit.IO;

public sealed class ProcessArgumentEscaperTests
{
    public sealed class TheEscapeMethod
    {
        [Theory]
        [InlineData(null, "\"\"")]
        [InlineData("", "\"\"")]
        [InlineData("123", "123")]
        [InlineData("123\\", "123\\")]
        [InlineData(" 1 2 3 ", "\" 1 2 3 \"")]
        [InlineData(" 1 2 3 \\", "\" 1 2 3 \\\\\"")]
        [InlineData("\"1\"2\"3\"", "\"\\\"1\\\"2\\\"3\\\"\"")]
        [InlineData("1\\2\\\\3\\\\\\", "1\\2\\\\3\\\\\\")]
        [InlineData("\\\\\"", "\"\\\\\\\\\\\"\"")]
        public void Should_Escape_According_To_Windows_Argv_Rules(string value, string expected)
        {
            // Given, When
            var result = ProcessArgumentEscaper.Escape(value);

            // Then
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(null, "\"\"")]
        [InlineData("", "\"\"")]
        [InlineData("123", "\"123\"")]
        [InlineData("123\\", "\"123\\\\\"")]
        [InlineData(" 1 2 3 ", "\" 1 2 3 \"")]
        [InlineData(" 1 2 3 \\", "\" 1 2 3 \\\\\"")]
        [InlineData("1\\2\\\\3\\\\\\", "\"1\\2\\\\3\\\\\\\\\\\\\"")]
        [InlineData("Hello World", "\"Hello World\"")]
        public void Should_Always_Quote_When_Requested(string value, string expected)
        {
            // Given, When
            var result = ProcessArgumentEscaper.Escape(value, alwaysQuote: true);

            // Then
            Assert.Equal(expected, result);
        }
    }

    public sealed class TheUnquoteMethod
    {
        [Theory]
        [InlineData(null, "")]
        [InlineData("", "")]
        [InlineData("123", "123")]
        [InlineData("\"123\"", "123")]
        [InlineData("\"123\\\\\"", "123\\")]
        [InlineData("\" 1 2 3 \"", " 1 2 3 ")]
        [InlineData("\" 1 2 3 \\\\\"", " 1 2 3 \\")]
        [InlineData("\"\\\"1\\\"2\\\"3\\\"\"", "\"1\"2\"3\"")]
        [InlineData("\"\\\\\\\\\\\"\"", "\\\\\"")]
        [InlineData("[REDACTED]", "[REDACTED]")]
        public void Should_Recover_Literal_Value(string value, string expected)
        {
            // Given, When
            var result = ProcessArgumentEscaper.Unquote(value);

            // Then
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("123")]
        [InlineData("123\\")]
        [InlineData(" 1 2 3 ")]
        [InlineData(" 1 2 3 \\")]
        [InlineData("\"1\"2\"3\"")]
        [InlineData("1\\2\\\\3\\\\\\")]
        [InlineData("\\\\\"")]
        public void Should_Roundtrip_Always_Quoted_Values(string value)
        {
            // Given
            var escaped = ProcessArgumentEscaper.Escape(value, alwaysQuote: true);

            // When
            var result = ProcessArgumentEscaper.Unquote(escaped);

            // Then
            Assert.Equal(value ?? string.Empty, result);
        }
    }

    public sealed class TheIsQuotedMethod
    {
        [Theory]
        [InlineData("\"hello\"", true)]
        [InlineData("\"123\\\\\"", true)]
        [InlineData("hello", false)]
        [InlineData("\"foo\\\"", false)]
        [InlineData("", false)]
        [InlineData(null, false)]
        [InlineData("\"", false)]
        public void Should_Detect_Properly_Quoted_Tokens(string value, bool expected)
        {
            // Given, When
            var result = ProcessArgumentEscaper.IsQuoted(value);

            // Then
            Assert.Equal(expected, result);
        }
    }
}
