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
    public sealed class FilePathTests
    {
        public sealed class TheConversionFromStringOperator
        {
            [Fact]
            public void Null_String_Converts_To_Null_FilePath()
            {
                // Given
                const string nullString = null;

                // When
                var path = (FilePath)nullString;

                // Then
                Assert.Null(path);
            }
        }

        public sealed class TheHasExtensionProperty
        {
            [Theory]
            [InlineData("assets/shaders/basic.txt", true)]
            [InlineData("assets/shaders/basic", false)]
            [InlineData("assets/shad.ers/basic", false)]
            [InlineData("assets/shaders/basic/", false)]
            [InlineData("assets/shad.ers/basic/", false)]
            public void Can_See_If_A_Path_Has_An_Extension(string fullPath, bool expected)
            {
                // Given, When
                var path = new FilePath(fullPath);

                // Then
                Assert.Equal(expected, path.HasExtension);
            }

            [WindowsTheory]
            [InlineData("C:/foo/bar/baz.txt", true)]
            [InlineData("C:/foo/bar/baz", false)]
            [InlineData("C:/foo/bar.baz/qux", false)]
            [InlineData("C:/foo/bar/baz/", false)]
            [InlineData(@"\\foo\bar\baz.txt", true)]
            [InlineData(@"\\foo\bar\baz", false)]
            [InlineData(@"\\foo\bar.baz\qux", false)]
            [InlineData(@"\\foo\bar\baz\", false)]
            public void Can_See_If_A_Windows_Path_Has_An_Extension(string fullPath, bool expected)
            {
                // Given, When
                var path = new FilePath(fullPath);

                // Then
                Assert.Equal(expected, path.HasExtension);
            }
        }

        public sealed class TheGetExtensionMethod
        {
            [Theory]
            [InlineData("assets/shaders/basic.frag", ".frag")]
            [InlineData("assets/shaders/basic.frag/test.vert", ".vert")]
            [InlineData("assets/shaders/basic.frag/test.foo.vert", ".vert")]
            [InlineData("assets/shaders/basic", null)]
            [InlineData("assets/shaders/basic.frag/test", null)]
            public void Can_Get_Extension(string fullPath, string expected)
            {
                // Given
                var path = new FilePath(fullPath);

                // When
                var result = path.GetExtension();

                // Then
                Assert.Equal(expected, result);
            }

            [WindowsTheory]
            [InlineData("C:/foo/bar/baz.txt", ".txt")]
            [InlineData("C:/foo/bar/baz.txt/qux.md", ".md")]
            [InlineData("C:/foo/bar/baz.txt/qux.md.rs", ".rs")]
            [InlineData("C:/foo/bar/baz", null)]
            [InlineData("C:/foo/bar/baz.txt/qux", null)]
            [InlineData(@"\\foo\bar\baz.txt", ".txt")]
            [InlineData(@"\\foo\bar\baz.txt\qux.md", ".md")]
            [InlineData(@"\\foo\bar\baz.txt\qux.md.rs", ".rs")]
            [InlineData(@"\\foo\bar\baz", null)]
            [InlineData(@"\\foo\bar\baz.txt\qux", null)]
            public void Can_Get_Windows_Extension(string fullPath, string expected)
            {
                // Given, When
                var path = new FilePath(fullPath);

                // When
                var result = path.GetExtension();

                // Then
                Assert.Equal(expected, result);
            }
        }

        public sealed class TheGetDirectoryMethod
        {
            [Theory]
            [InlineData("temp/hello.txt", "temp")]
            public void Can_Get_Directory_For_File_Path(string input, string expected)
            {
                // Given
                var path = new FilePath(input);

                // When
                var result = path.GetDirectory();

                // Then
                Assert.Equal(expected, result.FullPath);
            }

            [WindowsTheory]
            [InlineData("C:/temp/hello.txt", "C:/temp")]
            [InlineData(@"\\temp\hello.txt", @"\\temp")]
            public void Can_Get_Directory_For_Windows_File_Path(string fullPath, string expected)
            {
                // Given
                var path = new FilePath(fullPath);

                // When
                var result = path.GetDirectory();

                // Then
                Assert.Equal(expected, result.FullPath);
            }

            [Fact]
            public void Can_Get_Directory_For_Relative_File_Path_In_Root()
            {
                // Given
                var path = new FilePath("hello.txt");

                // When
                var result = path.GetDirectory();

                // Then
                Assert.Equal(string.Empty, result.FullPath);
            }

            [Fact]
            public void Can_Get_Directory_For_Absolute_File_Path_In_Root()
            {
                // Given
                var path = new FilePath("/hello.txt");

                // When
                var result = path.GetDirectory();

                // Then
                Assert.Equal("/", result.FullPath);
            }

            [WindowsTheory]
            [InlineData("C:/hello.txt", "C:/")]
            [InlineData(@"\\hello.txt", @"\\")]
            public void Can_Get_Directory_For_Absolute_File_Path_In_Windows_Root(string fullPath, string expected)
            {
                // Given
                var path = new FilePath(fullPath);

                // When
                var result = path.GetDirectory();

                // Then
                Assert.Equal(expected, result.FullPath);
            }
        }

        public sealed class TheChangeExtensionMethod
        {
            [Theory]
            [InlineData("temp/hello.txt", ".dat", "temp/hello.dat")]
            [InlineData("temp/hello", ".dat", "temp/hello.dat")]
            [InlineData("./", ".dat", "")]
            [InlineData("temp/hello.txt", null, "temp/hello")]
            [InlineData("temp/hello.txt", "", "temp/hello.")]
            [InlineData("temp/hello.txt", ".", "temp/hello.")]
            public void Can_Change_Extension_Of_Path(string input, string extension, string expected)
            {
                // Given
                var path = new FilePath(input);

                // When
                path = path.ChangeExtension(extension);

                // Then
                Assert.Equal(expected, path.ToString());
            }

            [WindowsTheory]
            [InlineData("C:/temp/hello.txt", ".dat", "C:/temp/hello.dat")]
            [InlineData("C:/temp/hello", ".dat", "C:/temp/hello.dat")]
            [InlineData("C:/", ".dat", "C:/.dat")]
            [InlineData("C:/temp/hello.txt", null, "C:/temp/hello")]
            [InlineData("C:/temp/hello.txt", "", "C:/temp/hello.")]
            [InlineData("C:/temp/hello.txt", ".", "C:/temp/hello.")]
            public void Can_Change_Extension_Of_Windows_Path(string input, string extension, string expected)
            {
                // Given
                var path = new FilePath(input);

                // When
                path = path.ChangeExtension(extension);

                // Then
                Assert.Equal(expected, path.ToString());
            }
        }

        public sealed class TheAppendExtensionMethod
        {
            [Fact]
            public void Should_Throw_If_Extension_Is_Null()
            {
                // Given
                var path = new FilePath("temp/hello.txt");

                // When
                var result = Record.Exception(() => path.AppendExtension(null));

                // Then
                AssertEx.IsArgumentNullException(result, "extension");
            }

            [Theory]
            [InlineData("dat", "temp/hello.txt.dat")]
            [InlineData(".dat", "temp/hello.txt.dat")]
            public void Can_Append_Extension_To_Path(string extension, string expected)
            {
                // Given
                var path = new FilePath("temp/hello.txt");

                // When
                path = path.AppendExtension(extension);

                // Then
                Assert.Equal(expected, path.ToString());
            }

            [WindowsTheory]
            [InlineData("C:/temp/hello.txt", ".dat", "C:/temp/hello.txt.dat")]
            [InlineData(@"\\temp\hello.txt", ".dat", @"\\temp\hello.txt.dat")]
            public void Can_Append_Extension_To_Windows_Path(string fullPath, string extension, string expected)
            {
                // Given
                var path = new FilePath(fullPath);

                // When
                path = path.AppendExtension(extension);

                // Then
                Assert.Equal(expected, path.ToString());
            }
        }

        public sealed class TheGetFilenameMethod
        {
            [Theory]
            [InlineData("/input/test.txt", "test.txt")]
            [InlineData("/input/test.foo.txt", "test.foo.txt")]
            [InlineData("/input/test", "test")]
            [InlineData("/test.txt", "test.txt")]
            [InlineData("/test.foo.txt", "test.foo.txt")]
            [InlineData("./test.txt", "test.txt")]
            [InlineData("./test.foo.txt", "test.foo.txt")]
            [InlineData("./", "")]
            [InlineData("/", "")]
            public void Can_Get_Filename_From_Path(string input, string expected)
            {
                // Given
                var path = new FilePath(input);

                // When
                var result = path.GetFilename();

                // Then
                Assert.Equal(expected, result.FullPath);
            }

            [WindowsTheory]
            [InlineData("C:/input/test.txt", "test.txt")]
            [InlineData("C:/input/test.foo.txt", "test.foo.txt")]
            [InlineData("C:/input/test", "test")]
            [InlineData("C:/test.txt", "test.txt")]
            [InlineData("C:/test.foo.txt", "test.foo.txt")]
            [InlineData("C:/", "")]
            [InlineData(@"\\input\test.txt", "test.txt")]
            [InlineData(@"\\input\test.foo.txt", "test.foo.txt")]
            [InlineData(@"\\input\test", "test")]
            [InlineData(@"\\test.txt", "test.txt")]
            [InlineData(@"\\test.foo.txt", "test.foo.txt")]
            [InlineData(@"\\", "")]
            public void Can_Get_Filename_From_Windows_Path(string input, string expected)
            {
                // Given
                var path = new FilePath(input);

                // When
                var result = path.GetFilename();

                // Then
                Assert.Equal(expected, result.FullPath);
            }
        }

        public sealed class TheGetFilenameWithoutExtensionMethod
        {
            [Theory]
            [InlineData("/input/test.txt", "test")]
            [InlineData("/input/test.foo.txt", "test.foo")]
            [InlineData("/input/test", "test")]
            [InlineData("/test.txt", "test")]
            [InlineData("/test.foo.txt", "test.foo")]
            [InlineData("./test.txt", "test")]
            [InlineData("./test.foo.txt", "test.foo")]
            [InlineData("./", "")]
            public void Should_Return_Filename_Without_Extension_From_Path(string fullPath, string expected)
            {
                // Given
                var path = new FilePath(fullPath);

                // When
                var result = path.GetFilenameWithoutExtension();

                // Then
                Assert.Equal(expected, result.FullPath);
            }

            [WindowsTheory]
            [InlineData("C:/input/test.txt", "test")]
            [InlineData("C:/input/test.foo.txt", "test.foo")]
            [InlineData("C:/input/test", "test")]
            [InlineData("C:/test.txt", "test")]
            [InlineData("C:/test.foo.txt", "test.foo")]
            [InlineData("C:/", "")]
            [InlineData(@"\\input\test.txt", "test")]
            [InlineData(@"\\input\test.foo.txt", "test.foo")]
            [InlineData(@"\\input\test", "test")]
            [InlineData(@"\\test.txt", "test")]
            [InlineData(@"\\test.foo.txt", "test.foo")]
            [InlineData(@"\\", "")]
            public void Should_Return_Filename_Without_Extension_From_Windows_Path(string fullPath, string expected)
            {
                // Given
                var path = new FilePath(fullPath);

                // When
                var result = path.GetFilenameWithoutExtension();

                // Then
                Assert.Equal(expected, result.FullPath);
            }
        }

        public sealed class TheMakeAbsoluteMethod
        {
            public sealed class WithEnvironment
            {
                [Fact]
                public void Should_Throw_If_Environment_Is_Null()
                {
                    // Given
                    var path = new FilePath("temp/hello.txt");

                    // When
                    var result = Record.Exception(() => path.MakeAbsolute((ICakeEnvironment)null));

                    // Then
                    AssertEx.IsArgumentNullException(result, "environment");
                }

                [Fact]
                public void Should_Return_A_Absolute_File_Path_If_File_Path_Is_Relative()
                {
                    // Given
                    var path = new FilePath("./test.txt");
                    var environment = Substitute.For<ICakeEnvironment>();
                    environment.WorkingDirectory.Returns(new DirectoryPath("/absolute"));

                    // When
                    var result = path.MakeAbsolute(environment);

                    // Then
                    Assert.Equal("/absolute/test.txt", result.FullPath);
                }

                [Theory]
                [InlineData("/test.txt")]
                public void Should_Return_Same_File_Path_If_File_Path_Is_Absolute(string fullPath)
                {
                    // Given
                    var path = new FilePath(fullPath);
                    var environment = Substitute.For<ICakeEnvironment>();
                    environment.WorkingDirectory.Returns(new DirectoryPath("/absolute"));

                    // When
                    var result = path.MakeAbsolute(environment);

                    // Then
                    Assert.Equal(fullPath, result.FullPath);
                }

                [WindowsTheory]
                [InlineData("C:/foo/bar.txt")]
                [InlineData(@"\\foo\bar.txt")]
                public void Should_Create_New_Absolute_Windows_Path_Identical_To_The_Path(string fullPath)
                {
                    // Given
                    var path = new FilePath(fullPath);
                    var environment = Substitute.For<ICakeEnvironment>();
                    environment.WorkingDirectory.Returns(new DirectoryPath("/absolute"));

                    // When
                    var result = path.MakeAbsolute(environment);

                    // Then
                    Assert.Equal(fullPath, result.FullPath);
                    Assert.NotSame(path, result);
                }
            }

            public sealed class WithDirectoryPath
            {
                [Fact]
                public void Should_Throw_If_Provided_Directory_Is_Null()
                {
                    // Given
                    var path = new FilePath("./test.txt");

                    // When
                    var result = Record.Exception(() => path.MakeAbsolute((DirectoryPath)null));

                    // Then
                    AssertEx.IsArgumentNullException(result, "path");
                }

                [Fact]
                public void Should_Throw_If_Provided_Directory_Is_Relative()
                {
                    // Given
                    var path = new FilePath("./test.txt");
                    var directory = new DirectoryPath("./relative");

                    // When
                    var result = Record.Exception(() => path.MakeAbsolute(directory));

                    // Then
                    Assert.IsType<InvalidOperationException>(result);
                    Assert.Equal("Cannot make a file path absolute with a relative directory path.", result?.Message);
                }

                [Fact]
                public void Should_Return_A_Absolute_File_Path_If_File_Path_Is_Relative()
                {
                    // Given
                    var path = new FilePath("./test.txt");
                    var directory = new DirectoryPath("/absolute");

                    // When
                    var result = path.MakeAbsolute(directory);

                    // Then
                    Assert.Equal("/absolute/test.txt", result.FullPath);
                }

                [Fact]
                public void Should_Return_Same_File_Path_If_File_Path_Is_Absolute()
                {
                    // Given
                    var path = new FilePath("/test.txt");
                    var directory = new DirectoryPath("/absolute");

                    // When
                    var result = path.MakeAbsolute(directory);

                    // Then
                    Assert.Equal("/test.txt", result.FullPath);
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
                    [InlineData("C:/A/B/C/hello.txt", "C:/A/B/C", ".")]
                    [InlineData("C:/hello.txt", "C:/", ".")]
                    [InlineData("C:/A/B/C/hello.txt", "C:/A/D/E", "../../D/E")]
                    [InlineData("C:/A/B/C/hello.txt", "C:/", "../../..")]
                    [InlineData("C:/A/B/C/D/E/F/hello.txt", "C:/A/B/C", "../../..")]
                    [InlineData("C:/A/B/C/hello.txt", "C:/A/B/C/D/E/F", "D/E/F")]

                    [InlineData(@"\\A\B\C\hello.txt", @"\\A\B\C", ".")]
                    [InlineData(@"\\hello.txt", @"\\", ".")]
                    [InlineData(@"\\A\B\C\hello.txt", @"\\A\D\E", @"../../D/E")]
                    [InlineData(@"\\A\B\C\hello.txt", @"\\", @"../../..")]
                    [InlineData(@"\\A\B\C\D\E\F\hello.txt", @"\\A\B\C", @"../../..")]
                    [InlineData(@"\\A\B\C\hello.txt", @"\\A\B\C\D\E\F", @"D/E/F")]
                    public void Should_Returns_Relative_Path_Between_Paths(string from, string to, string expected)
                    {
                        // Given
                        var path = new FilePath(from);

                        // When
                        var relativePath = path.GetRelativePath(new DirectoryPath(to));

                        // Then
                        Assert.Equal(expected, relativePath.FullPath);
                    }

                    [WindowsTheory]
                    [InlineData("C:/A/B/C/hello.txt", "D:/A/B/C")]
                    [InlineData("C:/A/B/hello.txt", "D:/E/")]
                    [InlineData("C:/hello.txt", "B:/")]
                    [InlineData(@"\\A\B\C\hello.txt", "D:/A/B/C")]
                    [InlineData(@"\\A\B\hello.txt", "D:/E/")]
                    [InlineData(@"\\hello.txt", "B:/")]
                    public void Should_Throw_If_No_Relative_Path_Can_Be_Found(string from, string to)
                    {
                        // Given
                        var path = new FilePath(from);

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
                        var path = new FilePath("C:/A/B/C/hello.txt");

                        // When
                        var result = Record.Exception(() => path.GetRelativePath((DirectoryPath)null));

                        // Then
                        AssertEx.IsArgumentNullException(result, "to");
                    }

                    [WindowsFact]
                    public void Should_Throw_If_Source_DirectoryPath_Is_Relative()
                    {
                        // Given
                        var path = new FilePath("A/hello.txt");

                        // When
                        var result = Record.Exception(() => path.GetRelativePath(new DirectoryPath("C:/D/E/F")));

                        // Then
                        Assert.IsType<InvalidOperationException>(result);
                        Assert.Equal("Source path must be an absolute path.", result?.Message);
                    }

                    [WindowsTheory]
                    [InlineData("C:/A/B/C/hello.txt")]
                    [InlineData(@"\\A\B\C\hello.txt")]
                    public void Should_Throw_If_Target_DirectoryPath_Is_Relative(string input)
                    {
                        // Given
                        var path = new FilePath(input);

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
                    [InlineData("/C/A/B/C/hello.txt", "/C/A/B/C", ".")]
                    [InlineData("/C/hello.txt", "/C/", ".")]
                    [InlineData("/C/A/B/C/hello.txt", "/C/A/D/E", "../../D/E")]
                    [InlineData("/C/A/B/C/hello.txt", "/C/", "../../..")]
                    [InlineData("/C/A/B/C/D/E/F/hello.txt", "/C/A/B/C", "../../..")]
                    [InlineData("/C/A/B/C/hello.txt", "/C/A/B/C/D/E/F", "D/E/F")]
                    public void Should_Returns_Relative_Path_Between_Paths(string from, string to, string expected)
                    {
                        // Given
                        var path = new FilePath(from);

                        // When
                        var relativePath = path.GetRelativePath(new DirectoryPath(to));

                        // Then
                        Assert.Equal(expected, relativePath.FullPath);
                    }

                    [Theory]
                    [InlineData("/C/A/B/C/hello.txt", "/D/A/B/C")]
                    [InlineData("/C/A/B/hello.txt", "/D/E/")]
                    [InlineData("/C/hello.txt", "/B/")]
                    public void Should_Throw_If_No_Relative_Path_Can_Be_Found(string from, string to)
                    {
                        // Given
                        var path = new FilePath(from);

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
                        var path = new FilePath("/C/A/B/C/hello.txt");

                        // When
                        var result = Record.Exception(() => path.GetRelativePath((DirectoryPath)null));

                        // Then
                        AssertEx.IsArgumentNullException(result, "to");
                    }

                    [Fact]
                    public void Should_Throw_If_Source_DirectoryPath_Is_Relative()
                    {
                        // Given
                        var path = new FilePath("A/hello.txt");

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
                        var path = new FilePath("/C/A/B/C/hello.txt");

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
                    [InlineData("C:/A/B/C/hello.txt", "C:/A/B/C/hello.txt", "hello.txt")]
                    [InlineData("C:/hello.txt", "C:/hello.txt", "hello.txt")]
                    [InlineData("C:/hello.txt", "C:/world.txt", "world.txt")]
                    [InlineData("C:/A/B/C/hello.txt", "C:/A/D/E/hello.txt", "../../D/E/hello.txt")]
                    [InlineData("C:/A/B/C/hello.txt", "C:/hello.txt", "../../../hello.txt")]
                    [InlineData("C:/A/B/C/D/E/F/hello.txt", "C:/A/B/C/hello.txt", "../../../hello.txt")]
                    [InlineData("C:/A/B/C/hello.txt", "C:/A/B/C/D/E/F/hello.txt", "D/E/F/hello.txt")]
                    [InlineData(@"\\A\B\C\hello.txt", @"\\A\B\C\hello.txt", "hello.txt")]
                    [InlineData(@"\\hello.txt", @"\\hello.txt", "hello.txt")]
                    [InlineData(@"\\hello.txt", @"\\world.txt", "world.txt")]
                    [InlineData(@"\\A\B\C\hello.txt", @"\\A\D\E\hello.txt", "../../D/E/hello.txt")]
                    [InlineData(@"\\A\B\C\hello.txt", @"\\hello.txt", "../../../hello.txt")]
                    [InlineData(@"\\A\B\C\D\E\F\hello.txt", @"\\A\B\C\hello.txt", "../../../hello.txt")]
                    [InlineData(@"\\A\B\C\hello.txt", @"\\A\B\C\D\E\F\hello.txt", "D/E/F/hello.txt")]
                    public void Should_Returns_Relative_Path_Between_Paths(string from, string to, string expected)
                    {
                        // Given
                        var path = new FilePath(from);

                        // When
                        var relativePath = path.GetRelativePath(new FilePath(to));

                        // Then
                        Assert.Equal(expected, relativePath.FullPath);
                    }

                    [WindowsTheory]
                    [InlineData("C:/A/B/C/hello.txt", "D:/A/B/C/hello.txt")]
                    [InlineData("C:/A/B/hello.txt", "D:/E/hello.txt")]
                    [InlineData("C:/hello.txt", "B:/hello.txt")]
                    [InlineData(@"\\A\B\C\hello.txt", "D:/A/B/C/hello.txt")]
                    [InlineData(@"\\A\B\hello.txt", "D:/E/hello.txt")]
                    [InlineData(@"\\hello.txt", "B:/hello.txt")]
                    public void Should_Throw_If_No_Relative_Path_Can_Be_Found(string from, string to)
                    {
                        // Given
                        var path = new FilePath(from);

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
                        var path = new FilePath("C:/A/B/C/hello.txt");

                        // When
                        var result = Record.Exception(() => path.GetRelativePath((FilePath)null));

                        // Then
                        AssertEx.IsArgumentNullException(result, "to");
                    }

                    [WindowsFact]
                    public void Should_Throw_If_Source_DirectoryPath_Is_Relative()
                    {
                        // Given
                        var path = new FilePath("A/hello.txt");

                        // When
                        var result = Record.Exception(() => path.GetRelativePath(new FilePath("C:/D/E/F/hello.txt")));

                        // Then
                        Assert.IsType<InvalidOperationException>(result);
                        Assert.Equal("Source path must be an absolute path.", result?.Message);
                    }

                    [WindowsTheory]
                    [InlineData("C:/A/B/C/hello.txt")]
                    [InlineData(@"\\A\B\C\hello.txt")]
                    public void Should_Throw_If_Target_FilePath_Is_Relative(string input)
                    {
                        // Given
                        var path = new FilePath(input);

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
                    [InlineData("/C/A/B/C/hello.txt", "/C/A/B/C/hello.txt", "hello.txt")]
                    [InlineData("/C/hello.txt", "/C/hello.txt", "hello.txt")]
                    [InlineData("/C/hello.txt", "/C/world.txt", "world.txt")]
                    [InlineData("/C/A/B/C/hello.txt", "/C/A/D/E/hello.txt", "../../D/E/hello.txt")]
                    [InlineData("/C/A/B/C/hello.txt", "/C/hello.txt", "../../../hello.txt")]
                    [InlineData("/C/A/B/C/D/E/F/hello.txt", "/C/A/B/C/hello.txt", "../../../hello.txt")]
                    [InlineData("/C/A/B/C/hello.txt", "/C/A/B/C/D/E/F/hello.txt", "D/E/F/hello.txt")]
                    public void Should_Returns_Relative_Path_Between_Paths(string from, string to, string expected)
                    {
                        // Given
                        var path = new FilePath(from);

                        // When
                        var relativePath = path.GetRelativePath(new FilePath(to));

                        // Then
                        Assert.Equal(expected, relativePath.FullPath);
                    }

                    [Theory]
                    [InlineData("/C/A/B/C/hello.txt", "/D/A/B/C/hello.txt")]
                    [InlineData("/C/A/B/hello.txt", "/D/E/hello.txt")]
                    [InlineData("/C/hello.txt", "/B/hello.txt")]
                    public void Should_Throw_If_No_Relative_Path_Can_Be_Found(string from, string to)
                    {
                        // Given
                        var path = new FilePath(from);

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
                        var path = new FilePath("/C/A/B/C/hello.txt");

                        // When
                        var result = Record.Exception(() => path.GetRelativePath((FilePath)null));

                        // Then
                        AssertEx.IsArgumentNullException(result, "to");
                    }

                    [Fact]
                    public void Should_Throw_If_Source_DirectoryPath_Is_Relative()
                    {
                        // Given
                        var path = new FilePath("A/hello.txt");

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
                        var path = new FilePath("/C/A/B/C/hello.txt");

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