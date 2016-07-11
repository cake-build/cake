// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Cake.Common.Tests.Fixtures.Build;
using Xunit;

namespace Cake.Common.Tests.Unit.Build.Jenkins
{
    public sealed class JenkinsEnvironmentInfoTests
    {
        public sealed class TheWorkspaceProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new JenkinsInfoFixture().CreateEnvironmentInfo();

                // When
                var result = info.Workspace;

                // Then
                Assert.Equal("C:\\Jenkins\\build\\456", result);
            }
        }

        public sealed class TheExecutorNumberProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new JenkinsInfoFixture().CreateEnvironmentInfo();

                // When
                var result = info.ExecutorNumber;

                // Then
                Assert.Equal(2112, result);
            }
        }

        public sealed class TheJenkinsHomeProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new JenkinsInfoFixture().CreateEnvironmentInfo();

                // When
                var result = info.JenkinsHome;

                // Then
                Assert.Equal("C:\\Jenkins\\build", result);
            }
        }

        public sealed class TheJenkinsUrlProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new JenkinsInfoFixture().CreateEnvironmentInfo();

                // When
                var result = info.JenkinsUrl;

                // Then
                Assert.Equal("http://localhost:8080/jenkins/", result);
            }
        }
    }
}
