using System.Collections.Generic;
using Cake.Common.Tests.Fixtures;
using Cake.Common.Tools.NuGet;
using Cake.Common.Tools.NuGet.SetApiKey;
using Cake.Core;
using Cake.Core.IO;
using NSubstitute;
using Xunit;

namespace Cake.Common.Tests.Unit.Tools.NuGet.SetApiKey
{
    public sealed class NuGetSetApiKeyTests
    {
        
        public sealed class TheSetApiKeyMethod
        {
            [Fact]
            public void Should_Throw_If_Target_Api_Key_Is_Null()
            {
                // Given
                var fixture = new NuGetFixture(defaultToolExist: false);
                var installer = fixture.CreateSetApiKey();


                // When
                var result = Record.Exception(() => installer.SetApiKey(null, "http://nugetfeed.com", new NuGetSetApiKeySettings()));

                // Then
                Assert.IsArgumentNullException(result, "apiKey");
            }
            [Fact]
            public void Should_Throw_If_Target_Source_Is_Null()
            {
                // Given
                var fixture = new NuGetFixture(defaultToolExist: false);
                var installer = fixture.CreateSetApiKey();


                // When
                var result = Record.Exception(() => installer.SetApiKey("*secret key*", null, new NuGetSetApiKeySettings()));

                // Then
                Assert.IsArgumentNullException(result, "source");
            }

            [Fact]
            public void Should_Throw_If_Settings_Are_Null()
            {
                // Given
                var fixture = new NuGetFixture(defaultToolExist: false);
                var installer = fixture.CreateSetApiKey();

                // When
                var result = Record.Exception(() => installer.SetApiKey("*secret key*", "http://nugetfeed.com", null));

                // Then
                Assert.IsArgumentNullException(result, "settings");
            }


            [Fact]
            public void Should_Throw_If_UnexpectedOutput()
            {
                // Given
                var fixture = new NuGetFixture();
                var installer = fixture.CreateSetApiKey();

                // When
                var result = Record.Exception(() => installer.SetApiKey("*secret key*", "http://nugetfeed.com", new NuGetSetApiKeySettings()));

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("SetApiKey returned unexpected response.", result.Message);
            }


            [Fact]
            public void Should_Throw_If_NuGet_Executable_Was_Not_Found()
            {
                // Given
                var fixture = new NuGetFixture(defaultToolExist: false);
                var installer = fixture.CreateSetApiKey();

                // When
                var result = Record.Exception(() => installer.SetApiKey("*secret key*", "http://nugetfeed.com", new NuGetSetApiKeySettings()));

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
                var apiKeySource = new KeyValuePair<string, string>("*secret key*", "http://nugetfeed.com");
                var fixture = new NuGetFixture(setApiKey:apiKeySource, toolPath:expected);
                var installer = fixture.CreateSetApiKey();
                
                // When
                installer.SetApiKey(apiKeySource.Key, apiKeySource.Value, new NuGetSetApiKeySettings
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
                var installer = fixture.CreateSetApiKey();

                // When
                var result = Record.Exception(() => installer.SetApiKey("*secret key*", "http://nugetfeed.com", new NuGetSetApiKeySettings()));

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
                var installer = fixture.CreateSetApiKey();

                // When
                var result = Record.Exception(() => installer.SetApiKey("*secret key*", "http://nugetfeed.com", new NuGetSetApiKeySettings()));

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("NuGet: Process returned an error.", result.Message);
            }

            [Fact]
            public void Should_Find_NuGet_Executable_If_Tool_Path_Not_Provided()
            {
                // Given
                var apiKeySource = new KeyValuePair<string, string>("*secret key*", "http://nugetfeed.com");
                var fixture = new NuGetFixture(setApiKey:apiKeySource);
                var installer = fixture.CreateSetApiKey();
                
                // When
                installer.SetApiKey("*secret key*", "http://nugetfeed.com", new NuGetSetApiKeySettings());

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Is<FilePath>(p => p.FullPath == "/Working/tools/NuGet.exe"),
                    Arg.Any<ProcessSettings>());
            }

            [Fact]
            public void Should_Add_Mandatory_Arguments()
            {
                // Given
                var apiKeySource = new KeyValuePair<string, string>("*secret key*", "http://nugetfeed.com");
                var fixture = new NuGetFixture(setApiKey:apiKeySource);
                var installer = fixture.CreateSetApiKey();
                var expected = string.Format("setapikey \"{0}\" -Source \"{1}\" -NonInteractive", apiKeySource.Key, apiKeySource.Value);

                // When
                installer.SetApiKey(apiKeySource.Key, apiKeySource.Value, new NuGetSetApiKeySettings());

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(p => 
                        p.Arguments.Render() == expected));
            }

            [Theory]
            [InlineData(NuGetVerbosity.Detailed, "detailed")]
            [InlineData(NuGetVerbosity.Normal, "normal")]
            [InlineData(NuGetVerbosity.Quiet, "quiet")]
            public void Should_Add_Verbosity_To_Arguments_If_Set(NuGetVerbosity verbosity, string name)
            {
                // Given
                var apiKeySource = new KeyValuePair<string, string>("*secret key*", "http://nugetfeed.com");
                var fixture = new NuGetFixture(setApiKey:apiKeySource);
                var installer = fixture.CreateSetApiKey();
                var expected = string.Format("setapikey \"{0}\" -Source \"{1}\" -Verbosity {2} -NonInteractive", apiKeySource.Key, apiKeySource.Value, name);

                // When
                installer.SetApiKey(apiKeySource.Key, apiKeySource.Value, new NuGetSetApiKeySettings
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
                var apiKeySource = new KeyValuePair<string, string>("*secret key*", "http://nugetfeed.com");
                var fixture = new NuGetFixture(setApiKey:apiKeySource);
                var installer = fixture.CreateSetApiKey();
                var expected = string.Format("setapikey \"{0}\" -Source \"{1}\" -ConfigFile \"/Working/nuget.config\" -NonInteractive", apiKeySource.Key, apiKeySource.Value);
                
                // When
                installer.SetApiKey(apiKeySource.Key, apiKeySource.Value, new NuGetSetApiKeySettings
                {
                    ConfigFile = "./nuget.config"
                });

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() == expected));

            }
        }
    }
}
