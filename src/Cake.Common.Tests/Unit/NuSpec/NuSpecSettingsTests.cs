// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.NuSpec;
using Xunit;

namespace Cake.Common.Tests.Unit.NuSpec
{
    public sealed class NuSpecSettingsTests
    {
        public sealed class TheConstructor
        {
            [Fact]
            public void Should_Set_DevelopmentDependency_To_False_By_Default()
            {
                // Given, When
                var settings = new NuSpecSettings();

                // Then
                Assert.False(settings.DevelopmentDependency);
            }

            [Fact]
            public void Should_Set_RequireLicenseAcceptance_To_False_By_Default()
            {
                // Given, When
                var settings = new NuSpecSettings();

                // Then
                Assert.False(settings.RequireLicenseAcceptance);
            }

            [Fact]
            public void Should_Set_Authors_To_Null_By_Default()
            {
                // Given, When
                var settings = new NuSpecSettings();

                // Then
                Assert.Null(settings.Authors);
            }

            [Fact]
            public void Should_Set_Owners_To_Null_By_Default()
            {
                // Given, When
                var settings = new NuSpecSettings();

                // Then
                Assert.Null(settings.Owners);
            }

            [Fact]
            public void Should_Set_ReleaseNotes_To_Null_By_Default()
            {
                // Given, When
                var settings = new NuSpecSettings();

                // Then
                Assert.Null(settings.ReleaseNotes);
            }

            [Fact]
            public void Should_Set_Tags_To_Null_By_Default()
            {
                // Given, When
                var settings = new NuSpecSettings();

                // Then
                Assert.Null(settings.Tags);
            }

            [Fact]
            public void Should_Set_Files_To_Null_By_Default()
            {
                // Given, When
                var settings = new NuSpecSettings();

                // Then
                Assert.Null(settings.Files);
            }

            [Fact]
            public void Should_Set_Dependencies_To_Null_By_Default()
            {
                // Given, When
                var settings = new NuSpecSettings();

                // Then
                Assert.Null(settings.Dependencies);
            }

            [Fact]
            public void Should_Set_References_To_Null_By_Default()
            {
                // Given, When
                var settings = new NuSpecSettings();

                // Then
                Assert.Null(settings.References);
            }

            [Fact]
            public void Should_Set_PackageTypes_To_Null_By_Default()
            {
                // Given, When
                var settings = new NuSpecSettings();

                // Then
                Assert.Null(settings.PackageTypes);
            }

            [Fact]
            public void Should_Set_FrameworkAssemblies_To_Null_By_Default()
            {
                // Given, When
                var settings = new NuSpecSettings();

                // Then
                Assert.Null(settings.FrameworkAssemblies);
            }

            [Fact]
            public void Should_Set_ContentFiles_To_Null_By_Default()
            {
                // Given, When
                var settings = new NuSpecSettings();

                // Then
                Assert.Null(settings.ContentFiles);
            }
        }
    }
}