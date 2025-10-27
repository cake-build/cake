// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core;
using Cake.Core.IO;
using Cake.Testing.Tests.Fixtures;

namespace Cake.Testing.Tests.Unit;

public partial class FakeFileSystemTests
{
    public class FakeFile
    {
        /// <summary>
        /// Creates a file state object for verification purposes.
        /// </summary>
        /// <param name="fileSystem">The file system to get the file from.</param>
        /// <param name="filePath">The path of the file to extract state from.</param>
        /// <returns>An anonymous object containing the file's key properties.</returns>
        private static object CreateFileState(IFileSystem fileSystem, FilePath filePath)
        {
            var file = fileSystem.GetFile(filePath);
            return new
            {
                file.Path,
                file.Exists,
                file.Hidden,
                file.Length,
                file.LastWriteTimeUtc,
                file.CreationTimeUtc,
                file.LastAccessTimeUtc,
                file.Attributes,
                file.UnixFileMode
            };
        }
        [Theory]
        [InlineData(PlatformFamily.Windows)]
        [InlineData(PlatformFamily.Linux)]
        [InlineData(PlatformFamily.OSX)]
        [InlineData(PlatformFamily.FreeBSD)]
        public async Task Time_Local_Setters_Should_Work_Correctly(PlatformFamily platformFamily)
        {
            // Given
            var fixture = FakeFileSystemFixture.Create(platformFamily);
            var filePath = new FilePath("/test/file1.txt");
            var file = fixture.FileSystem.GetFile(filePath);

            // When
            file.SetCreationTime(fixture.TestCreationDateTimeUtc);
            file.SetLastAccessTime(fixture.TestLastAccessDateTimeUtc);
            file.SetLastWriteTime(fixture.TestLastWriteDateTimeUtc);

            // Then
            await Verify(file);
        }

        [Theory]
        [InlineData(PlatformFamily.Windows)]
        [InlineData(PlatformFamily.Linux)]
        [InlineData(PlatformFamily.OSX)]
        [InlineData(PlatformFamily.FreeBSD)]
        public async Task Time_Utc_Setters_Should_Work_Correctly(PlatformFamily platformFamily)
        {
            // Given
            var fixture = FakeFileSystemFixture.Create(platformFamily);
            var filePath = new FilePath("/test/file1.txt");
            var file = fixture.FileSystem.GetFile(filePath);

            // When
            file.SetCreationTimeUtc(fixture.TestCreationDateTimeUtc);
            file.SetLastAccessTimeUtc(fixture.TestLastAccessDateTimeUtc);
            file.SetLastWriteTimeUtc(fixture.TestLastWriteDateTimeUtc);

            // Then
            await Verify(file);
        }

        [Theory]
        [InlineData(PlatformFamily.Windows)]
        [InlineData(PlatformFamily.Linux)]
        [InlineData(PlatformFamily.OSX)]
        [InlineData(PlatformFamily.FreeBSD)]
        public async Task Set_UnixFileMode_Should_Work_Correctly(PlatformFamily platformFamily)
        {
            // Given
            var fixture = FakeFileSystemFixture.Create(platformFamily);
            var filePath = new FilePath("/test/file1.txt");
            var file = fixture.FileSystem.GetFile(filePath);
            const UnixFileMode unixFileMode = UnixFileMode.UserRead | UnixFileMode.GroupRead | UnixFileMode.OtherRead;

            // When
            var result = Record.Exception(() => file.SetUnixFileMode(unixFileMode));

            // Then
            await Verify(
                new
                {
                    file,
                    result
                });
        }

        [Theory]
        [InlineData(PlatformFamily.Windows)]
        [InlineData(PlatformFamily.Linux)]
        [InlineData(PlatformFamily.OSX)]
        [InlineData(PlatformFamily.FreeBSD)]
        public async Task Should_Initialize_Time_Properties_When_Created(PlatformFamily platformFamily)
        {
            // Given
            var fixture = FakeFileSystemFixture.Create(platformFamily);
            var filePath = new FilePath("/test/file2.txt");
            var file = fixture.FileSystem.GetFile(filePath);

            // When
            file.SetContent(string.Empty);

            // Then
            await Verify(file);
        }

        [Theory]
        [InlineData(PlatformFamily.Windows)]
        [InlineData(PlatformFamily.Linux)]
        [InlineData(PlatformFamily.OSX)]
        [InlineData(PlatformFamily.FreeBSD)]
        public async Task Copy_Should_Create_New_File_When_Destination_Does_Not_Exist(PlatformFamily platformFamily)
        {
            // Given
            var fixture = FakeFileSystemFixture.Create(platformFamily);
            var sourcePath = new FilePath("/test/file1.txt");
            var destinationPath = new FilePath("/test/copied_file.txt");
            var sourceFile = fixture.FileSystem.GetFile(sourcePath);

            // Capture destination file state before copy
            var beforeState = CreateFileState(fixture.FileSystem, destinationPath);

            // When
            sourceFile.Copy(destinationPath, false);

            // Then
            await Verify(
                new
                {
                    sourceFile,
                    beforeState,
                    afterState = CreateFileState(fixture.FileSystem, destinationPath)
                });
        }

        [Theory]
        [InlineData(PlatformFamily.Linux, UnixFileMode.UserRead | UnixFileMode.GroupRead | UnixFileMode.OtherRead)]
        [InlineData(PlatformFamily.OSX, UnixFileMode.UserRead | UnixFileMode.GroupRead | UnixFileMode.OtherRead)]
        [InlineData(PlatformFamily.FreeBSD, UnixFileMode.UserRead | UnixFileMode.GroupRead | UnixFileMode.OtherRead)]
        public async Task Copy_Should_Create_New_File_With_Same_UnixFileMode(PlatformFamily platformFamily, UnixFileMode unixFileMode)
        {
            // Given
            var fixture = FakeFileSystemFixture.Create(platformFamily);
            var sourcePath = new FilePath("/test/file1.txt");
            var destinationPath = new FilePath("/test/copied_file.txt");
            var sourceFile = fixture.FileSystem.GetFile(sourcePath);
            sourceFile.SetUnixFileMode(unixFileMode);

            // Capture destination file state before copy
            var beforeState = CreateFileState(fixture.FileSystem, destinationPath);

            // When
            sourceFile.Copy(destinationPath, false);

            // Then
            await Verify(
                new
                {
                    sourceFile,
                    beforeState,
                    afterState = CreateFileState(fixture.FileSystem, destinationPath)
                });
        }

        [Theory]
        [InlineData(PlatformFamily.Windows)]
        [InlineData(PlatformFamily.Linux)]
        [InlineData(PlatformFamily.OSX)]
        [InlineData(PlatformFamily.FreeBSD)]
        public async Task Copy_Should_Overwrite_Existing_File_When_Overwrite_Is_True(PlatformFamily platformFamily)
        {
            // Given
            var fixture = FakeFileSystemFixture.Create(platformFamily);
            var sourcePath = new FilePath("/test/file1.txt");
            var destinationPath = new FilePath("/test/build/build.cake");
            var sourceFile = fixture.FileSystem.GetFile(sourcePath);
            sourceFile.SetContent("New content");

            // Capture destination file state before copy
            var beforeState = CreateFileState(fixture.FileSystem, destinationPath);

            // When
            sourceFile.Copy(destinationPath, true);

            // Then
            await Verify(
                new
                {
                    sourceFile,
                    beforeState,
                    afterState = CreateFileState(fixture.FileSystem, destinationPath)
                });
        }

        [Theory]
        [InlineData(PlatformFamily.Windows)]
        [InlineData(PlatformFamily.Linux)]
        [InlineData(PlatformFamily.OSX)]
        [InlineData(PlatformFamily.FreeBSD)]
        public async Task Copy_Should_Throw_Exception_When_Destination_Exists_And_Overwrite_Is_False(PlatformFamily platformFamily)
        {
            // Given
            var fixture = FakeFileSystemFixture.Create(platformFamily);
            var sourcePath = new FilePath("/test/file1.txt");
            var destinationPath = new FilePath("/test/build/build.cake");
            var sourceFile = fixture.FileSystem.GetFile(sourcePath);

            // When
            var result = Record.Exception(() => sourceFile.Copy(destinationPath, false));

            // Then
            await Verify(
                new
                {
                    sourceFile,
                    result
                });
        }

        [Theory]
        [InlineData(PlatformFamily.Windows)]
        [InlineData(PlatformFamily.Linux)]
        [InlineData(PlatformFamily.OSX)]
        [InlineData(PlatformFamily.FreeBSD)]
        public async Task Move_Should_Move_File_To_New_Location(PlatformFamily platformFamily)
        {
            // Given
            var fixture = FakeFileSystemFixture.Create(platformFamily);
            var sourcePath = new FilePath("/test/file1.txt");
            var destinationPath = new FilePath("/test/moved_file.txt");
            var sourceFile = fixture.FileSystem.GetFile(sourcePath);

            // Capture source and destination file states before move
            var beforeState = new
            {
                Source = CreateFileState(fixture.FileSystem, sourcePath),
                Destination = CreateFileState(fixture.FileSystem, destinationPath)
            };

            // When
            sourceFile.Move(destinationPath);

            // Then
            await Verify(
                new
                {
                    beforeState,
                    afterState = new
                    {
                        Source = CreateFileState(fixture.FileSystem, sourcePath),
                        Destination = CreateFileState(fixture.FileSystem, destinationPath)
                    }
                });
        }

        [Theory]
        [InlineData(PlatformFamily.Windows)]
        [InlineData(PlatformFamily.Linux)]
        [InlineData(PlatformFamily.OSX)]
        [InlineData(PlatformFamily.FreeBSD)]
        public async Task Move_Should_Throw_Exception_When_Destination_Already_Exists(PlatformFamily platformFamily)
        {
            // Given
            var fixture = FakeFileSystemFixture.Create(platformFamily);
            var sourcePath = new FilePath("/test/file1.txt");
            var destinationPath = new FilePath("/test/build/build.cake");
            var sourceFile = fixture.FileSystem.GetFile(sourcePath);

            // When
            var result = Record.Exception(() => sourceFile.Move(destinationPath));

            // Then
            await Verify(
                new
                {
                    sourceFile,
                    result
                });
        }

        [Theory]
        [InlineData(PlatformFamily.Windows)]
        [InlineData(PlatformFamily.Linux)]
        [InlineData(PlatformFamily.OSX)]
        [InlineData(PlatformFamily.FreeBSD)]
        public async Task Delete_Should_Remove_File_From_FileSystem(PlatformFamily platformFamily)
        {
            // Given
            var fixture = FakeFileSystemFixture.Create(platformFamily);
            var filePath = new FilePath("/test/file1.txt");
            var file = fixture.FileSystem.GetFile(filePath);
            var beforeState = CreateFileState(fixture.FileSystem, filePath);

            // When
            file.Delete();

            // Then
            await Verify(
                new
                {
                    beforeState,
                    afterState = CreateFileState(fixture.FileSystem, filePath)
                });
        }

        [Theory]
        [InlineData(PlatformFamily.Windows)]
        [InlineData(PlatformFamily.Linux)]
        [InlineData(PlatformFamily.OSX)]
        [InlineData(PlatformFamily.FreeBSD)]
        public async Task Delete_Should_Throw_Exception_When_File_Does_Not_Exist(PlatformFamily platformFamily)
        {
            // Given
            var fixture = FakeFileSystemFixture.Create(platformFamily);
            var filePath = new FilePath("/test/nonexistent.txt");
            var file = fixture.FileSystem.GetFile(filePath);

            // When
            var result = Record.Exception(() => file.Delete());

            // Then
            await Verify(
                new
                {
                    file,
                    result
                });
        }
    }
}
