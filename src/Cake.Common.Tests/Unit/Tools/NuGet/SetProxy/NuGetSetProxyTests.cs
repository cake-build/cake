using System.Collections.Generic;
using Cake.Common.Tests.Fixtures;
using Cake.Common.Tests.Fixtures.Tools;
using Cake.Common.Tests.Fixtures.Tools.NuGet;
using Cake.Common.Tools.NuGet;
using Cake.Common.Tools.NuGet.SetApiKey;
using Cake.Core;
using Cake.Core.IO;
using NSubstitute;
using Xunit;

namespace Cake.Common.Tests.Unit.Tools.NuGet.SetProxy
{
    public sealed class NuGetSetProxyTests
    {
        public sealed class TheSetProxyMethod
        {
            [Fact]
            public void Should_Throw_If_Url_Is_Null()
            {
                // Given
                var fixture = new NuGetSetProxyFixture();
                fixture.Url = null;

                // When
                var result = Record.Exception(() => fixture.SetProxy());

                // Then
                Assert.IsArgumentNullException(result, "url");
            }

            [Fact]
            public void Should_Throw_If_Settings_Are_Null()
            {
                // Given
                var fixture = new NuGetSetProxyFixture();
                fixture.Settings = null;

                // When
                var result = Record.Exception(() => fixture.SetProxy());

                // Then
                Assert.IsArgumentNullException(result, "settings");
            }

            [Fact]
            public void Should_Throw_If_Encounter_Unexpected_Output()
            {
                // Given
                var fixture = new NuGetSetProxyFixture();
                fixture.GivenUnexpectedOutput();

                // When
                var result = Record.Exception(() => fixture.SetProxy());

                // Then
                Assert.IsCakeException(result, "Set command returned unexpected response.");
            }

            [Fact]
            public void Should_Throw_If_NuGet_Executable_Was_Not_Found()
            {
                // Given
                var fixture = new NuGetSetProxyFixture();
                fixture.GivenDefaultToolDoNotExist();

                // When
                var result = Record.Exception(() => fixture.SetProxy());

                // Then
                Assert.IsCakeException(result, "NuGet: Could not locate executable.");
            }

            [Theory]
            [InlineData("C:/nuget/nuget.exe", "C:/nuget/nuget.exe")]
            [InlineData("./tools/nuget/nuget.exe", "/Working/tools/nuget/nuget.exe")]
            public void Should_Use_NuGet_Executable_From_Tool_Path_If_Provided(string toolPath, string expected)
            {
                // Given
                var fixture = new NuGetSetProxyFixture();
                fixture.Settings.ToolPath = toolPath;
                fixture.GivenCustomToolPathExist(expected);
                
                // When
                fixture.SetProxy();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Is<FilePath>(p => p.FullPath == expected),
                    Arg.Any<ProcessSettings>());
            }

            [Fact]
            public void Should_Throw_If_Process_Was_Not_Started()
            {
                // Given
                var fixture = new NuGetSetProxyFixture();
                fixture.GivenProcessCannotStart();

                // When
                var result = Record.Exception(() => fixture.SetProxy());

                // Then
                Assert.IsCakeException(result, "NuGet: Process was not started.");
            }

            [Fact]
            public void Should_Throw_If_Process_Has_A_Non_Zero_Exit_Code()
            {
                // Given
                var fixture = new NuGetSetProxyFixture();
                fixture.GivenProcessReturnError();

                // When
                var result = Record.Exception(() => fixture.SetProxy());

                // Then
                Assert.IsCakeException(result, "NuGet: Process returned an error.");
            }

            [Fact]
            public void Should_Find_NuGet_Executable_If_Tool_Path_Not_Provided()
            {
                // Given
                var fixture = new NuGetSetProxyFixture();
                
                // When
                fixture.SetProxy();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Is<FilePath>(p => p.FullPath == "/Working/tools/NuGet.exe"),
                    Arg.Any<ProcessSettings>());
            }

            [Theory]
            [InlineData(NuGetVerbosity.Detailed, "config -Set http_proxy=http://a.com -Verbosity detailed -NonInteractive")]
            [InlineData(NuGetVerbosity.Normal, "config -Set http_proxy=http://a.com -Verbosity normal -NonInteractive")]
            [InlineData(NuGetVerbosity.Quiet, "config -Set http_proxy=http://a.com -Verbosity quiet -NonInteractive")]
            public void Should_Add_Verbosity_To_Arguments_If_Set(NuGetVerbosity verbosity, string expected)
            {
                // Given
                var fixture = new NuGetSetProxyFixture();

                fixture.Settings.Verbosity = verbosity;

                // When
                fixture.SetProxy();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() == expected));
            }

            [Fact]
            public void Should_Add_UP_To_Arguments_If_Set()
            {
                // Given
                var fixture = new NuGetSetProxyFixture();
                fixture.Username = "Admin";
                fixture.Password = "Pass1";

                // When
                fixture.SetProxy();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() == "config -Set http_proxy=http://a.com -Set http_proxy.user=Admin -Set http_proxy.password=Pass1 -NonInteractive"));

            }

            [Fact]
            public void Should_Add_ConfigFile_To_Arguments_If_Set()
            {
                // Given
                var fixture = new NuGetSetProxyFixture();
                fixture.Settings.ConfigFile = "./nuget.config";

                // When
                fixture.SetProxy();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() == "config -Set http_proxy=http://a.com -ConfigFile \"/Working/nuget.config\" -NonInteractive"));

            }

            [Fact]
            public void Should_Add_Mandatory_Arguments()
            {
                // Given
                var fixture = new NuGetSetProxyFixture();

                // When
                fixture.SetProxy();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() == "config -Set http_proxy=http://a.com -NonInteractive"));
            }
        }
    }
}
