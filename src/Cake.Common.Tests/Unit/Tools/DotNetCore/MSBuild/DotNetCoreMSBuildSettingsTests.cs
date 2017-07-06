// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tools.DotNetCore.MSBuild;
using Xunit;

namespace Cake.Common.Tests.Unit.Tools.DotNetCore.MSBuild
{
    public sealed class DotNetCoreMSBuildSettingsTests
    {
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