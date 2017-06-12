// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tests.Fixtures.Tools;
using Xunit;

namespace Cake.Common.Tests.Unit.Tools.SignTool
{
    public sealed class SignToolResolverTests
    {
        public sealed class TheConstructor
        {
            [Fact]
            public void Should_Throw_If_File_System_Is_Null()
            {
                // Given
                var fixture = new SignToolResolverFixture();
                fixture.FileSystem = null;

                // When
                var result = Record.Exception(() => fixture.Resolve());

                // Then
                AssertEx.IsArgumentNullException(result, "fileSystem");
            }

            [Fact]
            public void Should_Throw_If_Environment_Is_Null()
            {
                // Given
                var fixture = new SignToolResolverFixture();
                fixture.Environment = null;

                // When
                var result = Record.Exception(() => fixture.Resolve());

                // Then
                AssertEx.IsArgumentNullException(result, "environment");
            }

            [Fact]
            public void Should_Throw_If_Registry_Is_Null()
            {
                // Given
                var fixture = new SignToolResolverFixture();
                fixture.Registry = null;

                // When
                var result = Record.Exception(() => fixture.Resolve());

                // Then
                AssertEx.IsArgumentNullException(result, "registry");
            }
        }

        public sealed class TheResolveMethod
        {
            [Theory]
            [InlineData(true)]
            [InlineData(false)]
            public void Should_Return_From_Disc_If_Found(bool is64Bit)
            {
                // Given
                var fixture = new SignToolResolverFixture(is64Bit);
                fixture.GivenThatToolExistInKnownPath();

                // When
                var result = fixture.Resolve();

                // Then
                Assert.NotNull(result);
            }

            [Fact]
            public void Should_Return_From_Registry_If_Found()
            {
                // Given
                var fixture = new SignToolResolverFixture();
                fixture.GivenThatToolHasRegistryKeyMicrosoftSdks();

                // When
                var result = fixture.Resolve();

                // Then
                Assert.NotNull(result);
            }

            [Theory]
            [InlineData(true)]
            [InlineData(false)]
            public void Should_Return_From_Registry_If_Windows_Kits_Found(bool is64Bit)
            {
                // Given
                var fixture = new SignToolResolverFixture(is64Bit);
                fixture.GivenThatToolHasRegistryKeyWindowsKits();

                // When
                var result = fixture.Resolve();

                // Then
                Assert.NotNull(result);
            }

            [Fact]
            public void Should_Throw_If_Not_Found_On_Disc_And_SDK_Registry_Path_Cannot_Be_Resolved()
            {
                // Given
                var fixture = new SignToolResolverFixture();
                fixture.GivenThatNoSdkRegistryKeyExist();

                // When
                var result = Record.Exception(() => fixture.Resolve());

                // Then
                AssertEx.IsCakeException(result, "Failed to find signtool.exe.");
            }

            [Fact]
            public void Should_Throw_If_SignTool_Cannot_Be_Resolved()
            {
                // Given
                var fixture = new SignToolResolverFixture();

                // When
                var result = Record.Exception(() => fixture.Resolve());

                // Then
                AssertEx.IsCakeException(result, "Failed to find signtool.exe.");
            }
        }
    }
}