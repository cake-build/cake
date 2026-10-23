// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tests.Fixtures.Tools.DotNet.Publish;
using Cake.Common.Tools.DotNet;
using Cake.Common.Tools.DotNet.MSBuild;
using Cake.Common.Tools.DotNet.Publish;
using Cake.Testing;

namespace Cake.Common.Tests.Unit.Tools.DotNet.Publish;

public sealed class DotNetPublisherTests
{
    public sealed class ThePublishMethod
    {
        [Fact]
        public void Should_Throw_If_Settings_Are_Null()
        {
            // Given
            var fixture = new DotNetPublisherFixture();
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
            var fixture = new DotNetPublisherFixture();
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
            var fixture = new DotNetPublisherFixture();
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
            var fixture = new DotNetPublisherFixture();

            // When
            var result = fixture.Run();

            // Then
            Assert.Equal("publish", result.Args);
        }

        [Fact]
        public void Should_Add_Path()
        {
            // Given
            var fixture = new DotNetPublisherFixture();
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
            var fixture = new DotNetPublisherFixture();
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
            var fixture = new DotNetPublisherFixture();
            fixture.Settings.NoBuild = true;
            fixture.Settings.NoDependencies = true;
            fixture.Settings.NoRestore = true;
            fixture.Settings.NoLogo = true;
            fixture.Settings.Framework = "dnxcore50";
            fixture.Settings.Configuration = "Release";
            fixture.Settings.Runtime = "runtime1";
            fixture.Settings.OutputDirectory = "./artifacts/";
            fixture.Settings.VersionSuffix = "rc1";
            fixture.Settings.Verbosity = DotNetVerbosity.Minimal;
            fixture.Settings.Force = true;
            fixture.Settings.SelfContained = true;
            fixture.Settings.Sources = new[] { "https://api.nuget.org/v3/index.json" };

            // When
            var result = fixture.Run();

            // Then
            Assert.Equal("publish --output \"/Working/artifacts\" --runtime runtime1 --framework dnxcore50 --configuration Release --version-suffix rc1 --no-build --no-dependencies --no-restore --nologo --force --self-contained --source \"https://api.nuget.org/v3/index.json\" --verbosity minimal", result.Args);
        }

        [Fact]
        public void Should_Add_SelfContained_False_Settings()
        {
            // Given
            var fixture = new DotNetPublisherFixture();
            fixture.Settings.NoDependencies = true;
            fixture.Settings.NoRestore = true;
            fixture.Settings.Framework = "dnxcore50";
            fixture.Settings.Configuration = "Release";
            fixture.Settings.Runtime = "runtime1";
            fixture.Settings.OutputDirectory = "./artifacts/";
            fixture.Settings.VersionSuffix = "rc1";
            fixture.Settings.Verbosity = DotNetVerbosity.Minimal;
            fixture.Settings.Force = true;
            fixture.Settings.SelfContained = false;
            fixture.Settings.Sources = new[] { "https://api.nuget.org/v3/index.json" };

            // When
            var result = fixture.Run();

            // Then
            Assert.Equal("publish --output \"/Working/artifacts\" --runtime runtime1 --framework dnxcore50 --configuration Release --version-suffix rc1 --no-dependencies --no-restore --force --no-self-contained --source \"https://api.nuget.org/v3/index.json\" --verbosity minimal", result.Args);
        }

        [Fact]
        public void Should_Add_Host_Arguments()
        {
            // Given
            var fixture = new DotNetPublisherFixture();
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
            var fixture = new DotNetPublisherFixture();
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
            var fixture = new DotNetPublisherFixture();
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
            var fixture = new DotNetPublisherFixture();
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
            var fixture = new DotNetPublisherFixture();
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
            var fixture = new DotNetPublisherFixture();
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
            var fixture = new DotNetPublisherFixture();
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
            var fixture = new DotNetPublisherFixture();
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
            var fixture = new DotNetPublisherFixture();
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
            var fixture = new DotNetPublisherFixture();
            fixture.Settings.EnableCompressionInSingleFile = true;

            // When
            var result = fixture.Run();

            // Then
            Assert.Equal("publish -p:EnableCompressionInSingleFile=true", result.Args);
        }

        [Fact]
        public void Should_Add_Os()
        {
            // Given
            var fixture = new DotNetPublisherFixture();
            fixture.Settings.OS = "linux";

            // When
            var result = fixture.Run();

            // Then
            Assert.Equal("publish --os linux", result.Args);
        }

        [Fact]
        public void Should_Add_PublishContainer_Target_When_Container_Is_Set()
        {
            // Given
            var fixture = new DotNetPublisherFixture();
            fixture.Settings.Container = new DotNetPublishContainerSettings();

            // When
            var result = fixture.Run();

            // Then
            Assert.Equal("publish -t:PublishContainer", result.Args);
        }

        [Fact]
        public void Should_Add_All_Container_Settings()
        {
            // Given
            var fixture = new DotNetPublisherFixture();
            fixture.Settings.Container = new DotNetPublishContainerSettings
            {
                BaseImage = "mcr.microsoft.com/dotnet/sdk:11.0-resolute-aot",
                Family = "noble-chiseled",
                Registry = "ghcr.io",
                Repository = "devlead/cake-sdk",
                ImageTag = "latest",
                User = "root",
                WorkingDirectory = "/app/",
                ArchiveOutputPath = "./artifacts/Cake.Sdk.tar.gz",
                LocalRegistry = DotNetContainerLocalRegistry.Docker,
                RuntimeIdentifier = "linux-x64",
                RuntimeIdentifiers = ["linux-x64", "linux-arm64"],
                ImageFormat = DotNetContainerImageFormat.OCI,
                AppCommandInstruction = DotNetContainerAppCommandInstruction.Entrypoint,
                GenerateLabels = false,
                PublishInParallel = true,
                PushNoCache = true,
                OS = DotNetContainerOS.Linux,
                Arch = DotNetContainerArchitecture.X64
            };

            // When
            var result = fixture.Run();

            // Then
            Assert.Equal(
                "publish --os linux --arch x64 -t:PublishContainer" +
                " -p:ContainerBaseImage=mcr.microsoft.com/dotnet/sdk:11.0-resolute-aot" +
                " -p:ContainerFamily=noble-chiseled" +
                " -p:ContainerRegistry=ghcr.io" +
                " -p:ContainerRepository=devlead/cake-sdk" +
                " -p:ContainerImageTag=latest" +
                " -p:ContainerUser=root" +
                " -p:ContainerWorkingDirectory=/app/" +
                " -p:ContainerArchiveOutputPath=/Working/artifacts/Cake.Sdk.tar.gz" +
                " -p:LocalRegistry=Docker" +
                " -p:ContainerRuntimeIdentifier=linux-x64" +
                " -p:ContainerRuntimeIdentifiers=\\\"linux-x64;linux-arm64\\\"" +
                " -p:ContainerImageFormat=OCI" +
                " -p:ContainerAppCommandInstruction=Entrypoint" +
                " -p:ContainerGenerateLabels=false" +
                " -p:ContainerPublishInParallel=true" +
                " -p:ContainerPushNoCache=true",
                result.Args);
        }

        [Fact]
        public void Should_Quote_Container_Image_Tags()
        {
            // Given
            var fixture = new DotNetPublisherFixture();
            fixture.Settings.Container = new DotNetPublishContainerSettings
            {
                ImageTags = ["1.0.0", "latest"]
            };

            // When
            var result = fixture.Run();

            // Then
            Assert.Equal("publish -t:PublishContainer -p:ContainerImageTags=\\\"1.0.0;latest\\\"", result.Args);
        }

        [Fact]
        public void Should_Escape_Container_Property_Values()
        {
            // Given
            var fixture = new DotNetPublisherFixture();
            fixture.Settings.Container = new DotNetPublishContainerSettings
            {
                ArchiveOutputPath = "/Working/my artifacts/image.tar.gz",
                ImageTags = ["a;b", "c"]
            };

            // When
            var result = fixture.Run();

            // Then
            Assert.Equal("publish -t:PublishContainer -p:ContainerImageTags=\\\"a%3Bb;c\\\" -p:ContainerArchiveOutputPath=/Working/my%20artifacts/image.tar.gz", result.Args);
        }

        [Fact]
        public void Should_Throw_If_Both_Container_Image_Tag_And_Image_Tags_Are_Set()
        {
            // Given
            var fixture = new DotNetPublisherFixture();
            fixture.Settings.Container = new DotNetPublishContainerSettings
            {
                ImageTag = "latest",
                ImageTags = ["1.0.0"]
            };

            // When
            var result = Record.Exception(() => fixture.Run());

            // Then
            AssertEx.IsArgumentException(result, "container", "Both ImageTag and ImageTags were provided, but only one or the other is allowed.");
        }

        [Fact]
        public void Should_Use_Container_OS_Over_Settings_OS()
        {
            // Given
            var fixture = new DotNetPublisherFixture();
            fixture.Settings.OS = "win";
            fixture.Settings.Container = new DotNetPublishContainerSettings
            {
                OS = DotNetContainerOS.Linux
            };

            // When
            var result = fixture.Run();

            // Then
            Assert.Equal("publish --os linux -t:PublishContainer", result.Args);
        }

        [Fact]
        public void Should_Use_Settings_OS_When_Container_OS_Is_Not_Set()
        {
            // Given
            var fixture = new DotNetPublisherFixture();
            fixture.Settings.OS = "linux";
            fixture.Settings.Container = new DotNetPublishContainerSettings
            {
                Arch = DotNetContainerArchitecture.Arm64
            };

            // When
            var result = fixture.Run();

            // Then
            Assert.Equal("publish --os linux --arch arm64 -t:PublishContainer", result.Args);
        }

        [Fact]
        public void Should_Append_Container_Settings_After_MSBuild_Settings()
        {
            // Given
            var fixture = new DotNetPublisherFixture();
            fixture.Settings.MSBuildSettings = new DotNetMSBuildSettings()
                .WithProperty("ContainerUser", "app");
            fixture.Settings.Container = new DotNetPublishContainerSettings
            {
                User = "root"
            };

            // When
            var result = fixture.Run();

            // Then
            Assert.Equal("publish /property:ContainerUser=app -t:PublishContainer -p:ContainerUser=root", result.Args);
        }
}
}
