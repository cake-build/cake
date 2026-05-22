// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Build.Rwx;
using Cake.Common.Tests.Fixtures.Build;
using Cake.Core.IO;
using Cake.Testing;
using NSubstitute;
using Xunit;

namespace Cake.Common.Tests.Unit.Build.Rwx
{
    public sealed class RwxProviderTests
    {
        [Fact]
        public void Should_Throw_If_Environment_Is_Null()
        {
            // Given, When
            var result = Record.Exception(() => new RwxProvider(null, Substitute.For<IFileSystem>()));

            // Then
            AssertEx.IsArgumentNullException(result, "environment");
        }

        [Fact]
        public void Should_Throw_If_FileSystem_Is_Null()
        {
            // Given, When
            var result = Record.Exception(() => new RwxProvider(Substitute.For<Cake.Core.ICakeEnvironment>(), null));

            // Then
            AssertEx.IsArgumentNullException(result, "fileSystem");
        }

        public sealed class TheIsRunningOnRwxProperty
        {
            [Fact]
            public void Should_Return_True_When_RWX_Env_Var_Set()
            {
                // Given
                var fixture = new RwxFixture();
                fixture.IsRunningOnRwx();
                var provider = fixture.CreateRwxProvider();

                // When
                var result = provider.IsRunningOnRwx;

                // Then
                Assert.True(result);
            }

            [Fact]
            public void Should_Return_False_When_RWX_Env_Var_Not_Set()
            {
                // Given
                var fixture = new RwxFixture();
                var provider = fixture.CreateRwxProvider();

                // When
                var result = provider.IsRunningOnRwx;

                // Then
                Assert.False(result);
            }
        }
    }
}
