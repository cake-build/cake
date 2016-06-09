// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Cake.Common.Tools.NuGet.Pack;
using Xunit;

namespace Cake.Common.Tests.Unit.Tools.NuGet.Pack
{
    public sealed class NuGetPackSettingsTests
    {
        public sealed class TheConstructor
        {
            [Fact]
            public void Should_Set_NoPackageAnalysis_To_False_By_Default()
            {
                // Given, When
                var settings = new NuGetPackSettings();

                // Then
                Assert.False(settings.NoPackageAnalysis);
            }

            [Fact]
            public void Should_Set_IncludeReferencedProjects_To_False_By_Default()
            {
                // Given, When
                var settings = new NuGetPackSettings();

                // Then
                Assert.False(settings.IncludeReferencedProjects);
            }

            [Fact]
            public void Should_Set_Symbols_To_False_By_Default()
            {
                // Given, When
                var settings = new NuGetPackSettings();

                // Then
                Assert.False(settings.Symbols);
            }
        }
    }
}
