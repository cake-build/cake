// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Cake.Core.IO;
using NSubstitute;
using Xunit;

namespace Cake.Core.Tests.Unit.IO
{
    public sealed class FileSystemExtensionsTest
    {
        public sealed class TheExistMethod
        {
            public sealed class WithFilePath
            {
                [Fact]
                public void Should_Return_False_If_File_System_Returned_Null()
                {
                    // Given
                    var fileSystem = Substitute.For<IFileSystem>();
                    fileSystem.GetFile(Arg.Any<FilePath>()).Returns((IFile)null);

                    // When
                    var result = fileSystem.Exist((FilePath)"file.txt");

                    // Then
                    Assert.False(result);
                }

                [Fact]
                public void Should_Return_False_If_File_Do_Not_Exist()
                {
                    // Given
                    var fileSystem = Substitute.For<IFileSystem>();
                    var file = Substitute.For<IFile>();
                    file.Exists.Returns(false);
                    fileSystem.GetFile(Arg.Any<FilePath>()).Returns(file);

                    // When
                    var result = fileSystem.Exist((FilePath)"file.txt");

                    // Then
                    Assert.False(result);
                }

                [Fact]
                public void Should_Return_True_If_File_Exist()
                {
                    // Given
                    var fileSystem = Substitute.For<IFileSystem>();
                    var file = Substitute.For<IFile>();
                    file.Exists.Returns(true);
                    fileSystem.GetFile(Arg.Any<FilePath>()).Returns(file);

                    // When
                    var result = fileSystem.Exist((FilePath)"file.txt");

                    // Then
                    Assert.True(result);
                }
            }

            public sealed class WithDirectoryPath
            {
                [Fact]
                public void Should_Return_False_If_File_System_Returned_Null()
                {
                    // Given
                    var fileSystem = Substitute.For<IFileSystem>();
                    fileSystem.GetDirectory(Arg.Any<DirectoryPath>()).Returns((IDirectory)null);

                    // When
                    var result = fileSystem.Exist((DirectoryPath)"/Target");

                    // Then
                    Assert.False(result);
                }

                [Fact]
                public void Should_Return_False_If_Directory_Do_Not_Exist()
                {
                    // Given
                    var fileSystem = Substitute.For<IFileSystem>();
                    var directory = Substitute.For<IDirectory>();
                    directory.Exists.Returns(false);
                    fileSystem.GetDirectory(Arg.Any<DirectoryPath>()).Returns(directory);

                    // When
                    var result = fileSystem.Exist((DirectoryPath)"/Target");

                    // Then
                    Assert.False(result);
                }

                [Fact]
                public void Should_Return_True_If_Directory_Exist()
                {
                    // Given
                    var fileSystem = Substitute.For<IFileSystem>();
                    var directory = Substitute.For<IDirectory>();
                    directory.Exists.Returns(true);
                    fileSystem.GetDirectory(Arg.Any<DirectoryPath>()).Returns(directory);

                    // When
                    var result = fileSystem.Exist((DirectoryPath)"/Target");

                    // Then
                    Assert.True(result);
                }
            }
        }
    }
}
