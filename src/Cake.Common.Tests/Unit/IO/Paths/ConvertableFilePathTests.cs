// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Cake.Common.IO.Paths;
using Cake.Core.IO;
using NSubstitute;
using Xunit;
using Xunit.v3;

namespace Cake.Common.Tests.Unit.IO.Paths
{
    public sealed class ConvertableFilePathTests
    {
        public sealed class TheConstructor
        {
            [Fact]
            public void Should_Throw_If_Path_Is_Null()
            {
                // Given, When
                var result = Record.Exception(() => new ConvertableFilePath(null));

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
                var path = new FilePath("file.txt");

                // When
                var result = new ConvertableFilePath(path).Path;

                // Then
                Assert.Same(path, result);
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
                    var path = new ConvertableFilePath("file.txt");

                    // When
                    var result = (FilePath)path;

                    // Then
                    Assert.Equal("file.txt", result.FullPath);
                }

                [Fact]
                [SuppressMessage("ReSharper", "ExpressionIsAlwaysNull")]
                public void Should_Return_Null_If_Convertable_Directory_Path_Is_Null()
                {
                    // Given
                    var path = (ConvertableFilePath)null;

                    // When
                    var result = (FilePath)path;

                    // Then
                    Assert.Null(result);
                }

                [Fact]
                [SuppressMessage("ReSharper", "ExpressionIsAlwaysNull")]
                public void Should_Return_Null_Exception_If_Convertable_Directory_Path_Is_Null()
                {
                    // Given
                    DirectoryPath dirPath = null;

                    // When
                    var filePath = new ConvertableFilePath("file.txt");


                    // Then
                    var ex = Assert.Throws<ArgumentNullException>(() => (dirPath + filePath).ToString());

                }

                [Fact]
                [SuppressMessage("ReSharper", "ExpressionIsAlwaysNull")]
                public void Should_Return_Null_Exception_If_Convertable_FilePath_Is_Null()
                {
                    // Given
                    DirectoryPath dirPath = new DirectoryPath("X");

                    // When
                    ConvertableFilePath filePath = null;


                    // Then
                    var ex = Assert.Throws<ArgumentNullException>(() => (dirPath + filePath).ToString());

                }

                [Fact]
                [SuppressMessage("ReSharper", "ExpressionIsAlwaysNull")]
                public void Should_Return_Combined_Directorypath_FilePath_Including_Separator()
                {
                    // Given
                    DirectoryPath dirPath = new DirectoryPath("X");
                    var filePath = new ConvertableFilePath("file.txt");

                    // When
                    var result = (dirPath + filePath).ToString();

                    // Then
                    Assert.Equal("X/file.txt", result);

                }

                [Fact]
                [SuppressMessage("ReSharper", "ExpressionIsAlwaysNull")]
                public void Should_Return_Combined_RootPath_Directorypath_FilePath_Including_Separator()
                {
                    // Given
                    var contextcake = Substitute.For<Core.ICakeContext>();

                    DirectoryPath dirPath = new ConvertableDirectoryPath(new DirectoryPath("X"));
                    var filePath = new ConvertableFilePath(new FilePath("file.txt"));
                    var rootdir = new ConvertableDirectoryPath(new DirectoryPath(".."));

                    // When
                    var result = (rootdir + dirPath + filePath).ToString();

                    // Then
                    Assert.Equal("../X/file.txt", result);

                }
            }

            public sealed class ConvertToString
            {
                [Fact]
                public void Should_Return_A_Representation_Of_The_Current_Instance()
                {
                    // Given
                    var path = new ConvertableFilePath("file.txt");

                    // When
                    var result = (string)path;

                    // Then
                    Assert.Equal("file.txt", result);
                }

                [Fact]
                [SuppressMessage("ReSharper", "ExpressionIsAlwaysNull")]
                public void Should_Return_Null_If_Convertable_Directory_Path_Is_Null()
                {
                    // Given
                    var path = (ConvertableFilePath)null;

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
                var path = new ConvertableFilePath("./foo/bar.baz");

                // When
                var result = path.ToString();

                // Then
                Assert.Equal("foo/bar.baz", result);
            }
        }
    }
}