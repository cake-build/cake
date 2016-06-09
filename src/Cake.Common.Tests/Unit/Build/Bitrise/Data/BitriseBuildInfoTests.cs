// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Cake.Common.Tests.Fixtures.Build;
using Xunit;

namespace Cake.Common.Tests.Unit.Build.Bitrise.Data
{
    public sealed class BitriseBuildInfoTests
    {
        public sealed class TheBuildNumberProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new BitriseInfoFixture().CreateBuildInfo();

                // When
                var result = info.BuildNumber;

                //Then
                Assert.Equal("456", result);
            }
        }

        public sealed class TheBuildUrlProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new BitriseInfoFixture().CreateBuildInfo();

                // When
                var result = info.BuildUrl;

                //Then
                Assert.Equal("https://www.bitrise.io/build/e794ed892f3a59dd", result);
            }
        }

        public sealed class TheBuildSlugProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new BitriseInfoFixture().CreateBuildInfo();

                // When
                var result = info.BuildSlug;

                //Then
                Assert.Equal("e794ed892f3a59dd", result);
            }
        }

        public sealed class TheBuildTriggerTimestampProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new BitriseInfoFixture().CreateBuildInfo();

                // When
                var result = info.BuildTriggerTimestamp;

                //Then
                Assert.Equal("2016-03-12 23:49:26", result);
            }
        }

        [Fact]
        public void Should_Return_Correct_Value()
        {
            // Given
            var info = new BitriseInfoFixture().CreateBuildInfo();

            // When
            var result = info.BuildStatus;

            //Then
            Assert.True(result);
        }
    }
}
