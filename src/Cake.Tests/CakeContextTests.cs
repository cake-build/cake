using System;
using Cake.Core;
using Cake.Core.IO;
using NSubstitute;
using Xunit;

namespace Cake.Tests
{
    public sealed class CakeContextTests
    {
        public sealed class TheConstructor
        {
            [Fact]
            public void Should_Throw_If_File_System_Is_Null()
            {
                // Given, When
                var exception = Record.Exception(() => new CakeContext(null, null));

                // Then
                Assert.IsType<ArgumentNullException>(exception);
                Assert.Equal("fileSystem", ((ArgumentNullException)exception).ParamName);
            }
        }

        public sealed class TheFileSystemProperty
        {
            [Fact]
            public void Should_Return_The_File_System_Provided_To_The_Constructor()
            {
                // Given, When
                var fileSystem = Substitute.For<IFileSystem>();
                var context = new CakeContext(fileSystem, null);

                // Then
                Assert.Equal(fileSystem, context.FileSystem);
            }
        }
    }
}
