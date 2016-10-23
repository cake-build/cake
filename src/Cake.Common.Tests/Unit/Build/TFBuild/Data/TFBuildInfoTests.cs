// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Common.Tests.Fixtures.Build;
using Xunit;

namespace Cake.Common.Tests.Unit.Build.TFBuild.Data
{
    public sealed class TFBuildInfoTests
    {
        public sealed class TheIdProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new TFBuildInfoFixture().CreateBuildInfo();

                // When
                var result = info.Id;

                // Then
                Assert.Equal(100234, result);
            }
        }

        public sealed class TheNumberProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new TFBuildInfoFixture().CreateBuildInfo();

                // When
                var result = info.Number;

                // Then
                Assert.Equal("Build-20160927.1", result);
            }
        }

        public sealed class TheUriProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new TFBuildInfoFixture().CreateBuildInfo();

                // When
                var result = info.Uri;

                // Then
                var uri = new Uri("vstfs:///Build/Build/1430");
                Assert.Equal(uri, result);
            }
        }

        public sealed class TheQueuedByProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new TFBuildInfoFixture().CreateBuildInfo();

                // When
                var result = info.QueuedBy;

                // Then
                Assert.Equal(@"[DefaultCollection]\Project Collection Service Accounts", result);
            }
        }

        public sealed class TheRequestedForProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new TFBuildInfoFixture().CreateBuildInfo();

                // When
                var result = info.RequestedFor;

                // Then
                Assert.Equal("Alistair Chapman", result);
            }
        }

        public sealed class TheRequestedForEmailProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new TFBuildInfoFixture().CreateBuildInfo();

                // When
                var result = info.RequestedForEmail;

                // Then
                Assert.Equal("author@mail.com", result);
            }
        }
    }
}
