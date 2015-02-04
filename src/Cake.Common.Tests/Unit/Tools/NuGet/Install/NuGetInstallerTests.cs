using Cake.Common.Tests.Fixtures;
using Cake.Common.Tools.NuGet;
using Cake.Common.Tools.NuGet.Install;
using Cake.Core;
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
                var fixture = new NuGetFixture(defaultToolExist: false);
                var installer = fixture.CreateInstaller();


                // When
                var result = Record.Exception(() => installer.Install(null, new NuGetInstallSettings()));

                // Then
                Assert.IsArgumentNullException(result, "packageId");
            }

            [Fact]
            public void Should_Throw_If_Settings_Are_Null()
            {
                // Given
                var fixture = new NuGetFixture(defaultToolExist: false);
                var installer = fixture.CreateInstaller();

                // When
                var result = Record.Exception(() => installer.Install("Cake", null));

                // Then
                Assert.IsArgumentNullException(result, "settings");
            }


            [Fact]
            public void Should_Throw_If_NuGet_Executable_Was_Not_Found()
            {
                // Given
                var fixture = new NuGetFixture(defaultToolExist: false);
                var installer = fixture.CreateInstaller();

                // When
                var result = Record.Exception(() => installer.Install("Cake", new NuGetInstallSettings()));

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("NuGet: Could not locate executable.", result.Message);
            }

            [Theory]
            [InlineData("C:/nuget/nuget.exe", "C:/nuget/nuget.exe")]
            [InlineData("./tools/nuget/nuget.exe", "/Working/tools/nuget/nuget.exe")]
            public void Should_Use_NuGet_Executable_From_Tool_Path_If_Provided(string toolPath, string expected)
            {
                // Given
                var fixture = new NuGetFixture(expected);
                var installer = fixture.CreateInstaller();

                // When
                installer.Install("Cake", new NuGetInstallSettings
                {
                    ToolPath = toolPath
                });

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Is<FilePath>(p => p.FullPath == expected),
                    Arg.Any<ProcessSettings>());
            }

            [Fact]
            public void Should_Throw_If_Process_Was_Not_Started()
            {
                // Given
                var fixture = new NuGetFixture();
                fixture.ProcessRunner.Start(Arg.Any<FilePath>(), Arg.Any<ProcessSettings>()).Returns((IProcess)null);
                var installer = fixture.CreateInstaller();

                // When
                var result = Record.Exception(() => installer.Install("Cake", new NuGetInstallSettings()));

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("NuGet: Process was not started.", result.Message);
            }

            [Fact]
            public void Should_Throw_If_Process_Has_A_Non_Zero_Exit_Code()
            {
                // Given
                var fixture = new NuGetFixture();
                fixture.Process.GetExitCode().Returns(1);
                var installer = fixture.CreateInstaller();

                // When
                var result = Record.Exception(() => installer.Install("Cake", new NuGetInstallSettings()));

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("NuGet: Process returned an error.", result.Message);
            }

            [Fact]
            public void Should_Find_NuGet_Executable_If_Tool_Path_Not_Provided()
            {
                // Given
                var fixture = new NuGetFixture();
                var installer = fixture.CreateInstaller();

                // When
                installer.Install("Cake", new NuGetInstallSettings());

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Is<FilePath>(p => p.FullPath == "/Working/tools/NuGet.exe"),
                    Arg.Any<ProcessSettings>());
            }

            [Fact]
            public void Should_Add_Mandatory_Arguments()
            {
                // Given
                var fixture = new NuGetFixture();
                var installer = fixture.CreateInstaller();

                // When
                installer.Install("Cake", new NuGetInstallSettings());

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(p => 
                        p.Arguments.Render() == "install \"Cake\" -NonInteractive"));
            }

            [Fact]
            public void Should_Add_RequireConsent_To_Arguments_If_True()
            {
                // Given
                var fixture = new NuGetFixture();
                var installer = fixture.CreateInstaller();

                // When
                installer.Install("Cake", new NuGetInstallSettings
                {
                    RequireConsent = true
                });

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() == "install \"Cake\" -RequireConsent -NonInteractive"));
            }

            [Fact]
            public void Should_Add_SolutionDirectory_To_Arguments_If_Set()
            {
                // Given
                var fixture = new NuGetFixture();
                var installer = fixture.CreateInstaller();

                // When
                installer.Install("Cake", new NuGetInstallSettings
                {
                    SolutionDirectory = "./solution"
                });

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() == "install \"Cake\" -SolutionDirectory \"/Working/solution\" -NonInteractive"));
            }

            [Fact]
            public void Should_Add_Sources_To_Arguments_If_Set()
            {
                // Given
                var fixture = new NuGetFixture();
                var installer = fixture.CreateInstaller();

                // When
                installer.Install("Cake", new NuGetInstallSettings
                {
                    Source = new[] { "A;B;C" }
                });

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() == "install \"Cake\" -Source \"A;B;C\" -NonInteractive"));

            }

            [Fact]
            public void Should_Add_NoCache_To_Arguments_If_True()
            {
                // Given
                var fixture = new NuGetFixture();
                var installer = fixture.CreateInstaller();

                // When
                installer.Install("Cake", new NuGetInstallSettings
                {
                    NoCache = true
                });

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() == "install \"Cake\" -NoCache -NonInteractive"));
            }

            [Fact]
            public void Should_Add_DisableParallelProcessing_To_Arguments_If_Set()
            {
                // Given
                var fixture = new NuGetFixture();
                var installer = fixture.CreateInstaller();

                // When
                installer.Install("Cake", new NuGetInstallSettings
                {
                    DisableParallelProcessing = true
                });

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() == "install \"Cake\" -DisableParallelProcessing -NonInteractive"));
            }

            [Theory]
            [InlineData(NuGetVerbosity.Detailed, "detailed")]
            [InlineData(NuGetVerbosity.Normal, "normal")]
            [InlineData(NuGetVerbosity.Quiet, "quiet")]
            public void Should_Add_Verbosity_To_Arguments_If_Set(NuGetVerbosity verbosity, string name)
            {
                // Given
                var fixture = new NuGetFixture();
                var installer = fixture.CreateInstaller();
                var expected = string.Format("install \"Cake\" -Verbosity {0} -NonInteractive", name);

                // When
                installer.Install("Cake", new NuGetInstallSettings
                {
                    Verbosity = verbosity
                });

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() == expected));
            }

            [Fact]
            public void Should_Add_ConfigFile_To_Arguments_If_Set()
            {
                // Given
                var fixture = new NuGetFixture();
                var installer = fixture.CreateInstaller();

                // When
                installer.Install("Cake", new NuGetInstallSettings
                {
                    ConfigFile = "./nuget.config"
                });

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
                var fixture = new NuGetFixture(defaultToolExist: false);
                var installer = fixture.CreateInstaller();


                // When
                var result = Record.Exception(() => installer.InstallFromConfig(null, new NuGetInstallSettings()));

                // Then
                Assert.IsArgumentNullException(result, "packageConfigPath");
            }

            [Fact]
            public void Should_Throw_If_Settings_Are_Null()
            {
                // Given
                var fixture = new NuGetFixture(defaultToolExist: false);
                var installer = fixture.CreateInstaller();

                // When
                var result = Record.Exception(() => installer.InstallFromConfig("./packages.config", null));

                // Then
                Assert.IsArgumentNullException(result, "settings");
            }


            [Fact]
            public void Should_Throw_If_NuGet_Executable_Was_Not_Found()
            {
                // Given
                var fixture = new NuGetFixture(defaultToolExist: false);
                var installer = fixture.CreateInstaller();

                // When
                var result = Record.Exception(() => installer.InstallFromConfig("./packages.config", new NuGetInstallSettings()));

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("NuGet: Could not locate executable.", result.Message);
            }

            [Theory]
            [InlineData("C:/nuget/nuget.exe", "C:/nuget/nuget.exe")]
            [InlineData("./tools/nuget/nuget.exe", "/Working/tools/nuget/nuget.exe")]
            public void Should_Use_NuGet_Executable_From_Tool_Path_If_Provided(string toolPath, string expected)
            {
                // Given
                var fixture = new NuGetFixture(expected);
                var installer = fixture.CreateInstaller();

                // When
                installer.InstallFromConfig("./packages.config", new NuGetInstallSettings
                {
                    ToolPath = toolPath
                });

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Is<FilePath>(p => p.FullPath == expected),
                    Arg.Any<ProcessSettings>());
            }

            [Fact]
            public void Should_Throw_If_Process_Was_Not_Started()
            {
                // Given
                var fixture = new NuGetFixture();
                fixture.ProcessRunner.Start(Arg.Any<FilePath>(), Arg.Any<ProcessSettings>()).Returns((IProcess)null);
                var installer = fixture.CreateInstaller();

                // When
                var result = Record.Exception(() => installer.InstallFromConfig("./packages.config", new NuGetInstallSettings()));

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("NuGet: Process was not started.", result.Message);
            }

            [Fact]
            public void Should_Throw_If_Process_Has_A_Non_Zero_Exit_Code()
            {
                // Given
                var fixture = new NuGetFixture();
                fixture.Process.GetExitCode().Returns(1);
                var installer = fixture.CreateInstaller();

                // When
                var result = Record.Exception(() => installer.InstallFromConfig("./packages.config", new NuGetInstallSettings()));

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("NuGet: Process returned an error.", result.Message);
            }

            [Fact]
            public void Should_Find_NuGet_Executable_If_Tool_Path_Not_Provided()
            {
                // Given
                var fixture = new NuGetFixture();
                var installer = fixture.CreateInstaller();

                // When
                installer.InstallFromConfig("./packages.config", new NuGetInstallSettings());

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Is<FilePath>(p => p.FullPath == "/Working/tools/NuGet.exe"),
                    Arg.Any<ProcessSettings>());
            }

            [Fact]
            public void Should_Add_Mandatory_Arguments()
            {
                // Given
                var fixture = new NuGetFixture();
                var installer = fixture.CreateInstaller();

                // When
                installer.InstallFromConfig("./packages.config", new NuGetInstallSettings());

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(p => 
                        p.Arguments.Render() == "install \"/Working/packages.config\" -NonInteractive"));
            }

            [Fact]
            public void Should_Add_RequireConsent_To_Arguments_If_True()
            {
                // Given
                var fixture = new NuGetFixture();
                var installer = fixture.CreateInstaller();

                // When
                installer.InstallFromConfig("./packages.config", new NuGetInstallSettings
                {
                    RequireConsent = true
                });

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() == "install \"/Working/packages.config\" -RequireConsent -NonInteractive"));
            }

            [Fact]
            public void Should_Add_SolutionDirectory_To_Arguments_If_Set()
            {
                // Given
                var fixture = new NuGetFixture();
                var installer = fixture.CreateInstaller();

                // When
                installer.InstallFromConfig("./packages.config", new NuGetInstallSettings
                {
                    SolutionDirectory = "./solution"
                });

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() == "install \"/Working/packages.config\" -SolutionDirectory \"/Working/solution\" -NonInteractive"));
            }

            [Fact]
            public void Should_Add_Sources_To_Arguments_If_Set()
            {
                // Given
                var fixture = new NuGetFixture();
                var installer = fixture.CreateInstaller();

                // When
                installer.InstallFromConfig("./packages.config", new NuGetInstallSettings
                {
                    Source = new[] { "A;B;C" }
                });

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() == "install \"/Working/packages.config\" -Source \"A;B;C\" -NonInteractive"));

            }

            [Fact]
            public void Should_Add_NoCache_To_Arguments_If_True()
            {
                // Given
                var fixture = new NuGetFixture();
                var installer = fixture.CreateInstaller();

                // When
                installer.InstallFromConfig("./packages.config", new NuGetInstallSettings
                {
                    NoCache = true
                });

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() == "install \"/Working/packages.config\" -NoCache -NonInteractive"));
            }

            [Fact]
            public void Should_Add_DisableParallelProcessing_To_Arguments_If_Set()
            {
                // Given
                var fixture = new NuGetFixture();
                var installer = fixture.CreateInstaller();

                // When
                installer.InstallFromConfig("./packages.config", new NuGetInstallSettings
                {
                    DisableParallelProcessing = true
                });

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() == "install \"/Working/packages.config\" -DisableParallelProcessing -NonInteractive"));
            }

            [Theory]
            [InlineData(NuGetVerbosity.Detailed, "detailed")]
            [InlineData(NuGetVerbosity.Normal, "normal")]
            [InlineData(NuGetVerbosity.Quiet, "quiet")]
            public void Should_Add_Verbosity_To_Arguments_If_Set(NuGetVerbosity verbosity, string name)
            {
                // Given
                var fixture = new NuGetFixture();
                var installer = fixture.CreateInstaller();
                var expected = string.Format("install \"/Working/packages.config\" -Verbosity {0} -NonInteractive", name);

                // When
                installer.InstallFromConfig("./packages.config", new NuGetInstallSettings
                {
                    Verbosity = verbosity
                });

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() == expected));
            }

            [Fact]
            public void Should_Add_ConfigFile_To_Arguments_If_Set()
            {
                // Given
                var fixture = new NuGetFixture();
                var installer = fixture.CreateInstaller();

                // When
                installer.InstallFromConfig("./packages.config", new NuGetInstallSettings
                {
                    ConfigFile = "./nuget.config"
                });

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() == "install \"/Working/packages.config\" -ConfigFile \"/Working/nuget.config\" -NonInteractive"));

            }
        }
    }
}
