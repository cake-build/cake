using Cake.Common.Tests.Fixtures.Tools.NuGet;
using Cake.Common.Tools.NuGet;
using Cake.Core.IO;
using NSubstitute;
using Xunit;

namespace Cake.Common.Tests.Unit.Tools.NuGet.Install
{
    public sealed class NuGetInstallerTests
    {
        public sealed class TheInstallMethod
        {
            [Fact]
            public void Should_Throw_If_Target_Package_Id_Is_Null()
            {
                // Given
                var fixture = new NuGetInstallerFixture();
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
                var fixture = new NuGetInstallerFixture();
                fixture.Settings = null;

                // When
                var result = Record.Exception(() => fixture.Install());

                // Then
                Assert.IsArgumentNullException(result, "settings");
            }

            [Fact]
            public void Should_Throw_If_NuGet_Executable_Was_Not_Found()
            {
                // Given
                var fixture = new NuGetInstallerFixture();
                fixture.GivenDefaultToolDoNotExist();

                // When
                var result = Record.Exception(() => fixture.Install());

                // Then
                Assert.IsCakeException(result, "NuGet: Could not locate executable.");
            }

            [Theory]
            [InlineData("C:/nuget/nuget.exe", "C:/nuget/nuget.exe")]
            [InlineData("./tools/nuget/nuget.exe", "/Working/tools/nuget/nuget.exe")]
            public void Should_Use_NuGet_Executable_From_Tool_Path_If_Provided(string toolPath, string expected)
            {
                // Given
                var fixture = new NuGetInstallerFixture();
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
                var fixture = new NuGetInstallerFixture();
                fixture.GivenProcessCannotStart();

                // When
                var result = Record.Exception(() => fixture.Install());

                // Then
                Assert.IsCakeException(result, "NuGet: Process was not started.");
            }

            [Fact]
            public void Should_Throw_If_Process_Has_A_Non_Zero_Exit_Code()
            {
                // Given
                var fixture = new NuGetInstallerFixture();
                fixture.GivenProcessReturnError();

                // When
                var result = Record.Exception(() => fixture.Install());

                // Then
                Assert.IsCakeException(result, "NuGet: Process returned an error.");
            }

            [Fact]
            public void Should_Find_NuGet_Executable_If_Tool_Path_Not_Provided()
            {
                // Given
                var fixture = new NuGetInstallerFixture();

                // When
                fixture.Install();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Is<FilePath>(p => p.FullPath == "/Working/tools/NuGet.exe"),
                    Arg.Any<ProcessSettings>());
            }

            [Fact]
            public void Should_Add_Mandatory_Arguments()
            {
                // Given
                var fixture = new NuGetInstallerFixture();

                // When
                fixture.Install();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() == "install \"Cake\" -NonInteractive"));
            }

            [Fact]
            public void Should_Add_RequireConsent_To_Arguments_If_True()
            {
                // Given
                var fixture = new NuGetInstallerFixture();
                fixture.Settings.RequireConsent = true;

                // When
                fixture.Install();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() == "install \"Cake\" -RequireConsent -NonInteractive"));
            }

            [Fact]
            public void Should_Add_SolutionDirectory_To_Arguments_If_Set()
            {
                // Given
                var fixture = new NuGetInstallerFixture();
                fixture.Settings.SolutionDirectory = "./solution";

                // When
                fixture.Install();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() == "install \"Cake\" -SolutionDirectory \"/Working/solution\" -NonInteractive"));
            }

            [Fact]
            public void Should_Add_Sources_To_Arguments_If_Set()
            {
                // Given
                var fixture = new NuGetInstallerFixture();
                fixture.Settings.Source = new[] { "A;B;C" };

                // When
                fixture.Install();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() == "install \"Cake\" -Source \"A;B;C\" -NonInteractive"));
            }

            [Fact]
            public void Should_Add_NoCache_To_Arguments_If_True()
            {
                // Given
                var fixture = new NuGetInstallerFixture();
                fixture.Settings.NoCache = true;

                // When
                fixture.Install();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() == "install \"Cake\" -NoCache -NonInteractive"));
            }

            [Fact]
            public void Should_Add_DisableParallelProcessing_To_Arguments_If_Set()
            {
                // Given
                var fixture = new NuGetInstallerFixture();
                fixture.Settings.DisableParallelProcessing = true;

                // When
                fixture.Install();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() == "install \"Cake\" -DisableParallelProcessing -NonInteractive"));
            }

            [Theory]
            [InlineData(NuGetVerbosity.Detailed, "install \"Cake\" -Verbosity detailed -NonInteractive")]
            [InlineData(NuGetVerbosity.Normal, "install \"Cake\" -Verbosity normal -NonInteractive")]
            [InlineData(NuGetVerbosity.Quiet, "install \"Cake\" -Verbosity quiet -NonInteractive")]
            public void Should_Add_Verbosity_To_Arguments_If_Set(NuGetVerbosity verbosity, string expected)
            {
                // Given
                var fixture = new NuGetInstallerFixture();
                fixture.Settings.Verbosity = verbosity;

                // When
                fixture.Install();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() == expected));
            }

            [Fact]
            public void Should_Add_ConfigFile_To_Arguments_If_Set()
            {
                // Given
                var fixture = new NuGetInstallerFixture();
                fixture.Settings.ConfigFile = "./nuget.config";

                // When
                fixture.Install();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() == "install \"Cake\" -ConfigFile \"/Working/nuget.config\" -NonInteractive"));
            }
        }

        public sealed class TheInstallFromConfigMethod
        {
            [Fact]
            public void Should_Throw_If_Target_Package_Config_Path_Is_Null()
            {
                // Given
                var fixture = new NuGetInstallerFixture();
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
                var fixture = new NuGetInstallerFixture();
                fixture.Settings = null;

                // When
                var result = Record.Exception(() => fixture.InstallFromConfig());

                // Then
                Assert.IsArgumentNullException(result, "settings");
            }

            [Fact]
            public void Should_Throw_If_NuGet_Executable_Was_Not_Found()
            {
                // Given
                var fixture = new NuGetInstallerFixture();
                fixture.GivenDefaultToolDoNotExist();

                // When
                var result = Record.Exception(() => fixture.InstallFromConfig());

                // Then
                Assert.IsCakeException(result, "NuGet: Could not locate executable.");
            }

            [Theory]
            [InlineData("C:/nuget/nuget.exe", "C:/nuget/nuget.exe")]
            [InlineData("./tools/nuget/nuget.exe", "/Working/tools/nuget/nuget.exe")]
            public void Should_Use_NuGet_Executable_From_Tool_Path_If_Provided(string toolPath, string expected)
            {
                // Given
                var fixture = new NuGetInstallerFixture();
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
                var fixture = new NuGetInstallerFixture();
                fixture.GivenProcessCannotStart();

                // When
                var result = Record.Exception(() => fixture.InstallFromConfig());

                // Then
                Assert.IsCakeException(result, "NuGet: Process was not started.");
            }

            [Fact]
            public void Should_Throw_If_Process_Has_A_Non_Zero_Exit_Code()
            {
                // Given
                var fixture = new NuGetInstallerFixture();
                fixture.GivenProcessReturnError();

                // When
                var result = Record.Exception(() => fixture.InstallFromConfig());

                // Then
                Assert.IsCakeException(result, "NuGet: Process returned an error.");
            }

            [Fact]
            public void Should_Find_NuGet_Executable_If_Tool_Path_Not_Provided()
            {
                // Given
                var fixture = new NuGetInstallerFixture();

                // When
                fixture.InstallFromConfig();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Is<FilePath>(p => p.FullPath == "/Working/tools/NuGet.exe"),
                    Arg.Any<ProcessSettings>());
            }

            [Fact]
            public void Should_Add_Mandatory_Arguments()
            {
                // Given
                var fixture = new NuGetInstallerFixture();

                // When
                fixture.InstallFromConfig();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() == "install \"/Working/packages.config\" -NonInteractive"));
            }

            [Fact]
            public void Should_Add_RequireConsent_To_Arguments_If_True()
            {
                // Given
                var fixture = new NuGetInstallerFixture();
                fixture.Settings.RequireConsent = true;

                // When
                fixture.InstallFromConfig();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() == "install \"/Working/packages.config\" -RequireConsent -NonInteractive"));
            }

            [Fact]
            public void Should_Add_SolutionDirectory_To_Arguments_If_Set()
            {
                // Given
                var fixture = new NuGetInstallerFixture();
                fixture.Settings.SolutionDirectory = "./solution";

                // When
                fixture.InstallFromConfig();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() == "install \"/Working/packages.config\" -SolutionDirectory \"/Working/solution\" -NonInteractive"));
            }

            [Fact]
            public void Should_Add_Sources_To_Arguments_If_Set()
            {
                // Given
                var fixture = new NuGetInstallerFixture();
                fixture.Settings.Source = new[] { "A;B;C" };

                // When
                fixture.InstallFromConfig();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() == "install \"/Working/packages.config\" -Source \"A;B;C\" -NonInteractive"));
            }

            [Fact]
            public void Should_Add_NoCache_To_Arguments_If_True()
            {
                // Given
                var fixture = new NuGetInstallerFixture();
                fixture.Settings.NoCache = true;

                // When
                fixture.InstallFromConfig();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() == "install \"/Working/packages.config\" -NoCache -NonInteractive"));
            }

            [Fact]
            public void Should_Add_DisableParallelProcessing_To_Arguments_If_Set()
            {
                // Given
                var fixture = new NuGetInstallerFixture();
                fixture.Settings.DisableParallelProcessing = true;

                // When
                fixture.InstallFromConfig();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() == "install \"/Working/packages.config\" -DisableParallelProcessing -NonInteractive"));
            }

            [Theory]
            [InlineData(NuGetVerbosity.Detailed, "detailed", "install \"/Working/packages.config\" -Verbosity detailed -NonInteractive")]
            [InlineData(NuGetVerbosity.Normal, "normal", "install \"/Working/packages.config\" -Verbosity normal -NonInteractive")]
            [InlineData(NuGetVerbosity.Quiet, "quiet", "install \"/Working/packages.config\" -Verbosity quiet -NonInteractive")]
            public void Should_Add_Verbosity_To_Arguments_If_Set(NuGetVerbosity verbosity, string name, string expected)
            {
                // Given
                var fixture = new NuGetInstallerFixture();
                fixture.Settings.Verbosity = verbosity;

                // When
                fixture.InstallFromConfig();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() == expected));
            }

            [Fact]
            public void Should_Add_ConfigFile_To_Arguments_If_Set()
            {
                // Given
                var fixture = new NuGetInstallerFixture();
                fixture.Settings.ConfigFile = "./nuget.config";

                // When
                fixture.InstallFromConfig();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() == "install \"/Working/packages.config\" -ConfigFile \"/Working/nuget.config\" -NonInteractive"));
            }
        }
    }
}