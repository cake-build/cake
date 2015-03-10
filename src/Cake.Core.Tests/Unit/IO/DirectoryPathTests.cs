using System;
using Cake.Core.IO;
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
                Assert.IsArgumentNullException(result, "path");
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
                Assert.IsArgumentNullException(result, "path");
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
                Assert.Equal("Cannot combine a directory path with an absolute file path.", result.Message);
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
                Assert.IsArgumentNullException(result, "path");
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
                Assert.Equal("Cannot combine a directory path with an absolute directory path.", result.Message);
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
                        () => path.MakeAbsolute((ICakeEnvironment) null));

                    // Then
                    Assert.IsArgumentNullException(result, "environment");
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
                    Assert.IsArgumentNullException(result, "path");
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
                    Assert.Equal("The provided path cannot be relative.", result.Message);
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

        public sealed class TheAddOperator
        {
            public sealed class ForDirectoryPath
            {
                [Fact]
                public void Should_Combine_Paths()
                {
                    // Given
                    var left = new DirectoryPath("./temp");
                    var right = new DirectoryPath("files");

                    // When
                    var result = left + right;

                    // Then
                    Assert.IsType<DirectoryPath>(result);
                    Assert.Equal("temp/files", result.FullPath);
                }

                [Fact]
                public void Should_Throw_If_Proxy_Is_Null()
                {
                    // Given
                    var left = (DirectoryPath)null;
                    var right = new DirectoryPath("files");

                    // When
                    var result = Record.Exception(() => left + right);

                    // Then
                    Assert.IsArgumentNullException(result, "left");
                }

                [Fact]
                public void Should_Throw_If_Directory_Path_Is_Null()
                {
                    // Given
                    var left = new DirectoryPath("./temp");
                    var right = (DirectoryPath)null;

                    // When
                    var result = Record.Exception(() => left + right);

                    // Then
                    Assert.IsArgumentNullException(result, "right");
                }
            }

            public sealed class ForFilePath
            {
                [Fact]
                public void Should_Combine_Paths()
                {
                    // Given
                    var left = new DirectoryPath("./temp");
                    var right = new FilePath("file.txt");

                    // When
                    var result = left + right;

                    // Then
                    Assert.IsType<FilePath>(result);
                    Assert.Equal("temp/file.txt", result.FullPath);
                }

                [Fact]
                public void Should_Throw_If_Left_Is_Null()
                {
                    // Given
                    var left = (DirectoryPath)null;
                    var right = new FilePath("file.txt");

                    // When
                    var result = Record.Exception(() => left + right);

                    // Then
                    Assert.IsArgumentNullException(result, "directory");
                }

                [Fact]
                public void Should_Return_Left_Path_If_Left_Is_Null()
                {
                    // Given
                    var left = new DirectoryPath("./temp");
                    var right = (FilePath)null;

                    // When
                    var result = Record.Exception(() => left + right);

                    // Then
                    Assert.IsArgumentNullException(result, "file");
                }
            }
        }
    }
}