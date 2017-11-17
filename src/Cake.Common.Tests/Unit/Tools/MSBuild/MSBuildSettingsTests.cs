// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tools.MSBuild;
using Cake.Core.Diagnostics;
using Xunit;

namespace Cake.Common.Tests.Unit.Tools.MSBuild
{
    public sealed class MSBuildSettingsTests
    {
        public sealed class TheConstructor
        {
            [Fact]
            public void Should_Set_Default_Tools_Version_To_Default()
            {
                // Given, When
                var settings = new MSBuildSettings();

                // Then
                Assert.Equal(MSBuildToolVersion.Default, settings.ToolVersion);
            }

            [Fact]
            public void Should_Set_Default_Platform_Target_To_Null()
            {
                // Given, When
                var settings = new MSBuildSettings();

                // Then
                Assert.Null(settings.PlatformTarget);
            }

            [Fact]
            public void Should_Set_Default_Verbosity_To_Normal()
            {
                // Given, When
                var settings = new MSBuildSettings();

                // Then
                Assert.Equal(Verbosity.Normal, settings.Verbosity);
            }
        }

        public sealed class TheTargetsProperty
        {
            [Fact]
            public void Should_Return_A_Set_That_Is_Case_Insensitive()
            {
                // Given
                var settings = new MSBuildSettings();

                // When
                settings.Targets.Add("TARGET");

                // Then
                Assert.True(settings.Targets.Contains("target"));
            }
        }

        public sealed class ThePropertiesProperty
        {
            [Fact]
            public void Should_Return_A_Dictionary_That_Is_Case_Insensitive()
            {
                // Given
                var settings = new MSBuildSettings();

                // When
                settings.Properties.Add("THEKEY", new[] { "THEVALUE" });

                // Then
                Assert.True(settings.Properties.ContainsKey("thekey"));
            }
        }

        public sealed class TheConfigurationProperty
        {
            [Fact]
            public void Should_Be_Empty_By_Default()
            {
                // Given, When
                var settings = new MSBuildSettings();

                // Then
                Assert.Equal(string.Empty, settings.Configuration);
            }
        }

        public sealed class ThePlatformProperty
        {
            [Fact]
            public void Should_Be_Null_By_Default()
            {
                // Given, When
                var settings = new MSBuildSettings();

                // Then
                Assert.Null(settings.PlatformTarget);
            }
        }

        public sealed class TheMaxCpuCountProperty
        {
            [Fact]
            public void Should_Be_Null_By_Default()
            {
                // Given
                var settings = new MSBuildSettings();

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
                var settings = new MSBuildSettings();

                // Then
                Assert.Null(settings.DetailedSummary);
            }
        }

        public sealed class TheNoConsoleLogProperty
        {
            [Fact]
            public void Should_Be_Null_By_Default()
            {
                // Given
                var settings = new MSBuildSettings();

                // Then
                Assert.Null(settings.NoConsoleLogger);
            }
        }

        public sealed class TheLoggersProperty
        {
            [Fact]
            public void Should_Be_Empty_By_Default()
            {
                // Given
                var settings = new MSBuildSettings();

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
                var settings = new MSBuildSettings();

                // Then
                Assert.Empty(settings.FileLoggers);
            }
        }

        public sealed class TheWarningsAsErrorCodesProperty
        {
            [Fact]
            public void Should_Be_Empty_By_Default()
            {
                // Given
                var settings = new MSBuildSettings();

                // Then
                Assert.Empty(settings.WarningsAsErrorCodes);
            }
        }

        public sealed class TheWarningsAsMessageCodesProperty
        {
            [Fact]
            public void Should_Be_Empty_By_Default()
            {
                // Given
                var settings = new MSBuildSettings();

                // Then
                Assert.Empty(settings.WarningsAsMessageCodes);
            }
        }
    }
}