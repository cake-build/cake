using Cake.Core;
using Cake.Core.IO;
using Cake.Testing.Tests.Fixtures;

namespace Cake.Testing.Tests.Unit;

public partial class FakeFileSystemTests
{
    public class FakeDirectory
    {
        /// <summary>
        /// Creates a directory state object for verification purposes.
        /// </summary>
        /// <param name="fileSystem">The file system to get the directory from.</param>
        /// <param name="directoryPath">The path of the directory to extract state from.</param>
        /// <returns>An anonymous object containing the directory's key properties.</returns>
        private static object CreateDirectoryState(IFileSystem fileSystem, DirectoryPath directoryPath)
        {
            var directory = fileSystem.GetDirectory(directoryPath);
            return new
            {
                directory.Path,
                directory.Exists,
                directory.Hidden,
                directory.LastWriteTimeUtc,
                directory.CreationTimeUtc,
                directory.LastAccessTimeUtc,
                directory.UnixFileMode
            };
        }
        [Theory]
        [InlineData(PlatformFamily.Windows)]
        [InlineData(PlatformFamily.Linux)]
        [InlineData(PlatformFamily.OSX)]
        [InlineData(PlatformFamily.FreeBSD)]
        public async Task FakeDirectory_Should_Initialize_Time_Properties_When_Created(PlatformFamily platformFamily)
        {
            // Given
            var fixture = FakeFileSystemFixture.Create(platformFamily);
            var directoryPath = new DirectoryPath("/test/directory");

            // When
            var directory = fixture.FileSystem.GetDirectory(directoryPath);
            directory.Create();

            // Then
            await Verify(directory);
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
            var directoryPath = new DirectoryPath("/test");
            var directory = fixture.FileSystem.GetDirectory(directoryPath);

            // When
            directory.SetCreationTimeUtc(fixture.TestCreationDateTimeUtc);
            directory.SetLastAccessTimeUtc(fixture.TestLastAccessDateTimeUtc);
            directory.SetLastWriteTimeUtc(fixture.TestLastWriteDateTimeUtc);

            // Then
            await Verify(directory);
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
            var directoryPath = new DirectoryPath("/test");
            var directory = fixture.FileSystem.GetDirectory(directoryPath);
            const UnixFileMode unixFileMode = UnixFileMode.UserRead | UnixFileMode.GroupRead | UnixFileMode.OtherRead;

            // When
            var result = Record.Exception(() => directory.SetUnixFileMode(unixFileMode));

            // Then
            await Verify(
                new
                {
                    directory,
                    result
                });
        }

        [Theory]
        [InlineData(PlatformFamily.Windows)]
        [InlineData(PlatformFamily.Linux)]
        [InlineData(PlatformFamily.OSX)]
        [InlineData(PlatformFamily.FreeBSD)]
        public async Task Create_Should_Create_New_Directory(PlatformFamily platformFamily)
        {
            // Given
            var fixture = FakeFileSystemFixture.Create(platformFamily);
            var directoryPath = new DirectoryPath("/test/new_directory");
            var directory = fixture.FileSystem.GetDirectory(directoryPath);

            // Capture directory state before create
            var beforeState = CreateDirectoryState(fixture.FileSystem, directoryPath);

            // When
            directory.Create();

            // Then
            await Verify(
                new
                {
                    directory,
                    beforeState,
                    afterState = CreateDirectoryState(fixture.FileSystem, directoryPath)
                });
        }

        [Theory]
        [InlineData(PlatformFamily.Windows)]
        [InlineData(PlatformFamily.Linux)]
        [InlineData(PlatformFamily.OSX)]
        [InlineData(PlatformFamily.FreeBSD)]
        public async Task Move_Should_Move_Directory_To_New_Location(PlatformFamily platformFamily)
        {
            // Given
            var fixture = FakeFileSystemFixture.Create(platformFamily);
            var sourcePath = new DirectoryPath("/test/build");
            var destinationPath = new DirectoryPath("/test/moved_build");
            var sourceDirectory = fixture.FileSystem.GetDirectory(sourcePath);

            // Capture source and destination directory states before move
            var beforeState = new
            {
                Source = CreateDirectoryState(fixture.FileSystem, sourcePath),
                Destination = CreateDirectoryState(fixture.FileSystem, destinationPath)
            };

            // When
            sourceDirectory.Move(destinationPath);

            // Then
            await Verify(
                new
                {
                    beforeState,
                    afterState = new
                    {
                        Source = CreateDirectoryState(fixture.FileSystem, sourcePath),
                        Destination = CreateDirectoryState(fixture.FileSystem, destinationPath)
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
            var sourcePath = new DirectoryPath("/test/build");
            var destinationPath = new DirectoryPath("/test"); // This exists in fixture
            var sourceDirectory = fixture.FileSystem.GetDirectory(sourcePath);

            // When
            var result = Record.Exception(() => sourceDirectory.Move(destinationPath));

            // Then
            await Verify(
                new
                {
                    sourceDirectory,
                    result
                });
        }

        [Theory]
        [InlineData(PlatformFamily.Windows)]
        [InlineData(PlatformFamily.Linux)]
        [InlineData(PlatformFamily.OSX)]
        [InlineData(PlatformFamily.FreeBSD)]
        public async Task Delete_Should_Remove_Empty_Directory_When_Recursive_Is_False(PlatformFamily platformFamily)
        {
            // Given
            var fixture = FakeFileSystemFixture.Create(platformFamily);
            var directoryPath = new DirectoryPath("/test/empty_dir");
            var directory = fixture.FileSystem.GetDirectory(directoryPath);
            directory.Create(); // Create an empty directory

            // Capture directory state before delete
            var beforeState = CreateDirectoryState(fixture.FileSystem, directoryPath);

            // When
            directory.Delete(false);

            // Then
            await Verify(
                new
                {
                    directory,
                    beforeState,
                    afterState = CreateDirectoryState(fixture.FileSystem, directoryPath)
                });
        }

        [Theory]
        [InlineData(PlatformFamily.Windows)]
        [InlineData(PlatformFamily.Linux)]
        [InlineData(PlatformFamily.OSX)]
        [InlineData(PlatformFamily.FreeBSD)]
        public async Task Delete_Should_Remove_Directory_With_Contents_When_Recursive_Is_True(PlatformFamily platformFamily)
        {
            // Given
            var fixture = FakeFileSystemFixture.Create(platformFamily);
            var directoryPath = new DirectoryPath("/test/build");
            var directory = fixture.FileSystem.GetDirectory(directoryPath);

            // Capture directory state before delete
            var beforeState = CreateDirectoryState(fixture.FileSystem, directoryPath);

            // When
            directory.Delete(true);

            // Then
            await Verify(
                new
                {
                    directory,
                    beforeState,
                    afterState = CreateDirectoryState(fixture.FileSystem, directoryPath)
                });
        }

        [Theory]
        [InlineData(PlatformFamily.Windows)]
        [InlineData(PlatformFamily.Linux)]
        [InlineData(PlatformFamily.OSX)]
        [InlineData(PlatformFamily.FreeBSD)]
        public async Task GetDirectories_Should_Return_Directories_Matching_Filter_Current_Scope(PlatformFamily platformFamily)
        {
            // Given
            var fixture = FakeFileSystemFixture.Create(platformFamily);
            var directoryPath = new DirectoryPath("/test");
            var directory = fixture.FileSystem.GetDirectory(directoryPath);

            // When
            var result = directory.GetDirectories("*", SearchScope.Current).ToList();

            // Then
            await Verify(
                new
                {
                    directory,
                    result
                });
        }

        [Theory]
        [InlineData(PlatformFamily.Windows)]
        [InlineData(PlatformFamily.Linux)]
        [InlineData(PlatformFamily.OSX)]
        [InlineData(PlatformFamily.FreeBSD)]
        public async Task GetDirectories_Should_Return_Directories_Matching_Filter_Recursive_Scope(PlatformFamily platformFamily)
        {
            // Given
            var fixture = FakeFileSystemFixture.Create(platformFamily);
            var directoryPath = new DirectoryPath("/test");
            var directory = fixture.FileSystem.GetDirectory(directoryPath);

            // When
            var result = directory.GetDirectories("*", SearchScope.Recursive).ToList();

            // Then
            await Verify(
                new
                {
                    directory,
                    result
                });
        }

        [Theory]
        [InlineData(PlatformFamily.Windows)]
        [InlineData(PlatformFamily.Linux)]
        [InlineData(PlatformFamily.OSX)]
        [InlineData(PlatformFamily.FreeBSD)]
        public async Task GetDirectories_Should_Return_Empty_When_No_Directories_Match_Filter(PlatformFamily platformFamily)
        {
            // Given
            var fixture = FakeFileSystemFixture.Create(platformFamily);
            var directoryPath = new DirectoryPath("/test");
            var directory = fixture.FileSystem.GetDirectory(directoryPath);

            // When
            var result = directory.GetDirectories("nonexistent*", SearchScope.Current).ToList();

            // Then
            await Verify(
                new
                {
                    directory,
                    result
                });
        }

        [Theory]
        [InlineData(PlatformFamily.Windows)]
        [InlineData(PlatformFamily.Linux)]
        [InlineData(PlatformFamily.OSX)]
        [InlineData(PlatformFamily.FreeBSD)]
        public async Task GetFiles_Should_Return_Files_Matching_Filter_Current_Scope(PlatformFamily platformFamily)
        {
            // Given
            var fixture = FakeFileSystemFixture.Create(platformFamily);
            var directoryPath = new DirectoryPath("/test");
            var directory = fixture.FileSystem.GetDirectory(directoryPath);

            // When
            var result = directory.GetFiles("*", SearchScope.Current).ToList();

            // Then
            await Verify(
                new
                {
                    directory,
                    result
                });
        }

        [Theory]
        [InlineData(PlatformFamily.Windows)]
        [InlineData(PlatformFamily.Linux)]
        [InlineData(PlatformFamily.OSX)]
        [InlineData(PlatformFamily.FreeBSD)]
        public async Task GetFiles_Should_Return_Files_Matching_Filter_Recursive_Scope(PlatformFamily platformFamily)
        {
            // Given
            var fixture = FakeFileSystemFixture.Create(platformFamily);
            var directoryPath = new DirectoryPath("/test");
            var directory = fixture.FileSystem.GetDirectory(directoryPath);

            // When
            var result = directory.GetFiles("*", SearchScope.Recursive).ToList();

            // Then
            await Verify(
                new
                {
                    directory,
                    result
                });
        }

        [Theory]
        [InlineData(PlatformFamily.Windows)]
        [InlineData(PlatformFamily.Linux)]
        [InlineData(PlatformFamily.OSX)]
        [InlineData(PlatformFamily.FreeBSD)]
        public async Task GetFiles_Should_Return_Files_Matching_Specific_Extension(PlatformFamily platformFamily)
        {
            // Given
            var fixture = FakeFileSystemFixture.Create(platformFamily);
            var directoryPath = new DirectoryPath("/test");
            var directory = fixture.FileSystem.GetDirectory(directoryPath);

            // When
            var result = directory.GetFiles("*.cake", SearchScope.Recursive).ToList();

            // Then
            await Verify(
                new
                {
                    directory,
                    result
                });
        }

        [Theory]
        [InlineData(PlatformFamily.Windows)]
        [InlineData(PlatformFamily.Linux)]
        [InlineData(PlatformFamily.OSX)]
        [InlineData(PlatformFamily.FreeBSD)]
        public async Task GetFiles_Should_Return_Empty_When_No_Files_Match_Filter(PlatformFamily platformFamily)
        {
            // Given
            var fixture = FakeFileSystemFixture.Create(platformFamily);
            var directoryPath = new DirectoryPath("/test");
            var directory = fixture.FileSystem.GetDirectory(directoryPath);

            // When
            var result = directory.GetFiles("*.nonexistent", SearchScope.Recursive).ToList();

            // Then
            await Verify(
                new
                {
                    directory,
                    result
                });
        }
    }
}