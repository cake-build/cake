// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Cake.Common.Security;
using Cake.Core;
using Cake.Core.IO;
using NSubstitute;
using Xunit;

namespace Cake.Common.Tests.Unit.Security
{
    public sealed class DirectoryHashCalculatorTests
    {
        public sealed class TheCalculateMethod
        {
            [Fact]
            public void Should_Throw_If_Directory_Path_Is_Null()
            {
                // Given
                var hashAlgorithmBuilder = Substitute.For<IHashAlgorithmBuilder>();
                var cakeContext = Substitute.For<ICakeContext>();
                var calculator = new DirectoryHashCalculator(cakeContext, hashAlgorithmBuilder);

                // When
                var result = Record.Exception(() => calculator.Calculate(null, null as string[], HashAlgorithm.MD5));

                // Then
                AssertEx.IsArgumentNullException(result, "directoryPath");
            }

            [Fact]
            public void Should_Throw_If_Directory_Does_Not_Exist()
            {
                // Given
                var hashAlgorithmBuilder = Substitute.For<IHashAlgorithmBuilder>();
                var fileSystem = Substitute.For<IFileSystem>();
                var file = Substitute.For<IFile>();
                file.Exists.Returns(false);
                fileSystem.GetFile(Arg.Any<FilePath>()).Returns(file);
                var cakeContext = Substitute.For<ICakeContext>();
                cakeContext.FileSystem.Returns(fileSystem);

                var calculator = new DirectoryHashCalculator(cakeContext, hashAlgorithmBuilder);

                // When
                var result = Record.Exception(() => calculator.Calculate("./non-existent-path", new List<string> { "./**/*.cs" }, HashAlgorithm.MD5));

                // Then
                AssertEx.IsExceptionWithMessage<CakeException>(result, "Directory 'non-existent-path' does not exist.");
            }
        }
    }
}