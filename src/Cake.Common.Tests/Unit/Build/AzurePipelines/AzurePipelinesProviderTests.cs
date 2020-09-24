using Cake.Common.Build.AzurePipelines;
using Cake.Common.Tests.Fakes;
using Cake.Common.Tests.Fixtures.Build;
using Cake.Core;
using Cake.Testing;
using Xunit;

namespace Cake.Common.Tests.Unit.Build.AzurePipelines
{
    public sealed class AzurePipelinesProviderTests
    {
        public sealed class TheConstructor
        {
            [Fact]
            public void Should_Throw_If_Environment_Is_Null()
            {
                // Given, When
                var writer = new FakeBuildSystemServiceMessageWriter();
                var result = Record.Exception(() => new AzurePipelinesProvider(null, writer));

                // Then
                AssertEx.IsArgumentNullException(result, "environment");
            }

            [Fact]
            public void Should_Throw_If_Writer_Is_Null()
            {
                // Given, When
                var result = Record.Exception(() => new AzurePipelinesProvider(new FakeEnvironment(PlatformFamily.Unknown), null));

                // Then
                AssertEx.IsArgumentNullException(result, "writer");
            }
        }

        public sealed class TheIsRunningOnAzurePipelinesProperty
        {
            [Fact]
            public void Should_Return_True_If_Running_On_AzurePipelines()
            {
                // Given
                var fixture = new AzurePipelinesFixture();
                fixture.IsRunningOnAzurePipelines();
                var tfBuild = fixture.CreateAzurePipelinesService();

                // When
                var result = tfBuild.IsRunningOnAzurePipelines;

                // Then
                Assert.True(result);
            }

            [Fact]
            public void Should_Return_False_If_Not_Running_On_AzurePipelines()
            {
                // Given
                var fixture = new AzurePipelinesFixture();
                var tfBuild = fixture.CreateAzurePipelinesService();

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
                var fixture = new AzurePipelinesFixture();
                fixture.IsRunningOnAzurePipelinesHosted();
                var tfBuild = fixture.CreateAzurePipelinesService();

                // When
                var result = tfBuild.IsRunningOnAzurePipelinesHosted;

                // Then
                Assert.True(result);
            }

            [Fact]
            public void Should_Return_False_If_Not_Running_On_AzurePipelinesHosted()
            {
                // Given
                var fixture = new AzurePipelinesFixture();
                var tfBuild = fixture.CreateAzurePipelinesService();

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
                var fixture = new AzurePipelinesFixture();
                fixture.IsRunningOnAzurePipelinesHosted(agentName);
                var tfBuild = fixture.CreateAzurePipelinesService();

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
                var fixture = new AzurePipelinesFixture();
                var tfBuild = fixture.CreateAzurePipelinesService();

                // When
                var result = tfBuild.Environment;

                // Then
                Assert.NotNull(result);
            }
        }
    }
}
