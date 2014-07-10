using System;
using System.Collections.Generic;
using System.IO;
using Cake.Common.Tests.Fixtures;
using Cake.Core;
using Cake.Core.IO;
using NSubstitute;
using Xunit;
using FileExtensions = Cake.Common.IO.FileExtensions;

namespace Cake.Common.Tests.Unit.IO
{
    public sealed class FileMoverTests
    {
        public sealed class TheMoveFileToDirectory
        {
            [Fact]
            public void Should_Throw_If_Context_Is_Null()
            {
                // Given, When
                var source = new FilePath("./source.txt");
                var target = new DirectoryPath("./target");

                var result = Record.Exception(() =>
                    FileExtensions.MoveFileToDirectory(null, source, target));

                // Then
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("context", ((ArgumentNullException)result).ParamName);
            }

            [Fact]
            public void Should_Throw_If_Source_File_Path_Is_Null()
            {
                // Given
                var context = Substitute.For<ICakeContext>();
                var target = new DirectoryPath("./target");

                // When
                var result = Record.Exception(() =>
                    FileExtensions.MoveFileToDirectory(context, null, target));

                // Then
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("filePath", ((ArgumentNullException)result).ParamName);
            }

            [Fact]
            public void Should_Throw_If_Target_File_Path_Is_Null()
            {
                // Given
                var context = Substitute.For<ICakeContext>();
                var source = new FilePath("./source.txt");

                // When
                var result = Record.Exception(() =>
                    FileExtensions.MoveFileToDirectory(context, source, null));

                // Then
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("targetDirectoryPath", ((ArgumentNullException)result).ParamName);
            }

            [Fact]
            public void Should_Move_File()
            {
                // Given
                var fixture = new FileCopyFixture();

                // When
                FileExtensions.MoveFileToDirectory(fixture.Context, "./file1.txt", "./target");

                // Then
                fixture.TargetFiles[0].Received(1).Move(
                    Arg.Is<FilePath>(p => p.FullPath == "/Working/target/file1.txt"));
            }
        }

        public sealed class TheMoveFilesMethod
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
                        FileExtensions.MoveFiles(null, fixture.SourceFilePaths, "./target"));

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
                        FileExtensions.MoveFiles(context, (IEnumerable<FilePath>)null, "./target"));

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
                        FileExtensions.MoveFiles(fixture.Context, fixture.SourceFilePaths, null));

                    // Then
                    Assert.IsType<ArgumentNullException>(result);
                    Assert.Equal("targetDirectoryPath", ((ArgumentNullException) result).ParamName);
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
                        FileExtensions.MoveFiles(fixture.Context, fixture.SourceFilePaths, "./target"));

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
                        FileExtensions.MoveFiles(fixture.Context, fixture.SourceFilePaths, "./target"));

                    // Then
                    Assert.IsType<FileNotFoundException>(result);
                    Assert.Equal("The file '/Working/file2.txt' do not exist.", result.Message);
                }

                [Fact]
                public void Should_Move_Files()
                {
                    // Given
                    var fixture = new FileCopyFixture();

                    // When
                    FileExtensions.MoveFiles(fixture.Context, fixture.SourceFilePaths, "./target");

                    // Then
                    fixture.TargetFiles[0].Received(1).Move(Arg.Any<FilePath>());
                    fixture.TargetFiles[1].Received(1).Move(Arg.Any<FilePath>());
                }
            }

            public sealed class WithGlobExpression
            {
                [Fact]
                public void Should_Throw_If_Context_Is_Null()
                {
                    // Given, When
                    var result = Record.Exception(() =>
                        FileExtensions.MoveFiles(null, "", "./target"));

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
                        FileExtensions.MoveFiles(fixture.Context, (string)null, "./target"));

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
                        FileExtensions.MoveFiles(fixture.Context, "", null));

                    // Then
                    Assert.IsType<ArgumentNullException>(result);
                    Assert.Equal("targetDirectoryPath", ((ArgumentNullException)result).ParamName);
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
                        FileExtensions.MoveFiles(fixture.Context, "*", "./target"));

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
                        FileExtensions.MoveFiles(fixture.Context, "*", "./target"));

                    // Then
                    Assert.IsType<FileNotFoundException>(result);
                    Assert.Equal("The file '/Working/file2.txt' do not exist.", result.Message);
                }

                [Fact]
                public void Should_Move_Files()
                {
                    // Given
                    var fixture = new FileCopyFixture();

                    // When
                    FileExtensions.MoveFiles(fixture.Context, "*", "./target");

                    // Then
                    fixture.TargetFiles[0].Received(1).Move(Arg.Any<FilePath>());
                    fixture.TargetFiles[1].Received(1).Move(Arg.Any<FilePath>());
                }
            }
        }

        public sealed class TheMoveFileMethod
        {
            [Fact]
            public void Should_Throw_If_Context_Is_Null()
            {
                // Given, When
                var source = new FilePath("./source.txt");
                var target = new FilePath("./target.txt");

                var result = Record.Exception(() =>
                    FileExtensions.MoveFile(null, source, target));

                // Then
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("context", ((ArgumentNullException)result).ParamName);
            }

            [Fact]
            public void Should_Throw_If_Source_File_Path_Is_Null()
            {
                // Given
                var context = Substitute.For<ICakeContext>();
                var target = new FilePath("./target.txt");

                // When
                var result = Record.Exception(() =>
                    FileExtensions.MoveFile(context, null, target));

                // Then
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("filePath", ((ArgumentNullException)result).ParamName);
            }

            [Fact]
            public void Should_Throw_If_Target_File_Path_Is_Null()
            {
                // Given
                var context = Substitute.For<ICakeContext>();
                var source = new FilePath("./source.txt");

                // When
                var result = Record.Exception(() =>
                    FileExtensions.MoveFile(context, source, null));

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
                    FileExtensions.MoveFile(fixture.Context, "./file1.txt", "./target/file1.txt"));

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
                    FileExtensions.MoveFile(fixture.Context, "./file1.txt", "./target/file1.txt"));

                // Then
                Assert.IsType<FileNotFoundException>(result);
                Assert.Equal("The file '/Working/file1.txt' do not exist.", result.Message);
            }

            [Fact]
            public void Should_Move_File()
            {
                // Given
                var fixture = new FileCopyFixture();

                // When
                FileExtensions.MoveFile(fixture.Context, "./file1.txt", "./target/file1.txt");

                // Then
                fixture.TargetFiles[0].Received(1).Move(
                    Arg.Is<FilePath>(p => p.FullPath == "/Working/target/file1.txt"));
            }
        }
    }
}
