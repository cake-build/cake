// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tests.Fixtures.Tools.Chocolatey.ApiKey;
using Cake.Common.Tests.Fixtures.Tools.Chocolatey.Installer;
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
                Assert.Equal("uninstall \"Cake\" --confirm", result.Args);
            }

            [Theory]
            [InlineData(true, "uninstall \"Cake\" --debug --confirm")]
            [InlineData(false, "uninstall \"Cake\" --confirm")]
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
            [InlineData(true, "uninstall \"Cake\" --verbose --confirm")]
            [InlineData(false, "uninstall \"Cake\" --confirm")]
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
            [InlineData(true, "uninstall \"Cake\" --trace --confirm")]
            [InlineData(false, "uninstall \"Cake\" --confirm")]
            public void Should_Add_Trace_Flag_To_Arguments_If_Set(bool trace, string expected)
            {
                // Given
                var fixture = new ChocolateyUninstallerFixture();
                fixture.Settings.Trace = trace;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "uninstall \"Cake\" --no-color --confirm")]
            [InlineData(false, "uninstall \"Cake\" --confirm")]
            public void Should_Add_NoColor_Flag_To_Arguments_If_Set(bool noColor, string expected)
            {
                // Given
                var fixture = new ChocolateyUninstallerFixture();
                fixture.Settings.NoColor = noColor;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "uninstall \"Cake\" --accept-license --confirm")]
            [InlineData(false, "uninstall \"Cake\" --confirm")]
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
            [InlineData(true, "uninstall \"Cake\" --confirm --force")]
            [InlineData(false, "uninstall \"Cake\" --confirm")]
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
            [InlineData(true, "uninstall \"Cake\" --confirm --what-if")]
            [InlineData(false, "uninstall \"Cake\" --confirm")]
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
            [InlineData(true, "uninstall \"Cake\" --confirm --limit-output")]
            [InlineData(false, "uninstall \"Cake\" --confirm")]
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
            [InlineData(5, "uninstall \"Cake\" --confirm --execution-timeout=\"5\"")]
            [InlineData(0, "uninstall \"Cake\" --confirm")]
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
            [InlineData(@"c:\temp", "uninstall \"Cake\" --confirm --cache-location=\"c:\\temp\"")]
            [InlineData("", "uninstall \"Cake\" --confirm")]
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
            [InlineData(true, "uninstall \"Cake\" --confirm --allow-unofficial")]
            [InlineData(false, "uninstall \"Cake\" --confirm")]
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
            [InlineData(true, "uninstall \"Cake\" --confirm --fail-on-error-output")]
            [InlineData(false, "uninstall \"Cake\" --confirm")]
            public void Should_Add_FailOnErrorOutput_Flag_To_Arguments_If_Set(bool failOnErrorOutput, string expected)
            {
                // Given
                var fixture = new ChocolateyUninstallerFixture();
                fixture.Settings.FailOnErrorOutput = failOnErrorOutput;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "uninstall \"Cake\" --confirm --use-system-powershell")]
            [InlineData(false, "uninstall \"Cake\" --confirm")]
            public void Should_Add_UseSystemPowerShell_Flag_To_Arguments_If_Set(bool useSystemPowerShell, string expected)
            {
                // Given
                var fixture = new ChocolateyUninstallerFixture();
                fixture.Settings.UseSystemPowerShell = useSystemPowerShell;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "uninstall \"Cake\" --confirm --no-progress")]
            [InlineData(false, "uninstall \"Cake\" --confirm")]
            public void Should_Add_NoProgress_Flag_To_Arguments_If_Set(bool noProgress, string expected)
            {
                // Given
                var fixture = new ChocolateyUninstallerFixture();
                fixture.Settings.NoProgress = noProgress;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData("proxy", "uninstall \"Cake\" --confirm --proxy=\"proxy\"")]
            [InlineData(null, "uninstall \"Cake\" --confirm")]
            public void Should_Add_Proxy_Flag_To_Arguments_If_Set(string proxy, string expected)
            {
                // Given
                var fixture = new ChocolateyUninstallerFixture();
                fixture.Settings.Proxy = proxy;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData("proxy-user", "uninstall \"Cake\" --confirm --proxy-user=\"proxy-user\"")]
            [InlineData(null, "uninstall \"Cake\" --confirm")]
            public void Should_Add_ProxyUser_Flag_To_Arguments_If_Set(string proxyUser, string expected)
            {
                // Given
                var fixture = new ChocolateyUninstallerFixture();
                fixture.Settings.ProxyUser = proxyUser;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData("proxy-password", "uninstall \"Cake\" --confirm --proxy-password=\"proxy-password\"")]
            [InlineData(null, "uninstall \"Cake\" --confirm")]
            public void Should_Add_ProxyPassword_Flag_To_Arguments_If_Set(string proxyPassword, string expected)
            {
                // Given
                var fixture = new ChocolateyUninstallerFixture();
                fixture.Settings.ProxyPassword = proxyPassword;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData("proxy1,proxy2", "uninstall \"Cake\" --confirm --proxy-bypass-list=\"proxy1,proxy2\"")]
            [InlineData(null, "uninstall \"Cake\" --confirm")]
            public void Should_Add_ProxyByPassList_Flag_To_Arguments_If_Set(string proxyByPassList, string expected)
            {
                // Given
                var fixture = new ChocolateyUninstallerFixture();
                fixture.Settings.ProxyByPassList = proxyByPassList;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "uninstall \"Cake\" --confirm --proxy-bypass-on-local")]
            [InlineData(false, "uninstall \"Cake\" --confirm")]
            public void Should_Add_ProxyByPassOnLocal_Flag_To_Arguments_If_Set(bool proxyBypassOnLocal, string expected)
            {
                // Given
                var fixture = new ChocolateyUninstallerFixture();
                fixture.Settings.ProxyBypassOnLocal = proxyBypassOnLocal;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData("./output.log", "uninstall \"Cake\" --confirm --log-file=\"/Working/output.log\"")]
            [InlineData(null, "uninstall \"Cake\" --confirm")]
            public void Should_Add_Log_File_Flag_To_Arguments_If_Set(string logFilePath, string expected)
            {
                // Given
                var fixture = new ChocolateyUninstallerFixture();
                fixture.Settings.LogFile = logFilePath;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "uninstall \"Cake\" --confirm --skip-compatibility-checks")]
            [InlineData(false, "uninstall \"Cake\" --confirm")]
            public void Should_Add_Skip_Compatibility_Flag_To_Arguments_If_Set(bool skipCompatibiity, string expected)
            {
                // Given
                var fixture = new ChocolateyUninstallerFixture();
                fixture.Settings.SkipCompatibilityChecks = skipCompatibiity;

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
                Assert.Equal("uninstall \"Cake\" --confirm --source=\"A\"", result.Args);
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
                Assert.Equal("uninstall \"Cake\" --confirm --version=\"1.0.0\"", result.Args);
            }

            [Theory]
            [InlineData(true, "uninstall \"Cake\" --confirm --all-versions")]
            [InlineData(false, "uninstall \"Cake\" --confirm")]
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
            [InlineData("arg1, arg2", "uninstall \"Cake\" --confirm --uninstall-arguments=\"arg1, arg2\"")]
            [InlineData("", "uninstall \"Cake\" --confirm")]
            public void Should_Add_UninstallArguments_Flag_To_Arguments_If_Set(string uninstallArguments, string expected)
            {
                // Given
                var fixture = new ChocolateyUninstallerFixture();
                fixture.Settings.UninstallArguments = uninstallArguments;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "uninstall \"Cake\" --confirm --override-arguments")]
            [InlineData(false, "uninstall \"Cake\" --confirm")]
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
            [InlineData(true, "uninstall \"Cake\" --confirm --not-silent")]
            [InlineData(false, "uninstall \"Cake\" --confirm")]
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
            [InlineData("param1", "uninstall \"Cake\" --confirm --package-parameters=\"param1\"")]
            [InlineData("", "uninstall \"Cake\" --confirm")]
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
            [InlineData(true, "uninstall \"Cake\" --confirm --apply-install-arguments-to-dependencies")]
            [InlineData(false, "uninstall \"Cake\" --confirm")]
            public void Should_Add_ApplyInstallArgumentsToDependencies_To_Arguments_If_Set(bool applyInstallArgumentsToDependencies, string expected)
            {
                // Given
                var fixture = new ChocolateyUninstallerFixture();
                fixture.Settings.ApplyInstallArgumentsToDependencies = applyInstallArgumentsToDependencies;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "uninstall \"Cake\" --confirm --apply-package-parameters-to-dependencies")]
            [InlineData(false, "uninstall \"Cake\" --confirm")]
            public void Should_Add_ApplyPackageParametersToDependencies_To_Arguments_If_Set(bool applyPackageParametersToDependencies, string expected)
            {
                // Given
                var fixture = new ChocolateyUninstallerFixture();
                fixture.Settings.ApplyPackageParametersToDependencies = applyPackageParametersToDependencies;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "uninstall \"Cake\" --confirm --side-by-side")]
            [InlineData(false, "uninstall \"Cake\" --confirm")]
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
            [InlineData(true, "uninstall \"Cake\" --confirm --force-dependencies")]
            [InlineData(false, "uninstall \"Cake\" --confirm")]
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
            [InlineData(true, "uninstall \"Cake\" --confirm --skip-automation-scripts")]
            [InlineData(false, "uninstall \"Cake\" --confirm")]
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
            [InlineData(true, "uninstall \"Cake\" --confirm --ignore-package-exit-codes")]
            [InlineData(false, "uninstall \"Cake\" --confirm")]
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
            [InlineData(true, "uninstall \"Cake\" --confirm --use-package-exit-codes")]
            [InlineData(false, "uninstall \"Cake\" --confirm")]
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
            [InlineData(true, "uninstall \"Cake\" --confirm --use-autouninstaller")]
            [InlineData(false, "uninstall \"Cake\" --confirm")]
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
            [InlineData(true, "uninstall \"Cake\" --confirm --skip-autouninstaller")]
            [InlineData(false, "uninstall \"Cake\" --confirm")]
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
            [InlineData(true, "uninstall \"Cake\" --confirm --fail-on-autouninstaller")]
            [InlineData(false, "uninstall \"Cake\" --confirm")]
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
            [InlineData(true, "uninstall \"Cake\" --confirm --ignore-autouninstaller-failure")]
            [InlineData(false, "uninstall \"Cake\" --confirm")]
            public void Should_Add_IgnoreAutoUninstallerFailure_To_Arguments_If_Set(bool ignoreAutoUninstaller, string expected)
            {
                // Given
                var fixture = new ChocolateyUninstallerFixture();
                fixture.Settings.IgnoreAutoUninstallerFailure = ignoreAutoUninstaller;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "uninstall \"Cake\" --confirm --stop-on-first-package-failure")]
            [InlineData(false, "uninstall \"Cake\" --confirm")]
            public void Should_Add_StopOnFirstFailure_Flag_To_Arguments_If_Set(bool stopOnFirstFailure, string expected)
            {
                // Given
                var fixture = new ChocolateyUninstallerFixture();
                fixture.Settings.StopOnFirstFailure = stopOnFirstFailure;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "uninstall \"Cake\" --confirm --exit-when-reboot-detected")]
            [InlineData(false, "uninstall \"Cake\" --confirm")]
            public void Should_Add_ExitWhenRebootDetected_Flag_To_Arguments_If_Set(bool exitWhenRebootDetected, string expected)
            {
                // Given
                var fixture = new ChocolateyUninstallerFixture();
                fixture.Settings.ExitWhenRebootDetected = exitWhenRebootDetected;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "uninstall \"Cake\" --confirm --ignore-detected-reboot")]
            [InlineData(false, "uninstall \"Cake\" --confirm")]
            public void Should_Add_IgnoreDetectedReboot_Flag_To_Arguments_If_Set(bool ignoreDetectedReboot, string expected)
            {
                // Given
                var fixture = new ChocolateyUninstallerFixture();
                fixture.Settings.IgnoreDetectedReboot = ignoreDetectedReboot;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "uninstall \"Cake\" --confirm --skip-hooks")]
            [InlineData(false, "uninstall \"Cake\" --confirm")]
            public void Should_Add_SkipHooks_Flag_To_Arguments_If_Set(bool skipHooks, string expected)
            {
                // Given
                var fixture = new ChocolateyUninstallerFixture();
                fixture.Settings.SkipHooks = skipHooks;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "uninstall \"Cake\" --confirm --from-programs-and-features")]
            [InlineData(false, "uninstall \"Cake\" --confirm")]
            public void Should_Add_FromProgramsAndFeatures_Flag_To_Arguments_If_Set(bool fromProgramsAndFeatures, string expected)
            {
                // Given
                var fixture = new ChocolateyUninstallerFixture();
                fixture.Settings.FromProgramsAndFeatures = fromProgramsAndFeatures;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }
        }
    }
}
