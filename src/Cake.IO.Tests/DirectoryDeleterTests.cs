using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cake.Core;
using Cake.Core.IO;
using Cake.IO.Tests.Fixtures;
using NSubstitute;
using Xunit;

namespace Cake.IO.Tests
{
    public sealed class DirectoryDeleterTests
    {
        public sealed class TheDeleteDirectoryMethod
        {
            [Fact]
            public void Should_Throw_If_Directory_Do_Not_Exist()
            {
                // Given
                var fixture = new FileSystemFixture();
                var context = Substitute.For<ICakeContext>();
                context.FileSystem.Returns(fixture.FileSystem);

                // When
                var result = Record.Exception(() =>
                    DirectoryDeleter.Delete(context, "/Temp/DoNotExist", recursive: false));

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
                    DirectoryDeleter.Delete(context, "/Temp/HasDirectories", recursive: false));

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
                    DirectoryDeleter.Delete(context, "/Temp/HasFiles", recursive: false));

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
                DirectoryDeleter.Delete(context, "/Temp/Hello/Empty", recursive: false);

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
                DirectoryDeleter.Delete(context, "/Temp/Hello", recursive: true);

                // Then
                Assert.False(fixture.FileSystem.GetDirectory("/Temp/Hello").Exists);
            }
        }
    }
}
