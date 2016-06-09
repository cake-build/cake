// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.Linq;
using Cake.Diagnostics.Formatting;
using Xunit;

namespace Cake.Tests.Unit.Diagnostics
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
                Assert.Equal(0, result.Length);
            }

            [Fact]
            public void Returns_Correct_Token_For_Message_With_No_Properties()
            {
                // Given, When
                var result = FormatParser.Parse("Hello World!").ToArray();

                // Then
                Assert.Equal(1, result.Length);
                Assert.IsType<LiteralToken>(result[0]);
                Assert.Equal("Hello World!", ((LiteralToken)result[0]).Text);
            }

            [Fact]
            public void Returns_Correct_Token_For_Message_With_One_Property()
            {
                // Given, When
                var result = FormatParser.Parse("{0}").ToArray();

                // Then
                Assert.Equal(1, result.Length);
                Assert.IsType<PropertyToken>(result[0]);
                Assert.Equal(0, ((PropertyToken)result[0]).Position);
            }

            [Fact]
            public void Should_Return_Literal_Tokens_For_Message_With_Escaped_Braces()
            {
                // Given, When
                var result = FormatParser.Parse("{{0}}").ToArray();

                // Then
                Assert.Equal(2, result.Length);
                Assert.IsType<LiteralToken>(result[0]);
                Assert.Equal("{{", ((LiteralToken)result[0]).Text);
                Assert.Equal("0}}", ((LiteralToken)result[1]).Text);
            }

            [Fact]
            public void Should_Return_Property_Token_With_Format_For_Property_With_Format()
            {
                // Given, When
                var result = FormatParser.Parse("{0:yyyy-MM-dd}").ToArray();

                // Then
                Assert.Equal(1, result.Length);
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
                Assert.Equal("Input string was not in a correct format.", result.Message);
            }

            [Fact]
            public void Should_Throw_If_A_Format_Item_With_Format_Is_Not_Positional()
            {
                // Given, When
                var result = Record.Exception(() => FormatParser.Parse("{Hello:yyyy-MM-dd}").ToArray());

                // Then
                Assert.IsType<FormatException>(result);
                Assert.Equal("Input string was not in a correct format.", result.Message);
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
                Assert.IsType<LiteralToken>(result[2]);
                Assert.IsType<PropertyToken>(result[3]);
                Assert.IsType<LiteralToken>(result[4]);
            }
        }
    }
}
