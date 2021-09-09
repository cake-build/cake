// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Linq;
using Cake.Common.Tools.MSBuild;
using Cake.Core.Diagnostics;
using Xunit;

namespace Cake.Common.Tests.Unit.Tools.MSBuild
{
    public sealed class MSBuildSettingsExtensionsTests
    {
        public sealed class TheWithTargetMethod
        {
            [Fact]
            public void Should_Add_Target_To_Configuration()
            {
                // Given
                var settings = new MSBuildSettings();

                // When
                settings.WithTarget("Target");

                // Then
                Assert.True(settings.Targets.Contains("Target"));
            }

            [Fact]
            public void Should_Return_The_Same_Configuration()
            {
                // Given
                var settings = new MSBuildSettings();

                // When
                var result = settings.WithTarget("Target");

                // Then
                Assert.Equal(settings, result);
            }
        }

        public sealed class TheUseToolVersionMethod
        {
            [Fact]
            public void Should_Set_Tool_Version()
            {
                // Given
                var settings = new MSBuildSettings();

                // When
                settings.UseToolVersion(MSBuildToolVersion.NET35);

                // Then
                Assert.Equal(MSBuildToolVersion.NET35, settings.ToolVersion);
            }

            [Fact]
            public void Should_Return_The_Same_Configuration()
            {
                // Given
                var settings = new MSBuildSettings();

                // When
                var result = settings.UseToolVersion(MSBuildToolVersion.NET35);

                // Then
                Assert.Equal(settings, result);
            }

            [Fact]
            public void Should_Set_Tool_Version_FromStringOverride()
            {
                // Given
                var settings = new MSBuildSettings();

                // When
                settings.UseToolVersion("net20");

                // Then
                Assert.Equal(MSBuildToolVersion.NET20, settings.ToolVersion);
            }

            [Fact]
            public void Should_Return_The_Same_Configuration_FromStringOverride()
            {
                // Given
                var settings = new MSBuildSettings();

                // When
                var result = settings.UseToolVersion("net35");

                // Then
                Assert.Equal(settings, result);
            }

            [Fact]
            public void Should_ThrowArgumetnException_When_String_Is_Null()
            {
                // Given
                var settings = new MSBuildSettings();

                // Then
                Assert.Throws<System.ArgumentException>(() => settings.UseToolVersion(null));
            }

            [Fact]
            public void Should_Return_Default_When_Input_Is_Bad()
            {
                // Given
                var settings = new MSBuildSettings();

                // When
                settings.UseToolVersion("fjaldskjflaksdjflkas");

                // Then
                Assert.Equal(MSBuildToolVersion.Default, settings.ToolVersion);
            }

            [Fact]
            public void Should_Return_VSCustom_When_VsVersion_CantBeMatch()
            {
                var settings = new MSBuildSettings();

                // When
                settings.UseToolVersion("2022");

                // Then
                Assert.Equal(MSBuildToolVersion.VSCustom, settings.ToolVersion);
            }

            [Fact]
            public void Should_Return_VSCustom_When_NETFramework_CantBeMatch()
            {
                var settings = new MSBuildSettings();

                // When
                settings.UseToolVersion("5");

                // Then
                Assert.Equal(MSBuildToolVersion.NETCustom, settings.ToolVersion);
            }
        }

        public sealed class TheSetPlatformTargetMethod
        {
            [Fact]
            public void Should_Set_Platform_Target()
            {
                // Given
                var settings = new MSBuildSettings();

                // When
                settings.SetPlatformTarget(PlatformTarget.x64);

                // Then
                Assert.Equal(PlatformTarget.x64, settings.PlatformTarget);
            }

            [Fact]
            public void Should_Return_The_Same_Configuration()
            {
                // Given
                var settings = new MSBuildSettings();

                // When
                var result = settings.SetPlatformTarget(PlatformTarget.x64);

                // Then
                Assert.Equal(settings, result);
            }
        }

        public sealed class TheSetPlatformTargetStringMethod
        {
            [Fact]
            public void Should_Set_Platform_Target_Via_Property()
            {
                // Given
                var settings = new MSBuildSettings();

                // When
                settings.SetPlatformTarget("Custom");

                // Then
                Assert.True(settings.Properties.ContainsKey("Platform"));
                Assert.Equal("Custom", string.Join(string.Empty, settings.Properties["Platform"]));
            }

            [Fact]
            public void Should_Return_The_Same_Configuration()
            {
                // Given
                var settings = new MSBuildSettings();

                // When
                var result = settings.SetPlatformTarget("Custom");

                // Then
                Assert.Equal(settings, result);
            }
        }

        public sealed class TheWithPropertyMethod
        {
            [Fact]
            public void Should_Add_Property_To_Configuration()
            {
                // Given
                var settings = new MSBuildSettings();

                // When
                settings.WithProperty("PropertyName", "Value");

                // Then
                Assert.True(settings.Properties.ContainsKey("PropertyName"));
            }

            [Fact]
            public void Should_Return_The_Same_Configuration()
            {
                // Given
                var settings = new MSBuildSettings();

                // When
                var result = settings.WithProperty("PropertyName", "Value");

                // Then
                Assert.Equal(settings, result);
            }
        }

        public sealed class TheSetConfigurationMethod
        {
            [Fact]
            public void Should_Set_Configuration()
            {
                // Given
                var settings = new MSBuildSettings();

                // When
                settings.SetConfiguration("TheConfiguration");

                // Then
                Assert.Equal("TheConfiguration", settings.Configuration);
            }

            [Fact]
            public void Should_Return_The_Same_Configuration()
            {
                // Given
                var settings = new MSBuildSettings();

                // When
                var result = settings.SetConfiguration("TheConfiguration");

                // Then
                Assert.Equal(settings, result);
            }
        }

        public sealed class TheSetMaxCpuCountMethod
        {
            [Fact]
            public void Should_Set_MaxCpuCount()
            {
                // Given
                var settings = new MSBuildSettings();

                // When
                settings.SetMaxCpuCount(4);

                // Then
                Assert.Equal(4, settings.MaxCpuCount);
            }

            [Fact]
            public void Should_Set_MaxCpuCount_To_Zero_If_Negative_Value()
            {
                // Given
                var settings = new MSBuildSettings();

                // When
                settings.SetMaxCpuCount(-1);

                // Then
                Assert.Equal(0, settings.MaxCpuCount);
            }

            [Fact]
            public void Should_Return_The_Same_Configuration()
            {
                // Given
                var settings = new MSBuildSettings();

                // When
                var result = settings.SetMaxCpuCount(4);

                // Then
                Assert.Equal(settings, result);
            }
        }

        public sealed class TheNodeReuseMethod
        {
            [Theory]
            [InlineData(true)]
            [InlineData(false)]
            public void Should_Set_Node_Reuse(bool reuse)
            {
                // Given
                var settings = new MSBuildSettings();

                // When
                settings.SetNodeReuse(reuse);

                // Then
                Assert.Equal(reuse, settings.NodeReuse);
            }

            [Fact]
            public void Should_Return_The_Same_Configuration()
            {
                // Given
                var settings = new MSBuildSettings();

                // When
                var result = settings.SetNodeReuse(true);

                // Then
                Assert.Equal(settings, result);
            }
        }

        public sealed class TheDetailedSummaryMethod
        {
            [Theory]
            [InlineData(true)]
            [InlineData(false)]
            public void Should_Set_Detailed_Summary(bool detailedSummary)
            {
                // Given
                var settings = new MSBuildSettings();

                // When
                settings.SetDetailedSummary(detailedSummary);

                // Then
                Assert.Equal(detailedSummary, settings.DetailedSummary);
            }

            [Fact]
            public void Should_Return_The_Same_Configuration()
            {
                // Given
                var settings = new MSBuildSettings();

                // When
                var result = settings.SetDetailedSummary(true);

                // Then
                Assert.Equal(settings, result);
            }
        }

        public sealed class TheNoConsoleLoggerMethod
        {
            [Theory]
            [InlineData(true)]
            [InlineData(false)]
            public void Should_Set_No_Console_Logger(bool noConsoleLog)
            {
                // Given
                var settings = new MSBuildSettings();

                // When
                settings.SetNoConsoleLogger(noConsoleLog);

                // Then
                Assert.Equal(noConsoleLog, settings.NoConsoleLogger);
            }

            [Fact]
            public void Should_Return_The_Same_Configuration()
            {
                // Given
                var settings = new MSBuildSettings();

                // When
                var result = settings.SetNoConsoleLogger(true);

                // Then
                Assert.Equal(settings, result);
            }
        }

        public sealed class TheNoLogoMethod
        {
            [Theory]
            [InlineData(true)]
            [InlineData(false)]
            public void Should_Set_No_Logo(bool noLogo)
            {
                // Given
                var settings = new MSBuildSettings();

                // When
                settings.SetNoLogo(noLogo);

                // Then
                Assert.Equal(noLogo, settings.NoLogo);
            }

            [Fact]
            public void Should_Return_The_Same_Configuration()
            {
                // Given
                var settings = new MSBuildSettings();

                // When
                var result = settings.SetNoLogo(true);

                // Then
                Assert.Equal(settings, result);
            }
        }

        public sealed class TheSetVersionMethod
        {
            private const string Version = "1.0.0-test";

            [Fact]
            public void Should_Set_Version()
            {
                // Given
                var settings = new MSBuildSettings();
                const string key = "Version";

                // When
                settings.SetVersion(Version);

                // Then
                Assert.True(settings.Properties.ContainsKey(key));
                Assert.Equal(Version, settings.Properties[key].FirstOrDefault());
                Assert.Equal(Version, settings.Version);
            }

            [Fact]
            public void Should_Return_The_Same_Configuration()
            {
                // Given
                var settings = new MSBuildSettings();

                // When
                var result = settings.SetVersion(Version);

                // Then
                Assert.Equal(settings, result);
            }
        }

        public sealed class TheSetVersionPrefixMethod
        {
            private const string VersionPrefix = "1.0.0";

            [Fact]
            public void Should_Set_VersionPrefix()
            {
                // Given
                var settings = new MSBuildSettings();
                const string key = "VersionPrefix";

                // When
                settings.SetVersionPrefix(VersionPrefix);

                // Then
                Assert.True(settings.Properties.ContainsKey(key));
                Assert.Equal(VersionPrefix, settings.Properties[key].FirstOrDefault());
                Assert.Equal(VersionPrefix, settings.VersionPrefix);
            }

            [Fact]
            public void Should_Return_The_Same_Configuration()
            {
                // Given
                var settings = new MSBuildSettings();

                // When
                var result = settings.SetVersionPrefix(VersionPrefix);

                // Then
                Assert.Equal(settings, result);
            }
        }

        public sealed class TheSetVersionSuffixMethod
        {
            private const string VersionSuffix = "test";

            [Fact]
            public void Should_Set_VersionSuffix()
            {
                // Given
                var settings = new MSBuildSettings();
                const string key = "VersionSuffix";

                // When
                settings.SetVersionSuffix(VersionSuffix);

                // Then
                Assert.True(settings.Properties.ContainsKey(key));
                Assert.Equal(VersionSuffix, settings.Properties[key].FirstOrDefault());
                Assert.Equal(VersionSuffix, settings.VersionSuffix);
            }

            [Fact]
            public void Should_Return_The_Same_Configuration()
            {
                // Given
                var settings = new MSBuildSettings();

                // When
                var result = settings.SetVersionSuffix(VersionSuffix);

                // Then
                Assert.Equal(settings, result);
            }
        }

        public sealed class TheSetFileVersionMethod
        {
            private const string FileVersion = "1.0.0.0";

            [Fact]
            public void Should_Set_FileVersion()
            {
                // Given
                var settings = new MSBuildSettings();
                const string key = "FileVersion";

                // When
                settings.SetFileVersion(FileVersion);

                // Then
                Assert.True(settings.Properties.ContainsKey(key));
                Assert.Equal(FileVersion, settings.Properties[key].FirstOrDefault());
                Assert.Equal(FileVersion, settings.FileVersion);
            }

            [Fact]
            public void Should_Return_The_Same_Configuration()
            {
                // Given
                var settings = new MSBuildSettings();

                // When
                var result = settings.SetFileVersion(FileVersion);

                // Then
                Assert.Equal(settings, result);
            }
        }

        public sealed class TheSetAssemblyVersionMethod
        {
            private const string AssemblyVersion = "1.0.0.0";

            [Fact]
            public void Should_Set_AssemblyVersion()
            {
                // Given
                var settings = new MSBuildSettings();
                const string key = "AssemblyVersion";

                // When
                settings.SetAssemblyVersion(AssemblyVersion);

                // Then
                Assert.True(settings.Properties.ContainsKey(key));
                Assert.Equal(AssemblyVersion, settings.Properties[key].FirstOrDefault());
                Assert.Equal(AssemblyVersion, settings.AssemblyVersion);
            }

            [Fact]
            public void Should_Return_The_Same_Configuration()
            {
                // Given
                var settings = new MSBuildSettings();

                // When
                var result = settings.SetAssemblyVersion(AssemblyVersion);

                // Then
                Assert.Equal(settings, result);
            }
        }

        public sealed class TheSetInformationalVersionMethod
        {
            private const string InformationalVersion = "1.0.0-test+7ad03d0";

            [Fact]
            public void Should_Set_InformationalVersion()
            {
                // Given
                var settings = new MSBuildSettings();
                const string key = "InformationalVersion";

                // When
                settings.SetInformationalVersion(InformationalVersion);

                // Then
                Assert.True(settings.Properties.ContainsKey(key));
                Assert.Equal(InformationalVersion, settings.Properties[key].FirstOrDefault());
                Assert.Equal(InformationalVersion, settings.InformationalVersion);
            }

            [Fact]
            public void Should_Return_The_Same_Configuration()
            {
                // Given
                var settings = new MSBuildSettings();

                // When
                var result = settings.SetInformationalVersion(InformationalVersion);

                // Then
                Assert.Equal(settings, result);
            }
        }

        public sealed class TheSetPackageVersionMethod
        {
            private const string PackageVersion = "1.0.0-test";

            [Fact]
            public void Should_Set_PackageVersion()
            {
                // Given
                var settings = new MSBuildSettings();
                const string key = "PackageVersion";

                // When
                settings.SetPackageVersion(PackageVersion);

                // Then
                Assert.True(settings.Properties.ContainsKey(key));
                Assert.Equal(PackageVersion, settings.Properties[key].FirstOrDefault());
                Assert.Equal(PackageVersion, settings.PackageVersion);
            }

            [Fact]
            public void Should_Return_The_Same_Configuration()
            {
                // Given
                var settings = new MSBuildSettings();

                // When
                var result = settings.SetPackageVersion(PackageVersion);

                // Then
                Assert.Equal(settings, result);
            }
        }

        public sealed class TheSetPackageReleaseNotesMethod
        {
            private const string PackageReleaseNotes = "1.0.0-test";

            [Fact]
            public void Should_Set_PackageReleaseNotes()
            {
                // Given
                var settings = new MSBuildSettings();
                const string key = "PackageReleaseNotes";

                // When
                settings.SetPackageReleaseNotes(PackageReleaseNotes);

                // Then
                Assert.True(settings.Properties.ContainsKey(key));
                Assert.Equal(PackageReleaseNotes, settings.Properties[key].FirstOrDefault());
                Assert.Equal(PackageReleaseNotes, settings.PackageReleaseNotes);
            }

            [Fact]
            public void Should_Return_The_Same_Configuration()
            {
                // Given
                var settings = new MSBuildSettings();

                // When
                var result = settings.SetPackageReleaseNotes(PackageReleaseNotes);

                // Then
                Assert.Equal(settings, result);
            }
        }

        public sealed class TheSetContinuousIntegrationBuildMethod
        {
            [Theory]
            [InlineData(null)]
            [InlineData(true)]
            [InlineData(false)]
            public void Should_Set_ContinuousIntegrationBuild(bool? continuousIntegrationBuild)
            {
                // Given
                var settings = new MSBuildSettings();

                // When
                settings.SetContinuousIntegrationBuild(continuousIntegrationBuild);

                // Then
                Assert.Equal(continuousIntegrationBuild, settings.ContinuousIntegrationBuild);
            }

            [Fact]
            public void Should_Return_The_Same_Configuration()
            {
                // Given
                var settings = new MSBuildSettings();

                // When
                var result = settings.SetContinuousIntegrationBuild();

                // Then
                Assert.Equal(settings, result);
            }
        }

        public sealed class TheNoImplicitTargetMethod
        {
            [Theory]
            [InlineData(true)]
            [InlineData(false)]
            public void Should_Set_No_Implicit_Target(bool noImplicitTarget)
            {
                // Given
                var settings = new MSBuildSettings();

                // When
                settings.SetNoImplicitTarget(noImplicitTarget);

                // Then
                Assert.Equal(noImplicitTarget, settings.NoImplicitTarget);
            }

            [Fact]
            public void Should_Return_The_Same_Configuration()
            {
                // Given
                var settings = new MSBuildSettings();

                // When
                var result = settings.SetNoImplicitTarget(true);

                // Then
                Assert.Equal(settings, result);
            }
        }

        public sealed class TheSetRestoreLockedModeMethod
        {
            [Theory]
            [InlineData(true)]
            [InlineData(false)]
            public void Should_Set_RestoreLockedMode(bool restoreLockedMode)
            {
                // Given
                var settings = new MSBuildSettings();

                // When
                settings.SetRestoreLockedMode(restoreLockedMode);

                // Then
                Assert.Equal(restoreLockedMode, settings.RestoreLockedMode);
            }

            [Fact]
            public void Should_Return_The_Same_Configuration()
            {
                // Given
                var settings = new MSBuildSettings();

                // When
                var result = settings.SetRestoreLockedMode(true);

                // Then
                Assert.Equal(settings, result);
            }
        }

        public sealed class TheSetVerbosityMethod
        {
            [Theory]
            [InlineData(Verbosity.Quiet)]
            [InlineData(Verbosity.Minimal)]
            [InlineData(Verbosity.Normal)]
            [InlineData(Verbosity.Verbose)]
            [InlineData(Verbosity.Diagnostic)]
            public void Should_Set_Verbosity(Verbosity verbosity)
            {
                // Given
                var settings = new MSBuildSettings();

                // When
                settings.SetVerbosity(verbosity);

                // Then
                Assert.Equal(verbosity, settings.Verbosity);
            }

            [Fact]
            public void Should_Return_The_Same_Configuration()
            {
                // Given
                var settings = new MSBuildSettings();

                // When
                var result = settings.SetVerbosity(Verbosity.Normal);

                // Then
                Assert.Equal(settings, result);
            }
        }

        public sealed class TheWithLoggersMethod
        {
            [Fact]
            public void Should_Add_Logger()
            {
                // Given
                var settings = new MSBuildSettings();

                // When
                settings.WithLogger("LoggerAssembly1", "LoggerClass1", "LoggerParameters1");
                settings.WithLogger("LoggerAssembly2", "LoggerClass2", "LoggerParameters2");

                // Then
                var loggers = settings.Loggers.ToArray();
                Assert.Equal(2, loggers.Length);
                Assert.Equal("LoggerAssembly1", loggers[0].Assembly);
                Assert.Equal("LoggerClass1", loggers[0].Class);
                Assert.Equal("LoggerParameters1", loggers[0].Parameters);
                Assert.Equal("LoggerAssembly2", loggers[1].Assembly);
                Assert.Equal("LoggerClass2", loggers[1].Class);
                Assert.Equal("LoggerParameters2", loggers[1].Parameters);
            }

            [Fact]
            public void Should_Return_The_Same_Configuration()
            {
                // Given
                var settings = new MSBuildSettings();

                // When
                var result = settings.WithLogger("Logger");

                // Then
                Assert.Equal(settings, result);
            }
        }

        public sealed class TheAddFileLoggersMethod
        {
            [Fact]
            public void Should_Add_Logger()
            {
                // Given
                var settings = new MSBuildSettings();
                var fileLogger = new MSBuildFileLogger();
                var fileLogger2 = new MSBuildFileLogger { LogFile = "A" };

                // When
                settings.AddFileLogger(fileLogger);
                settings.AddFileLogger(fileLogger2);

                // Then
                var loggers = settings.FileLoggers.ToArray();
                Assert.Equal(2, loggers.Length);
                Assert.Equal(fileLogger, loggers[0]);
                Assert.Equal(fileLogger2, loggers[1]);
                Assert.Equal("A", loggers[1].LogFile.FullPath);
            }

            [Fact]
            public void Should_Return_The_Same_Configuration()
            {
                // Given
                var settings = new MSBuildSettings();

                // When
                var result = settings.AddFileLogger(new MSBuildFileLogger());
                var result1 = settings.AddFileLogger();

                // Then
                Assert.Equal(settings, result);
                Assert.Equal(settings, result1);
            }
        }

        public sealed class TheEnableBinaryLoggerMethod
        {
            [Fact]
            public void Should_Enable_BinaryLogger()
            {
                // Given
                var settings = new MSBuildSettings();

                // When
                settings.EnableBinaryLogger();

                // Then
                Assert.NotNull(settings.BinaryLogger);
                Assert.True(settings.BinaryLogger.Enabled);
                Assert.Null(settings.BinaryLogger.FileName);
                Assert.Equal(MSBuildBinaryLogImports.Unspecified, settings.BinaryLogger.Imports);
            }

            [Fact]
            public void Should_Return_The_Same_Configuration()
            {
                // Given
                var settings = new MSBuildSettings();

                // When
                var result = settings.EnableBinaryLogger();

                // Then
                Assert.Equal(settings, result);
            }
        }

        public sealed class TheWithWarningsAsErrorMethod
        {
            [Fact]
            public void Should_Set_WarningsAsError()
            {
                // Given
                var settings = new MSBuildSettings();

                // When
                settings.WithWarningsAsError();

                // Then
                Assert.True(settings.WarningsAsError);
                Assert.Equal(0, settings.WarningsAsErrorCodes.Count);
            }

            [Fact]
            public void Should_Add_Codes()
            {
                // Given
                var settings = new MSBuildSettings();

                // When
                settings.WithWarningsAsError("12345");

                // Then
                Assert.True(settings.WarningsAsError);
                Assert.Equal(1, settings.WarningsAsErrorCodes.Count);
                Assert.Equal("12345", settings.WarningsAsErrorCodes.First());
            }

            [Fact]
            public void Should_Return_The_Same_Configuration()
            {
                // Given
                var settings = new MSBuildSettings();

                // When
                var result = settings.WithWarningsAsError("12345");

                // Then
                Assert.Equal(settings, result);
            }
        }

        public sealed class TheWithWarningsAsMessageMethod
        {
            [Fact]
            public void Should_Add_Codes()
            {
                // Given
                var settings = new MSBuildSettings();

                // When
                settings.WithWarningsAsMessage("12345");

                // Then
                Assert.Equal(1, settings.WarningsAsMessageCodes.Count);
                Assert.Equal("12345", settings.WarningsAsMessageCodes.First());
            }

            [Fact]
            public void Should_Return_The_Same_Configuration()
            {
                // Given
                var settings = new MSBuildSettings();

                // When
                var result = settings.WithWarningsAsMessage("12345");

                // Then
                Assert.Equal(settings, result);
            }
        }

        public sealed class TheWithRestoreMethod
        {
            [Fact]
            public void Should_Set_Restore_To_True()
            {
                // Given
                var settings = new MSBuildSettings();

                // When
                settings.WithRestore();

                // Then
                Assert.Equal(true, settings.Restore);
            }

            [Fact]
            public void Should_Return_The_Same_Configuration()
            {
                // Given
                var settings = new MSBuildSettings();

                // When
                var result = settings.WithRestore();

                // Then
                Assert.Equal(settings, result);
            }
        }

        public sealed class TheWithConsoleLoggerParameterMethod
        {
            [Fact]
            public void Should_Add_Console_Logger_Parameter()
            {
                // Given
                var settings = new MSBuildSettings();

               // When
                settings.WithConsoleLoggerParameter("ForceConsoleColor");
                settings.WithConsoleLoggerParameter("ShowCommandLine");

               // Then
                Assert.Contains("ForceConsoleColor", settings.ConsoleLoggerParameters);
                Assert.Contains("ShowCommandLine", settings.ConsoleLoggerParameters);
            }

            [Fact]
            public void Should_Return_The_Same_Configuration()
            {
                // Given
                var settings = new MSBuildSettings();

               // When
                var result = settings.WithConsoleLoggerParameter("ForceConsoleColor");
                var result1 = settings.WithConsoleLoggerParameter("ShowCommandLine");

               // Then
                Assert.Equal(settings, result);
                Assert.Equal(settings, result1);
            }
        }
    }
}