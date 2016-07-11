// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Cake.Common.Tests.Fixtures.Build;
using Xunit;

namespace Cake.Common.Tests.Unit.Build.Jenkins.Data
{
    public sealed class JenkinsJobInfoTests
    {
        public sealed class TheJobNameProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new JenkinsInfoFixture().CreateJobInfo();

                // When
                var result = info.JobName;

                // Then
                Assert.Equal("JOB1", result);
            }
        }

        public sealed class TheJobUrlProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new JenkinsInfoFixture().CreateJobInfo();

                // When
                var result = info.JobUrl;

                // Then
                Assert.Equal("http://localhost:8080/jenkins/job/cake/", result);
            }
        }
    }
}
