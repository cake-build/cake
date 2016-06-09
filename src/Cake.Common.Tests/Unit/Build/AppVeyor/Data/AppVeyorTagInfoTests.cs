// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Cake.Common.Tests.Fixtures.Build;
using NSubstitute;
using Xunit;

namespace Cake.Common.Tests.Unit.Build.AppVeyor.Data
{
    public sealed class AppVeyorTagInfoTests
    {
        public sealed class TheIsTagProperty
        {
            [Theory]
            [InlineData("true", true)]
            [InlineData("True", true)]
            [InlineData("false", false)]
            [InlineData("False", false)]
            [InlineData("Yes", false)]
            public void Should_Return_Correct_Value(string value, bool expected)
            {
                // Given
                var fixture = new AppVeyorInfoFixture();
                fixture.Environment.GetEnvironmentVariable("APPVEYOR_REPO_TAG").Returns(value);
                var info = fixture.CreateTagInfo();

                // When
                var result = info.IsTag;

                // Then
                Assert.Equal(expected, result);
            }
        }

        public sealed class TheNameProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new AppVeyorInfoFixture().CreateTagInfo();

                // When
                var result = info.Name;

                // Then
                Assert.Equal("v1.0.25", result);
            }
        }
    }
}
