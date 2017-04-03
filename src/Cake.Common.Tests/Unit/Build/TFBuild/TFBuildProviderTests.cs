// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Build.TFBuild;
using Cake.Common.Tests.Fixtures.Build;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Testing;
using Xunit;

namespace Cake.Common.Tests.Unit.Build.TFBuild
{
    public sealed class TFBuildProviderTests
    {
        public sealed class TheConstructor
        {
            [Fact]
            public void Should_Throw_If_Environment_Is_Null()
            {
                // Given, When
                var result = Record.Exception(() => new TFBuildProvider(null, new NullLog()));

                // Then
                AssertEx.IsArgumentNullException(result, "environment");
            }

            [Fact]
            public void Should_Throw_If_Log_Is_Null()
            {
                // Given, When
                var result = Record.Exception(() => new TFBuildProvider(new FakeEnvironment(PlatformFamily.Unknown), null));

                // Then
                AssertEx.IsArgumentNullException(result, "log");
            }
        }

        public sealed class TheIsRunningOnVSTSProperty
        {
            [Fact]
            public void Should_Return_True_If_Running_On_VSTS()
            {
                // Given
                var fixture = new TFBuildFixture();
                fixture.IsRunningOnVSTS();
                var vsts = fixture.CreateTFBuildService();

                // When
                var result = vsts.IsRunningOnVSTS;

                // Then
                Assert.True(result);
            }

            [Fact]
            public void Should_Return_False_If_Not_Running_On_VSTS()
            {
                // Given
                var fixture = new TFBuildFixture();
                var vsts = fixture.CreateTFBuildService();

                // When
                var result = vsts.IsRunningOnVSTS;

                // Then
                Assert.False(result);
            }
        }

        public sealed class TheIsRunningOnTFSProperty
        {
            [Fact]
            public void Should_Return_True_If_Running_On_TFS()
            {
                // Given
                var fixture = new TFBuildFixture();
                fixture.IsRunningOnTFS();
                var vsts = fixture.CreateTFBuildService();

                // When
                var result = vsts.IsRunningOnTFS;

                // Then
                Assert.True(result);
            }

            [Fact]
            public void Should_Return_False_If_Not_Running_On_TFS()
            {
                // Given
                var fixture = new TFBuildFixture();
                var vsts = fixture.CreateTFBuildService();

                // When
                var result = vsts.IsRunningOnTFS;

                // Then
                Assert.False(result);
            }
        }

        public sealed class TheEnvironmentProperty
        {
            [Fact]
            public void Should_Return_Non_Null_Reference()
            {
                // Given
                var fixture = new TFBuildFixture();
                var vsts = fixture.CreateTFBuildService();

                // When
                var result = vsts.Environment;

                // Then
                Assert.NotNull(result);
            }
        }
    }
}
