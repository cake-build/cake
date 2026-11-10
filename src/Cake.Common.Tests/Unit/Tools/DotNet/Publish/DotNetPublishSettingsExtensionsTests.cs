// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tools.DotNet.Publish;
using Cake.Core.IO;

namespace Cake.Common.Tests.Unit.Tools.DotNet.Publish;

public sealed class DotNetPublishSettingsExtensionsTests
{
    public sealed class TheWithContainerArchiveMethod
    {
        [Fact]
        public void Should_Throw_If_Settings_Are_Null()
        {
            // Given, When
            var result = Record.Exception(() => DotNetPublishSettingsExtensions.WithContainerArchive(null, "./image.tar.gz"));

            // Then
            AssertEx.IsArgumentNullException(result, "settings");
        }

        [Fact]
        public void Should_Throw_If_Archive_Output_Path_Is_Null()
        {
            // Given
            var settings = new DotNetPublishSettings();

            // When
            var result = Record.Exception(() => settings.WithContainerArchive(null));

            // Then
            AssertEx.IsArgumentNullException(result, "archiveOutputPath");
        }

        [Fact]
        public void Should_Set_Archive_Output_Path_And_Clear_Other_Destinations()
        {
            // Given
            var settings = new DotNetPublishSettings
            {
                Container = new DotNetPublishContainerSettings
                {
                    BaseImage = "mcr.microsoft.com/dotnet/runtime:10.0",
                    Registry = "ghcr.io",
                    LocalRegistry = DotNetContainerLocalRegistry.Podman
                }
            };

            // When
            settings.WithContainerArchive("./image.tar.gz");

            // Then
            Assert.Equal("image.tar.gz", settings.Container.ArchiveOutputPath.FullPath);
            Assert.Equal("mcr.microsoft.com/dotnet/runtime:10.0", settings.Container.BaseImage);
            Assert.Null(settings.Container.Registry);
            Assert.Null(settings.Container.LocalRegistry);
        }

        [Fact]
        public void Should_Set_Base_Image_If_Specified()
        {
            // Given
            var settings = new DotNetPublishSettings
            {
                Container = new DotNetPublishContainerSettings
                {
                    BaseImage = "mcr.microsoft.com/dotnet/runtime:10.0"
                }
            };

            // When
            settings.WithContainerArchive("./image.tar.gz", "mcr.microsoft.com/dotnet/sdk:11.0-resolute-aot");

            // Then
            Assert.Equal("mcr.microsoft.com/dotnet/sdk:11.0-resolute-aot", settings.Container.BaseImage);
        }

        [Fact]
        public void Should_Return_The_Same_Settings()
        {
            // Given
            var settings = new DotNetPublishSettings();

            // When
            var result = settings.WithContainerArchive("./image.tar.gz");

            // Then
            Assert.Same(settings, result);
        }
    }

    public sealed class TheWithContainerLocalRegistryMethod
    {
        [Fact]
        public void Should_Throw_If_Settings_Are_Null()
        {
            // Given, When
            var result = Record.Exception(() => DotNetPublishSettingsExtensions.WithContainerLocalRegistry(null, DotNetContainerLocalRegistry.Docker, "cake-sdk"));

            // Then
            AssertEx.IsArgumentNullException(result, "settings");
        }

        [Fact]
        public void Should_Throw_If_Local_Registry_Is_Default()
        {
            // Given
            var settings = new DotNetPublishSettings();

            // When
            var result = Record.Exception(() => settings.WithContainerLocalRegistry(default, "cake-sdk"));

            // Then
            AssertEx.IsArgumentNullException(result, "localRegistry");
        }

        [Theory]
        [InlineData(null)]
        [InlineData(" ")]
        public void Should_Throw_If_Repository_Is_Null_Or_Whitespace(string repository)
        {
            // Given
            var settings = new DotNetPublishSettings();

            // When
            var result = Record.Exception(() => settings.WithContainerLocalRegistry(DotNetContainerLocalRegistry.Docker, repository));

            // Then
            Assert.IsAssignableFrom<ArgumentException>(result);
            Assert.Equal("repository", ((ArgumentException)result).ParamName);
        }

        [Fact]
        public void Should_Set_Local_Registry_Repository_And_Tag_And_Clear_Other_Destinations()
        {
            // Given
            var settings = new DotNetPublishSettings
            {
                Container = new DotNetPublishContainerSettings
                {
                    User = "root",
                    ArchiveOutputPath = "./image.tar.gz",
                    Registry = "ghcr.io",
                    ImageTags = ["1.0.0", "latest"]
                }
            };

            // When
            settings.WithContainerLocalRegistry(DotNetContainerLocalRegistry.Wslc, "cake-sdk", "test");

            // Then
            Assert.Equal(DotNetContainerLocalRegistry.Wslc, settings.Container.LocalRegistry);
            Assert.Equal("cake-sdk", settings.Container.Repository);
            Assert.Equal("test", settings.Container.ImageTag);
            Assert.Null(settings.Container.ImageTags);
            Assert.Equal("root", settings.Container.User);
            Assert.Null(settings.Container.ArchiveOutputPath);
            Assert.Null(settings.Container.Registry);
        }

        [Fact]
        public void Should_Keep_Image_Tags_If_Image_Tag_Is_Not_Set()
        {
            // Given
            var settings = new DotNetPublishSettings
            {
                Container = new DotNetPublishContainerSettings
                {
                    ImageTags = ["1.0.0", "latest"]
                }
            };

            // When
            settings.WithContainerLocalRegistry(DotNetContainerLocalRegistry.Podman, "cake-sdk");

            // Then
            Assert.Equal(["1.0.0", "latest"], settings.Container.ImageTags);
            Assert.Null(settings.Container.ImageTag);
        }

        [Fact]
        public void Should_Set_Base_Image_If_Specified()
        {
            // Given
            var settings = new DotNetPublishSettings
            {
                Container = new DotNetPublishContainerSettings
                {
                    BaseImage = "mcr.microsoft.com/dotnet/runtime:10.0"
                }
            };

            // When
            settings.WithContainerLocalRegistry(DotNetContainerLocalRegistry.Docker, "cake-sdk", "test", "mcr.microsoft.com/dotnet/sdk:11.0-resolute-aot");

            // Then
            Assert.Equal("mcr.microsoft.com/dotnet/sdk:11.0-resolute-aot", settings.Container.BaseImage);
            Assert.Equal("test", settings.Container.ImageTag);
        }

        [Fact]
        public void Should_Keep_Base_Image_If_Base_Image_Is_Not_Set()
        {
            // Given
            var settings = new DotNetPublishSettings
            {
                Container = new DotNetPublishContainerSettings
                {
                    BaseImage = "mcr.microsoft.com/dotnet/sdk:11.0-resolute-aot"
                }
            };

            // When
            settings.WithContainerLocalRegistry(DotNetContainerLocalRegistry.Docker, "cake-sdk");

            // Then
            Assert.Equal("mcr.microsoft.com/dotnet/sdk:11.0-resolute-aot", settings.Container.BaseImage);
        }

        [Fact]
        public void Should_Return_The_Same_Settings()
        {
            // Given
            var settings = new DotNetPublishSettings();

            // When
            var result = settings.WithContainerLocalRegistry(DotNetContainerLocalRegistry.Docker, "cake-sdk");

            // Then
            Assert.Same(settings, result);
        }
    }

    public sealed class TheWithContainerRegistryMethod
    {
        [Fact]
        public void Should_Throw_If_Settings_Are_Null()
        {
            // Given, When
            var result = Record.Exception(() => DotNetPublishSettingsExtensions.WithContainerRegistry(null, "ghcr.io", "devlead/cake-sdk"));

            // Then
            AssertEx.IsArgumentNullException(result, "settings");
        }

        [Theory]
        [InlineData(null)]
        [InlineData(" ")]
        public void Should_Throw_If_Registry_Is_Null_Or_Whitespace(string registry)
        {
            // Given
            var settings = new DotNetPublishSettings();

            // When
            var result = Record.Exception(() => settings.WithContainerRegistry(registry, "devlead/cake-sdk"));

            // Then
            Assert.IsAssignableFrom<ArgumentException>(result);
            Assert.Equal("registry", ((ArgumentException)result).ParamName);
        }

        [Fact]
        public void Should_Set_Registry_Repository_And_Tag_And_Clear_Other_Destinations()
        {
            // Given
            var settings = new DotNetPublishSettings
            {
                Container = new DotNetPublishContainerSettings
                {
                    BaseImage = "mcr.microsoft.com/dotnet/sdk:11.0-resolute-aot",
                    ArchiveOutputPath = "./image.tar.gz",
                    LocalRegistry = DotNetContainerLocalRegistry.Docker
                }
            };

            // When
            settings.WithContainerRegistry("ghcr.io", "devlead/cake-sdk", "latest");

            // Then
            Assert.Equal("ghcr.io", settings.Container.Registry);
            Assert.Equal("devlead/cake-sdk", settings.Container.Repository);
            Assert.Equal("latest", settings.Container.ImageTag);
            Assert.Equal("mcr.microsoft.com/dotnet/sdk:11.0-resolute-aot", settings.Container.BaseImage);
            Assert.Null(settings.Container.ArchiveOutputPath);
            Assert.Null(settings.Container.LocalRegistry);
        }

        [Fact]
        public void Should_Set_Base_Image_If_Specified()
        {
            // Given
            var settings = new DotNetPublishSettings
            {
                Container = new DotNetPublishContainerSettings
                {
                    BaseImage = "mcr.microsoft.com/dotnet/runtime:10.0"
                }
            };

            // When
            settings.WithContainerRegistry("ghcr.io", "devlead/cake-sdk", baseImage: "mcr.microsoft.com/dotnet/sdk:11.0-resolute-aot");

            // Then
            Assert.Equal("mcr.microsoft.com/dotnet/sdk:11.0-resolute-aot", settings.Container.BaseImage);
            Assert.Null(settings.Container.ImageTag);
        }

        [Fact]
        public void Should_Return_The_Same_Settings()
        {
            // Given
            var settings = new DotNetPublishSettings();

            // When
            var result = settings.WithContainerRegistry("ghcr.io", "devlead/cake-sdk");

            // Then
            Assert.Same(settings, result);
        }
    }

    public sealed class TheWithContainerBaseImageMethod
    {
        [Fact]
        public void Should_Throw_If_Settings_Are_Null()
        {
            // Given, When
            var result = Record.Exception(() => DotNetPublishSettingsExtensions.WithContainerBaseImage(null, "mcr.microsoft.com/dotnet/sdk:11.0-resolute-aot"));

            // Then
            AssertEx.IsArgumentNullException(result, "settings");
        }

        [Theory]
        [InlineData(null)]
        [InlineData(" ")]
        public void Should_Throw_If_Base_Image_Is_Null_Or_Whitespace(string baseImage)
        {
            // Given
            var settings = new DotNetPublishSettings();

            // When
            var result = Record.Exception(() => settings.WithContainerBaseImage(baseImage));

            // Then
            Assert.IsAssignableFrom<ArgumentException>(result);
            Assert.Equal("baseImage", ((ArgumentException)result).ParamName);
        }

        [Fact]
        public void Should_Set_Base_Image_And_Keep_Other_Settings()
        {
            // Given
            var settings = new DotNetPublishSettings
            {
                Container = new DotNetPublishContainerSettings
                {
                    BaseImage = "mcr.microsoft.com/dotnet/runtime:10.0",
                    Registry = "ghcr.io",
                    Repository = "devlead/cake-sdk"
                }
            };

            // When
            settings.WithContainerBaseImage("mcr.microsoft.com/dotnet/sdk:11.0-resolute-aot");

            // Then
            Assert.Equal("mcr.microsoft.com/dotnet/sdk:11.0-resolute-aot", settings.Container.BaseImage);
            Assert.Equal("ghcr.io", settings.Container.Registry);
            Assert.Equal("devlead/cake-sdk", settings.Container.Repository);
        }

        [Fact]
        public void Should_Create_Container_Settings_If_Not_Set()
        {
            // Given
            var settings = new DotNetPublishSettings();

            // When
            settings.WithContainerBaseImage("mcr.microsoft.com/dotnet/sdk:11.0-resolute-aot");

            // Then
            Assert.Equal("mcr.microsoft.com/dotnet/sdk:11.0-resolute-aot", settings.Container.BaseImage);
        }

        [Fact]
        public void Should_Return_The_Same_Settings()
        {
            // Given
            var settings = new DotNetPublishSettings();

            // When
            var result = settings.WithContainerBaseImage("mcr.microsoft.com/dotnet/sdk:11.0-resolute-aot");

            // Then
            Assert.Same(settings, result);
        }
    }

    public sealed class TheContainerValueTypes
    {
        [Theory]
        [InlineData("Docker")]
        [InlineData("Podman")]
        [InlineData("Wslc")]
        [InlineData("MacOSContainer")]
        public void Should_Match_Known_Local_Registry_Values(string value)
        {
            // Given
            DotNetContainerLocalRegistry localRegistry = value;

            // When
            var result = localRegistry.ToString();

            // Then
            Assert.Equal(value, result);
            Assert.Contains(
                localRegistry,
                new[]
                {
                    DotNetContainerLocalRegistry.Docker,
                    DotNetContainerLocalRegistry.Podman,
                    DotNetContainerLocalRegistry.Wslc,
                    DotNetContainerLocalRegistry.MacOSContainer
                });
        }

        [Fact]
        public void Should_Convert_Custom_Local_Registry_From_String()
        {
            // Given, When
            DotNetContainerLocalRegistry localRegistry = "Future";

            // Then
            Assert.Equal("Future", localRegistry.Value);
        }

        [Fact]
        public void Should_Render_OS_Values()
        {
            // Given, When, Then
            Assert.Equal("linux", DotNetContainerOS.Linux.ToString());
            Assert.Equal("win", DotNetContainerOS.Windows.ToString());
        }

        [Fact]
        public void Should_Render_Architecture_Values()
        {
            // Given, When, Then
            Assert.Equal("x64", DotNetContainerArchitecture.X64.ToString());
            Assert.Equal("x86", DotNetContainerArchitecture.X86.ToString());
            Assert.Equal("arm64", DotNetContainerArchitecture.Arm64.ToString());
            Assert.Equal("arm", DotNetContainerArchitecture.Arm.ToString());
            Assert.Equal("s390x", DotNetContainerArchitecture.S390x.ToString());
            Assert.Equal("ppc64le", DotNetContainerArchitecture.Ppc64le.ToString());
            Assert.Equal("loongarch64", DotNetContainerArchitecture.LoongArch64.ToString());
            Assert.Equal("riscv64", DotNetContainerArchitecture.RiscV64.ToString());
        }
    }
}
