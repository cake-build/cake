// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Common.Tests.Fixtures.Build;
using Xunit;

namespace Cake.Common.Tests.Unit.Build.AzurePipelines.Data
{
    public sealed class AzurePipelinesTeamProjectInfoTests
    {
        public sealed class TheNameProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new AzurePipelinesInfoFixture().CreateTeamProjectInfo();

                // When
                var result = info.Name;

                // Then
                Assert.Equal("TeamProject", result);
            }
        }

        public sealed class TheIdProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new AzurePipelinesInfoFixture().CreateTeamProjectInfo();

                // When
                var result = info.Id;

                // Then
                Assert.Equal("D0A3B6B8-499B-4D4B-BD46-DB70C19E6D33", result);
            }
        }

        public sealed class TheCollectionUriProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new AzurePipelinesInfoFixture().CreateTeamProjectInfo();

                // When
                var result = info.CollectionUri;

                // Then
                var uri = new Uri("https://fabrikamfiber.visualstudio.com/");
                Assert.Equal(uri, result);
            }
        }
    }
}
