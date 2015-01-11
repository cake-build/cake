using Cake.Common.Tests.Fixtures;
using NSubstitute;
using Xunit;

namespace Cake.Common.Tests.Unit.Build.AppVeyor.Data
{
    public sealed class AppVeyorPullRequestInfoTests
    {
        public sealed class TheIsPullRequestProperty
        {
            [Theory]
            [InlineData("1", true)]
            [InlineData("0", false)]
            public void Should_Return_Correct_Value(string value, bool expected)
            {
                // Given
                var fixture = new AppVeyorInfoFixture();
                fixture.Environment.GetEnvironmentVariable("APPVEYOR_PULL_REQUEST_NUMBER").Returns(value);
                var info = fixture.CreatePullRequestInfo();

                // When
                var result = info.IsPullRequest;

                // Then
                Assert.Equal(expected, result);
            }
        }

        public sealed class TheNumberProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new AppVeyorInfoFixture().CreatePullRequestInfo();

                // When
                var result = info.Number;

                // Then
                Assert.Equal(1, result);
            }
        }

        public sealed class TheTitleProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new AppVeyorInfoFixture().CreatePullRequestInfo();

                // When
                var result = info.Title;

                // Then
                Assert.Equal("Changes stuff.", result);
            }
        }
    }
}
