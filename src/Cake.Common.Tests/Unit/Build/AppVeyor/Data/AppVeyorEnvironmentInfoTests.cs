// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Cake.Common.Tests.Fixtures.Build;
using NSubstitute;
using Xunit;

namespace Cake.Common.Tests.Unit.Build.AppVeyor.Data
{
    public sealed class AppVeyorEnvironmentInfoTests
    {
        public sealed class TheApiUrlProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new AppVeyorInfoFixture().CreateEnvironmentInfo();

                // When
                var result = info.ApiUrl;

                // Then
                Assert.Equal("http://localhost:1029/", result);
            }
        }

        public sealed class TheConfigurationProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new AppVeyorInfoFixture().CreateEnvironmentInfo();

                // When
                var result = info.Configuration;

                // Then
                Assert.Equal("x86", result);
            }
        }

        public sealed class TheJobIdProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new AppVeyorInfoFixture().CreateEnvironmentInfo();

                // When
                var result = info.JobId;

                // Then
                Assert.Equal("d6qpdshbol69ucbq", result);
            }
        }

        public sealed class TheJobNameProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new AppVeyorInfoFixture().CreateEnvironmentInfo();

                // When
                var result = info.JobName;

                // Then
                Assert.Equal("Job1", result);
            }
        }

        public sealed class ThePlatformProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new AppVeyorInfoFixture().CreateEnvironmentInfo();

                // When
                var result = info.Platform;

                // Then
                Assert.Equal("Debug", result);
            }
        }

        public sealed class TheScheduledBuildProperty
        {
            [Theory]
            [InlineData("true", true)]
            [InlineData("True", true)]
            [InlineData("false", false)]
            [InlineData("False", false)]
            [InlineData("Yes", false)]
            public void Should_Return_Correct_Value(string value, bool expected)
            {
                // Given
                var fixture = new AppVeyorInfoFixture();
                fixture.Environment.GetEnvironmentVariable("APPVEYOR_SCHEDULED_BUILD").Returns(value);
                var info = fixture.CreateEnvironmentInfo();

                // When
                var result = info.ScheduledBuild;

                // Then
                Assert.Equal(expected, result);
            }
        }

        public sealed class TheBuildProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new AppVeyorInfoFixture().CreateEnvironmentInfo();

                // When
                var result = info.Build;

                // Then
                Assert.NotNull(result);
            }
        }

        public sealed class TheProjectProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new AppVeyorInfoFixture().CreateEnvironmentInfo();

                // When
                var result = info.Project;

                // Then
                Assert.NotNull(result);
            }
        }

        public sealed class ThePullRequestProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new AppVeyorInfoFixture().CreateEnvironmentInfo();

                // When
                var result = info.PullRequest;

                // Then
                Assert.NotNull(result);
            }
        }

        public sealed class TheRepositoryProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new AppVeyorInfoFixture().CreateEnvironmentInfo();

                // When
                var result = info.Repository;

                // Then
                Assert.NotNull(result);
            }
        }
    }
}
