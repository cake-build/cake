﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tests.Fixtures.Build;
using Xunit;

namespace Cake.Common.Tests.Unit.Build.AzurePipelines.Data
{
    public sealed class AzurePipelinesDefinitionInfoTests
    {
        public sealed class TheIdProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new AzurePipelinesInfoFixture().CreateDefinitionInfo();

                // When
                var result = info.Id;

                // Then
                Assert.Equal(1855, result);
            }
        }

        public sealed class TheNameProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new AzurePipelinesInfoFixture().CreateDefinitionInfo();

                // When
                var result = info.Name;

                // Then
                Assert.Equal("Cake-CI", result);
            }
        }

        public sealed class TheVersionProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new AzurePipelinesInfoFixture().CreateDefinitionInfo();

                // When
                var result = info.Version;

                // Then
                Assert.Equal(47, result);
            }
        }
    }
}
