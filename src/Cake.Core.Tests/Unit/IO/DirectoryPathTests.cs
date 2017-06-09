// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Core.IO;
using Cake.Testing.Xunit;
using NSubstitute;
using Xunit;

namespace Cake.Core.Tests.Unit.IO
{
    public sealed class DirectoryPathTests
    {
        public sealed class TheGetDirectoryNameMethod
        {
            [Theory]
            [InlineData("C:/Data", "Data")]
            [InlineData("C:/Data/Work", "Work")]
            [InlineData("C:/Data/Work/file.txt", "file.txt")]
            public void Should_Return_Directory_Name(string directoryPath, string name)
            {
                // Given
                var path = new DirectoryPath(directoryPath);

                // When
                var result = path.GetDirectoryName();

                // Then
                Assert.Equal(name, result);
            }
        }

        public sealed class TheGetFilePathMethod
        {
            [Fact]
            public void Should_Throw_If_Path_Is_Null()
            {
                // Given
                var path = new DirectoryPath("assets");

                // When
                var result = Record.Exception(() => path.GetFilePath(null));

                // Then
                AssertEx.IsArgumentNullException(result, "path");
            }

            [Theory]
#if !UNIX
            [InlineData("c:/assets/shaders/", "simple.frag", "c:/assets/shaders/simple.frag")]
            [InlineData("c:/", "simple.frag", "c:/simple.frag")]
            [InlineData("c:/assets/shaders/", "test/simple.frag", "c:/assets/shaders/simple.frag")]
            [InlineData("c:/", "test/simple.frag", "c:/simple.frag")]
#endif
            [InlineData("assets/shaders", "simple.frag", "assets/shaders/simple.frag")]
            [InlineData("assets/shaders/", "simple.frag", "assets/shaders/simple.frag")]
            [InlineData("/assets/shaders/", "simple.frag", "/assets/shaders/simple.frag")]
            [InlineData("assets/shaders", "test/simple.frag", "assets/shaders/simple.frag")]
            [InlineData("assets/shaders/", "test/simple.frag", "assets/shaders/simple.frag")]
            [InlineData("/assets/shaders/", "test/simple.frag", "/assets/shaders/simple.frag")]
            public void Should_Combine_Paths(string first, string second, string expected)
            {
                // Given
                var path = new DirectoryPath(first);

                // When
                var result = path.GetFilePath(new FilePath(second));

                // Then
                Assert.Equal(expected, result.FullPath);
            }
        }

        public sealed class TheCombineWithFilePathMethod
        {
            [Fact]
            public void Should_Throw_If_Path_Is_Null()
            {
                // Given
                var path = new DirectoryPath("assets");

                // When
                var result = Record.Exception(() => path.CombineWithFilePath(null));

                // Then
                AssertEx.IsArgumentNullException(result, "path");
            }

            [Theory]
#if !UNIX
            [InlineData("c:/assets/shaders/", "simple.frag", "c:/assets/shaders/simple.frag")]
            [InlineData("c:/", "simple.frag", "c:/simple.frag")]
            [InlineData("c:/assets/shaders/", "test/simple.frag", "c:/assets/shaders/test/simple.frag")]
            [InlineData("c:/", "test/simple.frag", "c:/test/simple.frag")]
#endif
            [InlineData("assets/shaders", "simple.frag", "assets/shaders/simple.frag")]
            [InlineData("assets/shaders/", "simple.frag", "assets/shaders/simple.frag")]
            [InlineData("/assets/shaders/", "simple.frag", "/assets/shaders/simple.frag")]
            [InlineData("assets/shaders", "test/simple.frag", "assets/shaders/test/simple.frag")]
            [InlineData("assets/shaders/", "test/simple.frag", "assets/shaders/test/simple.frag")]
            [InlineData("/assets/shaders/", "test/simple.frag", "/assets/shaders/test/simple.frag")]
            public void Should_Combine_Paths(string first, string second, string expected)
            {
                // Given
                var path = new DirectoryPath(first);

                // When
                var result = path.CombineWithFilePath(new FilePath(second));

                // Then
                Assert.Equal(expected, result.FullPath);
            }

            [Fact]
            public void Can_Not_Combine_Directory_Path_With_Absolute_File_Path()
            {
                // Given
                var path = new DirectoryPath("assets");

                // When
                var result = Record.Exception(() => path.CombineWithFilePath(new FilePath("/other/asset.txt")));

                // Then
                Assert.IsType<InvalidOperationException>(result);
                Assert.Equal("Cannot combine a directory path with an absolute file path.", result?.Message);
            }
        }

        public sealed class TheCombineWithDirectoryPathMethod
        {
            [Theory]
#if !UNIX
            [InlineData("c:/assets/shaders/", "simple", "c:/assets/shaders/simple")]
            [InlineData("c:/", "simple", "c:/simple")]
#endif
            [InlineData("assets/shaders", "simple", "assets/shaders/simple")]
            [InlineData("assets/shaders/", "simple", "assets/shaders/simple")]
            [InlineData("/assets/shaders/", "simple", "/assets/shaders/simple")]
            public void Should_Combine_Paths(string first, string second, string expected)
            {
                // Given
                var path = new DirectoryPath(first);

                // When
                var result = path.Combine(new DirectoryPath(second));

                // Then
                Assert.Equal(expected, result.FullPath);
            }

            [Fact]
            public void Should_Throw_If_Path_Is_Null()
            {
                // Given
                var path = new DirectoryPath("assets");

                // When
                var result = Record.Exception(() => path.Combine(null));

                // Then
                AssertEx.IsArgumentNullException(result, "path");
            }

            [Fact]
            public void Can_Not_Combine_Directory_Path_With_Absolute_Directory_Path()
            {
                // Given
                var path = new DirectoryPath("assets");

                // When
                var result = Record.Exception(() => path.Combine(new DirectoryPath("/other/assets")));

                // Then
                Assert.IsType<InvalidOperationException>(result);
                Assert.Equal("Cannot combine a directory path with an absolute directory path.", result?.Message);
            }
        }

        public sealed class TheMakeAbsoluteMethod
        {
            public sealed class ThatTakesAnEnvironment
            {
                [Fact]
                public void Should_Throw_If_Provided_Environment_Is_Null()
                {
                    // Given
                    var path = new DirectoryPath("assets");

                    // When
                    var result = Record.Exception(
                        () => path.MakeAbsolute((ICakeEnvironment)null));

                    // Then
                    AssertEx.IsArgumentNullException(result, "environment");
                }

                [Fact]
                public void Should_Create_New_Absolute_Path_When_Path_Is_Relative()
                {
                    // Given
                    var environment = Substitute.For<ICakeEnvironment>();
                    environment.WorkingDirectory.Returns("/Working");
                    var path = new DirectoryPath("assets");

                    // When
                    var result = path.MakeAbsolute(environment);

                    // Then
                    Assert.Equal("/Working/assets", result.FullPath);
                }

                [Fact]
                public void Should_Create_New_Absolute_Path_Identical_To_The_Path()
                {
                    // Given
                    var environment = Substitute.For<ICakeEnvironment>();
                    var path = new DirectoryPath("/assets");

                    // When
                    var result = path.MakeAbsolute(environment);

                    // Then
                    Assert.Equal("/assets", result.FullPath);
                }
            }

            public sealed class ThatTakesAnotherDirectoryPath
            {
                [Fact]
                public void Should_Throw_If_Provided_Path_Is_Null()
                {
                    // Given
                    var path = new DirectoryPath("assets");

                    // When
                    var result = Record.Exception(
                        () => path.MakeAbsolute((DirectoryPath)null));

                    // Then
                    AssertEx.IsArgumentNullException(result, "path");
                }

                [Fact]
                public void Should_Throw_If_Provided_Path_Is_Relative()
                {
                    // Given
                    var path = new DirectoryPath("assets");

                    // When
                    var result = Record.Exception(() => path.MakeAbsolute("Working"));

                    // Then
                    Assert.IsType<CakeException>(result);
                    Assert.Equal("The provided path cannot be relative.", result?.Message);
                }

                [Fact]
                public void Should_Create_New_Absolute_Path_When_Path_Is_Relative()
                {
                    // Given
                    var path = new DirectoryPath("assets");

                    // When
                    var result = path.MakeAbsolute("/absolute");

                    // Then
                    Assert.Equal("/absolute/assets", result.FullPath);
                }

                [Fact]
                public void Should_Create_New_Absolute_Path_Identical_To_The_Path()
                {
                    // Given
                    var path = new DirectoryPath("/assets");

                    // When
                    var result = path.MakeAbsolute("/absolute");

                    // Then
                    Assert.Equal("/assets", result.FullPath);
                }
            }
        }

        public sealed class TheGetRelativePathMethod
        {
            public sealed class WithDirectoryPath
            {
                public sealed class InWindowsFormat
                {
                    [WindowsTheory]
                    [InlineData("C:/A/B/C", "C:/A/B/C", ".")]
                    [InlineData("C:/", "C:/", ".")]
                    [InlineData("C:/A/B/C", "C:/A/D/E", "../../D/E")]
                    [InlineData("C:/A/B/C", "C:/", "../../..")]
                    [InlineData("C:/A/B/C/D/E/F", "C:/A/B/C", "../../..")]
                    [InlineData("C:/A/B/C", "C:/A/B/C/D/E/F", "D/E/F")]
                    public void Should_Returns_Relative_Path_Between_Paths(string from, string to, string expected)
                    {
                        // Given
                        var path = new DirectoryPath(from);

                        // When
                        var relativePath = path.GetRelativePath(new DirectoryPath(to));

                        // Then
                        Assert.Equal(expected, relativePath.FullPath);
                    }

                    [WindowsTheory]
                    [InlineData("C:/A/B/C", "D:/A/B/C")]
                    [InlineData("C:/A/B", "D:/E/")]
                    [InlineData("C:/", "B:/")]
                    public void Should_Throw_If_No_Relative_Path_Can_Be_Found(string from, string to)
                    {
                        // Given
                        var path = new DirectoryPath(from);

                        // When
                        var result = Record.Exception(() => path.GetRelativePath(new DirectoryPath(to)));

                        // Then
                        Assert.IsType<InvalidOperationException>(result);
                        Assert.Equal("Paths must share a common prefix.", result?.Message);
                    }

                    [WindowsFact]
                    public void Should_Throw_If_Target_DirectoryPath_Is_Null()
                    {
                        // Given
                        var path = new DirectoryPath("C:/A/B/C");

                        // When
                        var result = Record.Exception(() => path.GetRelativePath((DirectoryPath)null));

                        // Then
                        AssertEx.IsArgumentNullException(result, "to");
                    }

                    [WindowsFact]
                    public void Should_Throw_If_Source_DirectoryPath_Is_Relative()
                    {
                        // Given
                        var path = new DirectoryPath("A");

                        // When
                        var result = Record.Exception(() => path.GetRelativePath(new DirectoryPath("C:/D/E/F")));

                        // Then
                        Assert.IsType<InvalidOperationException>(result);
                        Assert.Equal("Source path must be an absolute path.", result?.Message);
                    }

                    [WindowsFact]
                    public void Should_Throw_If_Target_DirectoryPath_Is_Relative()
                    {
                        // Given
                        var path = new DirectoryPath("C:/A/B/C");

                        // When
                        var result = Record.Exception(() => path.GetRelativePath(new DirectoryPath("D")));

                        // Then
                        Assert.IsType<InvalidOperationException>(result);
                        Assert.Equal("Target path must be an absolute path.", result?.Message);
                    }
                }

                public sealed class InUnixFormat
                {
                    [Theory]
                    [InlineData("/C/A/B/C", "/C/A/B/C", ".")]
                    [InlineData("/C/", "/C/", ".")]
                    [InlineData("/C/A/B/C", "/C/A/D/E", "../../D/E")]
                    [InlineData("/C/A/B/C", "/C/", "../../..")]
                    [InlineData("/C/A/B/C/D/E/F", "/C/A/B/C", "../../..")]
                    [InlineData("/C/A/B/C", "/C/A/B/C/D/E/F", "D/E/F")]
                    public void Should_Returns_Relative_Path_Between_Paths(string from, string to, string expected)
                    {
                        // Given
                        var path = new DirectoryPath(from);

                        // When
                        var relativePath = path.GetRelativePath(new DirectoryPath(to));

                        // Then
                        Assert.Equal(expected, relativePath.FullPath);
                    }

                    [Theory]
                    [InlineData("/C/A/B/C", "/D/A/B/C")]
                    [InlineData("/C/A/B", "/D/E/")]
                    [InlineData("/C/", "/B/")]
                    public void Should_Throw_If_No_Relative_Path_Can_Be_Found(string from, string to)
                    {
                        // Given
                        var path = new DirectoryPath(from);

                        // When
                        var result = Record.Exception(() => path.GetRelativePath(new DirectoryPath(to)));

                        // Then
                        Assert.IsType<InvalidOperationException>(result);
                        Assert.Equal("Paths must share a common prefix.", result?.Message);
                    }

                    [Fact]
                    public void Should_Throw_If_Target_DirectoryPath_Is_Null()
                    {
                        // Given
                        var path = new DirectoryPath("/C/A/B/C");

                        // When
                        var result = Record.Exception(() => path.GetRelativePath((DirectoryPath)null));

                        // Then
                        AssertEx.IsArgumentNullException(result, "to");
                    }

                    [Fact]
                    public void Should_Throw_If_Source_DirectoryPath_Is_Relative()
                    {
                        // Given
                        var path = new DirectoryPath("A");

                        // When
                        var result = Record.Exception(() => path.GetRelativePath(new DirectoryPath("/C/D/E/F")));

                        // Then
                        Assert.IsType<InvalidOperationException>(result);
                        Assert.Equal("Source path must be an absolute path.", result?.Message);
                    }

                    [Fact]
                    public void Should_Throw_If_Target_DirectoryPath_Is_Relative()
                    {
                        // Given
                        var path = new DirectoryPath("/C/A/B/C");

                        // When
                        var result = Record.Exception(() => path.GetRelativePath(new DirectoryPath("D")));

                        // Then
                        Assert.IsType<InvalidOperationException>(result);
                        Assert.Equal("Target path must be an absolute path.", result?.Message);
                    }
                }
            }

            public sealed class WithFilePath
            {
                public sealed class InWindowsFormat
                {
                    [WindowsTheory]
                    [InlineData("C:/A/B/C", "C:/A/B/C/hello.txt", "hello.txt")]
                    [InlineData("C:/", "C:/hello.txt", "hello.txt")]
                    [InlineData("C:/A/B/C", "C:/A/D/E/hello.txt", "../../D/E/hello.txt")]
                    [InlineData("C:/A/B/C", "C:/hello.txt", "../../../hello.txt")]
                    [InlineData("C:/A/B/C/D/E/F", "C:/A/B/C/hello.txt", "../../../hello.txt")]
                    [InlineData("C:/A/B/C", "C:/A/B/C/D/E/F/hello.txt", "D/E/F/hello.txt")]
                    public void Should_Returns_Relative_Path_Between_Paths(string from, string to, string expected)
                    {
                        // Given
                        var path = new DirectoryPath(from);

                        // When
                        var relativePath = path.GetRelativePath(new FilePath(to));

                        // Then
                        Assert.Equal(expected, relativePath.FullPath);
                    }

                    [WindowsTheory]
                    [InlineData("C:/A/B/C", "D:/A/B/C/hello.txt")]
                    [InlineData("C:/A/B", "D:/E/hello.txt")]
                    [InlineData("C:/", "B:/hello.txt")]
                    public void Should_Throw_If_No_Relative_Path_Can_Be_Found(string from, string to)
                    {
                        // Given
                        var path = new DirectoryPath(from);

                        // When
                        var result = Record.Exception(() => path.GetRelativePath(new FilePath(to)));

                        // Then
                        Assert.IsType<InvalidOperationException>(result);
                        Assert.Equal("Paths must share a common prefix.", result?.Message);
                    }

                    [WindowsFact]
                    public void Should_Throw_If_Target_FilePath_Is_Null()
                    {
                        // Given
                        var path = new DirectoryPath("C:/A/B/C");

                        // When
                        var result = Record.Exception(() => path.GetRelativePath((FilePath)null));

                        // Then
                        AssertEx.IsArgumentNullException(result, "to");
                    }

                    [WindowsFact]
                    public void Should_Throw_If_Source_DirectoryPath_Is_Relative()
                    {
                        // Given
                        var path = new DirectoryPath("A");

                        // When
                        var result = Record.Exception(() => path.GetRelativePath(new FilePath("C:/D/E/F/hello.txt")));

                        // Then
                        Assert.IsType<InvalidOperationException>(result);
                        Assert.Equal("Source path must be an absolute path.", result?.Message);
                    }

                    [WindowsFact]
                    public void Should_Throw_If_Target_FilePath_Is_Relative()
                    {
                        // Given
                        var path = new DirectoryPath("C:/A/B/C");

                        // When
                        var result = Record.Exception(() => path.GetRelativePath(new FilePath("D/hello.txt")));

                        // Then
                        Assert.IsType<InvalidOperationException>(result);
                        Assert.Equal("Target path must be an absolute path.", result?.Message);
                    }
                }

                public sealed class InUnixFormat
                {
                    [Theory]
                    [InlineData("/C/A/B/C", "/C/A/B/C/hello.txt", "hello.txt")]
                    [InlineData("/C/", "/C/hello.txt", "hello.txt")]
                    [InlineData("/C/A/B/C", "/C/A/D/E/hello.txt", "../../D/E/hello.txt")]
                    [InlineData("/C/A/B/C", "/C/hello.txt", "../../../hello.txt")]
                    [InlineData("/C/A/B/C/D/E/F", "/C/A/B/C/hello.txt", "../../../hello.txt")]
                    [InlineData("/C/A/B/C", "/C/A/B/C/D/E/F/hello.txt", "D/E/F/hello.txt")]
                    public void Should_Returns_Relative_Path_Between_Paths(string from, string to, string expected)
                    {
                        // Given
                        var path = new DirectoryPath(from);

                        // When
                        var relativePath = path.GetRelativePath(new FilePath(to));

                        // Then
                        Assert.Equal(expected, relativePath.FullPath);
                    }

                    [Theory]
                    [InlineData("/C/A/B/C", "/D/A/B/C/hello.txt")]
                    [InlineData("/C/A/B", "/D/E/hello.txt")]
                    [InlineData("/C/", "/B/hello.txt")]
                    public void Should_Throw_If_No_Relative_Path_Can_Be_Found(string from, string to)
                    {
                        // Given
                        var path = new DirectoryPath(from);

                        // When
                        var result = Record.Exception(() => path.GetRelativePath(new FilePath(to)));

                        // Then
                        Assert.IsType<InvalidOperationException>(result);
                        Assert.Equal("Paths must share a common prefix.", result?.Message);
                    }

                    [Fact]
                    public void Should_Throw_If_Target_FilePath_Is_Null()
                    {
                        // Given
                        var path = new DirectoryPath("/C/A/B/C");

                        // When
                        var result = Record.Exception(() => path.GetRelativePath((FilePath)null));

                        // Then
                        AssertEx.IsArgumentNullException(result, "to");
                    }

                    [Fact]
                    public void Should_Throw_If_Source_DirectoryPath_Is_Relative()
                    {
                        // Given
                        var path = new DirectoryPath("A");

                        // When
                        var result = Record.Exception(() => path.GetRelativePath(new FilePath("/C/D/E/F/hello.txt")));

                        // Then
                        Assert.IsType<InvalidOperationException>(result);
                        Assert.Equal("Source path must be an absolute path.", result?.Message);
                    }

                    [Fact]
                    public void Should_Throw_If_Target_FilePath_Is_Relative()
                    {
                        // Given
                        var path = new DirectoryPath("/C/A/B/C");

                        // When
                        var result = Record.Exception(() => path.GetRelativePath(new FilePath("D/hello.txt")));

                        // Then
                        Assert.IsType<InvalidOperationException>(result);
                        Assert.Equal("Target path must be an absolute path.", result?.Message);
                    }
                }
            }
        }
    }
}