// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tests.Fixtures.Build;
using Xunit;

namespace Cake.Common.Tests.Unit.Build.GoCD.Data
{
    public sealed class GoCDPipelineInfoTests
    {
        public sealed class ThePipelineNameProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new GoCDInfoFixture().CreatePipelineInfo();

                // When
                var result = info.Name;

                // Then
                Assert.Equal("main", result);
            }
        }

        public sealed class ThePipelineCounterProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new GoCDInfoFixture().CreatePipelineInfo();

                // When
                var result = info.Counter;

                // Then
                Assert.Equal(2345, result);
            }
        }

        public sealed class ThePipelineLabelProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new GoCDInfoFixture().CreatePipelineInfo();

                // When
                var result = info.Label;

                // Then
                Assert.Equal("1.1.2345", result);
            }
        }
    }
}