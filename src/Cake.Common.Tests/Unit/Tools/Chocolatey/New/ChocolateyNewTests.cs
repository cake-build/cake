// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Common.Tests.Fixtures.Tools.Chocolatey.ApiKey;
using Cake.Common.Tests.Fixtures.Tools.Chocolatey.New;
using Cake.Common.Tests.Properties;
using Cake.Common.Tools.Chocolatey.New;
using Cake.Core;
using Cake.Core.IO;
using Cake.Testing;
using Cake.Testing.Xunit;
using Xunit;

namespace Cake.Common.Tests.Unit.Tools.Chocolatey.New
{
    public sealed class ChocolateyNewTests
    {
        public sealed class TheNewMethod
        {
            [Fact]
            public void Should_Throw_If_Package_Id_Is_Null()
            {
                // Given
                var fixture = new ChocolateyNewFixture();
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
                var fixture = new ChocolateyNewFixture();
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
                var fixture = new ChocolateyNewFixture();
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
                var fixture = new ChocolateyNewFixture();
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
                var fixture = new ChocolateyNewFixture();
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
                var fixture = new ChocolateyNewFixture();
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
                var fixture = new ChocolateyNewFixture();
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
                var fixture = new ChocolateyNewFixture();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/Working/tools/choco.exe", result.Path.FullPath);
            }

            [Fact]
            public void Should_Add_Mandatory_Arguments()
            {
                // Given
                var fixture = new ChocolateyNewFixture();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("new \"MyPackage\" --confirm", result.Args);
            }

            [Theory]
            [InlineData(true, "new \"MyPackage\" --debug --confirm")]
            [InlineData(false, "new \"MyPackage\" --confirm")]
            public void Should_Add_Debug_Flag_To_Arguments_If_Set(bool debug, string expected)
            {
                // Given
                var fixture = new ChocolateyNewFixture();
                fixture.Settings.Debug = debug;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "new \"MyPackage\" --verbose --confirm")]
            [InlineData(false, "new \"MyPackage\" --confirm")]
            public void Should_Add_Verbose_Flag_To_Arguments_If_Set(bool verbose, string expected)
            {
                // Given
                var fixture = new ChocolateyNewFixture();
                fixture.Settings.Verbose = verbose;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "new \"MyPackage\" --trace --confirm")]
            [InlineData(false, "new \"MyPackage\" --confirm")]
            public void Should_Add_Trace_Flag_To_Arguments_If_Set(bool trace, string expected)
            {
                // Given
                var fixture = new ChocolateyNewFixture();
                fixture.Settings.Trace = trace;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "new \"MyPackage\" --no-color --confirm")]
            [InlineData(false, "new \"MyPackage\" --confirm")]
            public void Should_Add_NoColor_Flag_To_Arguments_If_Set(bool noColor, string expected)
            {
                // Given
                var fixture = new ChocolateyNewFixture();
                fixture.Settings.NoColor = noColor;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "new \"MyPackage\" --accept-license --confirm")]
            [InlineData(false, "new \"MyPackage\" --confirm")]
            public void Should_Add_AcceptLicense_Flag_To_Arguments_If_Set(bool acceptLicense, string expected)
            {
                // Given
                var fixture = new ChocolateyNewFixture();
                fixture.Settings.AcceptLicense = acceptLicense;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "new \"MyPackage\" --confirm --force")]
            [InlineData(false, "new \"MyPackage\" --confirm")]
            public void Should_Add_Force_Flag_To_Arguments_If_Set(bool force, string expected)
            {
                // Given
                var fixture = new ChocolateyNewFixture();
                fixture.Settings.Force = force;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "new \"MyPackage\" --confirm --what-if")]
            [InlineData(false, "new \"MyPackage\" --confirm")]
            public void Should_Add_Noop_Flag_To_Arguments_If_Set(bool noop, string expected)
            {
                // Given
                var fixture = new ChocolateyNewFixture();
                fixture.Settings.Noop = noop;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "new \"MyPackage\" --confirm --limit-output")]
            [InlineData(false, "new \"MyPackage\" --confirm")]
            public void Should_Add_LimitOutput_Flag_To_Arguments_If_Set(bool limitOutput, string expected)
            {
                // Given
                var fixture = new ChocolateyNewFixture();
                fixture.Settings.LimitOutput = limitOutput;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(5, "new \"MyPackage\" --confirm --execution-timeout=\"5\"")]
            [InlineData(0, "new \"MyPackage\" --confirm")]
            public void Should_Add_ExecutionTimeout_To_Arguments_If_Set(int executionTimeout, string expected)
            {
                // Given
                var fixture = new ChocolateyNewFixture();
                fixture.Settings.ExecutionTimeout = executionTimeout;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(@"c:\temp", "new \"MyPackage\" --confirm --cache-location=\"c:\\temp\"")]
            [InlineData("", "new \"MyPackage\" --confirm")]
            public void Should_Add_CacheLocation_Flag_To_Arguments_If_Set(string cacheLocation, string expected)
            {
                // Given
                var fixture = new ChocolateyNewFixture();
                fixture.Settings.CacheLocation = cacheLocation;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "new \"MyPackage\" --confirm --allow-unofficial")]
            [InlineData(false, "new \"MyPackage\" --confirm")]
            public void Should_Add_AllowUnofficial_Flag_To_Arguments_If_Set(bool allowUnofficial, string expected)
            {
                // Given
                var fixture = new ChocolateyNewFixture();
                fixture.Settings.AllowUnofficial = allowUnofficial;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "new \"MyPackage\" --confirm --fail-on-error-output")]
            [InlineData(false, "new \"MyPackage\" --confirm")]
            public void Should_Add_FailOnErrorOutput_Flag_To_Arguments_If_Set(bool failOnErrorOutput, string expected)
            {
                // Given
                var fixture = new ChocolateyNewFixture();
                fixture.Settings.FailOnErrorOutput = failOnErrorOutput;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "new \"MyPackage\" --confirm --use-system-powershell")]
            [InlineData(false, "new \"MyPackage\" --confirm")]
            public void Should_Add_UseSystemPowerShell_Flag_To_Arguments_If_Set(bool useSystemPowerShell, string expected)
            {
                // Given
                var fixture = new ChocolateyNewFixture();
                fixture.Settings.UseSystemPowerShell = useSystemPowerShell;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "new \"MyPackage\" --confirm --no-progress")]
            [InlineData(false, "new \"MyPackage\" --confirm")]
            public void Should_Add_NoProgress_Flag_To_Arguments_If_Set(bool noProgress, string expected)
            {
                // Given
                var fixture = new ChocolateyNewFixture();
                fixture.Settings.NoProgress = noProgress;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData("proxy", "new \"MyPackage\" --confirm --proxy=\"proxy\"")]
            [InlineData(null, "new \"MyPackage\" --confirm")]
            public void Should_Add_Proxy_Flag_To_Arguments_If_Set(string proxy, string expected)
            {
                // Given
                var fixture = new ChocolateyNewFixture();
                fixture.Settings.Proxy = proxy;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData("proxy-user", "new \"MyPackage\" --confirm --proxy-user=\"proxy-user\"")]
            [InlineData(null, "new \"MyPackage\" --confirm")]
            public void Should_Add_ProxyUser_Flag_To_Arguments_If_Set(string proxyUser, string expected)
            {
                // Given
                var fixture = new ChocolateyNewFixture();
                fixture.Settings.ProxyUser = proxyUser;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData("proxy-password", "new \"MyPackage\" --confirm --proxy-password=\"proxy-password\"")]
            [InlineData(null, "new \"MyPackage\" --confirm")]
            public void Should_Add_ProxyPassword_Flag_To_Arguments_If_Set(string proxyPassword, string expected)
            {
                // Given
                var fixture = new ChocolateyNewFixture();
                fixture.Settings.ProxyPassword = proxyPassword;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData("proxy1,proxy2", "new \"MyPackage\" --confirm --proxy-bypass-list=\"proxy1,proxy2\"")]
            [InlineData(null, "new \"MyPackage\" --confirm")]
            public void Should_Add_ProxyByPassList_Flag_To_Arguments_If_Set(string proxyByPassList, string expected)
            {
                // Given
                var fixture = new ChocolateyNewFixture();
                fixture.Settings.ProxyByPassList = proxyByPassList;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "new \"MyPackage\" --confirm --proxy-bypass-on-local")]
            [InlineData(false, "new \"MyPackage\" --confirm")]
            public void Should_Add_ProxyByPassOnLocal_Flag_To_Arguments_If_Set(bool proxyBypassOnLocal, string expected)
            {
                // Given
                var fixture = new ChocolateyNewFixture();
                fixture.Settings.ProxyBypassOnLocal = proxyBypassOnLocal;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData("./output.log", "new \"MyPackage\" --confirm --log-file=\"/Working/output.log\"")]
            [InlineData(null, "new \"MyPackage\" --confirm")]
            public void Should_Add_Log_File_Flag_To_Arguments_If_Set(string logFilePath, string expected)
            {
                // Given
                var fixture = new ChocolateyNewFixture();
                fixture.Settings.LogFile = logFilePath;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "new \"MyPackage\" --confirm --skip-compatibility-checks")]
            [InlineData(false, "new \"MyPackage\" --confirm")]
            public void Should_Add_Skip_Compatibility_Flag_To_Arguments_If_Set(bool skipCompatibiity, string expected)
            {
                // Given
                var fixture = new ChocolateyNewFixture();
                fixture.Settings.SkipCompatibilityChecks = skipCompatibiity;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "new \"MyPackage\" --confirm --automaticpackage")]
            [InlineData(false, "new \"MyPackage\" --confirm")]
            public void Should_Add_AutomaticPackage_Flag_To_Arguments_If_Set(bool automaticPackage, string expected)
            {
                // Given
                var fixture = new ChocolateyNewFixture();
                fixture.Settings.AutomaticPackage = automaticPackage;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData("templateA", "new \"MyPackage\" --confirm --template-name=\"templateA\"")]
            [InlineData(null, "new \"MyPackage\" --confirm")]
            public void Should_Add_TemplateName_Flag_To_Arguments_If_Set(string templateName, string expected)
            {
                // Given
                var fixture = new ChocolateyNewFixture();
                fixture.Settings.TemplateName = templateName;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Fact]
            public void Should_Add_Package_Version_To_Arguments_If_Not_Null()
            {
                // Given
                var fixture = new ChocolateyNewFixture();
                fixture.Settings.PackageVersion = "1.2.3";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("new \"MyPackage\" --confirm packageversion=\"1.2.3\"", result.Args);
            }

            [Fact]
            public void Should_Add_Maintainer_Name_To_Arguments_If_Not_Null()
            {
                // Given
                var fixture = new ChocolateyNewFixture();
                fixture.Settings.MaintainerName = "John Doe";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("new \"MyPackage\" --confirm maintainername=\"John Doe\"", result.Args);
            }

            [Fact]
            public void Should_Add_Maintainer_Repo_To_Arguments_If_Not_Null()
            {
                // Given
                var fixture = new ChocolateyNewFixture();
                fixture.Settings.MaintainerRepo = "johndoe";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("new \"MyPackage\" --confirm maintainerrepo=\"johndoe\"", result.Args);
            }

            [Fact]
            public void Should_Add_Installer_Type_To_Arguments_If_Not_Null()
            {
                // Given
                var fixture = new ChocolateyNewFixture();
                fixture.Settings.InstallerType = "msi";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("new \"MyPackage\" --confirm installertype=\"msi\"", result.Args);
            }

            [Fact]
            public void Should_Add_Url_To_Arguments_If_Not_Null()
            {
                // Given
                var fixture = new ChocolateyNewFixture();
                fixture.Settings.Url = "https://example.com";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("new \"MyPackage\" --confirm url=\"https://example.com\"", result.Args);
            }

            [Fact]
            public void Should_Add_Url64_To_Arguments_If_Not_Null()
            {
                // Given
                var fixture = new ChocolateyNewFixture();
                fixture.Settings.Url64 = "https://example.com";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("new \"MyPackage\" --confirm url64=\"https://example.com\"", result.Args);
            }

            [Theory]
            [InlineData("-Foo", "new \"MyPackage\" --confirm silentargs=\"-Foo\"")]
            [InlineData("--Foo", "new \"MyPackage\" --confirm silentargs=\"--Foo\"")]
            [InlineData("/Foo", "new \"MyPackage\" --confirm silentargs=\"/Foo\"")]
            [InlineData("-Foo=Bar", "new \"MyPackage\" --confirm silentargs=\"-Foo=Bar\"")]
            [InlineData("-Foo --Foo /Foo -Foo=Bar", "new \"MyPackage\" --confirm silentargs=\"-Foo --Foo /Foo -Foo=Bar\"")]
            public void Should_Add_Silent_Args_To_Arguments_If_Set(string silentArgs, string expected)
            {
                // Given
                var fixture = new ChocolateyNewFixture();
                fixture.Settings.SilentArgs = silentArgs;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData("Foo", "Bar", "new \"MyPackage\" --confirm Foo=\"Bar\"")]
            [InlineData("Foo", "Foo Bar", "new \"MyPackage\" --confirm Foo=\"Foo Bar\"")]
            public void Should_Add_Additional_Property_Value_To_Arguments_If_Set(string additionalPropertyName, string additionalPropertyValue, string expected)
            {
                // Given
                var fixture = new ChocolateyNewFixture();
                fixture.Settings.AdditionalPropertyValues.Add(additionalPropertyName, additionalPropertyValue);

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Fact]
            public void Should_Add_Additional_Property_Values_To_Arguments_If_Set()
            {
                // Given
                var fixture = new ChocolateyNewFixture();
                fixture.Settings.AdditionalPropertyValues.Add("Foo1", "Bar1");
                fixture.Settings.AdditionalPropertyValues.Add("Foo2", "Bar2");

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("new \"MyPackage\" --confirm Foo1=\"Bar1\" Foo2=\"Bar2\"", result.Args);
            }

            [Fact]
            public void Should_Add_Output_Directory_To_Arguments_If_Not_Null()
            {
                // Given
                var fixture = new ChocolateyNewFixture();
                fixture.Settings.OutputDirectory = "./Foo";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("new \"MyPackage\" --confirm --output-directory=\"/Working/Foo\"", result.Args);
            }

            [Theory]
            [InlineData(true, "new \"MyPackage\" --confirm --built-in-template")]
            [InlineData(false, "new \"MyPackage\" --confirm")]
            public void Should_Add_BuiltInTemplate_Flag_To_Arguments_If_Set(bool builtIn, string expected)
            {
                // Given
                var fixture = new ChocolateyNewFixture();
                fixture.Settings.BuiltInTemplate = builtIn;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData("./input/install.exe", "new \"MyPackage\" --confirm --file=\"./input/install.exe\"")]
            [InlineData(null, "new \"MyPackage\" --confirm")]
            public void Should_Add_File_Flag_To_Arguments_If_Set(string fileName, string expected)
            {
                // Given
                var fixture = new ChocolateyNewFixture();
                fixture.Settings.File = fileName;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData("./input/install.exe", "new \"MyPackage\" --confirm --file64=\"./input/install.exe\"")]
            [InlineData(null, "new \"MyPackage\" --confirm")]
            public void Should_Add_File64_Flag_To_Arguments_If_Set(string file64Name, string expected)
            {
                // Given
                var fixture = new ChocolateyNewFixture();
                fixture.Settings.File64 = file64Name;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "new \"MyPackage\" --confirm --use-original-files-location")]
            [InlineData(false, "new \"MyPackage\" --confirm")]
            public void Should_Add_UseOriginalFilesLocation_Flag_To_Arguments_If_Set(bool useOriginalFilesLocation, string expected)
            {
                // Given
                var fixture = new ChocolateyNewFixture();
                fixture.Settings.UseOriginalFilesLocation = useOriginalFilesLocation;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData("abcdef", "new \"MyPackage\" --confirm --download-checksum=\"abcdef\"")]
            [InlineData(null, "new \"MyPackage\" --confirm")]
            public void Should_Add_Checksum_Flag_To_Arguments_If_Set(string checksum, string expected)
            {
                // Given
                var fixture = new ChocolateyNewFixture();
                fixture.Settings.Checksum = checksum;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData("abcdef", "new \"MyPackage\" --confirm --download-checksum-x64=\"abcdef\"")]
            [InlineData(null, "new \"MyPackage\" --confirm")]
            public void Should_Add_Checksum64_Flag_To_Arguments_If_Set(string checksum64, string expected)
            {
                // Given
                var fixture = new ChocolateyNewFixture();
                fixture.Settings.Checksum64 = checksum64;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData("md5", "new \"MyPackage\" --confirm --download-checksum-type=\"md5\"")]
            [InlineData(null, "new \"MyPackage\" --confirm")]
            public void Should_Add_ChecksumType_Flag_To_Arguments_If_Set(string checkSumType, string expected)
            {
                // Given
                var fixture = new ChocolateyNewFixture();
                fixture.Settings.ChecksumType = checkSumType;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "new \"MyPackage\" --confirm --pause-on-error")]
            [InlineData(false, "new \"MyPackage\" --confirm")]
            public void Should_Add_PauseOnError_Flag_To_Arguments_If_Set(bool pauseOnError, string expected)
            {
                // Given
                var fixture = new ChocolateyNewFixture();
                fixture.Settings.PauseOnError = pauseOnError;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "new \"MyPackage\" --confirm --build-package")]
            [InlineData(false, "new \"MyPackage\" --confirm")]
            public void Should_Add_BuildPackage_Flag_To_Arguments_If_Set(bool buildPackage, string expected)
            {
                // Given
                var fixture = new ChocolateyNewFixture();
                fixture.Settings.BuildPackage = buildPackage;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "new \"MyPackage\" --confirm --from-programs-and-features")]
            [InlineData(false, "new \"MyPackage\" --confirm")]
            public void Should_Add_GeneratePackagesFromInstalledSoftware_Flag_To_Arguments_If_Set(bool generatePackagesFromInstalledSoftware, string expected)
            {
                // Given
                var fixture = new ChocolateyNewFixture();
                fixture.Settings.GeneratePackagesFromInstalledSoftware = generatePackagesFromInstalledSoftware;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "new \"MyPackage\" --confirm --remove-architecture-from-name")]
            [InlineData(false, "new \"MyPackage\" --confirm")]
            public void Should_Add_RemoveArchitectureFromName_Flag_To_Arguments_If_Set(bool removeArchitectureFromName, string expected)
            {
                // Given
                var fixture = new ChocolateyNewFixture();
                fixture.Settings.RemoveArchitectureFromName = removeArchitectureFromName;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(true, "new \"MyPackage\" --confirm --include-architecture-in-name")]
            [InlineData(false, "new \"MyPackage\" --confirm")]
            public void Should_Add_IncludeArchitectureInPackageName_Flag_To_Arguments_If_Set(bool includeArchitectureInPackageName, string expected)
            {
                // Given
                var fixture = new ChocolateyNewFixture();
                fixture.Settings.IncludeArchitectureInPackageName = includeArchitectureInPackageName;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }
        }
    }
}