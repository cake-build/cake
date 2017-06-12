// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics.CodeAnalysis;
using Cake.Common.IO.Paths;
using Cake.Core.IO;
using Xunit;

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