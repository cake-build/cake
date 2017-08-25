// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tests.Fixtures.Tools;
using Cake.Core;
using Cake.Core.IO;
using Cake.Testing;
using Cake.Testing.Xunit;
using Xunit;

namespace Cake.Common.Tests.Unit.Tools.MSTest
{
    public sealed class MSTestRunnerTests
    {
        [Fact]
        public void Should_Throw_If_Assembly_Path_Is_Null()
        {
            // Given
            var fixture = new MSTestRunnerFixture();
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
            var fixture = new MSTestRunnerFixture();
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
            var fixture = new MSTestRunnerFixture();
            fixture.GivenDefaultToolDoNotExist();

            // When
            var result = Record.Exception(() => fixture.Run());

            // Then
            Assert.IsType<CakeException>(result);
            Assert.Equal("MSTest: Could not locate executable.", result?.Message);
        }

        [Theory]
        [InlineData("/bin/xUnit/xunit.exe", "/bin/xUnit/xunit.exe")]
        [InlineData("./tools/xUnit/xunit.exe", "/Working/tools/xUnit/xunit.exe")]
        public void Should_Use_MSTest_From_Tool_Path_If_Provided(string toolPath, string expected)
        {
            // Given
            var fixture = new MSTestRunnerFixture();
            fixture.Settings.ToolPath = toolPath;
            fixture.GivenSettingsToolPathExist();

            // When
            var result = fixture.Run();

            // Then
            Assert.Equal(expected, result.Path.FullPath);
        }

        [WindowsTheory]
        [InlineData("C:/xUnit/xunit.exe", "C:/xUnit/xunit.exe")]
        public void Should_Use_MSTest_From_Tool_Path_If_Provided_On_Windows(string toolPath, string expected)
        {
            // Given
            var fixture = new MSTestRunnerFixture();
            fixture.Settings.ToolPath = toolPath;
            fixture.GivenSettingsToolPathExist();

            // When
            var result = fixture.Run();

            // Then
            Assert.Equal(expected, result.Path.FullPath);
        }

        [Theory]
        [InlineData("/ProgramFilesX86/Microsoft Visual Studio 14.0/Common7/IDE/mstest.exe")]
        [InlineData("/ProgramFilesX86/Microsoft Visual Studio 12.0/Common7/IDE/mstest.exe")]
        [InlineData("/ProgramFilesX86/Microsoft Visual Studio 11.0/Common7/IDE/mstest.exe")]
        [InlineData("/ProgramFilesX86/Microsoft Visual Studio 10.0/Common7/IDE/mstest.exe")]
        public void Should_Use_Available_Tool_Path(string existingToolPath)
        {
            // Given
            var fixture = new MSTestRunnerFixture();
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
            var fixture = new MSTestRunnerFixture();

            // When
            var result = fixture.Run();

            // Then
            Assert.Equal("/Working", result.Process.WorkingDirectory.FullPath);
        }

        [Fact]
        public void Should_Throw_If_Process_Was_Not_Started()
        {
            // Given
            var fixture = new MSTestRunnerFixture();
            fixture.GivenProcessCannotStart();

            // When
            var result = Record.Exception(() => fixture.Run());

            // Then
            Assert.IsType<CakeException>(result);
            Assert.Equal("MSTest: Process was not started.", result?.Message);
        }

        [Fact]
        public void Should_Throw_If_Process_Has_A_Non_Zero_Exit_Code()
        {
            // Given
            var fixture = new MSTestRunnerFixture();
            fixture.GivenProcessExitsWithCode(1);

            // When
            var result = Record.Exception(() => fixture.Run());

            // Then
            Assert.IsType<CakeException>(result);
            Assert.Equal("MSTest: Process returned an error (exit code 1).", result?.Message);
        }

        [Fact]
        public void Should_Not_Use_Isolation_By_Default()
        {
            // Given
            var fixture = new MSTestRunnerFixture();

            // When
            var result = fixture.Run();

            // Then
            Assert.Equal("\"/testcontainer:/Working/Test1.dll\" /noisolation", result.Args);
        }

        [Fact]
        public void Should_Use_Isolation_If_Disabled_In_Settings()
        {
            // Given
            var fixture = new MSTestRunnerFixture();
            fixture.Settings.NoIsolation = false;

            // When
            var result = fixture.Run();

            // Then
            Assert.Equal("\"/testcontainer:/Working/Test1.dll\"", result.Args);
        }

        [Fact]
        public void Should_Use_TestCategoryFilter_If_Provided()
        {
            // Given
            var fixture = new MSTestRunnerFixture();
            fixture.Settings.Category = "!Network";

            // When
            var result = fixture.Run();

            // Then
            Assert.Equal("\"/testcontainer:/Working/Test1.dll\" /category:\"!Network\" /noisolation", result.Args);
        }

        [Fact]
        public void Should_Use_TestResultsFile_If_Provided()
        {
            // Given
            var fixture = new MSTestRunnerFixture();
            fixture.Settings.ResultsFile = @"c:\temp\myresults.trx";

            // When
            var result = fixture.Run();

            // Then
            Assert.Equal("\"/testcontainer:/Working/Test1.dll\" /resultsfile:\"c:\\temp\\myresults.trx\" /noisolation", result.Args);
        }

        [Fact]
        public void Should_Not_Use_TestCategoryFilter_If_Not_Provided()
        {
            // Given
            var fixture = new MSTestRunnerFixture();

            // When
            var result = fixture.Run();

            // Then
            Assert.Equal("\"/testcontainer:/Working/Test1.dll\" /noisolation", result.Args);
        }

        [Fact]
        public void Should_Add_Testcontainer_For_Each_Assembly()
        {
            // Given
            var fixture = new MSTestRunnerFixture();
            fixture.AssemblyPaths = new[] { new FilePath("./Test1.dll"), new FilePath("./Test2.dll") };

            // When
            var result = fixture.Run();

            // Then
            Assert.Equal("\"/testcontainer:/Working/Test1.dll\" \"/testcontainer:/Working/Test2.dll\" /noisolation", result.Args);
        }

        [Fact]
        public void Should_Use_Test_Settings_File_If_Set()
        {
            // Given
            var fixture = new MSTestRunnerFixture();
            fixture.Settings.TestSettings = "my.testsettings";

            // When
            var result = fixture.Run();

            // Then
            Assert.Equal("\"/testcontainer:/Working/Test1.dll\" /noisolation /testsettings:\"/Working/my.testsettings\"", result.Args);
        }

        [Theory]
        [InlineData("/NonStandardPath/Microsoft Visual Studio 14.0/Common7/IDE/mstest.exe", "VS140COMNTOOLS", "/NonStandardPath/Microsoft Visual Studio 14.0/Common7/Tools/")]
        [InlineData("/NonStandardPath/Microsoft Visual Studio 13.0/Common7/IDE/mstest.exe", "VS130COMNTOOLS", "/NonStandardPath/Microsoft Visual Studio 13.0/Common7/Tools/")]
        [InlineData("/NonStandardPath/Microsoft Visual Studio 12.0/Common7/IDE/mstest.exe", "VS120COMNTOOLS", "/NonStandardPath/Microsoft Visual Studio 12.0/Common7/Tools/")]
        [InlineData("/NonStandardPath/Microsoft Visual Studio 11.0/Common7/IDE/mstest.exe", "VS110COMNTOOLS", "/NonStandardPath/Microsoft Visual Studio 11.0/Common7/Tools/")]
        [InlineData("/NonStandardPath/Microsoft Visual Studio 10.0/Common7/IDE/mstest.exe", "VS100COMNTOOLS", "/NonStandardPath/Microsoft Visual Studio 10.0/Common7/Tools/")]
        public void Should_Use_Available_Environment_Tool_Path(string existingToolPath, string environmentName, string environmentValue)
        {
            // Given
            var fixture = new MSTestRunnerFixture();
            fixture.GivenDefaultToolDoNotExist();
            fixture.FileSystem.CreateFile(existingToolPath);
            fixture.Environment.SetEnvironmentVariable(environmentName, environmentValue);

            // When
            var result = fixture.Run();

            // Then
            Assert.Equal(existingToolPath, result.Path.FullPath);
        }
    }
}