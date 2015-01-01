using Cake.Common.Tests.Fixtures;
using Cake.Common.Tools.NuGet;
using Cake.Common.Tools.NuGet.Restore;
using Cake.Core;
using Cake.Core.IO;
using NSubstitute;
using Xunit;

namespace Cake.Common.Tests.Unit.Tools.NuGet.Restore
{
    public sealed class NuGetRestorerTests
    {
        public sealed class TheRestoreMethod
        {
            [Fact]
            public void Should_Throw_If_Target_File_Path_Is_Null()
            {
                // Given
                var fixture = new NuGetFixture(defaultToolExist: false);
                var restorer = fixture.CreateRestorer();

                // When
                var result = Record.Exception(() => restorer.Restore(null, new NuGetRestoreSettings()));

                // Then
                Assert.IsArgumentNullException(result, "targetFilePath");
            }

            [Fact]
            public void Should_Throw_If_Settings_Are_Null()
            {
                // Given
                var fixture = new NuGetFixture(defaultToolExist: false);
                var restorer = fixture.CreateRestorer();

                // When
                var result = Record.Exception(() => restorer.Restore("./project.sln", null));

                // Then
                Assert.IsArgumentNullException(result, "settings");
            }

            [Fact]
            public void Should_Throw_If_NuGet_Executable_Was_Not_Found()
            {
                // Given
                var fixture = new NuGetFixture(defaultToolExist: false);
                var restorer = fixture.CreateRestorer();

                // When
                var result = Record.Exception(() => restorer.Restore("./project.sln", new NuGetRestoreSettings()));

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
                var restorer = fixture.CreateRestorer();

                // When
                restorer.Restore("./project.sln", new NuGetRestoreSettings
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
                var restorer = fixture.CreateRestorer();

                // When
                var result = Record.Exception(() => restorer.Restore("./project.sln", new NuGetRestoreSettings()));

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
                var restorer = fixture.CreateRestorer();

                // When
                var result = Record.Exception(() => restorer.Restore("./project.sln", new NuGetRestoreSettings()));

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("NuGet: Process returned an error.", result.Message);
            }

            [Fact]
            public void Should_Find_NuGet_Executable_If_Tool_Path_Not_Provided()
            {
                // Given
                var fixture = new NuGetFixture();
                var restorer = fixture.CreateRestorer();

                // When
                restorer.Restore("./project.sln", new NuGetRestoreSettings());

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
                var restorer = fixture.CreateRestorer();

                // When
                restorer.Restore("./project.sln", new NuGetRestoreSettings());

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(p => 
                        p.Arguments.Render() == "restore \"/Working/project.sln\" -NonInteractive"));
            }

            [Fact]
            public void Should_Add_RequireConsent_To_Arguments_If_True()
            {
                // Given
                var fixture = new NuGetFixture();
                var restorer = fixture.CreateRestorer();

                // When
                restorer.Restore("./project.sln", new NuGetRestoreSettings
                {
                    RequireConsent = true
                });

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() == "restore \"/Working/project.sln\" -RequireConsent -NonInteractive"));
            }

            [Fact]
            public void Should_Add_PackageDirectory_To_Arguments_If_Set()
            {
                // Given
                var fixture = new NuGetFixture();
                var restorer = fixture.CreateRestorer();

                // When
                restorer.Restore("./project.sln", new NuGetRestoreSettings
                {
                    PackagesDirectory = "./package"
                });

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() == "restore \"/Working/project.sln\" -PackagesDirectory \"/Working/package\" -NonInteractive"));
            }

            [Fact]
            public void Should_Add_Sources_To_Arguments_If_Set()
            {
                // Given
                var fixture = new NuGetFixture();
                var restorer = fixture.CreateRestorer();

                // When
                restorer.Restore("./project.sln", new NuGetRestoreSettings
                {
                    Source = new[] { "A;B;C" }
                });

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() == "restore \"/Working/project.sln\" -Source \"A;B;C\" -NonInteractive"));

            }

            [Fact]
            public void Should_Add_NoCache_To_Arguments_If_True()
            {
                // Given
                var fixture = new NuGetFixture();
                var restorer = fixture.CreateRestorer();

                // When
                restorer.Restore("./project.sln", new NuGetRestoreSettings
                {
                    NoCache = true
                });

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() == "restore \"/Working/project.sln\" -NoCache -NonInteractive"));
            }

            [Fact]
            public void Should_Add_DisableParallelProcessing_To_Arguments_If_Set()
            {
                // Given
                var fixture = new NuGetFixture();
                var restorer = fixture.CreateRestorer();

                // When
                restorer.Restore("./project.sln", new NuGetRestoreSettings
                {
                    DisableParallelProcessing = true
                });

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() == "restore \"/Working/project.sln\" -DisableParallelProcessing -NonInteractive"));
            }

            [Theory]
            [InlineData(NuGetVerbosity.Detailed, "detailed")]
            [InlineData(NuGetVerbosity.Normal, "normal")]
            [InlineData(NuGetVerbosity.Quiet, "quiet")]
            public void Should_Add_Verbosity_To_Arguments_If_Set(NuGetVerbosity verbosity, string name)
            {
                // Given
                var fixture = new NuGetFixture();
                var restorer = fixture.CreateRestorer();
                var expected = string.Format("restore \"/Working/project.sln\" -Verbosity {0} -NonInteractive", name);

                // When
                restorer.Restore("./project.sln", new NuGetRestoreSettings
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
                var restorer = fixture.CreateRestorer();

                // When
                restorer.Restore("./project.sln", new NuGetRestoreSettings
                {
                    ConfigFile = "./nuget.config"
                });

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() == "restore \"/Working/project.sln\" -ConfigFile \"/Working/nuget.config\" -NonInteractive"));

            }
        }
    }
}
