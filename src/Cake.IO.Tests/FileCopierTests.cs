using System;
using System.Collections.Generic;
using System.IO;
using Cake.Core;
using Cake.Core.IO;
using Cake.IO.Tests.Fixtures;
using NSubstitute;
using Xunit;

namespace Cake.IO.Tests
{
    public sealed class FileCopierTests
    {
        public sealed class TheCopyToDirectoryMethod
        {
            [Fact]
            public void Should_Throw_If_Context_Is_Null()
            {
                // Given, When
                var result = Record.Exception(() =>
                    FileExtensions.CopyFileToDirectory(null, "./file.txt", "./target"));

                // Then
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("context", ((ArgumentNullException)result).ParamName);
            }

            [Fact]
            public void Should_Throw_If_File_Path_Is_Null()
            {
                // Given
                var context = Substitute.For<ICakeContext>();

                // When
                var result = Record.Exception(() => 
                    FileExtensions.CopyFileToDirectory(context, null, "./target"));

                // Then
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("filePath", ((ArgumentNullException) result).ParamName);
            }

            [Fact]
            public void Should_Throw_If_Target_Directory_Path_Is_Null()
            {
                // Given
                var context = Substitute.For<ICakeContext>();

                // When
                var result = Record.Exception(() =>
                    FileExtensions.CopyFileToDirectory(context, "./file.txt", null));

                // Then
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("targetDirectoryPath", ((ArgumentNullException)result).ParamName);
            }

            [Fact]
            public void Should_Copy_File()
            {
                // Given
                var fixture = new FileCopyFixture();

                // When
                FileExtensions.CopyFileToDirectory(fixture.Context, "./file1.txt", "./target");

                // Then
                fixture.TargetFiles[0].Received(1).Copy(
                    Arg.Is<FilePath>(p => p.FullPath == "/Working/target/file1.txt"));
            }
        }

        public sealed class TheCopyFileMethod
        {
            [Fact]
            public void Should_Throw_If_Context_Is_Null()
            {
                // Given, When
                var result = Record.Exception(() =>
                    FileExtensions.CopyFile(null, "./file.txt", "./target"));

                // Then
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("context", ((ArgumentNullException)result).ParamName);
            }

            [Fact]
            public void Should_Throw_If_File_Path_Is_Null()
            {
                // Given
                var context = Substitute.For<ICakeContext>();

                // When
                var result = Record.Exception(() =>
                    FileExtensions.CopyFile(context, null, "./target"));

                // Then
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("filePath", ((ArgumentNullException)result).ParamName);
            }

            [Fact]
            public void Should_Throw_If_Target_File_Path_Is_Null()
            {
                // Given
                var context = Substitute.For<ICakeContext>();

                // When
                var result = Record.Exception(() =>
                    FileExtensions.CopyFile(context, "./file.txt", null));

                // Then
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("targetFilePath", ((ArgumentNullException)result).ParamName);
            }

            [Fact]
            public void Should_Throw_If_Target_Directory_Do_Not_Exist()
            {
                // Given
                var fixture = new FileCopyFixture();
                fixture.TargetDirectory = Substitute.For<IDirectory>();
                fixture.TargetDirectory.Exists.Returns(false);
                
                // When
                var result = Record.Exception(() => 
                    FileExtensions.CopyFile(fixture.Context, "./file1.txt", "./target/file1.txt"));

                // Then
                Assert.IsType<DirectoryNotFoundException>(result);
                Assert.Equal("The directory '/Working/target' do not exist.", result.Message);
            }

            [Fact]
            public void Should_Throw_If_File_Do_Not_Exist()
            {
                // Given
                var fixture = new FileCopyFixture();
                fixture.TargetFiles[0] = Substitute.For<IFile>();
                fixture.TargetFiles[0].Exists.Returns(false);

                // When
                var result = Record.Exception(() => 
                    FileExtensions.CopyFile(fixture.Context, "./file1.txt", "./target/file1.txt"));

                // Then
                Assert.IsType<FileNotFoundException>(result);
                Assert.Equal("The file '/Working/file1.txt' do not exist.", result.Message);
            }

            [Fact]
            public void Should_Copy_File()
            {
                // Given
                var fixture = new FileCopyFixture();

                // When
                FileExtensions.CopyFile(fixture.Context, "./file1.txt", "./target/file1.txt");

                // Then
                fixture.TargetFiles[0].Received(1).Copy(
                    Arg.Is<FilePath>(p => p.FullPath == "/Working/target/file1.txt"));
            }
        }

        public sealed class TheCopyFilesMethod
        {
            public sealed class WithFilePaths
            {
                [Fact]
                public void Should_Throw_If_Context_Is_Null()
                {
                    // Given
                    var fixture = new FileCopyFixture();

                    // When
                    var result = Record.Exception(() =>
                        FileExtensions.CopyFiles(null, fixture.SourceFilePaths, "./target"));

                    // Then
                    Assert.IsType<ArgumentNullException>(result);
                    Assert.Equal("context", ((ArgumentNullException) result).ParamName);
                }

                [Fact]
                public void Should_Throw_If_File_Paths_Are_Null()
                {
                    // Given
                    var context = Substitute.For<ICakeContext>();

                    // When
                    var result = Record.Exception(() =>
                        FileExtensions.CopyFiles(context, (IEnumerable<FilePath>)null, "./target"));

                    // Then
                    Assert.IsType<ArgumentNullException>(result);
                    Assert.Equal("filePaths", ((ArgumentNullException) result).ParamName);
                }

                [Fact]
                public void Should_Throw_If_Target_Directory_Path_Is_Null()
                {
                    // Given
                    var fixture = new FileCopyFixture();

                    // When
                    var result = Record.Exception(() =>
                        FileExtensions.CopyFiles(fixture.Context, fixture.SourceFilePaths, null));

                    // Then
                    Assert.IsType<ArgumentNullException>(result);
                    Assert.Equal("targetDirectoryPath", ((ArgumentNullException) result).ParamName);
                }

                [Fact]
                public void Should_Throw_If_Any_Target_Directory_Do_Not_Exist()
                {
                    // Given
                    var fixture = new FileCopyFixture();
                    fixture.TargetDirectory = Substitute.For<IDirectory>();
                    fixture.TargetDirectory.Exists.Returns(false);

                    // When
                    var result = Record.Exception(() =>
                        FileExtensions.CopyFiles(fixture.Context, fixture.SourceFilePaths, "./target"));

                    // Then
                    Assert.IsType<DirectoryNotFoundException>(result);
                    Assert.Equal("The directory '/Working/target' do not exist.", result.Message);
                }

                [Fact]
                public void Should_Throw_If_Any_File_Do_Not_Exist()
                {
                    // Given
                    var fixture = new FileCopyFixture();
                    fixture.TargetFiles[1] = Substitute.For<IFile>();
                    fixture.TargetFiles[1].Exists.Returns(false);

                    // When
                    var result = Record.Exception(() =>
                        FileExtensions.CopyFiles(fixture.Context, fixture.SourceFilePaths, "./target"));

                    // Then
                    Assert.IsType<FileNotFoundException>(result);
                    Assert.Equal("The file '/Working/file2.txt' do not exist.", result.Message);
                }

                [Fact]
                public void Should_Copy_Files()
                {
                    // Given
                    var fixture = new FileCopyFixture();

                    // When
                    FileExtensions.CopyFiles(fixture.Context, fixture.SourceFilePaths, "./target");

                    // Then
                    fixture.TargetFiles[0].Received(1).Copy(Arg.Any<FilePath>());
                    fixture.TargetFiles[1].Received(1).Copy(Arg.Any<FilePath>());
                }
            }

            public sealed class WithGlobExpression
            {
                [Fact]
                public void Should_Throw_If_Context_Is_Null()
                {
                    // Given, When
                    var result = Record.Exception(() =>
                        FileExtensions.CopyFiles(null, "", "./target"));

                    // Then
                    Assert.IsType<ArgumentNullException>(result);
                    Assert.Equal("context", ((ArgumentNullException)result).ParamName);
                }

                [Fact]
                public void Should_Throw_If_Glob_Expression_Is_Null()
                {
                    // Given
                    var fixture = new FileCopyFixture();

                    // When
                    var result = Record.Exception(() =>
                        FileExtensions.CopyFiles(fixture.Context, (string)null, "./target"));

                    // Then
                    Assert.IsType<ArgumentNullException>(result);
                    Assert.Equal("pattern", ((ArgumentNullException)result).ParamName);
                }

                [Fact]
                public void Should_Throw_If_Target_Directory_Path_Is_Null()
                {
                    // Given
                    var fixture = new FileCopyFixture();

                    // When
                    var result = Record.Exception(() =>
                        FileExtensions.CopyFiles(fixture.Context, "", null));

                    // Then
                    Assert.IsType<ArgumentNullException>(result);
                    Assert.Equal("targetDirectoryPath", ((ArgumentNullException)result).ParamName);
                }

                [Fact]
                public void Should_Throw_If_Any_Target_Directory_Do_Not_Exist()
                {
                    // Given
                    var fixture = new FileCopyFixture();
                    fixture.TargetDirectory = Substitute.For<IDirectory>();
                    fixture.TargetDirectory.Exists.Returns(false);

                    // When
                    var result = Record.Exception(() =>
                        FileExtensions.CopyFiles(fixture.Context, "*", "./target"));

                    // Then
                    Assert.IsType<DirectoryNotFoundException>(result);
                    Assert.Equal("The directory '/Working/target' do not exist.", result.Message);
                }

                [Fact]
                public void Should_Throw_If_Any_File_Do_Not_Exist()
                {
                    // Given
                    var fixture = new FileCopyFixture();
                    fixture.TargetFiles[1] = Substitute.For<IFile>();
                    fixture.TargetFiles[1].Exists.Returns(false);

                    // When
                    var result = Record.Exception(() =>
                        FileExtensions.CopyFiles(fixture.Context, "*", "./target"));

                    // Then
                    Assert.IsType<FileNotFoundException>(result);
                    Assert.Equal("The file '/Working/file2.txt' do not exist.", result.Message);
                }

                [Fact]
                public void Should_Copy_Files()
                {
                    // Given
                    var fixture = new FileCopyFixture();

                    // When
                    FileExtensions.CopyFiles(fixture.Context, "*", "./target");

                    // Then
                    fixture.TargetFiles[0].Received(1).Copy(Arg.Any<FilePath>());
                    fixture.TargetFiles[1].Received(1).Copy(Arg.Any<FilePath>());
                }
            }
        }
    }
}
