// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Cake.Common.Tests.Fixtures.Build;
using Xunit;

namespace Cake.Common.Tests.Unit.Build.Jenkins.Data
{
    public sealed class JenkinsBuildInfoTests
    {
        public sealed class TheBuildNumberProperty
        {
             [Fact]
             public void Should_Return_Correct_Value()
             {
                 // Given
                 var info = new JenkinsInfoFixture().CreateBuildInfo();

                // When
                 var result = info.BuildNumber;

                // Then
                 Assert.Equal(456, result);
             }
        }

        public sealed class TheBuildDisplayNameProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new JenkinsInfoFixture().CreateBuildInfo();

                // When
                var result = info.BuildDisplayName;

                // Then
                Assert.Equal("#456", result);
            }
        }

        public sealed class TheBuildIdProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new JenkinsInfoFixture().CreateBuildInfo();

                // When
                var result = info.BuildId;

                // Then
                Assert.Equal("456", result);
            }
        }

        public sealed class TheBuildTagProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new JenkinsInfoFixture().CreateBuildInfo();

                // When
                var result = info.BuildTag;

                // Then
                Assert.Equal("jenkins-JOB1-456", result);
            }
        }

        public sealed class TheBuildUrlProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new JenkinsInfoFixture().CreateBuildInfo();

                // When
                var result = info.BuildUrl;

                // Then
                Assert.Equal("http://localhost:8080/jenkins/job/cake/456/", result);
            }
        }

    }
}
