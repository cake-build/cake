// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tests.Fixtures.Tools.Chocolatey.ApiKey;
using Cake.Common.Tests.Fixtures.Tools.Chocolatey.Config;
using Cake.Testing;
using Cake.Testing.Xunit;
using Xunit;

namespace Cake.Common.Tests.Unit.Tools.Chocolatey.Config
{
    public sealed class ChocolateyConfigSetterTests
    {
        public sealed class TheSetMethod
        {
            [Fact]
            public void Should_Throw_If_Settings_Are_Null()
            {
                // Given
                var fixture = new ChocolateyConfigSetterFixture();
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
                var fixture = new ChocolateyConfigSetterFixture();
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
                var fixture = new ChocolateyConfigSetterFixture();
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
                var fixture = new ChocolateyConfigSetterFixture();
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
                var fixture = new ChocolateyConfigSetterFixture();
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
                var fixture = new ChocolateyConfigSetterFixture();
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
                var fixture = new ChocolateyConfigSetterFixture();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/Working/tools/choco.exe", result.Path.FullPath);
            }

            [Fact]
            public void Should_Add_Mandatory_Arguments()
            {
                // Given
                var fixture = new ChocolateyConfigSetterFixture();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("config set --name=\"cacheLocation\" " +
                             "--value=\"c:\\temp\" --confirm", result.Args);
            }

            [Theory]
            [InlineData(true, "config set --name=\"cacheLocation\" --value=\"c:\\temp\" --debug --confirm")]
            [InlineData(false, "config set --name=\"cacheLocation\" --value=\"c:\\temp\" --confirm")]
            public void Should_Add_Debug_Flag_To_Arguments_If_Set(bool debug, string expected)
            {
                // Given
                var fixture = new ChocolateyConfigSetterFixture();
                fixture.Settings.Debug = debug;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "config set --name=\"cacheLocation\" --value=\"c:\\temp\" --verbose --confirm")]
            [InlineData(false, "config set --name=\"cacheLocation\" --value=\"c:\\temp\" --confirm")]
            public void Should_Add_Verbose_Flag_To_Arguments_If_Set(bool verbose, string expected)
            {
                // Given
                var fixture = new ChocolateyConfigSetterFixture();
                fixture.Settings.Verbose = verbose;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "config set --name=\"cacheLocation\" --value=\"c:\\temp\" --trace --confirm")]
            [InlineData(false, "config set --name=\"cacheLocation\" --value=\"c:\\temp\" --confirm")]
            public void Should_Add_Trace_Flag_To_Arguments_If_Set(bool trace, string expected)
            {
                // Given
                var fixture = new ChocolateyConfigSetterFixture();
                fixture.Settings.Trace = trace;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "config set --name=\"cacheLocation\" --value=\"c:\\temp\" --no-color --confirm")]
            [InlineData(false, "config set --name=\"cacheLocation\" --value=\"c:\\temp\" --confirm")]
            public void Should_Add_NoColor_Flag_To_Arguments_If_Set(bool noColor, string expected)
            {
                // Given
                var fixture = new ChocolateyConfigSetterFixture();
                fixture.Settings.NoColor = noColor;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "config set --name=\"cacheLocation\" --value=\"c:\\temp\" --accept-license --confirm")]
            [InlineData(false, "config set --name=\"cacheLocation\" --value=\"c:\\temp\" --confirm")]
            public void Should_Add_AcceptLicense_Flag_To_Arguments_If_Set(bool acceptLicense, string expected)
            {
                // Given
                var fixture = new ChocolateyConfigSetterFixture();
                fixture.Settings.AcceptLicense = acceptLicense;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "config set --name=\"cacheLocation\" --value=\"c:\\temp\" --confirm --force")]
            [InlineData(false, "config set --name=\"cacheLocation\" --value=\"c:\\temp\" --confirm")]
            public void Should_Add_Force_Flag_To_Arguments_If_Set(bool force, string expected)
            {
                // Given
                var fixture = new ChocolateyConfigSetterFixture();
                fixture.Settings.Force = force;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "config set --name=\"cacheLocation\" --value=\"c:\\temp\" --confirm --what-if")]
            [InlineData(false, "config set --name=\"cacheLocation\" --value=\"c:\\temp\" --confirm")]
            public void Should_Add_Noop_Flag_To_Arguments_If_Set(bool noop, string expected)
            {
                // Given
                var fixture = new ChocolateyConfigSetterFixture();
                fixture.Settings.Noop = noop;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "config set --name=\"cacheLocation\" --value=\"c:\\temp\" --confirm --limit-output")]
            [InlineData(false, "config set --name=\"cacheLocation\" --value=\"c:\\temp\" --confirm")]
            public void Should_Add_LimitOutput_Flag_To_Arguments_If_Set(bool limitOutput, string expected)
            {
                // Given
                var fixture = new ChocolateyConfigSetterFixture();
                fixture.Settings.LimitOutput = limitOutput;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(5, "config set --name=\"cacheLocation\" --value=\"c:\\temp\" --confirm --execution-timeout=\"5\"")]
            [InlineData(0, "config set --name=\"cacheLocation\" --value=\"c:\\temp\" --confirm")]
            public void Should_Add_ExecutionTimeout_To_Arguments_If_Set(int executionTimeout, string expected)
            {
                // Given
                var fixture = new ChocolateyConfigSetterFixture();
                fixture.Settings.ExecutionTimeout = executionTimeout;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(@"c:\temp", "config set --name=\"cacheLocation\" --value=\"c:\\temp\" --confirm --cache-location=\"c:\\temp\"")]
            [InlineData("", "config set --name=\"cacheLocation\" --value=\"c:\\temp\" --confirm")]
            public void Should_Add_CacheLocation_Flag_To_Arguments_If_Set(string cacheLocation, string expected)
            {
                // Given
                var fixture = new ChocolateyConfigSetterFixture();
                fixture.Settings.CacheLocation = cacheLocation;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "config set --name=\"cacheLocation\" --value=\"c:\\temp\" --confirm --allow-unofficial")]
            [InlineData(false, "config set --name=\"cacheLocation\" --value=\"c:\\temp\" --confirm")]
            public void Should_Add_AllowUnofficial_Flag_To_Arguments_If_Set(bool allowUnofficial, string expected)
            {
                // Given
                var fixture = new ChocolateyConfigSetterFixture();
                fixture.Settings.AllowUnofficial = allowUnofficial;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "config set --name=\"cacheLocation\" --value=\"c:\\temp\" --confirm --fail-on-error-output")]
            [InlineData(false, "config set --name=\"cacheLocation\" --value=\"c:\\temp\" --confirm")]
            public void Should_Add_FailOnErrorOutput_Flag_To_Arguments_If_Set(bool failOnErrorOutput, string expected)
            {
                // Given
                var fixture = new ChocolateyConfigSetterFixture();
                fixture.Settings.FailOnErrorOutput = failOnErrorOutput;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "config set --name=\"cacheLocation\" --value=\"c:\\temp\" --confirm --use-system-powershell")]
            [InlineData(false, "config set --name=\"cacheLocation\" --value=\"c:\\temp\" --confirm")]
            public void Should_Add_UseSystemPowerShell_Flag_To_Arguments_If_Set(bool useSystemPowerShell, string expected)
            {
                // Given
                var fixture = new ChocolateyConfigSetterFixture();
                fixture.Settings.UseSystemPowerShell = useSystemPowerShell;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "config set --name=\"cacheLocation\" --value=\"c:\\temp\" --confirm --no-progress")]
            [InlineData(false, "config set --name=\"cacheLocation\" --value=\"c:\\temp\" --confirm")]
            public void Should_Add_NoProgress_Flag_To_Arguments_If_Set(bool noProgress, string expected)
            {
                // Given
                var fixture = new ChocolateyConfigSetterFixture();
                fixture.Settings.NoProgress = noProgress;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData("proxy", "config set --name=\"cacheLocation\" --value=\"c:\\temp\" --confirm --proxy=\"proxy\"")]
            [InlineData(null, "config set --name=\"cacheLocation\" --value=\"c:\\temp\" --confirm")]
            public void Should_Add_Proxy_Flag_To_Arguments_If_Set(string proxy, string expected)
            {
                // Given
                var fixture = new ChocolateyConfigSetterFixture();
                fixture.Settings.Proxy = proxy;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData("proxy-user", "config set --name=\"cacheLocation\" --value=\"c:\\temp\" --confirm --proxy-user=\"proxy-user\"")]
            [InlineData(null, "config set --name=\"cacheLocation\" --value=\"c:\\temp\" --confirm")]
            public void Should_Add_ProxyUser_Flag_To_Arguments_If_Set(string proxyUser, string expected)
            {
                // Given
                var fixture = new ChocolateyConfigSetterFixture();
                fixture.Settings.ProxyUser = proxyUser;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData("proxy-password", "config set --name=\"cacheLocation\" --value=\"c:\\temp\" --confirm --proxy-password=\"proxy-password\"")]
            [InlineData(null, "config set --name=\"cacheLocation\" --value=\"c:\\temp\" --confirm")]
            public void Should_Add_ProxyPassword_Flag_To_Arguments_If_Set(string proxyPassword, string expected)
            {
                // Given
                var fixture = new ChocolateyConfigSetterFixture();
                fixture.Settings.ProxyPassword = proxyPassword;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData("proxy1,proxy2", "config set --name=\"cacheLocation\" --value=\"c:\\temp\" --confirm --proxy-bypass-list=\"proxy1,proxy2\"")]
            [InlineData(null, "config set --name=\"cacheLocation\" --value=\"c:\\temp\" --confirm")]
            public void Should_Add_ProxyByPassList_Flag_To_Arguments_If_Set(string proxyByPassList, string expected)
            {
                // Given
                var fixture = new ChocolateyConfigSetterFixture();
                fixture.Settings.ProxyByPassList = proxyByPassList;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "config set --name=\"cacheLocation\" --value=\"c:\\temp\" --confirm --proxy-bypass-on-local")]
            [InlineData(false, "config set --name=\"cacheLocation\" --value=\"c:\\temp\" --confirm")]
            public void Should_Add_ProxyByPassOnLocal_Flag_To_Arguments_If_Set(bool proxyBypassOnLocal, string expected)
            {
                // Given
                var fixture = new ChocolateyConfigSetterFixture();
                fixture.Settings.ProxyBypassOnLocal = proxyBypassOnLocal;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData("./output.log", "config set --name=\"cacheLocation\" --value=\"c:\\temp\" --confirm --log-file=\"/Working/output.log\"")]
            [InlineData(null, "config set --name=\"cacheLocation\" --value=\"c:\\temp\" --confirm")]
            public void Should_Add_Log_File_Flag_To_Arguments_If_Set(string logFilePath, string expected)
            {
                // Given
                var fixture = new ChocolateyConfigSetterFixture();
                fixture.Settings.LogFile = logFilePath;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "config set --name=\"cacheLocation\" --value=\"c:\\temp\" --confirm --skip-compatibility-checks")]
            [InlineData(false, "config set --name=\"cacheLocation\" --value=\"c:\\temp\" --confirm")]
            public void Should_Add_Skip_Compatibility_Flag_To_Arguments_If_Set(bool skipCompatibiity, string expected)
            {
                // Given
                var fixture = new ChocolateyConfigSetterFixture();
                fixture.Settings.SkipCompatibilityChecks = skipCompatibiity;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }
        }
    }
}