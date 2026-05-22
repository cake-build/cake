// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tests.Fixtures.Build;
using NSubstitute;
using Xunit;

namespace Cake.Common.Tests.Unit.Build.Rwx.Data
{
    public sealed class RwxRuntimeInfoTests
    {
        public sealed class TheValuesPathProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new RwxInfoFixture().CreateRuntimeInfo();

                // When
                var result = info.ValuesPath;

                // Then
                Assert.Equal("/rwx/values", result.FullPath);
            }

            [Fact]
            public void Should_Return_Null_When_Env_Var_Missing()
            {
                // Given
                var fixture = new RwxInfoFixture();
                fixture.Environment.GetEnvironmentVariable("RWX_VALUES").Returns(null as string);
                var info = fixture.CreateRuntimeInfo();

                // When
                var result = info.ValuesPath;

                // Then
                Assert.Null(result);
            }
        }

        public sealed class TheArtifactsPathProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new RwxInfoFixture().CreateRuntimeInfo();

                // When
                var result = info.ArtifactsPath;

                // Then
                Assert.Equal("/rwx/artifacts", result.FullPath);
            }

            [Fact]
            public void Should_Return_Null_When_Env_Var_Missing()
            {
                // Given
                var fixture = new RwxInfoFixture();
                fixture.Environment.GetEnvironmentVariable("RWX_ARTIFACTS").Returns(null as string);
                var info = fixture.CreateRuntimeInfo();

                // When
                var result = info.ArtifactsPath;

                // Then
                Assert.Null(result);
            }
        }

        public sealed class TheEnvPathProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new RwxInfoFixture().CreateRuntimeInfo();

                // When
                var result = info.EnvPath;

                // Then
                Assert.Equal("/rwx/env", result.FullPath);
            }

            [Fact]
            public void Should_Return_Null_When_Env_Var_Missing()
            {
                // Given
                var fixture = new RwxInfoFixture();
                fixture.Environment.GetEnvironmentVariable("RWX_ENV").Returns(null as string);
                var info = fixture.CreateRuntimeInfo();

                // When
                var result = info.EnvPath;

                // Then
                Assert.Null(result);
            }
        }

        public sealed class TheIsRuntimeAvailableProperty
        {
            [Fact]
            public void Should_Return_True_When_All_Env_Vars_Set()
            {
                // Given
                var info = new RwxInfoFixture().CreateRuntimeInfo();

                // When
                var result = info.IsRuntimeAvailable;

                // Then
                Assert.True(result);
            }

            [Fact]
            public void Should_Return_False_When_Values_Missing()
            {
                // Given
                var fixture = new RwxInfoFixture();
                fixture.Environment.GetEnvironmentVariable("RWX_VALUES").Returns(null as string);
                var info = fixture.CreateRuntimeInfo();

                // When
                var result = info.IsRuntimeAvailable;

                // Then
                Assert.False(result);
            }

            [Fact]
            public void Should_Return_False_When_Artifacts_Missing()
            {
                // Given
                var fixture = new RwxInfoFixture();
                fixture.Environment.GetEnvironmentVariable("RWX_ARTIFACTS").Returns(null as string);
                var info = fixture.CreateRuntimeInfo();

                // When
                var result = info.IsRuntimeAvailable;

                // Then
                Assert.False(result);
            }

            [Fact]
            public void Should_Return_False_When_Env_Missing()
            {
                // Given
                var fixture = new RwxInfoFixture();
                fixture.Environment.GetEnvironmentVariable("RWX_ENV").Returns(null as string);
                var info = fixture.CreateRuntimeInfo();

                // When
                var result = info.IsRuntimeAvailable;

                // Then
                Assert.False(result);
            }
        }
    }
}
