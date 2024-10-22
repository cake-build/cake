// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tests.Fixtures.Tools.DotNet.Package.Search;
using Cake.Common.Tools.DotNet;
using Cake.Testing;
using Xunit;

namespace Cake.Common.Tests.Unit.Tools.DotNet.Package.Search
{
    public sealed class DotNetPackageSearcherTests
    {
        public sealed class TheSearchMethod
        {
            [Fact]
            public void Should_Throw_If_Settings_Are_Null()
            {
                // Given
                var fixture = new DotNetPackageSearcherFixture();
                fixture.Settings = null;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "settings");
            }

            [Fact]
            public void Should_Add_Mandatory_Arguments()
            {
                // Given
                var fixture = new DotNetPackageSearcherFixture();
                fixture.SearchTerm = "Cake";
                fixture.GivenNormalPackageResult();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("package search \"Cake\" --verbosity normal --format json", result.Args);
            }

            [Fact]
            public void Should_Add_ExactMatch_To_Arguments_If_True()
            {
                // Given
                var fixture = new DotNetPackageSearcherFixture();
                fixture.SearchTerm = "Cake";
                fixture.Settings.ExactMatch = true;
                fixture.GivenNormalPackageResult();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("package search \"Cake\" --exact-match --verbosity normal --format json", result.Args);
            }

            [Fact]
            public void Should_Add_Prerelease_To_Arguments_If_True()
            {
                // Given
                var fixture = new DotNetPackageSearcherFixture();
                fixture.SearchTerm = "Cake";
                fixture.Settings.Prerelease = true;
                fixture.GivenNormalPackageResult();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("package search \"Cake\" --prerelease --verbosity normal --format json", result.Args);
            }

            [Fact]
            public void Should_Add_Take_To_Arguments_If_True()
            {
                // Given
                var fixture = new DotNetPackageSearcherFixture();
                fixture.SearchTerm = "Cake";
                fixture.Settings.Take = 10;
                fixture.GivenNormalPackageResult();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("package search \"Cake\" --take 10 --verbosity normal --format json", result.Args);
            }

            [Fact]
            public void Should_Add_Skip_To_Arguments_If_True()
            {
                // Given
                var fixture = new DotNetPackageSearcherFixture();
                fixture.SearchTerm = "Cake";
                fixture.Settings.Skip = 10;
                fixture.GivenNormalPackageResult();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("package search \"Cake\" --skip 10 --verbosity normal --format json", result.Args);
            }

            [Fact]
            public void Should_Add_Sources_To_Arguments_If_Set()
            {
                // Given
                var fixture = new DotNetPackageSearcherFixture();
                fixture.SearchTerm = "Cake";
                fixture.Settings.Sources = new[] { "A", "B", "C", };
                fixture.GivenNormalPackageResult();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("package search \"Cake\" --source \"A\" --source \"B\" --source \"C\" --verbosity normal --format json", result.Args);
            }

            [Fact]
            public void Should_Add_ConfigFile_To_Arguments_If_Set()
            {
                // Given
                var fixture = new DotNetPackageSearcherFixture();
                fixture.SearchTerm = "Cake";
                fixture.Settings.ConfigFile = "./nuget.config";
                fixture.GivenNormalPackageResult();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("package search \"Cake\" --configfile \"/Working/nuget.config\" " +
                             "--verbosity normal --format json", result.Args);
            }

            [Fact]
            public void Should_Return_Correct_List_Of_DotNetPackageSearchItems()
            {
                // Given
                var fixture = new DotNetPackageSearcherFixture();
                fixture.SearchTerm = "Cake";
                fixture.GivenNormalPackageResult();

                // When
                var result = fixture.Run();

                // Then
                Assert.Collection(fixture.Result,
                    item =>
                    {
                        Assert.Equal(item.Name, "Cake");
                        Assert.Equal(item.Version, "0.22.2");
                    },
                    item =>
                    {
                        Assert.Equal(item.Name, "Cake.Core");
                        Assert.Equal(item.Version, "0.22.2");
                    },
                    item =>
                    {
                        Assert.Equal(item.Name, "Cake.CoreCLR");
                        Assert.Equal(item.Version, "0.22.2");
                    });
            }
        }
    }
}
