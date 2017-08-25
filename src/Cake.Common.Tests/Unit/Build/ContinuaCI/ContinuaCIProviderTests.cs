// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Build.ContinuaCI;
using Cake.Common.Tests.Fixtures.Build;
using Xunit;

namespace Cake.Common.Tests.Unit.Build.ContinuaCI
{
    public sealed class ContinuaCIProviderTests
    {
        public sealed class TheConstructor
        {
            [Fact]
            public void Should_Throw_If_Environment_Is_Null()
            {
                // Given, When
                var result = Record.Exception(() => new ContinuaCIProvider(null));

                // Then
                AssertEx.IsArgumentNullException(result, "environment");
            }
        }

        public sealed class TheIsRunningOnContinuaCIProperty
        {
            [Fact]
            public void Should_Return_True_If_Running_On_ContinuaCI()
            {
                // Given
                var fixture = new ContinuaCIFixture();
                fixture.IsRunningOnContinuaCI();
                var continuaCI = fixture.CreateContinuaCIService();

                // When
                var result = continuaCI.IsRunningOnContinuaCI;

                // Then
                Assert.True(result);
            }

            [Fact]
            public void Should_Return_False_If_Not_Running_On_ContinuaCI()
            {
                // Given
                var fixture = new ContinuaCIFixture();
                var continuaCI = fixture.CreateContinuaCIService();

                // When
                var result = continuaCI.IsRunningOnContinuaCI;

                // Then
                Assert.False(result);
            }

            public sealed class TheEnvironmentProperty
            {
                [Fact]
                public void Should_Return_Non_Null_Reference()
                {
                    // Given
                    var fixture = new ContinuaCIFixture();
                    var continuaCI = fixture.CreateContinuaCIService();

                    // When
                    var result = continuaCI.Environment;

                    // Then
                    Assert.NotNull(result);
                }
            }
        }
    }
}