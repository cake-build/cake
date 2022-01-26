// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tests.Fixtures.Tools.DotNet.Store;
using Cake.Common.Tools.DotNetCore;
using Cake.Core.IO;
using Cake.Testing;
using Xunit;

namespace Cake.Common.Tests.Unit.Tools.DotNet.Store
{
    public sealed class DotNetStoreTests
    {
        public sealed class TheTestMethod
        {
            [Fact]
            public void Should_Throw_If_Settings_Are_Null()
            {
                // Given
                var fixture = new DotNetStorerFixture
                {
                    ManifestFiles = new[] { (FilePath)"./test/unit.tests.csproj" },
                    Framework = "net6.0",
                    Runtime = "win-x64",
                    Settings = null
                };
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
                var fixture = new DotNetStorerFixture
                {
                    ManifestFiles = new[] { (FilePath)"./test/unit.tests.csproj" },
                    Framework = "net6.0",
                    Runtime = "win-x64"
                };
                fixture.GivenProcessCannotStart();

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsCakeException(result, ".NET CLI: Process was not started.");
            }

            [Fact]
            public void Should_Throw_If_Process_Has_A_Non_Zero_Exit_Code()
            {
                // Given
                var fixture = new DotNetStorerFixture
                {
                    ManifestFiles = new[] { (FilePath)"./test/unit.tests.csproj" },
                    Framework = "net6.0",
                    Runtime = "win-x64"
                };
                fixture.GivenProcessExitsWithCode(1);

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsCakeException(result, ".NET CLI: Process returned an error (exit code 1).");
            }

            [Fact]
            public void Should_Add_Mandatory_Arguments()
            {
                // Given
                var fixture = new DotNetStorerFixture
                {
                    ManifestFiles = new[] { (FilePath)"./test/unit.tests.csproj" },
                    Framework = "net6.0",
                    Runtime = "win-x64"
                };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("store --manifest \"/Working/test/unit.tests.csproj\" --framework net6.0 --runtime win-x64", result.Args);
            }

            [Fact]
            public void Should_Add_Multiple_Manifest_File_Arguments()
            {
                // Given
                var fixture = new DotNetStorerFixture
                {
                    ManifestFiles = new[]
                    {
                        (FilePath)"./test1/unit.tests.csproj",
                        (FilePath)"./test2/unit.tests.csproj",
                        (FilePath)"./test3/unit.tests.csproj",
                    },
                    Framework = "net6.0",
                    Runtime = "win-x64"
                };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("store --manifest \"/Working/test1/unit.tests.csproj\" --manifest \"/Working/test2/unit.tests.csproj\" --manifest \"/Working/test3/unit.tests.csproj\" --framework net6.0 --runtime win-x64", result.Args);
            }

            [Theory]
            [InlineData("./src/*.csproj", "store --manifest \"/Working/src/*.csproj\" --framework net6.0 --runtime win-x64")]
            [InlineData("./src/cake core tests.csproj", "store --manifest \"/Working/src/cake core tests.csproj\" --framework net6.0 --runtime win-x64")]
            [InlineData("./src/cake artifacts/cake core tests.csproj", "store --manifest \"/Working/src/cake artifacts/cake core tests.csproj\" --framework net6.0 --runtime win-x64")]
            public void Should_Quote_Manifest_File_Path(string text, string expected)
            {
                // Given
                var fixture = new DotNetStorerFixture
                {
                    ManifestFiles = new[] { (FilePath)text },
                    Framework = "net6.0",
                    Runtime = "win-x64"
                };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Fact]
            public void Should_Add_Settings()
            {
                // Given
                var fixture = new DotNetStorerFixture();
                fixture.ManifestFiles = new[] { (FilePath)"packages.csproj" };
                fixture.Framework = "dnxcore50";
                fixture.Runtime = "runtime1";
                fixture.Settings.FrameworkVersion = "2.0.0";
                fixture.Settings.OutputDirectory = "./artifacts/";
                fixture.Settings.SkipOptimization = true;
                fixture.Settings.SkipSymbols = true;
                fixture.Settings.UseCurrentRuntime = true;
                fixture.Settings.Verbosity = DotNetCoreVerbosity.Minimal;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("store --manifest \"/Working/packages.csproj\" --framework dnxcore50 --runtime runtime1 --framework-version 2.0.0 --output \"/Working/artifacts\" --skip-optimization --skip-symbols --use-current-runtime --verbosity minimal", result.Args);
            }
        }
    }
}
