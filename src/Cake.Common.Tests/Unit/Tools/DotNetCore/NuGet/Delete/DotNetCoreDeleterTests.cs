// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tests.Fixtures.Tools.DotNetCore.NuGet.Delete;
using Cake.Common.Tools.DotNetCore.NuGet.Delete;
using Cake.Testing;
using Xunit;

namespace Cake.Common.Tests.Unit.Tools.DotNetCore.NuGet.Delete
{
    public sealed class DotNetCoreDeleterTests
    {
        public sealed class TheDeleteMethod
        {
            [Fact]
            public void Should_Throw_If_Settings_Are_Null()
            {
                // Given
                var fixture = new DotNetCoreDeleterFixture();
                fixture.Settings = null;
                fixture.GivenDefaultToolDoNotExist();

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "settings");
            }

            [Theory]
            [InlineData(null)]
            [InlineData("")]
            [InlineData("       ")]
            public void Should_Not_Throw_If_PackageName_Is_Empty(string packageName)
            {
                // Given
                var fixture = new DotNetCoreDeleterFixture();
                fixture.PackageName = packageName;
                fixture.Settings = new DotNetCoreNuGetDeleteSettings();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("nuget delete", result.Args);
            }

            [Theory]
            [InlineData(null)]
            [InlineData("")]
            [InlineData("       ")]
            public void Should_Not_Throw_If_PackageVersion_Is_Empty(string packageVersion)
            {
                // Given
                var fixture = new DotNetCoreDeleterFixture();
                fixture.PackageName = "name";
                fixture.PackageVersion = packageVersion;
                fixture.Settings = new DotNetCoreNuGetDeleteSettings();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("nuget delete name", result.Args);
            }

            [Fact]
            public void Should_Throw_If_Process_Was_Not_Started()
            {
                // Given
                var fixture = new DotNetCoreDeleterFixture();
                fixture.GivenProcessCannotStart();

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsCakeException(result, ".NET Core CLI: Process was not started.");
            }

            [Fact]
            public void Should_Throw_If_Process_Has_A_Non_Zero_Exit_Code()
            {
                // Given
                var fixture = new DotNetCoreDeleterFixture();
                fixture.GivenProcessExitsWithCode(1);

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsCakeException(result, ".NET Core CLI: Process returned an error (exit code 1).");
            }

            [Fact]
            public void Should_Add_Additional_Arguments()
            {
                // Given
                const string packageName = "name";
                const string packageVersion = "1.2.3";
                const string source = "http://www.nuget.org/api/v2/package";
                const string apiKey = "key1234";

                var fixture = new DotNetCoreDeleterFixture();
                fixture.Settings.Source = source;
                fixture.Settings.NonInteractive = true;
                fixture.Settings.ApiKey = apiKey;
                fixture.Settings.ForceEnglishOutput = true;
                fixture.PackageName = packageName;
                fixture.PackageVersion = packageVersion;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(string.Format("nuget delete {0} {1} --source \"{2}\" --non-interactive --api-key \"{3}\" --force-english-output", packageName, packageVersion, source, apiKey), result.Args);
            }

            [Fact]
            public void Should_Add_Host_Arguments()
            {
                // Given
                var fixture = new DotNetCoreDeleterFixture();
                fixture.Settings.DiagnosticOutput = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("--diagnostics nuget delete", result.Args);
            }
        }
    }
}