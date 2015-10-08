using Cake.Common.Tests.Fixtures.Tools.Chocolatey;
using Cake.Core.IO;
using NSubstitute;
using Xunit;

namespace Cake.Common.Tests.Unit.Tools.Chocolatey.Features
{
    public sealed class ChocolateyFeatureTogglerTests
    {
        public sealed class TheEnableFeatureMethod
        {
            [Fact]
            public void Should_Throw_If_Settings_Are_Null()
            {
                // Given
                var fixture = new ChocolateyFeatureTogglerFixture();
                fixture.Settings = null;

                // When
                var result = Record.Exception(() => fixture.EnableFeature());

                // Then
                Assert.IsArgumentNullException(result, "settings");
            }

            [Fact]
            public void Should_Throw_If_Chocolatey_Executable_Was_Not_Found()
            {
                // Given
                var fixture = new ChocolateyFeatureTogglerFixture();
                fixture.GivenDefaultToolDoNotExist();

                // When
                var result = Record.Exception(() => fixture.EnableFeature());

                // Then
                Assert.IsCakeException(result, "Chocolatey: Could not locate executable.");
            }

            [Theory]
            [InlineData("C:/ProgramData/chocolatey/choco.exe", "C:/ProgramData/chocolatey/choco.exe")]
            public void Should_Use_Chocolatey_Executable_From_Tool_Path_If_Provided(string toolPath, string expected)
            {
                // Given
                var fixture = new ChocolateyFeatureTogglerFixture();
                fixture.GivenCustomToolPathExist(expected);
                fixture.Settings.ToolPath = toolPath;

                // When
                fixture.EnableFeature();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Is<FilePath>(p => p.FullPath == expected),
                    Arg.Any<ProcessSettings>());
            }

            [Fact]
            public void Should_Throw_If_Process_Was_Not_Started()
            {
                // Given
                var fixture = new ChocolateyFeatureTogglerFixture();
                fixture.GivenProcessCannotStart();

                // When
                var result = Record.Exception(() => fixture.EnableFeature());

                // Then
                Assert.IsCakeException(result, "Chocolatey: Process was not started.");
            }

            [Fact]
            public void Should_Throw_If_Process_Has_A_Non_Zero_Exit_Code()
            {
                // Given
                var fixture = new ChocolateyFeatureTogglerFixture();
                fixture.GivenProcessReturnError();

                // When
                var result = Record.Exception(() => fixture.EnableFeature());

                // Then
                Assert.IsCakeException(result, "Chocolatey: Process returned an error.");
            }

            [Fact]
            public void Should_Find_Chocolatey_Executable_If_Tool_Path_Not_Provided()
            {
                // Given
                var fixture = new ChocolateyFeatureTogglerFixture();

                // When
                fixture.EnableFeature();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Is<FilePath>(p => p.FullPath == "/Working/tools/choco.exe"),
                    Arg.Any<ProcessSettings>());
            }

            [Fact]
            public void Should_Add_Mandatory_Arguments()
            {
                // Given
                var fixture = new ChocolateyFeatureTogglerFixture();

                // When
                fixture.EnableFeature();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() == "feature enable -n \"checkSumFiles\" -y"));
            }

            [Theory]
            [InlineData(true, "feature enable -n \"checkSumFiles\" -d -y")]
            [InlineData(false, "feature enable -n \"checkSumFiles\" -y")]
            public void Should_Add_Debug_Flag_To_Arguments_If_Set(bool debug, string expected)
            {
                // Given
                var fixture = new ChocolateyFeatureTogglerFixture();
                fixture.Settings.Debug = debug;

                // When
                fixture.EnableFeature();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() == expected));
            }

            [Theory]
            [InlineData(true, "feature enable -n \"checkSumFiles\" -v -y")]
            [InlineData(false, "feature enable -n \"checkSumFiles\" -y")]
            public void Should_Add_Verbose_Flag_To_Arguments_If_Set(bool verbose, string expected)
            {
                // Given
                var fixture = new ChocolateyFeatureTogglerFixture();
                fixture.Settings.Verbose = verbose;

                // When
                fixture.EnableFeature();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() == expected));
            }

            [Theory]
            [InlineData(true, "feature enable -n \"checkSumFiles\" -y -f")]
            [InlineData(false, "feature enable -n \"checkSumFiles\" -y")]
            public void Should_Add_Force_Flag_To_Arguments_If_Set(bool force, string expected)
            {
                // Given
                var fixture = new ChocolateyFeatureTogglerFixture();
                fixture.Settings.Force = force;

                // When
                fixture.EnableFeature();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() == expected));
            }

            [Theory]
            [InlineData(true, "feature enable -n \"checkSumFiles\" -y --noop")]
            [InlineData(false, "feature enable -n \"checkSumFiles\" -y")]
            public void Should_Add_Noop_Flag_To_Arguments_If_Set(bool noop, string expected)
            {
                // Given
                var fixture = new ChocolateyFeatureTogglerFixture();
                fixture.Settings.Noop = noop;

                // When
                fixture.EnableFeature();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() == expected));
            }

            [Theory]
            [InlineData(true, "feature enable -n \"checkSumFiles\" -y -r")]
            [InlineData(false, "feature enable -n \"checkSumFiles\" -y")]
            public void Should_Add_LimitOutput_Flag_To_Arguments_If_Set(bool limitOutput, string expected)
            {
                // Given
                var fixture = new ChocolateyFeatureTogglerFixture();
                fixture.Settings.LimitOutput = limitOutput;

                // When
                fixture.EnableFeature();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() == expected));
            }

            [Theory]
            [InlineData(5, "feature enable -n \"checkSumFiles\" -y --execution-timeout \"5\"")]
            [InlineData(0, "feature enable -n \"checkSumFiles\" -y")]
            public void Should_Add_ExecutionTimeout_To_Arguments_If_Set(int executionTimeout, string expected)
            {
                // Given
                var fixture = new ChocolateyFeatureTogglerFixture();
                fixture.Settings.ExecutionTimeout = executionTimeout;

                // When
                fixture.EnableFeature();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() == expected));
            }

            [Theory]
            [InlineData(@"c:\temp", "feature enable -n \"checkSumFiles\" -y -c \"c:\\temp\"")]
            [InlineData("", "feature enable -n \"checkSumFiles\" -y")]
            public void Should_Add_CacheLocation_Flag_To_Arguments_If_Set(string cacheLocation, string expected)
            {
                // Given
                var fixture = new ChocolateyFeatureTogglerFixture();
                fixture.Settings.CacheLocation = cacheLocation;

                // When
                fixture.EnableFeature();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() == expected));
            }

            [Theory]
            [InlineData(true, "feature enable -n \"checkSumFiles\" -y --allowunofficial")]
            [InlineData(false, "feature enable -n \"checkSumFiles\" -y")]
            public void Should_Add_AllowUnofficial_Flag_To_Arguments_If_Set(bool allowUnofficial, string expected)
            {
                // Given
                var fixture = new ChocolateyFeatureTogglerFixture();
                fixture.Settings.AllowUnoffical = allowUnofficial;

                // When
                fixture.EnableFeature();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() == expected));
            }
        }

        public sealed class TheDisableFeatureMethod
        {
            [Fact]
            public void Should_Throw_If_Settings_Are_Null()
            {
                // Given
                var fixture = new ChocolateyFeatureTogglerFixture();
                fixture.Settings = null;

                // When
                var result = Record.Exception(() => fixture.DisableFeature());

                // Then
                Assert.IsArgumentNullException(result, "settings");
            }

            [Fact]
            public void Should_Throw_If_Chocolatey_Executable_Was_Not_Found()
            {
                // Given
                var fixture = new ChocolateyFeatureTogglerFixture();
                fixture.GivenDefaultToolDoNotExist();

                // When
                var result = Record.Exception(() => fixture.DisableFeature());

                // Then
                Assert.IsCakeException(result, "Chocolatey: Could not locate executable.");
            }

            [Theory]
            [InlineData("C:/ProgramData/chocolatey/choco.exe", "C:/ProgramData/chocolatey/choco.exe")]
            public void Should_Use_Chocolatey_Executable_From_Tool_Path_If_Provided(string toolPath, string expected)
            {
                // Given
                var fixture = new ChocolateyFeatureTogglerFixture();
                fixture.GivenCustomToolPathExist(expected);
                fixture.Settings.ToolPath = toolPath;

                // When
                fixture.DisableFeature();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Is<FilePath>(p => p.FullPath == expected),
                    Arg.Any<ProcessSettings>());
            }

            [Fact]
            public void Should_Throw_If_Process_Was_Not_Started()
            {
                // Given
                var fixture = new ChocolateyFeatureTogglerFixture();
                fixture.GivenProcessCannotStart();

                // When
                var result = Record.Exception(() => fixture.DisableFeature());

                // Then
                Assert.IsCakeException(result, "Chocolatey: Process was not started.");
            }

            [Fact]
            public void Should_Throw_If_Process_Has_A_Non_Zero_Exit_Code()
            {
                // Given
                var fixture = new ChocolateyFeatureTogglerFixture();
                fixture.GivenProcessReturnError();

                // When
                var result = Record.Exception(() => fixture.DisableFeature());

                // Then
                Assert.IsCakeException(result, "Chocolatey: Process returned an error.");
            }

            [Fact]
            public void Should_Find_Chocolatey_Executable_If_Tool_Path_Not_Provided()
            {
                // Given
                var fixture = new ChocolateyFeatureTogglerFixture();

                // When
                fixture.DisableFeature();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Is<FilePath>(p => p.FullPath == "/Working/tools/choco.exe"),
                    Arg.Any<ProcessSettings>());
            }

            [Fact]
            public void Should_Add_Mandatory_Arguments()
            {
                // Given
                var fixture = new ChocolateyFeatureTogglerFixture();

                // When
                fixture.DisableFeature();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() == "feature disable -n \"checkSumFiles\" -y"));
            }

            [Theory]
            [InlineData(true, "feature disable -n \"checkSumFiles\" -d -y")]
            [InlineData(false, "feature disable -n \"checkSumFiles\" -y")]
            public void Should_Add_Debug_Flag_To_Arguments_If_Set(bool debug, string expected)
            {
                // Given
                var fixture = new ChocolateyFeatureTogglerFixture();
                fixture.Settings.Debug = debug;

                // When
                fixture.DisableFeature();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() == expected));
            }

            [Theory]
            [InlineData(true, "feature disable -n \"checkSumFiles\" -v -y")]
            [InlineData(false, "feature disable -n \"checkSumFiles\" -y")]
            public void Should_Add_Verbose_Flag_To_Arguments_If_Set(bool verbose, string expected)
            {
                // Given
                var fixture = new ChocolateyFeatureTogglerFixture();
                fixture.Settings.Verbose = verbose;

                // When
                fixture.DisableFeature();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() == expected));
            }

            [Theory]
            [InlineData(true, "feature disable -n \"checkSumFiles\" -y -f")]
            [InlineData(false, "feature disable -n \"checkSumFiles\" -y")]
            public void Should_Add_Force_Flag_To_Arguments_If_Set(bool force, string expected)
            {
                // Given
                var fixture = new ChocolateyFeatureTogglerFixture();
                fixture.Settings.Force = force;

                // When
                fixture.DisableFeature();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() == expected));
            }

            [Theory]
            [InlineData(true, "feature disable -n \"checkSumFiles\" -y --noop")]
            [InlineData(false, "feature disable -n \"checkSumFiles\" -y")]
            public void Should_Add_Noop_Flag_To_Arguments_If_Set(bool noop, string expected)
            {
                // Given
                var fixture = new ChocolateyFeatureTogglerFixture();
                fixture.Settings.Noop = noop;

                // When
                fixture.DisableFeature();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() == expected));
            }

            [Theory]
            [InlineData(true, "feature disable -n \"checkSumFiles\" -y -r")]
            [InlineData(false, "feature disable -n \"checkSumFiles\" -y")]
            public void Should_Add_LimitOutput_Flag_To_Arguments_If_Set(bool limitOutput, string expected)
            {
                // Given
                var fixture = new ChocolateyFeatureTogglerFixture();
                fixture.Settings.LimitOutput = limitOutput;

                // When
                fixture.DisableFeature();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() == expected));
            }

            [Theory]
            [InlineData(5, "feature disable -n \"checkSumFiles\" -y --execution-timeout \"5\"")]
            [InlineData(0, "feature disable -n \"checkSumFiles\" -y")]
            public void Should_Add_ExecutionTimeout_To_Arguments_If_Set(int executionTimeout, string expected)
            {
                // Given
                var fixture = new ChocolateyFeatureTogglerFixture();
                fixture.Settings.ExecutionTimeout = executionTimeout;

                // When
                fixture.DisableFeature();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() == expected));
            }

            [Theory]
            [InlineData(@"c:\temp", "feature disable -n \"checkSumFiles\" -y -c \"c:\\temp\"")]
            [InlineData("", "feature disable -n \"checkSumFiles\" -y")]
            public void Should_Add_CacheLocation_Flag_To_Arguments_If_Set(string cacheLocation, string expected)
            {
                // Given
                var fixture = new ChocolateyFeatureTogglerFixture();
                fixture.Settings.CacheLocation = cacheLocation;

                // When
                fixture.DisableFeature();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() == expected));
            }

            [Theory]
            [InlineData(true, "feature disable -n \"checkSumFiles\" -y --allowunofficial")]
            [InlineData(false, "feature disable -n \"checkSumFiles\" -y")]
            public void Should_Add_AllowUnofficial_Flag_To_Arguments_If_Set(bool allowUnofficial, string expected)
            {
                // Given
                var fixture = new ChocolateyFeatureTogglerFixture();
                fixture.Settings.AllowUnoffical = allowUnofficial;

                // When
                fixture.DisableFeature();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() == expected));
            }
        }
    }
}