// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics.CodeAnalysis;
using Cake.Common.IO.Paths;
using Cake.Core.IO;
using Xunit;

namespace Cake.Common.Tests.Unit.IO.Paths
{
    public sealed class ConvertableDirectoryPathTests
    {
        public sealed class TheConstructor
        {
            [Fact]
            public void Should_Throw_If_Path_Is_Null()
            {
                // Given, When
                var result = Record.Exception(() => new ConvertableDirectoryPath(null));

                // Then
                AssertEx.IsArgumentNullException(result, "path");
            }
        }

        public sealed class ThePathProperty
        {
            [Fact]
            public void Should_Return_Path_Provided_To_Constructor()
            {
                // Given
                var path = new DirectoryPath("./root");

                // When
                var result = new ConvertableDirectoryPath(path).Path;

                // Then
                Assert.Same(path, result);
            }
        }

        public sealed class TheAddOperator
        {
            public sealed class AddingConvertableDirectoryPath
            {
                [Fact]
                public void Should_Combine_The_Two_Paths()
                {
                    // Given
                    var path = new ConvertableDirectoryPath("./root");

                    // When
                    var result = path + new ConvertableDirectoryPath("other");

                    // Then
                    Assert.Equal("root/other", result.Path.FullPath);
                }

                [Fact]
                public void Should_Return_A_New_Convertable_Directory_Path()
                {
                    // Given
                    var path = new ConvertableDirectoryPath("./root");

                    // When
                    var result = path + new ConvertableDirectoryPath("other");

                    // Then
                    Assert.IsType<ConvertableDirectoryPath>(result);
                }

                [Fact]
                public void Should_Throw_If_Left_Operand_Is_Null()
                {
                    // Given, When
                    var result = Record.Exception(() =>
                        (ConvertableDirectoryPath)null + new ConvertableDirectoryPath("other"));

                    // Then
                    AssertEx.IsArgumentNullException(result, "left");
                }

                [Fact]
                public void Should_Throw_If_Right_Operand_Is_Null()
                {
                    // Given, When
                    var result = Record.Exception(() =>
                        new ConvertableDirectoryPath("./root") + (ConvertableDirectoryPath)null);

                    // Then
                    AssertEx.IsArgumentNullException(result, "right");
                }
            }

            public sealed class AddingDirectoryPath
            {
                [Fact]
                public void Should_Combine_The_Two_Paths()
                {
                    // Given
                    var path = new ConvertableDirectoryPath("./root");

                    // When
                    var result = path + new DirectoryPath("other");

                    // Then
                    Assert.Equal("root/other", result.Path.FullPath);
                }

                [Fact]
                public void Should_Return_A_New_Convertable_Directory_Path()
                {
                    // Given
                    var path = new ConvertableDirectoryPath("./root");

                    // When
                    var result = path + new DirectoryPath("other");

                    // Then
                    Assert.IsType<ConvertableDirectoryPath>(result);
                }

                [Fact]
                public void Should_Throw_If_Left_Operand_Is_Null()
                {
                    // Given, When
                    var result = Record.Exception(() =>
                        (ConvertableDirectoryPath)null + new DirectoryPath("other"));

                    // Then
                    AssertEx.IsArgumentNullException(result, "left");
                }

                [Fact]
                public void Should_Throw_If_Right_Operand_Is_Null()
                {
                    // Given, When
                    var result = Record.Exception(() =>
                        new ConvertableDirectoryPath("./root") + (DirectoryPath)null);

                    // Then
                    AssertEx.IsArgumentNullException(result, "right");
                }
            }

            public sealed class AddingConvertableFilePath
            {
                [Fact]
                public void Should_Combine_The_Two_Paths()
                {
                    // Given
                    var path = new ConvertableDirectoryPath("./root");

                    // When
                    var result = path + new ConvertableFilePath("other.txt");

                    // Then
                    Assert.Equal("root/other.txt", result.Path.FullPath);
                }

                [Fact]
                public void Should_Return_A_New_Convertable_File_Path()
                {
                    // Given
                    var path = new ConvertableDirectoryPath("./root");

                    // When
                    var result = path + new ConvertableFilePath("other.txt");

                    // Then
                    Assert.IsType<ConvertableFilePath>(result);
                }

                [Fact]
                public void Should_Throw_If_Left_Operand_Is_Null()
                {
                    // Given, When
                    var result = Record.Exception(() =>
                        (ConvertableDirectoryPath)null + new ConvertableFilePath("other.txt"));

                    // Then
                    AssertEx.IsArgumentNullException(result, "directory");
                }

                [Fact]
                public void Should_Throw_If_Right_Operand_Is_Null()
                {
                    // Given, When
                    var result = Record.Exception(() =>
                        new ConvertableDirectoryPath("./root") + (ConvertableFilePath)null);

                    // Then
                    AssertEx.IsArgumentNullException(result, "file");
                }
            }

            public sealed class AddingFilePath
            {
                [Fact]
                public void Should_Combine_The_Two_Paths()
                {
                    // Given
                    var path = new ConvertableDirectoryPath("./root");

                    // When
                    var result = path + new FilePath("other.txt");

                    // Then
                    Assert.Equal("root/other.txt", result.Path.FullPath);
                }

                [Fact]
                public void Should_Return_A_New_Convertable_File_Path()
                {
                    // Given
                    var path = new ConvertableDirectoryPath("./root");

                    // When
                    var result = path + new FilePath("other.txt");

                    // Then
                    Assert.IsType<ConvertableFilePath>(result);
                }

                [Fact]
                public void Should_Throw_If_Left_Operand_Is_Null()
                {
                    // Given, When
                    var result = Record.Exception(() =>
                        (ConvertableDirectoryPath)null + new FilePath("other.txt"));

                    // Then
                    AssertEx.IsArgumentNullException(result, "directory");
                }

                [Fact]
                public void Should_Throw_If_Right_Operand_Is_Null()
                {
                    // Given, When
                    var result = Record.Exception(() =>
                        new ConvertableDirectoryPath("./root") + (FilePath)null);

                    // Then
                    AssertEx.IsArgumentNullException(result, "file");
                }
            }
        }

        public sealed class TheImplicitConversionOperator
        {
            public sealed class ConvertToDirectoryPath
            {
                [Fact]
                public void Should_Return_A_Representation_Of_The_Current_Instance()
                {
                    // Given
                    var path = new ConvertableDirectoryPath("./root");

                    // When
                    var result = (DirectoryPath)path;

                    // Then
                    Assert.Equal("root", result.FullPath);
                }

                [Fact]
                [SuppressMessage("ReSharper", "ExpressionIsAlwaysNull")]
                public void Should_Return_Null_If_Convertable_Directory_Path_Is_Null()
                {
                    // Given
                    var path = (ConvertableDirectoryPath)null;

                    // When
                    var result = (DirectoryPath)path;

                    // Then
                    Assert.Null(result);
                }
            }

            public sealed class ConvertToString
            {
                [Fact]
                public void Should_Return_A_Representation_Of_The_Current_Instance()
                {
                    // Given
                    var path = new ConvertableDirectoryPath("./root");

                    // When
                    var result = (string)path;

                    // Then
                    Assert.Equal("root", result);
                }

                [Fact]
                [SuppressMessage("ReSharper", "ExpressionIsAlwaysNull")]
                public void Should_Return_Null_If_Convertable_Directory_Path_Is_Null()
                {
                    // Given
                    var path = (ConvertableDirectoryPath)null;

                    // When
                    var result = (string)path;

                    // Then
                    Assert.Null(result);
                }
            }
        }

        public sealed class TheToStringMethod
        {
            [Fact]
            public void Should_Return_String_Representation_Of_Path()
            {
                // Given
                var path = new ConvertableDirectoryPath("./foo/bar");

                // When
                var result = path.ToString();

                // Then
                Assert.Equal("foo/bar", result);
            }
        }
    }
}