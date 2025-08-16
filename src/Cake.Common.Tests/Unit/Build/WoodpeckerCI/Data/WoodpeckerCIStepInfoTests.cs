// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Common.Tests.Fixtures.Build;
using Xunit;

namespace Cake.Common.Tests.Unit.Build.WoodpeckerCI.Data
{
    public sealed class WoodpeckerCIStepInfoTests
    {
        public sealed class TheNameProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new WoodpeckerCIInfoFixture().CreateStepInfo();

                // When
                var result = info.Name;

                // Then
                Assert.Equal("build package", result);
            }
        }

        public sealed class TheNumberProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new WoodpeckerCIInfoFixture().CreateStepInfo();

                // When
                var result = info.Number;

                // Then
                Assert.Equal(0, result);
            }
        }

        public sealed class TheStartedProperty
        {
            [Fact]
            public void Should_Return_Correct_DateTimeOffset_For_Valid_Timestamp()
            {
                // Given
                var info = new WoodpeckerCIInfoFixture().CreateStepInfo();

                // When
                var result = info.Started;

                // Then
                Assert.NotEqual(DateTimeOffset.MinValue, result);
                Assert.Equal(DateTimeOffset.FromUnixTimeSeconds(1722617519), result);
            }

            [Fact]
            public void Should_Return_MinValue_For_Invalid_Timestamp()
            {
                // Given
                var fixture = new WoodpeckerCIInfoFixture();
                fixture.SetEnvironmentVariable("CI_STEP_STARTED", "0");
                var info = fixture.CreateStepInfo();

                // When
                var result = info.Started;

                // Then
                Assert.Equal(DateTimeOffset.MinValue, result);
            }
        }

        public sealed class TheUrlProperty
        {
            [Fact]
            public void Should_Return_Correct_Uri_For_Valid_Url()
            {
                // Given
                var info = new WoodpeckerCIInfoFixture().CreateStepInfo();

                // When
                var result = info.Url;

                // Then
                Assert.NotNull(result);
                Assert.Equal("https://ci.example.com/repos/7/pipeline/8", result.ToString());
            }

            [Fact]
            public void Should_Return_Null_For_Invalid_Url()
            {
                // Given
                var fixture = new WoodpeckerCIInfoFixture();
                fixture.SetEnvironmentVariable("CI_STEP_URL", "not-a-valid-url");
                var info = fixture.CreateStepInfo();

                // When
                var result = info.Url;

                // Then
                Assert.Null(result);
            }
        }
    }
}
