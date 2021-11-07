using System.Collections.Generic;
using System.IO;
using System.Linq;
using Cake.Common.IO;
using Cake.Common.Tests.Fixtures.IO;
using Cake.Core.IO;
using Xunit;

namespace Cake.Common.Tests.Unit.IO
{
    public sealed class FileCopierTests
    {
        public sealed class FileCopierTestsMethod
        {
            [Fact]
            public void Should_Copy_Single_File_Relative_Path()
            {
                const string filePath = "./src/a/a.txt";
                const string dstPath = "./dst";

                // Given
                var fixture = new FileCopierFixture();
                fixture.EnsureFileExists(filePath);
                fixture.EnsureDirectoryExists(dstPath);

                // When
                FileCopier.CopyFiles(
                    fixture.Context,
                    new FilePath[]
                    {
                        filePath
                    },
                    new DirectoryPath(dstPath),
                    true);

                // Then
                Assert.True(fixture.ExistsFile($"{dstPath}/a/a.txt"));
            }

            [Fact]
            public void Should_Copy_Single_File_Absolute_Path()
            {
                const string filePath = "./src/a/a.txt";
                const string dstPath = "./dst";

                // Given
                var fixture = new FileCopierFixture();
                fixture.EnsureFileExists(filePath);
                fixture.EnsureDirectoryExists(dstPath);

                // When
                FileCopier.CopyFiles(
                    fixture.Context,
                    new[] { fixture.MakeAbsolute(filePath) },
                    new DirectoryPath(dstPath),
                    true);

                // Then
                Assert.True(fixture.ExistsFile($"{dstPath}/a/a.txt"));
            }

            [Fact]
            public void Should_Copy_Multiple_Files_Relative_Path()
            {
                const string filePath1 = "./src/a/a.txt";
                const string filePath2 = "./src/b/b.txt";
                const string dstPath = "./dst";

                // Given
                var fixture = new FileCopierFixture();
                fixture.EnsureFileExists(filePath1);
                fixture.EnsureFileExists(filePath2);
                fixture.EnsureDirectoryExists(dstPath);

                // When
                FileCopier.CopyFiles(
                    fixture.Context,
                    new FilePath[]
                    {
                        filePath1,
                        filePath2
                    },
                    new DirectoryPath(dstPath),
                    true);

                // Then
                Assert.True(fixture.ExistsFile($"{dstPath}/a/a.txt"));
                Assert.True(fixture.ExistsFile($"{dstPath}/b/b.txt"));
            }

            [Fact]
            public void Should_Copy_Multiple_Files_Absolute_Path()
            {
                const string filePath1 = "./src/a/a.txt";
                const string filePath2 = "./src/b/b.txt";
                const string dstPath = "./dst";

                // Given
                var fixture = new FileCopierFixture();
                fixture.EnsureFileExists(filePath1);
                fixture.EnsureFileExists(filePath2);
                fixture.EnsureDirectoryExists(dstPath);

                // When
                FileCopier.CopyFiles(
                    fixture.Context,
                    new[]
                        {
                            fixture.MakeAbsolute(filePath1),
                            fixture.MakeAbsolute(filePath2)
                        },
                    new DirectoryPath(dstPath),
                    true);

                // Then
                Assert.True(fixture.ExistsFile($"{dstPath}/a/a.txt"));
                Assert.True(fixture.ExistsFile($"{dstPath}/b/b.txt"));
            }

            [Fact]
            public void Should_Copy_Multiple_Files_Mixed_Path()
            {
                const string filePath1 = "./src/a/a.txt";
                const string filePath2 = "./src/b/b.txt";
                const string dstPath = "./dst";

                // Given
                var fixture = new FileCopierFixture();
                fixture.EnsureFileExists(filePath1);
                fixture.EnsureFileExists(filePath2);
                fixture.EnsureDirectoryExists(dstPath);

                // When
                FileCopier.CopyFiles(
                    fixture.Context,
                    new[]
                    {
                        filePath1,
                        fixture.MakeAbsolute(filePath2)
                    },
                    new DirectoryPath(dstPath),
                    true);

                // Then
                Assert.True(fixture.ExistsFile($"{dstPath}/a/a.txt"));
                Assert.True(fixture.ExistsFile($"{dstPath}/b/b.txt"));
            }

            [Fact]
            public void Should_Throw_If_Context_Is_Null()
            {
                // When
                var result = Record.Exception(() => FileCopier.CopyFiles(null, Enumerable.Empty<FilePath>(), new DirectoryPath(""), true));

                // Then
                AssertEx.IsArgumentNullException(result, "context");
            }

            [Fact]
            public void Should_Throw_If_File_Paths_Is_Null()
            {
                // Given
                var fixture = new FileCopierFixture();

                // When
                var result = Record.Exception(() => FileCopier.CopyFiles(fixture.Context, (List<FilePath>)null, new DirectoryPath(""), true));

                // Then
                AssertEx.IsArgumentNullException(result, "filePaths");
            }

            [Fact]
            public void Should_Throw_If_Target_Directory_Path_Is_Null()
            {
                // Given
                var fixture = new FileCopierFixture();

                // When
                var result = Record.Exception(() => FileCopier.CopyFiles(fixture.Context, Enumerable.Empty<FilePath>(), null, true));

                // Then
                AssertEx.IsArgumentNullException(result, "targetDirectoryPath");
            }

            [Fact]
            public void Should_Throw_If_Target_Directory_Does_Not_Exist()
            {
                const string dstPath = "/dst";

                // Given
                var fixture = new FileCopierFixture();

                // When
                var result = Record.Exception(() => FileCopier.CopyFiles(fixture.Context, Enumerable.Empty<FilePath>(), new DirectoryPath(dstPath), true));

                // Then
                AssertEx.IsExceptionWithMessage<DirectoryNotFoundException>(result, $"The directory '{dstPath}' does not exist.");
            }
        }
    }
}
