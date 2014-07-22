using System;
using Cake.Core;
using Cake.Core.IO;
using NSubstitute;
using Xunit;
using Cake.Common.IO;

namespace Cake.Common.Tests.Unit.IO
{
    public sealed class DirectoryCreatorTests
    {
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
            public void Should_Throw_If_Path_Is_Null()
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
    }
}
