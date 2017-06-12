// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tests.Fixtures.Tools.Chocolatey.Uninstaller;
using Cake.Testing;
using Cake.Testing.Xunit;
using Xunit;

namespace Cake.Common.Tests.Unit.Tools.Chocolatey.Uninstall
{
    public sealed class ChocolateyUninstallerTests
    {
        public sealed class TheUninstallMethod
        {
            [Fact]
            public void Should_Throw_If_Traget_Package_Id_Is_Null()
            {
                // Given
                var fixture = new ChocolateyUninstallerFixture();
                fixture.PackageIds = null;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "packageIds");
            }

            [Fact]
            public void Should_Throw_If_Settings_Are_Null()
            {
                // Given
                var fixture = new ChocolateyUninstallerFixture();
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
                var fixture = new ChocolateyUninstallerFixture();
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
                var fixture = new ChocolateyUninstallerFixture();
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
                var fixture = new ChocolateyUninstallerFixture();
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
                var fixture = new ChocolateyUninstallerFixture();
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
                var fixture = new ChocolateyUninstallerFixture();
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
                var fixture = new ChocolateyUninstallerFixture();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/Working/tools/choco.exe", result.Path.FullPath);
            }

            [Fact]
            public void Should_Add_Mandatory_Arguments()
            {
                // Given
                var fixture = new ChocolateyUninstallerFixture();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("uninstall \"Cake\" -y", result.Args);
            }

            [Theory]
            [InlineData(true, "uninstall \"Cake\" -d -y")]
            [InlineData(false, "uninstall \"Cake\" -y")]
            public void Should_Add_Debug_Flag_To_Arguments_If_Set(bool debug, string expected)
            {
                // Given
                var fixture = new ChocolateyUninstallerFixture();
                fixture.Settings.Debug = debug;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "uninstall \"Cake\" -v -y")]
            [InlineData(false, "uninstall \"Cake\" -y")]
            public void Should_Add_Verbose_Flag_To_Arguments_If_Set(bool verbose, string expected)
            {
                // Given
                var fixture = new ChocolateyUninstallerFixture();
                fixture.Settings.Verbose = verbose;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "uninstall \"Cake\" --acceptLicense -y")]
            [InlineData(false, "uninstall \"Cake\" -y")]
            public void Should_Add_AcceptLicense_Flag_To_Arguments_If_Set(bool acceptLicense, string expected)
            {
                // Given
                var fixture = new ChocolateyUninstallerFixture();
                fixture.Settings.AcceptLicense = acceptLicense;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "uninstall \"Cake\" -y -f")]
            [InlineData(false, "uninstall \"Cake\" -y")]
            public void Should_Add_Force_Flag_To_Arguments_If_Set(bool force, string expected)
            {
                // Given
                var fixture = new ChocolateyUninstallerFixture();
                fixture.Settings.Force = force;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "uninstall \"Cake\" -y --noop")]
            [InlineData(false, "uninstall \"Cake\" -y")]
            public void Should_Add_Noop_Flag_To_Arguments_If_Set(bool noop, string expected)
            {
                // Given
                var fixture = new ChocolateyUninstallerFixture();
                fixture.Settings.Noop = noop;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "uninstall \"Cake\" -y -r")]
            [InlineData(false, "uninstall \"Cake\" -y")]
            public void Should_Add_LimitOutput_Flag_To_Arguments_If_Set(bool limitOutput, string expected)
            {
                // Given
                var fixture = new ChocolateyUninstallerFixture();
                fixture.Settings.LimitOutput = limitOutput;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(5, "uninstall \"Cake\" -y --execution-timeout \"5\"")]
            [InlineData(0, "uninstall \"Cake\" -y")]
            public void Should_Add_ExecutionTimeout_To_Arguments_If_Set(int executionTimeout, string expected)
            {
                // Given
                var fixture = new ChocolateyUninstallerFixture();
                fixture.Settings.ExecutionTimeout = executionTimeout;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(@"c:\temp", "uninstall \"Cake\" -y -c \"c:\\temp\"")]
            [InlineData("", "uninstall \"Cake\" -y")]
            public void Should_Add_CacheLocation_Flag_To_Arguments_If_Set(string cacheLocation, string expected)
            {
                // Given
                var fixture = new ChocolateyUninstallerFixture();
                fixture.Settings.CacheLocation = cacheLocation;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "uninstall \"Cake\" -y --allowunofficial")]
            [InlineData(false, "uninstall \"Cake\" -y")]
            public void Should_Add_AllowUnofficial_Flag_To_Arguments_If_Set(bool allowUnofficial, string expected)
            {
                // Given
                var fixture = new ChocolateyUninstallerFixture();
                fixture.Settings.AllowUnofficial = allowUnofficial;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "uninstall \"Cake\" -y --failstderr")]
            [InlineData(false, "uninstall \"Cake\" -y")]
            public void Should_Add_FailOnStandardError_To_Arguments_If_Set(bool failOnStandardError, string expected)
            {
                // Given
                var fixture = new ChocolateyUninstallerFixture();
                fixture.Settings.FailOnStandardError = failOnStandardError;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "uninstall \"Cake\" -y --use-system-powershell")]
            [InlineData(false, "uninstall \"Cake\" -y")]
            public void Should_Add_UseSystemPowershell_To_Arguments_If_Set(bool useSystemPowershell, string expected)
            {
                // Given
                var fixture = new ChocolateyUninstallerFixture();
                fixture.Settings.UseSystemPowershell = useSystemPowershell;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Fact]
            public void Should_Add_Source_To_Arguments_If_Set()
            {
                // Given
                var fixture = new ChocolateyUninstallerFixture();
                fixture.Settings.Source = "A";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("uninstall \"Cake\" -y -s \"A\"", result.Args);
            }

            [Fact]
            public void Should_Add_Version_To_Arguments_If_Not_Null()
            {
                // Given
                var fixture = new ChocolateyUninstallerFixture();
                fixture.Settings.Version = "1.0.0";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("uninstall \"Cake\" -y --version \"1.0.0\"", result.Args);
            }

            [Theory]
            [InlineData(true, "uninstall \"Cake\" -y --allversions")]
            [InlineData(false, "uninstall \"Cake\" -y")]
            public void Should_Add_AllVersions_To_Arguments_If_Set(bool allVersions, string expected)
            {
                // Given
                var fixture = new ChocolateyUninstallerFixture();
                fixture.Settings.AllVersions = allVersions;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "uninstall \"Cake\" -y -o")]
            [InlineData(false, "uninstall \"Cake\" -y")]
            public void Should_Add_OverrideArguments_Flag_To_Arguments_If_Set(bool overrideArguments, string expected)
            {
                // Given
                var fixture = new ChocolateyUninstallerFixture();
                fixture.Settings.OverrideArguments = overrideArguments;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "uninstall \"Cake\" -y --notSilent")]
            [InlineData(false, "uninstall \"Cake\" -y")]
            public void Should_Add_NotSilent_Flag_To_Arguments_If_Set(bool notSilent, string expected)
            {
                // Given
                var fixture = new ChocolateyUninstallerFixture();
                fixture.Settings.NotSilent = notSilent;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData("param1", "uninstall \"Cake\" -y --params \"param1\"")]
            [InlineData("", "uninstall \"Cake\" -y")]
            public void Should_Add_PackageParameters_To_Arguments_If_Set(string packageParameters, string expected)
            {
                // Given
                var fixture = new ChocolateyUninstallerFixture();
                fixture.Settings.PackageParameters = packageParameters;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "uninstall \"Cake\" -y --argsglobal")]
            [InlineData(false, "uninstall \"Cake\" -y")]
            public void Should_Add_GlobalArguments_To_Arguments_If_Set(bool globalArguments, string expected)
            {
                // Given
                var fixture = new ChocolateyUninstallerFixture();
                fixture.Settings.GlobalArguments = globalArguments;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "uninstall \"Cake\" -y --paramsglobal")]
            [InlineData(false, "uninstall \"Cake\" -y")]
            public void Should_Add_GlobalPackageParameters_To_Arguments_If_Set(bool globalPackageParameters, string expected)
            {
                // Given
                var fixture = new ChocolateyUninstallerFixture();
                fixture.Settings.GlobalPackageParameters = globalPackageParameters;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "uninstall \"Cake\" -y -m")]
            [InlineData(false, "uninstall \"Cake\" -y")]
            public void Should_Add_SideBySide_Flag_To_Arguments_If_Set(bool sideBySide, string expected)
            {
                // Given
                var fixture = new ChocolateyUninstallerFixture();
                fixture.Settings.SideBySide = sideBySide;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "uninstall \"Cake\" -y -x")]
            [InlineData(false, "uninstall \"Cake\" -y")]
            public void Should_Add_ForceDependencies_Flag_To_Arguments_If_Set(bool forceDependencies, string expected)
            {
                // Given
                var fixture = new ChocolateyUninstallerFixture();
                fixture.Settings.ForceDependencies = forceDependencies;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "uninstall \"Cake\" -y -n")]
            [InlineData(false, "uninstall \"Cake\" -y")]
            public void Should_Add_SkipPowerShell_Flag_To_Arguments_If_Set(bool skipPowerShell, string expected)
            {
                // Given
                var fixture = new ChocolateyUninstallerFixture();
                fixture.Settings.SkipPowerShell = skipPowerShell;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "uninstall \"Cake\" -y --ignorepackageexitcodes")]
            [InlineData(false, "uninstall \"Cake\" -y")]
            public void Should_Add_IgnorePackageExitCoedes_To_Arguments_If_Set(bool ignorePackageExitCodes, string expected)
            {
                // Given
                var fixture = new ChocolateyUninstallerFixture();
                fixture.Settings.IgnorePackageExitCodes = ignorePackageExitCodes;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "uninstall \"Cake\" -y --usepackageexitcodes")]
            [InlineData(false, "uninstall \"Cake\" -y")]
            public void Should_Add_UsePackageExitCodes_To_Arguments_If_Set(bool usePackageExitCodes, string expected)
            {
                // Given
                var fixture = new ChocolateyUninstallerFixture();
                fixture.Settings.UsePackageExitCodes = usePackageExitCodes;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "uninstall \"Cake\" -y --use-autouninstaller")]
            [InlineData(false, "uninstall \"Cake\" -y")]
            public void Should_Add_UseAutoUninstaller_To_Arguments_If_Set(bool useAutoUninstaller, string expected)
            {
                // Given
                var fixture = new ChocolateyUninstallerFixture();
                fixture.Settings.UseAutoUninstaller = useAutoUninstaller;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "uninstall \"Cake\" -y --skipautouninstaller")]
            [InlineData(false, "uninstall \"Cake\" -y")]
            public void Should_Add_SkipAutoUninstaller_To_Arguments_If_Set(bool skipAutoUninstaller, string expected)
            {
                // Given
                var fixture = new ChocolateyUninstallerFixture();
                fixture.Settings.SkipAutoUninstaller = skipAutoUninstaller;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "uninstall \"Cake\" -y --failonautouninstaller")]
            [InlineData(false, "uninstall \"Cake\" -y")]
            public void Should_Add_FailOnAutoUninstaller_To_Arguments_If_Set(bool failOnAutoUninstaller, string expected)
            {
                // Given
                var fixture = new ChocolateyUninstallerFixture();
                fixture.Settings.FailOnAutoUninstaller = failOnAutoUninstaller;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "uninstall \"Cake\" -y --ignoreautouninstallerfailure")]
            [InlineData(false, "uninstall \"Cake\" -y")]
            public void Should_Add_IgnoreAutoUninstallerFailure_To_Arguments_If_Set(bool ignoreAutoUninstaller, string expected)
            {
                // Given
                var fixture = new ChocolateyUninstallerFixture();
                fixture.Settings.IgnoreAutoUninstaller = ignoreAutoUninstaller;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }
        }
    }
}
