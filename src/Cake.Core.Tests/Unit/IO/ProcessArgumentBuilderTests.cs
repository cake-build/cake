// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core;
using Cake.Core.IO;
using Cake.Core.IO.Arguments;
using Xunit;

namespace Cake.Core.Tests.Unit.IO
{
    public sealed class ProcessArgumentBuilderTests
    {
        public sealed class TheClearMethod
        {
            [Fact]
            public void Should_Remove_All_Arguments()
            {
                // Given
                var builder = new ProcessArgumentBuilder();
                builder.Append(new TextArgument("Hello World"));

                // When
                builder.Clear();

                // Then
                Assert.Empty(builder);
            }
        }

        public sealed class TheInsertMethod
        {
            [Fact]
            public void Should_Insert_Argument_At_Start()
            {
                // Given
                var builder = new ProcessArgumentBuilder();
                builder.Append(new TextArgument("middle"));
                builder.Append(new TextArgument("last"));

                // When
                builder.Insert(0, new TextArgument("first"));

                // Then
                Assert.Equal("first middle last", builder.Render());
            }

            [Fact]
            public void Should_Insert_Argument_In_The_Middle()
            {
                // Given
                var builder = new ProcessArgumentBuilder();
                builder.Append(new TextArgument("first"));
                builder.Append(new TextArgument("last"));

                // When
                builder.Insert(1, new TextArgument("middle"));

                // Then
                Assert.Equal("first middle last", builder.Render());
            }

            [Fact]
            public void Should_Insert_Argument_At_Count()
            {
                // Given
                var builder = new ProcessArgumentBuilder();
                builder.Append(new TextArgument("first"));
                builder.Append(new TextArgument("middle"));

                // When
                builder.Insert(builder.Count, new TextArgument("last"));

                // Then
                Assert.Equal("first middle last", builder.Render());
            }

            [Fact]
            public void Should_Throw_When_Index_Is_Out_Of_Range()
            {
                // Given
                var builder = new ProcessArgumentBuilder();
                builder.Append(new TextArgument("first"));

                // When
                var result = Record.Exception(() => builder.Insert(2, new TextArgument("last")));

                // Then
                Assert.IsType<System.ArgumentOutOfRangeException>(result);
            }
        }

        public sealed class TheInsertRangeMethod
        {
            [Fact]
            public void Should_Insert_Arguments_In_The_Middle()
            {
                // Given
                var builder = new ProcessArgumentBuilder();
                builder.Append(new TextArgument("first"));
                builder.Append(new TextArgument("last"));

                // When
                builder.InsertRange(1, new[]
                {
                    new TextArgument("second"),
                    new TextArgument("third")
                });

                // Then
                Assert.Equal("first second third last", builder.Render());
            }

            [Fact]
            public void Should_Throw_If_Arguments_Is_Null()
            {
                // Given
                var builder = new ProcessArgumentBuilder();

                // When
                var result = Record.Exception(() => builder.InsertRange(0, null));

                // Then
                Assert.IsType<System.ArgumentNullException>(result);
            }

            [Fact]
            public void Should_Throw_When_Index_Is_Out_Of_Range()
            {
                // Given
                var builder = new ProcessArgumentBuilder();
                builder.Append(new TextArgument("first"));

                // When
                var result = Record.Exception(() => builder.InsertRange(2, new[] { new TextArgument("last") }));

                // Then
                Assert.IsType<System.ArgumentOutOfRangeException>(result);
            }
        }

        public sealed class TheFromStringsMethod
        {
            [Fact]
            public void Should_Return_Empty_Builder_When_Values_Is_Null()
            {
                // Given, When
                var builder = ProcessArgumentBuilder.FromStrings(null);

                // Then
                Assert.Empty(builder);
            }
        }

        public sealed class TheFromStringsQuotedMethod
        {
            [Fact]
            public void Should_Return_Empty_Builder_When_Values_Is_Null()
            {
                // Given, When
                var builder = ProcessArgumentBuilder.FromStringsQuoted(null);

                // Then
                Assert.Empty(builder);
            }
        }

        public sealed class TheFilterUnsafeMethod
        {
            [Fact]
            public void Should_Redact_Quoted_Secret()
            {
                // Given
                var builder = new ProcessArgumentBuilder();
                builder.AppendQuotedSecret("password");

                // When
                var result = builder.FilterUnsafe("using password now");

                // Then
                Assert.Equal("using [REDACTED] now", result);
            }

            [Fact]
            public void Should_Redact_Quoted_Secret_Ending_With_Backslash()
            {
                // Given
                var builder = new ProcessArgumentBuilder();
                builder.AppendQuotedSecret("secret\\");

                // When
                var result = builder.FilterUnsafe("using secret\\ now");

                // Then
                Assert.Equal("using [REDACTED] now", result);
            }

            [Fact]
            public void Should_Redact_Quoted_Secret_Containing_Quotes()
            {
                // Given
                var builder = new ProcessArgumentBuilder();
                builder.AppendQuotedSecret("\"quoted\"");

                // When
                var result = builder.FilterUnsafe("using \"quoted\" now");

                // Then
                Assert.Equal("using [REDACTED] now", result);
            }
        }

        public sealed class ImplicitConversion
        {
            public sealed class FromString
            {
                [Theory]
                [InlineData("Hello World", "Hello World")]
                [InlineData("", "")]
                [InlineData(" \t ", " \t ")]
                [InlineData(null, "")]
                public void Should_Return_Builder_With_Correct_Content(string value, string expected)
                {
                    // Given, When
                    var builder = (ProcessArgumentBuilder)value;

                    // Then
                    Assert.Equal(expected, builder.Render());
                }
            }
        }
    }
}