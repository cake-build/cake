// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Cake.Common.Tests.Fixtures.Build;
using Xunit;

namespace Cake.Common.Tests.Unit.Build.TravicCI.Data
{
    public sealed class TravisBuildInfoTests
    {
        public sealed class TheBranchProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new TravisCIInfoFixture().CreateBuildInfo();

                // When
                var result = info.Branch;

                // Then
                Assert.Equal("master", result);
            }
        }

        public sealed class TheBuildDirectoryProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new TravisCIInfoFixture().CreateBuildInfo();

                // When
                var result = info.BuildDirectory;

                // Then
                Assert.Equal("/home/travis/build/", result);
            }
        }

        public sealed class TheBuildIdProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new TravisCIInfoFixture().CreateBuildInfo();

                // When
                var result = info.BuildId;

                // Then
                Assert.Equal("122134370", result);
            }
        }

        public sealed class TheBuildNumberProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new TravisCIInfoFixture().CreateBuildInfo();

                // When
                var result = info.BuildNumber;

                // Then
                Assert.Equal(934, result);
            }
        }

        public sealed class TheTestResultProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new TravisCIInfoFixture().CreateBuildInfo();

                // When
                var result = info.TestResult;

                // Then
                Assert.Equal(0, result);
            }
        }

        public sealed class TheTagProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new TravisCIInfoFixture().CreateBuildInfo();

                // When
                var result = info.Tag;

                // Then
                Assert.Equal("v0.10.0", result);
            }
        }
    }
}
