// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tests.Fixtures.Tools;
using Cake.Common.Tools.VSTest;
using Cake.Core;
using Cake.Core.IO;
using Cake.Testing;
using Cake.Testing.Xunit;
using Xunit;

namespace Cake.Common.Tests.Unit.Tools.VSTest
{
    public sealed class VSTestRunnerTests
    {
        [Fact]
        public void Should_Throw_If_Assembly_Path_Is_Null()
        {
            // Given
            var fixture = new VSTestRunnerFixture();
            fixture.AssemblyPaths = null;

            // When
            var result = Record.Exception(() => fixture.Run());

            // Then
            AssertEx.IsArgumentNullException(result, "assemblyPaths");
        }

        [Fact]
        public void Should_Throw_If_Settings_Are_Null()
        {
            // Given
            var fixture = new VSTestRunnerFixture();
            fixture.Settings = null;

            // When
            var result = Record.Exception(() => fixture.Run());

            // Then
            AssertEx.IsArgumentNullException(result, "settings");
        }

        [Fact]
        public void Should_Throw_If_Tool_Path_Was_Not_Found()
        {
            // Given
            var fixture = new VSTestRunnerFixture();
            fixture.GivenDefaultToolDoNotExist();

            // When
            var result = Record.Exception(() => fixture.Run());

            // Then
            Assert.IsType<CakeException>(result);
            Assert.Equal("VSTest: Could not locate executable.", result?.Message);
        }

        [Theory]
        [InlineData("/bin/VSTest/vstest.console.exe", "/bin/VSTest/vstest.console.exe")]
        [InlineData("./tools/VSTest/vstest.console.exe", "/Working/tools/VSTest/vstest.console.exe")]
        public void Should_Use_VSTest_From_Tool_Path_If_Provided(string toolPath, string expected)
        {
            // Given
            var fixture = new VSTestRunnerFixture();
            fixture.Settings.ToolPath = toolPath;
            fixture.GivenSettingsToolPathExist();

            // When
            var result = fixture.Run();

            // Then
            Assert.Equal(expected, result.Path.FullPath);
        }

        [WindowsTheory]
        [InlineData("C:/VSTest/vstest.console.exe", "C:/VSTest/vstest.console.exe")]
        public void Should_Use_VSTest_From_Tool_Path_If_Provided_On_Windows(string toolPath, string expected)
        {
            // Given
            var fixture = new VSTestRunnerFixture();
            fixture.Settings.ToolPath = toolPath;
            fixture.GivenSettingsToolPathExist();

            // When
            var result = fixture.Run();

            // Then
            Assert.Equal(expected, result.Path.FullPath);
        }

        [Theory]
        [InlineData("/ProgramFilesX86/Microsoft Visual Studio 15.0/Common7/IDE/CommonExtensions/Microsoft/TestWindow/vstest.console.exe")]
        [InlineData("/ProgramFilesX86/Microsoft Visual Studio 14.0/Common7/IDE/CommonExtensions/Microsoft/TestWindow/vstest.console.exe")]
        [InlineData("/ProgramFilesX86/Microsoft Visual Studio 12.0/Common7/IDE/CommonExtensions/Microsoft/TestWindow/vstest.console.exe")]
        [InlineData("/ProgramFilesX86/Microsoft Visual Studio 11.0/Common7/IDE/CommonExtensions/Microsoft/TestWindow/vstest.console.exe")]
        public void Should_Use_Available_Tool_Path(string existingToolPath)
        {
            // Given
            var fixture = new VSTestRunnerFixture();
            fixture.GivenDefaultToolDoNotExist();
            fixture.FileSystem.CreateFile(existingToolPath);

            // When
            var result = fixture.Run();

            // Then
            Assert.Equal(existingToolPath, result.Path.FullPath);
        }

        [Fact]
        public void Should_Set_Working_Directory()
        {
            // Given
            var fixture = new VSTestRunnerFixture();

            // When
            var result = fixture.Run();

            // Then
            Assert.Equal("/Working", result.Process.WorkingDirectory.FullPath);
        }

        [Fact]
        public void Should_Throw_If_Process_Was_Not_Started()
        {
            // Given
            var fixture = new VSTestRunnerFixture();
            fixture.GivenProcessCannotStart();

            // When
            var result = Record.Exception(() => fixture.Run());

            // Then
            Assert.IsType<CakeException>(result);
            Assert.Equal("VSTest: Process was not started.", result?.Message);
        }

        [Fact]
        public void Should_Throw_If_Process_Has_A_Non_Zero_Exit_Code()
        {
            // Given
            var fixture = new VSTestRunnerFixture();
            fixture.GivenProcessExitsWithCode(1);

            // When
            var result = Record.Exception(() => fixture.Run());

            // Then
            Assert.IsType<CakeException>(result);
            Assert.Equal("VSTest: Process returned an error (exit code 1).", result?.Message);
        }

        [Fact]
        public void Should_Have_No_Args_By_Default()
        {
            // Given
            var fixture = new VSTestRunnerFixture();

            // When
            var result = fixture.Run();

            // Then
            Assert.Equal("\"/Working/Test1.dll\"", result.Args);
        }

        [Fact]
        public void Should_Use_Isolation_If_Enabled_In_Settings()
        {
            // Given
            var fixture = new VSTestRunnerFixture();
            fixture.Settings.InIsolation = true;

            // When
            var result = fixture.Run();

            // Then
            Assert.Equal("\"/Working/Test1.dll\" /InIsolation", result.Args);
        }

        [Fact]
        public void Should_Enable_UseVsixExtensions_If_Enabled_In_Settings()
        {
            // Given
            var fixture = new VSTestRunnerFixture();
            fixture.Settings.UseVsixExtensions = true;

            // When
            var result = fixture.Run();

            // Then
            Assert.Equal("\"/Working/Test1.dll\" /UseVsixExtensions:true", result.Args);
        }

        [Fact]
        public void Should_Disable_UseVsixExtensions_If_Disabled_In_Settings()
        {
            // Given
            var fixture = new VSTestRunnerFixture();
            fixture.Settings.UseVsixExtensions = false;

            // When
            var result = fixture.Run();

            // Then
            Assert.Equal("\"/Working/Test1.dll\" /UseVsixExtensions:false", result.Args);
        }

        [Fact]
        public void Should_Use_TestAdapterPath_If_Provided()
        {
            // Given
            var fixture = new VSTestRunnerFixture();
            fixture.Settings.TestAdapterPath = new DirectoryPath("Path to/test adapters");

            // When
            var result = fixture.Run();

            // Then
            Assert.Equal("\"/Working/Test1.dll\" /TestAdapterPath:\"/Working/Path to/test adapters\"", result.Args);
        }

        [Fact]
        public void Should_Use_Logger_If_Provided()
        {
            // Given
            var fixture = new VSTestRunnerFixture();
            fixture.Settings.Logger = "MyCustomLogger";

            // When
            var result = fixture.Run();

            // Then
            Assert.Equal("\"/Working/Test1.dll\" /Logger:MyCustomLogger", result.Args);
        }

        [Fact]
        public void Should_Use_Diag_If_Provided()
        {
            // Given
            var fixture = new VSTestRunnerFixture();
            fixture.Settings.Diag = new FilePath("Path to/diag log.txt");

            // When
            var result = fixture.Run();

            // Then
            Assert.Equal("\"/Working/Test1.dll\" /Diag:\"/Working/Path to/diag log.txt\"", result.Args);
        }

        [Fact]
        public void Should_Use_SettingsFile_If_Provided()
        {
            // Given
            var fixture = new VSTestRunnerFixture();
            fixture.Settings.SettingsFile = new FilePath("Local.RunSettings");

            // When
            var result = fixture.Run();

            // Then
            Assert.Equal("\"/Working/Test1.dll\" /Settings:\"/Working/Local.RunSettings\"", result.Args);
        }

        [Fact]
        public void Should_Quote_Absolute_SettingsFile_Path()
        {
            // Given
            var fixture = new VSTestRunnerFixture();
            fixture.Settings.SettingsFile = new FilePath("Filename with spaces.RunSettings");

            // When
            var result = fixture.Run();

            // Then
            Assert.Equal("\"/Working/Test1.dll\" /Settings:\"/Working/Filename with spaces.RunSettings\"", result.Args);
        }

        [Fact]
        public void Should_Use_Parallel_If_Enabled_In_Settings()
        {
            // Given
            var fixture = new VSTestRunnerFixture();
            fixture.Settings.Parallel = true;

            // When
            var result = fixture.Run();

            // Then
            Assert.Equal("\"/Working/Test1.dll\" /Parallel", result.Args);
        }

        [Fact]
        public void Should_Use_EnableCodeCoverage_If_Enabled_In_Settings()
        {
            // Given
            var fixture = new VSTestRunnerFixture();
            fixture.Settings.EnableCodeCoverage = true;

            // When
            var result = fixture.Run();

            // Then
            Assert.Equal("\"/Working/Test1.dll\" /EnableCodeCoverage", result.Args);
        }

        [Fact]
        public void Should_Use_PlatformArchitecture_If_Provided()
        {
            // Given
            var fixture = new VSTestRunnerFixture();
            fixture.Settings.PlatformArchitecture = VSTestPlatform.x64;

            // When
            var result = fixture.Run();

            // Then
            Assert.Equal("\"/Working/Test1.dll\" /Platform:x64", result.Args);
        }

        [Fact]
        public void Should_Use_FrameworkVersion_If_Provided()
        {
            // Given
            var fixture = new VSTestRunnerFixture();
            fixture.Settings.FrameworkVersion = VSTestFrameworkVersion.NET40;

            // When
            var result = fixture.Run();

            // Then
            Assert.Equal("\"/Working/Test1.dll\" /Framework:Framework40", result.Args);
        }

        [Fact]
        public void Should_Use_TestCaseFilter_If_Provided()
        {
            // Given
            var fixture = new VSTestRunnerFixture();
            fixture.Settings.TestCaseFilter = "(FullyQualifiedName~Nightly|Name=MyTestMethod)";

            // When
            var result = fixture.Run();

            // Then
            Assert.Equal("\"/Working/Test1.dll\" /TestCaseFilter:\"(FullyQualifiedName~Nightly|Name=MyTestMethod)\"", result.Args);
        }

        [Fact]
        public void Should_Add_FilePath_For_Each_Assembly()
        {
            // Given
            var fixture = new VSTestRunnerFixture();
            fixture.AssemblyPaths = new[] { new FilePath("./Test1.dll"), new FilePath("./Test2.dll") };

            // When
            var result = fixture.Run();

            // Then
            Assert.Equal("\"/Working/Test1.dll\" \"/Working/Test2.dll\"", result.Args);
        }
    }
}