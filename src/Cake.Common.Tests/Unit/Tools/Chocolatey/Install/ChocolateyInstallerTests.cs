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
                Assert.Equal("install \"Cake\" --confirm", result.Args);
            }

            [Theory]
            [InlineData(true, "install \"Cake\" --debug --confirm")]
            [InlineData(false, "install \"Cake\" --confirm")]
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
            [InlineData(true, "install \"Cake\" --verbose --confirm")]
            [InlineData(false, "install \"Cake\" --confirm")]
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
            [InlineData(true, "install \"Cake\" --trace --confirm")]
            [InlineData(false, "install \"Cake\" --confirm")]
            public void Should_Add_Trace_Flag_To_Arguments_If_Set(bool trace, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallFixture();
                fixture.Settings.Trace = trace;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "install \"Cake\" --no-color --confirm")]
            [InlineData(false, "install \"Cake\" --confirm")]
            public void Should_Add_NoColor_Flag_To_Arguments_If_Set(bool noColor, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallFixture();
                fixture.Settings.NoColor = noColor;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "install \"Cake\" --accept-license --confirm")]
            [InlineData(false, "install \"Cake\" --confirm")]
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
            [InlineData(true, "install \"Cake\" --confirm --force")]
            [InlineData(false, "install \"Cake\" --confirm")]
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
            [InlineData(true, "install \"Cake\" --confirm --what-if")]
            [InlineData(false, "install \"Cake\" --confirm")]
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
            [InlineData(true, "install \"Cake\" --confirm --limit-output")]
            [InlineData(false, "install \"Cake\" --confirm")]
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
            [InlineData(5, "install \"Cake\" --confirm --execution-timeout=\"5\"")]
            [InlineData(0, "install \"Cake\" --confirm")]
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
            [InlineData(@"c:\temp", "install \"Cake\" --confirm --cache-location=\"c:\\temp\"")]
            [InlineData("", "install \"Cake\" --confirm")]
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
            [InlineData(true, "install \"Cake\" --confirm --allow-unofficial")]
            [InlineData(false, "install \"Cake\" --confirm")]
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

            [Theory]
            [InlineData(true, "install \"Cake\" --confirm --fail-on-error-output")]
            [InlineData(false, "install \"Cake\" --confirm")]
            public void Should_Add_FailOnErrorOutput_Flag_To_Arguments_If_Set(bool failOnErrorOutput, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallFixture();
                fixture.Settings.FailOnErrorOutput = failOnErrorOutput;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "install \"Cake\" --confirm --use-system-powershell")]
            [InlineData(false, "install \"Cake\" --confirm")]
            public void Should_Add_UseSystemPowerShell_Flag_To_Arguments_If_Set(bool useSystemPowerShell, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallFixture();
                fixture.Settings.UseSystemPowerShell = useSystemPowerShell;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "install \"Cake\" --confirm --no-progress")]
            [InlineData(false, "install \"Cake\" --confirm")]
            public void Should_Add_NoProgress_Flag_To_Arguments_If_Set(bool noProgress, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallFixture();
                fixture.Settings.NoProgress = noProgress;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData("proxy", "install \"Cake\" --confirm --proxy=\"proxy\"")]
            [InlineData(null, "install \"Cake\" --confirm")]
            public void Should_Add_Proxy_Flag_To_Arguments_If_Set(string proxy, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallFixture();
                fixture.Settings.Proxy = proxy;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData("proxy-user", "install \"Cake\" --confirm --proxy-user=\"proxy-user\"")]
            [InlineData(null, "install \"Cake\" --confirm")]
            public void Should_Add_ProxyUser_Flag_To_Arguments_If_Set(string proxyUser, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallFixture();
                fixture.Settings.ProxyUser = proxyUser;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData("proxy-password", "install \"Cake\" --confirm --proxy-password=\"proxy-password\"")]
            [InlineData(null, "install \"Cake\" --confirm")]
            public void Should_Add_ProxyPassword_Flag_To_Arguments_If_Set(string proxyPassword, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallFixture();
                fixture.Settings.ProxyPassword = proxyPassword;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData("proxy1,proxy2", "install \"Cake\" --confirm --proxy-bypass-list=\"proxy1,proxy2\"")]
            [InlineData(null, "install \"Cake\" --confirm")]
            public void Should_Add_ProxyByPassList_Flag_To_Arguments_If_Set(string proxyByPassList, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallFixture();
                fixture.Settings.ProxyByPassList = proxyByPassList;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "install \"Cake\" --confirm --proxy-bypass-on-local")]
            [InlineData(false, "install \"Cake\" --confirm")]
            public void Should_Add_ProxyByPassOnLocal_Flag_To_Arguments_If_Set(bool proxyBypassOnLocal, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallFixture();
                fixture.Settings.ProxyBypassOnLocal = proxyBypassOnLocal;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData("./output.log", "install \"Cake\" --confirm --log-file=\"/Working/output.log\"")]
            [InlineData(null, "install \"Cake\" --confirm")]
            public void Should_Add_Log_File_Flag_To_Arguments_If_Set(string logFilePath, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallFixture();
                fixture.Settings.LogFile = logFilePath;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "install \"Cake\" --confirm --skip-compatibility-checks")]
            [InlineData(false, "install \"Cake\" --confirm")]
            public void Should_Add_Skip_Compatibility_Flag_To_Arguments_If_Set(bool skipCompatibiity, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallFixture();
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
                var fixture = new ChocolateyInstallFixture();
                fixture.Settings.Source = "A";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("install \"Cake\" --confirm --source=\"A\"", result.Args);
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
                Assert.Equal("install \"Cake\" --confirm --version=\"1.0.0\"", result.Args);
            }

            [Theory]
            [InlineData(true, "install \"Cake\" --confirm --pre")]
            [InlineData(false, "install \"Cake\" --confirm")]
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
            [InlineData(true, "install \"Cake\" --confirm --forcex86")]
            [InlineData(false, "install \"Cake\" --confirm")]
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
            [InlineData("args1", "install \"Cake\" --confirm --install-arguments=\"args1\"")]
            [InlineData("", "install \"Cake\" --confirm")]
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
            [InlineData(true, "install \"Cake\" --confirm --override-arguments")]
            [InlineData(false, "install \"Cake\" --confirm")]
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
            [InlineData(true, "install \"Cake\" --confirm --not-silent")]
            [InlineData(false, "install \"Cake\" --confirm")]
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
            [InlineData("param1", "install \"Cake\" --confirm --package-parameters=\"param1\"")]
            [InlineData("", "install \"Cake\" --confirm")]
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
            [InlineData(true, "install \"Cake\" --confirm --apply-install-arguments-to-dependencies")]
            [InlineData(false, "install \"Cake\" --confirm")]
            public void Should_Add_ApplyInstallArgumentsToDependencies_To_Arguments_If_Set(bool applyInstallArgumentsToDependencies, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallFixture();
                fixture.Settings.ApplyInstallArgumentsToDependencies = applyInstallArgumentsToDependencies;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "install \"Cake\" --confirm --apply-package-parameters-to-dependencies")]
            [InlineData(false, "install \"Cake\" --confirm")]
            public void Should_Add_ApplyPackageParametersToDependencies_To_Arguments_If_Set(bool applyPackageParametersToDependencies, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallFixture();
                fixture.Settings.ApplyPackageParametersToDependencies = applyPackageParametersToDependencies;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "install \"Cake\" --confirm --allow-downgrade")]
            [InlineData(false, "install \"Cake\" --confirm")]
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
            [InlineData(true, "install \"Cake\" --confirm --side-by-side")]
            [InlineData(false, "install \"Cake\" --confirm")]
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
            [InlineData(true, "install \"Cake\" --confirm --ignore-dependencies")]
            [InlineData(false, "install \"Cake\" --confirm")]
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
            [InlineData(true, "install \"Cake\" --confirm --force-dependencies")]
            [InlineData(false, "install \"Cake\" --confirm")]
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
            [InlineData(true, "install \"Cake\" --confirm --skip-automation-scripts")]
            [InlineData(false, "install \"Cake\" --confirm")]
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
            [InlineData("user1", "install \"Cake\" --confirm --user=\"user1\"")]
            [InlineData("", "install \"Cake\" --confirm")]
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
            [InlineData("password1", "install \"Cake\" --confirm --password=\"password1\"")]
            [InlineData("", "install \"Cake\" --confirm")]
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
            [InlineData("./mycert.pfx", "install \"Cake\" --confirm --cert=\"/Working/mycert.pfx\"")]
            [InlineData(null, "install \"Cake\" --confirm")]
            public void Should_Add_Cert_To_Arguments_If_Set(string certificate, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallFixture();
                fixture.Settings.Certificate = certificate;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData("certpassword", "install \"Cake\" --confirm --certpassword=\"certpassword\"")]
            [InlineData("", "install \"Cake\" --confirm")]
            public void Should_Add_CertPassword_To_Arguments_If_Set(string certificatePassword, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallFixture();
                fixture.Settings.CertificatePassword = certificatePassword;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "install \"Cake\" --confirm --ignore-checksums")]
            [InlineData(false, "install \"Cake\" --confirm")]
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

            [Theory]
            [InlineData(true, "install \"Cake\" --confirm --allow-empty-checksums")]
            [InlineData(false, "install \"Cake\" --confirm")]
            public void Should_Add_AllowEmptyChecksums_Flag_To_Arguments_If_Set(bool allowEmptyChecksums, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallFixture();
                fixture.Settings.AllowEmptyChecksums = allowEmptyChecksums;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "install \"Cake\" --confirm --allow-empty-checksums-secure")]
            [InlineData(false, "install \"Cake\" --confirm")]
            public void Should_Add_AllowEmptyChecksumsSecure_Flag_To_Arguments_If_Set(bool allowEmptyChecksumsSecure, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallFixture();
                fixture.Settings.AllowEmptyChecksumsSecure = allowEmptyChecksumsSecure;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "install \"Cake\" --confirm --require-checksums")]
            [InlineData(false, "install \"Cake\" --confirm")]
            public void Should_Add_RequireChecksums_Flag_To_Arguments_If_Set(bool requireChecksums, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallFixture();
                fixture.Settings.RequireChecksums = requireChecksums;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData("abcdef", "install \"Cake\" --confirm --download-checksum=\"abcdef\"")]
            [InlineData(null, "install \"Cake\" --confirm")]
            public void Should_Add_Checksum_Flag_To_Arguments_If_Set(string checksum, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallFixture();
                fixture.Settings.Checksum = checksum;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData("abcdef", "install \"Cake\" --confirm --download-checksum-x64=\"abcdef\"")]
            [InlineData(null, "install \"Cake\" --confirm")]
            public void Should_Add_Checksum64_Flag_To_Arguments_If_Set(string checksum64, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallFixture();
                fixture.Settings.Checksum64 = checksum64;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData("md5", "install \"Cake\" --confirm --download-checksum-type=\"md5\"")]
            [InlineData(null, "install \"Cake\" --confirm")]
            public void Should_Add_ChecksumType_Flag_To_Arguments_If_Set(string checkSumType, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallFixture();
                fixture.Settings.ChecksumType = checkSumType;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData("md5", "install \"Cake\" --confirm --download-checksum-type-x64=\"md5\"")]
            [InlineData(null, "install \"Cake\" --confirm")]
            public void Should_Add_ChecksumType64_Flag_To_Arguments_If_Set(string checkSumType64, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallFixture();
                fixture.Settings.ChecksumType64 = checkSumType64;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "install \"Cake\" --confirm --ignore-package-exit-codes")]
            [InlineData(false, "install \"Cake\" --confirm")]
            public void Should_Add_IgnorePackageExitCodes_Flag_To_Arguments_If_Set(bool ignorePackageExitCodes, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallFixture();
                fixture.Settings.IgnorePackageExitCodes = ignorePackageExitCodes;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "install \"Cake\" --confirm --use-package-exit-codes")]
            [InlineData(false, "install \"Cake\" --confirm")]
            public void Should_Add_UsePackageExitCodes_Flag_To_Arguments_If_Set(bool usePackageExitCodes, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallFixture();
                fixture.Settings.UsePackageExitCodes = usePackageExitCodes;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "install \"Cake\" --confirm --stop-on-first-package-failure")]
            [InlineData(false, "install \"Cake\" --confirm")]
            public void Should_Add_StopOnFirstFailure_Flag_To_Arguments_If_Set(bool stopOnFirstFailure, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallFixture();
                fixture.Settings.StopOnFirstFailure = stopOnFirstFailure;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "install \"Cake\" --confirm --exit-when-reboot-detected")]
            [InlineData(false, "install \"Cake\" --confirm")]
            public void Should_Add_ExitWhenRebootDetected_Flag_To_Arguments_If_Set(bool exitWhenRebootDetected, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallFixture();
                fixture.Settings.ExitWhenRebootDetected = exitWhenRebootDetected;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "install \"Cake\" --confirm --ignore-detected-reboot")]
            [InlineData(false, "install \"Cake\" --confirm")]
            public void Should_Add_IgnoreDetectedReboot_Flag_To_Arguments_If_Set(bool ignoreDetectedReboot, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallFixture();
                fixture.Settings.IgnoreDetectedReboot = ignoreDetectedReboot;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "install \"Cake\" --confirm --disable-repository-optimizations")]
            [InlineData(false, "install \"Cake\" --confirm")]
            public void Should_Add_DisableRepositoryOptimizations_Flag_To_Arguments_If_Set(bool disableRepositoryOptimizations, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallFixture();
                fixture.Settings.DisableRepositoryOptimizations = disableRepositoryOptimizations;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "install \"Cake\" --confirm --pin-package")]
            [InlineData(false, "install \"Cake\" --confirm")]
            public void Should_Add_Pin_Flag_To_Arguments_If_Set(bool pin, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallFixture();
                fixture.Settings.Pin = pin;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "install \"Cake\" --confirm --skip-hooks")]
            [InlineData(false, "install \"Cake\" --confirm")]
            public void Should_Add_SkipHooks_Flag_To_Arguments_If_Set(bool skipHooks, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallFixture();
                fixture.Settings.SkipHooks = skipHooks;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "install \"Cake\" --confirm --skip-download-cache")]
            [InlineData(false, "install \"Cake\" --confirm")]
            public void Should_Add_SkipDownloadCache_Flag_To_Arguments_If_Set(bool skipDownloadCache, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallFixture();
                fixture.Settings.SkipDownloadCache = skipDownloadCache;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "install \"Cake\" --confirm --use-download-cache")]
            [InlineData(false, "install \"Cake\" --confirm")]
            public void Should_Add_UseDownloadCache_Flag_To_Arguments_If_Set(bool useDownloadCache, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallFixture();
                fixture.Settings.UseDownloadCache = useDownloadCache;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "install \"Cake\" --confirm --skip-virus-check")]
            [InlineData(false, "install \"Cake\" --confirm")]
            public void Should_Add_SkipVirusCheck_Flag_To_Arguments_If_Set(bool skipVirusCheck, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallFixture();
                fixture.Settings.SkipVirusCheck = skipVirusCheck;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "install \"Cake\" --confirm --virus-check")]
            [InlineData(false, "install \"Cake\" --confirm")]
            public void Should_Add_VirusCheck_Flag_To_Arguments_If_Set(bool virusCheck, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallFixture();
                fixture.Settings.VirusCheck = virusCheck;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(5, "install \"Cake\" --confirm --virus-positives-minimum=\"5\"")]
            [InlineData(0, "install \"Cake\" --confirm")]
            public void Should_Add_VirusPositivesMinimum_To_Arguments_If_Set(int virusPositivesMinimum, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallFixture();
                fixture.Settings.VirusPositivesMinimum = virusPositivesMinimum;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData("super-secret", "install \"Cake\" --confirm --install-arguments-sensitive=\"super-secret\"")]
            [InlineData(null, "install \"Cake\" --confirm")]
            public void Should_Add_InstallArgumentsSensitive_Flag_To_Arguments_If_Set(string installArgumentsSensitive, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallFixture();
                fixture.Settings.InstallArgumentsSensitive = installArgumentsSensitive;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData("super-secret", "install \"Cake\" --confirm --package-parameters-sensitive=\"super-secret\"")]
            [InlineData(null, "install \"Cake\" --confirm")]
            public void Should_Add_PackageParametersSensitive_Flag_To_Arguments_If_Set(string packageParametersSensitive, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallFixture();
                fixture.Settings.PackageParametersSensitive = packageParametersSensitive;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData("./install", "install \"Cake\" --confirm --install-directory=\"/Working/install\"")]
            [InlineData(null, "install \"Cake\" --confirm")]
            public void Should_Add_InstallDirectory_Flag_To_Arguments_If_Set(string installDirectory, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallFixture();
                fixture.Settings.InstallDirectory = installDirectory;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(5, "install \"Cake\" --confirm --maximum-download-bits-per-second=\"5\"")]
            [InlineData(0, "install \"Cake\" --confirm")]
            public void Should_Add_MaximumDownloadBitsPerSecond_Flag_To_Arguments_If_Set(int maximumDownloadBitsPerSecond, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallFixture();
                fixture.Settings.MaximumDownloadBitsPerSecond = maximumDownloadBitsPerSecond;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "install \"Cake\" --confirm --reduce-package-size")]
            [InlineData(false, "install \"Cake\" --confirm")]
            public void Should_Add_ReducePackageSize_Flag_To_Arguments_If_Set(bool reducePackageSize, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallFixture();
                fixture.Settings.ReducePackageSize = reducePackageSize;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "install \"Cake\" --confirm --no-reduce-package-size")]
            [InlineData(false, "install \"Cake\" --confirm")]
            public void Should_Add_NoReducePackageSize_Flag_To_Arguments_If_Set(bool noReducePackageSize, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallFixture();
                fixture.Settings.NoReducePackageSize = noReducePackageSize;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "install \"Cake\" --confirm --reduce-nupkg-only")]
            [InlineData(false, "install \"Cake\" --confirm")]
            public void Should_Add_ReduceNupkgOnly_Flag_To_Arguments_If_Set(bool reduceNupkgOnly, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallFixture();
                fixture.Settings.ReduceNupkgOnly = reduceNupkgOnly;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData("Just because", "install \"Cake\" --confirm --pin-reason=\"Just because\"")]
            [InlineData(null, "install \"Cake\" --confirm")]
            public void Should_Add_PinReason_Flag_To_Arguments_If_Set(string pinReason, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallFixture();
                fixture.Settings.PinReason = pinReason;

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
                Assert.Equal("install \"/Working/packages.config\" --confirm", result.Args);
            }

            [Theory]
            [InlineData(true, "install \"/Working/packages.config\" --debug --confirm")]
            [InlineData(false, "install \"/Working/packages.config\" --confirm")]
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
            [InlineData(true, "install \"/Working/packages.config\" --verbose --confirm")]
            [InlineData(false, "install \"/Working/packages.config\" --confirm")]
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
            [InlineData(true, "install \"/Working/packages.config\" --trace --confirm")]
            [InlineData(false, "install \"/Working/packages.config\" --confirm")]
            public void Should_Add_Trace_Flag_To_Arguments_If_Set(bool trace, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallFromConfigFixture();
                fixture.Settings.Trace = trace;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "install \"/Working/packages.config\" --no-color --confirm")]
            [InlineData(false, "install \"/Working/packages.config\" --confirm")]
            public void Should_Add_NoColor_Flag_To_Arguments_If_Set(bool noColor, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallFromConfigFixture();
                fixture.Settings.NoColor = noColor;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "install \"/Working/packages.config\" --accept-license --confirm")]
            [InlineData(false, "install \"/Working/packages.config\" --confirm")]
            public void Should_Add_AcceptLicense_Flag_To_Arguments_If_Set(bool acceptLicense, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallFromConfigFixture();
                fixture.Settings.AcceptLicense = acceptLicense;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "install \"/Working/packages.config\" --confirm --force")]
            [InlineData(false, "install \"/Working/packages.config\" --confirm")]
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
            [InlineData(true, "install \"/Working/packages.config\" --confirm --what-if")]
            [InlineData(false, "install \"/Working/packages.config\" --confirm")]
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
            [InlineData(true, "install \"/Working/packages.config\" --confirm --limit-output")]
            [InlineData(false, "install \"/Working/packages.config\" --confirm")]
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
            [InlineData(5, "install \"/Working/packages.config\" --confirm --execution-timeout=\"5\"")]
            [InlineData(0, "install \"/Working/packages.config\" --confirm")]
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
            [InlineData(@"c:\temp", "install \"/Working/packages.config\" --confirm --cache-location=\"c:\\temp\"")]
            [InlineData("", "install \"/Working/packages.config\" --confirm")]
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
            [InlineData(true, "install \"/Working/packages.config\" --confirm --allow-unofficial")]
            [InlineData(false, "install \"/Working/packages.config\" --confirm")]
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

            [Theory]
            [InlineData(true, "install \"/Working/packages.config\" --confirm --fail-on-error-output")]
            [InlineData(false, "install \"/Working/packages.config\" --confirm")]
            public void Should_Add_FailOnErrorOutput_Flag_To_Arguments_If_Set(bool failOnErrorOutput, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallFromConfigFixture();
                fixture.Settings.FailOnErrorOutput = failOnErrorOutput;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "install \"/Working/packages.config\" --confirm --use-system-powershell")]
            [InlineData(false, "install \"/Working/packages.config\" --confirm")]
            public void Should_Add_UseSystemPowerShell_Flag_To_Arguments_If_Set(bool useSystemPowerShell, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallFromConfigFixture();
                fixture.Settings.UseSystemPowerShell = useSystemPowerShell;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "install \"/Working/packages.config\" --confirm --no-progress")]
            [InlineData(false, "install \"/Working/packages.config\" --confirm")]
            public void Should_Add_NoProgress_Flag_To_Arguments_If_Set(bool noProgress, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallFromConfigFixture();
                fixture.Settings.NoProgress = noProgress;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData("proxy", "install \"/Working/packages.config\" --confirm --proxy=\"proxy\"")]
            [InlineData(null, "install \"/Working/packages.config\" --confirm")]
            public void Should_Add_Proxy_Flag_To_Arguments_If_Set(string proxy, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallFromConfigFixture();
                fixture.Settings.Proxy = proxy;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData("proxy-user", "install \"/Working/packages.config\" --confirm --proxy-user=\"proxy-user\"")]
            [InlineData(null, "install \"/Working/packages.config\" --confirm")]
            public void Should_Add_ProxyUser_Flag_To_Arguments_If_Set(string proxyUser, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallFromConfigFixture();
                fixture.Settings.ProxyUser = proxyUser;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData("proxy-password", "install \"/Working/packages.config\" --confirm --proxy-password=\"proxy-password\"")]
            [InlineData(null, "install \"/Working/packages.config\" --confirm")]
            public void Should_Add_ProxyPassword_Flag_To_Arguments_If_Set(string proxyPassword, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallFromConfigFixture();
                fixture.Settings.ProxyPassword = proxyPassword;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData("proxy1,proxy2", "install \"/Working/packages.config\" --confirm --proxy-bypass-list=\"proxy1,proxy2\"")]
            [InlineData(null, "install \"/Working/packages.config\" --confirm")]
            public void Should_Add_ProxyByPassList_Flag_To_Arguments_If_Set(string proxyByPassList, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallFromConfigFixture();
                fixture.Settings.ProxyByPassList = proxyByPassList;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "install \"/Working/packages.config\" --confirm --proxy-bypass-on-local")]
            [InlineData(false, "install \"/Working/packages.config\" --confirm")]
            public void Should_Add_ProxyByPassOnLocal_Flag_To_Arguments_If_Set(bool proxyBypassOnLocal, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallFromConfigFixture();
                fixture.Settings.ProxyBypassOnLocal = proxyBypassOnLocal;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData("./output.log", "install \"/Working/packages.config\" --confirm --log-file=\"/Working/output.log\"")]
            [InlineData(null, "install \"/Working/packages.config\" --confirm")]
            public void Should_Add_Log_File_Flag_To_Arguments_If_Set(string logFilePath, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallFromConfigFixture();
                fixture.Settings.LogFile = logFilePath;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "install \"/Working/packages.config\" --confirm --skip-compatibility-checks")]
            [InlineData(false, "install \"/Working/packages.config\" --confirm")]
            public void Should_Add_Skip_Compatibility_Flag_To_Arguments_If_Set(bool skipCompatibiity, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallFromConfigFixture();
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
                var fixture = new ChocolateyInstallFromConfigFixture();
                fixture.Settings.Source = "A";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("install \"/Working/packages.config\" --confirm --source=\"A\"", result.Args);
            }

            [Theory]
            [InlineData(true, "install \"/Working/packages.config\" --confirm --pre")]
            [InlineData(false, "install \"/Working/packages.config\" --confirm")]
            public void Should_Add_Prerelease_Flag_To_Arguments_If_Set(bool prerelease, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallFromConfigFixture();
                fixture.Settings.Prerelease = prerelease;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "install \"/Working/packages.config\" --confirm --forcex86")]
            [InlineData(false, "install \"/Working/packages.config\" --confirm")]
            public void Should_Add_Forcex86_Flag_To_Arguments_If_Set(bool forcex86, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallFromConfigFixture();
                fixture.Settings.Forcex86 = forcex86;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData("args1", "install \"/Working/packages.config\" --confirm --install-arguments=\"args1\"")]
            [InlineData("", "install \"/Working/packages.config\" --confirm")]
            public void Should_Add_InstallArguments_To_Arguments_If_Set(string installArgs, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallFromConfigFixture();
                fixture.Settings.InstallArguments = installArgs;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "install \"/Working/packages.config\" --confirm --override-arguments")]
            [InlineData(false, "install \"/Working/packages.config\" --confirm")]
            public void Should_Add_OverrideArguments_Flag_To_Arguments_If_Set(bool overrideArguments, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallFromConfigFixture();
                fixture.Settings.OverrideArguments = overrideArguments;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "install \"/Working/packages.config\" --confirm --not-silent")]
            [InlineData(false, "install \"/Working/packages.config\" --confirm")]
            public void Should_Add_NotSilent_Flag_To_Arguments_If_Set(bool notSilent, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallFromConfigFixture();
                fixture.Settings.NotSilent = notSilent;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData("param1", "install \"/Working/packages.config\" --confirm --package-parameters=\"param1\"")]
            [InlineData("", "install \"/Working/packages.config\" --confirm")]
            public void Should_Add_PackageParameters_To_Arguments_If_Set(string packageParameters, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallFromConfigFixture();
                fixture.Settings.PackageParameters = packageParameters;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "install \"/Working/packages.config\" --confirm --apply-install-arguments-to-dependencies")]
            [InlineData(false, "install \"/Working/packages.config\" --confirm")]
            public void Should_Add_ApplyInstallArgumentsToDependencies_To_Arguments_If_Set(bool applyInstallArgumentsToDependencies, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallFromConfigFixture();
                fixture.Settings.ApplyInstallArgumentsToDependencies = applyInstallArgumentsToDependencies;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "install \"/Working/packages.config\" --confirm --apply-package-parameters-to-dependencies")]
            [InlineData(false, "install \"/Working/packages.config\" --confirm")]
            public void Should_Add_ApplyPackageParametersToDependencies_To_Arguments_If_Set(bool applyPackageParametersToDependencies, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallFromConfigFixture();
                fixture.Settings.ApplyPackageParametersToDependencies = applyPackageParametersToDependencies;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "install \"/Working/packages.config\" --confirm --allow-downgrade")]
            [InlineData(false, "install \"/Working/packages.config\" --confirm")]
            public void Should_Add_AllowDowngrade_Flag_To_Arguments_If_Set(bool allowDowngrade, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallFromConfigFixture();
                fixture.Settings.AllowDowngrade = allowDowngrade;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "install \"/Working/packages.config\" --confirm --side-by-side")]
            [InlineData(false, "install \"/Working/packages.config\" --confirm")]
            public void Should_Add_SideBySide_Flag_To_Arguments_If_Set(bool sideBySide, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallFromConfigFixture();
                fixture.Settings.SideBySide = sideBySide;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "install \"/Working/packages.config\" --confirm --ignore-dependencies")]
            [InlineData(false, "install \"/Working/packages.config\" --confirm")]
            public void Should_Add_IgnoreDependencies_Flag_To_Arguments_If_Set(bool ignoreDependencies, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallFromConfigFixture();
                fixture.Settings.IgnoreDependencies = ignoreDependencies;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "install \"/Working/packages.config\" --confirm --force-dependencies")]
            [InlineData(false, "install \"/Working/packages.config\" --confirm")]
            public void Should_Add_ForceDependencies_Flag_To_Arguments_If_Set(bool forceDependencies, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallFromConfigFixture();
                fixture.Settings.ForceDependencies = forceDependencies;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "install \"/Working/packages.config\" --confirm --skip-automation-scripts")]
            [InlineData(false, "install \"/Working/packages.config\" --confirm")]
            public void Should_Add_SkipPowerShell_Flag_To_Arguments_If_Set(bool skipPowerShell, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallFromConfigFixture();
                fixture.Settings.SkipPowerShell = skipPowerShell;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData("user1", "install \"/Working/packages.config\" --confirm --user=\"user1\"")]
            [InlineData("", "install \"/Working/packages.config\" --confirm")]
            public void Should_Add_User_To_Arguments_If_Set(string user, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallFromConfigFixture();
                fixture.Settings.User = user;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData("password1", "install \"/Working/packages.config\" --confirm --password=\"password1\"")]
            [InlineData("", "install \"/Working/packages.config\" --confirm")]
            public void Should_Add_Password_To_Arguments_If_Set(string password, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallFromConfigFixture();
                fixture.Settings.Password = password;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData("./mycert.pfx", "install \"/Working/packages.config\" --confirm --cert=\"/Working/mycert.pfx\"")]
            [InlineData(null, "install \"/Working/packages.config\" --confirm")]
            public void Should_Add_Cert_To_Arguments_If_Set(string certificate, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallFromConfigFixture();
                fixture.Settings.Certificate = certificate;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData("certpassword", "install \"/Working/packages.config\" --confirm --certpassword=\"certpassword\"")]
            [InlineData("", "install \"/Working/packages.config\" --confirm")]
            public void Should_Add_CertPassword_To_Arguments_If_Set(string certificatePassword, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallFromConfigFixture();
                fixture.Settings.CertificatePassword = certificatePassword;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "install \"/Working/packages.config\" --confirm --ignore-checksums")]
            [InlineData(false, "install \"/Working/packages.config\" --confirm")]
            public void Should_Add_IgnoreChecksums_Flag_To_Arguments_If_Set(bool ignoreChecksums, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallFromConfigFixture();
                fixture.Settings.IgnoreChecksums = ignoreChecksums;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "install \"/Working/packages.config\" --confirm --allow-empty-checksums")]
            [InlineData(false, "install \"/Working/packages.config\" --confirm")]
            public void Should_Add_AllowEmptyChecksums_Flag_To_Arguments_If_Set(bool allowEmptyChecksums, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallFromConfigFixture();
                fixture.Settings.AllowEmptyChecksums = allowEmptyChecksums;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "install \"/Working/packages.config\" --confirm --allow-empty-checksums-secure")]
            [InlineData(false, "install \"/Working/packages.config\" --confirm")]
            public void Should_Add_AllowEmptyChecksumsSecure_Flag_To_Arguments_If_Set(bool allowEmptyChecksumsSecure, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallFromConfigFixture();
                fixture.Settings.AllowEmptyChecksumsSecure = allowEmptyChecksumsSecure;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "install \"/Working/packages.config\" --confirm --require-checksums")]
            [InlineData(false, "install \"/Working/packages.config\" --confirm")]
            public void Should_Add_RequireChecksums_Flag_To_Arguments_If_Set(bool requireChecksums, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallFromConfigFixture();
                fixture.Settings.RequireChecksums = requireChecksums;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData("abcdef", "install \"/Working/packages.config\" --confirm --download-checksum=\"abcdef\"")]
            [InlineData(null, "install \"/Working/packages.config\" --confirm")]
            public void Should_Add_Checksum_Flag_To_Arguments_If_Set(string checksum, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallFromConfigFixture();
                fixture.Settings.Checksum = checksum;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData("abcdef", "install \"/Working/packages.config\" --confirm --download-checksum-x64=\"abcdef\"")]
            [InlineData(null, "install \"/Working/packages.config\" --confirm")]
            public void Should_Add_Checksum64_Flag_To_Arguments_If_Set(string checksum64, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallFromConfigFixture();
                fixture.Settings.Checksum64 = checksum64;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData("md5", "install \"/Working/packages.config\" --confirm --download-checksum-type=\"md5\"")]
            [InlineData(null, "install \"/Working/packages.config\" --confirm")]
            public void Should_Add_ChecksumType_Flag_To_Arguments_If_Set(string checkSumType, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallFromConfigFixture();
                fixture.Settings.ChecksumType = checkSumType;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData("md5", "install \"/Working/packages.config\" --confirm --download-checksum-type-x64=\"md5\"")]
            [InlineData(null, "install \"/Working/packages.config\" --confirm")]
            public void Should_Add_ChecksumType64_Flag_To_Arguments_If_Set(string checkSumType64, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallFromConfigFixture();
                fixture.Settings.ChecksumType64 = checkSumType64;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "install \"/Working/packages.config\" --confirm --ignore-package-exit-codes")]
            [InlineData(false, "install \"/Working/packages.config\" --confirm")]
            public void Should_Add_IgnorePackageExitCodes_Flag_To_Arguments_If_Set(bool ignorePackageExitCodes, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallFromConfigFixture();
                fixture.Settings.IgnorePackageExitCodes = ignorePackageExitCodes;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "install \"/Working/packages.config\" --confirm --use-package-exit-codes")]
            [InlineData(false, "install \"/Working/packages.config\" --confirm")]
            public void Should_Add_UsePackageExitCodes_Flag_To_Arguments_If_Set(bool usePackageExitCodes, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallFromConfigFixture();
                fixture.Settings.UsePackageExitCodes = usePackageExitCodes;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "install \"/Working/packages.config\" --confirm --stop-on-first-package-failure")]
            [InlineData(false, "install \"/Working/packages.config\" --confirm")]
            public void Should_Add_StopOnFirstFailure_Flag_To_Arguments_If_Set(bool stopOnFirstFailure, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallFromConfigFixture();
                fixture.Settings.StopOnFirstFailure = stopOnFirstFailure;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "install \"/Working/packages.config\" --confirm --exit-when-reboot-detected")]
            [InlineData(false, "install \"/Working/packages.config\" --confirm")]
            public void Should_Add_ExitWhenRebootDetected_Flag_To_Arguments_If_Set(bool exitWhenRebootDetected, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallFromConfigFixture();
                fixture.Settings.ExitWhenRebootDetected = exitWhenRebootDetected;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "install \"/Working/packages.config\" --confirm --ignore-detected-reboot")]
            [InlineData(false, "install \"/Working/packages.config\" --confirm")]
            public void Should_Add_IgnoreDetectedReboot_Flag_To_Arguments_If_Set(bool ignoreDetectedReboot, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallFromConfigFixture();
                fixture.Settings.IgnoreDetectedReboot = ignoreDetectedReboot;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "install \"/Working/packages.config\" --confirm --disable-repository-optimizations")]
            [InlineData(false, "install \"/Working/packages.config\" --confirm")]
            public void Should_Add_DisableRepositoryOptimizations_Flag_To_Arguments_If_Set(bool disableRepositoryOptimizations, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallFromConfigFixture();
                fixture.Settings.DisableRepositoryOptimizations = disableRepositoryOptimizations;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "install \"/Working/packages.config\" --confirm --pin-package")]
            [InlineData(false, "install \"/Working/packages.config\" --confirm")]
            public void Should_Add_Pin_Flag_To_Arguments_If_Set(bool pin, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallFromConfigFixture();
                fixture.Settings.Pin = pin;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "install \"/Working/packages.config\" --confirm --skip-hooks")]
            [InlineData(false, "install \"/Working/packages.config\" --confirm")]
            public void Should_Add_SkipHooks_Flag_To_Arguments_If_Set(bool skipHooks, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallFromConfigFixture();
                fixture.Settings.SkipHooks = skipHooks;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "install \"/Working/packages.config\" --confirm --skip-download-cache")]
            [InlineData(false, "install \"/Working/packages.config\" --confirm")]
            public void Should_Add_SkipDownloadCache_Flag_To_Arguments_If_Set(bool skipDownloadCache, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallFromConfigFixture();
                fixture.Settings.SkipDownloadCache = skipDownloadCache;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "install \"/Working/packages.config\" --confirm --use-download-cache")]
            [InlineData(false, "install \"/Working/packages.config\" --confirm")]
            public void Should_Add_UseDownloadCache_Flag_To_Arguments_If_Set(bool useDownloadCache, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallFromConfigFixture();
                fixture.Settings.UseDownloadCache = useDownloadCache;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "install \"/Working/packages.config\" --confirm --skip-virus-check")]
            [InlineData(false, "install \"/Working/packages.config\" --confirm")]
            public void Should_Add_SkipVirusCheck_Flag_To_Arguments_If_Set(bool skipVirusCheck, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallFromConfigFixture();
                fixture.Settings.SkipVirusCheck = skipVirusCheck;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "install \"/Working/packages.config\" --confirm --virus-check")]
            [InlineData(false, "install \"/Working/packages.config\" --confirm")]
            public void Should_Add_VirusCheck_Flag_To_Arguments_If_Set(bool virusCheck, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallFromConfigFixture();
                fixture.Settings.VirusCheck = virusCheck;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(5, "install \"/Working/packages.config\" --confirm --virus-positives-minimum=\"5\"")]
            [InlineData(0, "install \"/Working/packages.config\" --confirm")]
            public void Should_Add_VirusPositivesMinimum_To_Arguments_If_Set(int virusPositivesMinimum, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallFromConfigFixture();
                fixture.Settings.VirusPositivesMinimum = virusPositivesMinimum;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData("super-secret", "install \"/Working/packages.config\" --confirm --install-arguments-sensitive=\"super-secret\"")]
            [InlineData(null, "install \"/Working/packages.config\" --confirm")]
            public void Should_Add_InstallArgumentsSensitive_Flag_To_Arguments_If_Set(string installArgumentsSensitive, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallFromConfigFixture();
                fixture.Settings.InstallArgumentsSensitive = installArgumentsSensitive;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData("super-secret", "install \"/Working/packages.config\" --confirm --package-parameters-sensitive=\"super-secret\"")]
            [InlineData(null, "install \"/Working/packages.config\" --confirm")]
            public void Should_Add_PackageParametersSensitive_Flag_To_Arguments_If_Set(string packageParametersSensitive, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallFromConfigFixture();
                fixture.Settings.PackageParametersSensitive = packageParametersSensitive;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData("./install", "install \"/Working/packages.config\" --confirm --install-directory=\"/Working/install\"")]
            [InlineData(null, "install \"/Working/packages.config\" --confirm")]
            public void Should_Add_InstallDirectory_Flag_To_Arguments_If_Set(string installDirectory, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallFromConfigFixture();
                fixture.Settings.InstallDirectory = installDirectory;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(5, "install \"/Working/packages.config\" --confirm --maximum-download-bits-per-second=\"5\"")]
            [InlineData(0, "install \"/Working/packages.config\" --confirm")]
            public void Should_Add_MaximumDownloadBitsPerSecond_Flag_To_Arguments_If_Set(int maximumDownloadBitsPerSecond, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallFromConfigFixture();
                fixture.Settings.MaximumDownloadBitsPerSecond = maximumDownloadBitsPerSecond;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "install \"/Working/packages.config\" --confirm --reduce-package-size")]
            [InlineData(false, "install \"/Working/packages.config\" --confirm")]
            public void Should_Add_ReducePackageSize_Flag_To_Arguments_If_Set(bool reducePackageSize, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallFromConfigFixture();
                fixture.Settings.ReducePackageSize = reducePackageSize;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "install \"/Working/packages.config\" --confirm --no-reduce-package-size")]
            [InlineData(false, "install \"/Working/packages.config\" --confirm")]
            public void Should_Add_NoReducePackageSize_Flag_To_Arguments_If_Set(bool noReducePackageSize, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallFromConfigFixture();
                fixture.Settings.NoReducePackageSize = noReducePackageSize;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "install \"/Working/packages.config\" --confirm --reduce-nupkg-only")]
            [InlineData(false, "install \"/Working/packages.config\" --confirm")]
            public void Should_Add_ReduceNupkgOnly_Flag_To_Arguments_If_Set(bool reduceNupkgOnly, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallFromConfigFixture();
                fixture.Settings.ReduceNupkgOnly = reduceNupkgOnly;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData("Just because", "install \"/Working/packages.config\" --confirm --pin-reason=\"Just because\"")]
            [InlineData(null, "install \"/Working/packages.config\" --confirm")]
            public void Should_Add_PinReason_Flag_To_Arguments_If_Set(string pinReason, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallFromConfigFixture();
                fixture.Settings.PinReason = pinReason;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }
        }
    }
}