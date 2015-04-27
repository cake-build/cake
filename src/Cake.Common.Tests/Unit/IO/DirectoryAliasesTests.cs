using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Cake.Common.IO;
using Cake.Common.IO.Paths;
using Cake.Common.Tests.Fixtures;
using Cake.Core;
using Cake.Core.IO;
using Cake.Testing.Fakes;
using NSubstitute;
using Xunit;

namespace Cake.Common.Tests.Unit.IO
{
    public sealed class DirectoryAliasesTests
    {
        public sealed class TheDirectoryMethod
        {
            [Fact]
            public void Should_Throw_If_Context_Is_Null()
            {
                // Given
                const string path = "./temp";

                // When
                var result = Record.Exception(() => DirectoryAliases.Directory(null, path));

                // Then
                Assert.IsArgumentNullException(result, "context");
            }

            [Fact]
            public void Should_Throw_If_Path_Is_Null()
            {
                // Given
                var context = Substitute.For<ICakeContext>();

                // When
                var result = Record.Exception(() => DirectoryAliases.Directory(context, null));

                // Then
                Assert.IsArgumentNullException(result, "path");
            }

            [Fact]
            public void Should_Return_A_Convertable_Directory_Path()
            {
                // Given
                var context = Substitute.For<ICakeContext>();

                // When
                var result = DirectoryAliases.Directory(context, "./temp");

                // Then                
                Assert.IsType<ConvertableDirectoryPath>(result);
            }
        }


        public sealed class TheCleanMethod
        {
            [Fact]
            public void Should_Throw_If_Context_Is_Null()
            {
                // Given, When
                var result = Record.Exception(() =>
                    DirectoryAliases.CleanDirectory(null, "/Temp/Hello"));

                // Then
                Assert.IsArgumentNullException(result, "context");
            }

            [Fact]
            public void Should_Throw_If_Directory_Are_Null()
            {
                // Given
                var context = Substitute.For<ICakeContext>();
                context.FileSystem.Returns(Substitute.For<IFileSystem>());

                // When
                var result = Record.Exception(() =>
                    context.CleanDirectory(null));

                // Then
                Assert.IsArgumentNullException(result, "path");
            }

            [Fact]
            public void Should_Delete_Files_In_Provided_Directory()
            {
                // Given
                var directory = new DirectoryPath("/Temp/Hello");
                var fixture = new FileSystemFixture();
                var context = Substitute.For<ICakeContext>();
                context.FileSystem.Returns(fixture.FileSystem);

                // When
                context.CleanDirectory(directory);

                // Then
                Assert.Empty(fixture.FileSystem.GetDirectory(directory).GetFiles("*", SearchScope.Recursive));
            }

            [Fact]
            public void Should_Delete_Files_Respecting_Predicate_In_Provided_Directory()
            {
                // Given
                var directory = new DirectoryPath("/Temp/Hello");
                var fixture = new FileSystemFixture();
                var context = Substitute.For<ICakeContext>();
                Func<IFileSystemInfo, bool> wherePredicate = entry=>!entry.Hidden;
                context.FileSystem.Returns(fixture.FileSystem);
                var filesNotMatchingPredicate = fixture
                    .FileSystem
                    .GetDirectory(directory)
                    .GetFiles("*", SearchScope.Recursive)
                    .Where(entry=>!wherePredicate(entry))
                    .ToArray();

                // When
                context.CleanDirectory(directory, wherePredicate);

                // Then
                Assert.Empty(fixture.FileSystem.GetDirectory(directory).GetFiles("*", SearchScope.Recursive).Where(wherePredicate));
                Assert.Equal(filesNotMatchingPredicate, fixture.FileSystem.GetDirectory(directory).GetFiles("*", SearchScope.Recursive));
            }

            [Fact]
            public void Should_Delete_Directories_In_Provided_Directory()
            {
                // Given
                var directory = new DirectoryPath("/Temp/Hello");
                var fixture = new FileSystemFixture();
                var context = Substitute.For<ICakeContext>();
                context.FileSystem.Returns(fixture.FileSystem);

                // When
                context.CleanDirectory(directory);

                // Then
                Assert.Empty(fixture.FileSystem.GetDirectory(directory).GetDirectories("*", SearchScope.Recursive));
            }

            [Fact]
            public void Should_Delete_Directories_Respecting_Predicate_In_Provided_Directory()
            {
                // Given
                var directory = new DirectoryPath("/Temp/Hello");
                var fixture = new FileSystemFixture();
                var context = Substitute.For<ICakeContext>();
                context.FileSystem.Returns(fixture.FileSystem);
                Func<IFileSystemInfo, bool> wherePredicate = entry=>!entry.Hidden;
                var directoriesNotMatchingPredicate = fixture.FileSystem
                    .GetDirectory(directory)
                    .GetDirectories("*", SearchScope.Recursive)
                    .Where(entry => !wherePredicate(entry))
                    .ToArray();

                // When
                context.CleanDirectory(directory, wherePredicate);

                // Then
                Assert.Empty(fixture.FileSystem.GetDirectory(directory).GetDirectories("*", SearchScope.Recursive).Where(wherePredicate));
                Assert.Equal(directoriesNotMatchingPredicate, fixture.FileSystem.GetDirectory(directory).GetDirectories("*", SearchScope.Recursive));
            }

            [Fact]
            public void Should_Leave_Provided_Directory_Itself_Intact()
            {
                // Given
                var directory = new DirectoryPath("/Temp/Hello");
                var fixture = new FileSystemFixture();
                var context = Substitute.For<ICakeContext>();
                context.FileSystem.Returns(fixture.FileSystem);

                // When
                context.CleanDirectory(directory);

                // Then
                Assert.True(fixture.FileSystem.GetDirectory(directory).Exists);
            }

            [Fact]
            public void Should_Create_Directory_If_Missing()
            {
                // Given
                var directory = new DirectoryPath("/NonExisting");
                var fixture = new FileSystemFixture();
                var context = Substitute.For<ICakeContext>();
                context.FileSystem.Returns(fixture.FileSystem);

                // When
                context.CleanDirectory(directory);

                // Then
                Assert.True(fixture.FileSystem.Exist((DirectoryPath)"/NonExisting"));
            }
        }

        public sealed class TheCleanDirectoriesMethod
        {
            public sealed class WithPaths
            {
                [Fact]
                public void Should_Throw_If_Context_Is_Null()
                {
                    // Given
                    var paths = new DirectoryPath[] { "/Temp/Hello", "/Temp/Goodbye" };

                    // When
                    var result = Record.Exception(() =>
                        DirectoryAliases.CleanDirectories(null, paths));

                    // Then
                    Assert.IsArgumentNullException(result, "context");
                }

                [Fact]
                public void Should_Throw_If_Directories_Are_Null()
                {
                    // Given
                    var context = Substitute.For<ICakeContext>();

                    // When
                    var result = Record.Exception(() =>
                        context.CleanDirectories((IEnumerable<DirectoryPath>)null));

                    // Then
                    Assert.IsArgumentNullException(result, "directories");
                }
                
                [Fact]
                public void Should_Delete_Files_In_Provided_Directories()
                {
                    // Given
                    var fixture = new FileSystemFixture();
                    var context = Substitute.For<ICakeContext>();
                    context.FileSystem.Returns(fixture.FileSystem);

                    var paths = new DirectoryPath[] {
                        "/Temp/Hello", "/Temp/Goodbye"
                    };

                    // When
                    Record.Exception(() => context.CleanDirectories(paths));

                    // Then
                    Assert.Empty(fixture.FileSystem.GetDirectory(paths[0]).GetFiles("*", SearchScope.Recursive));
                    Assert.Empty(fixture.FileSystem.GetDirectory(paths[1]).GetFiles("*", SearchScope.Recursive));
                }

                [Fact]
                public void Should_Leave_Provided_Directories_Intact()
                {
                    // Given
                    var fixture = new FileSystemFixture();
                    var context = Substitute.For<ICakeContext>();
                    context.FileSystem.Returns(fixture.FileSystem);

                    var paths = new DirectoryPath[] {
                        "/Temp/Hello", "/Temp/Goodbye"
                    };

                    // When
                    context.CleanDirectories(paths);

                    // Then
                    Assert.True(fixture.FileSystem.GetDirectory(paths[0]).Exists);
                    Assert.True(fixture.FileSystem.GetDirectory(paths[1]).Exists);
                }

                [Fact]
                public void Should_Create_Directories_If_Missing()
                {
                    // Given
                    var fixture = new FileSystemFixture();
                    var context = Substitute.For<ICakeContext>();
                    context.FileSystem.Returns(fixture.FileSystem);

                    var paths = new DirectoryPath[] {
                        "/Temp/Hello", "/NonExisting"
                    };

                    // When
                    context.CleanDirectories(paths);

                    // Then
                    Assert.True(fixture.FileSystem.Exist((DirectoryPath)"/NonExisting"));
                }
            }

            public sealed class WithStrings
            {
                [Fact]
                public void Should_Throw_If_Context_Is_Null()
                {
                    // Given
                    var paths = new[] { "/Temp/Hello", "/Temp/Goodbye" };

                    // When
                    var result = Record.Exception(() =>
                        DirectoryAliases.CleanDirectories(null, paths));

                    // Then
                    Assert.IsArgumentNullException(result, "context");
                }

                [Fact]
                public void Should_Throw_If_Directories_Are_Null()
                {
                    // Given
                    var context = Substitute.For<ICakeContext>();

                    // When
                    var result = Record.Exception(() =>
                        context.CleanDirectories((IEnumerable<string>)null));

                    // Then
                    Assert.IsArgumentNullException(result, "directories");
                }

                [Fact]
                public void Should_Delete_Files_In_Provided_Directories()
                {
                    // Given
                    var fixture = new FileSystemFixture();
                    var context = Substitute.For<ICakeContext>();
                    context.FileSystem.Returns(fixture.FileSystem);

                    var paths = new[] {
                        "/Temp/Hello", "/Temp/Goodbye"
                    };

                    // When
                    Record.Exception(() => context.CleanDirectories(paths));

                    // Then
                    Assert.Empty(fixture.FileSystem.GetDirectory(paths[0]).GetFiles("*", SearchScope.Recursive));
                    Assert.Empty(fixture.FileSystem.GetDirectory(paths[1]).GetFiles("*", SearchScope.Recursive));
                }

                [Fact]
                public void Should_Leave_Provided_Directories_Intact()
                {
                    // Given
                    var fixture = new FileSystemFixture();
                    var context = Substitute.For<ICakeContext>();
                    context.FileSystem.Returns(fixture.FileSystem);

                    var paths = new[] {
                        "/Temp/Hello", "/Temp/Goodbye"
                    };

                    // When
                    context.CleanDirectories(paths);

                    // Then
                    Assert.True(fixture.FileSystem.GetDirectory(paths[0]).Exists);
                    Assert.True(fixture.FileSystem.GetDirectory(paths[1]).Exists);
                }

                [Fact]
                public void Should_Create_Directories_If_Missing()
                {
                    // Given
                    var fixture = new FileSystemFixture();
                    var context = Substitute.For<ICakeContext>();
                    context.FileSystem.Returns(fixture.FileSystem);

                    var paths = new[] {
                        "/Temp/Hello", "/NonExisting"
                    };

                    // When
                    context.CleanDirectories(paths);

                    // Then
                    Assert.True(fixture.FileSystem.Exist((DirectoryPath)"/NonExisting"));
                }
            }
        }

        public sealed class TheCreateMethod
        {
            [Fact]
            public void Should_Throw_If_Context_Is_Null()
            {
                // Given
                var path = new DirectoryPath("/Temp");

                // When
                var result = Record.Exception(() =>
                    DirectoryAliases.CreateDirectory(null, path));

                // Then
                Assert.IsArgumentNullException(result, "context");
            }

            [Fact]
            public void Should_Throw_If_Directory_Is_Null()
            {
                // Given
                var context = Substitute.For<ICakeContext>();

                // When
                var result = Record.Exception(() =>
                    context.CreateDirectory(null));

                // Then
                Assert.IsArgumentNullException(result, "path");
            }

            [Fact]
            public void Should_Create_Non_Existing_Directory()
            {
                // Given
                var fileSystem = Substitute.For<IFileSystem>();
                var directory = Substitute.For<IDirectory>();
                directory.Exists.Returns(false);
                fileSystem.GetDirectory(Arg.Is<DirectoryPath>(p => p.FullPath == "/Temp")).Returns(directory);

                var context = Substitute.For<ICakeContext>();
                context.FileSystem.Returns(fileSystem);

                // When
                context.CreateDirectory("/Temp");

                // Then
                directory.Received(1).Create();
            }

            [Fact]
            public void Should_Not_Try_To_Create_Directory_If_It_Already_Exist()
            {
                // Given
                var fileSystem = Substitute.For<IFileSystem>();
                var directory = Substitute.For<IDirectory>();
                directory.Exists.Returns(true);
                fileSystem.GetDirectory(Arg.Is<DirectoryPath>(p => p.FullPath == "/Temp")).Returns(directory);

                var context = Substitute.For<ICakeContext>();
                context.FileSystem.Returns(fileSystem);

                // When
                context.CreateDirectory("/Temp");

                // Then
                directory.Received(0).Create();
            }

            [Fact]
            public void Should_Make_Relative_Path_Absolute()
            {
                // Given
                var fileSystem = Substitute.For<IFileSystem>();
                var context = Substitute.For<ICakeContext>();
                var environment = Substitute.For<ICakeEnvironment>();
                environment.WorkingDirectory.Returns("/Temp");

                context.FileSystem.Returns(fileSystem);
                context.Environment.Returns(environment);

                // When
                context.CreateDirectory("Hello");

                // Then
                fileSystem.Received(1).GetDirectory(Arg.Is<DirectoryPath>(
                    p => p.FullPath == "/Temp/Hello"));
            }
        }

        public sealed class TheDeleteDirectoryMethod
        {
            [Fact]
            public void Should_Throw_If_Context_Is_Null()
            {
                // Given, When
                var result = Record.Exception(() => DirectoryAliases.DeleteDirectory(null, "/Temp/DoNotExist"));

                // Then
                Assert.IsArgumentNullException(result, "context");
            }

            [Fact]
            public void Should_Throw_If_Directory_Is_Null()
            {
                // Given
                var fixture = new FileSystemFixture();
                var context = Substitute.For<ICakeContext>();
                context.FileSystem.Returns(fixture.FileSystem);

                // When
                var result = Record.Exception(() => context.DeleteDirectory(null));

                // Then
                Assert.IsArgumentNullException(result, "path");
            }

            [Fact]
            public void Should_Throw_If_Directory_Do_Not_Exist()
            {
                // Given
                var fixture = new FileSystemFixture();
                var context = Substitute.For<ICakeContext>();
                context.FileSystem.Returns(fixture.FileSystem);

                // When
                var result = Record.Exception(() =>
                    context.DeleteDirectory("/Temp/DoNotExist"));

                // Then
                Assert.IsType<IOException>(result);
                Assert.Equal("The directory '/Temp/DoNotExist' do not exist.", result.Message);
            }

            [Fact]
            public void Should_Throw_When_Deleting_Directory_With_Sub_Directories_If_Non_Recursive()
            {
                // Given
                var fixture = new FileSystemFixture();
                var context = Substitute.For<ICakeContext>();
                context.FileSystem.Returns(fixture.FileSystem);

                // When
                var result = Record.Exception(() =>
                    context.DeleteDirectory("/Temp/HasDirectories"));

                // Then
                Assert.IsType<IOException>(result);
                Assert.Equal("Cannot delete directory '/Temp/HasDirectories' without recursion since it's not empty.", result.Message);
            }

            [Fact]
            public void Should_Throw_When_Deleting_Directory_With_Files_If_Non_Recursive()
            {
                // Given
                var fixture = new FileSystemFixture();
                var context = Substitute.For<ICakeContext>();
                context.FileSystem.Returns(fixture.FileSystem);

                // When
                var result = Record.Exception(() =>
                    context.DeleteDirectory("/Temp/HasFiles"));

                // Then
                Assert.IsType<IOException>(result);
                Assert.Equal("Cannot delete directory '/Temp/HasFiles' without recursion since it's not empty.", result.Message);
            }

            [Fact]
            public void Should_Delete_Empty_Directory_If_Non_Recursive()
            {
                // Given
                var fixture = new FileSystemFixture();
                var context = Substitute.For<ICakeContext>();
                context.FileSystem.Returns(fixture.FileSystem);

                // When
                context.DeleteDirectory("/Temp/Hello/Empty");

                // Then
                Assert.False(fixture.FileSystem.GetDirectory("/Temp/Hello/Empty").Exists);
            }

            [Fact]
            public void Should_Delete_Directory_With_Content_If_Recurive()
            {
                // Given
                var fixture = new FileSystemFixture();
                var context = Substitute.For<ICakeContext>();
                context.FileSystem.Returns(fixture.FileSystem);

                // When
                context.DeleteDirectory("/Temp/Hello", true);

                // Then
                Assert.False(fixture.FileSystem.GetDirectory("/Temp/Hello").Exists);
            }
        }

        public sealed class TheDeleteDirectoriesMethod
        {
            public sealed class WithPaths
            {
                [Fact]
                public void Should_Throw_If_Context_Is_Null()
                {
                    // Given
                    var paths = new DirectoryPath[] { "/Temp/DoNotExist" };

                    // When
                    var result = Record.Exception(() =>
                        DirectoryAliases.DeleteDirectories(null, paths));

                    // Then
                    Assert.IsArgumentNullException(result, "context");
                }

                [Fact]
                public void Should_Throw_If_Directories_Are_Null()
                {
                    // Given
                    var context = Substitute.For<ICakeContext>();

                    // When
                    var result = Record.Exception(() =>
                        context.DeleteDirectories((IEnumerable<DirectoryPath>)null));

                    // Then
                    Assert.IsArgumentNullException(result, "directories");
                }

                [Fact]
                public void Should_Throw_If_Any_Directory_Do_Not_Exist()
                {
                    // Given
                    var fixture = new FileSystemFixture();
                    var context = Substitute.For<ICakeContext>();
                    context.FileSystem.Returns(fixture.FileSystem);

                    var paths = new DirectoryPath[] { "/Temp/DoNotExist" };

                    // When
                    var result = Record.Exception(() =>
                        context.DeleteDirectories(paths));

                    // Then
                    Assert.IsType<IOException>(result);
                    Assert.Equal("The directory '/Temp/DoNotExist' do not exist.", result.Message);
                }

                [Fact]
                public void Should_Throw_When_Deleting_Directory_With_Sub_Directories_If_Non_Recursive()
                {
                    // Given
                    var fixture = new FileSystemFixture();
                    var context = Substitute.For<ICakeContext>();
                    context.FileSystem.Returns(fixture.FileSystem);

                    var paths = new DirectoryPath[] { "/Temp/HasDirectories" };

                    // When
                    var result = Record.Exception(() =>
                        context.DeleteDirectories(paths));

                    // Then
                    Assert.IsType<IOException>(result);
                    Assert.Equal("Cannot delete directory '/Temp/HasDirectories' without recursion since it's not empty.", result.Message);
                }

                [Fact]
                public void Should_Throw_When_Deleting_Directory_With_Files_If_Non_Recursive()
                {
                    // Given
                    var fixture = new FileSystemFixture();
                    var context = Substitute.For<ICakeContext>();
                    context.FileSystem.Returns(fixture.FileSystem);

                    var paths = new DirectoryPath[] { "/Temp/HasFiles" };

                    // When
                    var result = Record.Exception(() =>
                        context.DeleteDirectories(paths));

                    // Then
                    Assert.IsType<IOException>(result);
                    Assert.Equal("Cannot delete directory '/Temp/HasFiles' without recursion since it's not empty.", result.Message);
                }

                [Fact]
                public void Should_Delete_Empty_Directory_If_Non_Recursive()
                {
                    // Given
                    var fixture = new FileSystemFixture();
                    var context = Substitute.For<ICakeContext>();
                    context.FileSystem.Returns(fixture.FileSystem);

                    var paths = new DirectoryPath[] { "/Temp/Hello/Empty", "/Temp/Hello/More/Empty" };

                    // When
                    context.DeleteDirectories(paths);

                    // Then
                    Assert.False(fixture.FileSystem.GetDirectory("/Temp/Hello/Empty").Exists);
                    Assert.False(fixture.FileSystem.GetDirectory("/Temp/Hello/More/Empty").Exists);
                }

                [Fact]
                public void Should_Delete_Directory_With_Content_If_Recurive()
                {
                    // Given
                    var fixture = new FileSystemFixture();
                    var context = Substitute.For<ICakeContext>();
                    context.FileSystem.Returns(fixture.FileSystem);

                    var paths = new DirectoryPath[] { "/Temp/Hello", "/Temp/Goodbye" };

                    // When
                    context.DeleteDirectories(paths, true);

                    // Then
                    Assert.False(fixture.FileSystem.GetDirectory("/Temp/Hello").Exists);
                    Assert.False(fixture.FileSystem.GetDirectory("/Temp/Goodbye").Exists);
                }                
            }

            public sealed class WithStrings
            {
                [Fact]
                public void Should_Throw_If_Context_Is_Null()
                {
                    // Given
                    var paths = new[] { "/Temp/DoNotExist" };

                    // When
                    var result = Record.Exception(() =>
                        DirectoryAliases.DeleteDirectories(null, paths));

                    // Then
                    Assert.IsArgumentNullException(result, "context");
                }

                [Fact]
                public void Should_Throw_If_Directories_Are_Null()
                {
                    // Given
                    var context = Substitute.For<ICakeContext>();

                    // When
                    var result = Record.Exception(() =>
                        context.DeleteDirectories((IEnumerable<string>)null));

                    // Then
                    Assert.IsArgumentNullException(result, "directories");
                }

                [Fact]
                public void Should_Throw_If_Any_Directory_Do_Not_Exist()
                {
                    // Given
                    var fixture = new FileSystemFixture();
                    var context = Substitute.For<ICakeContext>();
                    context.FileSystem.Returns(fixture.FileSystem);

                    var paths = new[] { "/Temp/DoNotExist" };

                    // When
                    var result = Record.Exception(() =>
                        context.DeleteDirectories(paths));

                    // Then
                    Assert.IsType<IOException>(result);
                    Assert.Equal("The directory '/Temp/DoNotExist' do not exist.", result.Message);
                }

                [Fact]
                public void Should_Throw_When_Deleting_Directory_With_Sub_Directories_If_Non_Recursive()
                {
                    // Given
                    var fixture = new FileSystemFixture();
                    var context = Substitute.For<ICakeContext>();
                    context.FileSystem.Returns(fixture.FileSystem);

                    var paths = new[] { "/Temp/HasDirectories" };

                    // When
                    var result = Record.Exception(() =>
                        context.DeleteDirectories(paths));

                    // Then
                    Assert.IsType<IOException>(result);
                    Assert.Equal("Cannot delete directory '/Temp/HasDirectories' without recursion since it's not empty.", result.Message);
                }

                [Fact]
                public void Should_Throw_When_Deleting_Directory_With_Files_If_Non_Recursive()
                {
                    // Given
                    var fixture = new FileSystemFixture();
                    var context = Substitute.For<ICakeContext>();
                    context.FileSystem.Returns(fixture.FileSystem);

                    var paths = new[] { "/Temp/HasFiles" };

                    // When
                    var result = Record.Exception(() =>
                        context.DeleteDirectories(paths));

                    // Then
                    Assert.IsType<IOException>(result);
                    Assert.Equal("Cannot delete directory '/Temp/HasFiles' without recursion since it's not empty.", result.Message);
                }

                [Fact]
                public void Should_Delete_Empty_Directory_If_Non_Recursive()
                {
                    // Given
                    var fixture = new FileSystemFixture();
                    var context = Substitute.For<ICakeContext>();
                    context.FileSystem.Returns(fixture.FileSystem);

                    var paths = new[] { "/Temp/Hello/Empty", "/Temp/Hello/More/Empty" };

                    // When
                    context.DeleteDirectories(paths);

                    // Then
                    Assert.False(fixture.FileSystem.GetDirectory("/Temp/Hello/Empty").Exists);
                    Assert.False(fixture.FileSystem.GetDirectory("/Temp/Hello/More/Empty").Exists);
                }

                [Fact]
                public void Should_Delete_Directory_With_Content_If_Recurive()
                {
                    // Given
                    var fixture = new FileSystemFixture();
                    var context = Substitute.For<ICakeContext>();
                    context.FileSystem.Returns(fixture.FileSystem);

                    var paths = new[] { "/Temp/Hello", "/Temp/Goodbye" };

                    // When
                    context.DeleteDirectories(paths, true);

                    // Then
                    Assert.False(fixture.FileSystem.GetDirectory("/Temp/Hello").Exists);
                    Assert.False(fixture.FileSystem.GetDirectory("/Temp/Goodbye").Exists);
                }                
            }
        }

        public sealed class TheCopyDirectoryMethod
        {
            [Fact]
            public void Should_Throw_If_Context_Is_Null()
            {
                // Given
                var sourcePath = new DirectoryPath("C:/Temp");
                var destinationPath = new DirectoryPath("C:/Temp2");

                // When
                var result = Record.Exception(() =>
                    DirectoryAliases.CopyDirectory(null, sourcePath, destinationPath));

                // Then
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("context", ((ArgumentNullException)result).ParamName);
            }

            [Fact]
            public void Should_Throw_If_Source_Directory_Is_Null()
            {
                // Given
                var context = Substitute.For<ICakeContext>();

                // When
                var result = Record.Exception(() =>
                    context.CopyDirectory(null, null));

                // Then
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("source", ((ArgumentNullException)result).ParamName);
            }

            [Fact]
            public void Should_Throw_If_Destination_Directory_Is_Null()
            {
                // Given
                var context = Substitute.For<ICakeContext>();
                var sourcePath = new DirectoryPath("C:/Temp");

                // When
                var result = Record.Exception(() =>
                    context.CopyDirectory(sourcePath, null));

                // Then
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("destination", ((ArgumentNullException)result).ParamName);
            }

            [Fact]
            public void Should_Throw_If_Source_Directory_Does_Not_Exist()
            {
                // Given
                var context = Substitute.For<ICakeContext>();
                context.FileSystem.Returns(new FakeFileSystem(false));
                var sourcePath = new DirectoryPath("C:/Temp");
                var destinationPath = new DirectoryPath("C:/Temp2");

                // When
                var result = Record.Exception(() =>
                    context.CopyDirectory(sourcePath, destinationPath));

                // Then
                Assert.IsType<DirectoryNotFoundException>(result);
            }

            [Fact]
            public void Should_Create_Destination_Folder_If_Not_Exist()
            {
                // Given
                var context = Substitute.For<ICakeContext>();
                var fileSystem = CreateFileStructure(new FakeFileSystem(false));
                context.FileSystem.Returns(fileSystem);
                var sourcePath = new DirectoryPath("C:/Temp");
                var destinationPath = new DirectoryPath("C:/Temp2");

                // When
                context.CopyDirectory(sourcePath, destinationPath);
                
                // Then
                Assert.True(fileSystem.Directories.ContainsKey(destinationPath));
            }

            [Fact]
            public void Should_Copy_Files()
            {
                // Given
                var context = Substitute.For<ICakeContext>();
                var fileSystem = CreateFileStructure(new FakeFileSystem(false));
                context.FileSystem.Returns(fileSystem);
                var sourcePath = new DirectoryPath("C:/Temp");
                var destinationPath = new DirectoryPath("C:/Temp2");

                // When
                context.CopyDirectory(sourcePath, destinationPath);

                // Then
                Assert.True(fileSystem.Files.ContainsKey(new FilePath("C:/Temp/file1.txt")));
                Assert.True(fileSystem.Files.ContainsKey(new FilePath("C:/Temp/file2.txt")));
            }

            [Fact]
            public void Should_Recursively_Copy_Files_And_Directory()
            {
                // Given
                var context = Substitute.For<ICakeContext>();
                var fileSystem = CreateFileStructure(new FakeFileSystem(false));
                context.FileSystem.Returns(fileSystem);
                var sourcePath = new DirectoryPath("C:/Temp");
                var destinationPath = new DirectoryPath("C:/Temp2");

                // When
                context.CopyDirectory(sourcePath, destinationPath);

                // Then
                // Directories should exist
                Assert.True(fileSystem.Directories.ContainsKey(new DirectoryPath("C:/Temp2/Stuff")));
                Assert.True(fileSystem.Directories.ContainsKey(new DirectoryPath("C:/Temp2/Things")));

                // Files should exist
                Assert.True(fileSystem.Files.ContainsKey(new FilePath("C:/Temp2/Stuff/file1.txt")));
                Assert.True(fileSystem.Files.ContainsKey(new FilePath("C:/Temp2/Stuff/file2.txt")));
                Assert.True(fileSystem.Files.ContainsKey(new FilePath("C:/Temp2/Things/file1.txt")));
            }

            private FakeFileSystem CreateFileStructure(FakeFileSystem ffs)
            {
                Action<string> dir = path => ffs.GetCreatedDirectory(path);
                Action<string> file = path => ffs.GetCreatedFile(path);

                dir("C:/Temp");
                {
                    file("C:/Temp/file1.txt");
                    file("C:/Temp/file2.txt");
                    dir("C:/Temp/Stuff");
                    {
                        file("C:/Temp/Stuff/file1.txt");
                        file("C:/Temp/Stuff/file2.txt");
                    }
                    dir("C:/Temp/Things");
                    {
                        file("C:/Temp/Things/file1.txt");
                    }
                }

                return ffs;
            }
        }

        public sealed class TheDirectoryExistsMethod
        {
            [Fact]
            public void Should_Throw_If_Context_Is_Null()
            {
                // Given, When
                var result = Record.Exception(() => DirectoryAliases.DirectoryExists(null, "something"));

                // Then
                Assert.IsArgumentNullException(result, "context");
            }

            [Fact]
            public void Should_Throw_If_Path_Is_Null()
            {
                // Given
                var context = Substitute.For<ICakeContext>();

                // When
                var result = Record.Exception(() => DirectoryAliases.DirectoryExists(context, null));

                // Then
                Assert.IsArgumentNullException(result, "path");
            }

            [Fact]
            public void Should_Return_False_If_Directory_Does_Not_Exist()
            {
                // Given
                var context = Substitute.For<ICakeContext>();
                var fileSystem = new FakeFileSystem(false);
                context.FileSystem.Returns(fileSystem);

                // When
                var result = DirectoryAliases.DirectoryExists(context, "non-existent-path");

                // Then
                Assert.False(result);
            }

            [Fact]
            public void Should_Return_True_If_Directory_Exist()
            {
                // Given
                var context = Substitute.For<ICakeContext>();
                var fileSystem = new FakeFileSystem(false);
                fileSystem.GetCreatedDirectory("some path");
                context.FileSystem.Returns(fileSystem);

                // When
                var result = DirectoryAliases.DirectoryExists(context, "some path");

                // Then
                Assert.True(result);
            }
        }

        public sealed class TheGetAbsoluteDirectoryPathMethod
        {
            [Fact]
            public void Should_Throw_If_Context_Is_Null()
            {
                // Given, When
                var result = Record.Exception(() => DirectoryAliases.GetAbsoluteDirectoryPath(null, "./build"));

                // Then
                Assert.IsArgumentNullException(result, "context");
            }

            [Fact]
            public void Should_Throw_If_Path_Is_Null()
            {
                // Given
                var context = Substitute.For<ICakeContext>();

                // When
                var result = Record.Exception(() => DirectoryAliases.GetAbsoluteDirectoryPath(context, null));

                // Then
                Assert.IsArgumentNullException(result, "path");
            }

            [Fact]
            public void Should_Return_Absolute_Directory_Path()
            {
                // Given
                var context = Substitute.For<ICakeContext>();
                context.Environment.WorkingDirectory.Returns(d => "/Working");

                // When
                var result = DirectoryAliases.GetAbsoluteDirectoryPath(context, "./build");

                // Then
                Assert.Equal("/Working/build", result.FullPath);
            }
        }
    }
}
