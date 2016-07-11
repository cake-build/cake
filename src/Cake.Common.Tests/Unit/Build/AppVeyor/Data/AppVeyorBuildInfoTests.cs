// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Cake.Common.Tests.Fixtures.Build;
using Xunit;

namespace Cake.Common.Tests.Unit.Build.AppVeyor.Data
{
    public sealed class AppVeyorBuildInfoTests
    {
        public sealed class TheFolderProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new AppVeyorInfoFixture().CreateBuildInfo();

                // When
                var result = info.Folder;

                // Then
                Assert.Equal(@"C:\projects\cake", result);
            }
        }

        public sealed class TheIdProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new AppVeyorInfoFixture().CreateBuildInfo();

                // When
                var result = info.Id;

                // Then
                Assert.Equal("378354", result);
            }
        }

        public sealed class TheNumberProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new AppVeyorInfoFixture().CreateBuildInfo();

                // When
                var result = info.Number;

                // Then
                Assert.Equal(2, result);
            }
        }

        public sealed class TheVersionProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new AppVeyorInfoFixture().CreateBuildInfo();

                // When
                var result = info.Version;

                // Then
                Assert.Equal("1.0.2", result);
            }
        }
    }
}
