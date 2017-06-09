// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
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
                AssertEx.IsArgumentNullException(result, "context");
            }

            [Fact]
            public void Should_Throw_If_Path_Is_Null()
            {
                // Given
                var context = Substitute.For<ICakeContext>();

                // When
                var result = Record.Exception(() => DirectoryAliases.Directory(context, null));

                // Then
                AssertEx.IsArgumentNullException(result, "path");
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
                AssertEx.IsArgumentNullException(result, "context");
            }

            [Fact]
            public void Should_Throw_If_Directory_Are_Null()
            {
                // Given
                var context = Substitute.For<ICakeContext>();
                context.FileSystem.Returns(Substitute.For<IFileSystem>());

                // When
                var result = Record.Exception(() =>
                    DirectoryAliases.CleanDirectory(context, null));

                // Then
                AssertEx.IsArgumentNullException(result, "path");
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
                DirectoryAliases.CleanDirectory(context, directory);

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
                Func<IFileSystemInfo, bool> wherePredicate = entry => !entry.Hidden;
                context.FileSystem.Returns(fixture.FileSystem);
                var filesNotMatchingPredicate = fixture
                    .FileSystem
                    .GetDirectory(directory)
                    .GetFiles("*", SearchScope.Recursive)
                    .Where(entry => !wherePredicate(entry))
                    .ToArray();

                // When
                DirectoryAliases.CleanDirectory(context, directory, wherePredicate);

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
                DirectoryAliases.CleanDirectory(context, directory);

                // Then
                Assert.Empty(fixture.FileSystem.GetDirectory(directory).GetDirectories("*", SearchScope.Recursive));
            }

            [Fact]
            public void Should_Delete_Directories_Respecting_Predicate_In_Provided_Directory()
            {
                // Given
                var fixture = new FileSystemFixture();
                var context = Substitute.For<ICakeContext>();
                context.FileSystem.Returns(fixture.FileSystem);
                var directory = fixture.FileSystem.GetDirectory("/Temp/Hello");

                // When
                DirectoryAliases.CleanDirectory(context, directory.Path, info => !info.Hidden);

                // Then
                Assert.Equal(1, directory.GetDirectories("*", SearchScope.Recursive).Count());
                Assert.True(fixture.FileSystem.GetDirectory("/Temp/Hello/Hidden").Exists);
            }

            [Fact]
            public void Should_Delete_Files_And_Directories_In_Provided_Directory_Using_Relative_Path()
            {
                // Given
                var directory = new DirectoryPath("./Hello");
                var fixture = new FileSystemFixture();
                var context = Substitute.For<ICakeContext>();
                context.FileSystem.Returns(fixture.FileSystem);
                context.Environment.WorkingDirectory.Returns(info => new DirectoryPath("/Temp"));

                // When
                DirectoryAliases.CleanDirectory(context, directory);

                // Then
                Assert.Empty(fixture.FileSystem.GetDirectory(directory).GetFiles("*", SearchScope.Recursive));
                Assert.Empty(fixture.FileSystem.GetDirectory(directory).GetDirectories("*", SearchScope.Recursive));
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
                DirectoryAliases.CleanDirectory(context, directory);

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
                DirectoryAliases.CleanDirectory(context, directory);

                // Then
                Assert.True(fixture.FileSystem.Exist((DirectoryPath)"/NonExisting"));
            }

            [Fact]
            public void Should_Skip_Predicate_Folder()
            {
                // Given
                var fixture = new FileSystemFixture();
                var context = Substitute.For<ICakeContext>();
                context.FileSystem.Returns(fixture.FileSystem);
                var directory = context.FileSystem.GetDirectory("/Temp");

                // When
                DirectoryAliases.CleanDirectory(context, directory.Path, predicate => predicate.Path.FullPath != "/Temp/Goodbye");

                // Then
                Assert.True(context.FileSystem.GetDirectory("/Temp").Exists);
                Assert.Equal(1, directory.GetDirectories("*", SearchScope.Recursive).Count());
                Assert.True(context.FileSystem.GetDirectory("/Temp/Goodbye").Exists);
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
                    AssertEx.IsArgumentNullException(result, "context");
                }

                [Fact]
                public void Should_Throw_If_Directories_Are_Null()
                {
                    // Given
                    var context = Substitute.For<ICakeContext>();

                    // When
                    var result = Record.Exception(() =>
                        DirectoryAliases.CleanDirectories(context, (IEnumerable<DirectoryPath>)null));

                    // Then
                    AssertEx.IsArgumentNullException(result, "directories");
                }

                [Fact]
                public void Should_Delete_Files_In_Provided_Directories()
                {
                    // Given
                    var fixture = new FileSystemFixture();
                    var context = Substitute.For<ICakeContext>();
                    context.FileSystem.Returns(fixture.FileSystem);

                    var paths = new DirectoryPath[]
                    {
                        "/Temp/Hello", "/Temp/Goodbye"
                    };

                    // When
                    DirectoryAliases.CleanDirectories(context, paths);

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

                    var paths = new DirectoryPath[]
                    {
                        "/Temp/Hello", "/Temp/Goodbye"
                    };

                    // When
                    DirectoryAliases.CleanDirectories(context, paths);

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

                    var paths = new DirectoryPath[]
                    {
                        "/Temp/Hello", "/NonExisting"
                    };

                    // When
                    DirectoryAliases.CleanDirectories(context, paths);

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
                    AssertEx.IsArgumentNullException(result, "context");
                }

                [Fact]
                public void Should_Throw_If_Directories_Are_Null()
                {
                    // Given
                    var context = Substitute.For<ICakeContext>();

                    // When
                    var result = Record.Exception(() =>
                        DirectoryAliases.CleanDirectories(context, (IEnumerable<string>)null));

                    // Then
                    AssertEx.IsArgumentNullException(result, "directories");
                }

                [Fact]
                public void Should_Delete_Files_In_Provided_Directories()
                {
                    // Given
                    var fixture = new FileSystemFixture();
                    var context = Substitute.For<ICakeContext>();
                    context.FileSystem.Returns(fixture.FileSystem);

                    var paths = new[]
                    {
                        "/Temp/Hello", "/Temp/Goodbye"
                    };

                    // When
                    DirectoryAliases.CleanDirectories(context, paths);

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

                    var paths = new[]
                    {
                        "/Temp/Hello", "/Temp/Goodbye"
                    };

                    // When
                    DirectoryAliases.CleanDirectories(context, paths);

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

                    var paths = new[]
                    {
                        "/Temp/Hello", "/NonExisting"
                    };

                    // When
                    DirectoryAliases.CleanDirectories(context, paths);

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
                AssertEx.IsArgumentNullException(result, "context");
            }

            [Fact]
            public void Should_Throw_If_Directory_Is_Null()
            {
                // Given
                var context = Substitute.For<ICakeContext>();

                // When
                var result = Record.Exception(() =>
                    DirectoryAliases.CreateDirectory(context, null));

                // Then
                AssertEx.IsArgumentNullException(result, "path");
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
                DirectoryAliases.CreateDirectory(context, "/Temp");

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
                DirectoryAliases.CreateDirectory(context, "/Temp");

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
                DirectoryAliases.CreateDirectory(context, "Hello");

                // Then
                fileSystem.Received(1).GetDirectory(Arg.Is<DirectoryPath>(
                    p => p.FullPath == "/Temp/Hello"));
            }
        }

        public sealed class TheEnsureExistsMethod
        {
            [Fact]
            public void Should_Throw_If_Context_Is_Null()
            {
                // Given
                var path = new DirectoryPath("/Temp");

                // When
                var result = Record.Exception(() =>
                     DirectoryAliases.EnsureDirectoryExists(null, path));

                // Then
                AssertEx.IsArgumentNullException(result, "context");
            }

            [Fact]
            public void Should_Throw_If_Directory_Is_Null()
            {
                // Given
                var context = Substitute.For<ICakeContext>();

                // When
                var result = Record.Exception(() =>
                     DirectoryAliases.EnsureDirectoryExists(context, null));

                // Then
                AssertEx.IsArgumentNullException(result, "path");
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
                DirectoryAliases.EnsureDirectoryExists(context, "/Temp");

                // Then
                directory.Received(1).Create();
            }

            [Fact]
            public void Should_Not_Create_Directory_Or_Fail_If_It_Already_Exist()
            {
                // Given
                var fileSystem = Substitute.For<IFileSystem>();
                var directory = Substitute.For<IDirectory>();
                directory.Exists.Returns(true);
                fileSystem.GetDirectory(Arg.Is<DirectoryPath>(p => p.FullPath == "/Temp")).Returns(directory);

                var context = Substitute.For<ICakeContext>();
                context.FileSystem.Returns(fileSystem);

                // When
                DirectoryAliases.EnsureDirectoryExists(context, "/Temp");

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
                DirectoryAliases.EnsureDirectoryExists(context, "Hello");

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
                AssertEx.IsArgumentNullException(result, "context");
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
                AssertEx.IsArgumentNullException(result, "path");
            }

            [Fact]
            public void Should_Throw_If_Directory_Do_Not_Exist()
            {
                // Given
                var fixture = new FileSystemFixture();
                var context = Substitute.For<ICakeContext>();
                context.FileSystem.Returns(fixture.FileSystem);

                // When
                var result = Record.Exception(() => context.DeleteDirectory("/Temp/DoNotExist"));

                // Then
                Assert.IsType<IOException>(result);
                Assert.Equal("The directory '/Temp/DoNotExist' does not exist.", result?.Message);
            }

            [Fact]
            public void Should_Throw_When_Deleting_Directory_With_Sub_Directories_If_Non_Recursive()
            {
                // Given
                var fixture = new FileSystemFixture();
                var context = Substitute.For<ICakeContext>();
                context.FileSystem.Returns(fixture.FileSystem);

                // When
                var result = Record.Exception(() => context.DeleteDirectory("/Temp/HasDirectories"));

                // Then
                Assert.IsType<IOException>(result);
                Assert.Equal("Cannot delete directory '/Temp/HasDirectories' without recursion since it's not empty.", result?.Message);
            }

            [Fact]
            public void Should_Throw_When_Deleting_Directory_With_Files_If_Non_Recursive()
            {
                // Given
                var fixture = new FileSystemFixture();
                var context = Substitute.For<ICakeContext>();
                context.FileSystem.Returns(fixture.FileSystem);

                // When
                var result = Record.Exception(() => context.DeleteDirectory("/Temp/HasFiles"));

                // Then
                Assert.IsType<IOException>(result);
                Assert.Equal("Cannot delete directory '/Temp/HasFiles' without recursion since it's not empty.", result?.Message);
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
            public void Should_Delete_Directory_With_Content_If_Recursive()
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
                    AssertEx.IsArgumentNullException(result, "context");
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
                    AssertEx.IsArgumentNullException(result, "directories");
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
                    var result = Record.Exception(() => context.DeleteDirectories(paths));

                    // Then
                    Assert.IsType<IOException>(result);
                    Assert.Equal("The directory '/Temp/DoNotExist' does not exist.", result?.Message);
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
                    Assert.Equal("Cannot delete directory '/Temp/HasDirectories' without recursion since it's not empty.", result?.Message);
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
                    Assert.Equal("Cannot delete directory '/Temp/HasFiles' without recursion since it's not empty.", result?.Message);
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
                public void Should_Delete_Directory_With_Content_If_Recursive()
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
                    AssertEx.IsArgumentNullException(result, "context");
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
                    AssertEx.IsArgumentNullException(result, "directories");
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
                    Assert.Equal("The directory '/Temp/DoNotExist' does not exist.", result?.Message);
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
                    Assert.Equal("Cannot delete directory '/Temp/HasDirectories' without recursion since it's not empty.", result?.Message);
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
                    Assert.Equal("Cannot delete directory '/Temp/HasFiles' without recursion since it's not empty.", result?.Message);
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
                public void Should_Delete_Directory_With_Content_If_Recursive()
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
                Assert.Equal("context", ((ArgumentNullException)result)?.ParamName);
            }

            [Fact]
            public void Should_Throw_If_Source_Directory_Is_Null()
            {
                // Given
                var context = Substitute.For<ICakeContext>();

                // When
                var result = Record.Exception(() =>
                    DirectoryAliases.CopyDirectory(context, null, null));

                // Then
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("source", ((ArgumentNullException)result)?.ParamName);
            }

            [Fact]
            public void Should_Throw_If_Destination_Directory_Is_Null()
            {
                // Given
                var context = Substitute.For<ICakeContext>();
                var sourcePath = new DirectoryPath("C:/Temp");

                // When
                var result = Record.Exception(() =>
                    DirectoryAliases.CopyDirectory(context, sourcePath, null));

                // Then
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("destination", ((ArgumentNullException)result)?.ParamName);
            }

            [Fact]
            public void Should_Throw_If_Source_Directory_Does_Not_Exist()
            {
                // Given
                var context = Substitute.For<ICakeContext>();
                var environment = FakeEnvironment.CreateUnixEnvironment();
                context.FileSystem.Returns(new FakeFileSystem(environment));
                var sourcePath = new DirectoryPath("/Temp");
                var destinationPath = new DirectoryPath("/Temp2");

                // When
                var result = Record.Exception(() =>
                    DirectoryAliases.CopyDirectory(context, sourcePath, destinationPath));

                // Then
                Assert.IsType<DirectoryNotFoundException>(result);
                Assert.Equal("Source directory does not exist or could not be found: /Temp", result?.Message);
            }

            [Fact]
            public void Should_Create_Destination_Folder_If_Not_Exist()
            {
                // Given
                var context = Substitute.For<ICakeContext>();
                var environment = FakeEnvironment.CreateUnixEnvironment();
                var fileSystem = new FakeFileSystem(environment);
                CreateFileStructure(fileSystem);
                context.FileSystem.Returns(fileSystem);
                var sourcePath = new DirectoryPath("/Temp");
                var destinationPath = new DirectoryPath("/Temp2");

                // When
                DirectoryAliases.CopyDirectory(context, sourcePath, destinationPath);

                // Then
                Assert.True(fileSystem.GetDirectory(destinationPath).Exists);
            }

            [Fact]
            public void Should_Copy_Files()
            {
                // Given
                var context = Substitute.For<ICakeContext>();
                var environment = FakeEnvironment.CreateUnixEnvironment();
                var fileSystem = new FakeFileSystem(environment);
                CreateFileStructure(fileSystem);
                context.FileSystem.Returns(fileSystem);
                var sourcePath = new DirectoryPath("/Temp");
                var destinationPath = new DirectoryPath("/Temp2");

                // When
                DirectoryAliases.CopyDirectory(context, sourcePath, destinationPath);

                // Then
                Assert.True(fileSystem.GetFile("/Temp/file1.txt").Exists);
                Assert.True(fileSystem.GetFile("/Temp/file2.txt").Exists);
            }

            [Fact]
            public void Should_Recursively_Copy_Files_And_Directory()
            {
                // Given
                var context = Substitute.For<ICakeContext>();
                var environment = FakeEnvironment.CreateUnixEnvironment();
                var fileSystem = new FakeFileSystem(environment);
                CreateFileStructure(fileSystem);
                context.FileSystem.Returns(fileSystem);
                var sourcePath = new DirectoryPath("/Temp");
                var destinationPath = new DirectoryPath("/Temp2");

                // When
                DirectoryAliases.CopyDirectory(context, sourcePath, destinationPath);

                // Then
                // Directories should exist
                Assert.True(fileSystem.GetDirectory("/Temp2/Stuff").Exists);
                Assert.True(fileSystem.GetDirectory("/Temp2/Things").Exists);

                // Files should exist
                Assert.True(fileSystem.GetFile("/Temp2/Stuff/file1.txt").Exists);
                Assert.True(fileSystem.GetFile("/Temp2/Stuff/file2.txt").Exists);
                Assert.True(fileSystem.GetFile("/Temp2/Things/file1.txt").Exists);
            }

            private static void CreateFileStructure(FakeFileSystem ffs)
            {
                Action<string> dir = path => ffs.CreateDirectory(path);
                Action<string> file = path => ffs.CreateFile(path);

                dir("/Temp");
                {
                    file("/Temp/file1.txt");
                    file("/Temp/file2.txt");
                    dir("/Temp/Stuff");
                    {
                        file("/Temp/Stuff/file1.txt");
                        file("/Temp/Stuff/file2.txt");
                    }
                    dir("/Temp/Things");
                    {
                        file("/Temp/Things/file1.txt");
                    }
                }
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
                AssertEx.IsArgumentNullException(result, "context");
            }

            [Fact]
            public void Should_Throw_If_Path_Is_Null()
            {
                // Given
                var context = Substitute.For<ICakeContext>();

                // When
                var result = Record.Exception(() => DirectoryAliases.DirectoryExists(context, null));

                // Then
                AssertEx.IsArgumentNullException(result, "path");
            }

            [Fact]
            public void Should_Return_False_If_Directory_Does_Not_Exist()
            {
                // Given
                var context = Substitute.For<ICakeContext>();
                var environment = FakeEnvironment.CreateUnixEnvironment();
                var fileSystem = new FakeFileSystem(environment);
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
                var environment = FakeEnvironment.CreateUnixEnvironment();
                var fileSystem = new FakeFileSystem(environment);
                fileSystem.CreateDirectory("some path");
                context.FileSystem.Returns(fileSystem);

                // When
                var result = DirectoryAliases.DirectoryExists(context, "some path");

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
                var result = Record.Exception(() => DirectoryAliases.MakeAbsolute(null, "./build"));

                // Then
                AssertEx.IsArgumentNullException(result, "context");
            }

            [Fact]
            public void Should_Throw_If_Path_Is_Null()
            {
                // Given
                var context = Substitute.For<ICakeContext>();

                // When
                var result = Record.Exception(() => DirectoryAliases.MakeAbsolute(context, null));

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
                var result = DirectoryAliases.MakeAbsolute(context, "./build");

                // Then
                Assert.Equal("/Working/build", result.FullPath);
            }
        }

        public sealed class TheMoveDirectoryMethod
        {
            [Fact]
            public void Should_Throw_If_Context_Is_Null()
            {
                // Given, When
                var source = new DirectoryPath("./source");
                var target = new DirectoryPath("./target");

                var result = Record.Exception(() =>
                    DirectoryAliases.MoveDirectory(null, source, target));

                // Then
                AssertEx.IsArgumentNullException(result, "context");
            }

            [Fact]
            public void Should_Throw_If_Source_Directory_Path_Is_Null()
            {
                // Given
                var context = Substitute.For<ICakeContext>();
                var target = new DirectoryPath("./target");

                // When
                var result = Record.Exception(() =>
                    DirectoryAliases.MoveDirectory(context, null, target));

                // Then
                AssertEx.IsArgumentNullException(result, "directoryPath");
            }

            [Fact]
            public void Should_Throw_If_Target_Directory_Path_Is_Null()
            {
                // Given
                var context = Substitute.For<ICakeContext>();
                var source = new DirectoryPath("./source");

                // When
                var result = Record.Exception(() =>
                    DirectoryAliases.MoveDirectory(context, source, null));

                // Then
                AssertEx.IsArgumentNullException(result, "targetDirectoryPath");
            }

            [Fact]
            public void Should_Recursively_Move_Files_And_Directory()
            {
                // Given
                var context = Substitute.For<ICakeContext>();
                var environment = FakeEnvironment.CreateUnixEnvironment();
                var fileSystem = new FakeFileSystem(environment);
                CreateFileStructure(fileSystem);
                context.FileSystem.Returns(fileSystem);
                var sourcePath = new DirectoryPath("/Temp");
                var destinationPath = new DirectoryPath("/Temp2");

                // When
                DirectoryAliases.MoveDirectory(context, sourcePath, destinationPath);

                // Then
                Assert.False(fileSystem.GetDirectory("/Temp/Stuff").Exists);
                Assert.False(fileSystem.GetDirectory("/Temp/Things").Exists);
                Assert.True(fileSystem.GetDirectory("/Temp2/Stuff").Exists);
                Assert.True(fileSystem.GetDirectory("/Temp2/Things").Exists);

                Assert.False(fileSystem.GetFile("/Temp/Stuff/file1.txt").Exists);
                Assert.False(fileSystem.GetFile("/Temp/Stuff/file2.txt").Exists);
                Assert.False(fileSystem.GetFile("/Temp/Things/file1.txt").Exists);
                Assert.True(fileSystem.GetFile("/Temp2/Stuff/file1.txt").Exists);
                Assert.True(fileSystem.GetFile("/Temp2/Stuff/file2.txt").Exists);
                Assert.True(fileSystem.GetFile("/Temp2/Things/file1.txt").Exists);
            }

            private static void CreateFileStructure(FakeFileSystem ffs)
            {
                Action<string> dir = path => ffs.CreateDirectory(path);
                Action<string> file = path => ffs.CreateFile(path);

                dir("/Temp");
                {
                    file("/Temp/file1.txt");
                    file("/Temp/file2.txt");
                    dir("/Temp/Stuff");
                    {
                        file("/Temp/Stuff/file1.txt");
                        file("/Temp/Stuff/file2.txt");
                    }
                    dir("/Temp/Things");
                    {
                        file("/Temp/Things/file1.txt");
                    }
                }
            }
        }
    }
}