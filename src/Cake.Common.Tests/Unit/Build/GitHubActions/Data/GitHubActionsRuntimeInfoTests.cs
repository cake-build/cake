using Cake.Common.Tests.Fixtures.Build;
using Xunit;

namespace Cake.Common.Tests.Unit.Build.GitHubActions.Data
{
    public sealed class GitHubActionsRuntimeInfoTests
    {
        public sealed class TheIsRuntimeAvailableProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new GitHubActionsInfoFixture().CreateRuntimeInfo();

                // When
                var result = info.IsRuntimeAvailable;

                // Then
                Assert.Equal(true, result);
            }
        }

        public sealed class TheTokenProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new GitHubActionsInfoFixture().CreateRuntimeInfo();

                // When
                var result = info.Token;

                // Then
                Assert.Equal("zht1j5NeW2T5ZsOxncX4CUEiWYhD4ZRwoDghkARk", result);
            }
        }

        public sealed class TheUrlProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new GitHubActionsInfoFixture().CreateRuntimeInfo();

                // When
                var result = info.Url;

                // Then
                Assert.Equal("https://pipelines.actions.githubusercontent.com/ip0FyYnZXxdEOcOwPHkRsZJd2x6G5XoT486UsAb0/", result);
            }
        }

        public sealed class TheEnvPathProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new GitHubActionsInfoFixture().CreateRuntimeInfo();

                // When
                var result = info.EnvPath.FullPath;

                // Then
                Assert.Equal("/opt/github.env", result);
            }
        }

        public sealed class TheSystemPathProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new GitHubActionsInfoFixture().CreateRuntimeInfo();

                // When
                var result = info.SystemPath.FullPath;

                // Then
                Assert.Equal("/opt/github.path", result);
            }
        }
    }
}