// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Common.Build.TravisCI;
using Cake.Common.Tests.Fixtures.Build;
using Cake.Testing.Extensions;
using Xunit;

namespace Cake.Common.Tests.Unit.Build.TravicCI
{
    public sealed class TravisCIProviderTests
    {
        public sealed class TravisCIWriteStartFold
        {
            [Fact]
            public void Should_Be_In_Correct_Format()
            {
                // Given
                var fixture = new TravisCIFixture();
                var travisCI = fixture.CreateTravisCIProvider();

                // When
                travisCI.WriteStartFold("cake");

                // Then
                Assert.Contains("travis_fold:start:cake\r", fixture.Log.AggregateLogMessages(), StringComparison.Ordinal);
            }
        }

        public sealed class TravisCIWriteEndFold
        {
            [Fact]
            public void Should_Be_In_Correct_Format()
            {
                // Given
                var fixture = new TravisCIFixture();
                var travisCI = fixture.CreateTravisCIProvider();

                // When
                travisCI.WriteEndFold("cake");

                // Then
                Assert.Contains("travis_fold:end:cake\r", fixture.Log.AggregateLogMessages(), StringComparison.Ordinal);
            }
        }

        public sealed class TravisFold
        {
            [Fact]
            public void Should_Write_On_Dispose()
            {
                // Given
                var fixture = new TravisCIFixture();
                var travisCI = fixture.CreateTravisCIProvider();

                // When
                using (var folded = travisCI.Fold("cake"))
                {
                }

                // Then
                Assert.Contains("travis_fold:start:cake\r", fixture.Log.AggregateLogMessages(), StringComparison.Ordinal);
                Assert.Contains("travis_fold:end:cake\r", fixture.Log.AggregateLogMessages(), StringComparison.Ordinal);
            }

            [Fact]
            public void Should_Write_Start_Fold()
            {
                // Given
                var fixture = new TravisCIFixture();
                var travisCI = fixture.CreateTravisCIProvider();

                // When
                using (var folded = travisCI.Fold("cake"))
                {
                }

                // Then
                Assert.Contains("travis_fold:start:cake\r", fixture.Log.AggregateLogMessages(), StringComparison.Ordinal);
            }

            [Fact]
            public void Should_Write_End_Fold()
            {
                // Given
                var fixture = new TravisCIFixture();
                var travisCI = fixture.CreateTravisCIProvider();

                // When
                using (var folded = travisCI.Fold("cake"))
                {
                }

                // Then
                Assert.Contains("travis_fold:end:cake\r", fixture.Log.AggregateLogMessages(), StringComparison.Ordinal);
            }
        }
    }
}
