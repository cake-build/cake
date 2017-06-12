// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tests.Fixtures.Tools.Chocolatey.Pin;
using Cake.Testing;
using Cake.Testing.Xunit;
using Xunit;

namespace Cake.Common.Tests.Unit.Tools.Chocolatey.Pin
{
    public sealed class ChocolateyPinnerTests
    {
        public sealed class ThePinMethod
        {
            [Fact]
            public void Should_Throw_If_Settings_Are_Null()
            {
                // Given
                var fixture = new ChocolateyPinnerFixture();
                fixture.Settings = null;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "settings");
            }

            [Fact]
            public void Should_Throw_If_Chocolatey_Executable_Was_Not_Found()
            {
                // Given
                var fixture = new ChocolateyPinnerFixture();
                fixture.GivenDefaultToolDoNotExist();

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsCakeException(result, "Chocolatey: Could not locate executable.");
            }

            [Theory]
            [InlineData("/bin/chocolatey/choco.exe", "/bin/chocolatey/choco.exe")]
            [InlineData("./chocolatey/choco.exe", "/Working/chocolatey/choco.exe")]
            public void Should_Use_Chocolatey_Executable_From_Tool_Path_If_Provided(string toolPath, string expected)
            {
                // Given
                var fixture = new ChocolateyPinnerFixture();
                fixture.Settings.ToolPath = toolPath;
                fixture.GivenSettingsToolPathExist();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Path.FullPath);
            }

            [WindowsTheory]
            [InlineData("C:/ProgramData/chocolatey/choco.exe", "C:/ProgramData/chocolatey/choco.exe")]
            public void Should_Use_Chocolatey_Executable_From_Tool_Path_If_Provided_On_Windows(string toolPath, string expected)
            {
                // Given
                var fixture = new ChocolateyPinnerFixture();
                fixture.Settings.ToolPath = toolPath;
                fixture.GivenSettingsToolPathExist();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Path.FullPath);
            }

            [Fact]
            public void Should_Throw_If_Process_Was_Not_Started()
            {
                // Given
                var fixture = new ChocolateyPinnerFixture();
                fixture.GivenProcessCannotStart();

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsCakeException(result, "Chocolatey: Process was not started.");
            }

            [Fact]
            public void Should_Throw_If_Process_Has_A_Non_Zero_Exit_Code()
            {
                // Given
                var fixture = new ChocolateyPinnerFixture();
                fixture.GivenProcessExitsWithCode(1);

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsCakeException(result, "Chocolatey: Process returned an error (exit code 1).");
            }

            [Fact]
            public void Should_Find_Chocolatey_Executable_If_Tool_Path_Not_Provided()
            {
                // Given
                var fixture = new ChocolateyPinnerFixture();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/Working/tools/choco.exe", result.Path.FullPath);
            }

            [Fact]
            public void Should_Add_Mandatory_Arguments()
            {
                // Given
                var fixture = new ChocolateyPinnerFixture();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("pin add -n \"Cake\" -y", result.Args);
            }

            [Theory]
            [InlineData(true, "pin add -n \"Cake\" -d -y")]
            [InlineData(false, "pin add -n \"Cake\" -y")]
            public void Should_Add_Debug_Flag_To_Arguments_If_Set(bool debug, string expected)
            {
                // Given
                var fixture = new ChocolateyPinnerFixture();
                fixture.Settings.Debug = debug;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "pin add -n \"Cake\" -v -y")]
            [InlineData(false, "pin add -n \"Cake\" -y")]
            public void Should_Add_Verbose_Flag_To_Arguments_If_Set(bool verbose, string expected)
            {
                // Given
                var fixture = new ChocolateyPinnerFixture();
                fixture.Settings.Verbose = verbose;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "pin add -n \"Cake\" -y -f")]
            [InlineData(false, "pin add -n \"Cake\" -y")]
            public void Should_Add_Force_Flag_To_Arguments_If_Set(bool force, string expected)
            {
                // Given
                var fixture = new ChocolateyPinnerFixture();
                fixture.Settings.Force = force;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "pin add -n \"Cake\" -y --noop")]
            [InlineData(false, "pin add -n \"Cake\" -y")]
            public void Should_Add_Noop_Flag_To_Arguments_If_Set(bool noop, string expected)
            {
                // Given
                var fixture = new ChocolateyPinnerFixture();
                fixture.Settings.Noop = noop;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "pin add -n \"Cake\" -y -r")]
            [InlineData(false, "pin add -n \"Cake\" -y")]
            public void Should_Add_LimitOutput_Flag_To_Arguments_If_Set(bool limitOutput, string expected)
            {
                // Given
                var fixture = new ChocolateyPinnerFixture();
                fixture.Settings.LimitOutput = limitOutput;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(5, "pin add -n \"Cake\" -y --execution-timeout \"5\"")]
            [InlineData(0, "pin add -n \"Cake\" -y")]
            public void Should_Add_ExecutionTimeout_To_Arguments_If_Set(int executionTimeout, string expected)
            {
                // Given
                var fixture = new ChocolateyPinnerFixture();
                fixture.Settings.ExecutionTimeout = executionTimeout;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(@"c:\temp", "pin add -n \"Cake\" -y -c \"c:\\temp\"")]
            [InlineData("", "pin add -n \"Cake\" -y")]
            public void Should_Add_CacheLocation_Flag_To_Arguments_If_Set(string cacheLocation, string expected)
            {
                // Given
                var fixture = new ChocolateyPinnerFixture();
                fixture.Settings.CacheLocation = cacheLocation;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "pin add -n \"Cake\" -y --allowunofficial")]
            [InlineData(false, "pin add -n \"Cake\" -y")]
            public void Should_Add_AllowUnofficial_Flag_To_Arguments_If_Set(bool allowUnofficial, string expected)
            {
                // Given
                var fixture = new ChocolateyPinnerFixture();
                fixture.Settings.AllowUnofficial = allowUnofficial;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Fact]
            public void Should_Add_Version_To_Arguments_If_Not_Null()
            {
                // Given
                var fixture = new ChocolateyPinnerFixture();
                fixture.Settings.Version = "1.0.0";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("pin add -n \"Cake\" --version \"1.0.0\" -y", result.Args);
            }
        }
    }
}