// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Core.Text;
using Xunit;

namespace Cake.Core.Tests.Unit.Text
{
    public sealed class TextTransformationTemplateTests
    {
        public sealed class TheConstructor
        {
            [Fact]
            public void Should_Throw_If_Provided_Template_Is_Null()
            {
                // Given, When
                var result = Record.Exception(() => new TextTransformationTemplate(null));

                // Then
                AssertEx.IsArgumentNullException(result, "template");
            }
        }

        public sealed class TheRegisterMethod
        {
            [Fact]
            public void Should_Throw_If_Key_Is_Null()
            {
                // Given
                var transformation = new TextTransformationTemplate("template");

                // When
                var result = Record.Exception(() => transformation.Register(null, "value"));

                // Then
                AssertEx.IsArgumentNullException(result, "key");
            }

            [Theory]
            [InlineData("")]
            [InlineData("\t")]
            public void Should_Throw_If_Key_Is_Empty(string key)
            {
                // Given
                var transformation = new TextTransformationTemplate("template");

                // When
                var result = Record.Exception(() => transformation.Register(key, "value"));

                // Then
                Assert.IsType<ArgumentException>(result);
                Assert.Equal("key", ((ArgumentException)result)?.ParamName);
                Assert.Equal($"Key cannot be empty.{Environment.NewLine}Parameter name: key", result?.Message);
            }

            [Theory]
            [InlineData("key", "key")]
            [InlineData("KEY", "key")]
            public void Should_Throw_If_Two_Tokens_With_The_Same_Key_Are_Added(string first, string second)
            {
                // Given
                var transformation = new TextTransformationTemplate("template");
                transformation.Register(first, "value");

                // When
                var result = Record.Exception(() => transformation.Register(second, "othervalue"));

                // Then
                Assert.IsType<InvalidOperationException>(result);
                Assert.Equal("The key 'key' has already been added.", result?.Message);
            }
        }

        public sealed class TheRenderMethod
        {
            [Fact]
            public void Should_Replace_Tokens()
            {
                // Given
                var transformation = new TextTransformationTemplate("<%greeting%> world!");
                transformation.Register("greeting", "Hello");

                // When
                var result = transformation.Render();

                // Then
                Assert.Equal("Hello world!", result);
            }

            [Fact]
            public void Should_Replace_Tokens_With_Different_Placeholder()
            {
                // Given
                var placeholder = new Tuple<string, string>("{{", "}}");
                var transformation = new TextTransformationTemplate("{{greeting}} world!", placeholder);
                transformation.Register("greeting", "Hello");

                // When
                var result = transformation.Render();

                // Then
                Assert.Equal("Hello world!", result);
            }

            [Fact]
            public void Should_Keep_Unmatched_Tokens()
            {
                // Given
                var transformation = new TextTransformationTemplate("<%greeting%> <%subject%>!");
                transformation.Register("greeting", "Hello");

                // When
                var result = transformation.Render();

                // Then
                Assert.Equal("Hello <%subject%>!", result);
            }

            [Fact]
            public void Should_Keep_Unmatched_Tokens_With_Different_Placeholder()
            {
                // Given
                var placeholder = new Tuple<string, string>("{{", "}}");
                var transformation = new TextTransformationTemplate("{{greeting}} {{subject}}!", placeholder);
                transformation.Register("greeting", "Hello");

                // When
                var result = transformation.Render();

                // Then
                Assert.Equal("Hello {{subject}}!", result);
            }

            [Theory]
            [InlineData("Today's date is <%date:yyyy-MM-dd%>!", "Today's date is 2014-12-23!")]
            [InlineData("Today's date and time is <%date:yyyy-MM-dd HH:mm:ss%>!", "Today's date and time is 2014-12-23 18:27:43!")]
            public void Should_Apply_Format_If_Provided_And_Applicable(string template, string expected)
            {
                // Given
                var transformation = new TextTransformationTemplate(template);
                transformation.Register("date", new DateTime(2014, 12, 23, 18, 27, 43));

                // When
                var result = transformation.Render();

                // Then
                Assert.Equal(expected, result);
            }

            [Theory]
            [InlineData("<%guid:D %>")]
            [InlineData("<% guid:D%>")]
            [InlineData("<% guid:D %>")]
            [InlineData("<%guid %>")]
            [InlineData("<% guid%>")]
            [InlineData("<% guid %>")]
            public void Should_Trim_Content_Of_Token(string template)
            {
                // Given
                var transformation = new TextTransformationTemplate(template);
                transformation.Register("guid", new Guid("A700936F-8E15-4BB7-82CE-312547B66440"));

                // When
                var result = transformation.Render();

                // Then
                Assert.Equal("a700936f-8e15-4bb7-82ce-312547b66440", result);
            }

            [Fact]
            public void Should_Return_Key_If_The_Value_Was_Not_Formattable()
            {
                // Given
                var transformation = new TextTransformationTemplate("Hello <%pointer:foo%>");
                transformation.Register("pointer", IntPtr.Zero);

                // When
                var result = transformation.Render();

                // Then
                Assert.Equal("Hello <%pointer:foo%>", result);
            }

            [Fact]
            public void Should_Return_Key_If_The_Value_Was_Not_Formattable_With_Different_Placeholder()
            {
                // Given
                var placeholder = new Tuple<string, string>("{{", "}}");
                var transformation = new TextTransformationTemplate("Hello {{pointer:foo}}", placeholder);
                transformation.Register("pointer", IntPtr.Zero);

                // When
                var result = transformation.Render();

                // Then
                Assert.Equal("Hello {{pointer:foo}}", result);
            }

            [Fact]
            public void Should_Replace_Null_Token_Value_With_Empty_String()
            {
                // Given
                var transformation = new TextTransformationTemplate("Hello <%subject%>!");
                transformation.Register("subject", null);

                // When
                var result = transformation.Render();

                // Then
                Assert.Equal("Hello !", result);
            }

            [Fact]
            public void Should_Replace_Formated_Null_Token_Value_With_Empty_String()
            {
                // Given
                var transformation = new TextTransformationTemplate("Hello <%subject:foo%>!");
                transformation.Register("subject", null);

                // When
                var result = transformation.Render();

                // Then
                Assert.Equal("Hello !", result);
            }

            [Fact]
            public void Should_Invoke_ToString_On_Provided_Value()
            {
                // Given
                var transformation = new TextTransformationTemplate("Hello <%subject%>!");
                transformation.Register("subject", new Guid("A700936F-8E15-4BB7-82CE-312547B66440"));

                // When
                var result = transformation.Render();

                // Then
                Assert.Equal("Hello a700936f-8e15-4bb7-82ce-312547b66440!", result);
            }

            [Fact]
            public void Should_Escape_Regex_Characters_In_Text()
            {
                // Given
                var transformation = new TextTransformationTemplate(".$^{[(|)*+?\\");

                // When
                var result = transformation.Render();

                // Then
                Assert.Equal(".$^{[(|)*+?\\", result);
            }

            [Theory]
            [InlineData(".")]
            [InlineData("$")]
            [InlineData("^")]
            [InlineData("{")]
            [InlineData("[")]
            [InlineData("(")]
            [InlineData("|")]
            [InlineData(")")]
            [InlineData("*")]
            [InlineData("+")]
            [InlineData("?")]
            public void Should_Escape_Regex_Characters_In_Placeholder(string placeholder)
            {
                // Given
                var template = string.Concat("Hello ", placeholder, "subject", placeholder, "!");
                var transformation = new TextTransformationTemplate(template, Tuple.Create(placeholder, placeholder));
                transformation.Register("subject", "world");

                // When
                var result = transformation.Render();

                // Then
                Assert.Equal("Hello world!", result);
            }
        }
    }
}