// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Build.Bamboo;
using Cake.Common.Tests.Fixtures.Build;
using Xunit;

namespace Cake.Common.Tests.Unit.Build.Bamboo
{
    public sealed class BambooProviderTests
    {
        public sealed class TheConstructor
        {
            [Fact]
            public void Should_Throw_If_Environment_Is_Null()
            {
                // Given, When
                var result = Record.Exception(() => new BambooProvider(null));

                // Then
                AssertEx.IsArgumentNullException(result, "environment");
            }
        }

        public sealed class TheIsRunningOnBambooProperty
        {
            [Fact]
            public void Should_Return_True_If_Running_On_Bamboo()
            {
                // Given
                var fixture = new BambooFixture();
                fixture.IsRunningOnBamboo();
                var bamboo = fixture.CreateBambooService();

                // When
                var result = bamboo.IsRunningOnBamboo;

                // Then
                Assert.True(result);
            }

            [Fact]
            public void Should_Return_False_If_Not_Running_On_Bamboo()
            {
                // Given
                var fixture = new BambooFixture();
                var bamboo = fixture.CreateBambooService();

                // When
                var result = bamboo.IsRunningOnBamboo;

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
                var fixture = new BambooFixture();
                var bamboo = fixture.CreateBambooService();

                // When
                var result = bamboo.Environment;

                // Then
                Assert.NotNull(result);
            }
        }
    }
}