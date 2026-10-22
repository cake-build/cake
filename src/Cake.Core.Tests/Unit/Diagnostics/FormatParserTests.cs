// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using Cake.Core.Diagnostics.Formatting;
using Xunit;

namespace Cake.Core.Tests.Unit.Diagnostics;

public class FormatParserTests
{
    public sealed class TheParseMethod : FormatParserTests
    {
        [Fact]
        public void Return_No_Token_For_An_Empty_Format()
        {
            // Given, When
            var result = FormatParser.Parse(string.Empty).ToArray();

            // Then
            Assert.Empty(result);
        }

        [Fact]
        public void Returns_Correct_Token_For_Message_With_No_Properties()
        {
            // Given, When
            var result = FormatParser.Parse("Hello World!").ToArray();

            // Then
            var token = Assert.IsType<LiteralToken>(Assert.Single(result));
            Assert.Equal("Hello World!", token.Text);
        }

        [Fact]
        public void Returns_Correct_Token_For_Message_With_One_Property()
        {
            // Given, When
            var result = FormatParser.Parse("{0}").ToArray();

            // Then
            var token = Assert.IsType<PropertyToken>(Assert.Single(result));
            Assert.Equal(0, token.Position);
        }

        [Fact]
        public void Should_Return_Literal_Tokens_For_Message_With_Nothing_But_Escaped_Braces()
        {
            // Given, When
            var result = FormatParser.Parse("{{}}").ToArray();

            // Then
            var token = Assert.IsType<LiteralToken>(Assert.Single(result));
            Assert.Equal("{}", token.Text);
        }

        [Fact]
        public void Should_Return_Literal_Tokens_For_Message_With_Escaped_Braces_And_Properties()
        {
            // Given, When
            var result = FormatParser.Parse("{{}} {0}").ToArray();

            // Then
            Assert.Equal(2, result.Length);
            var token0 = Assert.IsType<LiteralToken>(result[0]);
            Assert.Equal("{} ", token0.Text);

            var token1 = Assert.IsType<PropertyToken>(result[1]);
            Assert.Equal(0, token1.Position);
            Assert.Null(token1.Format);
        }

        [Fact]
        public void Should_Return_Literal_Token_For_Unbalanced_Escaped_Opening_Curly_Braces()
        {
            // Given, When
            var result = FormatParser.Parse("{{ test {0}").ToArray();

            // Then
            Assert.Equal(2, result.Length);
            var token0 = Assert.IsType<LiteralToken>(result[0]);
            Assert.Equal("{ test ", token0.Text);

            var token1 = Assert.IsType<PropertyToken>(result[1]);
            Assert.Equal(0, token1.Position);
            Assert.Null(token1.Format);
        }

        [Fact]
        public void Should_Return_Literal_Tokens_For_Multiple_Unbalanced_Escaped_Opening_Curly_Braces()
        {
            // Given, When
            var result = FormatParser.Parse("{{{{ test {0}").ToArray();

            // Then
            Assert.Equal(2, result.Length);
            var token0 = Assert.IsType<LiteralToken>(result[0]);
            Assert.Equal("{{ test ", token0.Text);

            var token1 = Assert.IsType<PropertyToken>(result[1]);
            Assert.Equal(0, token1.Position);
            Assert.Null(token1.Format);
        }

        [Fact]
        public void Should_Return_Literal_Token_For_Unbalanced_Escaped_Closing_Curly_Braces()
        {
            // Given, When
            var result = FormatParser.Parse("test {0:d} }}").ToArray();

            // Then
            Assert.Equal(3, result.Length);
            var token0 = Assert.IsType<LiteralToken>(result[0]);
            Assert.Equal("test ", token0.Text);

            var token1 = Assert.IsType<PropertyToken>(result[1]);
            Assert.Equal(0, token1.Position);
            Assert.Equal("d", token1.Format);

            var token2 = Assert.IsType<LiteralToken>(result[2]);
            Assert.Equal(" }", token2.Text);
        }

        [Fact]
        public void Should_Return_Literal_Tokens_For_Message_With_Escaped_Braces()
        {
            // Given, When
            var result = FormatParser.Parse("{{0}}").ToArray();

            // Then
            var token = Assert.IsType<LiteralToken>(Assert.Single(result));
            Assert.Equal("{0}", token.Text);
        }

        [Fact]
        public void Should_Return_Property_Token_With_Format_For_Property_With_Format()
        {
            // Given, When
            var result = FormatParser.Parse("{0:yyyy-MM-dd}").ToArray();

            // Then
            var token = Assert.IsType<PropertyToken>(Assert.Single(result));
            Assert.Equal(0, token.Position);
            Assert.Equal("yyyy-MM-dd", token.Format);
        }

        [Fact]
        public void Should_Preserve_Colons_In_Format_For_Property_With_Format()
        {
            // Given, When
            var result = FormatParser.Parse("{0:HH:mm:ss}").ToArray();

            // Then
            var token = Assert.IsType<PropertyToken>(Assert.Single(result));
            Assert.Equal(0, token.Position);
            Assert.Equal("HH:mm:ss", token.Format);
        }

        [Theory]
        [InlineData("{0,10}", 10, null)]
        [InlineData("{0,-5}", -5, null)]
        [InlineData("{0, 5}", 5, null)]
        [InlineData("{0,-5:N2}", -5, "N2")]
        [InlineData("{0,8:HH:mm}", 8, "HH:mm")]
        public void Should_Return_Property_Token_With_Alignment_For_Property_With_Alignment(string format, int expectedAlignment, string expectedFormat)
        {
            // Given, When
            var result = FormatParser.Parse(format).ToArray();

            // Then
            var token = Assert.IsType<PropertyToken>(Assert.Single(result));
            Assert.Equal(0, token.Position);
            Assert.Equal(expectedAlignment, token.Alignment);
            Assert.Equal(expectedFormat, token.Format);
        }

        [Theory]
        [InlineData("{0,}")]
        [InlineData("{0,abc}")]
        [InlineData("{Hello,5}")]
        public void Should_Throw_If_A_Format_Item_Has_Invalid_Alignment_Or_Position(string format)
        {
            // Given, When
            var result = Record.Exception(() => FormatParser.Parse(format).ToArray());

            // Then
            Assert.IsType<FormatException>(result);
            Assert.Equal("Input string was not in a correct format.", result?.Message);
        }

        [Fact]
        public void Should_Throw_If_A_Format_Item_Is_Not_Positional()
        {
            // Given, When
            var result = Record.Exception(() => FormatParser.Parse("{Hello}").ToArray());

            // Then
            Assert.IsType<FormatException>(result);
            Assert.Equal("Input string was not in a correct format.", result?.Message);
        }

        [Fact]
        public void Should_Throw_If_A_Format_Item_Is_Not_Positional_Even_If_Other_Valid_Tokens_Are_Present()
        {
            // Given, When
            var result = Record.Exception(() => FormatParser.Parse("{} {0}").ToArray());

            // Then
            Assert.IsType<FormatException>(result);
            Assert.Equal("Input string was not in a correct format.", result?.Message);
        }

        [Fact]
        public void Should_Throw_If_A_Format_Item_With_Format_Is_Not_Positional()
        {
            // Given, When
            var result = Record.Exception(() => FormatParser.Parse("{Hello:yyyy-MM-dd}").ToArray());

            // Then
            Assert.IsType<FormatException>(result);
            Assert.Equal("Input string was not in a correct format.", result?.Message);
        }

        [Fact]
        public void Should_Return_Correct_Tokens_For_Message_Which_Mixes_Properties_And_Literals()
        {
            // Given, When
            var result = FormatParser.Parse("Hello {0}! My name is {1}!").ToArray();

            // Then
            Assert.Equal(5, result.Length);

            var token0 = Assert.IsType<LiteralToken>(result[0]);
            Assert.Equal("Hello ", token0.Text);

            var token1 = Assert.IsType<PropertyToken>(result[1]);
            Assert.Equal(0, token1.Position);

            var token2 = Assert.IsType<LiteralToken>(result[2]);
            Assert.Equal("! My name is ", token2.Text);

            var token3 = Assert.IsType<PropertyToken>(result[3]);
            Assert.Equal(1, token3.Position);

            var token4 = Assert.IsType<LiteralToken>(result[4]);
            Assert.Equal("!", token4.Text);
        }
    }
}
