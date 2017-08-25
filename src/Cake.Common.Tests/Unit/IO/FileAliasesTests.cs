// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.IO;
using System.Linq;
using Cake.Common.IO;
using Cake.Common.IO.Paths;
using Cake.Common.Tests.Fixtures.IO;
using Cake.Core;
using Cake.Core.IO;
using Cake.Testing;
using NSubstitute;
using Xunit;
using LogLevel = Cake.Core.Diagnostics.LogLevel;
using Verbosity = Cake.Core.Diagnostics.Verbosity;

namespace Cake.Common.Tests.Unit.IO
{
    public sealed class FileAliasesTests
    {
        public sealed class TheFileMethod
        {
            [Fact]
            public void Should_Throw_If_Context_Is_Null()
            {
                // Given
                const string path = "./file.txt";

                // When
                var result = Record.Exception(() => FileAliases.File(null, path));

                // Then
                AssertEx.IsArgumentNullException(result, "context");
            }

            [Fact]
            public void Should_Throw_If_Path_Is_Null()
            {
                // Given
                var context = Substitute.For<ICakeContext>();

                // When
                var result = Record.Exception(() => FileAliases.File(context, null));

                // Then
                AssertEx.IsArgumentNullException(result, "path");
            }

            [Fact]
            public void Should_Return_A_Convertable_File_Path()
            {
                // Given
                var context = Substitute.For<ICakeContext>();

                // When
                var result = FileAliases.File(context, "./file.txt");

                // Then
                Assert.IsType<ConvertableFilePath>(result);
            }
        }

        public sealed class TheCopyToDirectoryMethod
        {
            [Fact]
            public void Should_Throw_If_Context_Is_Null()
            {
                // Given, When
                var result = Record.Exception(() =>
                    FileAliases.CopyFileToDirectory(null, "./file.txt", "./target"));

                // Then
                AssertEx.IsArgumentNullException(result, "context");
            }

            [Fact]
            public void Should_Throw_If_File_Path_Is_Null()
            {
                // Given
                var context = Substitute.For<ICakeContext>();

                // When
                var result = Record.Exception(() =>
                    FileAliases.CopyFileToDirectory(context, null, "./target"));

                // Then
                AssertEx.IsArgumentNullException(result, "filePath");
            }

            [Fact]
            public void Should_Throw_If_Target_Directory_Path_Is_Null()
            {
                // Given
                var context = Substitute.For<ICakeContext>();

                // When
                var result = Record.Exception(() =>
                    FileAliases.CopyFileToDirectory(context, "./file.txt", null));

                // Then
                AssertEx.IsArgumentNullException(result, "targetDirectoryPath");
            }

            [Fact]
            public void Should_Copy_File()
            {
                // Given
                var fixture = new FileCopyFixture();

                // When
                FileAliases.CopyFileToDirectory(fixture.Context, "./file1.txt", "./target");

                // Then
                fixture.TargetFiles[0].Received(1).Copy(
                    Arg.Is<FilePath>(p => p.FullPath == "/Working/target/file1.txt"), true);
            }
        }

        public sealed class TheCopyFileMethod
        {
            [Fact]
            public void Should_Throw_If_Context_Is_Null()
            {
                // Given, When
                var result = Record.Exception(() =>
                    FileAliases.CopyFile(null, "./file.txt", "./target"));

                // Then
                AssertEx.IsArgumentNullException(result, "context");
            }

            [Fact]
            public void Should_Throw_If_File_Path_Is_Null()
            {
                // Given
                var context = Substitute.For<ICakeContext>();

                // When
                var result = Record.Exception(() =>
                    FileAliases.CopyFile(context, null, "./target"));

                // Then
                AssertEx.IsArgumentNullException(result, "filePath");
            }

            [Fact]
            public void Should_Throw_If_Target_File_Path_Is_Null()
            {
                // Given
                var context = Substitute.For<ICakeContext>();

                // When
                var result = Record.Exception(() =>
                    FileAliases.CopyFile(context, "./file.txt", null));

                // Then
                AssertEx.IsArgumentNullException(result, "targetFilePath");
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
                    FileAliases.CopyFile(fixture.Context, "./file1.txt", "./target/file1.txt"));

                // Then
                Assert.IsType<DirectoryNotFoundException>(result);
                Assert.Equal("The directory '/Working/target' does not exist.", result?.Message);
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
                    FileAliases.CopyFile(fixture.Context, "./file1.txt", "./target/file1.txt"));

                // Then
                Assert.IsType<FileNotFoundException>(result);
                Assert.Equal("The file '/Working/file1.txt' does not exist.", result?.Message);
            }

            [Fact]
            public void Should_Copy_File()
            {
                // Given
                var fixture = new FileCopyFixture();

                // When
                FileAliases.CopyFile(fixture.Context, "./file1.txt", "./target/file1.txt");

                // Then
                fixture.TargetFiles[0].Received(1).Copy(
                    Arg.Is<FilePath>(p => p.FullPath == "/Working/target/file1.txt"), true);
            }

            [Fact]
            public void Should_Log_Verbose_Message_With_Correct_Target()
            {
                // Given
                var fixture = new FileCopyFixture();

                // When
                FileAliases.CopyFile(fixture.Context, "./file1.txt", "./target/file1.txt");

                // Then
                Assert.Contains(fixture.Log.Entries,
                    entry =>
                        entry.Level == LogLevel.Verbose && entry.Verbosity == Verbosity.Verbose &&
                        entry.Message == "Copying file file1.txt to /Working/target/file1.txt");
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
                        FileAliases.CopyFiles(null, fixture.SourceFilePaths, "./target"));

                    // Then
                    AssertEx.IsArgumentNullException(result, "context");
                }

                [Fact]
                public void Should_Throw_If_File_Paths_Are_Null()
                {
                    // Given
                    var context = Substitute.For<ICakeContext>();

                    // When
                    var result = Record.Exception(() =>
                        FileAliases.CopyFiles(context, (IEnumerable<FilePath>)null, "./target"));

                    // Then
                    AssertEx.IsArgumentNullException(result, "filePaths");
                }

                [Fact]
                public void Should_Throw_If_Target_Directory_Path_Is_Null()
                {
                    // Given
                    var fixture = new FileCopyFixture();

                    // When
                    var result = Record.Exception(() =>
                        FileAliases.CopyFiles(fixture.Context, fixture.SourceFilePaths, null));

                    // Then
                    AssertEx.IsArgumentNullException(result, "targetDirectoryPath");
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
                        FileAliases.CopyFiles(fixture.Context, fixture.SourceFilePaths, "./target"));

                    // Then
                    Assert.IsType<DirectoryNotFoundException>(result);
                    Assert.Equal("The directory '/Working/target' does not exist.", result?.Message);
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
                        FileAliases.CopyFiles(fixture.Context, fixture.SourceFilePaths, "./target"));

                    // Then
                    Assert.IsType<FileNotFoundException>(result);
                    Assert.Equal("The file '/Working/file2.txt' does not exist.", result?.Message);
                }

                [Fact]
                public void Should_Keep_Folder_Structure()
                {
                    // Given
                    var fixture = new FileCopyFixture();

                    // When
                    FileAliases.CopyFiles(fixture.Context, fixture.SourceFilePaths, "./target");

                    // Then
                    fixture.TargetFiles[0].Received(1).Copy(Arg.Any<FilePath>(), true);
                    fixture.TargetFiles[1].Received(1).Copy(Arg.Any<FilePath>(), true);
                }

                [Fact]
                public void Should_Copy_Files()
                {
                    // Given
                    var fixture = new FileCopyFixture();

                    // When
                    FileAliases.CopyFiles(fixture.Context, fixture.SourceFilePaths, "./target");

                    // Then
                    fixture.TargetFiles[0].Received(1).Copy(Arg.Any<FilePath>(), true);
                    fixture.TargetFiles[1].Received(1).Copy(Arg.Any<FilePath>(), true);
                }
            }

            public sealed class WithStrings
            {
                [Fact]
                public void Should_Throw_If_Context_Is_Null()
                {
                    // Given
                    var fixture = new FileCopyFixture();
                    var filePaths = fixture.SourceFilePaths.Select(x => x.FullPath);

                    // When
                    var result = Record.Exception(() =>
                        FileAliases.CopyFiles(null, filePaths, "./target"));

                    // Then
                    AssertEx.IsArgumentNullException(result, "context");
                }

                [Fact]
                public void Should_Throw_If_File_Paths_Are_Null()
                {
                    // Given
                    var context = Substitute.For<ICakeContext>();

                    // When
                    var result = Record.Exception(() =>
                        FileAliases.CopyFiles(context, (IEnumerable<string>)null, "./target"));

                    // Then
                    AssertEx.IsArgumentNullException(result, "filePaths");
                }

                [Fact]
                public void Should_Throw_If_Target_Directory_Path_Is_Null()
                {
                    // Given
                    var fixture = new FileCopyFixture();
                    var filePaths = fixture.SourceFilePaths.Select(x => x.FullPath);

                    // When
                    var result = Record.Exception(() =>
                        FileAliases.CopyFiles(fixture.Context, filePaths, null));

                    // Then
                    AssertEx.IsArgumentNullException(result, "targetDirectoryPath");
                }

                [Fact]
                public void Should_Throw_If_Any_Target_Directory_Do_Not_Exist()
                {
                    // Given
                    var fixture = new FileCopyFixture();
                    var filePaths = fixture.SourceFilePaths.Select(x => x.FullPath);
                    fixture.TargetDirectory = Substitute.For<IDirectory>();
                    fixture.TargetDirectory.Exists.Returns(false);

                    // When
                    var result = Record.Exception(() =>
                        FileAliases.CopyFiles(fixture.Context, filePaths, "./target"));

                    // Then
                    Assert.IsType<DirectoryNotFoundException>(result);
                    Assert.Equal("The directory '/Working/target' does not exist.", result?.Message);
                }

                [Fact]
                public void Should_Throw_If_Any_File_Do_Not_Exist()
                {
                    // Given
                    var fixture = new FileCopyFixture();
                    var filePaths = fixture.SourceFilePaths.Select(x => x.FullPath);
                    fixture.TargetFiles[1] = Substitute.For<IFile>();
                    fixture.TargetFiles[1].Exists.Returns(false);

                    // When
                    var result = Record.Exception(() =>
                        FileAliases.CopyFiles(fixture.Context, filePaths, "./target"));

                    // Then
                    Assert.IsType<FileNotFoundException>(result);
                    Assert.Equal("The file '/Working/file2.txt' does not exist.", result?.Message);
                }

                [Fact]
                public void Should_Keep_Folder_Structure()
                {
                    // Given
                    var fixture = new FileCopyFixture();
                    var filePaths = fixture.SourceFilePaths.Select(x => x.FullPath);

                    // When
                    FileAliases.CopyFiles(fixture.Context, filePaths, "./target");

                    // Then
                    fixture.TargetFiles[0].Received(1).Copy(Arg.Any<FilePath>(), true);
                    fixture.TargetFiles[1].Received(1).Copy(Arg.Any<FilePath>(), true);
                }

                [Fact]
                public void Should_Copy_Files()
                {
                    // Given
                    var fixture = new FileCopyFixture();
                    var filePaths = fixture.SourceFilePaths.Select(x => x.FullPath);

                    // When
                    FileAliases.CopyFiles(fixture.Context, filePaths, "./target");

                    // Then
                    fixture.TargetFiles[0].Received(1).Copy(Arg.Any<FilePath>(), true);
                    fixture.TargetFiles[1].Received(1).Copy(Arg.Any<FilePath>(), true);
                }
            }

            public sealed class WithGlobExpression
            {
                [Fact]
                public void Should_Throw_If_Context_Is_Null()
                {
                    // Given, When
                    var result = Record.Exception(() =>
                        FileAliases.CopyFiles(null, "", "./target"));

                    // Then
                    AssertEx.IsArgumentNullException(result, "context");
                }

                [Fact]
                public void Should_Throw_If_Glob_Expression_Is_Null()
                {
                    // Given
                    var fixture = new FileCopyFixture();

                    // When
                    var result = Record.Exception(() =>
                        FileAliases.CopyFiles(fixture.Context, (string)null, "./target"));

                    // Then
                    AssertEx.IsArgumentNullException(result, "pattern");
                }

                [Fact]
                public void Should_Throw_If_Target_Directory_Path_Is_Null()
                {
                    // Given
                    var fixture = new FileCopyFixture();

                    // When
                    var result = Record.Exception(() =>
                        FileAliases.CopyFiles(fixture.Context, "*", null));

                    // Then
                    AssertEx.IsArgumentNullException(result, "targetDirectoryPath");
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
                        FileAliases.CopyFiles(fixture.Context, "*", "./target"));

                    // Then
                    Assert.IsType<DirectoryNotFoundException>(result);
                    Assert.Equal("The directory '/Working/target' does not exist.", result?.Message);
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
                        FileAliases.CopyFiles(fixture.Context, "*", "./target"));

                    // Then
                    Assert.IsType<FileNotFoundException>(result);
                    Assert.Equal("The file '/Working/file2.txt' does not exist.", result?.Message);
                }

                [Fact]
                public void Should_Keep_Folder_Structure()
                {
                    // Given
                    var fixture = new FileCopyFixture();

                    // When
                    FileAliases.CopyFiles(fixture.Context, "*", "./target", true);

                    // Then
                    fixture.TargetFiles[0].Received(1).Copy(Arg.Any<FilePath>(), true);
                    fixture.TargetFiles[1].Received(1).Copy(Arg.Any<FilePath>(), true);
                }

                [Fact]
                public void Should_Copy_Files()
                {
                    // Given
                    var fixture = new FileCopyFixture();

                    // When
                    FileAliases.CopyFiles(fixture.Context, "*", "./target");

                    // Then
                    fixture.TargetFiles[0].Received(1).Copy(Arg.Any<FilePath>(), true);
                    fixture.TargetFiles[1].Received(1).Copy(Arg.Any<FilePath>(), true);
                }
            }
        }

        public sealed class TheDeleteFileMethod
        {
            [Fact]
            public void Should_Throw_If_Context_Is_Null()
            {
                // Given
                var filePath = new FilePath("./file.txt");

                // When
                var result = Record.Exception(() =>
                    FileAliases.DeleteFile(null, filePath));

                // Then
                AssertEx.IsArgumentNullException(result, "context");
            }

            [Fact]
            public void Should_Throw_If_File_Path_Is_Null()
            {
                // Given
                var context = Substitute.For<ICakeContext>();

                // When
                var result = Record.Exception(() =>
                    FileAliases.DeleteFile(context, null));

                // Then
                AssertEx.IsArgumentNullException(result, "filePath");
            }

            [Fact]
            public void Should_Throw_If_File_Do_Not_Exist()
            {
                // Given
                var fixture = new FileDeleteFixture();

                // When
                var result = Record.Exception(() =>
                    FileAliases.DeleteFile(fixture.Context, "/file.txt"));

                // Then
                Assert.IsType<FileNotFoundException>(result);
                Assert.Equal("The file '/file.txt' does not exist.", result?.Message);
            }

            [Fact]
            public void Should_Make_Relative_File_Path_Absolute()
            {
                // Given
                var fixture = new FileDeleteFixture();

                // When
                FileAliases.DeleteFile(fixture.Context, "file1.txt");

                // Then
                fixture.FileSystem.Received(1).GetFile(Arg.Is<FilePath>(
                    p => p.FullPath == "/Working/file1.txt"));
            }

            [Fact]
            public void Should_Delete_File()
            {
                // Given
                var fixture = new FileDeleteFixture();

                // When
                FileAliases.DeleteFile(fixture.Context, fixture.Paths[0]);

                // Then
                fixture.Files[0].Received(1).Delete();
            }
        }

        public sealed class TheDeleteFilesMethod
        {
            public sealed class WithFilePaths
            {
                [Fact]
                public void Should_Throw_If_Context_Is_Null()
                {
                    // Given
                    var filePaths = new FilePath[] { };

                    // When
                    var result = Record.Exception(() =>
                        FileAliases.DeleteFiles(null, filePaths));

                    // Then
                    AssertEx.IsArgumentNullException(result, "context");
                }

                [Fact]
                public void Should_Throw_If_File_Paths_Are_Null()
                {
                    // Given
                    var context = Substitute.For<ICakeContext>();

                    // When
                    var result = Record.Exception(() =>
                        FileAliases.DeleteFiles(context, (IEnumerable<FilePath>)null));

                    // Then
                    AssertEx.IsArgumentNullException(result, "filePaths");
                }

                [Fact]
                public void Should_Delete_Files()
                {
                    // Given
                    var fixture = new FileDeleteFixture();

                    // When
                    FileAliases.DeleteFiles(fixture.Context, fixture.Paths);

                    // Then
                    fixture.Files[0].Received(1).Delete();
                    fixture.Files[1].Received(1).Delete();
                }
            }

            public sealed class WithGlobExpression
            {
                [Fact]
                public void Should_Throw_If_Context_Is_Null()
                {
                    // Given, When
                    var result = Record.Exception(() =>
                        FileAliases.DeleteFiles(null, "*"));

                    // Then
                    AssertEx.IsArgumentNullException(result, "context");
                }

                [Fact]
                public void Should_Throw_If_Glob_Expression_Is_Null()
                {
                    // Given
                    var context = Substitute.For<ICakeContext>();

                    // When
                    var result = Record.Exception(() =>
                        FileAliases.DeleteFiles(context, (string)null));

                    // Then
                    AssertEx.IsArgumentNullException(result, "pattern");
                }

                [Fact]
                public void Should_Delete_Files()
                {
                    // Given
                    var fixture = new FileDeleteFixture();

                    // When
                    FileAliases.DeleteFiles(fixture.Context, "*");

                    // Then
                    fixture.Files[0].Received(1).Delete();
                    fixture.Files[1].Received(1).Delete();
                }
            }
        }

        public sealed class TheMoveFileToDirectory
        {
            [Fact]
            public void Should_Throw_If_Context_Is_Null()
            {
                // Given, When
                var source = new FilePath("./source.txt");
                var target = new DirectoryPath("./target");

                var result = Record.Exception(() =>
                    FileAliases.MoveFileToDirectory(null, source, target));

                // Then
                AssertEx.IsArgumentNullException(result, "context");
            }

            [Fact]
            public void Should_Throw_If_Source_File_Path_Is_Null()
            {
                // Given
                var context = Substitute.For<ICakeContext>();
                var target = new DirectoryPath("./target");

                // When
                var result = Record.Exception(() =>
                    FileAliases.MoveFileToDirectory(context, null, target));

                // Then
                AssertEx.IsArgumentNullException(result, "filePath");
            }

            [Fact]
            public void Should_Throw_If_Target_File_Path_Is_Null()
            {
                // Given
                var context = Substitute.For<ICakeContext>();
                var source = new FilePath("./source.txt");

                // When
                var result = Record.Exception(() =>
                    FileAliases.MoveFileToDirectory(context, source, null));

                // Then
                AssertEx.IsArgumentNullException(result, "targetDirectoryPath");
            }

            [Fact]
            public void Should_Move_File()
            {
                // Given
                var fixture = new FileCopyFixture();

                // When
                FileAliases.MoveFileToDirectory(fixture.Context, "./file1.txt", "./target");

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
                        FileAliases.MoveFiles(null, fixture.SourceFilePaths, "./target"));

                    // Then
                    AssertEx.IsArgumentNullException(result, "context");
                }

                [Fact]
                public void Should_Throw_If_File_Paths_Are_Null()
                {
                    // Given
                    var context = Substitute.For<ICakeContext>();

                    // When
                    var result = Record.Exception(() =>
                        FileAliases.MoveFiles(context, (IEnumerable<FilePath>)null, "./target"));

                    // Then
                    AssertEx.IsArgumentNullException(result, "filePaths");
                }

                [Fact]
                public void Should_Throw_If_Target_Directory_Path_Is_Null()
                {
                    // Given
                    var fixture = new FileCopyFixture();

                    // When
                    var result = Record.Exception(() =>
                        FileAliases.MoveFiles(fixture.Context, fixture.SourceFilePaths, null));

                    // Then
                    AssertEx.IsArgumentNullException(result, "targetDirectoryPath");
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
                        FileAliases.MoveFiles(fixture.Context, fixture.SourceFilePaths, "./target"));

                    // Then
                    Assert.IsType<DirectoryNotFoundException>(result);
                    Assert.Equal("The directory '/Working/target' does not exist.", result?.Message);
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
                        FileAliases.MoveFiles(fixture.Context, fixture.SourceFilePaths, "./target"));

                    // Then
                    Assert.IsType<FileNotFoundException>(result);
                    Assert.Equal("The file '/Working/file2.txt' does not exist.", result?.Message);
                }

                [Fact]
                public void Should_Move_Files()
                {
                    // Given
                    var fixture = new FileCopyFixture();

                    // When
                    FileAliases.MoveFiles(fixture.Context, fixture.SourceFilePaths, "./target");

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
                        FileAliases.MoveFiles(null, "", "./target"));

                    // Then
                    AssertEx.IsArgumentNullException(result, "context");
                }

                [Fact]
                public void Should_Throw_If_Glob_Expression_Is_Null()
                {
                    // Given
                    var fixture = new FileCopyFixture();

                    // When
                    var result = Record.Exception(() =>
                        FileAliases.MoveFiles(fixture.Context, (string)null, "./target"));

                    // Then
                    AssertEx.IsArgumentNullException(result, "pattern");
                }

                [Fact]
                public void Should_Throw_If_Target_Directory_Path_Is_Null()
                {
                    // Given
                    var fixture = new FileCopyFixture();

                    // When
                    var result = Record.Exception(() =>
                        FileAliases.MoveFiles(fixture.Context, "*", null));

                    // Then
                    AssertEx.IsArgumentNullException(result, "targetDirectoryPath");
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
                        FileAliases.MoveFiles(fixture.Context, "*", "./target"));

                    // Then
                    Assert.IsType<DirectoryNotFoundException>(result);
                    Assert.Equal("The directory '/Working/target' does not exist.", result?.Message);
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
                        FileAliases.MoveFiles(fixture.Context, "*", "./target"));

                    // Then
                    Assert.IsType<FileNotFoundException>(result);
                    Assert.Equal("The file '/Working/file2.txt' does not exist.", result?.Message);
                }

                [Fact]
                public void Should_Move_Files()
                {
                    // Given
                    var fixture = new FileCopyFixture();

                    // When
                    FileAliases.MoveFiles(fixture.Context, "*", "./target");

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
                    FileAliases.MoveFile(null, source, target));

                // Then
                AssertEx.IsArgumentNullException(result, "context");
            }

            [Fact]
            public void Should_Throw_If_Source_File_Path_Is_Null()
            {
                // Given
                var context = Substitute.For<ICakeContext>();
                var target = new FilePath("./target.txt");

                // When
                var result = Record.Exception(() =>
                    FileAliases.MoveFile(context, null, target));

                // Then
                AssertEx.IsArgumentNullException(result, "filePath");
            }

            [Fact]
            public void Should_Throw_If_Target_File_Path_Is_Null()
            {
                // Given
                var context = Substitute.For<ICakeContext>();
                var source = new FilePath("./source.txt");

                // When
                var result = Record.Exception(() =>
                    FileAliases.MoveFile(context, source, null));

                // Then
                AssertEx.IsArgumentNullException(result, "targetFilePath");
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
                    FileAliases.MoveFile(fixture.Context, "./file1.txt", "./target/file1.txt"));

                // Then
                Assert.IsType<DirectoryNotFoundException>(result);
                Assert.Equal("The directory '/Working/target' does not exist.", result?.Message);
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
                    FileAliases.MoveFile(fixture.Context, "./file1.txt", "./target/file1.txt"));

                // Then
                Assert.IsType<FileNotFoundException>(result);
                Assert.Equal("The file '/Working/file1.txt' does not exist.", result?.Message);
            }

            [Fact]
            public void Should_Move_File()
            {
                // Given
                var fixture = new FileCopyFixture();

                // When
                FileAliases.MoveFile(fixture.Context, "./file1.txt", "./target/file1.txt");

                // Then
                fixture.TargetFiles[0].Received(1).Move(
                    Arg.Is<FilePath>(p => p.FullPath == "/Working/target/file1.txt"));
            }
        }

        public sealed class TheFileExistsMethod
        {
            [Fact]
            public void Should_Throw_If_Context_Is_Null()
            {
                // Given, When
                var result = Record.Exception(() => FileAliases.FileExists(null, "some file"));

                // Then
                AssertEx.IsArgumentNullException(result, "context");
            }

            [Fact]
            public void Should_Throw_If_Path_Is_Null()
            {
                // Given
                var context = Substitute.For<ICakeContext>();

                // When
                var result = Record.Exception(() => FileAliases.FileExists(context, null));

                // Then
                AssertEx.IsArgumentNullException(result, "filePath");
            }

            [Fact]
            public void Should_Return_False_If_Directory_Does_Not_Exist()
            {
                // Given
                var context = Substitute.For<ICakeContext>();
                var environment = FakeEnvironment.CreateUnixEnvironment();
                var fileSystem = new FakeFileSystem(environment);
                context.FileSystem.Returns(fileSystem);
                context.Environment.Returns(environment);

                // When
                var result = FileAliases.FileExists(context, "non-existent-file.txt");

                // Then
                Assert.False(result);
            }

            [Fact]
            public void Should_Return_True_If_Relative_Path_Exist()
            {
                // Given
                var context = Substitute.For<ICakeContext>();
                var environment = FakeEnvironment.CreateUnixEnvironment();
                environment.WorkingDirectory = "/Working";
                var fileSystem = new FakeFileSystem(environment);
                fileSystem.CreateFile("/Working/some file.txt");
                context.FileSystem.Returns(fileSystem);
                context.Environment.Returns(environment);

                // When
                var result = FileAliases.FileExists(context, "some file.txt");

                // Then
                Assert.True(result);
            }

            [Fact]
            public void Should_Return_True_If_Absolute_Path_Exist()
            {
                // Given
                var context = Substitute.For<ICakeContext>();
                var environment = FakeEnvironment.CreateUnixEnvironment();
                environment.WorkingDirectory = "/Working/target";
                var fileSystem = new FakeFileSystem(environment);
                fileSystem.CreateFile("/Working/target/some file.txt");
                context.FileSystem.Returns(fileSystem);
                context.Environment.Returns(environment);

                // When
                var result = FileAliases.FileExists(context, "/Working/target/some file.txt");

                // Then
                Assert.True(result);
            }
        }

        public sealed class TheMakeAbsoluteMethod
        {
            [Fact]
            public void Should_Throw_If_Context_Is_Null()
            {
                // Given, When
                var result = Record.Exception(() => FileAliases.MakeAbsolute(null, "./build.txt"));

                // Then
                AssertEx.IsArgumentNullException(result, "context");
            }

            [Fact]
            public void Should_Throw_If_Path_Is_Null()
            {
                // Given
                var context = Substitute.For<ICakeContext>();

                // When
                var result = Record.Exception(() => FileAliases.MakeAbsolute(context, null));

                // Then
                AssertEx.IsArgumentNullException(result, "path");
            }

            [Fact]
            public void Should_Return_Absolute_Directory_Path()
            {
                // Given
                var context = Substitute.For<ICakeContext>();
                context.Environment.WorkingDirectory.Returns(d => "/Working");

                // When
                var result = FileAliases.MakeAbsolute(context, "./build.txt");

                // Then
                Assert.Equal("/Working/build.txt", result.FullPath);
            }
        }

        public sealed class TheFileSizeMethod
        {
            [Fact]
            public void Should_Throw_If_Context_Is_Null()
            {
                // Given, When
                var result = Record.Exception(() => FileAliases.FileSize(null, "some file"));

                // Then
                AssertEx.IsArgumentNullException(result, "context");
            }

            [Fact]
            public void Should_Throw_If_Path_Is_Null()
            {
                // Given
                var context = Substitute.For<ICakeContext>();

                // When
                var result = Record.Exception(() => FileAliases.FileSize(context, null));

                // Then
                AssertEx.IsArgumentNullException(result, "filePath");
            }

            [Fact]
            public void Should_Throw_If_Directory_Does_Not_Exist()
            {
                // Given
                var context = Substitute.For<ICakeContext>();
                var environment = FakeEnvironment.CreateUnixEnvironment();
                var fileSystem = new FakeFileSystem(environment);
                context.FileSystem.Returns(fileSystem);
                context.Environment.Returns(environment);

                // When / Then
                Assert.Throws<FileNotFoundException>(() => FileAliases.FileSize(context, "non-existent-file.txt"));
            }

            [Theory]
            [InlineData("/Working", "/Working/some file.txt")]
            [InlineData("/Working/target", "/Working/target/some file.txt")]
            public void Should_Return_Size_If_Path_Exist(string workingDirectory, string filePath)
            {
                // Given
                var context = Substitute.For<ICakeContext>();
                var environment = FakeEnvironment.CreateUnixEnvironment();
                environment.WorkingDirectory = workingDirectory;
                var fileSystem = new FakeFileSystem(environment);
                fileSystem.CreateFile(filePath, new byte[] { 1, 2, 3, 4 });
                context.FileSystem.Returns(fileSystem);
                context.Environment.Returns(environment);

                // When
                var result = FileAliases.FileSize(context, filePath);

                // Then
                Assert.Equal(result, 4);
            }
        }
    }
}