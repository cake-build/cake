// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tests.Fixtures.Tools.DotNetCore.Pack;
using Cake.Common.Tools.DotNetCore;
using Cake.Testing;
using Xunit;

namespace Cake.Common.Tests.Unit.Tools.DotNetCore.Pack
{
    public sealed class DotNetCorePackerTests
    {
        public sealed class ThePackMethod
        {
            [Fact]
            public void Should_Throw_If_Settings_Are_Null()
            {
                // Given
                var fixture = new DotNetCorePackFixture();
                fixture.Settings = null;
                fixture.GivenDefaultToolDoNotExist();

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "settings");
            }

            [Fact]
            public void Should_Throw_If_Process_Was_Not_Started()
            {
                // Given
                var fixture = new DotNetCorePackFixture();
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
                var fixture = new DotNetCorePackFixture();
                fixture.GivenProcessExitsWithCode(1);

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsCakeException(result, ".NET Core CLI: Process returned an error (exit code 1).");
            }

            [Fact]
            public void Should_Add_Mandatory_Arguments()
            {
                // Given
                var fixture = new DotNetCorePackFixture();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("pack", result.Args);
            }

            [Fact]
            public void Should_Add_Project()
            {
                // Given
                var fixture = new DotNetCorePackFixture();
                fixture.Project = "./src/*";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("pack \"./src/*\"", result.Args);
            }

            [Theory]
            [InlineData("./src/*", "pack \"./src/*\"")]
            [InlineData("./src/cake artifacts/", "pack \"./src/cake artifacts/\"")]
            [InlineData("./src/nuget/cake build", "pack \"./src/nuget/cake build\"")]
            public void Should_Quote_Project_Path(string text, string expected)
            {
                // Given
                var fixture = new DotNetCorePackFixture();
                fixture.Project = text;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Fact]
            public void Should_Add_Settings()
            {
                // Given
                var fixture = new DotNetCorePackFixture();
                fixture.Settings.NoBuild = true;
                fixture.Settings.Configuration = "Release";
                fixture.Settings.OutputDirectory = "./artifacts/";
                fixture.Settings.VersionSuffix = "rc1";
                fixture.Settings.IncludeSource = true;
                fixture.Settings.IncludeSymbols = true;
                fixture.Settings.Serviceable = true;
                fixture.Settings.Verbosity = DotNetCoreVerbosity.Minimal;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("pack --output \"/Working/artifacts\" --no-build --include-symbols --include-source --configuration Release --version-suffix rc1 --serviceable --verbosity Minimal", result.Args);
            }

            [Fact]
            public void Should_Add_Host_Arguments()
            {
                // Given
                var fixture = new DotNetCorePackFixture();
                fixture.Settings.DiagnosticOutput = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("--diagnostics pack", result.Args);
            }
        }
    }
}
