// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core.IO;
using Cake.Testing;
using Xunit;

namespace Cake.Core.Tests.Unit.IO;

public sealed class FileSystemTests
{
    public sealed class TheGetFileSystemInfoMethod
    {
        public sealed class WithPath
        {
            [Fact]
            public void Should_Throw_If_Path_Is_Null()
            {
                // Given
                IFileSystem fileSystem = new FakeFileSystem(FakeEnvironment.CreateUnixEnvironment());

                // When
                var result = Record.Exception(() => fileSystem.GetFileSystemInfo((Path)null));

                // Then
                AssertEx.IsArgumentNullException(result, "path");
            }

            [Fact]
            public void Should_Return_File_When_Path_Is_A_File()
            {
                // Given
                var fakeFileSystem = new FakeFileSystem(FakeEnvironment.CreateUnixEnvironment());
                IFileSystem fileSystem = fakeFileSystem;
                var expected = fakeFileSystem.CreateFile("/test/file.txt");

                // When
                var result = fileSystem.GetFileSystemInfo(new FilePath("/test/file.txt"));

                // Then
                Assert.Same(expected, result);
            }

            [Fact]
            public void Should_Return_Directory_When_Path_Is_A_Directory()
            {
                // Given
                var fakeFileSystem = new FakeFileSystem(FakeEnvironment.CreateUnixEnvironment());
                IFileSystem fileSystem = fakeFileSystem;
                var expected = fakeFileSystem.CreateDirectory("/test");

                // When
                var result = fileSystem.GetFileSystemInfo(new DirectoryPath("/test"));

                // Then
                Assert.Same(expected, result);
            }

            [Fact]
            public void Should_Return_Nonexistent_File_When_Path_Does_Not_Exist()
            {
                // Given
                IFileSystem fileSystem = new FakeFileSystem(FakeEnvironment.CreateUnixEnvironment());

                // When
                var result = fileSystem.GetFileSystemInfo(new FilePath("/test/missing.txt"));

                // Then
                Assert.IsAssignableFrom<IFile>(result);
                Assert.False(result.Exists);
            }
        }

        public sealed class WithString
        {
            [Fact]
            public void Should_Throw_If_Path_Is_Null()
            {
                // Given
                IFileSystem fileSystem = new FakeFileSystem(FakeEnvironment.CreateUnixEnvironment());

                // When
                var result = Record.Exception(() => fileSystem.GetFileSystemInfo((string)null));

                // Then
                AssertEx.IsArgumentNullException(result, "path");
            }

            [Fact]
            public void Should_Return_File_When_Path_Is_A_File()
            {
                // Given
                var fakeFileSystem = new FakeFileSystem(FakeEnvironment.CreateUnixEnvironment());
                IFileSystem fileSystem = fakeFileSystem;
                var expected = fakeFileSystem.CreateFile("/test/file.txt");

                // When
                var result = fileSystem.GetFileSystemInfo("/test/file.txt");

                // Then
                Assert.Same(expected, result);
            }

            [Fact]
            public void Should_Return_Directory_When_Path_Is_A_Directory()
            {
                // Given
                var fakeFileSystem = new FakeFileSystem(FakeEnvironment.CreateUnixEnvironment());
                IFileSystem fileSystem = fakeFileSystem;
                var expected = fakeFileSystem.CreateDirectory("/test");

                // When
                var result = fileSystem.GetFileSystemInfo("/test");

                // Then
                Assert.Same(expected, result);
            }

            [Fact]
            public void Should_Return_Nonexistent_File_When_Path_Does_Not_Exist()
            {
                // Given
                IFileSystem fileSystem = new FakeFileSystem(FakeEnvironment.CreateUnixEnvironment());

                // When
                var result = fileSystem.GetFileSystemInfo("/test/missing.txt");

                // Then
                Assert.IsAssignableFrom<IFile>(result);
                Assert.False(result.Exists);
            }
        }
    }
}
