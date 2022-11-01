// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Common.Tests.Fixtures.Tools.Chocolatey.ApiKey;
using Cake.Common.Tests.Fixtures.Tools.Chocolatey.Push;
using Cake.Testing;
using Cake.Testing.Xunit;
using Xunit;

namespace Cake.Common.Tests.Unit.Tools.Chocolatey.Push
{
    public sealed class ChocolateyPusherTests
    {
        public sealed class ThePushMethod
        {
            [Fact]
            public void Should_Throw_If_Settings_Are_Null()
            {
                // Given
                var fixture = new ChocolateyPusherFixture();
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
                var fixture = new ChocolateyPusherFixture();
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
                var fixture = new ChocolateyPusherFixture();
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
                var fixture = new ChocolateyPusherFixture();
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
                var fixture = new ChocolateyPusherFixture();
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
                var fixture = new ChocolateyPusherFixture();
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
                var fixture = new ChocolateyPusherFixture();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/Working/tools/choco.exe", result.Path.FullPath);
            }

            [Fact]
            public void Should_Add_Mandatory_Arguments()
            {
                // Given
                var fixture = new ChocolateyPusherFixture();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("push \"/Working/existing.nupkg\" --confirm", result.Args);
            }

            [Theory]
            [InlineData(true, "push \"/Working/existing.nupkg\" --debug --confirm")]
            [InlineData(false, "push \"/Working/existing.nupkg\" --confirm")]
            public void Should_Add_Debug_Flag_To_Arguments_If_Set(bool debug, string expected)
            {
                // Given
                var fixture = new ChocolateyPusherFixture();
                fixture.Settings.Debug = debug;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "push \"/Working/existing.nupkg\" --verbose --confirm")]
            [InlineData(false, "push \"/Working/existing.nupkg\" --confirm")]
            public void Should_Add_Verbose_Flag_To_Arguments_If_Set(bool verbose, string expected)
            {
                // Given
                var fixture = new ChocolateyPusherFixture();
                fixture.Settings.Verbose = verbose;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "push \"/Working/existing.nupkg\" --trace --confirm")]
            [InlineData(false, "push \"/Working/existing.nupkg\" --confirm")]
            public void Should_Add_Trace_Flag_To_Arguments_If_Set(bool trace, string expected)
            {
                // Given
                var fixture = new ChocolateyPusherFixture();
                fixture.Settings.Trace = trace;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "push \"/Working/existing.nupkg\" --no-color --confirm")]
            [InlineData(false, "push \"/Working/existing.nupkg\" --confirm")]
            public void Should_Add_NoColor_Flag_To_Arguments_If_Set(bool noColor, string expected)
            {
                // Given
                var fixture = new ChocolateyPusherFixture();
                fixture.Settings.NoColor = noColor;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "push \"/Working/existing.nupkg\" --accept-license --confirm")]
            [InlineData(false, "push \"/Working/existing.nupkg\" --confirm")]
            public void Should_Add_AcceptLicense_Flag_To_Arguments_If_Set(bool acceptLicense, string expected)
            {
                // Given
                var fixture = new ChocolateyPusherFixture();
                fixture.Settings.AcceptLicense = acceptLicense;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "push \"/Working/existing.nupkg\" --confirm --force")]
            [InlineData(false, "push \"/Working/existing.nupkg\" --confirm")]
            public void Should_Add_Force_Flag_To_Arguments_If_Set(bool force, string expected)
            {
                // Given
                var fixture = new ChocolateyPusherFixture();
                fixture.Settings.Force = force;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "push \"/Working/existing.nupkg\" --confirm --what-if")]
            [InlineData(false, "push \"/Working/existing.nupkg\" --confirm")]
            public void Should_Add_Noop_Flag_To_Arguments_If_Set(bool noop, string expected)
            {
                // Given
                var fixture = new ChocolateyPusherFixture();
                fixture.Settings.Noop = noop;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "push \"/Working/existing.nupkg\" --confirm --limit-output")]
            [InlineData(false, "push \"/Working/existing.nupkg\" --confirm")]
            public void Should_Add_LimitOutput_Flag_To_Arguments_If_Set(bool limitOutput, string expected)
            {
                // Given
                var fixture = new ChocolateyPusherFixture();
                fixture.Settings.LimitOutput = limitOutput;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(5, "push \"/Working/existing.nupkg\" --confirm --execution-timeout=\"5\"")]
            [InlineData(0, "push \"/Working/existing.nupkg\" --confirm")]
            public void Should_Add_ExecutionTimeout_To_Arguments_If_Set(int executionTimeout, string expected)
            {
                // Given
                var fixture = new ChocolateyPusherFixture();
                fixture.Settings.ExecutionTimeout = executionTimeout;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(@"c:\temp", "push \"/Working/existing.nupkg\" --confirm --cache-location=\"c:\\temp\"")]
            [InlineData("", "push \"/Working/existing.nupkg\" --confirm")]
            public void Should_Add_CacheLocation_Flag_To_Arguments_If_Set(string cacheLocation, string expected)
            {
                // Given
                var fixture = new ChocolateyPusherFixture();
                fixture.Settings.CacheLocation = cacheLocation;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "push \"/Working/existing.nupkg\" --confirm --allow-unofficial")]
            [InlineData(false, "push \"/Working/existing.nupkg\" --confirm")]
            public void Should_Add_AllowUnofficial_Flag_To_Arguments_If_Set(bool allowUnofficial, string expected)
            {
                // Given
                var fixture = new ChocolateyPusherFixture();
                fixture.Settings.AllowUnofficial = allowUnofficial;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }
            [Theory]
            [InlineData(true, "push \"/Working/existing.nupkg\" --confirm --fail-on-error-output")]
            [InlineData(false, "push \"/Working/existing.nupkg\" --confirm")]
            public void Should_Add_FailOnErrorOutput_Flag_To_Arguments_If_Set(bool failOnErrorOutput, string expected)
            {
                // Given
                var fixture = new ChocolateyPusherFixture();
                fixture.Settings.FailOnErrorOutput = failOnErrorOutput;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "push \"/Working/existing.nupkg\" --confirm --use-system-powershell")]
            [InlineData(false, "push \"/Working/existing.nupkg\" --confirm")]
            public void Should_Add_UseSystemPowerShell_Flag_To_Arguments_If_Set(bool useSystemPowerShell, string expected)
            {
                // Given
                var fixture = new ChocolateyPusherFixture();
                fixture.Settings.UseSystemPowerShell = useSystemPowerShell;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "push \"/Working/existing.nupkg\" --confirm --no-progress")]
            [InlineData(false, "push \"/Working/existing.nupkg\" --confirm")]
            public void Should_Add_NoProgress_Flag_To_Arguments_If_Set(bool noProgress, string expected)
            {
                // Given
                var fixture = new ChocolateyPusherFixture();
                fixture.Settings.NoProgress = noProgress;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData("proxy", "push \"/Working/existing.nupkg\" --confirm --proxy=\"proxy\"")]
            [InlineData(null, "push \"/Working/existing.nupkg\" --confirm")]
            public void Should_Add_Proxy_Flag_To_Arguments_If_Set(string proxy, string expected)
            {
                // Given
                var fixture = new ChocolateyPusherFixture();
                fixture.Settings.Proxy = proxy;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData("proxy-user", "push \"/Working/existing.nupkg\" --confirm --proxy-user=\"proxy-user\"")]
            [InlineData(null, "push \"/Working/existing.nupkg\" --confirm")]
            public void Should_Add_ProxyUser_Flag_To_Arguments_If_Set(string proxyUser, string expected)
            {
                // Given
                var fixture = new ChocolateyPusherFixture();
                fixture.Settings.ProxyUser = proxyUser;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData("proxy-password", "push \"/Working/existing.nupkg\" --confirm --proxy-password=\"proxy-password\"")]
            [InlineData(null, "push \"/Working/existing.nupkg\" --confirm")]
            public void Should_Add_ProxyPassword_Flag_To_Arguments_If_Set(string proxyPassword, string expected)
            {
                // Given
                var fixture = new ChocolateyPusherFixture();
                fixture.Settings.ProxyPassword = proxyPassword;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData("proxy1,proxy2", "push \"/Working/existing.nupkg\" --confirm --proxy-bypass-list=\"proxy1,proxy2\"")]
            [InlineData(null, "push \"/Working/existing.nupkg\" --confirm")]
            public void Should_Add_ProxyByPassList_Flag_To_Arguments_If_Set(string proxyByPassList, string expected)
            {
                // Given
                var fixture = new ChocolateyPusherFixture();
                fixture.Settings.ProxyByPassList = proxyByPassList;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "push \"/Working/existing.nupkg\" --confirm --proxy-bypass-on-local")]
            [InlineData(false, "push \"/Working/existing.nupkg\" --confirm")]
            public void Should_Add_ProxyByPassOnLocal_Flag_To_Arguments_If_Set(bool proxyBypassOnLocal, string expected)
            {
                // Given
                var fixture = new ChocolateyPusherFixture();
                fixture.Settings.ProxyBypassOnLocal = proxyBypassOnLocal;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData("./output.log", "push \"/Working/existing.nupkg\" --confirm --log-file=\"/Working/output.log\"")]
            [InlineData(null, "push \"/Working/existing.nupkg\" --confirm")]
            public void Should_Add_Log_File_Flag_To_Arguments_If_Set(string logFilePath, string expected)
            {
                // Given
                var fixture = new ChocolateyPusherFixture();
                fixture.Settings.LogFile = logFilePath;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "push \"/Working/existing.nupkg\" --confirm --skip-compatibility-checks")]
            [InlineData(false, "push \"/Working/existing.nupkg\" --confirm")]
            public void Should_Add_Skip_Compatibility_Flag_To_Arguments_If_Set(bool skipCompatibiity, string expected)
            {
                // Given
                var fixture = new ChocolateyPusherFixture();
                fixture.Settings.SkipCompatibilityChecks = skipCompatibiity;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Fact]
            public void Should_Add_Source_To_Arguments_If_Not_Null()
            {
                // Given
                var fixture = new ChocolateyPusherFixture();
                fixture.Settings.Source = "http://customsource/";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("push \"/Working/existing.nupkg\" " +
                             "--confirm --source=\"http://customsource/\"", result.Args);
            }

            [Fact]
            public void Should_Add_Api_Key_To_Arguments_If_Not_Null()
            {
                // Given
                var fixture = new ChocolateyPusherFixture();
                fixture.Settings.ApiKey = "1234";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("push \"/Working/existing.nupkg\" --confirm --api-key=\"1234\"", result.Args);
            }

            [Theory]
            [InlineData("abcdef", "push \"/Working/existing.nupkg\" --confirm --client-code=\"abcdef\"")]
            [InlineData(null, "push \"/Working/existing.nupkg\" --confirm")]
            public void Should_Add_ClientCode_Flag_To_Arguments_If_Set(string clientCode, string expected)
            {
                // Given
                var fixture = new ChocolateyPusherFixture();
                fixture.Settings.ClientCode = clientCode;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData("https://test.com", "push \"/Working/existing.nupkg\" --confirm --redirect-url=\"https://test.com\"")]
            [InlineData(null, "push \"/Working/existing.nupkg\" --confirm")]
            public void Should_Add_RedirectUrl_Flag_To_Arguments_If_Set(string redirectUrl, string expected)
            {
                // Given
                var fixture = new ChocolateyPusherFixture();
                fixture.Settings.RedirectUrl = redirectUrl;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData("IntuneA", "push \"/Working/existing.nupkg\" --confirm --endpoint=\"IntuneA\"")]
            [InlineData(null, "push \"/Working/existing.nupkg\" --confirm")]
            public void Should_Add_Endpoint_Flag_To_Arguments_If_Set(string endPoint, string expected)
            {
                // Given
                var fixture = new ChocolateyPusherFixture();
                fixture.Settings.EndPoint = endPoint;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "push \"/Working/existing.nupkg\" --confirm --skip-cleanup")]
            [InlineData(false, "push \"/Working/existing.nupkg\" --confirm")]
            public void Should_Add_SkipCleanup_Flag_To_Arguments_If_Set(bool skipCleanup, string expected)
            {
                // Given
                var fixture = new ChocolateyPusherFixture();
                fixture.Settings.SkipCleanup = skipCleanup;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }
        }
    }
}