// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tests.Fixtures.Build;
using Xunit;

namespace Cake.Common.Tests.Unit.Build.GoCD.Data
{
    public sealed class GoCDEnvironmentInfoTests
    {
        public sealed class TheGoCDUrlProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new GoCDInfoFixture().CreateEnvironmentInfo();

                // When
                var result = info.GoCDUrl;

                // Then
                Assert.Equal("https://127.0.0.1:8154/go", result);
            }
        }

        public sealed class TheEnvironmentNameProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new GoCDInfoFixture().CreateEnvironmentInfo();

                // When
                var result = info.EnvironmentName;

                // Then
                Assert.Equal("Development", result);
            }
        }

        public sealed class TheJobNameProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new GoCDInfoFixture().CreateEnvironmentInfo();

                // When
                var result = info.JobName;

                // Then
                Assert.Equal("linux-firefox", result);
            }
        }

        public sealed class TheUserProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new GoCDInfoFixture().CreateEnvironmentInfo();

                // When
                var result = info.User;

                // Then
                Assert.Equal("changes", result);
            }
        }
    }
}