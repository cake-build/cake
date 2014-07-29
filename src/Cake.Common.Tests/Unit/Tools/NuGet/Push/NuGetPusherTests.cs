using System;
using System.Diagnostics;
using Cake.Common.Tests.Fixtures;
using Cake.Common.Tools.NuGet;
using Cake.Common.Tools.NuGet.Push;
using Cake.Core;
using Cake.Core.IO;
using NSubstitute;
using Xunit;
using Xunit.Extensions;

namespace Cake.Common.Tests.Unit.Tools.NuGet.Push
{
    public sealed class NuGetPusherTests
    {
        public sealed class ThePushMethod
        {
            [Fact]
            public void Should_Throw_If_Nuspec_File_Path_Is_Null()
            {
                // Given
                var fixture = new NuGetFixture();
                var pusher = fixture.CreatePusher();

                // When
                var result = Record.Exception(() => pusher.Push(null, new NuGetPushSettings()));

                // Then
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("packageFilePath", ((ArgumentNullException)result).ParamName);
            }

            [Fact]
            public void Should_Throw_If_Settings_Is_Null()
            {
                // Given
                var fixture = new NuGetFixture();
                var pusher = fixture.CreatePusher();

                // When
                var result = Record.Exception(() => pusher.Push("./existing.nupkg", null));

                // Then
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("settings", ((ArgumentNullException)result).ParamName);
            }

            [Fact]
            public void Should_Throw_If_NuGet_Executable_Was_Not_Found()
            {
                // Given
                var fixture = new NuGetFixture(defaultToolExist: false);
                var pusher = fixture.CreatePusher();

                // When
                var result = Record.Exception(() => pusher.Push("./existing.nupkg", new NuGetPushSettings()));

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
                var pusher = fixture.CreatePusher();

                // When
                pusher.Push("./existing.nupkg", new NuGetPushSettings
                {
                    ToolPath = toolPath
                });

                // Then
                fixture.ProcessRunner.Received(1).Start(Arg.Is<ProcessStartInfo>(
                    p => p.FileName == expected));
            }

            [Fact]
            public void Should_Find_NuGet_Executable_If_Tool_Path_Not_Provided()
            {
                // Given
                var fixture = new NuGetFixture();
                var pusher = fixture.CreatePusher();

                // When
                pusher.Push("./existing.nupkg", new NuGetPushSettings());

                // Then
                fixture.ProcessRunner.Received(1).Start(Arg.Is<ProcessStartInfo>(
                    p => p.FileName == "/Working/tools/NuGet.exe"));
            }

            [Fact]
            public void Should_Throw_If_Process_Was_Not_Started()
            {
                // Given
                var fixture = new NuGetFixture();
                fixture.ProcessRunner.Start(Arg.Any<ProcessStartInfo>()).Returns((IProcess)null);
                var pusher = fixture.CreatePusher();

                // When
                var result = Record.Exception(() => pusher.Push("./existing.nupkg", new NuGetPushSettings()));

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
                var pusher = fixture.CreatePusher();

                // When
                var result = Record.Exception(() => pusher.Push("./existing.nupkg", new NuGetPushSettings()));

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("NuGet: Process returned an error.", result.Message);
            }

            [Fact]
            public void Should_Add_NuGet_Package_To_Arguments()
            {
                // Given
                var fixture = new NuGetFixture();
                var pusher = fixture.CreatePusher();

                // When
                pusher.Push("./existing.nupkg", new NuGetPushSettings
               {
                    NonInteractive = false
                });

                // Then
                fixture.ProcessRunner.Received(1).Start(Arg.Is<ProcessStartInfo>(
                    p => p.Arguments == "push \"/Working/existing.nupkg\""));
            }

            [Fact]
            public void Should_Add_Api_Key_To_Arguments_If_Not_Null()
            {
                // Given
                var fixture = new NuGetFixture();
                var pusher = fixture.CreatePusher();

                // When
                pusher.Push("./existing.nupkg", new NuGetPushSettings
                {
                    NonInteractive = false,
                    ApiKey = "1234"
                });

                // Then
                fixture.ProcessRunner.Received(1).Start(Arg.Is<ProcessStartInfo>(
                    p => p.Arguments == "push \"/Working/existing.nupkg\" 1234"));
            }

            [Fact]
            public void Should_Add_Configuration_File_To_Arguments_If_Not_Null()
            {
                // Given
                var fixture = new NuGetFixture();
                var pusher = fixture.CreatePusher();

                // When
                pusher.Push("./existing.nupkg", new NuGetPushSettings
                {
                    NonInteractive = false,
                    ConfigFile = "./NuGet.config"
                });

                // Then
                fixture.ProcessRunner.Received(1).Start(Arg.Is<ProcessStartInfo>(
                    p => p.Arguments == "push \"/Working/existing.nupkg\" -ConfigFile \"/Working/NuGet.config\""));
            }

            [Fact]
            public void Should_Add_Non_Interactive_Flag_To_Arguments_If_True()
            {
                // Given
                var fixture = new NuGetFixture();
                var pusher = fixture.CreatePusher();

                // When
                pusher.Push("./existing.nupkg", new NuGetPushSettings
                {
                    NonInteractive = true
                });

                // Then
                fixture.ProcessRunner.Received(1).Start(Arg.Is<ProcessStartInfo>(
                    p => p.Arguments == "push \"/Working/existing.nupkg\" -NonInteractive"));
            }

            [Fact]
            public void Should_Add_Source_To_Arguments_If_Not_Null()
            {
                // Given
                var fixture = new NuGetFixture();
                var pusher = fixture.CreatePusher();

                // When
                pusher.Push("./existing.nupkg", new NuGetPushSettings
                {
                    NonInteractive = false,
                    Source = "http://customsource/"
                });

                // Then
                fixture.ProcessRunner.Received(1).Start(Arg.Is<ProcessStartInfo>(
                    p => p.Arguments == "push \"/Working/existing.nupkg\" -Source \"http://customsource/\""));
            }

            [Fact]
            public void Should_Add_Timeout_To_Arguments_If_Not_Null()
            {
                // Given
                var fixture = new NuGetFixture();
                var pusher = fixture.CreatePusher();

                // When
                pusher.Push("./existing.nupkg", new NuGetPushSettings
                {
                    NonInteractive = false,
                    Timeout = TimeSpan.FromSeconds(987) 
                });

                // Then
                fixture.ProcessRunner.Received(1).Start(Arg.Is<ProcessStartInfo>(
                    p => p.Arguments == "push \"/Working/existing.nupkg\" -Timeout 987"));
            }

            [Theory]
            [InlineData(NuGetVerbosity.Detailed, "detailed")]
            [InlineData(NuGetVerbosity.Normal, "normal")]
            [InlineData(NuGetVerbosity.Quiet, "quiet")]
            public void Should_Add_Verbosity_To_Arguments_If_Not_Null(NuGetVerbosity verbosity, string name)
            {
                // Given
                var fixture = new NuGetFixture();
                var pusher = fixture.CreatePusher();
                var expected = string.Format("push \"/Working/existing.nupkg\" -Verbosity {0}", name);

                // When
                pusher.Push("./existing.nupkg", new NuGetPushSettings
                {
                    NonInteractive = false,
                    Verbosity = verbosity
                });

                // Then
                fixture.ProcessRunner.Received(1).Start(Arg.Is<ProcessStartInfo>(
                    p => p.Arguments == expected));
            }
        }
    }
}
