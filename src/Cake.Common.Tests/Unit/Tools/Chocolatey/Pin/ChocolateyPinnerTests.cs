using Cake.Common.Tests.Fixtures.Tools.Chocolatey;
using Cake.Core.IO;
using NSubstitute;
using Xunit;

namespace Cake.Common.Tests.Unit.Tools.Chocolatey.Pin
{
    public sealed class ChocolateyPinnerTests
    {
        public sealed class ThePinMethod
        {
            [Fact]
            public void Should_Throw_If_Settings_Are_Null()
            {
                // Given
                var fixture = new ChocolateyPinnerFixture();
                fixture.Settings = null;

                // When
                var result = Record.Exception(() => fixture.Pin());

                // Then
                Assert.IsArgumentNullException(result, "settings");
            }

            [Fact]
            public void Should_Throw_If_Chocolatey_Executable_Was_Not_Found()
            {
                // Given
                var fixture = new ChocolateyPinnerFixture();
                fixture.GivenDefaultToolDoNotExist();

                // When
                var result = Record.Exception(() => fixture.Pin());

                // Then
                Assert.IsCakeException(result, "Chocolatey: Could not locate executable.");
            }

            [Theory]
            [InlineData("C:/ProgramData/chocolatey/choco.exe", "C:/ProgramData/chocolatey/choco.exe")]
            public void Should_Use_Chocolatey_Executable_From_Tool_Path_If_Provided(string toolPath, string expected)
            {
                // Given
                var fixture = new ChocolateyPinnerFixture();
                fixture.GivenCustomToolPathExist(expected);
                fixture.Settings.ToolPath = toolPath;

                // When
                fixture.Pin();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Is<FilePath>(p => p.FullPath == expected),
                    Arg.Any<ProcessSettings>());
            }

            [Fact]
            public void Should_Throw_If_Process_Was_Not_Started()
            {
                // Given
                var fixture = new ChocolateyPinnerFixture();
                fixture.GivenProcessCannotStart();

                // When
                var result = Record.Exception(() => fixture.Pin());

                // Then
                Assert.IsCakeException(result, "Chocolatey: Process was not started.");
            }

            [Fact]
            public void Should_Throw_If_Process_Has_A_Non_Zero_Exit_Code()
            {
                // Given
                var fixture = new ChocolateyPinnerFixture();
                fixture.GivenProcessReturnError();

                // When
                var result = Record.Exception(() => fixture.Pin());

                // Then
                Assert.IsCakeException(result, "Chocolatey: Process returned an error.");
            }

            [Fact]
            public void Should_Find_Chocolatey_Executable_If_Tool_Path_Not_Provided()
            {
                // Given
                var fixture = new ChocolateyPinnerFixture();

                // When
                fixture.Pin();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Is<FilePath>(p => p.FullPath == "/Working/tools/choco.exe"),
                    Arg.Any<ProcessSettings>());
            }

            [Fact]
            public void Should_Add_Mandatory_Arguments()
            {
                // Given
                var fixture = new ChocolateyPinnerFixture();

                // When
                fixture.Pin();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() == "pin add -n \"Cake\" -y"));
            }

            [Theory]
            [InlineData(true, "pin add -n \"Cake\" -d -y")]
            [InlineData(false, "pin add -n \"Cake\" -y")]
            public void Should_Add_Debug_Flag_To_Arguments_If_Set(bool debug, string expected)
            {
                // Given
                var fixture = new ChocolateyPinnerFixture();
                fixture.Settings.Debug = debug;

                // When
                fixture.Pin();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() == expected));
            }

            [Theory]
            [InlineData(true, "pin add -n \"Cake\" -v -y")]
            [InlineData(false, "pin add -n \"Cake\" -y")]
            public void Should_Add_Verbose_Flag_To_Arguments_If_Set(bool verbose, string expected)
            {
                // Given
                var fixture = new ChocolateyPinnerFixture();
                fixture.Settings.Verbose = verbose;

                // When
                fixture.Pin();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() == expected));
            }

            [Theory]
            [InlineData(true, "pin add -n \"Cake\" -y -f")]
            [InlineData(false, "pin add -n \"Cake\" -y")]
            public void Should_Add_Force_Flag_To_Arguments_If_Set(bool force, string expected)
            {
                // Given
                var fixture = new ChocolateyPinnerFixture();
                fixture.Settings.Force = force;

                // When
                fixture.Pin();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() == expected));
            }

            [Theory]
            [InlineData(true, "pin add -n \"Cake\" -y --noop")]
            [InlineData(false, "pin add -n \"Cake\" -y")]
            public void Should_Add_Noop_Flag_To_Arguments_If_Set(bool noop, string expected)
            {
                // Given
                var fixture = new ChocolateyPinnerFixture();
                fixture.Settings.Noop = noop;

                // When
                fixture.Pin();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() == expected));
            }

            [Theory]
            [InlineData(true, "pin add -n \"Cake\" -y -r")]
            [InlineData(false, "pin add -n \"Cake\" -y")]
            public void Should_Add_LimitOutput_Flag_To_Arguments_If_Set(bool limitOutput, string expected)
            {
                // Given
                var fixture = new ChocolateyPinnerFixture();
                fixture.Settings.LimitOutput = limitOutput;

                // When
                fixture.Pin();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() == expected));
            }

            [Theory]
            [InlineData(5, "pin add -n \"Cake\" -y --execution-timeout \"5\"")]
            [InlineData(0, "pin add -n \"Cake\" -y")]
            public void Should_Add_ExecutionTimeout_To_Arguments_If_Set(int executionTimeout, string expected)
            {
                // Given
                var fixture = new ChocolateyPinnerFixture();
                fixture.Settings.ExecutionTimeout = executionTimeout;

                // When
                fixture.Pin();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() == expected));
            }

            [Theory]
            [InlineData(@"c:\temp", "pin add -n \"Cake\" -y -c \"c:\\temp\"")]
            [InlineData("", "pin add -n \"Cake\" -y")]
            public void Should_Add_CacheLocation_Flag_To_Arguments_If_Set(string cacheLocation, string expected)
            {
                // Given
                var fixture = new ChocolateyPinnerFixture();
                fixture.Settings.CacheLocation = cacheLocation;

                // When
                fixture.Pin();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() == expected));
            }

            [Theory]
            [InlineData(true, "pin add -n \"Cake\" -y --allowunofficial")]
            [InlineData(false, "pin add -n \"Cake\" -y")]
            public void Should_Add_AllowUnofficial_Flag_To_Arguments_If_Set(bool allowUnofficial, string expected)
            {
                // Given
                var fixture = new ChocolateyPinnerFixture();
                fixture.Settings.AllowUnoffical = allowUnofficial;

                // When
                fixture.Pin();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() == expected));
            }

            [Fact]
            public void Should_Add_Version_To_Arguments_If_Not_Null()
            {
                // Given
                var fixture = new ChocolateyPinnerFixture();
                fixture.Settings.Version = "1.0.0";

                // When
                fixture.Pin();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(),
                    Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() == "pin add -n \"Cake\" --version \"1.0.0\" -y"));
            }
        }
    }
}