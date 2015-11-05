﻿using Cake.Common.Security;
using Cake.Core;
using Cake.Core.IO;
using NSubstitute;
using Xunit;

namespace Cake.Common.Tests.Unit.Security
{
    public sealed class FileHashCalculatorTests
    {
        public sealed class TheCalculateMethod
        {
            [Fact]
            public void Should_Throw_If_File_Path_Is_Null()
            {
                // Given
                var fileSystem = Substitute.For<IFileSystem>();
                var calculator = new FileHashCalculator(fileSystem);

                // When
                var result = Record.Exception(() => calculator.Calculate(null, HashAlgorithm.MD5));

                // Then
                Assert.IsArgumentNullException(result, "filePath");
            }

            [Fact]
            public void Should_Throw_If_File_Does_Not_Exist()
            {
                // Given
                var fileSystem = Substitute.For<IFileSystem>();
                var file = Substitute.For<IFile>();
                file.Exists.Returns(false);
                fileSystem.GetFile(Arg.Any<FilePath>()).Returns(file);

                var calculator = new FileHashCalculator(fileSystem);

                // When
                var result = Record.Exception(() => calculator.Calculate("./non-existent-path", HashAlgorithm.MD5));

                // Then
                Assert.IsExceptionWithMessage<CakeException>(result, "File 'non-existent-path' does not exist.");
            }
        }
    }
}