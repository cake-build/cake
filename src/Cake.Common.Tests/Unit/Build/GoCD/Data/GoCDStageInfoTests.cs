// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tests.Fixtures.Build;
using Xunit;

namespace Cake.Common.Tests.Unit.Build.GoCD.Data
{
    public sealed class GoCDStageInfoTests
    {
        public sealed class TheCounterProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new GoCDInfoFixture().CreateStageInfo();

                // When
                var result = info.Counter;

                // Then
                Assert.Equal(1, result);
            }
        }

        public sealed class TheStageNameProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new GoCDInfoFixture().CreateStageInfo();

                // When
                var result = info.Name;

                // Then
                Assert.Equal("dev", result);
            }
        }
    }
}