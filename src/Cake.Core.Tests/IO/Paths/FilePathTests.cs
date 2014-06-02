using Cake.Core.IO;
using Xunit;
using Xunit.Extensions;

namespace Cake.Core.Tests.IO.Paths
{
    public sealed class FilePathTests
    {
        [Theory]
        [InlineData("assets/shaders/basic.txt", true)]
        [InlineData("assets/shaders/basic", false)]
        [InlineData("assets/shaders/basic/", false)]
        public void Can_See_If_A_Path_Has_An_Extension(string fullPath, bool expected)
        {
            // Given, When
            var path = new FilePath(fullPath);

            // Then
            Assert.Equal(expected, path.HasExtension);
        }

        [Theory]
        [InlineData("assets/shaders/basic.frag", ".frag")]
        [InlineData("assets/shaders/basic.frag/test.vert", ".vert")]
        [InlineData("assets/shaders/basic", null)]
        [InlineData("assets/shaders/basic.frag/test", null)]
        public void Can_Get_Extension(string fullPath, string expected)
        {
            // Given, When
            var result = new FilePath(fullPath);
            var extension = result.GetExtension();

            // Then
            Assert.Equal(expected, extension);
        }

        [Fact]
        public void Can_Get_Directory_For_File_Path()
        {
            // Given, When
            var path = new FilePath("temp/hello.txt");
            var directory = path.GetDirectory();

            // Then
            Assert.Equal("temp", directory.FullPath);
        }

        [Fact]
        public void Can_Get_Directory_For_File_Path_In_Root()
        {
            // Given, When
            var path = new FilePath("hello.txt");
            var directory = path.GetDirectory();

            // Then
            Assert.Equal(string.Empty, directory.FullPath);
        }

        [Fact]
        public void Can_Change_Extension_Of_Path()
        {
            // Given
            var path = new FilePath("temp/hello.txt");

            // When
            path = path.ChangeExtension(".dat");

            // Then
            Assert.Equal("temp/hello.dat", path.ToString());
        }

        [Fact]
        public void Can_Get_Filename_From_Path()
        {
            // Given
            var path = new FilePath("/input/test.txt");

            // When
            var result = path.GetFilename();

            // Then
            Assert.Equal("test.txt", result.FullPath);
        }
    }
}