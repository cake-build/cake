// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using Cake.Common.Tests.Fixtures.Tools.DotNetCore.MSBuild;
using Cake.Common.Tools.DotNetCore;
using Cake.Common.Tools.DotNetCore.MSBuild;
using Cake.Common.Tools.MSBuild;
using Cake.Testing;
using Xunit;

namespace Cake.Common.Tests.Unit.Tools.DotNetCore.MSBuild
{
    public sealed class DotNetCoreMSBuildBuilderTests
    {
        public sealed class TheBuildMethod
        {
            [Fact]
            public void Should_Throw_If_Settings_Are_Null()
            {
                // Given
                var fixture = new DotNetCoreMSBuildBuilderFixture();
                fixture.Project = "./src/*";
                fixture.Settings = null;
                fixture.GivenDefaultToolDoNotExist();

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "settings");
            }

            [Fact]
            public void Should_Not_Throw_If_Project_Is_Null()
            {
                // Given
                var fixture = new DotNetCoreMSBuildBuilderFixture()
                {
                    Settings = new DotNetCoreMSBuildSettings()
                };

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.Null(result);
            }

            [Fact]
            public void Should_Throw_If_Process_Was_Not_Started()
            {
                // Given
                var fixture = new DotNetCoreMSBuildBuilderFixture()
                {
                    Project = "./src/*",
                    Settings = new DotNetCoreMSBuildSettings()
                };
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
                var fixture = new DotNetCoreMSBuildBuilderFixture()
                {
                    Project = "./src/*"
                };
                fixture.GivenProcessExitsWithCode(1);

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsCakeException(result, ".NET Core CLI: Process returned an error (exit code 1).");
            }

            [Fact]
            public void Should_Use_Project_Path_If_Specified()
            {
                // Given
                var fixture = new DotNetCoreMSBuildBuilderFixture();
                fixture.Project = "./src/foo/foo.csproj";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("msbuild \"./src/foo/foo.csproj\"", result.Args);
            }

            [Fact]
            public void Should_Use_Directory_Path_If_Specified()
            {
                // Given
                var fixture = new DotNetCoreMSBuildBuilderFixture();
                fixture.Project = "./src/";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("msbuild \"./src/\"", result.Args);
            }

            [Fact]
            public void Should_Add_Target_Argument()
            {
                // Given
                var fixture = new DotNetCoreMSBuildBuilderFixture();
                fixture.Settings.Targets.Add("A");

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("msbuild /target:A", result.Args);
            }

            [Fact]
            public void Should_Append_Target_Argument_When_Multiple_Values()
            {
                // Given
                var fixture = new DotNetCoreMSBuildBuilderFixture();
                fixture.Settings.Targets.Add("A");
                fixture.Settings.Targets.Add("B");

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("msbuild /target:A;B", result.Args);
            }

            [Theory]
            [InlineData(null)]
            [InlineData("")]
            [InlineData("          ")]
            public void Should_Throw_If_Target_Has_No_Value(string targetValue)
            {
                // Given
                var fixture = new DotNetCoreMSBuildBuilderFixture();
                fixture.Settings.Targets.Add(targetValue);

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentException(result, "Targets", "Specify the name of the target");
            }

            [Fact]
            public void Should_Add_Property_Argument()
            {
                // Given
                var fixture = new DotNetCoreMSBuildBuilderFixture();
                fixture.Settings.Properties.Add("A", new[] { "B" });

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("msbuild /property:A=B", result.Args);
            }

            [Fact]
            public void Should_Add_Multiple_Property_Arguments_When_Multiple_Values()
            {
                // Given
                var fixture = new DotNetCoreMSBuildBuilderFixture();
                fixture.Settings.Properties.Add("A", new[] { "B", "E" });
                fixture.Settings.Properties.Add("C", new[] { "D" });

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("msbuild /property:A=B /property:A=E /property:C=D", result.Args);
            }

            [Theory]
            [InlineData(null)]
            [InlineData(new object[] { new string[] { } })]
            [InlineData(new object[] { new[] { "" } })]
            [InlineData(new object[] { new[] { "          " } })]
            public void Should_Throw_If_Property_Has_No_Value(string[] propertyValues)
            {
                // Given
                var fixture = new DotNetCoreMSBuildBuilderFixture();
                fixture.Settings.Properties.Add("F", propertyValues);

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentException(result, "Properties", "A property must have at least one non-empty value");
            }

            [Theory]
            [InlineData(0)]
            [InlineData(-10)]
            public void Should_Use_As_Many_Processors_As_Possible_If_MaxCpuCount_Is_Zero_Or_Less(int maxCpuCount)
            {
                // Given
                var fixture = new DotNetCoreMSBuildBuilderFixture();
                fixture.Settings.MaxCpuCount = maxCpuCount;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("msbuild /maxcpucount", result.Args);
            }

            [Fact]
            public void Should_Use_Specified_Number_Of_Max_Processors()
            {
                // Given
                var fixture = new DotNetCoreMSBuildBuilderFixture();
                fixture.Settings.MaxCpuCount = 4;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("msbuild /maxcpucount:4", result.Args);
            }

            [Theory]
            [InlineData(MSBuildVersion.MSBuild20, "2.0")]
            [InlineData(MSBuildVersion.MSBuild35, "3.5")]
            [InlineData(MSBuildVersion.MSBuild4, "4.0")]
            [InlineData(MSBuildVersion.MSBuild12, "12.0")]
            [InlineData(MSBuildVersion.MSBuild14, "14.0")]
            [InlineData(MSBuildVersion.MSBuild15, "15.0")]
            public void Should_Add_ToolVersion_Argument_If_Specified(MSBuildVersion toolVersion, string expectedToolVersion)
            {
                // Given
                var fixture = new DotNetCoreMSBuildBuilderFixture();
                fixture.Settings.ToolVersion = toolVersion;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal($"msbuild /toolsversion:{expectedToolVersion}", result.Args);
            }

            [Theory]
            [InlineData(100)]
            [InlineData(-8)]
            [InlineData(0)]
            public void Should_Throw_If_ToolVersion_Is_Invalid(int toolVersion)
            {
                // Given
                var fixture = new DotNetCoreMSBuildBuilderFixture();
                fixture.Settings.ToolVersion = (MSBuildVersion)toolVersion;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsExceptionWithMessage<ArgumentOutOfRangeException>(result, $"Invalid value{Environment.NewLine}Parameter name: toolVersion{Environment.NewLine}Actual value was {toolVersion}.");
            }

            [Fact]
            public void Should_Add_NoConsoleLogger_If_Specified()
            {
                // Given
                var fixture = new DotNetCoreMSBuildBuilderFixture();
                fixture.Settings.DisableConsoleLogger = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("msbuild /noconsolelogger", result.Args);
            }

            [Fact]
            public void Should_Ignore_Console_Logger_Settings_If_No_Console_Logger_Specified()
            {
                // Given
                var fixture = new DotNetCoreMSBuildBuilderFixture();
                fixture.Settings.DisableConsoleLogger = true;
                fixture.Settings.ConsoleLoggerSettings = new MSBuildLoggerSettings();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("msbuild /noconsolelogger", result.Args);
            }

            [Fact]
            public void Should_Add_ConsoleLoggerParameters_If_Specified()
            {
                // Given
                var fixture = new DotNetCoreMSBuildBuilderFixture();
                fixture.Settings.ConsoleLoggerSettings = new MSBuildLoggerSettings
                {
                    PerformanceSummary = true,
                    SummaryOutputLevel = MSBuildLoggerOutputLevel.ErrorsOnly,
                    Verbosity = DotNetCoreVerbosity.Diagnostic
                };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("msbuild /consoleloggerparameters:PerformanceSummary;ErrorsOnly;Verbosity=Diagnostic", result.Args);
            }

            [Fact]
            public void Should_Add_FileLogger_Arguments()
            {
                // Given
                var fixture = new DotNetCoreMSBuildBuilderFixture();
                fixture.Settings.FileLoggers.Add(new MSBuildFileLoggerSettings { AppendToLogFile = false, PerformanceSummary = true, Verbosity = DotNetCoreVerbosity.Diagnostic });

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(@"msbuild /fileLogger /fileloggerparameters:PerformanceSummary;Verbosity=Diagnostic", result.Args);
            }

            [Fact]
            public void Should_Throw_Exception_For_Too_Many_FileLoggers()
            {
                // Given
                var fixture = new DotNetCoreMSBuildBuilderFixture();
                fixture.Settings.FileLoggers.Add(new MSBuildFileLoggerSettings());
                fixture.Settings.FileLoggers.Add(new MSBuildFileLoggerSettings());
                fixture.Settings.FileLoggers.Add(new MSBuildFileLoggerSettings());
                fixture.Settings.FileLoggers.Add(new MSBuildFileLoggerSettings());
                fixture.Settings.FileLoggers.Add(new MSBuildFileLoggerSettings());
                fixture.Settings.FileLoggers.Add(new MSBuildFileLoggerSettings());
                fixture.Settings.FileLoggers.Add(new MSBuildFileLoggerSettings());
                fixture.Settings.FileLoggers.Add(new MSBuildFileLoggerSettings());
                fixture.Settings.FileLoggers.Add(new MSBuildFileLoggerSettings());
                fixture.Settings.FileLoggers.Add(new MSBuildFileLoggerSettings());
                fixture.Settings.FileLoggers.Add(new MSBuildFileLoggerSettings());

                // When
                var ex = Assert.Throws<InvalidOperationException>(() => fixture.Run());

                // Then
                Assert.Equal(@"Too Many FileLoggers", ex.Message);
            }

            [Fact]
            public void Should_Add_DistributedLogger_Argument()
            {
                // Given
                var fixture = new DotNetCoreMSBuildBuilderFixture();
                fixture.Settings.DistributedLoggers.Add(new MSBuildDistributedLogger
                {
                    CentralLogger = new MSBuildLogger { Assembly = "A" },
                    ForwardingLogger = new MSBuildLogger { Assembly = "B" }
                });

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("msbuild /distributedlogger:\"A\"*\"B\"", result.Args);
            }

            [Fact]
            public void Should_Add_Multiple_DistributedLogger_Arguments_When_Multiple_Values()
            {
                // Given
                var fixture = new DotNetCoreMSBuildBuilderFixture();
                fixture.Settings.DistributedLoggers.Add(new MSBuildDistributedLogger
                {
                    CentralLogger = new MSBuildLogger { Assembly = "A" },
                    ForwardingLogger = new MSBuildLogger { Assembly = "B" }
                });

                fixture.Settings.DistributedLoggers.Add(new MSBuildDistributedLogger
                {
                    CentralLogger = new MSBuildLogger { Class = "C", Assembly = "D" },
                    ForwardingLogger = new MSBuildLogger { Class = "E", Assembly = "F", Parameters = "g" }
                });

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("msbuild /distributedlogger:\"A\"*\"B\" /distributedlogger:C,D*E,F;g", result.Args);
            }

            [Theory]
            [InlineData(null, null)]
            [InlineData(null, "A")]
            [InlineData("A", null)]
            [InlineData("", "")]
            [InlineData("", "A")]
            [InlineData("A", "")]
            [InlineData("          ", "          ")]
            [InlineData("          ", "A")]
            [InlineData("A", "          ")]
            public void Should_Throw_If_DistributedLogger_Has_No_Assembly_Value(string centralLoggerAssembly, string forwardingLoggerAssembly)
            {
                // Given
                var fixture = new DotNetCoreMSBuildBuilderFixture();
                fixture.Settings.DistributedLoggers.Add(new MSBuildDistributedLogger
                {
                    CentralLogger = new MSBuildLogger { Assembly = centralLoggerAssembly },
                    ForwardingLogger = new MSBuildLogger { Assembly = forwardingLoggerAssembly }
                });

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "Assembly");
            }

            [Fact]
            public void Should_Add_DistributedFileLogger_If_Specified()
            {
                // Given
                var fixture = new DotNetCoreMSBuildBuilderFixture();
                fixture.Settings.DistributedFileLogger = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("msbuild /distributedFileLogger", result.Args);
            }

            [Fact]
            public void Should_Add_Logger_Argument()
            {
                // Given
                var fixture = new DotNetCoreMSBuildBuilderFixture();
                fixture.Settings.Loggers.Add(new MSBuildLogger { Assembly = "A" });

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("msbuild /logger:\"A\"", result.Args);
            }

            [Fact]
            public void Should_Add_Multiple_Logger_Arguments_When_Multiple_Values()
            {
                // Given
                var fixture = new DotNetCoreMSBuildBuilderFixture();
                fixture.Settings.Loggers.Add(new MSBuildLogger { Assembly = "A" });
                fixture.Settings.Loggers.Add(new MSBuildLogger { Class = "C", Assembly = "B" });
                fixture.Settings.Loggers.Add(new MSBuildLogger { Class = "E", Assembly = "F", Parameters = "g" });

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("msbuild /logger:\"A\" /logger:C,B /logger:E,F;g", result.Args);
            }

            [Theory]
            [InlineData(null)]
            [InlineData("")]
            [InlineData("          ")]
            public void Should_Throw_If_Logger_Has_No_Assembly_Value(string loggerAssembly)
            {
                // Given
                var fixture = new DotNetCoreMSBuildBuilderFixture();
                fixture.Settings.Loggers.Add(new MSBuildLogger { Assembly = loggerAssembly });

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "Assembly");
            }

            [Theory]
            [InlineData(MSBuildTreatAllWarningsAs.Default, new[] { "Err1" }, ":Err1")]
            [InlineData(MSBuildTreatAllWarningsAs.Default, new[] { "Err1", "Err2" }, ":Err1;Err2")]
            [InlineData(MSBuildTreatAllWarningsAs.Error, new[] { "Err1", "Err2" }, "")]
            [InlineData(MSBuildTreatAllWarningsAs.Error, new string[] { }, "")]
            public void Should_Add_WarnAsError_Argument(MSBuildTreatAllWarningsAs treatAllWarningsAs, string[] errorCodes, string expectedValue)
            {
                // Given
                var fixture = new DotNetCoreMSBuildBuilderFixture();
                fixture.Settings.TreatAllWarningsAs = treatAllWarningsAs;

                foreach (var errorCode in errorCodes)
                {
                    fixture.Settings.WarningCodesAsError.Add(errorCode);
                }

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal($"msbuild /warnaserror{expectedValue}", result.Args);
            }

            [Theory]
            [InlineData(MSBuildTreatAllWarningsAs.Default, new[] { "Err1" }, ":Err1")]
            [InlineData(MSBuildTreatAllWarningsAs.Default, new[] { "Err1", "Err2" }, ":Err1;Err2")]
            [InlineData(MSBuildTreatAllWarningsAs.Message, new[] { "Err1", "Err2" }, "")]
            [InlineData(MSBuildTreatAllWarningsAs.Message, new string[] { }, "")]
            public void Should_Add_WarnAsMessage_Argument(MSBuildTreatAllWarningsAs treatAllWarningsAs, string[] errorCodes, string expectedValue)
            {
                // Given
                var fixture = new DotNetCoreMSBuildBuilderFixture();
                fixture.Settings.TreatAllWarningsAs = treatAllWarningsAs;

                foreach (var errorCode in errorCodes)
                {
                    fixture.Settings.WarningCodesAsMessage.Add(errorCode);
                }

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal($"msbuild /warnasmessage{expectedValue}", result.Args);
            }

            [Fact]
            public void Should_Add_IgnoreProjectExtensions_Argument()
            {
                // Given
                var fixture = new DotNetCoreMSBuildBuilderFixture();
                fixture.Settings.IgnoreProjectExtensions.Add(".sln");

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("msbuild /ignoreprojectextensions:.sln", result.Args);
            }

            [Fact]
            public void Should_Append_IgnoreProjectExtensions_Argument_When_Multiple_Values()
            {
                // Given
                var fixture = new DotNetCoreMSBuildBuilderFixture();
                fixture.Settings.IgnoreProjectExtensions.Add(".vcproj");
                fixture.Settings.IgnoreProjectExtensions.Add(".sln");

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("msbuild /ignoreprojectextensions:.vcproj,.sln", result.Args);
            }

            [Fact]
            public void Should_Add_DetailedSummary_If_Specified()
            {
                // Given
                var fixture = new DotNetCoreMSBuildBuilderFixture();
                fixture.Settings.DetailedSummary = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("msbuild /detailedsummary", result.Args);
            }

            [Fact]
            public void Should_Add_NoAutoResponse_If_Specified()
            {
                // Given
                var fixture = new DotNetCoreMSBuildBuilderFixture();
                fixture.Settings.ExcludeAutoResponseFiles = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("msbuild /noautoresponse", result.Args);
            }

            [Fact]
            public void Should_Add_NoLogo_If_Specified()
            {
                // Given
                var fixture = new DotNetCoreMSBuildBuilderFixture();
                fixture.Settings.NoLogo = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("msbuild /nologo", result.Args);
            }

            [Fact]
            public void Should_Add_ResponseFile_Argument()
            {
                // Given
                var fixture = new DotNetCoreMSBuildBuilderFixture();
                fixture.Settings.ResponseFiles.Add("/src/inputs.rsp");

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("msbuild @\"/src/inputs.rsp\"", result.Args);
            }

            [Fact]
            public void Should_Append_ResponseFile_Argument_When_Multiple_Values()
            {
                // Given
                var fixture = new DotNetCoreMSBuildBuilderFixture();
                fixture.Settings.ResponseFiles.Add("/src/inputs1.rsp");
                fixture.Settings.ResponseFiles.Add("/src/inputs_2.rsp");

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("msbuild @\"/src/inputs1.rsp\" @\"/src/inputs_2.rsp\"", result.Args);
            }

            [Fact]
            public void Should_Add_Host_Arguments()
            {
                // Given
                var fixture = new DotNetCoreMSBuildBuilderFixture();
                fixture.Settings.DiagnosticOutput = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("--diagnostics msbuild", result.Args);
            }
        }

        public class TheConsoleLoggerSettingsProperty
        {
            [Fact]
            public void Should_Append_PerformanceSummary_If_Specified()
            {
                // Given
                var fixture = new DotNetCoreMSBuildBuilderFixture();
                fixture.Settings.ConsoleLoggerSettings = new MSBuildLoggerSettings
                {
                    PerformanceSummary = true
                };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("msbuild /consoleloggerparameters:PerformanceSummary", result.Args);
            }

            [Fact]
            public void Should_Append_NoSummary_If_Specified()
            {
                // Given
                var fixture = new DotNetCoreMSBuildBuilderFixture();
                fixture.Settings.ConsoleLoggerSettings = new MSBuildLoggerSettings
                {
                    NoSummary = true
                };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("msbuild /consoleloggerparameters:NoSummary", result.Args);
            }

            [Theory]
            [InlineData(MSBuildLoggerOutputLevel.ErrorsOnly)]
            [InlineData(MSBuildLoggerOutputLevel.WarningsOnly)]
            public void Should_Append_SummaryOutputLevel_If_Specified(MSBuildLoggerOutputLevel outputLevel)
            {
                // Given
                var fixture = new DotNetCoreMSBuildBuilderFixture();
                fixture.Settings.ConsoleLoggerSettings = new MSBuildLoggerSettings
                {
                    SummaryOutputLevel = outputLevel
                };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal($"msbuild /consoleloggerparameters:{outputLevel}", result.Args);
            }

            [Fact]
            public void Should_Not_Append_SummaryOutputLevel_If_Default()
            {
                // Given
                var fixture = new DotNetCoreMSBuildBuilderFixture();
                fixture.Settings.ConsoleLoggerSettings = new MSBuildLoggerSettings
                {
                    SummaryOutputLevel = MSBuildLoggerOutputLevel.Default
                };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("msbuild", result.Args);
            }

            [Fact]
            public void Should_Append_HideItemAndPropertyList_If_Specified()
            {
                // Given
                var fixture = new DotNetCoreMSBuildBuilderFixture();
                fixture.Settings.ConsoleLoggerSettings = new MSBuildLoggerSettings
                {
                    HideItemAndPropertyList = true
                };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("msbuild /consoleloggerparameters:NoItemAndPropertyList", result.Args);
            }

            [Fact]
            public void Should_Append_ShowCommandLine_If_Specified()
            {
                // Given
                var fixture = new DotNetCoreMSBuildBuilderFixture();
                fixture.Settings.ConsoleLoggerSettings = new MSBuildLoggerSettings
                {
                    ShowCommandLine = true
                };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("msbuild /consoleloggerparameters:ShowCommandLine", result.Args);
            }

            [Fact]
            public void Should_Append_ShowTimestamp_If_Specified()
            {
                // Given
                var fixture = new DotNetCoreMSBuildBuilderFixture();
                fixture.Settings.ConsoleLoggerSettings = new MSBuildLoggerSettings
                {
                    ShowTimestamp = true
                };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("msbuild /consoleloggerparameters:ShowTimestamp", result.Args);
            }

            [Fact]
            public void Should_Append_ForceNoAlign_If_Specified()
            {
                // Given
                var fixture = new DotNetCoreMSBuildBuilderFixture();
                fixture.Settings.ConsoleLoggerSettings = new MSBuildLoggerSettings
                {
                    ForceNoAlign = true
                };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("msbuild /consoleloggerparameters:ForceNoAlign", result.Args);
            }

            [Fact]
            public void Should_Append_ShowEventId_If_Specified()
            {
                // Given
                var fixture = new DotNetCoreMSBuildBuilderFixture();
                fixture.Settings.ConsoleLoggerSettings = new MSBuildLoggerSettings
                {
                    ShowEventId = true
                };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("msbuild /consoleloggerparameters:ShowEventId", result.Args);
            }

            [Fact]
            public void Should_Append_DisableConsoleColor_If_Specified()
            {
                // Given
                var fixture = new DotNetCoreMSBuildBuilderFixture();
                fixture.Settings.ConsoleLoggerSettings = new MSBuildLoggerSettings
                {
                    ConsoleColorType = MSBuildConsoleColorType.Disabled
                };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("msbuild /consoleloggerparameters:DisableConsoleColor", result.Args);
            }

            [Fact]
            public void Should_Append_ForceConsoleColor_If_Specified()
            {
                // Given
                var fixture = new DotNetCoreMSBuildBuilderFixture();
                fixture.Settings.ConsoleLoggerSettings = new MSBuildLoggerSettings
                {
                    ConsoleColorType = MSBuildConsoleColorType.ForceAnsi
                };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("msbuild /consoleloggerparameters:ForceConsoleColor", result.Args);
            }

            [Fact]
            public void Should_Append_DisableMultiprocessorLogging_If_Specified()
            {
                // Given
                var fixture = new DotNetCoreMSBuildBuilderFixture();
                fixture.Settings.ConsoleLoggerSettings = new MSBuildLoggerSettings
                {
                    DisableMultiprocessorLogging = true
                };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("msbuild /consoleloggerparameters:DisableMPLogging", result.Args);
            }

            [Theory]
            [InlineData(DotNetCoreVerbosity.Quiet)]
            [InlineData(DotNetCoreVerbosity.Minimal)]
            [InlineData(DotNetCoreVerbosity.Normal)]
            [InlineData(DotNetCoreVerbosity.Detailed)]
            [InlineData(DotNetCoreVerbosity.Diagnostic)]
            public void Should_Append_Verbosity_If_Specified(DotNetCoreVerbosity verbosity)
            {
                // Given
                var fixture = new DotNetCoreMSBuildBuilderFixture();
                fixture.Settings.ConsoleLoggerSettings = new MSBuildLoggerSettings
                {
                    Verbosity = verbosity
                };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal($"msbuild /consoleloggerparameters:Verbosity={verbosity}", result.Args);
            }
        }

        public sealed class TheFileLoggerSettingsProperty
        {
            public string FileEncoding { get; set; }

            [Fact]
            public void Should_Append_LogFile_If_Specified()
            {
                // Given
                var fixture = new DotNetCoreMSBuildBuilderFixture();
                fixture.Settings.FileLoggers.Add(new MSBuildFileLoggerSettings
                {
                    LogFile = "msbuild.log"
                });

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("msbuild /fileLogger /fileloggerparameters:LogFile=\"/Working/msbuild.log\"", result.Args);
            }

            [Fact]
            public void Should_Append_LogFile_If_Empty()
            {
                // Given
                var fixture = new DotNetCoreMSBuildBuilderFixture();
                fixture.Settings.FileLoggers.Add(new MSBuildFileLoggerSettings { LogFile = string.Empty });

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("msbuild /fileLogger /fileloggerparameters:LogFile=\"\"", result.Args);
            }

            [Fact]
            public void Should_Append_AppendToLogFile_If_Specified()
            {
                // Given
                var fixture = new DotNetCoreMSBuildBuilderFixture();
                fixture.Settings.FileLoggers.Add(new MSBuildFileLoggerSettings { AppendToLogFile = true });

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("msbuild /fileLogger /fileloggerparameters:Append", result.Args);
            }

            [Fact]
            public void Should_Append_FileEncoding_If_Specified()
            {
                // Given
                const string fileEncoding = "UTF8";
                var fixture = new DotNetCoreMSBuildBuilderFixture();
                fixture.Settings.FileLoggers.Add(new MSBuildFileLoggerSettings { FileEncoding = fileEncoding });

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal($"msbuild /fileLogger /fileloggerparameters:Encoding={fileEncoding}", result.Args);
            }

            [Fact]
            public void Should_Append_PerformanceSummary_If_Specified()
            {
                // Given
                var fixture = new DotNetCoreMSBuildBuilderFixture();
                fixture.Settings.FileLoggers.Add(new MSBuildFileLoggerSettings { PerformanceSummary = true });

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("msbuild /fileLogger /fileloggerparameters:PerformanceSummary", result.Args);
            }

            [Fact]
            public void Should_Append_NoSummary_If_Specified()
            {
                // Given
                var fixture = new DotNetCoreMSBuildBuilderFixture();
                fixture.Settings.FileLoggers.Add(new MSBuildFileLoggerSettings { NoSummary = true });

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("msbuild /fileLogger /fileloggerparameters:NoSummary", result.Args);
            }

            [Theory]
            [InlineData(MSBuildLoggerOutputLevel.ErrorsOnly)]
            [InlineData(MSBuildLoggerOutputLevel.WarningsOnly)]
            public void Should_Append_SummaryOutputLevel_If_Specified(MSBuildLoggerOutputLevel outputLevel)
            {
                // Given
                var fixture = new DotNetCoreMSBuildBuilderFixture();
                fixture.Settings.FileLoggers.Add(new MSBuildFileLoggerSettings { SummaryOutputLevel = outputLevel });

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal($"msbuild /fileLogger /fileloggerparameters:{outputLevel}", result.Args);
            }

            [Fact]
            public void Should_Not_Append_SummaryOutputLevel_If_Default()
            {
                // Given
                var fixture = new DotNetCoreMSBuildBuilderFixture();
                fixture.Settings.FileLoggers.Add(new MSBuildFileLoggerSettings { SummaryOutputLevel = MSBuildLoggerOutputLevel.Default });

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("msbuild", result.Args);
            }

            [Fact]
            public void Should_Append_HideItemAndPropertyList_If_Specified()
            {
                // Given
                var fixture = new DotNetCoreMSBuildBuilderFixture();
                fixture.Settings.FileLoggers.Add(new MSBuildFileLoggerSettings { HideItemAndPropertyList = true });

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("msbuild /fileLogger /fileloggerparameters:NoItemAndPropertyList", result.Args);
            }

            [Fact]
            public void Should_Append_ShowCommandLine_If_Specified()
            {
                // Given
                var fixture = new DotNetCoreMSBuildBuilderFixture();
                fixture.Settings.FileLoggers.Add(new MSBuildFileLoggerSettings { ShowCommandLine = true });

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("msbuild /fileLogger /fileloggerparameters:ShowCommandLine", result.Args);
            }

            [Fact]
            public void Should_Append_ShowTimestamp_If_Specified()
            {
                // Given
                var fixture = new DotNetCoreMSBuildBuilderFixture();
                fixture.Settings.FileLoggers.Add(new MSBuildFileLoggerSettings { ShowTimestamp = true });

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("msbuild /fileLogger /fileloggerparameters:ShowTimestamp", result.Args);
            }

            [Fact]
            public void Should_Append_ForceNoAlign_If_Specified()
            {
                // Given
                var fixture = new DotNetCoreMSBuildBuilderFixture();
                fixture.Settings.FileLoggers.Add(new MSBuildFileLoggerSettings { ForceNoAlign = true });

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("msbuild /fileLogger /fileloggerparameters:ForceNoAlign", result.Args);
            }

            [Fact]
            public void Should_Append_ShowEventId_If_Specified()
            {
                // Given
                var fixture = new DotNetCoreMSBuildBuilderFixture();
                fixture.Settings.FileLoggers.Add(new MSBuildFileLoggerSettings { ShowEventId = true });

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("msbuild /fileLogger /fileloggerparameters:ShowEventId", result.Args);
            }

            [Fact]
            public void Should_Append_DisableConsoleColor_If_Specified()
            {
                // Given
                var fixture = new DotNetCoreMSBuildBuilderFixture();
                fixture.Settings.FileLoggers.Add(new MSBuildFileLoggerSettings { ConsoleColorType = MSBuildConsoleColorType.Disabled });

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("msbuild /fileLogger /fileloggerparameters:DisableConsoleColor", result.Args);
            }

            [Fact]
            public void Should_Append_ForceConsoleColor_If_Specified()
            {
                // Given
                var fixture = new DotNetCoreMSBuildBuilderFixture();
                fixture.Settings.FileLoggers.Add(new MSBuildFileLoggerSettings { ConsoleColorType = MSBuildConsoleColorType.ForceAnsi });

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("msbuild /fileLogger /fileloggerparameters:ForceConsoleColor", result.Args);
            }

            [Fact]
            public void Should_Append_DisableMultiprocessorLogging_If_Specified()
            {
                // Given
                var fixture = new DotNetCoreMSBuildBuilderFixture();
                fixture.Settings.FileLoggers.Add(new MSBuildFileLoggerSettings { DisableMultiprocessorLogging = true });

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("msbuild /fileLogger /fileloggerparameters:DisableMPLogging", result.Args);
            }

            [Theory]
            [InlineData(DotNetCoreVerbosity.Quiet)]
            [InlineData(DotNetCoreVerbosity.Minimal)]
            [InlineData(DotNetCoreVerbosity.Normal)]
            [InlineData(DotNetCoreVerbosity.Detailed)]
            [InlineData(DotNetCoreVerbosity.Diagnostic)]
            public void Should_Append_Verbosity_If_Specified(DotNetCoreVerbosity verbosity)
            {
                // Given
                var fixture = new DotNetCoreMSBuildBuilderFixture();
                fixture.Settings.FileLoggers.Add(new MSBuildFileLoggerSettings { Verbosity = verbosity });

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal($"msbuild /fileLogger /fileloggerparameters:Verbosity={verbosity}", result.Args);
            }
        }
    }
}
