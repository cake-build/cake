using System;
using System.Collections.Generic;
using System.Linq;
using Cake.Common.Tests.Fixtures;
using Cake.Common.Tools.WiX;
using Cake.Core;
using Cake.Core.IO;
using NSubstitute;
using Xunit;

namespace Cake.Common.Tests.Unit.Tools.WiX
{
    public sealed class LightRunnerTests
    {
        public sealed class TheRunMethod
        {
            [Fact]
            public void Should_Throw_If_Object_Files_Is_Null()
            {
                // Given
                var fixture = new WiXFixture();
                var runner = fixture.CreateLightRunner();

                // When
                var result = Record.Exception(() => runner.Run(null, new LightSettings()));

                // Then
                Assert.IsArgumentNullException(result, "objectFiles");
            }

            [Fact]
            public void Should_Throw_If_Object_Files_Is_Empty()
            {
                // Given
                var fixture = new WiXFixture();
                var runner = fixture.CreateLightRunner();

                // When
                var result = Record.Exception(() => runner.Run(new FilePath[0], new LightSettings()));

                // Then
                Assert.IsType<ArgumentException>(result);
                Assert.Equal("objectFiles", ((ArgumentException)result).ParamName);
            }

            [Fact]
            public void Should_Throw_If_Settings_Is_Null()
            {
                // Given
                var fixture = new WiXFixture();
                var runner = fixture.CreateLightRunner();

                // When
                var result = Record.Exception(() => runner.Run(new[] {new FilePath("/Working/AssemblyFile.lol")}, null));

                // Then
                Assert.IsArgumentNullException(result, "settings");
            }

            [Fact]
            public void Should_Throw_If_Light_Runner_Was_Not_Found()
            {
                // Given
                var fixture = new WiXFixture();
                fixture.Globber.Match("./tools/**/light.exe").Returns(Enumerable.Empty<Path>());
                var runner = fixture.CreateLightRunner();

                // When
                var result = Record.Exception(() => runner.Run(new[] {new FilePath("/Test.wixobj")}, new LightSettings()));

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("Light: Could not locate executable.", result.Message);
            }

            [Theory]
            [InlineData("C:/WiX/light.exe", "C:/WiX/light.exe")]
            [InlineData("./tools/WiX/light.exe", "/Working/tools/WiX/light.exe")]
            public void Should_Use_Light_Runner_From_Tool_Path_If_Provided(string toolPath, string expected)
            {
                // Given
                var fixture = new WiXFixture(expected);
                var runner = fixture.CreateLightRunner();

                // When
                runner.Run(new[] {new FilePath("./Test.wixobj")}, new LightSettings
                {
                    ToolPath = toolPath
                });

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Is<FilePath>(p => p.FullPath == expected),
                    Arg.Any<ProcessSettings>());
            }

            [Fact]
            public void Should_Find_Light_Runner_If_Tool_Path_Not_Provided()
            {
                // Given
                var fixture = new WiXFixture();
                var runner = fixture.CreateLightRunner();

                // When
                runner.Run(new[] {new FilePath("./Test.wixobj")}, new LightSettings());

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Is<FilePath>(p => p.FullPath == "/Working/tools/light.exe"),
                    Arg.Any<ProcessSettings>());
            }

            [Fact]
            public void Should_Use_Provided_Object_Files_In_Process_Arguments()
            {
                // Given
                var fixture = new WiXFixture();
                var runner = fixture.CreateLightRunner();

                // When
                runner.Run(new[] {new FilePath("./Test.wixobj"), new FilePath("./Test2.wixobj")}, new LightSettings());

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() == "\"/Working/Test.wixobj\" \"/Working/Test2.wixobj\""));
            }

            [Fact]
            public void Should_Set_Working_Directory()
            {
                // Given
                var fixture = new WiXFixture();
                var runner = fixture.CreateLightRunner();

                // When
                runner.Run(new[] { new FilePath("./Test.wixobj") }, new LightSettings());

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), 
                    Arg.Is<ProcessSettings>(p => 
                        p.WorkingDirectory.FullPath == "/Working"));
            }

            [Fact]
            public void Should_Throw_If_Process_Was_Not_Started()
            {
                // Given
                var fixture = new WiXFixture();
                fixture.ProcessRunner.Start(Arg.Any<FilePath>(), Arg.Any<ProcessSettings>()).Returns((IProcess)null);
                var runner = fixture.CreateLightRunner();

                // When
                var result = Record.Exception(() => runner.Run(new[] { new FilePath("./Test.wixobj") }, new LightSettings()));

                // Then
                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("Light: Process was not started.", result.Message);   
            }

            [Fact]
            public void Should_Throw_If_Process_Has_A_Non_Zero_Exit_Code()
            {
                // Given
                var fixture = new WiXFixture();
                fixture.Process.GetExitCode().Returns(1);
                var runner = fixture.CreateLightRunner();

                // When
                var result = Record.Exception(() => runner.Run(new[] { new FilePath("./Test.wixobj") }, new LightSettings()));

                // Then
                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("Light: Process returned an error.", result.Message);
            }

            [Fact]
            public void Should_Add_Defines_To_Arguments_If_Provided()
            {
                // Given
                var fixture = new WiXFixture();
                var runner = fixture.CreateLightRunner();

                // When
                runner.Run(new[] { new FilePath("./Test.wixobj") }, new LightSettings
                {
                    Defines = new Dictionary<string, string>
                    {
                        { "Foo", "Bar" }
                    }
                });

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(),
                    Arg.Is<ProcessSettings>(p => 
                        p.Arguments.Render() == "-dFoo=Bar \"/Working/Test.wixobj\""));
            }

            [Fact]
            public void Should_Add_Extensions_To_Arguments_If_Provided()
            {
                // Given
                var fixture = new WiXFixture();
                var runner = fixture.CreateLightRunner();

                // When
                runner.Run(new[] {new FilePath("./Test.wixobj")}, new LightSettings
                {
                    Extensions = new[] {"WixUIExtension"}
                });

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), 
                    Arg.Is<ProcessSettings>(p => 
                        p.Arguments.Render() == "-ext WixUIExtension \"/Working/Test.wixobj\""));
            }

            [Fact]
            public void Should_Add_NoLogo_To_Arguments_If_Provided()
            {
                // Given
                var fixture = new WiXFixture();
                var runner = fixture.CreateLightRunner();

                // When
                runner.Run(new[] { new FilePath("./Test.wixobj") }, new LightSettings
                {
                    NoLogo = true
                });

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), 
                    Arg.Is<ProcessSettings>(p => 
                        p.Arguments.Render() == "-nologo \"/Working/Test.wixobj\""));
            }

            [Fact]
            public void Should_Add_Output_Directory_To_Arguments_If_Provided()
            {
                // Given
                var fixture = new WiXFixture();
                var runner = fixture.CreateLightRunner();

                // When
                runner.Run(new[] { new FilePath("./Test.wixobj") }, new LightSettings
                {
                    OutputFile = "./bin/test.msi"
                });

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), 
                    Arg.Is<ProcessSettings>(p => 
                        p.Arguments.Render() == "-o \"/Working/bin/test.msi\" \"/Working/Test.wixobj\""));
            }

            [Fact]
            public void Should_Add_RawArguments_To_Arguments_If_Provided()
            {
                // Given
                var fixture = new WiXFixture();
                var runner = fixture.CreateLightRunner();

                // When
                runner.Run(new[] { new FilePath("./Test.wixobj") }, new LightSettings
                {
                    RawArguments = "-dFoo=Bar"
                });

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), 
                    Arg.Is<ProcessSettings>(p => 
                        p.Arguments.Render() == "-dFoo=Bar \"/Working/Test.wixobj\""));
            }
        }
    }
}
