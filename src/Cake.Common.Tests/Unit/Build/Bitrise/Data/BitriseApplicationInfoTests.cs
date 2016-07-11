// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Cake.Common.Tests.Fixtures.Build;
using Xunit;

namespace Cake.Common.Tests.Unit.Build.Bitrise.Data
{
    public sealed class BitriseApplicationInfoTests
    {
        public sealed class TheApplicationTitleProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new BitriseInfoFixture().CreateApplicationInfo();

                // When
                var result = info.ApplicationTitle;

                //Then
                Assert.Equal("CAKE-EXE", result);
            }
        }

        public sealed class TheApplicationUrlProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new BitriseInfoFixture().CreateApplicationInfo();

                // When
                var result = info.ApplicationUrl;

                //Then
                Assert.Equal("https://www.bitrise.io/app/089v339k300ba3cd", result);
            }
        }

        public sealed class TheApplicationSlugProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new BitriseInfoFixture().CreateApplicationInfo();

                // When
                var result = info.AppSlug;

                //Then
                Assert.Equal("089v339k300ba3cd", result);
            }
        }
    }
}
