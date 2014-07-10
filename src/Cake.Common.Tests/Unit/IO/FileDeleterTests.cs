using System;
using System.Collections.Generic;
using System.IO;
using Cake.Common.Tests.Fixtures;
using Cake.Core;
using Cake.Core.IO;
using NSubstitute;
using Xunit;

namespace Cake.Common.Tests.Unit.IO
{
    using Common.IO;

    public sealed class FileDeleterTests
    {
        public sealed class TheDeleteFileMethod
        {
            [Fact]
            public void Should_Throw_If_Context_Is_Null()
            {
                // Given
                var filePath = new FilePath("./file.txt");

                // When
                var result = Record.Exception(() =>
                    FileExtensions.DeleteFile(null, filePath));

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
                    FileExtensions.DeleteFile(context, null));

                // Then
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("filePath", ((ArgumentNullException)result).ParamName);
            }

            [Fact]
            public void Should_Throw_If_File_Do_Not_Exist()
            {
                // Given
                var fixture = new FileDeleteFixture();

                // When
                var result = Record.Exception(() =>
                    FileExtensions.DeleteFile(fixture.Context, "/file.txt"));

                // Then
                Assert.IsType<FileNotFoundException>(result);
                Assert.Equal("The file '/file.txt' do not exist.", result.Message);
            }

            [Fact]
            public void Should_Make_Relative_File_Path_Absolute()
            {
                // Given
                var fixture = new FileDeleteFixture();

                // When
                FileExtensions.DeleteFile(fixture.Context, "file1.txt");

                // Then
                fixture.FileSystem.Received(1).GetFile(Arg.Is<FilePath>(
                    p=> p.FullPath == "/Working/file1.txt"));
            }

            [Fact]
            public void Should_Delete_File()
            {
                // Given
                var fixture = new FileDeleteFixture();

                // When
                FileExtensions.DeleteFile(fixture.Context, fixture.Paths[0]);

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
                    var filePaths = new FilePath[] {};

                    // When
                    var result = Record.Exception(() =>
                        FileExtensions.DeleteFiles(null, filePaths));

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
                        FileExtensions.DeleteFiles(context, (IEnumerable<FilePath>)null));

                    // Then
                    Assert.IsType<ArgumentNullException>(result);
                    Assert.Equal("filePaths", ((ArgumentNullException) result).ParamName);
                }

                [Fact]
                public void Should_Delete_Files()
                {
                    // Given
                    var fixture = new FileDeleteFixture();

                    // When
                    FileExtensions.DeleteFiles(fixture.Context, fixture.Paths);

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
                        FileExtensions.DeleteFiles(null, "*"));

                    // Then
                    Assert.IsType<ArgumentNullException>(result);
                    Assert.Equal("context", ((ArgumentNullException)result).ParamName);
                }

                [Fact]
                public void Should_Throw_If_Glob_Expression_Is_Null()
                {
                    // Given
                    var context = Substitute.For<ICakeContext>();

                    // When
                    var result = Record.Exception(() =>
                        FileExtensions.DeleteFiles(context, (string)null));

                    // Then
                    Assert.IsType<ArgumentNullException>(result);
                    Assert.Equal("pattern", ((ArgumentNullException)result).ParamName);
                }

                [Fact]
                public void Should_Delete_Files()
                {
                    // Given
                    var fixture = new FileDeleteFixture();

                    // When
                    FileExtensions.DeleteFiles(fixture.Context, "*");

                    // Then
                    fixture.Files[0].Received(1).Delete();
                    fixture.Files[1].Received(1).Delete();
                }
            }
        }
    }
}
