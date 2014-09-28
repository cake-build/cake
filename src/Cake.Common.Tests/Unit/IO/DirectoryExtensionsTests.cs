using System;
using System.Collections.Generic;
using System.IO;
using Cake.Common.IO;
using Cake.Common.Tests.Fixtures;
using Cake.Core;
using Cake.Core.IO;
using NSubstitute;
using Xunit;

namespace Cake.Common.Tests.Unit.IO
{
    public sealed class DirectoryExtensionsTests
    {
        public sealed class TheCleanMethod
        {
            [Fact]
            public void Should_Throw_If_Context_Is_Null()
            {
                // Given, When
                var result = Record.Exception(() =>
                    DirectoryExtensions.CleanDirectory(null, "/Temp/Hello"));

                // Then
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("context", ((ArgumentNullException)result).ParamName);
            }

            [Fact]
            public void Should_Throw_If_Directory_Are_Null()
            {
                // Given
                var context = Substitute.For<ICakeContext>();
                context.FileSystem.Returns(Substitute.For<IFileSystem>());

                // When
                var result = Record.Exception(() =>
                    DirectoryExtensions.CleanDirectory(context, null));

                // Then
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("path", ((ArgumentNullException) result).ParamName);
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
                DirectoryExtensions.CleanDirectory(context, directory);

                // Then
                Assert.Empty(fixture.FileSystem.GetDirectory(directory).GetFiles("*", SearchScope.Recursive));
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
                DirectoryExtensions.CleanDirectory(context, directory);

                // Then
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
                DirectoryExtensions.CleanDirectory(context, directory);

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
                DirectoryExtensions.CleanDirectory(context, directory);

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
                        DirectoryExtensions.CleanDirectories(null, paths));

                    // Then
                    Assert.IsType<ArgumentNullException>(result);
                    Assert.Equal("context", ((ArgumentNullException)result).ParamName);
                }

                [Fact]
                public void Should_Throw_If_Directories_Are_Null()
                {
                    // Given
                    var context = Substitute.For<ICakeContext>();

                    // When
                    var result = Record.Exception(() =>
                        DirectoryExtensions.CleanDirectories(context, (IEnumerable<DirectoryPath>)null));

                    // Then
                    Assert.IsType<ArgumentNullException>(result);
                    Assert.Equal("directories", ((ArgumentNullException)result).ParamName);
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
                    Record.Exception(() => DirectoryExtensions.CleanDirectories(context, paths));

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
                    DirectoryExtensions.CleanDirectories(context, paths);

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
                    DirectoryExtensions.CleanDirectories(context, paths);

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
                        DirectoryExtensions.CleanDirectories(null, paths));

                    // Then
                    Assert.IsType<ArgumentNullException>(result);
                    Assert.Equal("context", ((ArgumentNullException)result).ParamName);
                }

                [Fact]
                public void Should_Throw_If_Directories_Are_Null()
                {
                    // Given
                    var context = Substitute.For<ICakeContext>();

                    // When
                    var result = Record.Exception(() =>
                        DirectoryExtensions.CleanDirectories(context, (IEnumerable<string>)null));

                    // Then
                    Assert.IsType<ArgumentNullException>(result);
                    Assert.Equal("directories", ((ArgumentNullException)result).ParamName);
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
                    Record.Exception(() => DirectoryExtensions.CleanDirectories(context, paths));

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
                    DirectoryExtensions.CleanDirectories(context, paths);

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
                    DirectoryExtensions.CleanDirectories(context, paths);

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
                    DirectoryExtensions.CreateDirectory(null, path));

                // Then
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("context", ((ArgumentNullException)result).ParamName);
            }

            [Fact]
            public void Should_Throw_If_Directory_Is_Null()
            {
                // Given
                var context = Substitute.For<ICakeContext>();

                // When
                var result = Record.Exception(() =>
                    DirectoryExtensions.CreateDirectory(context, null));

                // Then
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("path", ((ArgumentNullException)result).ParamName);
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
                DirectoryExtensions.CreateDirectory(context, "/Temp");

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
                DirectoryExtensions.CreateDirectory(context, "/Temp");

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
                DirectoryExtensions.CreateDirectory(context, "Hello");

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
                // Given
                var fixture = new FileSystemFixture();

                // When
                var result = Record.Exception(() => DirectoryExtensions.DeleteDirectory(null, "/Temp/DoNotExist"));

                // Then
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("context", ((ArgumentNullException)result).ParamName);
            }

            [Fact]
            public void Should_Throw_If_Directory_Is_Null()
            {
                // Given
                var fixture = new FileSystemFixture();
                var context = Substitute.For<ICakeContext>();
                context.FileSystem.Returns(fixture.FileSystem);

                // When
                var result = Record.Exception(() => DirectoryExtensions.DeleteDirectory(context, null));

                // Then
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("path", ((ArgumentNullException)result).ParamName);
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
                    DirectoryExtensions.DeleteDirectory(context, "/Temp/DoNotExist"));

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
                    DirectoryExtensions.DeleteDirectory(context, "/Temp/HasDirectories"));

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
                    DirectoryExtensions.DeleteDirectory(context, "/Temp/HasFiles"));

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
                DirectoryExtensions.DeleteDirectory(context, "/Temp/Hello/Empty");

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
                DirectoryExtensions.DeleteDirectory(context, "/Temp/Hello", true);

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
                        DirectoryExtensions.DeleteDirectories(null, paths));

                    // Then
                    Assert.IsType<ArgumentNullException>(result);
                    Assert.Equal("context", ((ArgumentNullException)result).ParamName);
                }

                [Fact]
                public void Should_Throw_If_Directories_Are_Null()
                {
                    // Given
                    var context = Substitute.For<ICakeContext>();

                    // When
                    var result = Record.Exception(() =>
                        DirectoryExtensions.DeleteDirectories(context, (IEnumerable<DirectoryPath>)null));

                    // Then
                    Assert.IsType<ArgumentNullException>(result);
                    Assert.Equal("directories", ((ArgumentNullException)result).ParamName);
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
                        DirectoryExtensions.DeleteDirectories(context, paths));

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
                        DirectoryExtensions.DeleteDirectories(context, paths));

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
                        DirectoryExtensions.DeleteDirectories(context, paths));

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
                    DirectoryExtensions.DeleteDirectories(context, paths);

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
                    DirectoryExtensions.DeleteDirectories(context, paths, true);

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
                        DirectoryExtensions.DeleteDirectories(null, paths));

                    // Then
                    Assert.IsType<ArgumentNullException>(result);
                    Assert.Equal("context", ((ArgumentNullException)result).ParamName);
                }

                [Fact]
                public void Should_Throw_If_Directories_Are_Null()
                {
                    // Given
                    var context = Substitute.For<ICakeContext>();

                    // When
                    var result = Record.Exception(() =>
                        DirectoryExtensions.DeleteDirectories(context, (IEnumerable<string>)null));

                    // Then
                    Assert.IsType<ArgumentNullException>(result);
                    Assert.Equal("directories", ((ArgumentNullException)result).ParamName);
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
                        DirectoryExtensions.DeleteDirectories(context, paths));

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
                        DirectoryExtensions.DeleteDirectories(context, paths));

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
                        DirectoryExtensions.DeleteDirectories(context, paths));

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
                    DirectoryExtensions.DeleteDirectories(context, paths);

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
                    DirectoryExtensions.DeleteDirectories(context, paths, true);

                    // Then
                    Assert.False(fixture.FileSystem.GetDirectory("/Temp/Hello").Exists);
                    Assert.False(fixture.FileSystem.GetDirectory("/Temp/Goodbye").Exists);
                }                
            }
        }
    }
}
