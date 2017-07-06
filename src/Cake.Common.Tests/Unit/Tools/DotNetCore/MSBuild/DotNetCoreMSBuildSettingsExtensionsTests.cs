// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Linq;
using Cake.Common.Tools.DotNetCore.MSBuild;
using Cake.Common.Tools.MSBuild;
using Cake.Core.IO;
using Xunit;

namespace Cake.Common.Tests.Unit.Tools.DotNetCore.MSBuild
{
    public sealed class DotNetCoreMSBuildSettingsExtensionsTests
    {
        public sealed class TheWithTargetMethod
        {
            [Fact]
            public void Should_Add_Target_To_Configuration()
            {
                // Given
                var settings = new DotNetCoreMSBuildSettings();

                // When
                settings.WithTarget("Target");

                // Then
                Assert.True(settings.Targets.Contains("Target"));
            }

            [Fact]
            public void Should_Return_The_Same_Configuration()
            {
                // Given
                var settings = new DotNetCoreMSBuildSettings();

                // When
                var result = settings.WithTarget("Target");

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
                var settings = new DotNetCoreMSBuildSettings();

                // When
                settings.WithProperty("PropertyName", "Value");

                // Then
                Assert.True(settings.Properties.ContainsKey("PropertyName"));
            }

            [Fact]
            public void Should_Return_The_Same_Configuration()
            {
                // Given
                var settings = new DotNetCoreMSBuildSettings();

                // When
                var result = settings.WithProperty("PropertyName", "Value");

                // Then
                Assert.Equal(settings, result);
            }
        }

        public sealed class TheDetailedSummaryMethod
        {
            [Fact]
            public void Should_Set_Detailed_Summary()
            {
                // Given
                var settings = new DotNetCoreMSBuildSettings();

                // When
                settings.ShowDetailedSummary();

                // Then
                Assert.True(settings.DetailedSummary);
            }

            [Fact]
            public void Should_Return_The_Same_Configuration()
            {
                // Given
                var settings = new DotNetCoreMSBuildSettings();

                // When
                var result = settings.ShowDetailedSummary();

                // Then
                Assert.Equal(settings, result);
            }
        }

        public sealed class TheWithIgnoredProjectExtensionMethod
        {
            [Fact]
            public void Should_Add_Ignored_Extension_To_Configuration()
            {
                // Given
                var settings = new DotNetCoreMSBuildSettings();

                // When
                settings.WithIgnoredProjectExtension(".sln");

                // Then
                Assert.True(settings.IgnoreProjectExtensions.Contains(".sln"));
            }

            [Fact]
            public void Should_Return_The_Same_Configuration()
            {
                // Given
                var settings = new DotNetCoreMSBuildSettings();

                // When
                var result = settings.WithIgnoredProjectExtension(".sln");

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
                var settings = new DotNetCoreMSBuildSettings();

                // When
                settings.SetMaxCpuCount(4);

                // Then
                Assert.Equal(4, settings.MaxCpuCount);
            }

            [Fact]
            public void Should_Set_MaxCpuCount_To_Zero_If_Negative_Value()
            {
                // Given
                var settings = new DotNetCoreMSBuildSettings();

                // When
                settings.SetMaxCpuCount(-1);

                // Then
                Assert.Equal(0, settings.MaxCpuCount);
            }

            [Fact]
            public void Should_Return_The_Same_Configuration()
            {
                // Given
                var settings = new DotNetCoreMSBuildSettings();

                // When
                var result = settings.SetMaxCpuCount(4);

                // Then
                Assert.Equal(settings, result);
            }
        }

        public sealed class TheExcludeAutoResponseFilesMethod
        {
            [Fact]
            public void Should_Set_Exclude_Auto_Response_Files()
            {
                // Given
                var settings = new DotNetCoreMSBuildSettings();

                // When
                settings.ExcludeAutoResponseFiles();

                // Then
                Assert.True(settings.ExcludeAutoResponseFiles);
            }

            [Fact]
            public void Should_Return_The_Same_Configuration()
            {
                // Given
                var settings = new DotNetCoreMSBuildSettings();

                // When
                var result = settings.ExcludeAutoResponseFiles();

                // Then
                Assert.Equal(settings, result);
            }
        }

        public sealed class TheHideLogoMethod
        {
            [Fact]
            public void Should_Set_No_Logo()
            {
                // Given
                var settings = new DotNetCoreMSBuildSettings();

                // When
                settings.HideLogo();

                // Then
                Assert.True(settings.NoLogo);
            }

            [Fact]
            public void Should_Return_The_Same_Configuration()
            {
                // Given
                var settings = new DotNetCoreMSBuildSettings();

                // When
                var result = settings.HideLogo();

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
                var settings = new DotNetCoreMSBuildSettings();

                // When
                settings.UseToolVersion(MSBuildVersion.MSBuild35);

                // Then
                Assert.Equal(MSBuildVersion.MSBuild35, settings.ToolVersion);
            }

            [Fact]
            public void Should_Return_The_Same_Configuration()
            {
                // Given
                var settings = new DotNetCoreMSBuildSettings();

                // When
                var result = settings.UseToolVersion(MSBuildVersion.MSBuild35);

                // Then
                Assert.Equal(settings, result);
            }
        }

        public sealed class TheValidateProjectFileMethod
        {
            [Fact]
            public void Should_Set_Validate_Project_File()
            {
                // Given
                var settings = new DotNetCoreMSBuildSettings();

                // When
                settings.ValidateProjectFile();

                // Then
                Assert.True(settings.ValidateProjectFile);
            }

            [Fact]
            public void Should_Return_The_Same_Configuration()
            {
                // Given
                var settings = new DotNetCoreMSBuildSettings();

                // When
                var result = settings.ValidateProjectFile();

                // Then
                Assert.Equal(settings, result);
            }
        }

        public sealed class TheWithResponseFileMethod
        {
            [Fact]
            public void Should_Add_Response_File_To_Configuration()
            {
                // Given
                var settings = new DotNetCoreMSBuildSettings();
                var filePath = FilePath.FromString(".sln");

                // When
                settings.WithResponseFile(filePath);

                // Then
                Assert.True(settings.ResponseFiles.Contains(filePath));
            }

            [Fact]
            public void Should_Return_The_Same_Configuration()
            {
                // Given
                var settings = new DotNetCoreMSBuildSettings();

                // When
                var result = settings.WithResponseFile(".sln");

                // Then
                Assert.Equal(settings, result);
            }
        }

        public sealed class TheUseDistributedFileLoggerMethod
        {
            [Fact]
            public void Should_Set_Distributed_File_Logger()
            {
                // Given
                var settings = new DotNetCoreMSBuildSettings();

                // When
                settings.UseDistributedFileLogger();

                // Then
                Assert.True(settings.DistributedFileLogger);
            }

            [Fact]
            public void Should_Return_The_Same_Configuration()
            {
                // Given
                var settings = new DotNetCoreMSBuildSettings();

                // When
                var result = settings.UseDistributedFileLogger();

                // Then
                Assert.Equal(settings, result);
            }
        }

        public sealed class TheWithDistributedLoggerMethod
        {
            [Fact]
            public void Should_Add_Distributed_Logger_To_Configuration()
            {
                // Given
                var settings = new DotNetCoreMSBuildSettings();
                var distributedLogger = new MSBuildDistributedLogger
                {
                    CentralLogger = new MSBuildLogger
                    {
                        Assembly = "LoggerAssembly1",
                        Class = "LoggerClass1",
                        Parameters = "LoggerParameters1"
                    },
                    ForwardingLogger = new MSBuildLogger
                    {
                        Assembly = "LoggerAssembly2",
                        Class = "LoggerClass2",
                        Parameters = "LoggerParameters2"
                    }
                };

                // When
                settings.WithDistributedLogger(distributedLogger);

                // Then
                Assert.True(settings.DistributedLoggers.Contains(distributedLogger));
            }

            [Fact]
            public void Should_Return_The_Same_Configuration()
            {
                // Given
                var settings = new DotNetCoreMSBuildSettings();

                // When
                var result = settings.WithDistributedLogger(new MSBuildDistributedLogger());

                // Then
                Assert.Equal(settings, result);
            }
        }

        public sealed class TheSetConsoleLoggerSettingsMethod
        {
            [Fact]
            public void Should_Set_Console_Logger_Settings()
            {
                // Given
                var settings = new DotNetCoreMSBuildSettings();
                var consoleLoggerParameters = new MSBuildLoggerSettings
                {
                    PerformanceSummary = true,
                    ConsoleColorType = MSBuildConsoleColorType.ForceAnsi
                };

                // When
                settings.SetConsoleLoggerSettings(consoleLoggerParameters);

                // Then
                Assert.Same(consoleLoggerParameters, settings.ConsoleLoggerSettings);
            }

            [Fact]
            public void Should_Return_The_Same_Configuration()
            {
                // Given
                var settings = new DotNetCoreMSBuildSettings();

                // When
                var result = settings.SetConsoleLoggerSettings(new MSBuildLoggerSettings());

                // Then
                Assert.Equal(settings, result);
            }
        }

        public sealed class TheAddFileLoggerMethod
        {
            [Fact]
            public void Should_Add_Logger()
            {
                // Given
                var settings = new DotNetCoreMSBuildSettings();
                var fileLogger = new MSBuildFileLoggerSettings();
                var fileLogger2 = new MSBuildFileLoggerSettings() { LogFile = "A" };

                // When
                settings.AddFileLogger(fileLogger);
                settings.AddFileLogger(fileLogger2);

                // Then
                var loggers = settings.FileLoggers.ToArray();
                Assert.Equal(2, loggers.Length);
                Assert.Equal(fileLogger, loggers[0]);
                Assert.Equal(fileLogger2, loggers[1]);
                Assert.Equal("A", loggers[1].LogFile);
            }

            [Fact]
            public void Should_Return_The_Same_Configuration()
            {
                // Given
                var settings = new DotNetCoreMSBuildSettings();

                // When
                var result = settings.AddFileLogger(new MSBuildFileLoggerSettings());
                var result1 = settings.AddFileLogger();

                // Then
                Assert.Equal(settings, result);
                Assert.Equal(settings, result1);
            }
        }

        public sealed class TheWithLoggerMethod
        {
            [Fact]
            public void Should_Add_Logger()
            {
                // Given
                var settings = new DotNetCoreMSBuildSettings();

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
                var settings = new DotNetCoreMSBuildSettings();

                // When
                var result = settings.WithLogger("Logger");

                // Then
                Assert.Equal(settings, result);
            }
        }

        public sealed class TheDisableConsoleLoggerMethod
        {
            [Fact]
            public void Should_Set_Disable_Console_Logger()
            {
                // Given
                var settings = new DotNetCoreMSBuildSettings();

                // When
                settings.DisableConsoleLogger();

                // Then
                Assert.True(settings.DisableConsoleLogger);
            }

            [Fact]
            public void Should_Return_The_Same_Configuration()
            {
                // Given
                var settings = new DotNetCoreMSBuildSettings();

                // When
                var result = settings.DisableConsoleLogger();

                // Then
                Assert.Equal(settings, result);
            }
        }

        public sealed class TheSetWarningCodeAsErrorMethod
        {
            [Fact]
            public void Should_Add_Warning_Code_To_Configuration()
            {
                // Given
                var settings = new DotNetCoreMSBuildSettings();

                // When
                settings.SetWarningCodeAsError("ERR1");

                // Then
                Assert.True(settings.WarningCodesAsError.Contains("ERR1"));
            }

            [Fact]
            public void Should_Return_The_Same_Configuration()
            {
                // Given
                var settings = new DotNetCoreMSBuildSettings();

                // When
                var result = settings.SetWarningCodeAsError("ERR1");

                // Then
                Assert.Equal(settings, result);
            }

            [Theory]
            [InlineData(null)]
            [InlineData("")]
            [InlineData("          ")]
            public void Should_Throw_Exception_If_Null_Or_Whitespace(string warningCode)
            {
                // Given
                var settings = new DotNetCoreMSBuildSettings();

                // When
                var result = Record.Exception(() => settings.SetWarningCodeAsError(warningCode));

                // Then
                AssertEx.IsArgumentException(result, "warningCode", "Warning code cannot be null or empty");
            }
        }

        public sealed class TheSetWarningCodeAsMessageMethod
        {
            [Fact]
            public void Should_Add_Warning_Code_To_Configuration()
            {
                // Given
                var settings = new DotNetCoreMSBuildSettings();

                // When
                settings.SetWarningCodeAsMessage("ERR1");

                // Then
                Assert.True(settings.WarningCodesAsMessage.Contains("ERR1"));
            }

            [Fact]
            public void Should_Return_The_Same_Configuration()
            {
                // Given
                var settings = new DotNetCoreMSBuildSettings();

                // When
                var result = settings.SetWarningCodeAsMessage("ERR1");

                // Then
                Assert.Equal(settings, result);
            }

            [Theory]
            [InlineData(null)]
            [InlineData("")]
            [InlineData("          ")]
            public void Should_Throw_Exception_If_Null_Or_Whitespace(string warningCode)
            {
                // Given
                var settings = new DotNetCoreMSBuildSettings();

                // When
                var result = Record.Exception(() => settings.SetWarningCodeAsMessage(warningCode));

                // Then
                AssertEx.IsArgumentException(result, "warningCode", "Warning code cannot be null or empty");
            }
        }

        public sealed class TheTreatAllWarningsAsMethod
        {
            [Theory]
            [InlineData(MSBuildTreatAllWarningsAs.Message)]
            [InlineData(MSBuildTreatAllWarningsAs.Error)]
            public void Should_Set_Warning_Code_Behaviour(MSBuildTreatAllWarningsAs treatAllAs)
            {
                // Given
                var settings = new DotNetCoreMSBuildSettings();

                // When
                settings.TreatAllWarningsAs(treatAllAs);

                // Then
                Assert.Equal(treatAllAs, settings.TreatAllWarningsAs);
            }

            [Fact]
            public void Should_Return_The_Same_Configuration()
            {
                // Given
                var settings = new DotNetCoreMSBuildSettings();

                // When
                var result = settings.TreatAllWarningsAs(MSBuildTreatAllWarningsAs.Error);

                // Then
                Assert.Equal(settings, result);
            }
        }

        public sealed class TheSetConfigurationMethod
        {
            private const string Configuration = "TheConfiguration";

            [Fact]
            public void Should_Set_Configuration()
            {
                // Given
                var settings = new DotNetCoreMSBuildSettings();
                const string key = "Configuration";

                // When
                settings.SetConfiguration(Configuration);

                // Then
                Assert.True(settings.Properties.ContainsKey(key));
                Assert.Equal(Configuration, settings.Properties[key].FirstOrDefault());
            }

            [Fact]
            public void Should_Return_The_Same_Configuration()
            {
                // Given
                var settings = new DotNetCoreMSBuildSettings();

                // When
                var result = settings.SetConfiguration(Configuration);

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
                var settings = new DotNetCoreMSBuildSettings();
                const string key = "Version";

                // When
                settings.SetVersion(Version);

                // Then
                Assert.True(settings.Properties.ContainsKey(key));
                Assert.Equal(Version, settings.Properties[key].FirstOrDefault());
            }

            [Fact]
            public void Should_Return_The_Same_Configuration()
            {
                // Given
                var settings = new DotNetCoreMSBuildSettings();

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
            public void Should_Set_Version_Prefix()
            {
                // Given
                var settings = new DotNetCoreMSBuildSettings();
                const string key = "VersionPrefix";

                // When
                settings.SetVersionPrefix(VersionPrefix);

                // Then
                Assert.True(settings.Properties.ContainsKey(key));
                Assert.Equal(VersionPrefix, settings.Properties[key].FirstOrDefault());
            }

            [Fact]
            public void Should_Return_The_Same_Configuration()
            {
                // Given
                var settings = new DotNetCoreMSBuildSettings();

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
            public void Should_Set_Version_Suffix()
            {
                // Given
                var settings = new DotNetCoreMSBuildSettings();
                const string key = "VersionSuffix";

                // When
                settings.SetVersionSuffix(VersionSuffix);

                // Then
                Assert.True(settings.Properties.ContainsKey(key));
                Assert.Equal(VersionSuffix, settings.Properties[key].FirstOrDefault());
            }

            [Fact]
            public void Should_Return_The_Same_Configuration()
            {
                // Given
                var settings = new DotNetCoreMSBuildSettings();

                // When
                var result = settings.SetVersionSuffix(VersionSuffix);

                // Then
                Assert.Equal(settings, result);
            }
        }

        public sealed class TheSetTargetFrameworkMethod
        {
            private const string TargetFramework = "netstandard1.6";

            [Fact]
            public void Should_Set_Target_Framework()
            {
                // Given
                var settings = new DotNetCoreMSBuildSettings();
                const string key = "TargetFrameworks";

                // When
                settings.SetTargetFramework(TargetFramework);

                // Then
                Assert.True(settings.Properties.ContainsKey(key));
                Assert.Equal(TargetFramework, settings.Properties[key].FirstOrDefault());
            }

            [Fact]
            public void Should_Return_The_Same_Configuration()
            {
                // Given
                var settings = new DotNetCoreMSBuildSettings();

                // When
                var result = settings.SetTargetFramework(TargetFramework);

                // Then
                Assert.Equal(settings, result);
            }
        }

        public sealed class TheSetRuntimeMethod
        {
            private const string Runtime = "ubuntu.16.10-x64";

            [Fact]
            public void Should_Set_Runtime()
            {
                // Given
                var settings = new DotNetCoreMSBuildSettings();
                const string key = "RuntimeIdentifiers";

                // When
                settings.SetRuntime(Runtime);

                // Then
                Assert.True(settings.Properties.ContainsKey(key));
                Assert.Equal(Runtime, settings.Properties[key].FirstOrDefault());
            }

            [Fact]
            public void Should_Return_The_Same_Configuration()
            {
                // Given
                var settings = new DotNetCoreMSBuildSettings();

                // When
                var result = settings.SetRuntime(Runtime);

                // Then
                Assert.Equal(settings, result);
            }
        }
    }
}