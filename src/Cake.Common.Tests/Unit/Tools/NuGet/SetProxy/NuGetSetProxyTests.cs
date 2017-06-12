// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tests.Fixtures.Tools.NuGet.SetProxy;
using Cake.Common.Tools.NuGet;
using Cake.Testing;
using Cake.Testing.Xunit;
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
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "url");
            }

            [Fact]
            public void Should_Throw_If_Settings_Are_Null()
            {
                // Given
                var fixture = new NuGetSetProxyFixture();
                fixture.Settings = null;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "settings");
            }

            [Fact]
            public void Should_Throw_If_Encounter_Unexpected_Output()
            {
                // Given
                var fixture = new NuGetSetProxyFixture();
                fixture.GivenUnexpectedOutput();

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsCakeException(result, "Set command returned unexpected response.");
            }

            [Fact]
            public void Should_Throw_If_NuGet_Executable_Was_Not_Found()
            {
                // Given
                var fixture = new NuGetSetProxyFixture();
                fixture.GivenDefaultToolDoNotExist();

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsCakeException(result, "NuGet: Could not locate executable.");
            }

            [Theory]
            [InlineData("/bin/tools/nuget/nuget.exe", "/bin/tools/nuget/nuget.exe")]
            [InlineData("./tools/nuget/nuget.exe", "/Working/tools/nuget/nuget.exe")]
            public void Should_Use_NuGet_Executable_From_Tool_Path_If_Provided(string toolPath, string expected)
            {
                // Given
                var fixture = new NuGetSetProxyFixture();
                fixture.Settings.ToolPath = toolPath;
                fixture.GivenSettingsToolPathExist();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Path.FullPath);
            }

            [WindowsTheory]
            [InlineData("C:/nuget/nuget.exe", "C:/nuget/nuget.exe")]
            public void Should_Use_NuGet_Executable_From_Tool_Path_If_Provided_On_Windows(string toolPath, string expected)
            {
                // Given
                var fixture = new NuGetSetProxyFixture();
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
                var fixture = new NuGetSetProxyFixture();
                fixture.GivenProcessCannotStart();

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsCakeException(result, "NuGet: Process was not started.");
            }

            [Fact]
            public void Should_Throw_If_Process_Has_A_Non_Zero_Exit_Code()
            {
                // Given
                var fixture = new NuGetSetProxyFixture();
                fixture.GivenProcessExitsWithCode(1);

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsCakeException(result, "NuGet: Process returned an error (exit code 1).");
            }

            [Fact]
            public void Should_Find_NuGet_Executable_If_Tool_Path_Not_Provided()
            {
                // Given
                var fixture = new NuGetSetProxyFixture();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/Working/tools/NuGet.exe", result.Path.FullPath);
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
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Fact]
            public void Should_Add_UP_To_Arguments_If_Set()
            {
                // Given
                var fixture = new NuGetSetProxyFixture();
                fixture.Username = "Admin";
                fixture.Password = "Pass1";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("config -Set http_proxy=http://a.com " +
                             "-Set http_proxy.user=Admin -Set http_proxy.password=Pass1 " +
                             "-NonInteractive", result.Args);
            }

            [Fact]
            public void Should_Add_ConfigFile_To_Arguments_If_Set()
            {
                // Given
                var fixture = new NuGetSetProxyFixture();
                fixture.Settings.ConfigFile = "./nuget.config";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("config -Set http_proxy=http://a.com " +
                             "-ConfigFile \"/Working/nuget.config\" " +
                             "-NonInteractive", result.Args);
            }

            [Fact]
            public void Should_Add_Mandatory_Arguments()
            {
                // Given
                var fixture = new NuGetSetProxyFixture();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("config -Set http_proxy=http://a.com -NonInteractive", result.Args);
            }
        }
    }
}