// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tests.Fixtures.Tools.DotNetCore.Build;
using Cake.Common.Tools.DotNetCore;
using Cake.Common.Tools.DotNetCore.Build;
using Cake.Testing;
using Xunit;

namespace Cake.Common.Tests.Unit.Tools.DotNetCore.Build
{
    public sealed class DotNetCoreBuilderTests
    {
        public sealed class TheBuildMethod
        {
            [Fact]
            public void Should_Throw_If_Settings_Are_Null()
            {
                // Given
                var fixture = new DotNetCoreBuilderFixture();
                fixture.Project = "./src/*";
                fixture.Settings = null;
                fixture.GivenDefaultToolDoNotExist();

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "settings");
            }

            [Fact]
            public void Should_Throw_If_Project_Is_Null()
            {
                // Given
                var fixture = new DotNetCoreBuilderFixture();
                fixture.Settings = new DotNetCoreBuildSettings();
                fixture.GivenDefaultToolDoNotExist();

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "project");
            }

            [Fact]
            public void Should_Throw_If_Process_Was_Not_Started()
            {
                // Given
                var fixture = new DotNetCoreBuilderFixture();
                fixture.Project = "./src/*";
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
                var fixture = new DotNetCoreBuilderFixture();
                fixture.Project = "./src/*";
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
                var fixture = new DotNetCoreBuilderFixture();
                fixture.Project = "./src/*";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("build \"./src/*\"", result.Args);
            }

            [Fact]
            public void Should_Add_Additional_Arguments()
            {
                // Given
                var fixture = new DotNetCoreBuilderFixture();
                fixture.Settings.Framework = "net451";
                fixture.Settings.Runtime = "runtime1";
                fixture.Settings.Configuration = "Release";
                fixture.Settings.VersionSuffix = "rc1";
                fixture.Project = "./src/*";
                fixture.Settings.Verbosity = DotNetCoreVerbosity.Minimal;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("build \"./src/*\" --runtime runtime1 --framework net451 --configuration Release --version-suffix rc1 --verbosity Minimal", result.Args);
            }

            [Fact]
            public void Should_Add_OutputPath_Arguments()
            {
                // Given
                var fixture = new DotNetCoreBuilderFixture();
                fixture.Settings.OutputDirectory = "./artifacts/";
                fixture.Project = "./src/*";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("build \"./src/*\" --output \"/Working/artifacts\"", result.Args);
            }

            [Theory]
            [InlineData("./src/*", "build \"./src/*\"")]
            [InlineData("./src/cake build/", "build \"./src/cake build/\"")]
            [InlineData("./src/cake build/cake cli", "build \"./src/cake build/cake cli\"")]
            public void Should_Quote_Project_Path(string text, string expected)
            {
                // Given
                var fixture = new DotNetCoreBuilderFixture();
                fixture.Project = text;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Fact]
            public void Should_Add_Build_Arguments()
            {
                // Given
                var fixture = new DotNetCoreBuilderFixture();
                fixture.Settings.NoIncremental = true;
                fixture.Settings.NoDependencies = true;
                fixture.Project = "./src/*";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("build \"./src/*\" --no-incremental --no-dependencies", result.Args);
            }

            [Fact]
            public void Should_Add_Host_Arguments()
            {
                // Given
                var fixture = new DotNetCoreBuilderFixture();
                fixture.Project = "./src/*";
                fixture.Settings.DiagnosticOutput = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("--diagnostics build \"./src/*\"", result.Args);
            }
        }
    }
}