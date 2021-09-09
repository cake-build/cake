// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tools.DotNetCore.MSBuild;
using Xunit;

namespace Cake.Common.Tests.Unit.Tools.DotNetCore.MSBuild
{
    public sealed class DotNetCoreMSBuildSettingsTests
    {
        public sealed class TheVersionProperty
        {
            [Fact]
            public void Should_Be_Null_By_Default()
            {
                // Given
                var settings = new DotNetCoreMSBuildSettings();

                // Then
                Assert.Null(settings.Version);
            }

            [Fact]
            public void Should_Add_Version_Property_To_Configuration()
            {
                // Given
                var settings = new DotNetCoreMSBuildSettings();

                // When
                settings.Version = "1.0.0-test";

                // Then
                Assert.True(settings.Properties.ContainsKey("Version"));
                Assert.True(settings.Properties["Version"].Contains("1.0.0-test"));
            }
        }

        public sealed class TheVersionPrefixProperty
        {
            [Fact]
            public void Should_Be_Null_By_Default()
            {
                // Given
                var settings = new DotNetCoreMSBuildSettings();

                // Then
                Assert.Null(settings.VersionPrefix);
            }

            [Fact]
            public void Should_Add_VersionPrefix_Property_To_Configuration()
            {
                // Given
                var settings = new DotNetCoreMSBuildSettings();

                // When
                settings.VersionPrefix = "1.0.0";

                // Then
                Assert.True(settings.Properties.ContainsKey("VersionPrefix"));
                Assert.True(settings.Properties["VersionPrefix"].Contains("1.0.0"));
            }
        }

        public sealed class TheVersionSuffixProperty
        {
            [Fact]
            public void Should_Be_Null_By_Default()
            {
                // Given
                var settings = new DotNetCoreMSBuildSettings();

                // Then
                Assert.Null(settings.VersionSuffix);
            }

            [Fact]
            public void Should_Add_VersionSuffix_Property_To_Configuration()
            {
                // Given
                var settings = new DotNetCoreMSBuildSettings();

                // When
                settings.VersionSuffix = "test";

                // Then
                Assert.True(settings.Properties.ContainsKey("VersionSuffix"));
                Assert.True(settings.Properties["VersionSuffix"].Contains("test"));
            }
        }

        public sealed class TheFileVersionProperty
        {
            [Fact]
            public void Should_Be_Null_By_Default()
            {
                // Given
                var settings = new DotNetCoreMSBuildSettings();

                // Then
                Assert.Null(settings.FileVersion);
            }

            [Fact]
            public void Should_Add_FileVersion_Property_To_Configuration()
            {
                // Given
                var settings = new DotNetCoreMSBuildSettings();

                // When
                settings.FileVersion = "1.0.0.0";

                // Then
                Assert.True(settings.Properties.ContainsKey("FileVersion"));
                Assert.True(settings.Properties["FileVersion"].Contains("1.0.0.0"));
            }
        }

        public sealed class TheAssemblyVersionProperty
        {
            [Fact]
            public void Should_Be_Null_By_Default()
            {
                // Given
                var settings = new DotNetCoreMSBuildSettings();

                // Then
                Assert.Null(settings.AssemblyVersion);
            }

            [Fact]
            public void Should_Add_AssemblyVersion_Property_To_Configuration()
            {
                // Given
                var settings = new DotNetCoreMSBuildSettings();

                // When
                settings.AssemblyVersion = "1.0.0.0";

                // Then
                Assert.True(settings.Properties.ContainsKey("AssemblyVersion"));
                Assert.True(settings.Properties["AssemblyVersion"].Contains("1.0.0.0"));
            }
        }

        public sealed class TheInformationalVersionProperty
        {
            [Fact]
            public void Should_Be_Null_By_Default()
            {
                // Given
                var settings = new DotNetCoreMSBuildSettings();

                // Then
                Assert.Null(settings.InformationalVersion);
            }

            [Fact]
            public void Should_Add_InformationalVersion_Property_To_Configuration()
            {
                // Given
                var settings = new DotNetCoreMSBuildSettings();

                // When
                settings.InformationalVersion = "1.0.0-test+7ad03d0";

                // Then
                Assert.True(settings.Properties.ContainsKey("InformationalVersion"));
                Assert.True(settings.Properties["InformationalVersion"].Contains("1.0.0-test+7ad03d0"));
            }
        }

        public sealed class ThePackageVersionProperty
        {
            [Fact]
            public void Should_Be_Null_By_Default()
            {
                // Given
                var settings = new DotNetCoreMSBuildSettings();

                // Then
                Assert.Null(settings.PackageVersion);
            }

            [Fact]
            public void Should_Add_PackageVersion_Property_To_Configuration()
            {
                // Given
                var settings = new DotNetCoreMSBuildSettings();

                // When
                settings.PackageVersion = "1.0.0-test";

                // Then
                Assert.True(settings.Properties.ContainsKey("PackageVersion"));
                Assert.True(settings.Properties["PackageVersion"].Contains("1.0.0-test"));
            }
        }

        public sealed class ThePackageReleaseNotesProperty
        {
            [Fact]
            public void Should_Be_Null_By_Default()
            {
                // Given
                var settings = new DotNetCoreMSBuildSettings();

                // Then
                Assert.Null(settings.PackageReleaseNotes);
            }

            [Fact]
            public void Should_Add_PackageReleaseNotes_Property_To_Configuration()
            {
                // Given
                var settings = new DotNetCoreMSBuildSettings();

                // When
                settings.PackageReleaseNotes = "https://...";

                // Then
                Assert.True(settings.Properties.ContainsKey("PackageReleaseNotes"));
                Assert.True(settings.Properties["PackageReleaseNotes"].Contains("https://..."));
            }
        }

        public sealed class TheDistributedFileLoggerProperty
        {
            [Fact]
            public void Should_Be_Null_By_Default()
            {
                // Given
                var settings = new DotNetCoreMSBuildSettings();

                // Then
                Assert.False(settings.DistributedFileLogger);
            }
        }

        public sealed class TheValidateProjectFileProperty
        {
            [Fact]
            public void Should_Be_Null_By_Default()
            {
                // Given
                var settings = new DotNetCoreMSBuildSettings();

                // Then
                Assert.False(settings.ValidateProjectFile);
            }
        }

        public sealed class TheExcludeAutoResponseFilesProperty
        {
            [Fact]
            public void Should_Be_Null_By_Default()
            {
                // Given
                var settings = new DotNetCoreMSBuildSettings();

                // Then
                Assert.False(settings.ExcludeAutoResponseFiles);
            }
        }

        public sealed class TheTreatAllWarningsAsProperty
        {
            [Fact]
            public void Should_Be_Default_By_Default()
            {
                // Given
                var settings = new DotNetCoreMSBuildSettings();

                // Then
                Assert.Equal(MSBuildTreatAllWarningsAs.Default, settings.TreatAllWarningsAs);
            }
        }

        public sealed class TheWarningCodesAsErrorProperty
        {
            [Fact]
            public void Should_Be_Empty_By_Default()
            {
                // Given
                var settings = new DotNetCoreMSBuildSettings();

                // Then
                Assert.Empty(settings.WarningCodesAsError);
            }
        }

        public sealed class TheWarningCodesAsMessageProperty
        {
            [Fact]
            public void Should_Be_Empty_By_Default()
            {
                // Given
                var settings = new DotNetCoreMSBuildSettings();

                // Then
                Assert.Empty(settings.WarningCodesAsMessage);
            }
        }

        public sealed class TheConsoleLoggerSettingsProperty
        {
            [Fact]
            public void Should_Be_Null_By_Default()
            {
                // Given
                var settings = new DotNetCoreMSBuildSettings();

                // Then
                Assert.Null(settings.ConsoleLoggerSettings);
            }
        }

        public sealed class TheVerbosityProperty
        {
            [Fact]
            public void Should_Be_Null_By_Default()
            {
                // Given
                var settings = new DotNetCoreMSBuildSettings();

                // Then
                Assert.Null(settings.Verbosity);
            }
        }

        public sealed class TheToolVersionProperty
        {
            [Fact]
            public void Should_Be_Null_By_Default()
            {
                // Given
                var settings = new DotNetCoreMSBuildSettings();

                // Then
                Assert.Null(settings.ToolVersion);
            }
        }

        public sealed class TheTargetsProperty
        {
            [Fact]
            public void Should_Be_Empty_By_Default()
            {
                // Given
                var settings = new DotNetCoreMSBuildSettings();

                // Then
                Assert.Empty(settings.Targets);
            }
        }

        public sealed class ThePropertiesProperty
        {
            [Fact]
            public void Should_Be_Empty_By_Default()
            {
                // Given
                var settings = new DotNetCoreMSBuildSettings();

                // Then
                Assert.Empty(settings.Properties);
            }

            [Fact]
            public void Should_Return_A_Dictionary_That_Is_Case_Insensitive()
            {
                // Given
                var settings = new DotNetCoreMSBuildSettings();

                // When
                settings.Properties.Add("THEKEY", new[] { "THEVALUE" });

                // Then
                Assert.True(settings.Properties.ContainsKey("thekey"));
            }
        }

        public sealed class TheMaxCpuCountProperty
        {
            [Fact]
            public void Should_Be_Null_By_Default()
            {
                // Given
                var settings = new DotNetCoreMSBuildSettings();

                // Then
                Assert.Null(settings.MaxCpuCount);
            }
        }

        public sealed class TheDetailedSummaryProperty
        {
            [Fact]
            public void Should_Be_Null_By_Default()
            {
                // Given
                var settings = new DotNetCoreMSBuildSettings();

                // Then
                Assert.False(settings.DetailedSummary);
            }
        }

        public sealed class TheDisableConsoleLoggerProperty
        {
            [Fact]
            public void Should_Be_Null_By_Default()
            {
                // Given
                var settings = new DotNetCoreMSBuildSettings();

                // Then
                Assert.False(settings.DisableConsoleLogger);
            }
        }

        public sealed class TheLoggersProperty
        {
            [Fact]
            public void Should_Be_Empty_By_Default()
            {
                // Given
                var settings = new DotNetCoreMSBuildSettings();

                // Then
                Assert.Empty(settings.Loggers);
            }
        }

        public sealed class TheFileLoggersProperty
        {
            [Fact]
            public void Should_Be_Empty_By_Default()
            {
                // Given
                var settings = new DotNetCoreMSBuildSettings();

                // Then
                Assert.Empty(settings.FileLoggers);
            }
        }

        public sealed class TheIgnoreProjectExtensionsProperty
        {
            [Fact]
            public void Should_Be_Empty_By_Default()
            {
                // Given
                var settings = new DotNetCoreMSBuildSettings();

                // Then
                Assert.Empty(settings.IgnoreProjectExtensions);
            }
        }

        public sealed class TheResponseFilesProperty
        {
            [Fact]
            public void Should_Be_Empty_By_Default()
            {
                // Given
                var settings = new DotNetCoreMSBuildSettings();

                // Then
                Assert.Empty(settings.ResponseFiles);
            }
        }

        public sealed class TheDistributedLoggersProperty
        {
            [Fact]
            public void Should_Be_Empty_By_Default()
            {
                // Given
                var settings = new DotNetCoreMSBuildSettings();

                // Then
                Assert.Empty(settings.DistributedLoggers);
            }
        }
    }
}