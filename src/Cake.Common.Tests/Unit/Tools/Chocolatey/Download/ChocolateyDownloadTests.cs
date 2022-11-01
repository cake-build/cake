// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tests.Fixtures.Tools.Chocolatey.Download;
using Cake.Testing;
using Cake.Testing.Xunit;
using Xunit;

namespace Cake.Common.Tests.Unit.Tools.Chocolatey.Download
{
    public sealed class ChocolateyDownloadTests
    {
        public sealed class TheDownloadMethod
        {
            [Fact]
            public void Should_Throw_If_Package_Id_Is_Null()
            {
                // Given
                var fixture = new ChocolateyDownloadFixture();
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
                var fixture = new ChocolateyDownloadFixture();
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
                var fixture = new ChocolateyDownloadFixture();
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
                var fixture = new ChocolateyDownloadFixture();
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
                var fixture = new ChocolateyDownloadFixture();
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
                var fixture = new ChocolateyDownloadFixture();
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
                var fixture = new ChocolateyDownloadFixture();
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
                var fixture = new ChocolateyDownloadFixture();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/Working/tools/choco.exe", result.Path.FullPath);
            }

            [Fact]
            public void Should_Add_Mandatory_Arguments()
            {
                // Given
                var fixture = new ChocolateyDownloadFixture();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("download \"MyPackage\" --confirm", result.Args);
            }

            [Theory]
            [InlineData(true, "download \"MyPackage\" --debug --confirm")]
            [InlineData(false, "download \"MyPackage\" --confirm")]
            public void Should_Add_Debug_Flag_To_Arguments_If_Set(bool debug, string expected)
            {
                // Given
                var fixture = new ChocolateyDownloadFixture();
                fixture.Settings.Debug = debug;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "download \"MyPackage\" --verbose --confirm")]
            [InlineData(false, "download \"MyPackage\" --confirm")]
            public void Should_Add_Verbose_Flag_To_Arguments_If_Set(bool verbose, string expected)
            {
                // Given
                var fixture = new ChocolateyDownloadFixture();
                fixture.Settings.Verbose = verbose;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "download \"MyPackage\" --trace --confirm")]
            [InlineData(false, "download \"MyPackage\" --confirm")]
            public void Should_Add_Trace_Flag_To_Arguments_If_Set(bool trace, string expected)
            {
                // Given
                var fixture = new ChocolateyDownloadFixture();
                fixture.Settings.Trace = trace;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "download \"MyPackage\" --no-color --confirm")]
            [InlineData(false, "download \"MyPackage\" --confirm")]
            public void Should_Add_NoColor_Flag_To_Arguments_If_Set(bool noColor, string expected)
            {
                // Given
                var fixture = new ChocolateyDownloadFixture();
                fixture.Settings.NoColor = noColor;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "download \"MyPackage\" --accept-license --confirm")]
            [InlineData(false, "download \"MyPackage\" --confirm")]
            public void Should_Add_AcceptLicense_Flag_To_Arguments_If_Set(bool acceptLicense, string expected)
            {
                // Given
                var fixture = new ChocolateyDownloadFixture();
                fixture.Settings.AcceptLicense = acceptLicense;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "download \"MyPackage\" --confirm --force")]
            [InlineData(false, "download \"MyPackage\" --confirm")]
            public void Should_Add_Force_Flag_To_Arguments_If_Set(bool force, string expected)
            {
                // Given
                var fixture = new ChocolateyDownloadFixture();
                fixture.Settings.Force = force;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "download \"MyPackage\" --confirm --what-if")]
            [InlineData(false, "download \"MyPackage\" --confirm")]
            public void Should_Add_Noop_Flag_To_Arguments_If_Set(bool noop, string expected)
            {
                // Given
                var fixture = new ChocolateyDownloadFixture();
                fixture.Settings.Noop = noop;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "download \"MyPackage\" --confirm --limit-output")]
            [InlineData(false, "download \"MyPackage\" --confirm")]
            public void Should_Add_LimitOutput_Flag_To_Arguments_If_Set(bool limitOutput, string expected)
            {
                // Given
                var fixture = new ChocolateyDownloadFixture();
                fixture.Settings.LimitOutput = limitOutput;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(5, "download \"MyPackage\" --confirm --execution-timeout=\"5\"")]
            [InlineData(0, "download \"MyPackage\" --confirm")]
            public void Should_Add_ExecutionTimeout_To_Arguments_If_Set(int executionTimeout, string expected)
            {
                // Given
                var fixture = new ChocolateyDownloadFixture();
                fixture.Settings.ExecutionTimeout = executionTimeout;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(@"c:\temp", "download \"MyPackage\" --confirm --cache-location=\"c:\\temp\"")]
            [InlineData("", "download \"MyPackage\" --confirm")]
            public void Should_Add_CacheLocation_Flag_To_Arguments_If_Set(string cacheLocation, string expected)
            {
                // Given
                var fixture = new ChocolateyDownloadFixture();
                fixture.Settings.CacheLocation = cacheLocation;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "download \"MyPackage\" --confirm --allow-unofficial")]
            [InlineData(false, "download \"MyPackage\" --confirm")]
            public void Should_Add_AllowUnofficial_Flag_To_Arguments_If_Set(bool allowUnofficial, string expected)
            {
                // Given
                var fixture = new ChocolateyDownloadFixture();
                fixture.Settings.AllowUnofficial = allowUnofficial;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "download \"MyPackage\" --confirm --fail-on-error-output")]
            [InlineData(false, "download \"MyPackage\" --confirm")]
            public void Should_Add_FailOnErrorOutput_Flag_To_Arguments_If_Set(bool failOnErrorOutput, string expected)
            {
                // Given
                var fixture = new ChocolateyDownloadFixture();
                fixture.Settings.FailOnErrorOutput = failOnErrorOutput;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "download \"MyPackage\" --confirm --use-system-powershell")]
            [InlineData(false, "download \"MyPackage\" --confirm")]
            public void Should_Add_UseSystemPowerShell_Flag_To_Arguments_If_Set(bool useSystemPowerShell, string expected)
            {
                // Given
                var fixture = new ChocolateyDownloadFixture();
                fixture.Settings.UseSystemPowerShell = useSystemPowerShell;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "download \"MyPackage\" --confirm --no-progress")]
            [InlineData(false, "download \"MyPackage\" --confirm")]
            public void Should_Add_NoProgress_Flag_To_Arguments_If_Set(bool noProgress, string expected)
            {
                // Given
                var fixture = new ChocolateyDownloadFixture();
                fixture.Settings.NoProgress = noProgress;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData("proxy", "download \"MyPackage\" --confirm --proxy=\"proxy\"")]
            [InlineData(null, "download \"MyPackage\" --confirm")]
            public void Should_Add_Proxy_Flag_To_Arguments_If_Set(string proxy, string expected)
            {
                // Given
                var fixture = new ChocolateyDownloadFixture();
                fixture.Settings.Proxy = proxy;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData("proxy-user", "download \"MyPackage\" --confirm --proxy-user=\"proxy-user\"")]
            [InlineData(null, "download \"MyPackage\" --confirm")]
            public void Should_Add_ProxyUser_Flag_To_Arguments_If_Set(string proxyUser, string expected)
            {
                // Given
                var fixture = new ChocolateyDownloadFixture();
                fixture.Settings.ProxyUser = proxyUser;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData("proxy-password", "download \"MyPackage\" --confirm --proxy-password=\"proxy-password\"")]
            [InlineData(null, "download \"MyPackage\" --confirm")]
            public void Should_Add_ProxyPassword_Flag_To_Arguments_If_Set(string proxyPassword, string expected)
            {
                // Given
                var fixture = new ChocolateyDownloadFixture();
                fixture.Settings.ProxyPassword = proxyPassword;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData("proxy1,proxy2", "download \"MyPackage\" --confirm --proxy-bypass-list=\"proxy1,proxy2\"")]
            [InlineData(null, "download \"MyPackage\" --confirm")]
            public void Should_Add_ProxyByPassList_Flag_To_Arguments_If_Set(string proxyByPassList, string expected)
            {
                // Given
                var fixture = new ChocolateyDownloadFixture();
                fixture.Settings.ProxyByPassList = proxyByPassList;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "download \"MyPackage\" --confirm --proxy-bypass-on-local")]
            [InlineData(false, "download \"MyPackage\" --confirm")]
            public void Should_Add_ProxyByPassOnLocal_Flag_To_Arguments_If_Set(bool proxyBypassOnLocal, string expected)
            {
                // Given
                var fixture = new ChocolateyDownloadFixture();
                fixture.Settings.ProxyBypassOnLocal = proxyBypassOnLocal;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData("./output.log", "download \"MyPackage\" --confirm --log-file=\"/Working/output.log\"")]
            [InlineData(null, "download \"MyPackage\" --confirm")]
            public void Should_Add_Log_File_Flag_To_Arguments_If_Set(string logFilePath, string expected)
            {
                // Given
                var fixture = new ChocolateyDownloadFixture();
                fixture.Settings.LogFile = logFilePath;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "download \"MyPackage\" --confirm --skip-compatibility-checks")]
            [InlineData(false, "download \"MyPackage\" --confirm")]
            public void Should_Add_Skip_Compatibility_Flag_To_Arguments_If_Set(bool skipCompatibiity, string expected)
            {
                // Given
                var fixture = new ChocolateyDownloadFixture();
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
                var fixture = new ChocolateyDownloadFixture();
                fixture.Settings.Source = "A";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("download \"MyPackage\" --confirm --source=\"A\"", result.Args);
            }

            [Fact]
            public void Should_Add_Version_To_Arguments_If_Not_Null()
            {
                // Given
                var fixture = new ChocolateyDownloadFixture();
                fixture.Settings.Version = "1.0.0";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("download \"MyPackage\" --confirm --version=\"1.0.0\"", result.Args);
            }

            [Theory]
            [InlineData(true, "download \"MyPackage\" --confirm --pre")]
            [InlineData(false, "download \"MyPackage\" --confirm")]
            public void Should_Add_Prerelease_Flag_To_Arguments_If_Set(bool prerelease, string expected)
            {
                // Given
                var fixture = new ChocolateyDownloadFixture();
                fixture.Settings.Prerelease = prerelease;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData("user1", "download \"MyPackage\" --confirm --user=\"user1\"")]
            [InlineData("", "download \"MyPackage\" --confirm")]
            public void Should_Add_User_To_Arguments_If_Set(string user, string expected)
            {
                // Given
                var fixture = new ChocolateyDownloadFixture();
                fixture.Settings.User = user;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData("password1", "download \"MyPackage\" --confirm --password=\"password1\"")]
            [InlineData("", "download \"MyPackage\" --confirm")]
            public void Should_Add_Password_To_Arguments_If_Set(string password, string expected)
            {
                // Given
                var fixture = new ChocolateyDownloadFixture();
                fixture.Settings.Password = password;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData("./mycert.pfx", "download \"MyPackage\" --confirm --cert=\"/Working/mycert.pfx\"")]
            [InlineData(null, "download \"MyPackage\" --confirm")]
            public void Should_Add_Cert_To_Arguments_If_Set(string certificate, string expected)
            {
                // Given
                var fixture = new ChocolateyDownloadFixture();
                fixture.Settings.Certificate = certificate;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData("certpassword", "download \"MyPackage\" --confirm --certpassword=\"certpassword\"")]
            [InlineData("", "download \"MyPackage\" --confirm")]
            public void Should_Add_CertPassword_To_Arguments_If_Set(string certificatePassword, string expected)
            {
                // Given
                var fixture = new ChocolateyDownloadFixture();
                fixture.Settings.CertificatePassword = certificatePassword;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Fact]
            public void Should_Add_Output_Directory_To_Arguments_If_Not_Null()
            {
                // Given
                var fixture = new ChocolateyDownloadFixture();
                fixture.Settings.OutputDirectory = "./Foo";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("download \"MyPackage\" --confirm --output-directory=\"/Working/Foo\"", result.Args);
            }

            [Theory]
            [InlineData(true, "download \"MyPackage\" --confirm --ignore-dependencies")]
            [InlineData(false, "download \"MyPackage\" --confirm")]
            public void Should_Add_IgnoreDependencies_Flag_To_Arguments_If_Set(bool ignoreDependencies, string expected)
            {
                // Given
                var fixture = new ChocolateyDownloadFixture();
                fixture.Settings.IgnoreDependencies = ignoreDependencies;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "download \"MyPackage\" --confirm --installed-packages")]
            [InlineData(false, "download \"MyPackage\" --confirm")]
            public void Should_Add_Installed_Flag_To_Arguments_If_Set(bool installed, string expected)
            {
                // Given
                var fixture = new ChocolateyDownloadFixture();
                fixture.Settings.Installed = installed;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "download \"MyPackage\" --confirm --ignore-unfound")]
            [InlineData(false, "download \"MyPackage\" --confirm")]
            public void Should_Add_IgnoreUnfound_Flag_To_Arguments_If_Set(bool ignoreUnfound, string expected)
            {
                // Given
                var fixture = new ChocolateyDownloadFixture();
                fixture.Settings.IgnoreUnfound = ignoreUnfound;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "download \"MyPackage\" --confirm --disable-repository-optimizations")]
            [InlineData(false, "download \"MyPackage\" --confirm")]
            public void Should_Add_DisableRepositoryOptimizations_Flag_To_Arguments_If_Set(bool disableRepositoryOptimizations, string expected)
            {
                // Given
                var fixture = new ChocolateyDownloadFixture();
                fixture.Settings.DisableRepositoryOptimizations = disableRepositoryOptimizations;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "download \"MyPackage\" --confirm --internalize")]
            [InlineData(false, "download \"MyPackage\" --confirm")]
            public void Should_Add_Internalize_Flag_To_Arguments_If_Set(bool internalize, string expected)
            {
                // Given
                var fixture = new ChocolateyDownloadFixture();
                fixture.Settings.Internalize = internalize;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "download \"MyPackage\" --confirm --internalize --internalize-all-urls")]
            [InlineData(false, "download \"MyPackage\" --confirm --internalize")]
            public void Should_Add_InternalizeAllUrls_Flag_To_Arguments_If_Set(bool internalizeAllUrls, string expected)
            {
                // Given
                var fixture = new ChocolateyDownloadFixture();
                fixture.Settings.Internalize = true;
                fixture.Settings.InternalizeAllUrls = internalizeAllUrls;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true)]
            [InlineData(false)]
            public void Should_Not_Add_InternalizeAllUrls_Flag_To_Arguments_If_Internalize_Is_Not_Set(bool internalizeAllUrls)
            {
                // Given
                var fixture = new ChocolateyDownloadFixture();
                fixture.Settings.Internalize = false;
                fixture.Settings.InternalizeAllUrls = internalizeAllUrls;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("download \"MyPackage\" --confirm", result.Args);
            }

            [Theory]
            [InlineData(@"\\foo", "download \"MyPackage\" --confirm --internalize --resources-location=\"\\\\foo\"")]
            [InlineData("https://foo.local", "download \"MyPackage\" --confirm --internalize --resources-location=\"https://foo.local\"")]
            [InlineData(null, "download \"MyPackage\" --confirm --internalize")]
            [InlineData("", "download \"MyPackage\" --confirm --internalize")]
            public void Should_Add_ResourceLocation_To_Arguments_If_Set(string resourcesLocation, string expected)
            {
                // Given
                var fixture = new ChocolateyDownloadFixture();
                fixture.Settings.Internalize = true;
                fixture.Settings.ResourcesLocation = resourcesLocation;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(@"\\foo")]
            [InlineData("https://foo.local")]
            [InlineData(null)]
            [InlineData("")]
            public void Should_Not_Add_ResourceLocation_To_Arguments_If_Internalize_Is_Not_Set(string resourcesLocation)
            {
                // Given
                var fixture = new ChocolateyDownloadFixture();
                fixture.Settings.Internalize = false;
                fixture.Settings.ResourcesLocation = resourcesLocation;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("download \"MyPackage\" --confirm", result.Args);
            }

            [Theory]
            [InlineData(@"\\foo", "download \"MyPackage\" --confirm --internalize --resources-location=\"\\\\resources\" --download-location=\"\\\\foo\"")]
            [InlineData("https://foo.local", "download \"MyPackage\" --confirm --internalize --resources-location=\"\\\\resources\" --download-location=\"https://foo.local\"")]
            [InlineData(null, "download \"MyPackage\" --confirm --internalize --resources-location=\"\\\\resources\"")]
            [InlineData("", "download \"MyPackage\" --confirm --internalize --resources-location=\"\\\\resources\"")]
            public void Should_Add_DownloadLocation_To_Arguments_If_Set(string downloadLocation, string expected)
            {
                // Given
                var fixture = new ChocolateyDownloadFixture();
                fixture.Settings.Internalize = true;
                fixture.Settings.ResourcesLocation = @"\\resources";
                fixture.Settings.DownloadLocation = downloadLocation;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(@"\\foo")]
            [InlineData("https://foo.local")]
            [InlineData(null)]
            [InlineData("")]
            public void Should_Not_Add_DownloadLocation_To_Arguments_If_Internalize_Is_Not_Set(string downloadLocation)
            {
                // Given
                var fixture = new ChocolateyDownloadFixture();
                fixture.Settings.Internalize = false;
                fixture.Settings.ResourcesLocation = @"\\foo";
                fixture.Settings.DownloadLocation = downloadLocation;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("download \"MyPackage\" --confirm", result.Args);
            }

            [Theory]
            [InlineData(@"\\foo")]
            [InlineData("https://foo.local")]
            [InlineData(null)]
            [InlineData("")]
            public void Should_Not_Add_DownloadLocation_To_Arguments_If_ResourcesLocation_Is_Not_Set(string downloadLocation)
            {
                // Given
                var fixture = new ChocolateyDownloadFixture();
                fixture.Settings.Internalize = true;
                fixture.Settings.ResourcesLocation = null;
                fixture.Settings.DownloadLocation = downloadLocation;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("download \"MyPackage\" --confirm --internalize", result.Args);
            }

            [Theory]
            [InlineData(true, "download \"MyPackage\" --confirm --internalize --append-use-original-location")]
            [InlineData(false, "download \"MyPackage\" --confirm --internalize")]
            public void Should_Add_AppendUseOriginalLocation_Flag_To_Arguments_If_Set(bool appendUseOriginalLocation, string expected)
            {
                // Given
                var fixture = new ChocolateyDownloadFixture();
                fixture.Settings.Internalize = true;
                fixture.Settings.AppendUseOriginalLocation = appendUseOriginalLocation;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true)]
            [InlineData(false)]
            public void Should_Not_Add_AppendUseOriginalLocation_Flag_To_Arguments_If_Internalize_Is_Not_Set(bool appendUseOriginalLocation)
            {
                // Given
                var fixture = new ChocolateyDownloadFixture();
                fixture.Settings.Internalize = false;
                fixture.Settings.AppendUseOriginalLocation = appendUseOriginalLocation;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("download \"MyPackage\" --confirm", result.Args);
            }

            [Theory]
            [InlineData(true, "download \"MyPackage\" --confirm --skip-download-cache")]
            [InlineData(false, "download \"MyPackage\" --confirm")]
            public void Should_Add_SkipDownloadCache_Flag_To_Arguments_If_Set(bool skipDownloadCache, string expected)
            {
                // Given
                var fixture = new ChocolateyDownloadFixture();
                fixture.Settings.SkipDownloadCache = skipDownloadCache;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "download \"MyPackage\" --confirm --use-download-cache")]
            [InlineData(false, "download \"MyPackage\" --confirm")]
            public void Should_Add_UseDownloadCache_Flag_To_Arguments_If_Set(bool useDownloadCache, string expected)
            {
                // Given
                var fixture = new ChocolateyDownloadFixture();
                fixture.Settings.UseDownloadCache = useDownloadCache;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "download \"MyPackage\" --confirm --skip-virus-check")]
            [InlineData(false, "download \"MyPackage\" --confirm")]
            public void Should_Add_SkipVirusCheck_Flag_To_Arguments_If_Set(bool skipVirusCheck, string expected)
            {
                // Given
                var fixture = new ChocolateyDownloadFixture();
                fixture.Settings.SkipVirusCheck = skipVirusCheck;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "download \"MyPackage\" --confirm --virus-check")]
            [InlineData(false, "download \"MyPackage\" --confirm")]
            public void Should_Add_VirusCheck_Flag_To_Arguments_If_Set(bool virusCheck, string expected)
            {
                // Given
                var fixture = new ChocolateyDownloadFixture();
                fixture.Settings.VirusCheck = virusCheck;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(5, "download \"MyPackage\" --confirm --virus-positives-minimum=\"5\"")]
            [InlineData(0, "download \"MyPackage\" --confirm")]
            public void Should_Add_VirusPositivesMinimum_To_Arguments_If_Set(int virusPositivesMinimum, string expected)
            {
                // Given
                var fixture = new ChocolateyDownloadFixture();
                fixture.Settings.VirusPositivesMinimum = virusPositivesMinimum;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }
        }
    }
}