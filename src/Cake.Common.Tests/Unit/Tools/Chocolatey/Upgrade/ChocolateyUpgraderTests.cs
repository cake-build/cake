// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tests.Fixtures.Tools.Chocolatey.Upgrade;
using Cake.Testing;
using Cake.Testing.Xunit;
using Xunit;

namespace Cake.Common.Tests.Unit.Tools.Chocolatey.Upgrade
{
    public sealed class ChocolateyUpgraderTests
    {
        public sealed class TheInstallMethod
        {
            [Fact]
            public void Should_Throw_If_Target_Package_Id_Is_Null()
            {
                // Given
                var fixture = new ChocolateyUpgraderFixture();
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
                var fixture = new ChocolateyUpgraderFixture();
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
                var fixture = new ChocolateyUpgraderFixture();
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
                var fixture = new ChocolateyUpgraderFixture();
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
                var fixture = new ChocolateyUpgraderFixture();
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
                var fixture = new ChocolateyUpgraderFixture();
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
                var fixture = new ChocolateyUpgraderFixture();
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
                var fixture = new ChocolateyUpgraderFixture();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/Working/tools/choco.exe", result.Path.FullPath);
            }

            [Fact]
            public void Should_Add_Mandatory_Arguments()
            {
                // Given
                var fixture = new ChocolateyUpgraderFixture();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("upgrade \"Cake\" --confirm", result.Args);
            }

            [Theory]
            [InlineData(true, "upgrade \"Cake\" --debug --confirm")]
            [InlineData(false, "upgrade \"Cake\" --confirm")]
            public void Should_Add_Debug_Flag_To_Arguments_If_Set(bool debug, string expected)
            {
                // Given
                var fixture = new ChocolateyUpgraderFixture();
                fixture.Settings.Debug = debug;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "upgrade \"Cake\" --verbose --confirm")]
            [InlineData(false, "upgrade \"Cake\" --confirm")]
            public void Should_Add_Verbose_Flag_To_Arguments_If_Set(bool verbose, string expected)
            {
                // Given
                var fixture = new ChocolateyUpgraderFixture();
                fixture.Settings.Verbose = verbose;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "upgrade \"Cake\" --trace --confirm")]
            [InlineData(false, "upgrade \"Cake\" --confirm")]
            public void Should_Add_Trace_Flag_To_Arguments_If_Set(bool trace, string expected)
            {
                // Given
                var fixture = new ChocolateyUpgraderFixture();
                fixture.Settings.Trace = trace;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "upgrade \"Cake\" --no-color --confirm")]
            [InlineData(false, "upgrade \"Cake\" --confirm")]
            public void Should_Add_NoColor_Flag_To_Arguments_If_Set(bool noColor, string expected)
            {
                // Given
                var fixture = new ChocolateyUpgraderFixture();
                fixture.Settings.NoColor = noColor;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "upgrade \"Cake\" --accept-license --confirm")]
            [InlineData(false, "upgrade \"Cake\" --confirm")]
            public void Should_Add_AcceptLicense_Flag_To_Arguments_If_Set(bool acceptLicense, string expected)
            {
                // Given
                var fixture = new ChocolateyUpgraderFixture();
                fixture.Settings.AcceptLicense = acceptLicense;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "upgrade \"Cake\" --confirm --force")]
            [InlineData(false, "upgrade \"Cake\" --confirm")]
            public void Should_Add_Force_Flag_To_Arguments_If_Set(bool force, string expected)
            {
                // Given
                var fixture = new ChocolateyUpgraderFixture();
                fixture.Settings.Force = force;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "upgrade \"Cake\" --confirm --what-if")]
            [InlineData(false, "upgrade \"Cake\" --confirm")]
            public void Should_Add_Noop_Flag_To_Arguments_If_Set(bool noop, string expected)
            {
                // Given
                var fixture = new ChocolateyUpgraderFixture();
                fixture.Settings.Noop = noop;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "upgrade \"Cake\" --confirm --limit-output")]
            [InlineData(false, "upgrade \"Cake\" --confirm")]
            public void Should_Add_LimitOutput_Flag_To_Arguments_If_Set(bool limitOutput, string expected)
            {
                // Given
                var fixture = new ChocolateyUpgraderFixture();
                fixture.Settings.LimitOutput = limitOutput;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(5, "upgrade \"Cake\" --confirm --execution-timeout=\"5\"")]
            [InlineData(0, "upgrade \"Cake\" --confirm")]
            public void Should_Add_ExecutionTimeout_To_Arguments_If_Set(int executionTimeout, string expected)
            {
                // Given
                var fixture = new ChocolateyUpgraderFixture();
                fixture.Settings.ExecutionTimeout = executionTimeout;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(@"c:\temp", "upgrade \"Cake\" --confirm --cache-location=\"c:\\temp\"")]
            [InlineData("", "upgrade \"Cake\" --confirm")]
            public void Should_Add_CacheLocation_Flag_To_Arguments_If_Set(string cacheLocation, string expected)
            {
                // Given
                var fixture = new ChocolateyUpgraderFixture();
                fixture.Settings.CacheLocation = cacheLocation;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "upgrade \"Cake\" --confirm --allow-unofficial")]
            [InlineData(false, "upgrade \"Cake\" --confirm")]
            public void Should_Add_AllowUnofficial_Flag_To_Arguments_If_Set(bool allowUnofficial, string expected)
            {
                // Given
                var fixture = new ChocolateyUpgraderFixture();
                fixture.Settings.AllowUnofficial = allowUnofficial;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "upgrade \"Cake\" --confirm --fail-on-error-output")]
            [InlineData(false, "upgrade \"Cake\" --confirm")]
            public void Should_Add_FailOnErrorOutput_Flag_To_Arguments_If_Set(bool failOnErrorOutput, string expected)
            {
                // Given
                var fixture = new ChocolateyUpgraderFixture();
                fixture.Settings.FailOnErrorOutput = failOnErrorOutput;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "upgrade \"Cake\" --confirm --use-system-powershell")]
            [InlineData(false, "upgrade \"Cake\" --confirm")]
            public void Should_Add_UseSystemPowerShell_Flag_To_Arguments_If_Set(bool useSystemPowerShell, string expected)
            {
                // Given
                var fixture = new ChocolateyUpgraderFixture();
                fixture.Settings.UseSystemPowerShell = useSystemPowerShell;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "upgrade \"Cake\" --confirm --no-progress")]
            [InlineData(false, "upgrade \"Cake\" --confirm")]
            public void Should_Add_NoProgress_Flag_To_Arguments_If_Set(bool noProgress, string expected)
            {
                // Given
                var fixture = new ChocolateyUpgraderFixture();
                fixture.Settings.NoProgress = noProgress;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData("proxy", "upgrade \"Cake\" --confirm --proxy=\"proxy\"")]
            [InlineData(null, "upgrade \"Cake\" --confirm")]
            public void Should_Add_Proxy_Flag_To_Arguments_If_Set(string proxy, string expected)
            {
                // Given
                var fixture = new ChocolateyUpgraderFixture();
                fixture.Settings.Proxy = proxy;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData("proxy-user", "upgrade \"Cake\" --confirm --proxy-user=\"proxy-user\"")]
            [InlineData(null, "upgrade \"Cake\" --confirm")]
            public void Should_Add_ProxyUser_Flag_To_Arguments_If_Set(string proxyUser, string expected)
            {
                // Given
                var fixture = new ChocolateyUpgraderFixture();
                fixture.Settings.ProxyUser = proxyUser;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData("proxy-password", "upgrade \"Cake\" --confirm --proxy-password=\"proxy-password\"")]
            [InlineData(null, "upgrade \"Cake\" --confirm")]
            public void Should_Add_ProxyPassword_Flag_To_Arguments_If_Set(string proxyPassword, string expected)
            {
                // Given
                var fixture = new ChocolateyUpgraderFixture();
                fixture.Settings.ProxyPassword = proxyPassword;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData("proxy1,proxy2", "upgrade \"Cake\" --confirm --proxy-bypass-list=\"proxy1,proxy2\"")]
            [InlineData(null, "upgrade \"Cake\" --confirm")]
            public void Should_Add_ProxyByPassList_Flag_To_Arguments_If_Set(string proxyByPassList, string expected)
            {
                // Given
                var fixture = new ChocolateyUpgraderFixture();
                fixture.Settings.ProxyByPassList = proxyByPassList;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "upgrade \"Cake\" --confirm --proxy-bypass-on-local")]
            [InlineData(false, "upgrade \"Cake\" --confirm")]
            public void Should_Add_ProxyByPassOnLocal_Flag_To_Arguments_If_Set(bool proxyBypassOnLocal, string expected)
            {
                // Given
                var fixture = new ChocolateyUpgraderFixture();
                fixture.Settings.ProxyBypassOnLocal = proxyBypassOnLocal;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData("./output.log", "upgrade \"Cake\" --confirm --log-file=\"/Working/output.log\"")]
            [InlineData(null, "upgrade \"Cake\" --confirm")]
            public void Should_Add_Log_File_Flag_To_Arguments_If_Set(string logFilePath, string expected)
            {
                // Given
                var fixture = new ChocolateyUpgraderFixture();
                fixture.Settings.LogFile = logFilePath;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "upgrade \"Cake\" --confirm --skip-compatibility-checks")]
            [InlineData(false, "upgrade \"Cake\" --confirm")]
            public void Should_Add_Skip_Compatibility_Flag_To_Arguments_If_Set(bool skipCompatibiity, string expected)
            {
                // Given
                var fixture = new ChocolateyUpgraderFixture();
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
                var fixture = new ChocolateyUpgraderFixture();
                fixture.Settings.Source = "A";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("upgrade \"Cake\" --confirm --source=\"A\"", result.Args);
            }

            [Fact]
            public void Should_Add_Version_To_Arguments_If_Not_Null()
            {
                // Given
                var fixture = new ChocolateyUpgraderFixture();
                fixture.Settings.Version = "1.0.0";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("upgrade \"Cake\" --confirm --version=\"1.0.0\"", result.Args);
            }

            [Theory]
            [InlineData(true, "upgrade \"Cake\" --confirm --pre")]
            [InlineData(false, "upgrade \"Cake\" --confirm")]
            public void Should_Add_Prerelease_Flag_To_Arguments_If_Set(bool prerelease, string expected)
            {
                // Given
                var fixture = new ChocolateyUpgraderFixture();
                fixture.Settings.Prerelease = prerelease;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "upgrade \"Cake\" --confirm --forcex86")]
            [InlineData(false, "upgrade \"Cake\" --confirm")]
            public void Should_Add_Forcex86_Flag_To_Arguments_If_Set(bool forcex86, string expected)
            {
                // Given
                var fixture = new ChocolateyUpgraderFixture();
                fixture.Settings.Forcex86 = forcex86;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData("args1", "upgrade \"Cake\" --confirm --install-arguments=\"args1\"")]
            [InlineData("", "upgrade \"Cake\" --confirm")]
            public void Should_Add_InstallArguments_To_Arguments_If_Set(string installArgs, string expected)
            {
                // Given
                var fixture = new ChocolateyUpgraderFixture();
                fixture.Settings.InstallArguments = installArgs;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "upgrade \"Cake\" --confirm --override-arguments")]
            [InlineData(false, "upgrade \"Cake\" --confirm")]
            public void Should_Add_OverrideArguments_Flag_To_Arguments_If_Set(bool overrideArguments, string expected)
            {
                // Given
                var fixture = new ChocolateyUpgraderFixture();
                fixture.Settings.OverrideArguments = overrideArguments;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "upgrade \"Cake\" --confirm --not-silent")]
            [InlineData(false, "upgrade \"Cake\" --confirm")]
            public void Should_Add_NotSilent_Flag_To_Arguments_If_Set(bool notSilent, string expected)
            {
                // Given
                var fixture = new ChocolateyUpgraderFixture();
                fixture.Settings.NotSilent = notSilent;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData("param1", "upgrade \"Cake\" --confirm --package-parameters=\"param1\"")]
            [InlineData("", "upgrade \"Cake\" --confirm")]
            public void Should_Add_PackageParameters_To_Arguments_If_Set(string packageParameters, string expected)
            {
                // Given
                var fixture = new ChocolateyUpgraderFixture();
                fixture.Settings.PackageParameters = packageParameters;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "upgrade \"Cake\" --confirm --apply-install-arguments-to-dependencies")]
            [InlineData(false, "upgrade \"Cake\" --confirm")]
            public void Should_Add_ApplyInstallArgumentsToDependencies_To_Arguments_If_Set(bool applyInstallArgumentsToDependencies, string expected)
            {
                // Given
                var fixture = new ChocolateyUpgraderFixture();
                fixture.Settings.ApplyInstallArgumentsToDependencies = applyInstallArgumentsToDependencies;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "upgrade \"Cake\" --confirm --apply-package-parameters-to-dependencies")]
            [InlineData(false, "upgrade \"Cake\" --confirm")]
            public void Should_Add_ApplyPackageParametersToDependencies_To_Arguments_If_Set(bool applyPackageParametersToDependencies, string expected)
            {
                // Given
                var fixture = new ChocolateyUpgraderFixture();
                fixture.Settings.ApplyPackageParametersToDependencies = applyPackageParametersToDependencies;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "upgrade \"Cake\" --confirm --allow-downgrade")]
            [InlineData(false, "upgrade \"Cake\" --confirm")]
            public void Should_Add_AllowDowngrade_Flag_To_Arguments_If_Set(bool allowDowngrade, string expected)
            {
                // Given
                var fixture = new ChocolateyUpgraderFixture();
                fixture.Settings.AllowDowngrade = allowDowngrade;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "upgrade \"Cake\" --confirm --side-by-side")]
            [InlineData(false, "upgrade \"Cake\" --confirm")]
            public void Should_Add_SideBySide_Flag_To_Arguments_If_Set(bool sideBySide, string expected)
            {
                // Given
                var fixture = new ChocolateyUpgraderFixture();
                fixture.Settings.SideBySide = sideBySide;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "upgrade \"Cake\" --confirm --ignore-dependencies")]
            [InlineData(false, "upgrade \"Cake\" --confirm")]
            public void Should_Add_IgnoreDependencies_Flag_To_Arguments_If_Set(bool ignoreDependencies, string expected)
            {
                // Given
                var fixture = new ChocolateyUpgraderFixture();
                fixture.Settings.IgnoreDependencies = ignoreDependencies;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "upgrade \"Cake\" --confirm --skip-automation-scripts")]
            [InlineData(false, "upgrade \"Cake\" --confirm")]
            public void Should_Add_SkipPowerShell_Flag_To_Arguments_If_Set(bool skipPowerShell, string expected)
            {
                // Given
                var fixture = new ChocolateyUpgraderFixture();
                fixture.Settings.SkipPowerShell = skipPowerShell;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "upgrade \"Cake\" --confirm --fail-on-unfound")]
            [InlineData(false, "upgrade \"Cake\" --confirm")]
            public void Should_Add_FailOnUnfound_Flag_To_Arguments_If_Set(bool failOnUnfound, string expected)
            {
                // Given
                var fixture = new ChocolateyUpgraderFixture();
                fixture.Settings.FailOnUnfound = failOnUnfound;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "upgrade \"Cake\" --confirm --ignore-unfound")]
            [InlineData(false, "upgrade \"Cake\" --confirm")]
            public void Should_Add_IgnoreUnfound_Flag_To_Arguments_If_Set(bool ignoreUnfound, string expected)
            {
                // Given
                var fixture = new ChocolateyUpgraderFixture();
                fixture.Settings.IgnoreUnfound = ignoreUnfound;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "upgrade \"Cake\" --confirm --fail-on-not-installed")]
            [InlineData(false, "upgrade \"Cake\" --confirm")]
            public void Should_Add_FailOnNotInstalled_Flag_To_Arguments_If_Set(bool failOnNotInstalled, string expected)
            {
                // Given
                var fixture = new ChocolateyUpgraderFixture();
                fixture.Settings.FailOnNotInstalled = failOnNotInstalled;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData("user1", "upgrade \"Cake\" --confirm --user=\"user1\"")]
            [InlineData("", "upgrade \"Cake\" --confirm")]
            public void Should_Add_User_To_Arguments_If_Set(string user, string expected)
            {
                // Given
                var fixture = new ChocolateyUpgraderFixture();
                fixture.Settings.User = user;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData("password1", "upgrade \"Cake\" --confirm --password=\"password1\"")]
            [InlineData("", "upgrade \"Cake\" --confirm")]
            public void Should_Add_Password_To_Arguments_If_Set(string password, string expected)
            {
                // Given
                var fixture = new ChocolateyUpgraderFixture();
                fixture.Settings.Password = password;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData("./mycert.pfx", "upgrade \"Cake\" --confirm --cert=\"/Working/mycert.pfx\"")]
            [InlineData(null, "upgrade \"Cake\" --confirm")]
            public void Should_Add_Cert_To_Arguments_If_Set(string certificate, string expected)
            {
                // Given
                var fixture = new ChocolateyUpgraderFixture();
                fixture.Settings.Certificate = certificate;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData("certpassword", "upgrade \"Cake\" --confirm --certpassword=\"certpassword\"")]
            [InlineData("", "upgrade \"Cake\" --confirm")]
            public void Should_Add_CertPassword_To_Arguments_If_Set(string certificatePassword, string expected)
            {
                // Given
                var fixture = new ChocolateyUpgraderFixture();
                fixture.Settings.CertificatePassword = certificatePassword;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "upgrade \"Cake\" --confirm --ignore-checksums")]
            [InlineData(false, "upgrade \"Cake\" --confirm")]
            public void Should_Add_IgnoreChecksums_Flag_To_Arguments_If_Set(bool ignoreChecksums, string expected)
            {
                // Given
                var fixture = new ChocolateyUpgraderFixture();
                fixture.Settings.IgnoreChecksums = ignoreChecksums;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "upgrade \"Cake\" --confirm --allow-empty-checksums")]
            [InlineData(false, "upgrade \"Cake\" --confirm")]
            public void Should_Add_AllowEmptyChecksums_Flag_To_Arguments_If_Set(bool allowEmptyChecksums, string expected)
            {
                // Given
                var fixture = new ChocolateyUpgraderFixture();
                fixture.Settings.AllowEmptyChecksums = allowEmptyChecksums;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "upgrade \"Cake\" --confirm --allow-empty-checksums-secure")]
            [InlineData(false, "upgrade \"Cake\" --confirm")]
            public void Should_Add_AllowEmptyChecksumsSecure_Flag_To_Arguments_If_Set(bool allowEmptyChecksumsSecure, string expected)
            {
                // Given
                var fixture = new ChocolateyUpgraderFixture();
                fixture.Settings.AllowEmptyChecksumsSecure = allowEmptyChecksumsSecure;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "upgrade \"Cake\" --confirm --require-checksums")]
            [InlineData(false, "upgrade \"Cake\" --confirm")]
            public void Should_Add_RequireChecksums_Flag_To_Arguments_If_Set(bool requireChecksums, string expected)
            {
                // Given
                var fixture = new ChocolateyUpgraderFixture();
                fixture.Settings.RequireChecksums = requireChecksums;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData("abcdef", "upgrade \"Cake\" --confirm --download-checksum=\"abcdef\"")]
            [InlineData(null, "upgrade \"Cake\" --confirm")]
            public void Should_Add_Checksum_Flag_To_Arguments_If_Set(string checksum, string expected)
            {
                // Given
                var fixture = new ChocolateyUpgraderFixture();
                fixture.Settings.Checksum = checksum;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData("abcdef", "upgrade \"Cake\" --confirm --download-checksum-x64=\"abcdef\"")]
            [InlineData(null, "upgrade \"Cake\" --confirm")]
            public void Should_Add_Checksum64_Flag_To_Arguments_If_Set(string checksum64, string expected)
            {
                // Given
                var fixture = new ChocolateyUpgraderFixture();
                fixture.Settings.Checksum64 = checksum64;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData("md5", "upgrade \"Cake\" --confirm --download-checksum-type=\"md5\"")]
            [InlineData(null, "upgrade \"Cake\" --confirm")]
            public void Should_Add_ChecksumType_Flag_To_Arguments_If_Set(string checkSumType, string expected)
            {
                // Given
                var fixture = new ChocolateyUpgraderFixture();
                fixture.Settings.ChecksumType = checkSumType;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData("md5", "upgrade \"Cake\" --confirm --download-checksum-type-x64=\"md5\"")]
            [InlineData(null, "upgrade \"Cake\" --confirm")]
            public void Should_Add_ChecksumType64_Flag_To_Arguments_If_Set(string checkSumType64, string expected)
            {
                // Given
                var fixture = new ChocolateyUpgraderFixture();
                fixture.Settings.ChecksumType64 = checkSumType64;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "upgrade \"Cake\" --confirm --ignore-package-exit-codes")]
            [InlineData(false, "upgrade \"Cake\" --confirm")]
            public void Should_Add_IgnorePackageExitCodes_Flag_To_Arguments_If_Set(bool ignorePackageExitCodes, string expected)
            {
                // Given
                var fixture = new ChocolateyUpgraderFixture();
                fixture.Settings.IgnorePackageExitCodes = ignorePackageExitCodes;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "upgrade \"Cake\" --confirm --use-package-exit-codes")]
            [InlineData(false, "upgrade \"Cake\" --confirm")]
            public void Should_Add_UsePackageExitCodes_Flag_To_Arguments_If_Set(bool usePackageExitCodes, string expected)
            {
                // Given
                var fixture = new ChocolateyUpgraderFixture();
                fixture.Settings.UsePackageExitCodes = usePackageExitCodes;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData("packageA,packageB", "upgrade \"Cake\" --confirm --except=\"packageA,packageB\"")]
            [InlineData(null, "upgrade \"Cake\" --confirm")]
            public void Should_Add_Except_Flag_To_Arguments_If_Set(string except, string expected)
            {
                // Given
                var fixture = new ChocolateyUpgraderFixture();
                fixture.Settings.Except = except;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "upgrade \"Cake\" --confirm --stop-on-first-package-failure")]
            [InlineData(false, "upgrade \"Cake\" --confirm")]
            public void Should_Add_StopOnFirstFailure_Flag_To_Arguments_If_Set(bool stopOnFirstFailure, string expected)
            {
                // Given
                var fixture = new ChocolateyUpgraderFixture();
                fixture.Settings.StopOnFirstFailure = stopOnFirstFailure;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "upgrade \"Cake\" --confirm --skip-if-not-installed")]
            [InlineData(false, "upgrade \"Cake\" --confirm")]
            public void Should_Add_SkipIfNotInstalled_Flag_To_Arguments_If_Set(bool skipIfNotInstalled, string expected)
            {
                // Given
                var fixture = new ChocolateyUpgraderFixture();
                fixture.Settings.SkipIfNotInstalled = skipIfNotInstalled;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "upgrade \"Cake\" --confirm --install-if-not-installed")]
            [InlineData(false, "upgrade \"Cake\" --confirm")]
            public void Should_Add_InstallIfNotInstalled_Flag_To_Arguments_If_Set(bool installIfNotInstalled, string expected)
            {
                // Given
                var fixture = new ChocolateyUpgraderFixture();
                fixture.Settings.InstallIfNotInstalled = installIfNotInstalled;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "upgrade \"Cake\" --confirm --exclude-prerelease")]
            [InlineData(false, "upgrade \"Cake\" --confirm")]
            public void Should_Add_ExcludePrerelease_Flag_To_Arguments_If_Set(bool excludePrerelease, string expected)
            {
                // Given
                var fixture = new ChocolateyUpgraderFixture();
                fixture.Settings.ExcludePrerelease = excludePrerelease;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "upgrade \"Cake\" --confirm --use-remembered-arguments")]
            [InlineData(false, "upgrade \"Cake\" --confirm")]
            public void Should_Add_UseRememberedArguments_Flag_To_Arguments_If_Set(bool useRememberedArguments, string expected)
            {
                // Given
                var fixture = new ChocolateyUpgraderFixture();
                fixture.Settings.UseRememberedArguments = useRememberedArguments;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "upgrade \"Cake\" --confirm --ignore-remembered-arguments")]
            [InlineData(false, "upgrade \"Cake\" --confirm")]
            public void Should_Add_IgnoreRememeredArguments_Flag_To_Arguments_If_Set(bool ignoreRememeredArguments, string expected)
            {
                // Given
                var fixture = new ChocolateyUpgraderFixture();
                fixture.Settings.IgnoreRememeredArguments = ignoreRememeredArguments;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "upgrade \"Cake\" --confirm --exit-when-reboot-detected")]
            [InlineData(false, "upgrade \"Cake\" --confirm")]
            public void Should_Add_ExitWhenRebootDetected_Flag_To_Arguments_If_Set(bool exitWhenRebootDetected, string expected)
            {
                // Given
                var fixture = new ChocolateyUpgraderFixture();
                fixture.Settings.ExitWhenRebootDetected = exitWhenRebootDetected;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "upgrade \"Cake\" --confirm --ignore-detected-reboot")]
            [InlineData(false, "upgrade \"Cake\" --confirm")]
            public void Should_Add_IgnoreDetectedReboot_Flag_To_Arguments_If_Set(bool ignoreDetectedReboot, string expected)
            {
                // Given
                var fixture = new ChocolateyUpgraderFixture();
                fixture.Settings.IgnoreDetectedReboot = ignoreDetectedReboot;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "upgrade \"Cake\" --confirm --disable-repository-optimizations")]
            [InlineData(false, "upgrade \"Cake\" --confirm")]
            public void Should_Add_DisableRepositoryOptimizations_Flag_To_Arguments_If_Set(bool disableRepositoryOptimizations, string expected)
            {
                // Given
                var fixture = new ChocolateyUpgraderFixture();
                fixture.Settings.DisableRepositoryOptimizations = disableRepositoryOptimizations;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "upgrade \"Cake\" --confirm --pin-package")]
            [InlineData(false, "upgrade \"Cake\" --confirm")]
            public void Should_Add_Pin_Flag_To_Arguments_If_Set(bool pin, string expected)
            {
                // Given
                var fixture = new ChocolateyUpgraderFixture();
                fixture.Settings.Pin = pin;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "upgrade \"Cake\" --confirm --skip-hooks")]
            [InlineData(false, "upgrade \"Cake\" --confirm")]
            public void Should_Add_SkipHooks_Flag_To_Arguments_If_Set(bool skipHooks, string expected)
            {
                // Given
                var fixture = new ChocolateyUpgraderFixture();
                fixture.Settings.SkipHooks = skipHooks;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "upgrade \"Cake\" --confirm --skip-download-cache")]
            [InlineData(false, "upgrade \"Cake\" --confirm")]
            public void Should_Add_SkipDownloadCache_Flag_To_Arguments_If_Set(bool skipDownloadCache, string expected)
            {
                // Given
                var fixture = new ChocolateyUpgraderFixture();
                fixture.Settings.SkipDownloadCache = skipDownloadCache;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "upgrade \"Cake\" --confirm --use-download-cache")]
            [InlineData(false, "upgrade \"Cake\" --confirm")]
            public void Should_Add_UseDownloadCache_Flag_To_Arguments_If_Set(bool useDownloadCache, string expected)
            {
                // Given
                var fixture = new ChocolateyUpgraderFixture();
                fixture.Settings.UseDownloadCache = useDownloadCache;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "upgrade \"Cake\" --confirm --skip-virus-check")]
            [InlineData(false, "upgrade \"Cake\" --confirm")]
            public void Should_Add_SkipVirusCheck_Flag_To_Arguments_If_Set(bool skipVirusCheck, string expected)
            {
                // Given
                var fixture = new ChocolateyUpgraderFixture();
                fixture.Settings.SkipVirusCheck = skipVirusCheck;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "upgrade \"Cake\" --confirm --virus-check")]
            [InlineData(false, "upgrade \"Cake\" --confirm")]
            public void Should_Add_VirusCheck_Flag_To_Arguments_If_Set(bool virusCheck, string expected)
            {
                // Given
                var fixture = new ChocolateyUpgraderFixture();
                fixture.Settings.VirusCheck = virusCheck;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(5, "upgrade \"Cake\" --confirm --virus-positives-minimum=\"5\"")]
            [InlineData(0, "upgrade \"Cake\" --confirm")]
            public void Should_Add_VirusPositivesMinimum_To_Arguments_If_Set(int virusPositivesMinimum, string expected)
            {
                // Given
                var fixture = new ChocolateyUpgraderFixture();
                fixture.Settings.VirusPositivesMinimum = virusPositivesMinimum;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData("super-secret", "upgrade \"Cake\" --confirm --install-arguments-sensitive=\"super-secret\"")]
            [InlineData(null, "upgrade \"Cake\" --confirm")]
            public void Should_Add_InstallArgumentsSensitive_Flag_To_Arguments_If_Set(string installArgumentsSensitive, string expected)
            {
                // Given
                var fixture = new ChocolateyUpgraderFixture();
                fixture.Settings.InstallArgumentsSensitive = installArgumentsSensitive;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData("super-secret", "upgrade \"Cake\" --confirm --package-parameters-sensitive=\"super-secret\"")]
            [InlineData(null, "upgrade \"Cake\" --confirm")]
            public void Should_Add_PackageParametersSensitive_Flag_To_Arguments_If_Set(string packageParametersSensitive, string expected)
            {
                // Given
                var fixture = new ChocolateyUpgraderFixture();
                fixture.Settings.PackageParametersSensitive = packageParametersSensitive;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData("./install", "upgrade \"Cake\" --confirm --install-directory=\"/Working/install\"")]
            [InlineData(null, "upgrade \"Cake\" --confirm")]
            public void Should_Add_InstallDirectory_Flag_To_Arguments_If_Set(string installDirectory, string expected)
            {
                // Given
                var fixture = new ChocolateyUpgraderFixture();
                fixture.Settings.InstallDirectory = installDirectory;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(5, "upgrade \"Cake\" --confirm --maximum-download-bits-per-second=\"5\"")]
            [InlineData(0, "upgrade \"Cake\" --confirm")]
            public void Should_Add_MaximumDownloadBitsPerSecond_Flag_To_Arguments_If_Set(int maximumDownloadBitsPerSecond, string expected)
            {
                // Given
                var fixture = new ChocolateyUpgraderFixture();
                fixture.Settings.MaximumDownloadBitsPerSecond = maximumDownloadBitsPerSecond;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "upgrade \"Cake\" --confirm --reduce-package-size")]
            [InlineData(false, "upgrade \"Cake\" --confirm")]
            public void Should_Add_ReducePackageSize_Flag_To_Arguments_If_Set(bool reducePackageSize, string expected)
            {
                // Given
                var fixture = new ChocolateyUpgraderFixture();
                fixture.Settings.ReducePackageSize = reducePackageSize;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "upgrade \"Cake\" --confirm --no-reduce-package-size")]
            [InlineData(false, "upgrade \"Cake\" --confirm")]
            public void Should_Add_NoReducePackageSize_Flag_To_Arguments_If_Set(bool noReducePackageSize, string expected)
            {
                // Given
                var fixture = new ChocolateyUpgraderFixture();
                fixture.Settings.NoReducePackageSize = noReducePackageSize;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "upgrade \"Cake\" --confirm --reduce-nupkg-only")]
            [InlineData(false, "upgrade \"Cake\" --confirm")]
            public void Should_Add_ReduceNupkgOnly_Flag_To_Arguments_If_Set(bool reduceNupkgOnly, string expected)
            {
                // Given
                var fixture = new ChocolateyUpgraderFixture();
                fixture.Settings.ReduceNupkgOnly = reduceNupkgOnly;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "upgrade \"Cake\" --confirm --exclude-chocolatey-packages-during-upgrade-all")]
            [InlineData(false, "upgrade \"Cake\" --confirm")]
            public void Should_Add_ExcludeChocolateyPackagesDuringUpgradeAll_Flag_To_Arguments_If_Set(bool excludeChocolateyPackagesDuringUpgradeAll, string expected)
            {
                // Given
                var fixture = new ChocolateyUpgraderFixture();
                fixture.Settings.ExcludeChocolateyPackagesDuringUpgradeAll = excludeChocolateyPackagesDuringUpgradeAll;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "upgrade \"Cake\" --confirm --include-chocolatey-packages-during-upgrade-all")]
            [InlineData(false, "upgrade \"Cake\" --confirm")]
            public void Should_Add_IncludeChocolateyPackagesDuringUpgradeAll_Flag_To_Arguments_If_Set(bool includeChocolateyPackagesDuringUpgradeAll, string expected)
            {
                // Given
                var fixture = new ChocolateyUpgraderFixture();
                fixture.Settings.IncludeChocolateyPackagesDuringUpgradeAll = includeChocolateyPackagesDuringUpgradeAll;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData("Just because", "upgrade \"Cake\" --confirm --pin-reason=\"Just because\"")]
            [InlineData(null, "upgrade \"Cake\" --confirm")]
            public void Should_Add_PinReason_Flag_To_Arguments_If_Set(string pinReason, string expected)
            {
                // Given
                var fixture = new ChocolateyUpgraderFixture();
                fixture.Settings.PinReason = pinReason;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }
        }
    }
}