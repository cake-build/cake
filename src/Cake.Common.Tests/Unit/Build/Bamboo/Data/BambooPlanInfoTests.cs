// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Cake.Common.Tests.Fixtures.Build;
using Xunit;

namespace Cake.Common.Tests.Unit.Build.Bamboo.Data
{
    public sealed class BambooPlanInfoTests
    {
        public sealed class ThePlanNameProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new BambooInfoFixture().CreatePlanInfo();

                // When
                var result = info.PlanName;

                // Then
                Assert.Equal("cake-bamboo - dev", result);
            }
        }

        public sealed class ThePlanKeyProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new BambooInfoFixture().CreatePlanInfo();

                // When
                var result = info.PlanKey;

                // Then
                Assert.Equal("CAKE-CAKE", result);
            }
        }

        public sealed class TheShortJobKeyProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new BambooInfoFixture().CreatePlanInfo();

                // When
                var result = info.ShortJobKey;

                // Then
                Assert.Equal("JOB1", result);
            }
        }

        public sealed class TheShortJobNameProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new BambooInfoFixture().CreatePlanInfo();

                // When
                var result = info.ShortJobName;

                // Then
                Assert.Equal("Build Cake", result);
            }
        }

        public sealed class TheShortPlanKeyProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new BambooInfoFixture().CreatePlanInfo();

                // When
                var result = info.ShortPlanKey;

                // Then
                Assert.Equal("CAKE", result);
            }
        }

        public sealed class TheShortPlanNameProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new BambooInfoFixture().CreatePlanInfo();

                // When
                var result = info.ShortPlanName;

                // Then
                Assert.Equal("Cake", result);
            }
        }

    }

}
