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
    }
}