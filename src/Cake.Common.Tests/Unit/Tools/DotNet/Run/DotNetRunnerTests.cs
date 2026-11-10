// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tests.Fixtures.Tools.DotNet.Run;
using Cake.Common.Tools.DotNet;
using Cake.Common.Tools.DotNet.Run;
using Cake.Core.IO;
using Cake.Testing;

namespace Cake.Common.Tests.Unit.Tools.DotNet.Run;

public sealed class DotNetRunnerTests
{
    public sealed class TheRunMethod
    {
        [Fact]
        public void Should_Throw_If_Settings_Are_Null()
        {
            // Given
            var fixture = new DotNetRunnerFixture();
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
            var fixture = new DotNetRunnerFixture();
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
            var fixture = new DotNetRunnerFixture();
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
            var fixture = new DotNetRunnerFixture();

            // When
            var result = fixture.Run();

            // Then
            Assert.Equal("run", result.Args);
        }

        [Fact]
        public void Should_Add_Path_Arguments()
        {
            // Given
            var fixture = new DotNetRunnerFixture();
            fixture.Project = "./tools/tool/";
            fixture.Arguments = "--args=\"value\"";
            // When
            var result = fixture.Run();

            // Then
            Assert.Equal("run --project \"./tools/tool/\" -- --args=\"value\"", result.Args);
        }

        [Fact]
        public void Should_Add_Additional_Settings()
        {
            // Given
            var fixture = new DotNetRunnerFixture();
            fixture.Settings.Framework = "dnxcore50";
            fixture.Settings.Configuration = "Release";
            fixture.Settings.Runtime = "win7-x86";
            fixture.Settings.Sources = new[] { "https://api.nuget.org/v3/index.json" };
            fixture.Settings.RollForward = DotNetRollForward.Major;
            fixture.Settings.ApplicationEnvironment.Add("ASPNETCORE_ENVIRONMENT", "Development");
            fixture.Settings.ApplicationEnvironment.Add("DOTNET_ENVIRONMENT", "Staging");
            fixture.Settings.File = "./app.cs";
            fixture.Settings.Device = "iPhone";
            fixture.Settings.ArtifactsPath = "./artifacts";
            fixture.Settings.NoCache = true;

            // When
            var result = fixture.Run();

            // Then
            Assert.Equal("run --framework dnxcore50 --configuration Release --runtime win7-x86 --source \"https://api.nuget.org/v3/index.json\" --roll-forward Major --environment \"ASPNETCORE_ENVIRONMENT=Development\" --environment \"DOTNET_ENVIRONMENT=Staging\" --file \"/Working/app.cs\" --device \"iPhone\" --artifacts-path \"/Working/artifacts\" --no-cache", result.Args);
        }

        [Fact]
        public void Should_Add_Host_Arguments()
        {
            // Given
            var fixture = new DotNetRunnerFixture();
            fixture.Settings.DiagnosticOutput = true;

            // When
            var result = fixture.Run();

            // Then
            Assert.Equal("--diagnostics run", result.Args);
        }

        [Theory]
        [InlineData(DotNetRollForward.Minor, "run --roll-forward Minor")]
        [InlineData(DotNetRollForward.LatestPatch, "run --roll-forward LatestPatch")]
        [InlineData(DotNetRollForward.Major, "run --roll-forward Major")]
        [InlineData(DotNetRollForward.LatestMinor, "run --roll-forward LatestMinor")]
        [InlineData(DotNetRollForward.LatestMajor, "run --roll-forward LatestMajor")]
        [InlineData(DotNetRollForward.Disable, "run --roll-forward Disable")]
        public void Should_Add_RollForward_Arguments(DotNetRollForward rollForward, string expected)
        {
            // Given
            var fixture = new DotNetRunnerFixture();
            fixture.Settings.RollForward = rollForward;

            // When
            var result = fixture.Run();

            // Then
            Assert.Equal(expected, result.Args);
        }
    }

    public sealed class TheRunCodeMethod
    {
        [Fact]
        public void Should_Throw_If_Settings_Are_Null()
        {
            // Given
            var fixture = new DotNetRunnerFixture();
            fixture.Code = "Console.WriteLine();";
            fixture.Settings = null;
            fixture.GivenDefaultToolDoNotExist();

            // When
            var result = Record.Exception(() => fixture.Run());

            // Then
            AssertEx.IsArgumentNullException(result, "settings");
        }

        [Fact]
        public void Should_Throw_If_Code_Is_Null()
        {
            // Given
            var fixture = new DotNetRunnerFixture();
            var tool = new DotNetRunner(
                fixture.FileSystem, fixture.Environment, fixture.ProcessRunner, fixture.Tools);

            // When
            var result = Record.Exception(() => tool.RunCode((string)null, null, fixture.Settings));

            // Then
            AssertEx.IsArgumentNullException(result, "text");
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public void Should_Throw_If_Code_Is_Empty_Or_White_Space(string code)
        {
            // Given
            var fixture = new DotNetRunnerFixture();
            var tool = new DotNetRunner(
                fixture.FileSystem, fixture.Environment, fixture.ProcessRunner, fixture.Tools);

            // When
            var result = Record.Exception(() => tool.RunCode(code, null, fixture.Settings));

            // Then
            AssertEx.IsArgumentException(result, "code", "Code cannot be empty.");
        }

        [Fact]
        public void Should_Throw_If_File_Is_Set()
        {
            // Given
            var fixture = new DotNetRunnerFixture();
            fixture.Code = "Console.WriteLine();";
            fixture.Settings.File = "./app.cs";

            // When
            var result = Record.Exception(() => fixture.Run());

            // Then
            AssertEx.IsArgumentException(result, "settings", "File cannot be used together with stdin code.");
        }

        [Fact]
        public void Should_Add_Stdin_Argument()
        {
            // Given
            var fixture = new DotNetRunnerFixture();
            fixture.Code = "Console.WriteLine();";

            // When
            var result = fixture.Run();

            // Then
            Assert.Equal("run -", result.Args);
            Assert.Equal((ProcessStandardInput)"Console.WriteLine();", result.Process.StandardInput);
            Assert.Equal("/Working", result.Process.WorkingDirectory.FullPath);
        }

        [Fact]
        public void Should_Add_Settings_And_Application_Arguments()
        {
            // Given
            var fixture = new DotNetRunnerFixture();
            fixture.Code = "Console.WriteLine();";
            fixture.Arguments = "--name=World";
            fixture.Settings.Framework = "net10.0";
            fixture.Settings.Configuration = "Release";

            // When
            var result = fixture.Run();

            // Then
            Assert.Equal("run --framework net10.0 --configuration Release - -- --name=World", result.Args);
            Assert.Equal((ProcessStandardInput)"Console.WriteLine();", result.Process.StandardInput);
        }

        [Fact]
        public void Should_Use_Explicit_Working_Directory()
        {
            // Given
            var fixture = new DotNetRunnerFixture();
            fixture.Code = "Console.WriteLine();";
            fixture.Settings.WorkingDirectory = "./temp";

            // When
            var result = fixture.Run();

            // Then
            Assert.Equal("/Working/temp", result.Process.WorkingDirectory.FullPath);
        }
    }
}
