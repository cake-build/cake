// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tests.Fixtures.Tools.SpecFlow.TestExecutionReport;
using Cake.Common.Tools.MSTest;
using Cake.Common.Tools.NUnit;
using Cake.Common.Tools.XUnit;
using Cake.Core;
using Cake.Core.IO;
using Cake.Testing;
using Xunit;

namespace Cake.Common.Tests.Unit.Tools.SpecFlow.TestExecutionReport
{
    public sealed class SpecFlowTestExecutionReporterTests
    {
        public sealed class TheRunMethod
        {
            [Fact]
            public void Should_Throw_If_Context_Is_Null()
            {
                // Given
                var fixture = new SpecFlowTestExecutionReporterFixture();
                fixture.Context = null;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "context");
            }

            [Fact]
            public void Should_Throw_If_Action_Is_Null()
            {
                // Given
                var fixture = new SpecFlowTestExecutionReporterFixture();
                fixture.Action = null;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "action");
            }

            [Fact]
            public void Should_Throw_If_Project_File_Is_Null()
            {
                // Given
                var fixture = new SpecFlowTestExecutionReporterFixture();
                fixture.ProjectFile = null;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "projectFile");
            }

            [Fact]
            public void Should_Throw_If_Settings_Are_Null()
            {
                // Given
                var fixture = new SpecFlowTestExecutionReporterFixture();
                fixture.Settings = null;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "settings");
            }

            [Fact]
            public void Should_Throw_If_No_Tool_Was_Intercepted()
            {
                // Given
                var fixture = new SpecFlowTestExecutionReporterFixture();
                fixture.Action = context => { };

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsCakeException(result, "No tool was started.");
            }

            [Fact]
            public void Should_Capture_Tool_And_Arguments_From_Action()
            {
                // Given
                var fixture = new SpecFlowTestExecutionReporterFixture();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("mstestexecutionreport \"/Working/Tests.csproj\" " +
                             "/testResult:\"/Working/TestResult.trx\"", result.Args);
            }

            [Fact]
            public void Should_Throw_If_Action_Is_Not_Supported()
            {
                // Given
                var fixture = new SpecFlowTestExecutionReporterFixture();
                fixture.Action = context =>
                {
                    context.ProcessRunner.Start(
                        new FilePath("/Working/tools/Test.exe"),
                        new ProcessSettings
                        {
                            Arguments = null
                        });
                };

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsCakeException(result, "Unsupported tool /Working/tools/Test.exe.");
            }

            [Theory]
            [InlineData(null)]
            [InlineData(" ")]
            public void Should_Throw_If_Action_Does_Not_Contain_Arguments(string arguments)
            {
                // Given
                var fixture = new SpecFlowTestExecutionReporterFixture();
                fixture.Action = context =>
                {
                    context.ProcessRunner.Start(
                        new FilePath("/Working/tools/MSTest.exe"),
                        new ProcessSettings
                        {
                            Arguments = arguments
                        });
                };

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsCakeException(result, "No arguments were found for tool.");
            }

            [Fact]
            public void Should_Append_Out()
            {
                // Given
                var fixture = new SpecFlowTestExecutionReporterFixture();
                fixture.Settings.Out = "/Working/out.html";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("mstestexecutionreport \"/Working/Tests.csproj\" " +
                             "/testResult:\"/Working/TestResult.trx\" " +
                             "/out:\"/Working/out.html\"", result.Args);
            }

            [Fact]
            public void Should_Append_XsltFile()
            {
                // Given
                var fixture = new SpecFlowTestExecutionReporterFixture();
                fixture.Settings.XsltFile = "/Working/template.xslt";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("mstestexecutionreport \"/Working/Tests.csproj\" " +
                             "/testResult:\"/Working/TestResult.trx\" " +
                             "/xsltFile:\"/Working/template.xslt\"", result.Args);
            }

            [Fact]
            public void Should_Rethrow_Exception_From_Action()
            {
                // Given
                var exception = new CakeException("The exception message");
                var fixture = new SpecFlowTestExecutionReporterFixture();
                fixture.Settings.ThrowOnTestFailure = true;
                var intercepting = true;

                fixture.Action = context =>
                {
                    context.ProcessRunner.Start(
                        new FilePath("/Working/tools/MSTest.exe"),
                        new ProcessSettings()
                        {
                            Arguments = "/resultsfile:\"/Working/TestResult.trx\""
                        });

                    // Quick fix to avoid throwing exception while intercepting action
                    if (intercepting)
                    {
                        intercepting = false;
                    }
                    else
                    {
                        throw exception;
                    }
                };

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsCakeException(result, exception.Message);
            }

            [Fact]
            public void Should_Not_Rethrow_Exception_From_Action()
            {
                // Given
                var exception = new CakeException("The exception message");
                var fixture = new SpecFlowTestExecutionReporterFixture();
                fixture.Settings.ThrowOnTestFailure = false;
                var intercepting = true;

                fixture.Action = context =>
                {
                    var process = context.ProcessRunner.Start(
                        new FilePath("/Working/tools/MSTest.exe"),
                        new ProcessSettings()
                        {
                            Arguments = "/resultsfile:\"/Working/TestResult.trx\""
                        });

                    // Quick fix to avoid throwing exception while intercepting action
                    if (intercepting)
                    {
                        intercepting = false;
                    }
                    else
                    {
                        throw exception;
                    }
                };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("mstestexecutionreport \"/Working/Tests.csproj\" " +
                             "/testResult:\"/Working/TestResult.trx\"", result.Args);
            }

            [Fact]
            public void Should_Capture_XUnit2()
            {
                // Given
                var fixture = new SpecFlowTestExecutionReporterFixture();
                fixture.FileSystem.CreateFile("/Working/tools/xunit.console.exe");

                var xUnit2Settings = new XUnit2Settings { ShadowCopy = false };
                xUnit2Settings.ArgumentCustomization = builder => builder.Append("-nunit \"/Working/TestResult.xml\"");

                fixture.Action = context =>
                {
                    context.XUnit2(new FilePath[] { "./Test.dll" }, xUnit2Settings);
                };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("nunitexecutionreport \"/Working/Tests.csproj\" " +
                             "/xmlTestResult:\"/Working/TestResult.xml\"", result.Args);
            }

            [Fact]
            public void Should_Capture_XUnit2_And_Throw_If_NUnit_Is_Missing()
            {
                // Given
                var fixture = new SpecFlowTestExecutionReporterFixture();
                fixture.FileSystem.CreateFile("/Working/tools/xunit.console.exe");

                fixture.Action = context =>
                {
                    context.XUnit2(new FilePath[] { "./Test.dll" }, new XUnit2Settings { ShadowCopy = false });
                };

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsCakeException(result, "XUnit2 must contain argument \"-nunit <filename>\"");
            }

            [Fact]
            public void Should_Capture_NUnit3()
            {
                // Given
                var fixture = new SpecFlowTestExecutionReporterFixture();
                fixture.FileSystem.CreateFile("/Working/tools/nunit3-console.exe");

                var nUnit3Settings = new NUnit3Settings
                {
                    ShadowCopy = false,
                    Results = "/Working/TestResult.xml",
                    ResultFormat = "nunit2",
                    Labels = NUnit3Labels.All,
                    OutputFile = "/Working/TestResult.txt"
                };

                fixture.Action = context =>
                {
                    context.NUnit3(new FilePath[] { "./Test.dll" }, nUnit3Settings);
                };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("nunitexecutionreport \"/Working/Tests.csproj\" " +
                             "/xmlTestResult:\"/Working/TestResult.xml\" " +
                             "/testOutput:\"/Working/TestResult.txt\"", result.Args);
            }

            [Fact]
            public void Should_Capture_NUnit3_And_Throw_If_Result_Is_Missing()
            {
                // Given
                var fixture = new SpecFlowTestExecutionReporterFixture();
                fixture.FileSystem.CreateFile("/Working/tools/nunit3-console.exe");

                var nUnit3Settings = new NUnit3Settings
                {
                    ShadowCopy = false,
                    Labels = NUnit3Labels.All,
                    OutputFile = "/Working/TestResult.txt"
                };

                fixture.Action = context =>
                {
                    context.NUnit3(new FilePath[] { "./Test.dll" }, nUnit3Settings);
                };

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsCakeException(result, "NUnit3 must contain argument \"--result=<filename>;format=nunit2\"");
            }

            [Fact]
            public void Should_Capture_NUnit3_And_Throw_If_Output_Is_Missing()
            {
                // Given
                var fixture = new SpecFlowTestExecutionReporterFixture();
                fixture.FileSystem.CreateFile("/Working/tools/nunit3-console.exe");

                var nUnit3Settings = new NUnit3Settings
                {
                    ShadowCopy = false,
                    Results = "/Working/TestResult.xml",
                    ResultFormat = "nunit2"
                };

                fixture.Action = context =>
                {
                    context.NUnit3(new FilePath[] { "./Test.dll" }, nUnit3Settings);
                };

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsCakeException(result, "NUnit3 must contain argument \"--out=<filename>\"");
            }

            [Fact]
            public void Should_Capture_NUnit3_And_Throw_If_Labels_Is_Missing()
            {
                // Given
                var fixture = new SpecFlowTestExecutionReporterFixture();
                fixture.FileSystem.CreateFile("/Working/tools/nunit3-console.exe");

                var nUnit3Settings = new NUnit3Settings
                {
                    ShadowCopy = false,
                    Results = "/Working/TestResult.xml",
                    ResultFormat = "nunit2",
                    OutputFile = "/Working/TestResult.txt"
                };

                fixture.Action = context =>
                {
                    context.NUnit3(new FilePath[] { "./Test.dll" }, nUnit3Settings);
                };

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsCakeException(result, "NUnit3 must contain argument \"--labels=All\"");
            }

            [Fact]
            public void Should_Capture_MSTest()
            {
                // Given
                var fixture = new SpecFlowTestExecutionReporterFixture();
                fixture.FileSystem.CreateFile("/Working/tools/MSTest.exe");

                var msTestSettings = new MSTestSettings
                {
                    NoIsolation = true
                };
                msTestSettings.ArgumentCustomization = builder => builder.Append("/resultsfile:/Working/TestResult.trx");
                msTestSettings.ToolPath = "/Working/tools/MSTest.exe";

                fixture.Action = context =>
                {
                    context.MSTest(new FilePath[] { "./Test.dll" }, msTestSettings);
                };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("mstestexecutionreport \"/Working/Tests.csproj\" " +
                             "/testResult:/Working/TestResult.trx", result.Args);
            }

            [Fact]
            public void Should_Capture_MSTest_And_Throw_If_ResultsFile_Is_Missing()
            {
                // Given
                var fixture = new SpecFlowTestExecutionReporterFixture();
                fixture.FileSystem.CreateFile("/Working/tools/MSTest.exe");

                var msTestSettings = new MSTestSettings
                {
                    NoIsolation = true
                };
                msTestSettings.ToolPath = "/Working/tools/MSTest.exe";

                fixture.Action = context =>
                {
                    context.MSTest(new FilePath[] { "./Test.dll" }, msTestSettings);
                };

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsCakeException(result, "MSTest must contain argument \"/resultsfile:<filename>\"");
            }
        }
    }
}