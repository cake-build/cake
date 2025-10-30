// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Build.WoodpeckerCI;
using Cake.Common.Tests.Fixtures.Build;
using Cake.Core;
using Cake.Core.IO;
using NSubstitute;
using Xunit;

namespace Cake.Common.Tests.Unit.Build.WoodpeckerCI
{
    public sealed class WoodpeckerCIProviderTests
    {
        public sealed class TheIsRunningOnWoodpeckerCIProperty
        {
            [Fact]
            public void Should_Return_True_When_CI_Environment_Variable_Is_Set_To_Woodpecker()
            {
                // Given
                var fixture = new WoodpeckerCIInfoFixture();
                var provider = new WoodpeckerCIProvider(fixture.Environment, Substitute.For<IFileSystem>());

                // When
                var result = provider.IsRunningOnWoodpeckerCI;

                // Then
                Assert.True(result);
            }

            [Fact]
            public void Should_Return_False_When_CI_Environment_Variable_Is_Not_Set()
            {
                // Given
                var environment = Substitute.For<ICakeEnvironment>();
                environment.GetEnvironmentVariable("CI").Returns((string)null);
                var provider = new WoodpeckerCIProvider(environment, Substitute.For<IFileSystem>());

                // When
                var result = provider.IsRunningOnWoodpeckerCI;

                // Then
                Assert.False(result);
            }

            [Fact]
            public void Should_Return_False_When_CI_Environment_Variable_Is_Set_To_Something_Else()
            {
                // Given
                var environment = Substitute.For<ICakeEnvironment>();
                environment.GetEnvironmentVariable("CI").Returns("github");
                var provider = new WoodpeckerCIProvider(environment, Substitute.For<IFileSystem>());

                // When
                var result = provider.IsRunningOnWoodpeckerCI;

                // Then
                Assert.False(result);
            }
        }

        public sealed class TheEnvironmentProperty
        {
            [Fact]
            public void Should_Return_Non_Null_Environment()
            {
                // Given
                var fixture = new WoodpeckerCIInfoFixture();
                var provider = new WoodpeckerCIProvider(fixture.Environment, Substitute.For<IFileSystem>());

                // When
                var result = provider.Environment;

                // Then
                Assert.NotNull(result);
            }
        }

        public sealed class TheCommandsProperty
        {
            [Fact]
            public void Should_Return_Non_Null_Commands()
            {
                // Given
                var fixture = new WoodpeckerCIInfoFixture();
                var provider = new WoodpeckerCIProvider(fixture.Environment, Substitute.For<IFileSystem>());

                // When
                var result = provider.Commands;

                // Then
                Assert.NotNull(result);
            }
        }
    }
}
