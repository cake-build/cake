﻿using Cake.Common.Tests.Fixtures.Build;
using Xunit;

namespace Cake.Common.Tests.Unit.Build.TravicCI
{
    public sealed class TravisCIEnvironmentInfoTests
    {
        public sealed class TheCIProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new TravisCIInfoFixture().CreateEnvironmentInfo();

                // When
                var result = info.CI;

                // Then
                Assert.True(result);
            }
        }

        public sealed class TheHomeProperty
        {
            [Fact]
            public void Should_Return_Correct_Values()
            {
                // Given
                var info = new TravisCIInfoFixture().CreateEnvironmentInfo();

                // When
                var result = info.Home;

                // Then
                Assert.Equal("/home/travis", result);
            }
        }

        public sealed class TheTravisProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new TravisCIInfoFixture().CreateEnvironmentInfo();

                // When
                var result = info.Travis;

                // Then
                Assert.True(result);
            }
        }
    }
}
