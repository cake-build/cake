// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Cake.Scripting.Mono.CodeGen.Parsing;
using Xunit;

namespace Cake.Tests.Unit.Scripting.Mono
{
    public sealed class ScriptTokenizerTests
    {
        public sealed class TheGetNextTokenMethod
        {
            [Theory]
            [InlineData("\"Hello World\"", 0, 13)]
            [InlineData(" \"Hello World\" ", 1, 13)]
            [InlineData(" \"Hello \\\"World\\\"\" ", 1, 17)] // "Hello \"World\""
            [InlineData("\"Hello \\\"World\\\"\" ", 0, 17)] // "Hello \"World\""
            public void Should_Parse_Strings_Correctly(string content, int start, int end)
            {
                // Given
                var tokenizer = new ScriptTokenizer(content);

                // When
                var token = tokenizer.GetNextToken();

                // Then
                Assert.Equal(ScriptTokenType.String, token.Type);
                Assert.Equal(start, token.Index);
                Assert.Equal(end, token.Length);
            }

            [Theory]
            [InlineData("Hello", 0, 5)]
            [InlineData(" Hello ", 1, 5)]
            public void Should_Parse_Word_Correctly(string content, int index, int length)
            {
                // Given
                var tokenizer = new ScriptTokenizer(content);

                // When
                var token = tokenizer.GetNextToken();

                // Then
                Assert.Equal(ScriptTokenType.Word, token.Type);
                Assert.Equal(index, token.Index);
                Assert.Equal(length, token.Length);
            }

            [Theory]
            [InlineData("/", 0, 1)]
            [InlineData(" / ", 1, 1)]
            public void Should_Parse_Character_Correctly(string content, int index, int length)
            {
                // Given
                var tokenizer = new ScriptTokenizer(content);

                // When
                var token = tokenizer.GetNextToken();

                // Then
                Assert.Equal(ScriptTokenType.Character, token.Type);
                Assert.Equal(index, token.Index);
                Assert.Equal(length, token.Length);
            }

            [Fact]
            public void Should_Skip_Single_Line_Comment()
            {
                // Given
                var tokenizer = new ScriptTokenizer("// This should be ignored \n {");

                // When
                var token = tokenizer.GetNextToken();

                // Then
                Assert.Equal(ScriptTokenType.LeftBrace, token.Type);
                Assert.Equal(28, token.Index);
                Assert.Equal(1, token.Length);
            }

            [Fact]
            public void Should_Return_Null_For_Only_A_Single_Comment()
            {
                // Given
                var tokenizer = new ScriptTokenizer("// This should be ignored");

                // When
                var token = tokenizer.GetNextToken();

                // Then
                Assert.Null(token);
            }

            [Fact]
            public void Should_Return_Null_When_EOL_Is_Reached()
            {
                // Given
                var tokenizer = new ScriptTokenizer("");

                // When
                var token = tokenizer.GetNextToken();

                // Then
                Assert.Null(token);
            }

            [Fact]
            public void Should_Skip_Multi_Line_Comment()
            {
                // Given
                var tokenizer = new ScriptTokenizer("/* This should be ignored \n   And this as well */ {");

                // When
                var token = tokenizer.GetNextToken();

                // Then
                Assert.Equal(ScriptTokenType.LeftBrace, token.Type);
                Assert.Equal(50, token.Index);
                Assert.Equal(1, token.Length);
            }

            [Fact]
            public void Should_Skip_New_Line()
            {
                // Given
                var tokenizer = new ScriptTokenizer(" \n Hello ");

                // When
                var token = tokenizer.GetNextToken();

                // Then
                Assert.Equal(ScriptTokenType.Word, token.Type);
                Assert.Equal(3, token.Index);
                Assert.Equal(5, token.Length);
            }

            [Fact]
            public void Should_Skip_Carriage_Return()
            {
                // Given
                var tokenizer = new ScriptTokenizer(" \r\n Hello ");

                // When
                var token = tokenizer.GetNextToken();

                // Then
                Assert.Equal(ScriptTokenType.Word, token.Type);
                Assert.Equal(4, token.Index);
                Assert.Equal(5, token.Length);
            }

            [Fact]
            public void Should_Parse_Left_Brace_Correctly()
            {
                // Given
                var tokenizer = new ScriptTokenizer(" { ");

                // When
                var token = tokenizer.GetNextToken();

                // Then
                Assert.Equal(ScriptTokenType.LeftBrace, token.Type);
                Assert.Equal(1, token.Index);
                Assert.Equal(1, token.Length);
            }

            [Fact]
            public void Should_Parse_Right_Brace_Correctly()
            {
                // Given
                var tokenizer = new ScriptTokenizer(" } ");

                // When
                var token = tokenizer.GetNextToken();

                // Then
                Assert.Equal(ScriptTokenType.RightBrace, token.Type);
                Assert.Equal(1, token.Index);
                Assert.Equal(1, token.Length);
            }

            [Fact]
            public void Should_Parse_Left_Parenthesis_Correctly()
            {
                // Given
                var tokenizer = new ScriptTokenizer(" ( ");

                // When
                var token = tokenizer.GetNextToken();

                // Then
                Assert.Equal(ScriptTokenType.LeftParenthesis, token.Type);
                Assert.Equal(1, token.Index);
                Assert.Equal(1, token.Length);
            }

            [Fact]
            public void Should_Parse_Right_Parenthesis_Correctly()
            {
                // Given
                var tokenizer = new ScriptTokenizer(" ) ");

                // When
                var token = tokenizer.GetNextToken();

                // Then
                Assert.Equal(ScriptTokenType.RightParenthesis, token.Type);
                Assert.Equal(1, token.Index);
                Assert.Equal(1, token.Length);
            }

            [Fact]
            public void Should_Parse_Semicolon_Correctly()
            {
                // Given
                var tokenizer = new ScriptTokenizer(" ; ");

                // When
                var token = tokenizer.GetNextToken();

                // Then
                Assert.Equal(ScriptTokenType.Semicolon, token.Type);
                Assert.Equal(1, token.Index);
                Assert.Equal(1, token.Length);
            }
        }
    }
}
