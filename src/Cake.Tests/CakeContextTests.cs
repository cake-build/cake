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
                // Given
                var environment = Substitute.For<ICakeEnvironment>();
                var globber = Substitute.For<IGlobber>();

                // When
                var exception = Record.Exception(() => new CakeContext(null, environment, globber));

                // Then
                Assert.IsType<ArgumentNullException>(exception);
                Assert.Equal("fileSystem", ((ArgumentNullException)exception).ParamName);
            }

            [Fact]
            public void Should_Throw_If_Environment_Is_Null()
            {
                // Given
                var fileSystem = Substitute.For<IFileSystem>();
                var globber = Substitute.For<IGlobber>();

                // When
                var exception = Record.Exception(() => new CakeContext(fileSystem, null, globber));

                // Then
                Assert.IsType<ArgumentNullException>(exception);
                Assert.Equal("environment", ((ArgumentNullException)exception).ParamName);
            }

            [Fact]
            public void Should_Throw_If_Globber_Is_Null()
            {
                // Given
                var fileSystem = Substitute.For<IFileSystem>();
                var environment = Substitute.For<ICakeEnvironment>();

                // When
                var exception = Record.Exception(() => new CakeContext(fileSystem, environment, null));

                // Then
                Assert.IsType<ArgumentNullException>(exception);
                Assert.Equal("globber", ((ArgumentNullException)exception).ParamName);
            }
        }

        public sealed class TheFileSystemProperty
        {
            [Fact]
            public void Should_Return_The_File_System_Provided_To_The_Constructor()
            {
                // Given, When
                var fileSystem = Substitute.For<IFileSystem>();
                var environment = Substitute.For<ICakeEnvironment>();
                var globber = Substitute.For<IGlobber>();
                var context = new CakeContext(fileSystem, environment, globber);

                // Then
                Assert.Equal(fileSystem, context.FileSystem);
            }
        }
    }
}
