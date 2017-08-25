// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tests.Fixtures.Tools.Chocolatey.Installer;
using Cake.Testing;
using Cake.Testing.Xunit;
using Xunit;

namespace Cake.Common.Tests.Unit.Tools.Chocolatey.Install
{
    public sealed class ChocolateyInstallerTests
    {
        public sealed class TheInstallMethod
        {
            [Fact]
            public void Should_Throw_If_Target_Package_Id_Is_Null()
            {
                // Given
                var fixture = new ChocolateyInstallFixture();
                fixture.PackageId = null;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "packageId");
            }

            [Fact]
            public void Should_Throw_If_Settings_Are_Null()
            {
                // Given
                var fixture = new ChocolateyInstallFixture();
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
                var fixture = new ChocolateyInstallFixture();
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
                var fixture = new ChocolateyInstallFixture();
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
                var fixture = new ChocolateyInstallFixture();
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
                var fixture = new ChocolateyInstallFixture();
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
                var fixture = new ChocolateyInstallFixture();
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
                var fixture = new ChocolateyInstallFixture();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/Working/tools/choco.exe", result.Path.FullPath);
            }

            [Fact]
            public void Should_Add_Mandatory_Arguments()
            {
                // Given
                var fixture = new ChocolateyInstallFixture();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("install \"Cake\" -y", result.Args);
            }

            [Theory]
            [InlineData(true, "install \"Cake\" -d -y")]
            [InlineData(false, "install \"Cake\" -y")]
            public void Should_Add_Debug_Flag_To_Arguments_If_Set(bool debug, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallFixture();
                fixture.Settings.Debug = debug;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "install \"Cake\" -v -y")]
            [InlineData(false, "install \"Cake\" -y")]
            public void Should_Add_Verbose_Flag_To_Arguments_If_Set(bool verbose, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallFixture();
                fixture.Settings.Verbose = verbose;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "install \"Cake\" --acceptLicense -y")]
            [InlineData(false, "install \"Cake\" -y")]
            public void Should_Add_AcceptLicense_Flag_To_Arguments_If_Set(bool acceptLicense, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallFixture();
                fixture.Settings.AcceptLicense = acceptLicense;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "install \"Cake\" -y -f")]
            [InlineData(false, "install \"Cake\" -y")]
            public void Should_Add_Force_Flag_To_Arguments_If_Set(bool force, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallFixture();
                fixture.Settings.Force = force;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "install \"Cake\" -y --noop")]
            [InlineData(false, "install \"Cake\" -y")]
            public void Should_Add_Noop_Flag_To_Arguments_If_Set(bool noop, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallFixture();
                fixture.Settings.Noop = noop;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "install \"Cake\" -y -r")]
            [InlineData(false, "install \"Cake\" -y")]
            public void Should_Add_LimitOutput_Flag_To_Arguments_If_Set(bool limitOutput, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallFixture();
                fixture.Settings.LimitOutput = limitOutput;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(5, "install \"Cake\" -y --execution-timeout \"5\"")]
            [InlineData(0, "install \"Cake\" -y")]
            public void Should_Add_ExecutionTimeout_To_Arguments_If_Set(int executionTimeout, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallFixture();
                fixture.Settings.ExecutionTimeout = executionTimeout;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(@"c:\temp", "install \"Cake\" -y -c \"c:\\temp\"")]
            [InlineData("", "install \"Cake\" -y")]
            public void Should_Add_CacheLocation_Flag_To_Arguments_If_Set(string cacheLocation, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallFixture();
                fixture.Settings.CacheLocation = cacheLocation;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "install \"Cake\" -y --allowunofficial")]
            [InlineData(false, "install \"Cake\" -y")]
            public void Should_Add_AllowUnofficial_Flag_To_Arguments_If_Set(bool allowUnofficial, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallFixture();
                fixture.Settings.AllowUnofficial = allowUnofficial;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Fact]
            public void Should_Add_Source_To_Arguments_If_Set()
            {
                // Given
                var fixture = new ChocolateyInstallFixture();
                fixture.Settings.Source = "A";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("install \"Cake\" -y -s \"A\"", result.Args);
            }

            [Fact]
            public void Should_Add_Version_To_Arguments_If_Not_Null()
            {
                // Given
                var fixture = new ChocolateyInstallFixture();
                fixture.Settings.Version = "1.0.0";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("install \"Cake\" -y --version \"1.0.0\"", result.Args);
            }

            [Theory]
            [InlineData(true, "install \"Cake\" -y --pre")]
            [InlineData(false, "install \"Cake\" -y")]
            public void Should_Add_Prerelease_Flag_To_Arguments_If_Set(bool prerelease, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallFixture();
                fixture.Settings.Prerelease = prerelease;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "install \"Cake\" -y --x86")]
            [InlineData(false, "install \"Cake\" -y")]
            public void Should_Add_Forcex86_Flag_To_Arguments_If_Set(bool forcex86, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallFixture();
                fixture.Settings.Forcex86 = forcex86;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData("args1", "install \"Cake\" -y --ia \"args1\"")]
            [InlineData("", "install \"Cake\" -y")]
            public void Should_Add_InstallArguments_To_Arguments_If_Set(string installArgs, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallFixture();
                fixture.Settings.InstallArguments = installArgs;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "install \"Cake\" -y -o")]
            [InlineData(false, "install \"Cake\" -y")]
            public void Should_Add_OverrideArguments_Flag_To_Arguments_If_Set(bool overrideArguments, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallFixture();
                fixture.Settings.OverrideArguments = overrideArguments;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "install \"Cake\" -y --notSilent")]
            [InlineData(false, "install \"Cake\" -y")]
            public void Should_Add_NotSilent_Flag_To_Arguments_If_Set(bool notSilent, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallFixture();
                fixture.Settings.NotSilent = notSilent;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData("param1", "install \"Cake\" -y --params \"param1\"")]
            [InlineData("", "install \"Cake\" -y")]
            public void Should_Add_PackageParameters_To_Arguments_If_Set(string packageParameters, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallFixture();
                fixture.Settings.PackageParameters = packageParameters;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "install \"Cake\" -y --allowdowngrade")]
            [InlineData(false, "install \"Cake\" -y")]
            public void Should_Add_AllowDowngrade_Flag_To_Arguments_If_Set(bool allowDowngrade, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallFixture();
                fixture.Settings.AllowDowngrade = allowDowngrade;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "install \"Cake\" -y -m")]
            [InlineData(false, "install \"Cake\" -y")]
            public void Should_Add_SideBySide_Flag_To_Arguments_If_Set(bool sideBySide, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallFixture();
                fixture.Settings.SideBySide = sideBySide;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "install \"Cake\" -y -i")]
            [InlineData(false, "install \"Cake\" -y")]
            public void Should_Add_IgnoreDependencies_Flag_To_Arguments_If_Set(bool ignoreDependencies, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallFixture();
                fixture.Settings.IgnoreDependencies = ignoreDependencies;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "install \"Cake\" -y -x")]
            [InlineData(false, "install \"Cake\" -y")]
            public void Should_Add_ForceDependencies_Flag_To_Arguments_If_Set(bool forceDependencies, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallFixture();
                fixture.Settings.ForceDependencies = forceDependencies;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "install \"Cake\" -y -n")]
            [InlineData(false, "install \"Cake\" -y")]
            public void Should_Add_SkipPowerShell_Flag_To_Arguments_If_Set(bool skipPowerShell, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallFixture();
                fixture.Settings.SkipPowerShell = skipPowerShell;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData("user1", "install \"Cake\" -y -u \"user1\"")]
            [InlineData("", "install \"Cake\" -y")]
            public void Should_Add_User_To_Arguments_If_Set(string user, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallFixture();
                fixture.Settings.User = user;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData("password1", "install \"Cake\" -y -p \"password1\"")]
            [InlineData("", "install \"Cake\" -y")]
            public void Should_Add_Password_To_Arguments_If_Set(string password, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallFixture();
                fixture.Settings.Password = password;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "install \"Cake\" -y --ignorechecksums")]
            [InlineData(false, "install \"Cake\" -y")]
            public void Should_Add_IgnoreChecksums_Flag_To_Arguments_If_Set(bool ignoreChecksums, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallFixture();
                fixture.Settings.IgnoreChecksums = ignoreChecksums;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }
        }

        public sealed class TheInstallFromConfigMethod
        {
            [Fact]
            public void Should_Throw_If_Target_Package_Config_Path_Is_Null()
            {
                // Given
                var fixture = new ChocolateyInstallFromConfigFixture();
                fixture.PackageConfigPath = null;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "packageConfigPath");
            }

            [Fact]
            public void Should_Throw_If_Settings_Are_Null()
            {
                // Given
                var fixture = new ChocolateyInstallFromConfigFixture();
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
                var fixture = new ChocolateyInstallFromConfigFixture();
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
                var fixture = new ChocolateyInstallFromConfigFixture();
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
                var fixture = new ChocolateyInstallFromConfigFixture();
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
                var fixture = new ChocolateyInstallFromConfigFixture();
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
                var fixture = new ChocolateyInstallFromConfigFixture();
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
                var fixture = new ChocolateyInstallFromConfigFixture();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/Working/tools/choco.exe", result.Path.FullPath);
            }

            [Fact]
            public void Should_Add_Mandatory_Arguments()
            {
                // Given
                var fixture = new ChocolateyInstallFromConfigFixture();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("install \"/Working/packages.config\" -y", result.Args);
            }

            [Theory]
            [InlineData(true, "install \"/Working/packages.config\" -d -y")]
            [InlineData(false, "install \"/Working/packages.config\" -y")]
            public void Should_Add_Debug_Flag_To_Arguments_If_Set(bool debug, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallFromConfigFixture();
                fixture.Settings.Debug = debug;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "install \"/Working/packages.config\" -v -y")]
            [InlineData(false, "install \"/Working/packages.config\" -y")]
            public void Should_Add_Verbose_Flag_To_Arguments_If_Set(bool verbose, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallFromConfigFixture();
                fixture.Settings.Verbose = verbose;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "install \"/Working/packages.config\" -y -f")]
            [InlineData(false, "install \"/Working/packages.config\" -y")]
            public void Should_Add_Force_Flag_To_Arguments_If_Set(bool force, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallFromConfigFixture();
                fixture.Settings.Force = force;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "install \"/Working/packages.config\" -y --noop")]
            [InlineData(false, "install \"/Working/packages.config\" -y")]
            public void Should_Add_Noop_Flag_To_Arguments_If_Set(bool noop, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallFromConfigFixture();
                fixture.Settings.Noop = noop;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "install \"/Working/packages.config\" -y -r")]
            [InlineData(false, "install \"/Working/packages.config\" -y")]
            public void Should_Add_LimitOutput_Flag_To_Arguments_If_Set(bool limitOutput, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallFromConfigFixture();
                fixture.Settings.LimitOutput = limitOutput;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(5, "install \"/Working/packages.config\" -y --execution-timeout \"5\"")]
            [InlineData(0, "install \"/Working/packages.config\" -y")]
            public void Should_Add_ExecutionTimeout_To_Arguments_If_Set(int executionTimeout, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallFromConfigFixture();
                fixture.Settings.ExecutionTimeout = executionTimeout;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(@"c:\temp", "install \"/Working/packages.config\" -y -c \"c:\\temp\"")]
            [InlineData("", "install \"/Working/packages.config\" -y")]
            public void Should_Add_CacheLocation_Flag_To_Arguments_If_Set(string cacheLocation, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallFromConfigFixture();
                fixture.Settings.CacheLocation = cacheLocation;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "install \"/Working/packages.config\" -y --allowunofficial")]
            [InlineData(false, "install \"/Working/packages.config\" -y")]
            public void Should_Add_AllowUnofficial_Flag_To_Arguments_If_Set(bool allowUnofficial, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallFromConfigFixture();
                fixture.Settings.AllowUnofficial = allowUnofficial;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Fact]
            public void Should_Add_Source_To_Arguments_If_Set()
            {
                // Given
                var fixture = new ChocolateyInstallFromConfigFixture();
                fixture.Settings.Source = "A";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("install \"/Working/packages.config\" -y -s \"A\"", result.Args);
            }
        }
    }
}