// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Cake.Common.Tests.Fixtures.Build;
using Xunit;

namespace Cake.Common.Tests.Unit.Build.Bamboo.Data
{
    public sealed class BambooBuildInfoTests
    {
        public sealed class TheFolderProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new BambooInfoFixture().CreateBuildInfo();

                // When
                var result = info.Folder;

                // Then
                Assert.Equal(@"C:\build\CAKE-CAKE-JOB1", result);
            }
        }

        public sealed class TheNumberProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new BambooInfoFixture().CreateBuildInfo();

                // When
                var result = info.Number;

                // Then
                Assert.Equal(28, result);
            }
        }

        public sealed class TheBuildKeyProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new BambooInfoFixture().CreateBuildInfo();

                // When
                var result = info.BuildKey;

                // Then
                Assert.Equal("CAKE-CAKE-JOB1", result);
            }
        }

        public sealed class TheResultKeyProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new BambooInfoFixture().CreateBuildInfo();

                // When
                var result = info.ResultKey;

                // Then
                Assert.Equal("CAKE-CAKE-JOB1-28", result);
            }
        }

        public sealed class TheResultUrlProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new BambooInfoFixture().CreateBuildInfo();

                // When
                var result = info.ResultsUrl;

                // Then
                Assert.Equal("https://cakebuild.atlassian.net/builds/browse/CAKE-CAKE-JOB1-28", result);
            }
        }

        public sealed class TheBuildTimestampProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new BambooInfoFixture().CreateBuildInfo();

                // When
                var result = info.BuildTimestamp;

                // Then
                Assert.Equal("2015-12-15T22:53:37.847+01:00", result);
            }
        }
    }
}
