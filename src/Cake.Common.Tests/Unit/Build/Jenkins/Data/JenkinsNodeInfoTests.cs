// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Cake.Common.Tests.Fixtures.Build;
using Xunit;

namespace Cake.Common.Tests.Unit.Build.Jenkins.Data
{
    public sealed class JenkinsNodeInfoTests
    {
        public sealed class TheNodeNameProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new JenkinsInfoFixture().CreateNodeInfo();

                // When
                var result = info.NodeName;

                // Then
                Assert.Equal("master", result);
            }
        }

        public sealed class TheNodeLabelsProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new JenkinsInfoFixture().CreateNodeInfo();

                // When
                var result = info.NodeLabels;

                // Then
                Assert.Equal("cake development build", result);
            }
        }
    }
}
