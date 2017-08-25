// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Text;
using Cake.Core;
using Cake.Core.IO;
using Cake.Testing;
using NSubstitute;
using Xunit;

namespace Cake.Common.Tests.Unit.Text
{
    public sealed class TextTransformationAliasesTests
    {
        public sealed class TheTransformTextMethod
        {
            public sealed class WithDefaultPlaceholder
            {
                [Fact]
                public void Should_Throw_If_Context_Is_Null()
                {
                    // Given, When
                    var result = Record.Exception(() =>
                        TextTransformationAliases.TransformText(null, "Hello World"));

                    // Then
                    AssertEx.IsArgumentNullException(result, "context");
                }

                [Fact]
                public void Should_Throw_If_Template_Is_Null()
                {
                    // Given
                    var context = Substitute.For<ICakeContext>();

                    // When
                    var result = Record.Exception(() =>
                        TextTransformationAliases.TransformText(context, null));

                    // Then
                    AssertEx.IsArgumentNullException(result, "template");
                }

                [Fact]
                public void Should_Create_Text_Transformation()
                {
                    // Given
                    var context = Substitute.For<ICakeContext>();

                    // When
                    var result = TextTransformationAliases.TransformText(context, "Hello World");

                    // Then
                    Assert.Equal("Hello World", result.ToString());
                }

                [Fact]
                public void Should_Transform_Text_Using_Specified_Placeholders()
                {
                    // Given
                    var context = Substitute.For<ICakeContext>();
                    var transform = TextTransformationAliases.TransformText(context, "Hello <%subject%>");
                    transform.WithToken("subject", "World");

                    // When
                    var result = transform.ToString();

                    // Then
                    Assert.Equal("Hello World", result);
                }
            }

            public sealed class WithCustomPlaceholder
            {
                [Fact]
                public void Should_Throw_If_Context_Is_Null()
                {
                    // Given, When
                    var result = Record.Exception(() =>
                        TextTransformationAliases.TransformText(null, "Hello World", "{", "}"));

                    // Then
                    AssertEx.IsArgumentNullException(result, "context");
                }

                [Fact]
                public void Should_Throw_If_Template_Is_Null()
                {
                    // Given
                    var context = Substitute.For<ICakeContext>();

                    // When
                    var result = Record.Exception(() =>
                        TextTransformationAliases.TransformText(context, null, "{", "}"));

                    // Then
                    AssertEx.IsArgumentNullException(result, "template");
                }

                [Fact]
                public void Should_Throw_If_Left_Placeholder_Is_Null()
                {
                    // Given
                    var context = Substitute.For<ICakeContext>();

                    // When
                    var result = Record.Exception(() =>
                        TextTransformationAliases.TransformText(context, null, null, "}"));

                    // Then
                    AssertEx.IsArgumentNullException(result, "leftPlaceholder");
                }

                [Fact]
                public void Should_Throw_If_Right_Placeholder_Is_Null()
                {
                    // Given
                    var context = Substitute.For<ICakeContext>();

                    // When
                    var result = Record.Exception(() =>
                        TextTransformationAliases.TransformText(context, null, "{", null));

                    // Then
                    AssertEx.IsArgumentNullException(result, "rightPlaceholder");
                }

                [Fact]
                public void Should_Create_Text_Transformation()
                {
                    // Given
                    var context = Substitute.For<ICakeContext>();

                    // When
                    var result = TextTransformationAliases.TransformText(context, "Hello World", "{", "}");

                    // Then
                    Assert.Equal("Hello World", result.ToString());
                }

                [Fact]
                public void Should_Transform_Text_Using_Specified_Placeholders()
                {
                    // Given
                    var context = Substitute.For<ICakeContext>();
                    var transform = TextTransformationAliases.TransformText(context, "Hello {subject}", "{", "}");
                    transform.WithToken("subject", "World");

                    // When
                    var result = transform.ToString();

                    // Then
                    Assert.Equal("Hello World", result);
                }
            }
        }

        public sealed class TheTransformTextFileMethod
        {
            public sealed class WithDefaultPlaceholder
            {
                [Fact]
                public void Should_Throw_If_Context_Is_Null()
                {
                    // Given, When
                    var result = Record.Exception(() =>
                        TextTransformationAliases.TransformTextFile(
                            null, new FilePath("./template.txt")));

                    // Then
                    AssertEx.IsArgumentNullException(result, "context");
                }

                [Fact]
                public void Should_Throw_If_Template_Is_Null()
                {
                    // Given
                    var context = Substitute.For<ICakeContext>();

                    // When
                    var result = Record.Exception(() =>
                        TextTransformationAliases.TransformTextFile(
                            context, null));

                    // Then
                    AssertEx.IsArgumentNullException(result, "path");
                }

                [Fact]
                public void Should_Create_Text_Transformation_From_Disc_Template()
                {
                    // Given
                    var environment = FakeEnvironment.CreateUnixEnvironment();
                    var fileSystem = new FakeFileSystem(environment);
                    fileSystem.CreateFile("/Working/template.txt").SetContent("Hello World");

                    var context = Substitute.For<ICakeContext>();
                    context.FileSystem.Returns(fileSystem);
                    context.Environment.Returns(environment);

                    // When
                    var result = TextTransformationAliases.TransformTextFile(
                        context, "./template.txt");

                    // Then
                    Assert.Equal("Hello World", result.ToString());
                }

                [Fact]
                public void Should_Transform_Text_From_Disc_Template_Using_Default_Placeholders()
                {
                    // Given
                    var environment = FakeEnvironment.CreateUnixEnvironment();
                    var fileSystem = new FakeFileSystem(environment);
                    fileSystem.CreateFile("/Working/template.txt").SetContent("Hello <%subject%>");

                    var context = Substitute.For<ICakeContext>();
                    context.FileSystem.Returns(fileSystem);
                    context.Environment.Returns(environment);

                    var transform = TextTransformationAliases.TransformTextFile(context, "./template.txt");
                    transform.WithToken("subject", "World");

                    // When
                    var result = transform.ToString();

                    // Then
                    Assert.Equal("Hello World", result);
                }
            }

            public sealed class WithCustomPlaceholder
            {
                [Fact]
                public void Should_Throw_If_Context_Is_Null()
                {
                    // Given, When
                    var result = Record.Exception(() =>
                        TextTransformationAliases.TransformTextFile(
                            null, new FilePath("./template.txt"), "{", "}"));

                    // Then
                    AssertEx.IsArgumentNullException(result, "context");
                }

                [Fact]
                public void Should_Throw_If_Template_Is_Null()
                {
                    // Given
                    var context = Substitute.For<ICakeContext>();

                    // When
                    var result = Record.Exception(() =>
                        TextTransformationAliases.TransformTextFile(
                            context, null, "{", "}"));

                    // Then
                    AssertEx.IsArgumentNullException(result, "path");
                }

                [Fact]
                public void Should_Throw_If_Left_Placeholder_Is_Null()
                {
                    // Given
                    var context = Substitute.For<ICakeContext>();

                    // When
                    var result = Record.Exception(() =>
                        TextTransformationAliases.TransformTextFile(
                            context, new FilePath("./template.txt"), null, "}"));

                    // Then
                    AssertEx.IsArgumentNullException(result, "leftPlaceholder");
                }

                [Fact]
                public void Should_Throw_If_Right_Placeholder_Is_Null()
                {
                    // Given
                    var context = Substitute.For<ICakeContext>();

                    // When
                    var result = Record.Exception(() =>
                        TextTransformationAliases.TransformTextFile(
                            context, new FilePath("./template.txt"), "{", null));

                    // Then
                    AssertEx.IsArgumentNullException(result, "rightPlaceholder");
                }

                [Fact]
                public void Should_Create_Text_Transformation_From_Disc_Template()
                {
                    // Given
                    var environment = FakeEnvironment.CreateUnixEnvironment();
                    var fileSystem = new FakeFileSystem(environment);
                    fileSystem.CreateFile("/Working/template.txt").SetContent("Hello World");

                    var context = Substitute.For<ICakeContext>();
                    context.FileSystem.Returns(fileSystem);
                    context.Environment.Returns(environment);

                    // When
                    var result = TextTransformationAliases.TransformTextFile(
                        context, "./template.txt", "{", "}");

                    // Then
                    Assert.Equal("Hello World", result.ToString());
                }

                [Fact]
                public void Should_Transform_Text_From_Disc_Template_Using_Specified_Placeholders()
                {
                    // Given
                    var environment = FakeEnvironment.CreateUnixEnvironment();
                    var fileSystem = new FakeFileSystem(environment);
                    fileSystem.CreateFile("/Working/template.txt").SetContent("Hello {subject}");

                    var context = Substitute.For<ICakeContext>();
                    context.FileSystem.Returns(fileSystem);
                    context.Environment.Returns(environment);

                    var transform = TextTransformationAliases.TransformTextFile(context, "./template.txt", "{", "}");
                    transform.WithToken("subject", "World");

                    // When
                    var result = transform.ToString();

                    // Then
                    Assert.Equal("Hello World", result);
                }
            }
        }
    }
}