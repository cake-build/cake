// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Build.GoCD;
using Cake.Common.Tests.Fixtures.Build;
using Xunit;

namespace Cake.Common.Tests.Unit.Build.GoCD
{
    public sealed class GoCDProviderTests
    {
        public sealed class TheConstructor
        {
            [Fact]
            public void Should_Throw_If_Environment_Is_Null()
            {
                // Given, When
                var result = Record.Exception(() => new GoCDProvider(null));

                // Then
                Assert.IsArgumentNullException(result, "environment");
            }
        }

        public sealed class TheIsRunningOnGoCDProperty
        {
            [Fact]
            public void Should_Return_True_If_Running_On_GoCD()
            {
                // Given
                var fixture = new GoCDFixture();
                fixture.IsRunningOnGoCD();
                var gocd = fixture.CreateGoCDService();

                // When
                var result = gocd.IsRunningOnGoCD;

                // Then
                Assert.True(result);
            }

            [Fact]
            public void Should_Return_False_If_Not_Running_On_GoCD()
            {
                // Given
                var fixture = new GoCDFixture();
                var gocd = fixture.CreateGoCDService();

                // When
                var result = gocd.IsRunningOnGoCD;

                // Then
                Assert.False(result);
            }
        }

        public sealed class TheEnvironmentProperty
        {
            [Fact]
            public void Should_Return_Non_Null_Reference()
            {
                // Given
                var fixture = new GoCDFixture();
                var gocd = fixture.CreateGoCDService();

                // When
                var result = gocd.Environment;

                // Then
                Assert.NotNull(result);
            }
        }
    }
}