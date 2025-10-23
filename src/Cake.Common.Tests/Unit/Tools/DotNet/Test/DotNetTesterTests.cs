// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tests.Fixtures.Tools.DotNet.Test;
using Cake.Common.Tools.DotNet.Test;
using Cake.Core;
using Cake.Testing;
using Xunit;

namespace Cake.Common.Tests.Unit.Tools.DotNet.Test
{
    public sealed class DotNetTesterTests
    {
        public sealed class TheTestMethod
        {
            [Fact]
            public void Should_Throw_If_Settings_Are_Null()
            {
                // Given
                var fixture = new DotNetTesterFixture();
                fixture.Project = "./src/*";
                fixture.Settings = null;
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
                var fixture = new DotNetTesterFixture();
                fixture.Project = "./src/*";
                fixture.GivenProcessCannotStart();

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsCakeException(result, ".NET CLI: Process was not started.");
            }

            [Fact]
            public void Should_Throw_If_Process_Has_A_Non_Zero_Exit_Code()
            {
                // Given
                var fixture = new DotNetTesterFixture();
                fixture.Project = "./src/*";
                fixture.GivenProcessExitsWithCode(1);

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsCakeException(result, ".NET CLI: Process returned an error (exit code 1).");
            }

            [Fact]
            public void Should_Add_Mandatory_Arguments()
            {
                // Given
                var fixture = new DotNetTesterFixture();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("test", result.Args);
            }

            [Fact]
            public void Should_Add_Path()
            {
                // Given
                var fixture = new DotNetTesterFixture();
                fixture.Project = "./test/Project.Tests/*";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("test \"./test/Project.Tests/*\"", result.Args);
            }

            [Theory]
            [InlineData("./test/*", "test \"./test/*\"")]
            [InlineData("./test/cake unit tests/", "test \"./test/cake unit tests/\"")]
            [InlineData("./test/cake unit tests/cake core tests", "test \"./test/cake unit tests/cake core tests\"")]
            public void Should_Quote_Project_Path(string text, string expected)
            {
                // Given
                var fixture = new DotNetTesterFixture();
                fixture.Project = text;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Fact]
            public void Should_Add_RunSettings_Arguments()
            {
                // Given
                var fixture = new DotNetTesterFixture();
                fixture.Arguments = new[] { "MSTest.DeploymentEnabled=false", "MSTest.MapInconclusiveToFailed=true" }.ToProcessArguments();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("test -- MSTest.DeploymentEnabled=false MSTest.MapInconclusiveToFailed=true", result.Args);
            }

            [Fact]
            public void Should_Add_Additional_Settings()
            {
                // Given
                var fixture = new DotNetTesterFixture();
                fixture.Settings.NoBuild = true;
                fixture.Settings.NoRestore = true;
                fixture.Settings.NoLogo = true;
                fixture.Settings.Framework = "dnxcore50";
                fixture.Settings.Configuration = "Release";
                fixture.Settings.Collectors = new[] { "XPlat Code Coverage" };
                fixture.Settings.OutputDirectory = "./artifacts/";
                fixture.Settings.Settings = "./demo.runsettings";
                fixture.Settings.Filter = "Priority = 1";
                fixture.Settings.TestAdapterPath = @"/Working/custom-test-adapter";
                fixture.Settings.Loggers = new[] { "html;LogFileName=/Working/logfile.html" };
                fixture.Settings.DiagnosticFile = "./artifacts/logging/diagnostics.txt";
                fixture.Settings.ResultsDirectory = "./tests/";
                fixture.Settings.VSTestReportPath = "./tests/TestResults.xml";
                fixture.Settings.Runtime = "win-x64";
                fixture.Settings.Blame = true;
                fixture.Settings.Sources = new[] { "https://api.nuget.org/v3/index.json" };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("test --settings \"/Working/demo.runsettings\" --filter \"Priority = 1\" --test-adapter-path \"/Working/custom-test-adapter\" --logger \"html;LogFileName=/Working/logfile.html\" --output \"/Working/artifacts\" --framework dnxcore50 --configuration Release --collect \"XPlat Code Coverage\" --diag \"/Working/artifacts/logging/diagnostics.txt\" --no-build --no-restore --nologo --results-directory \"/Working/tests\" --logger trx;LogFileName=\"/Working/tests/TestResults.xml\" --runtime win-x64 --source \"https://api.nuget.org/v3/index.json\" --blame", result.Args);
            }

            [Fact]
            public void Should_Add_Host_Arguments()
            {
                // Given
                var fixture = new DotNetTesterFixture();
                fixture.Settings.DiagnosticOutput = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("--diagnostics test", result.Args);
            }

            [Fact]
            public void Should_Add_Project_Path_With_Explicit_Project_Type()
            {
                // Given
                var fixture = new DotNetTesterFixture();
                fixture.Project = "./test/Project.Tests.csproj";
                fixture.Settings.PathType = DotNetTestPathType.Project;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("test --project \"./test/Project.Tests.csproj\"", result.Args);
            }

            [Fact]
            public void Should_Add_Solution_Path_With_Explicit_Solution_Type()
            {
                // Given
                var fixture = new DotNetTesterFixture();
                fixture.Project = "./test/TestSolution.sln";
                fixture.Settings.PathType = DotNetTestPathType.Solution;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("test --solution \"./test/TestSolution.sln\"", result.Args);
            }

            [Theory]
            [InlineData("./test/Project.csproj", "test --project \"./test/Project.csproj\"")]
            [InlineData("./test/Project.vbproj", "test --project \"./test/Project.vbproj\"")]
            [InlineData("./test/Project.fsproj", "test --project \"./test/Project.fsproj\"")]
            [InlineData("./test/Project.vcxproj", "test --project \"./test/Project.vcxproj\"")]
            public void Should_Auto_Detect_Project_Files(string projectPath, string expected)
            {
                // Given
                var fixture = new DotNetTesterFixture();
                fixture.Project = projectPath;
                fixture.Settings.PathType = DotNetTestPathType.Auto;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Fact]
            public void Should_Auto_Detect_Solution_Files()
            {
                // Given
                var fixture = new DotNetTesterFixture();
                fixture.Project = "./test/TestSolution.sln";
                fixture.Settings.PathType = DotNetTestPathType.Auto;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("test --solution \"./test/TestSolution.sln\"", result.Args);
            }

            [Fact]
            public void Should_Use_Legacy_Behavior_For_Unknown_Extensions_With_Auto()
            {
                // Given
                var fixture = new DotNetTesterFixture();
                fixture.Project = "./test/UnknownFile.xyz";
                fixture.Settings.PathType = DotNetTestPathType.Auto;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("test \"./test/UnknownFile.xyz\"", result.Args);
            }

            [Fact]
            public void Should_Use_Legacy_Behavior_When_PathType_Is_None()
            {
                // Given
                var fixture = new DotNetTesterFixture();
                fixture.Project = "./test/Project.csproj";
                fixture.Settings.PathType = DotNetTestPathType.None;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("test \"./test/Project.csproj\"", result.Args);
            }

            [Fact]
            public void Should_Use_Legacy_Behavior_When_PathType_Is_Default()
            {
                // Given
                var fixture = new DotNetTesterFixture();
                fixture.Project = "./test/Project.csproj";
                // PathType defaults to None

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("test \"./test/Project.csproj\"", result.Args);
            }

            [Fact]
            public void Should_Combine_PathType_With_Other_Settings()
            {
                // Given
                var fixture = new DotNetTesterFixture();
                fixture.Project = "./test/Project.csproj";
                fixture.Settings.PathType = DotNetTestPathType.Project;
                fixture.Settings.NoBuild = true;
                fixture.Settings.Configuration = "Release";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("test --project \"./test/Project.csproj\" --configuration Release --no-build", result.Args);
            }
        }
    }
}
