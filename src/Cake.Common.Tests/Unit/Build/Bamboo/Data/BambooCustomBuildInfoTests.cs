// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Cake.Common.Tests.Fixtures.Build;
using Xunit;

namespace Cake.Common.Tests.Unit.Build.Bamboo.Data
{
    public sealed class BambooCustomBuildInfoTests
    {

        public sealed class TheIsCustomBuildProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new BambooInfoFixture().CreateCustomBuildInfo();

                // When
                var result = info.IsCustomBuild;

                // Then
                Assert.Equal(true, result);
            }
        }

        public sealed class TheRevisonNameProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new BambooInfoFixture().CreateCustomBuildInfo();

                // When
                var result = info.RevisonName;

                // Then
                Assert.Equal("Cake with Iceing", result);
            }
        }
    }
}
