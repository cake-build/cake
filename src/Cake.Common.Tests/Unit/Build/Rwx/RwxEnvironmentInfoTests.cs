// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tests.Fixtures.Build;
using Xunit;

namespace Cake.Common.Tests.Unit.Build.Rwx
{
    public sealed class RwxEnvironmentInfoTests
    {
        public sealed class TheCIProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new RwxInfoFixture().CreateEnvironmentInfo();

                // When
                var result = info.CI;

                // Then
                Assert.True(result);
            }
        }

        public sealed class TheRwxProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new RwxInfoFixture().CreateEnvironmentInfo();

                // When
                var result = info.Rwx;

                // Then
                Assert.True(result);
            }
        }

        public sealed class TheRuntimeProperty
        {
            [Fact]
            public void Should_Be_Populated()
            {
                // Given
                var info = new RwxInfoFixture().CreateEnvironmentInfo();

                // When, Then
                Assert.NotNull(info.Runtime);
                Assert.Equal("/rwx/values", info.Runtime.ValuesPath.FullPath);
                Assert.Equal("/rwx/artifacts", info.Runtime.ArtifactsPath.FullPath);
            }
        }
    }
}
