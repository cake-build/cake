using Cake.Common.Tests.Fixtures.Tools.Chocolatey;
using Cake.Core.IO;
using NSubstitute;
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
                var result = Record.Exception(() => fixture.Upgrade());

                // Then
                Assert.IsArgumentNullException(result, "packageId");
            }

            [Fact]
            public void Should_Throw_If_Settings_Are_Null()
            {
                // Given
                var fixture = new ChocolateyUpgraderFixture();
                fixture.Settings = null;

                // When
                var result = Record.Exception(() => fixture.Upgrade());

                // Then
                Assert.IsArgumentNullException(result, "settings");
            }

            [Fact]
            public void Should_Throw_If_Chocolatey_Executable_Was_Not_Found()
            {
                // Given
                var fixture = new ChocolateyUpgraderFixture();
                fixture.GivenDefaultToolDoNotExist();

                // When
                var result = Record.Exception(() => fixture.Upgrade());

                // Then
                Assert.IsCakeException(result, "Chocolatey: Could not locate executable.");
            }

            [Theory]
            [InlineData("C:/ProgramData/chocolatey/choco.exe", "C:/ProgramData/chocolatey/choco.exe")]
            public void Should_Use_Chocolatey_Executable_From_Tool_Path_If_Provided(string toolPath, string expected)
            {
                // Given
                var fixture = new ChocolateyUpgraderFixture();
                fixture.GivenCustomToolPathExist(expected);
                fixture.Settings.ToolPath = toolPath;

                // When
                fixture.Upgrade();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Is<FilePath>(p => p.FullPath == expected),
                    Arg.Any<ProcessSettings>());
            }

            [Fact]
            public void Should_Throw_If_Process_Was_Not_Started()
            {
                // Given
                var fixture = new ChocolateyUpgraderFixture();
                fixture.GivenProcessCannotStart();

                // When
                var result = Record.Exception(() => fixture.Upgrade());

                // Then
                Assert.IsCakeException(result, "Chocolatey: Process was not started.");
            }

            [Fact]
            public void Should_Throw_If_Process_Has_A_Non_Zero_Exit_Code()
            {
                // Given
                var fixture = new ChocolateyUpgraderFixture();
                fixture.GivenProcessReturnError();

                // When
                var result = Record.Exception(() => fixture.Upgrade());

                // Then
                Assert.IsCakeException(result, "Chocolatey: Process returned an error.");
            }

            [Fact]
            public void Should_Find_Chocolatey_Executable_If_Tool_Path_Not_Provided()
            {
                // Given
                var fixture = new ChocolateyUpgraderFixture();

                // When
                fixture.Upgrade();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Is<FilePath>(p => p.FullPath == "/Working/tools/choco.exe"),
                    Arg.Any<ProcessSettings>());
            }

            [Fact]
            public void Should_Add_Mandatory_Arguments()
            {
                // Given
                var fixture = new ChocolateyUpgraderFixture();

                // When
                fixture.Upgrade();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() == "upgrade \"Cake\" -y"));
            }

            [Theory]
            [InlineData(true, "upgrade \"Cake\" -d -y")]
            [InlineData(false, "upgrade \"Cake\" -y")]
            public void Should_Add_Debug_Flag_To_Arguments_If_Set(bool debug, string expected)
            {
                // Given
                var fixture = new ChocolateyUpgraderFixture();
                fixture.Settings.Debug = debug;

                // When
                fixture.Upgrade();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() == expected));
            }

            [Theory]
            [InlineData(true, "upgrade \"Cake\" -v -y")]
            [InlineData(false, "upgrade \"Cake\" -y")]
            public void Should_Add_Verbose_Flag_To_Arguments_If_Set(bool verbose, string expected)
            {
                // Given
                var fixture = new ChocolateyUpgraderFixture();
                fixture.Settings.Verbose = verbose;

                // When
                fixture.Upgrade();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() == expected));
            }

            [Theory]
            [InlineData(true, "upgrade \"Cake\" --acceptLicense -y")]
            [InlineData(false, "upgrade \"Cake\" -y")]
            public void Should_Add_AcceptLicense_Flag_To_Arguments_If_Set(bool acceptLicense, string expected)
            {
                // Given
                var fixture = new ChocolateyUpgraderFixture();
                fixture.Settings.AcceptLicense = acceptLicense;

                // When
                fixture.Upgrade();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() == expected));
            }

            [Theory]
            [InlineData(true, "upgrade \"Cake\" -y -f")]
            [InlineData(false, "upgrade \"Cake\" -y")]
            public void Should_Add_Force_Flag_To_Arguments_If_Set(bool force, string expected)
            {
                // Given
                var fixture = new ChocolateyUpgraderFixture();
                fixture.Settings.Force = force;

                // When
                fixture.Upgrade();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() == expected));
            }

            [Theory]
            [InlineData(true, "upgrade \"Cake\" -y --noop")]
            [InlineData(false, "upgrade \"Cake\" -y")]
            public void Should_Add_Noop_Flag_To_Arguments_If_Set(bool noop, string expected)
            {
                // Given
                var fixture = new ChocolateyUpgraderFixture();
                fixture.Settings.Noop = noop;

                // When
                fixture.Upgrade();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() == expected));
            }

            [Theory]
            [InlineData(true, "upgrade \"Cake\" -y -r")]
            [InlineData(false, "upgrade \"Cake\" -y")]
            public void Should_Add_LimitOutput_Flag_To_Arguments_If_Set(bool limitOutput, string expected)
            {
                // Given
                var fixture = new ChocolateyUpgraderFixture();
                fixture.Settings.LimitOutput = limitOutput;

                // When
                fixture.Upgrade();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() == expected));
            }

            [Theory]
            [InlineData(5, "upgrade \"Cake\" -y --execution-timeout \"5\"")]
            [InlineData(0, "upgrade \"Cake\" -y")]
            public void Should_Add_ExecutionTimeout_To_Arguments_If_Set(int executionTimeout, string expected)
            {
                // Given
                var fixture = new ChocolateyUpgraderFixture();
                fixture.Settings.ExecutionTimeout = executionTimeout;

                // When
                fixture.Upgrade();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() == expected));
            }

            [Theory]
            [InlineData(@"c:\temp", "upgrade \"Cake\" -y -c \"c:\\temp\"")]
            [InlineData("", "upgrade \"Cake\" -y")]
            public void Should_Add_CacheLocation_Flag_To_Arguments_If_Set(string cacheLocation, string expected)
            {
                // Given
                var fixture = new ChocolateyUpgraderFixture();
                fixture.Settings.CacheLocation = cacheLocation;

                // When
                fixture.Upgrade();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() == expected));
            }

            [Theory]
            [InlineData(true, "upgrade \"Cake\" -y --allowunofficial")]
            [InlineData(false, "upgrade \"Cake\" -y")]
            public void Should_Add_AllowUnofficial_Flag_To_Arguments_If_Set(bool allowUnofficial, string expected)
            {
                // Given
                var fixture = new ChocolateyUpgraderFixture();
                fixture.Settings.AllowUnoffical = allowUnofficial;

                // When
                fixture.Upgrade();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() == expected));
            }

            [Fact]
            public void Should_Add_Source_To_Arguments_If_Set()
            {
                // Given
                var fixture = new ChocolateyUpgraderFixture();
                fixture.Settings.Source = "A";

                // When
                fixture.Upgrade();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() == "upgrade \"Cake\" -y -s \"A\""));
            }

            [Fact]
            public void Should_Add_Version_To_Arguments_If_Not_Null()
            {
                // Given
                var fixture = new ChocolateyUpgraderFixture();
                fixture.Settings.Version = "1.0.0";

                // When
                fixture.Upgrade();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(),
                    Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() == "upgrade \"Cake\" -y --version \"1.0.0\""));
            }

            [Theory]
            [InlineData(true, "upgrade \"Cake\" -y --pre")]
            [InlineData(false, "upgrade \"Cake\" -y")]
            public void Should_Add_Prerelease_Flag_To_Arguments_If_Set(bool prerelease, string expected)
            {
                // Given
                var fixture = new ChocolateyUpgraderFixture();
                fixture.Settings.Prerelease = prerelease;

                // When
                fixture.Upgrade();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() == expected));
            }

            [Theory]
            [InlineData(true, "upgrade \"Cake\" -y --x86")]
            [InlineData(false, "upgrade \"Cake\" -y")]
            public void Should_Add_Forcex86_Flag_To_Arguments_If_Set(bool forcex86, string expected)
            {
                // Given
                var fixture = new ChocolateyUpgraderFixture();
                fixture.Settings.Forcex86 = forcex86;

                // When
                fixture.Upgrade();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() == expected));
            }

            [Theory]
            [InlineData("args1", "upgrade \"Cake\" -y --ia \"args1\"")]
            [InlineData("", "upgrade \"Cake\" -y")]
            public void Should_Add_InstallArguments_To_Arguments_If_Set(string installArgs, string expected)
            {
                // Given
                var fixture = new ChocolateyUpgraderFixture();
                fixture.Settings.InstallArguments = installArgs;

                // When
                fixture.Upgrade();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() == expected));
            }

            [Theory]
            [InlineData(true, "upgrade \"Cake\" -y -o")]
            [InlineData(false, "upgrade \"Cake\" -y")]
            public void Should_Add_OverrideArguments_Flag_To_Arguments_If_Set(bool overrideArguments, string expected)
            {
                // Given
                var fixture = new ChocolateyUpgraderFixture();
                fixture.Settings.OverrideArguments = overrideArguments;

                // When
                fixture.Upgrade();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() == expected));
            }

            [Theory]
            [InlineData(true, "upgrade \"Cake\" -y --notSilent")]
            [InlineData(false, "upgrade \"Cake\" -y")]
            public void Should_Add_NotSilent_Flag_To_Arguments_If_Set(bool notSilent, string expected)
            {
                // Given
                var fixture = new ChocolateyUpgraderFixture();
                fixture.Settings.NotSilent = notSilent;

                // When
                fixture.Upgrade();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() == expected));
            }

            [Theory]
            [InlineData("param1", "upgrade \"Cake\" -y --params \"param1\"")]
            [InlineData("", "upgrade \"Cake\" -y")]
            public void Should_Add_PackageParameters_To_Arguments_If_Set(string packageParamters, string expected)
            {
                // Given
                var fixture = new ChocolateyUpgraderFixture();
                fixture.Settings.PackageParameters = packageParamters;

                // When
                fixture.Upgrade();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() == expected));
            }

            [Theory]
            [InlineData(true, "upgrade \"Cake\" -y --allowdowngrade")]
            [InlineData(false, "upgrade \"Cake\" -y")]
            public void Should_Add_AllowDowngrade_Flag_To_Arguments_If_Set(bool allowDowngrade, string expected)
            {
                // Given
                var fixture = new ChocolateyUpgraderFixture();
                fixture.Settings.AllowDowngrade = allowDowngrade;

                // When
                fixture.Upgrade();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() == expected));
            }

            [Theory]
            [InlineData(true, "upgrade \"Cake\" -y -m")]
            [InlineData(false, "upgrade \"Cake\" -y")]
            public void Should_Add_SideBySide_Flag_To_Arguments_If_Set(bool sideBySide, string expected)
            {
                // Given
                var fixture = new ChocolateyUpgraderFixture();
                fixture.Settings.SideBySide = sideBySide;

                // When
                fixture.Upgrade();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() == expected));
            }

            [Theory]
            [InlineData(true, "upgrade \"Cake\" -y -i")]
            [InlineData(false, "upgrade \"Cake\" -y")]
            public void Should_Add_IgnoreDependencies_Flag_To_Arguments_If_Set(bool ignoreDependencies, string expected)
            {
                // Given
                var fixture = new ChocolateyUpgraderFixture();
                fixture.Settings.IgnoreDependencies = ignoreDependencies;

                // When
                fixture.Upgrade();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() == expected));
            }

            [Theory]
            [InlineData(true, "upgrade \"Cake\" -y -n")]
            [InlineData(false, "upgrade \"Cake\" -y")]
            public void Should_Add_SkipPowerShell_Flag_To_Arguments_If_Set(bool skipPowerShell, string expected)
            {
                // Given
                var fixture = new ChocolateyUpgraderFixture();
                fixture.Settings.SkipPowerShell = skipPowerShell;

                // When
                fixture.Upgrade();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() == expected));
            }

            [Theory]
            [InlineData(true, "upgrade \"Cake\" -y --failonunfound")]
            [InlineData(false, "upgrade \"Cake\" -y")]
            public void Should_Add_FailOnUnfound_Flag_To_Arguments_If_Set(bool failOnUnfound, string expected)
            {
                // Given
                var fixture = new ChocolateyUpgraderFixture();
                fixture.Settings.FailOnUnfound = failOnUnfound;

                // When
                fixture.Upgrade();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() == expected));
            }

            [Theory]
            [InlineData(true, "upgrade \"Cake\" -y --failonnotinstalled")]
            [InlineData(false, "upgrade \"Cake\" -y")]
            public void Should_Add_FailOnNotInstalled_Flag_To_Arguments_If_Set(bool failOnNotInstalled, string expected)
            {
                // Given
                var fixture = new ChocolateyUpgraderFixture();
                fixture.Settings.FailOnNotInstalled = failOnNotInstalled;

                // When
                fixture.Upgrade();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() == expected));
            }

            [Theory]
            [InlineData("user1", "upgrade \"Cake\" -y -u \"user1\"")]
            [InlineData("", "upgrade \"Cake\" -y")]
            public void Should_Add_User_To_Arguments_If_Set(string user, string expected)
            {
                // Given
                var fixture = new ChocolateyUpgraderFixture();
                fixture.Settings.User = user;

                // When
                fixture.Upgrade();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() == expected));
            }

            [Theory]
            [InlineData("password1", "upgrade \"Cake\" -y -p \"password1\"")]
            [InlineData("", "upgrade \"Cake\" -y")]
            public void Should_Add_Password_To_Arguments_If_Set(string password, string expected)
            {
                // Given
                var fixture = new ChocolateyUpgraderFixture();
                fixture.Settings.Password = password;

                // When
                fixture.Upgrade();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() == expected));
            }

            [Theory]
            [InlineData(true, "upgrade \"Cake\" -y --ignorechecksums")]
            [InlineData(false, "upgrade \"Cake\" -y")]
            public void Should_Add_IgnoreChecksums_Flag_To_Arguments_If_Set(bool ignoreChecksums, string expected)
            {
                // Given
                var fixture = new ChocolateyUpgraderFixture();
                fixture.Settings.IgnoreChecksums = ignoreChecksums;

                // When
                fixture.Upgrade();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() == expected));
            }
        }
    }
}