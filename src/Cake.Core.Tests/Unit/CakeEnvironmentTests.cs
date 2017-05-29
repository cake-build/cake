// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Testing;
using Xunit;

namespace Cake.Core.Tests.Unit
{
    public sealed class CakeEnvironmentTests
    {
        public sealed class ThePathComparerProperty
        {
            [Theory]
            [InlineData(PlatformFamily.Windows, false)]
            [InlineData(PlatformFamily.Linux, true)]
            [InlineData(PlatformFamily.OSX, true)]
            [InlineData(PlatformFamily.Unknown, false)]
            public void Should_Return_Environment_Specific_PathComparer(PlatformFamily family, bool isCaseSensitive)
            {
                // Given
                var environment = new CakeEnvironment(new FakePlatform(family), new FakeRuntime(), new FakeLog());

                // When
                var pathComparer = environment.PathComparer;

                // Then
                Assert.Equal(isCaseSensitive, pathComparer.IsCaseSensitive);
            }
        }
    }
}
