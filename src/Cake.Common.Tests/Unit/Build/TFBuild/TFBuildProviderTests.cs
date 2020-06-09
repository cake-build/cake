// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Build.TFBuild;
using Cake.Common.Tests.Fakes;
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
                var result = Record.Exception(() => new TFBuildProvider(null, new FakeBuildSystemServiceMessageWriter()));

                // Then
                AssertEx.IsArgumentNullException(result, "environment");
            }

            [Fact]
            public void Should_Throw_If_Writer_Is_Null()
            {
                // Given, When
                var result = Record.Exception(() => new TFBuildProvider(new FakeEnvironment(PlatformFamily.Unknown), null));

                // Then
                AssertEx.IsArgumentNullException(result, "writer");
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
                var tfBuild = fixture.CreateTFBuildService();

                // When
#pragma warning disable 618
                var result = tfBuild.IsRunningOnTFS;
#pragma warning restore 618

                // Then
                Assert.True(result);
            }

            [Fact]
            public void Should_Return_False_If_Not_Running_On_TFS()
            {
                // Given
                var fixture = new TFBuildFixture();
                var tfBuild = fixture.CreateTFBuildService();

                // When
#pragma warning disable 618
                var result = tfBuild.IsRunningOnTFS;
#pragma warning restore 618

                // Then
                Assert.False(result);
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
                var tfBuild = fixture.CreateTFBuildService();

                // When
#pragma warning disable 618
                var result = tfBuild.IsRunningOnVSTS;
#pragma warning restore 618

                // Then
                Assert.True(result);
            }

            [Fact]
            public void Should_Return_False_If_Not_Running_On_VSTS()
            {
                // Given
                var fixture = new TFBuildFixture();
                var tfBuild = fixture.CreateTFBuildService();

                // When
#pragma warning disable 618
                var result = tfBuild.IsRunningOnVSTS;
#pragma warning restore 618

                // Then
                Assert.False(result);
            }
        }

        public sealed class TheIsRunningOnAzurePipelinesProperty
        {
            [Fact]
            public void Should_Return_True_If_Running_On_AzurePipelines()
            {
                // Given
                var fixture = new TFBuildFixture();
                fixture.IsRunningOnAzurePipelines();
                var tfBuild = fixture.CreateTFBuildService();

                // When
                var result = tfBuild.IsRunningOnAzurePipelines;

                // Then
                Assert.True(result);
            }

            [Fact]
            public void Should_Return_False_If_Not_Running_On_AzurePipelines()
            {
                // Given
                var fixture = new TFBuildFixture();
                var tfBuild = fixture.CreateTFBuildService();

                // When
                var result = tfBuild.IsRunningOnAzurePipelines;

                // Then
                Assert.False(result);
            }
        }

        public sealed class TheIsRunningOnAzurePipelinesHostedProperty
        {
            [Fact]
            public void Should_Return_True_If_Running_On_AzurePipelinesHosted()
            {
                // Given
                var fixture = new TFBuildFixture();
                fixture.IsRunningOnAzurePipelinesHosted();
                var tfBuild = fixture.CreateTFBuildService();

                // When
                var result = tfBuild.IsRunningOnAzurePipelinesHosted;

                // Then
                Assert.True(result);
            }

            [Fact]
            public void Should_Return_False_If_Not_Running_On_AzurePipelinesHosted()
            {
                // Given
                var fixture = new TFBuildFixture();
                var tfBuild = fixture.CreateTFBuildService();

                // When
                var result = tfBuild.IsRunningOnAzurePipelinesHosted;

                // Then
                Assert.False(result);
            }

            [Theory]
            [InlineData("Hosted Agent 2")]
            [InlineData("Azure Pipelines 3")]
            public void Should_Return_True_If_Running_On_AzurePipelinesExtraAgent(string agentName)
            {
                // Given
                var fixture = new TFBuildFixture();
                fixture.IsRunningOnAzurePipelinesHosted(agentName);
                var tfBuild = fixture.CreateTFBuildService();

                // When
                var result = tfBuild.IsRunningOnAzurePipelinesHosted;

                // Then
                Assert.True(result);
            }
        }

        public sealed class TheEnvironmentProperty
        {
            [Fact]
            public void Should_Return_Non_Null_Reference()
            {
                // Given
                var fixture = new TFBuildFixture();
                var tfBuild = fixture.CreateTFBuildService();

                // When
                var result = tfBuild.Environment;

                // Then
                Assert.NotNull(result);
            }
        }
    }
}
