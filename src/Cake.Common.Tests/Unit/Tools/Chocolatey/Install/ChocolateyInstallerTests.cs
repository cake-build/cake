using Cake.Common.Tests.Fixtures.Tools.Chocolatey;
using Cake.Core.IO;
using NSubstitute;
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
                var fixture = new ChocolateyInstallerFixture();
                fixture.PackageId = null;

                // When
                var result = Record.Exception(() => fixture.Install());

                // Then
                Assert.IsArgumentNullException(result, "packageId");
            }

            [Fact]
            public void Should_Throw_If_Settings_Are_Null()
            {
                // Given
                var fixture = new ChocolateyInstallerFixture();
                fixture.Settings = null;

                // When
                var result = Record.Exception(() => fixture.Install());

                // Then
                Assert.IsArgumentNullException(result, "settings");
            }

            [Fact]
            public void Should_Throw_If_Chocolatey_Executable_Was_Not_Found()
            {
                // Given
                var fixture = new ChocolateyInstallerFixture();
                fixture.GivenDefaultToolDoNotExist();

                // When
                var result = Record.Exception(() => fixture.Install());

                // Then
                Assert.IsCakeException(result, "Chocolatey: Could not locate executable.");
            }

            [Theory]
            [InlineData("C:/ProgramData/chocolatey/choco.exe", "C:/ProgramData/chocolatey/choco.exe")]
            public void Should_Use_Chocolatey_Executable_From_Tool_Path_If_Provided(string toolPath, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallerFixture();
                fixture.GivenCustomToolPathExist(expected);
                fixture.Settings.ToolPath = toolPath;

                // When
                fixture.Install();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Is<FilePath>(p => p.FullPath == expected),
                    Arg.Any<ProcessSettings>());
            }

            [Fact]
            public void Should_Throw_If_Process_Was_Not_Started()
            {
                // Given
                var fixture = new ChocolateyInstallerFixture();
                fixture.GivenProcessCannotStart();

                // When
                var result = Record.Exception(() => fixture.Install());

                // Then
                Assert.IsCakeException(result, "Chocolatey: Process was not started.");
            }

            [Fact]
            public void Should_Throw_If_Process_Has_A_Non_Zero_Exit_Code()
            {
                // Given
                var fixture = new ChocolateyInstallerFixture();
                fixture.GivenProcessReturnError();

                // When
                var result = Record.Exception(() => fixture.Install());

                // Then
                Assert.IsCakeException(result, "Chocolatey: Process returned an error.");
            }

            [Fact]
            public void Should_Find_Chocolatey_Executable_If_Tool_Path_Not_Provided()
            {
                // Given
                var fixture = new ChocolateyInstallerFixture();

                // When
                fixture.Install();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Is<FilePath>(p => p.FullPath == "/Working/tools/choco.exe"),
                    Arg.Any<ProcessSettings>());
            }

            [Fact]
            public void Should_Add_Mandatory_Arguments()
            {
                // Given
                var fixture = new ChocolateyInstallerFixture();

                // When
                fixture.Install();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() == "install \"Cake\" -y"));
            }

            [Theory]
            [InlineData(true, "install \"Cake\" -d -y")]
            [InlineData(false, "install \"Cake\" -y")]
            public void Should_Add_Debug_Flag_To_Arguments_If_Set(bool debug, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallerFixture();
                fixture.Settings.Debug = debug;

                // When
                fixture.Install();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() == expected));
            }

            [Theory]
            [InlineData(true, "install \"Cake\" -v -y")]
            [InlineData(false, "install \"Cake\" -y")]
            public void Should_Add_Verbose_Flag_To_Arguments_If_Set(bool verbose, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallerFixture();
                fixture.Settings.Verbose = verbose;

                // When
                fixture.Install();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() == expected));
            }

            [Theory]
            [InlineData(true, "install \"Cake\" --acceptLicense -y")]
            [InlineData(false, "install \"Cake\" -y")]
            public void Should_Add_AcceptLicense_Flag_To_Arguments_If_Set(bool acceptLicense, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallerFixture();
                fixture.Settings.AcceptLicense = acceptLicense;

                // When
                fixture.Install();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() == expected));
            }

            [Theory]
            [InlineData(true, "install \"Cake\" -y -f")]
            [InlineData(false, "install \"Cake\" -y")]
            public void Should_Add_Force_Flag_To_Arguments_If_Set(bool force, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallerFixture();
                fixture.Settings.Force = force;

                // When
                fixture.Install();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() == expected));
            }

            [Theory]
            [InlineData(true, "install \"Cake\" -y --noop")]
            [InlineData(false, "install \"Cake\" -y")]
            public void Should_Add_Noop_Flag_To_Arguments_If_Set(bool noop, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallerFixture();
                fixture.Settings.Noop = noop;

                // When
                fixture.Install();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() == expected));
            }

            [Theory]
            [InlineData(true, "install \"Cake\" -y -r")]
            [InlineData(false, "install \"Cake\" -y")]
            public void Should_Add_LimitOutput_Flag_To_Arguments_If_Set(bool limitOutput, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallerFixture();
                fixture.Settings.LimitOutput = limitOutput;

                // When
                fixture.Install();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() == expected));
            }

            [Theory]
            [InlineData(5, "install \"Cake\" -y --execution-timeout \"5\"")]
            [InlineData(0, "install \"Cake\" -y")]
            public void Should_Add_ExecutionTimeout_To_Arguments_If_Set(int executionTimeout, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallerFixture();
                fixture.Settings.ExecutionTimeout = executionTimeout;

                // When
                fixture.Install();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() == expected));
            }

            [Theory]
            [InlineData(@"c:\temp", "install \"Cake\" -y -c \"c:\\temp\"")]
            [InlineData("", "install \"Cake\" -y")]
            public void Should_Add_CacheLocation_Flag_To_Arguments_If_Set(string cacheLocation, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallerFixture();
                fixture.Settings.CacheLocation = cacheLocation;

                // When
                fixture.Install();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() == expected));
            }

            [Theory]
            [InlineData(true, "install \"Cake\" -y --allowunofficial")]
            [InlineData(false, "install \"Cake\" -y")]
            public void Should_Add_AllowUnofficial_Flag_To_Arguments_If_Set(bool allowUnofficial, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallerFixture();
                fixture.Settings.AllowUnoffical = allowUnofficial;

                // When
                fixture.Install();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() == expected));
            }

            [Fact]
            public void Should_Add_Source_To_Arguments_If_Set()
            {
                // Given
                var fixture = new ChocolateyInstallerFixture();
                fixture.Settings.Source = "A";

                // When
                fixture.Install();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() == "install \"Cake\" -y -s \"A\""));
            }

            [Fact]
            public void Should_Add_Version_To_Arguments_If_Not_Null()
            {
                // Given
                var fixture = new ChocolateyInstallerFixture();
                fixture.Settings.Version = "1.0.0";

                // When
                fixture.Install();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(),
                    Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() == "install \"Cake\" -y --version \"1.0.0\""));
            }

            [Theory]
            [InlineData(true, "install \"Cake\" -y --pre")]
            [InlineData(false, "install \"Cake\" -y")]
            public void Should_Add_Prerelease_Flag_To_Arguments_If_Set(bool prerelease, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallerFixture();
                fixture.Settings.Prerelease = prerelease;

                // When
                fixture.Install();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() == expected));
            }

            [Theory]
            [InlineData(true, "install \"Cake\" -y --x86")]
            [InlineData(false, "install \"Cake\" -y")]
            public void Should_Add_Forcex86_Flag_To_Arguments_If_Set(bool forcex86, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallerFixture();
                fixture.Settings.Forcex86 = forcex86;

                // When
                fixture.Install();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() == expected));
            }

            [Theory]
            [InlineData("args1", "install \"Cake\" -y --ia \"args1\"")]
            [InlineData("", "install \"Cake\" -y")]
            public void Should_Add_InstallArguments_To_Arguments_If_Set(string installArgs, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallerFixture();
                fixture.Settings.InstallArguments = installArgs;

                // When
                fixture.Install();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() == expected));
            }

            [Theory]
            [InlineData(true, "install \"Cake\" -y -o")]
            [InlineData(false, "install \"Cake\" -y")]
            public void Should_Add_OverrideArguments_Flag_To_Arguments_If_Set(bool overrideArguments, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallerFixture();
                fixture.Settings.OverrideArguments = overrideArguments;

                // When
                fixture.Install();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() == expected));
            }

            [Theory]
            [InlineData(true, "install \"Cake\" -y --notSilent")]
            [InlineData(false, "install \"Cake\" -y")]
            public void Should_Add_NotSilent_Flag_To_Arguments_If_Set(bool notSilent, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallerFixture();
                fixture.Settings.NotSilent = notSilent;

                // When
                fixture.Install();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() == expected));
            }

            [Theory]
            [InlineData("param1", "install \"Cake\" -y --params \"param1\"")]
            [InlineData("", "install \"Cake\" -y")]
            public void Should_Add_PackageParameters_To_Arguments_If_Set(string packageParamters, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallerFixture();
                fixture.Settings.PackageParameters = packageParamters;

                // When
                fixture.Install();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() == expected));
            }

            [Theory]
            [InlineData(true, "install \"Cake\" -y --allowdowngrade")]
            [InlineData(false, "install \"Cake\" -y")]
            public void Should_Add_AllowDowngrade_Flag_To_Arguments_If_Set(bool allowDowngrade, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallerFixture();
                fixture.Settings.AllowDowngrade = allowDowngrade;

                // When
                fixture.Install();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() == expected));
            }

            [Theory]
            [InlineData(true, "install \"Cake\" -y -m")]
            [InlineData(false, "install \"Cake\" -y")]
            public void Should_Add_SideBySide_Flag_To_Arguments_If_Set(bool sideBySide, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallerFixture();
                fixture.Settings.SideBySide = sideBySide;

                // When
                fixture.Install();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() == expected));
            }

            [Theory]
            [InlineData(true, "install \"Cake\" -y -i")]
            [InlineData(false, "install \"Cake\" -y")]
            public void Should_Add_IgnoreDependencies_Flag_To_Arguments_If_Set(bool ignoreDependencies, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallerFixture();
                fixture.Settings.IgnoreDependencies = ignoreDependencies;

                // When
                fixture.Install();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() == expected));
            }

            [Theory]
            [InlineData(true, "install \"Cake\" -y -x")]
            [InlineData(false, "install \"Cake\" -y")]
            public void Should_Add_ForceDependencies_Flag_To_Arguments_If_Set(bool forceDependencies, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallerFixture();
                fixture.Settings.ForceDependencies = forceDependencies;

                // When
                fixture.Install();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() == expected));
            }

            [Theory]
            [InlineData(true, "install \"Cake\" -y -n")]
            [InlineData(false, "install \"Cake\" -y")]
            public void Should_Add_SkipPowerShell_Flag_To_Arguments_If_Set(bool skipPowerShell, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallerFixture();
                fixture.Settings.SkipPowerShell = skipPowerShell;

                // When
                fixture.Install();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() == expected));
            }

            [Theory]
            [InlineData("user1", "install \"Cake\" -y -u \"user1\"")]
            [InlineData("", "install \"Cake\" -y")]
            public void Should_Add_User_To_Arguments_If_Set(string user, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallerFixture();
                fixture.Settings.User = user;

                // When
                fixture.Install();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() == expected));
            }

            [Theory]
            [InlineData("password1", "install \"Cake\" -y -p \"password1\"")]
            [InlineData("", "install \"Cake\" -y")]
            public void Should_Add_Password_To_Arguments_If_Set(string password, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallerFixture();
                fixture.Settings.Password = password;

                // When
                fixture.Install();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() == expected));
            }

            [Theory]
            [InlineData(true, "install \"Cake\" -y --ignorechecksums")]
            [InlineData(false, "install \"Cake\" -y")]
            public void Should_Add_IgnoreChecksums_Flag_To_Arguments_If_Set(bool ignoreChecksums, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallerFixture();
                fixture.Settings.IgnoreChecksums = ignoreChecksums;

                // When
                fixture.Install();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() == expected));
            }
        }

        public sealed class TheInstallFromConfigMethod
        {
            [Fact]
            public void Should_Throw_If_Target_Package_Config_Path_Is_Null()
            {
                // Given
                var fixture = new ChocolateyInstallerFixture();
                fixture.PackageConfigPath = null;

                // When
                var result = Record.Exception(() => fixture.InstallFromConfig());

                // Then
                Assert.IsArgumentNullException(result, "packageConfigPath");
            }

            [Fact]
            public void Should_Throw_If_Settings_Are_Null()
            {
                // Given
                var fixture = new ChocolateyInstallerFixture();
                fixture.Settings = null;

                // When
                var result = Record.Exception(() => fixture.InstallFromConfig());

                // Then
                Assert.IsArgumentNullException(result, "settings");
            }

            [Fact]
            public void Should_Throw_If_Chocolatey_Executable_Was_Not_Found()
            {
                // Given
                var fixture = new ChocolateyInstallerFixture();
                fixture.GivenDefaultToolDoNotExist();

                // When
                var result = Record.Exception(() => fixture.InstallFromConfig());

                // Then
                Assert.IsCakeException(result, "Chocolatey: Could not locate executable.");
            }

            [Theory]
            [InlineData("C:/ProgramData/chocolatey/choco.exe", "C:/ProgramData/chocolatey/choco.exe")]
            public void Should_Use_Chocolatey_Executable_From_Tool_Path_If_Provided(string toolPath, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallerFixture();
                fixture.Settings.ToolPath = toolPath;
                fixture.GivenCustomToolPathExist(expected);

                // When
                fixture.InstallFromConfig();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Is<FilePath>(p => p.FullPath == expected),
                    Arg.Any<ProcessSettings>());
            }

            [Fact]
            public void Should_Throw_If_Process_Was_Not_Started()
            {
                // Given
                var fixture = new ChocolateyInstallerFixture();
                fixture.GivenProcessCannotStart();

                // When
                var result = Record.Exception(() => fixture.InstallFromConfig());

                // Then
                Assert.IsCakeException(result, "Chocolatey: Process was not started.");
            }

            [Fact]
            public void Should_Throw_If_Process_Has_A_Non_Zero_Exit_Code()
            {
                // Given
                var fixture = new ChocolateyInstallerFixture();
                fixture.GivenProcessReturnError();

                // When
                var result = Record.Exception(() => fixture.InstallFromConfig());

                // Then
                Assert.IsCakeException(result, "Chocolatey: Process returned an error.");
            }

            [Fact]
            public void Should_Find_Chocolatey_Executable_If_Tool_Path_Not_Provided()
            {
                // Given
                var fixture = new ChocolateyInstallerFixture();

                // When
                fixture.InstallFromConfig();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Is<FilePath>(p => p.FullPath == "/Working/tools/choco.exe"),
                    Arg.Any<ProcessSettings>());
            }

            [Fact]
            public void Should_Add_Mandatory_Arguments()
            {
                // Given
                var fixture = new ChocolateyInstallerFixture();

                // When
                fixture.InstallFromConfig();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() == "install \"/Working/packages.config\" -y"));
            }

            [Theory]
            [InlineData(true, "install \"/Working/packages.config\" -d -y")]
            [InlineData(false, "install \"/Working/packages.config\" -y")]
            public void Should_Add_Debug_Flag_To_Arguments_If_Set(bool debug, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallerFixture();
                fixture.Settings.Debug = debug;

                // When
                fixture.InstallFromConfig();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() == expected));
            }

            [Theory]
            [InlineData(true, "install \"/Working/packages.config\" -v -y")]
            [InlineData(false, "install \"/Working/packages.config\" -y")]
            public void Should_Add_Verbose_Flag_To_Arguments_If_Set(bool verbose, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallerFixture();
                fixture.Settings.Verbose = verbose;

                // When
                fixture.InstallFromConfig();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() == expected));
            }

            [Theory]
            [InlineData(true, "install \"/Working/packages.config\" -y -f")]
            [InlineData(false, "install \"/Working/packages.config\" -y")]
            public void Should_Add_Force_Flag_To_Arguments_If_Set(bool force, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallerFixture();
                fixture.Settings.Force = force;

                // When
                fixture.InstallFromConfig();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() == expected));
            }

            [Theory]
            [InlineData(true, "install \"/Working/packages.config\" -y --noop")]
            [InlineData(false, "install \"/Working/packages.config\" -y")]
            public void Should_Add_Noop_Flag_To_Arguments_If_Set(bool noop, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallerFixture();
                fixture.Settings.Noop = noop;

                // When
                fixture.InstallFromConfig();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() == expected));
            }

            [Theory]
            [InlineData(true, "install \"/Working/packages.config\" -y -r")]
            [InlineData(false, "install \"/Working/packages.config\" -y")]
            public void Should_Add_LimitOutput_Flag_To_Arguments_If_Set(bool limitOutput, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallerFixture();
                fixture.Settings.LimitOutput = limitOutput;

                // When
                fixture.InstallFromConfig();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() == expected));
            }

            [Theory]
            [InlineData(5, "install \"/Working/packages.config\" -y --execution-timeout \"5\"")]
            [InlineData(0, "install \"/Working/packages.config\" -y")]
            public void Should_Add_ExecutionTimeout_To_Arguments_If_Set(int executionTimeout, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallerFixture();
                fixture.Settings.ExecutionTimeout = executionTimeout;

                // When
                fixture.InstallFromConfig();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() == expected));
            }

            [Theory]
            [InlineData(@"c:\temp", "install \"/Working/packages.config\" -y -c \"c:\\temp\"")]
            [InlineData("", "install \"/Working/packages.config\" -y")]
            public void Should_Add_CacheLocation_Flag_To_Arguments_If_Set(string cacheLocation, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallerFixture();
                fixture.Settings.CacheLocation = cacheLocation;

                // When
                fixture.InstallFromConfig();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() == expected));
            }

            [Theory]
            [InlineData(true, "install \"/Working/packages.config\" -y --allowunofficial")]
            [InlineData(false, "install \"/Working/packages.config\" -y")]
            public void Should_Add_AllowUnofficial_Flag_To_Arguments_If_Set(bool allowUnofficial, string expected)
            {
                // Given
                var fixture = new ChocolateyInstallerFixture();
                fixture.Settings.AllowUnoffical = allowUnofficial;

                // When
                fixture.InstallFromConfig();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() == expected));
            }

            [Fact]
            public void Should_Add_Source_To_Arguments_If_Set()
            {
                // Given
                var fixture = new ChocolateyInstallerFixture();
                fixture.Settings.Source = "A";

                // When
                fixture.InstallFromConfig();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() == "install \"/Working/packages.config\" -y -s \"A\""));
            }
        }
    }
}