using System;
using Cake.Core.IO;
using NSubstitute;
using Xunit;

namespace Cake.Core.Tests.Unit.IO
{
    public sealed class FilePathTests
    {
        public sealed class TheHasExtensionProperty
        {
            [Theory]
            [InlineData("assets/shaders/basic.txt", true)]
            [InlineData("assets/shaders/basic", false)]
            [InlineData("assets/shaders/basic/", false)]
            public void Can_See_If_A_Path_Has_An_Extension(string fullPath, bool expected)
            {
                // Given, When
                var path = new FilePath(fullPath);

                // Then
                Assert.Equal(expected, path.HasExtension);
            }
        }

        public sealed class TheGetExtensionProperty
        {
            [Theory]
            [InlineData("assets/shaders/basic.frag", ".frag")]
            [InlineData("assets/shaders/basic.frag/test.vert", ".vert")]
            [InlineData("assets/shaders/basic", null)]
            [InlineData("assets/shaders/basic.frag/test", null)]
            public void Can_Get_Extension(string fullPath, string expected)
            {
                // Given, When
                var result = new FilePath(fullPath);
                var extension = result.GetExtension();

                // Then
                Assert.Equal(expected, extension);
            }
        }

        public sealed class TheGetDirectoryMethod
        {
            [Fact]
            public void Can_Get_Directory_For_File_Path()
            {
                // Given, When
                var path = new FilePath("temp/hello.txt");
                var directory = path.GetDirectory();

                // Then
                Assert.Equal("temp", directory.FullPath);
            }

            [Fact]
            public void Can_Get_Directory_For_File_Path_In_Root()
            {
                // Given, When
                var path = new FilePath("hello.txt");
                var directory = path.GetDirectory();

                // Then
                Assert.Equal(string.Empty, directory.FullPath);
            }
        }

        public sealed class TheChangeExtensionMethod
        {
            [Fact]
            public void Can_Change_Extension_Of_Path()
            {
                // Given
                var path = new FilePath("temp/hello.txt");

                // When
                path = path.ChangeExtension(".dat");

                // Then
                Assert.Equal("temp/hello.dat", path.ToString());
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
                Assert.IsArgumentNullException(result, "extension");
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
        }

        public sealed class TheGetFilenameMethod
        {
            [Fact]
            public void Can_Get_Filename_From_Path()
            {
                // Given
                var path = new FilePath("/input/test.txt");

                // When
                var result = path.GetFilename();

                // Then
                Assert.Equal("test.txt", result.FullPath);
            }
        }

        public sealed class TheGetFilenameWithoutExtensionMethod
        {
            [Theory]
            [InlineData("/input/test.txt", "test")]
            [InlineData("/input/test", "test")]
            public void Should_Return_Filename_Without_Extension_From_Path(string fullPath, string expected)
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
                    Assert.IsArgumentNullException(result, "environment");
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

                [Fact]
                public void Should_Return_Same_File_Path_If_File_Path_Is_Absolute()
                {
                    // Given
                    var path = new FilePath("/test.txt");
                    var environment = Substitute.For<ICakeEnvironment>();
                    environment.WorkingDirectory.Returns(new DirectoryPath("/absolute"));

                    // When
                    var result = path.MakeAbsolute(environment);

                    // Then
                    Assert.Equal("/test.txt", result.FullPath);
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
                    Assert.IsArgumentNullException(result, "path");
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
                    Assert.Equal("Cannot make a file path absolute with a relative directory path.", result.Message);
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
    }
}