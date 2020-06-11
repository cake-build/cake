// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using Cake.Core.Diagnostics.Formatting;
using Xunit;

namespace Cake.Core.Tests.Unit.Diagnostics
{
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
                Assert.Single(result);
                Assert.IsType<LiteralToken>(result[0]);
                Assert.Equal("Hello World!", ((LiteralToken)result[0]).Text);
            }

            [Fact]
            public void Returns_Correct_Token_For_Message_With_One_Property()
            {
                // Given, When
                var result = FormatParser.Parse("{0}").ToArray();

                // Then
                Assert.Single(result);
                Assert.IsType<PropertyToken>(result[0]);
                Assert.Equal(0, ((PropertyToken)result[0]).Position);
            }

            [Fact]
            public void Should_Return_Literal_Tokens_For_Message_With_Nothing_But_Escaped_Braces()
            {
                // Given, When
                var result = FormatParser.Parse("{{}}").ToArray();

                // Then
                Assert.Single(result);
                Assert.IsType<LiteralToken>(result[0]);
                Assert.Equal("{}", ((LiteralToken)result[0]).Text);
            }

            [Fact]
            public void Should_Return_Literal_Tokens_For_Message_With_Escaped_Braces_And_Properties()
            {
                // Given, When
                var result = FormatParser.Parse("{{}} {0}").ToArray();

                // Then
                Assert.Equal(2, result.Length);
                Assert.IsType<LiteralToken>(result[0]);
                Assert.Equal("{} ", ((LiteralToken)result[0]).Text);

                Assert.IsType<PropertyToken>(result[1]);
                Assert.Equal(0, ((PropertyToken)result[1]).Position);
                Assert.Equal(null, ((PropertyToken)result[1]).Format);
            }

            [Fact]
            public void Should_Return_Literal_Token_For_Unbalanced_Escaped_Opening_Curly_Braces()
            {
                // Given, When
                var result = FormatParser.Parse("{{ test {0}").ToArray();

                // Then
                Assert.Equal(2, result.Length);
                Assert.IsType<LiteralToken>(result[0]);
                Assert.Equal("{ test ", ((LiteralToken)result[0]).Text);

                Assert.IsType<PropertyToken>(result[1]);
                Assert.Equal(0, ((PropertyToken)result[1]).Position);
                Assert.Equal(null, ((PropertyToken)result[1]).Format);
            }

            [Fact]
            public void Should_Return_Literal_Tokens_For_Multiple_Unbalanced_Escaped_Opening_Curly_Braces()
            {
                // Given, When
                var result = FormatParser.Parse("{{{{ test {0}").ToArray();

                // Then
                Assert.Equal(2, result.Length);
                Assert.IsType<LiteralToken>(result[0]);
                Assert.Equal("{{ test ", ((LiteralToken)result[0]).Text);

                Assert.IsType<PropertyToken>(result[1]);
                Assert.Equal(0, ((PropertyToken)result[1]).Position);
                Assert.Equal(null, ((PropertyToken)result[1]).Format);
            }

            [Fact]
            public void Should_Return_Literal_Token_For_Unbalanced_Escaped_Closing_Curly_Braces()
            {
                // Given, When
                var result = FormatParser.Parse("test {0:d} }}").ToArray();

                // Then
                Assert.Equal(3, result.Length);
                Assert.IsType<LiteralToken>(result[0]);
                Assert.Equal("test ", ((LiteralToken)result[0]).Text);

                Assert.IsType<PropertyToken>(result[1]);
                Assert.Equal(0, ((PropertyToken)result[1]).Position);
                Assert.Equal("d", ((PropertyToken)result[1]).Format);

                Assert.IsType<LiteralToken>(result[2]);
                Assert.Equal(" }", ((LiteralToken)result[2]).Text);
            }

            [Fact]
            public void Should_Return_Literal_Tokens_For_Message_With_Escaped_Braces()
            {
                // Given, When
                var result = FormatParser.Parse("{{0}}").ToArray();

                // Then
                Assert.Single(result);
                Assert.IsType<LiteralToken>(result[0]);
                Assert.Equal("{0}", ((LiteralToken)result[0]).Text);
            }

            [Fact]
            public void Should_Return_Property_Token_With_Format_For_Property_With_Format()
            {
                // Given, When
                var result = FormatParser.Parse("{0:yyyy-MM-dd}").ToArray();

                // Then
                Assert.Single(result);
                Assert.IsType<PropertyToken>(result[0]);
                Assert.Equal(0, ((PropertyToken)result[0]).Position);
                Assert.Equal("yyyy-MM-dd", ((PropertyToken)result[0]).Format);
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

                Assert.IsType<LiteralToken>(result[0]);

                Assert.IsType<PropertyToken>(result[1]);
                Assert.Equal(0, ((PropertyToken)result[1]).Position);

                Assert.IsType<LiteralToken>(result[2]);

                Assert.IsType<PropertyToken>(result[3]);
                Assert.Equal(1, ((PropertyToken)result[3]).Position);

                Assert.IsType<LiteralToken>(result[4]);
            }
        }
    }
}