// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Security;
using Xunit;

namespace Cake.Common.Tests.Unit.Security
{
    public sealed class HashAlgorithmBuilderTests
    {
        public sealed class TheCreateHashAlgorithmMethod
        {
            [Fact]
            public void Should_Return_MD5_Algorithm_Instance_If_Input_Is_Cake_HashAlgorithm_Enum_MD5()
            {
                // Given
                var hashAlgorithmbuilder = new HashAlgorithmBuilder();

                // When
                var result = hashAlgorithmbuilder.CreateHashAlgorithm(HashAlgorithm.MD5);

                // Then
                Assert.IsAssignableFrom<System.Security.Cryptography.MD5>(result);
            }

            [Fact]
            public void Should_Return_SHA256_Algorithm_Instance_If_Input_Is_Cake_HashAlgorithm_Enum_SHA256()
            {
                // Given
                var hashAlgorithmbuilder = new HashAlgorithmBuilder();

                // When
                var result = hashAlgorithmbuilder.CreateHashAlgorithm(HashAlgorithm.SHA256);

                // Then
                Assert.IsAssignableFrom<System.Security.Cryptography.SHA256>(result);
            }

            [Fact]
            public void Should_Return_SHA512_Algorithm_Instance_If_Input_Is_Cake_HashAlgorithm_Enum_SHA512()
            {
                // Given
                var hashAlgorithmbuilder = new HashAlgorithmBuilder();

                // When
                var result = hashAlgorithmbuilder.CreateHashAlgorithm(HashAlgorithm.SHA512);

                // Then
                Assert.IsAssignableFrom<System.Security.Cryptography.SHA512>(result);
            }
        }
    }
}