// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tests.Fixtures.Build;
using Xunit;

namespace Cake.Common.Tests.Unit.Build.Rwx.Data
{
    public sealed class RwxRunInfoTests
    {
        public sealed class TheIdProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new RwxInfoFixture().CreateRunInfo();

                // When
                var result = info.Id;

                // Then
                Assert.Equal("01J9ABCDEFG", result);
            }
        }

        public sealed class TheTitleProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new RwxInfoFixture().CreateRunInfo();

                // When
                var result = info.Title;

                // Then
                Assert.Equal("Build and test", result);
            }
        }

        public sealed class TheUrlProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new RwxInfoFixture().CreateRunInfo();

                // When
                var result = info.Url;

                // Then
                Assert.Equal("https://cloud.rwx.com/runs/01J9ABCDEFG", result);
            }
        }
    }
}
