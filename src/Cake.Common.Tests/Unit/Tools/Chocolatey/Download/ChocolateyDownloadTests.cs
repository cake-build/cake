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
                Assert.Equal("download \"MyPackage\" -y", result.Args);
            }

            [Theory]
            [InlineData(true, "download \"MyPackage\" -d -y")]
            [InlineData(false, "download \"MyPackage\" -y")]
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
            [InlineData(true, "download \"MyPackage\" -v -y")]
            [InlineData(false, "download \"MyPackage\" -y")]
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
            [InlineData(true, "download \"MyPackage\" --acceptLicense -y")]
            [InlineData(false, "download \"MyPackage\" -y")]
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
            [InlineData(true, "download \"MyPackage\" -y -f")]
            [InlineData(false, "download \"MyPackage\" -y")]
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
            [InlineData(true, "download \"MyPackage\" -y --noop")]
            [InlineData(false, "download \"MyPackage\" -y")]
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
            [InlineData(true, "download \"MyPackage\" -y -r")]
            [InlineData(false, "download \"MyPackage\" -y")]
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
            [InlineData(5, "download \"MyPackage\" -y --execution-timeout \"5\"")]
            [InlineData(0, "download \"MyPackage\" -y")]
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
            [InlineData(@"c:\temp", "download \"MyPackage\" -y -c \"c:\\temp\"")]
            [InlineData("", "download \"MyPackage\" -y")]
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
            [InlineData(true, "download \"MyPackage\" -y --allowunofficial")]
            [InlineData(false, "download \"MyPackage\" -y")]
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

            [Fact]
            public void Should_Add_Source_To_Arguments_If_Set()
            {
                // Given
                var fixture = new ChocolateyDownloadFixture();
                fixture.Settings.Source = "A";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("download \"MyPackage\" -y -s \"A\"", result.Args);
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
                Assert.Equal("download \"MyPackage\" -y --version \"1.0.0\"", result.Args);
            }

            [Theory]
            [InlineData(true, "download \"MyPackage\" -y --pre")]
            [InlineData(false, "download \"MyPackage\" -y")]
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

            [Fact]
            public void Should_Add_Output_Directory_To_Arguments_If_Not_Null()
            {
                // Given
                var fixture = new ChocolateyDownloadFixture();
                fixture.Settings.OutputDirectory = "./Foo";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("download \"MyPackage\" -y --outputdirectory \"/Working/Foo\"", result.Args);
            }

            [Theory]
            [InlineData(true, "download \"MyPackage\" -y -i")]
            [InlineData(false, "download \"MyPackage\" -y")]
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
            [InlineData(true, "download \"MyPackage\" -y --internalize")]
            [InlineData(false, "download \"MyPackage\" -y")]
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
            [InlineData(true, "download \"MyPackage\" -y --internalize --internalize-all-urls")]
            [InlineData(false, "download \"MyPackage\" -y --internalize")]
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
                Assert.Equal("download \"MyPackage\" -y", result.Args);
            }

            [Theory]
            [InlineData(@"\\foo", "download \"MyPackage\" -y --internalize --resources-location=\"\\\\foo\"")]
            [InlineData("https://foo.local", "download \"MyPackage\" -y --internalize --resources-location=\"https://foo.local\"")]
            [InlineData(null, "download \"MyPackage\" -y --internalize")]
            [InlineData("", "download \"MyPackage\" -y --internalize")]
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
                Assert.Equal("download \"MyPackage\" -y", result.Args);
            }

            [Theory]
            [InlineData(@"\\foo", "download \"MyPackage\" -y --internalize --resources-location=\"\\\\resources\" --download-location=\"\\\\foo\"")]
            [InlineData("https://foo.local", "download \"MyPackage\" -y --internalize --resources-location=\"\\\\resources\" --download-location=\"https://foo.local\"")]
            [InlineData(null, "download \"MyPackage\" -y --internalize --resources-location=\"\\\\resources\"")]
            [InlineData("", "download \"MyPackage\" -y --internalize --resources-location=\"\\\\resources\"")]
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
                Assert.Equal("download \"MyPackage\" -y", result.Args);
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
                Assert.Equal("download \"MyPackage\" -y --internalize", result.Args);
            }

            [Theory]
            [InlineData(true, "download \"MyPackage\" -y --internalize --append-useoriginallocation")]
            [InlineData(false, "download \"MyPackage\" -y --internalize")]
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
                Assert.Equal("download \"MyPackage\" -y", result.Args);
            }

            [Theory]
            [InlineData("user1", "download \"MyPackage\" -y -u \"user1\"")]
            [InlineData("", "download \"MyPackage\" -y")]
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
            [InlineData("password1", "download \"MyPackage\" -y -p \"password1\"")]
            [InlineData("", "download \"MyPackage\" -y")]
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
        }
    }
}