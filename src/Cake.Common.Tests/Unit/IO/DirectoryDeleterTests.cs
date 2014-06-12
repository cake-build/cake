using System.IO;
using Cake.Common.IO;
using Cake.Common.Tests.Fixtures;
using Cake.Core;
using NSubstitute;
using Xunit;

namespace Cake.Common.Tests.Unit.IO
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
    }
}
