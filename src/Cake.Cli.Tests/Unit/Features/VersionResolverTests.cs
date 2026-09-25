// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Cli;
using Xunit;

namespace Cake.Cli.Tests.Unit.Features;

public sealed class VersionResolverTests
{
    [Fact]
    public void GetVersion_Should_Not_Throw_And_Return_Non_Empty_Value()
    {
        // Given
        var resolver = new VersionResolver();

        // When
        var version = resolver.GetVersion();

        // Then
        Assert.False(string.IsNullOrWhiteSpace(version));
    }

    [Fact]
    public void GetProductVersion_Should_Not_Throw_And_Return_Non_Empty_Value()
    {
        // Given
        var resolver = new VersionResolver();

        // When
        var version = resolver.GetProductVersion();

        // Then
        Assert.False(string.IsNullOrWhiteSpace(version));
    }
}
