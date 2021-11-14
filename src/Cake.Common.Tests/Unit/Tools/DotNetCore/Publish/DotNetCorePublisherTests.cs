// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tests.Fixtures.Tools.DotNetCore.Publish;
using Cake.Common.Tools.DotNetCore;
using Cake.Testing;
using Xunit;

namespace Cake.Common.Tests.Unit.Tools.DotNetCore.Publish
{
    public sealed class DotNetCorePublisherTests
    {
        public sealed class ThePublishMethod
        {
            [Fact]
            public void Should_Throw_If_Settings_Are_Null()
            {
                // Given
                var fixture = new DotNetCorePublisherFixture();
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
                var fixture = new DotNetCorePublisherFixture();
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
                var fixture = new DotNetCorePublisherFixture();
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
                var fixture = new DotNetCorePublisherFixture();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("publish", result.Args);
            }

            [Fact]
            public void Should_Add_Path()
            {
                // Given
                var fixture = new DotNetCorePublisherFixture();
                fixture.Project = "./src/*";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("publish \"./src/*\"", result.Args);
            }

            [Theory]
            [InlineData("./src/*", "publish \"./src/*\"")]
            [InlineData("./src/cake artifacts/", "publish \"./src/cake artifacts/\"")]
            [InlineData("./src/cake artifacts/cake binaries", "publish \"./src/cake artifacts/cake binaries\"")]
            public void Should_Quote_Project_Path(string text, string expected)
            {
                // Given
                var fixture = new DotNetCorePublisherFixture();
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
                var fixture = new DotNetCorePublisherFixture();
                fixture.Settings.NoBuild = true;
                fixture.Settings.NoDependencies = true;
                fixture.Settings.NoRestore = true;
                fixture.Settings.NoLogo = true;
                fixture.Settings.Framework = "dnxcore50";
                fixture.Settings.Configuration = "Release";
                fixture.Settings.Runtime = "runtime1";
                fixture.Settings.OutputDirectory = "./artifacts/";
                fixture.Settings.VersionSuffix = "rc1";
                fixture.Settings.Verbosity = DotNetCoreVerbosity.Minimal;
                fixture.Settings.Force = true;
                fixture.Settings.SelfContained = true;
                fixture.Settings.Sources = new[] { "https://api.nuget.org/v3/index.json" };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("publish --output \"/Working/artifacts\" --runtime runtime1 --framework dnxcore50 --configuration Release --version-suffix rc1 --no-build --no-dependencies --no-restore --nologo --force --self-contained true --source \"https://api.nuget.org/v3/index.json\" --verbosity minimal", result.Args);
            }

            [Fact]
            public void Should_Add_SelfContained_False_Settings()
            {
                // Given
                var fixture = new DotNetCorePublisherFixture();
                fixture.Settings.NoDependencies = true;
                fixture.Settings.NoRestore = true;
                fixture.Settings.Framework = "dnxcore50";
                fixture.Settings.Configuration = "Release";
                fixture.Settings.Runtime = "runtime1";
                fixture.Settings.OutputDirectory = "./artifacts/";
                fixture.Settings.VersionSuffix = "rc1";
                fixture.Settings.Verbosity = DotNetCoreVerbosity.Minimal;
                fixture.Settings.Force = true;
                fixture.Settings.SelfContained = false;
                fixture.Settings.Sources = new[] { "https://api.nuget.org/v3/index.json" };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("publish --output \"/Working/artifacts\" --runtime runtime1 --framework dnxcore50 --configuration Release --version-suffix rc1 --no-dependencies --no-restore --force --self-contained false --source \"https://api.nuget.org/v3/index.json\" --verbosity minimal", result.Args);
            }

            [Fact]
            public void Should_Add_Host_Arguments()
            {
                // Given
                var fixture = new DotNetCorePublisherFixture();
                fixture.Settings.DiagnosticOutput = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("--diagnostics publish", result.Args);
            }

            [Fact]
            public void Should_Add_PublishSingleFile()
            {
                // Given
                var fixture = new DotNetCorePublisherFixture();
                fixture.Settings.PublishSingleFile = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("publish -p:PublishSingleFile=true", result.Args);
            }

            [Fact]
            public void Should_Add_PublishTrimmed()
            {
                // Given
                var fixture = new DotNetCorePublisherFixture();
                fixture.Settings.PublishTrimmed = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("publish -p:PublishTrimmed=true", result.Args);
            }

            [Fact]
            public void Should_Add_TieredCompilationQuickJit()
            {
                // Given
                var fixture = new DotNetCorePublisherFixture();
                fixture.Settings.TieredCompilationQuickJit = false;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("publish -p:TieredCompilationQuickJit=false", result.Args);
            }

            [Fact]
            public void Should_Add_TieredCompilation()
            {
                // Given
                var fixture = new DotNetCorePublisherFixture();
                fixture.Settings.TieredCompilation = false;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("publish -p:TieredCompilation=false", result.Args);
            }

            [Fact]
            public void Should_Add_PublishReadyToRun()
            {
                // Given
                var fixture = new DotNetCorePublisherFixture();
                fixture.Settings.PublishReadyToRun = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("publish -p:PublishReadyToRun=true", result.Args);
            }

            [Fact]
            public void Should_Add_PublishReadyToRunShowWarnings()
            {
                // Given
                var fixture = new DotNetCorePublisherFixture();
                fixture.Settings.PublishReadyToRunShowWarnings = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("publish -p:PublishReadyToRunShowWarnings=true", result.Args);
            }

            [Fact]
            public void Should_Add_IncludeNativeLibrariesForSelfExtract()
            {
                // Given
                var fixture = new DotNetCorePublisherFixture();
                fixture.Settings.IncludeNativeLibrariesForSelfExtract = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("publish -p:IncludeNativeLibrariesForSelfExtract=true", result.Args);
            }

            [Fact]
            public void Should_Add_IncludeAllContentForSelfExtract()
            {
                // Given
                var fixture = new DotNetCorePublisherFixture();
                fixture.Settings.IncludeAllContentForSelfExtract = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("publish -p:IncludeAllContentForSelfExtract=true", result.Args);
            }

            [Fact]
            public void Should_Add_EnableCompressionInSingleFile()
            {
                // Given
                var fixture = new DotNetCorePublisherFixture();
                fixture.Settings.EnableCompressionInSingleFile = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("publish -p:EnableCompressionInSingleFile=true", result.Args);
            }
        }
    }
}
