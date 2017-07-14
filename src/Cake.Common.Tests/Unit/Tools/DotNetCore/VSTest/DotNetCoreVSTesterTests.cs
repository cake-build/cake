// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tests.Fixtures.Tools.DotNetCore.VSTest;
using Cake.Common.Tools.DotNetCore.VSTest;
using Cake.Common.Tools.VSTest;
using Cake.Core.IO;
using Cake.Testing;
using Xunit;

namespace Cake.Common.Tests.Unit.Tools.DotNetCore.VSTest
{
    public sealed class DotNetCoreVSTesterTests
    {
        public sealed class TheTestMethod
        {
            [Fact]
            public void Should_Throw_If_Settings_Are_Null()
            {
                // Given
                var fixture = new DotNetCoreVSTesterFixture
                {
                    TestFiles = new[] { (FilePath)"./test/unit.tests.csproj" },
                    Settings = null
                };
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
                var fixture = new DotNetCoreVSTesterFixture { TestFiles = new[] { (FilePath)"./test/unit.tests.csproj" } };
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
                var fixture = new DotNetCoreVSTesterFixture { TestFiles = new[] { (FilePath)"./test/unit.tests.csproj" } };
                fixture.GivenProcessExitsWithCode(1);

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsCakeException(result, ".NET Core CLI: Process returned an error (exit code 1).");
            }

            [Fact]
            public void Should_Add_Mandatory_Arguments()
            {
                // Given
                var fixture = new DotNetCoreVSTesterFixture { TestFiles = new[] { (FilePath)"./test/unit.tests.csproj" } };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("vstest \"/Working/test/unit.tests.csproj\"", result.Args);
            }

            [Fact]
            public void Should_Add_Multiple_Test_File_Arguments()
            {
                // Given
                var fixture = new DotNetCoreVSTesterFixture
                {
                    TestFiles = new[]
                    {
                        (FilePath)"./test1/unit.tests.csproj",
                        (FilePath)"./test2/unit.tests.csproj",
                        (FilePath)"./test3/unit.tests.csproj",
                    }
                };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("vstest \"/Working/test1/unit.tests.csproj\" \"/Working/test2/unit.tests.csproj\" \"/Working/test3/unit.tests.csproj\"", result.Args);
            }

            [Fact]
            public void Should_Quote_Test_File_Path()
            {
                // Given
                var fixture = new DotNetCoreVSTesterFixture { TestFiles = new[] { (FilePath)"./test/cake unit tests/cake core tests.csproj" } };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("vstest \"/Working/test/cake unit tests/cake core tests.csproj\"", result.Args);
            }

            [Fact]
            public void Should_Add_Settings_Argument()
            {
                // Given
                var fixture = new DotNetCoreVSTesterFixture
                {
                    TestFiles = new[] { (FilePath)"./test/unit.tests.csproj" },
                    Settings = new DotNetCoreVSTestSettings
                    {
                        Settings = "./test/demo.runsettings"
                    }
                };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("vstest \"/Working/test/unit.tests.csproj\" --Settings:\"/Working/test/demo.runsettings\"", result.Args);
            }

            [Fact]
            public void Should_Add_Tests_Argument()
            {
                // Given
                var fixture = new DotNetCoreVSTesterFixture
                {
                    TestFiles = new[] { (FilePath)"./test/unit.tests.csproj" },
                    Settings = new DotNetCoreVSTestSettings
                    {
                        TestsToRun = new[] { "TestMethod1", "TestMethod2" }
                    }
                };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("vstest \"/Working/test/unit.tests.csproj\" --Tests:TestMethod1,TestMethod2", result.Args);
            }

            [Fact]
            public void Should_Add_TestAdapter_Argument()
            {
                // Given
                var fixture = new DotNetCoreVSTesterFixture
                {
                    TestFiles = new[] { (FilePath)"./test/unit.tests.csproj" },
                    Settings = new DotNetCoreVSTestSettings
                    {
                        TestAdapterPath = @"/Working/custom-test-adapter"
                    }
                };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("vstest \"/Working/test/unit.tests.csproj\" --TestAdapterPath:\"/Working/custom-test-adapter\"", result.Args);
            }

            [Fact]
            public void Should_Add_Platform_Argument()
            {
                // Given
                var fixture = new DotNetCoreVSTesterFixture
                {
                    TestFiles = new[] { (FilePath)"./test/unit.tests.csproj" },
                    Settings = new DotNetCoreVSTestSettings
                    {
                        Platform = VSTestPlatform.x64
                    }
                };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("vstest \"/Working/test/unit.tests.csproj\" --Platform:x64", result.Args);
            }

            [Fact]
            public void Should_Add_Framework_Argument()
            {
                // Given
                var fixture = new DotNetCoreVSTesterFixture
                {
                    TestFiles = new[] { (FilePath)"./test/unit.tests.csproj" },
                    Settings = new DotNetCoreVSTestSettings
                    {
                        Framework = "dnxcore50"
                    }
                };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("vstest \"/Working/test/unit.tests.csproj\" --Framework:dnxcore50", result.Args);
            }

            [Fact]
            public void Should_Add_Parallel_Argument()
            {
                // Given
                var fixture = new DotNetCoreVSTesterFixture
                {
                    TestFiles = new[] { (FilePath)"./test/unit.tests.csproj" },
                    Settings = new DotNetCoreVSTestSettings
                    {
                        Parallel = true
                    }
                };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("vstest \"/Working/test/unit.tests.csproj\" --Parallel", result.Args);
            }

            [Fact]
            public void Should_Add_TestCaseFilter_Argument()
            {
                // Given
                var fixture = new DotNetCoreVSTesterFixture
                {
                    TestFiles = new[] { (FilePath)"./test/unit.tests.csproj" },
                    Settings = new DotNetCoreVSTestSettings
                    {
                        TestCaseFilter = "FullyQualifiedName~Cake.Common.Core.DotNetCore"
                    }
                };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("vstest \"/Working/test/unit.tests.csproj\" --TestCaseFilter:\"FullyQualifiedName~Cake.Common.Core.DotNetCore\"", result.Args);
            }

            [Fact]
            public void Should_Add_Logger_Argument()
            {
                // Given
                var fixture = new DotNetCoreVSTesterFixture
                {
                    TestFiles = new[] { (FilePath)"./test/unit.tests.csproj" },
                    Settings = new DotNetCoreVSTestSettings
                    {
                        Logger = @"trx;LogFileName=/Working/logfile.trx"
                    }
                };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("vstest \"/Working/test/unit.tests.csproj\" --logger:\"trx;LogFileName=/Working/logfile.trx\"", result.Args);
            }

            [Fact]
            public void Should_Add_ParentProcessId_Argument()
            {
                // Given
                var fixture = new DotNetCoreVSTesterFixture
                {
                    TestFiles = new[] { (FilePath)"./test/unit.tests.csproj" },
                    Settings = new DotNetCoreVSTestSettings
                    {
                        ParentProcessId = @"100"
                    }
                };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("vstest \"/Working/test/unit.tests.csproj\" --ParentProcessId:100", result.Args);
            }

            [Fact]
            public void Should_Add_Port_Argument()
            {
                // Given
                var fixture = new DotNetCoreVSTesterFixture
                {
                    TestFiles = new[] { (FilePath)"./test/unit.tests.csproj" },
                    Settings = new DotNetCoreVSTestSettings
                    {
                        Port = 8000
                    }
                };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("vstest \"/Working/test/unit.tests.csproj\" --Port:8000", result.Args);
            }

            [Fact]
            public void Should_Add_Diag_Argument()
            {
                // Given
                var fixture = new DotNetCoreVSTesterFixture
                {
                    TestFiles = new[] { (FilePath)"./test/unit.tests.csproj" },
                    Settings = new DotNetCoreVSTestSettings
                    {
                        DiagnosticFile = "./artifacts/logging/diagnostics.txt"
                    }
                };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("vstest \"/Working/test/unit.tests.csproj\" --Diag:\"/Working/artifacts/logging/diagnostics.txt\"", result.Args);
            }

            [Fact]
            public void Should_Add_Extra_Argument()
            {
                // Given
                var fixture = new DotNetCoreVSTesterFixture
                {
                    TestFiles = new[] { (FilePath)"./test/unit.tests.csproj" },
                    Settings = new DotNetCoreVSTestSettings
                    {
                        Arguments =
                        {
                            { "Arg1", "Value1" },
                            { "Arg2", "Value2" },
                        }
                    }
                };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("vstest \"/Working/test/unit.tests.csproj\" Arg1=\"Value1\" Arg2=\"Value2\"", result.Args);
            }

            [Fact]
            public void Should_Add_Host_Arguments()
            {
                // Given
                var fixture = new DotNetCoreVSTesterFixture { TestFiles = new[] { (FilePath)"./test/unit.tests.csproj" } };
                fixture.Settings.DiagnosticOutput = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("--diagnostics vstest \"/Working/test/unit.tests.csproj\"", result.Args);
            }
        }
    }
}
