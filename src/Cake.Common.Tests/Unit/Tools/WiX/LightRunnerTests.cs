// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using Cake.Common.Tests.Fixtures.Tools;
using Cake.Core;
using Cake.Core.IO;
using Cake.Testing;
using Cake.Testing.Xunit;
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
                var fixture = new LightFixture();
                fixture.ObjectFiles = null;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "objectFiles");
            }

            [Fact]
            public void Should_Throw_If_Object_Files_Is_Empty()
            {
                // Given
                var fixture = new LightFixture();
                fixture.ObjectFiles = new List<FilePath>();

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsType<ArgumentException>(result);
                Assert.Equal("objectFiles", ((ArgumentException)result)?.ParamName);
            }

            [Fact]
            public void Should_Throw_If_Settings_Is_Null()
            {
                // Given
                var fixture = new LightFixture();
                fixture.Settings = null;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "settings");
            }

            [Fact]
            public void Should_Throw_If_Light_Runner_Was_Not_Found()
            {
                // Given
                var fixture = new LightFixture();
                fixture.GivenDefaultToolDoNotExist();

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("Light: Could not locate executable.", result?.Message);
            }

            [Theory]
            [InlineData("/bin/tools/WiX/light.exe", "/bin/tools/WiX/light.exe")]
            [InlineData("./tools/WiX/light.exe", "/Working/tools/WiX/light.exe")]
            public void Should_Use_Light_Runner_From_Tool_Path_If_Provided(string toolPath, string expected)
            {
                // Given
                var fixture = new LightFixture();
                fixture.Settings.ToolPath = toolPath;
                fixture.GivenSettingsToolPathExist();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Path.FullPath);
            }

            [WindowsTheory]
            [InlineData("C:/WiX/light.exe", "C:/WiX/light.exe")]
            public void Should_Use_Light_Runner_From_Tool_Path_If_Provided_On_Windows(string toolPath, string expected)
            {
                // Given
                var fixture = new LightFixture();
                fixture.Settings.ToolPath = toolPath;
                fixture.GivenSettingsToolPathExist();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Path.FullPath);
            }

            [Fact]
            public void Should_Find_Light_Runner_If_Tool_Path_Not_Provided()
            {
                // Given
                var fixture = new LightFixture();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/Working/tools/light.exe", result.Path.FullPath);
            }

            [Fact]
            public void Should_Use_Provided_Object_Files_In_Process_Arguments()
            {
                // Given
                var fixture = new LightFixture();
                fixture.ObjectFiles = new List<FilePath>();
                fixture.ObjectFiles.Add(new FilePath("./Test.wixobj"));
                fixture.ObjectFiles.Add(new FilePath("./Test2.wixobj"));

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("\"/Working/Test.wixobj\" \"/Working/Test2.wixobj\"", result.Args);
            }

            [Fact]
            public void Should_Set_Working_Directory()
            {
                // Given
                var fixture = new LightFixture();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/Working", result.Process.WorkingDirectory.FullPath);
            }

            [Fact]
            public void Should_Throw_If_Process_Was_Not_Started()
            {
                // Given
                var fixture = new LightFixture();
                fixture.GivenProcessCannotStart();

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("Light: Process was not started.", result?.Message);
            }

            [Fact]
            public void Should_Throw_If_Process_Has_A_Non_Zero_Exit_Code()
            {
                // Given
                var fixture = new LightFixture();
                fixture.GivenProcessExitsWithCode(1);

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("Light: Process returned an error (exit code 1).", result?.Message);
            }

            [Fact]
            public void Should_Add_Defines_To_Arguments_If_Provided()
            {
                // Given
                var fixture = new LightFixture();
                fixture.Settings.Defines = new Dictionary<string, string>();
                fixture.Settings.Defines.Add("Foo", "Bar");

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("-dFoo=Bar \"/Working/Test.wixobj\"", result.Args);
            }

            [Fact]
            public void Should_Add_Extensions_To_Arguments_If_Provided()
            {
                // Given
                var fixture = new LightFixture();
                fixture.Settings.Extensions = new[] { "WixUIExtension" };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("-ext WixUIExtension \"/Working/Test.wixobj\"", result.Args);
            }

            [Fact]
            public void Should_Add_NoLogo_To_Arguments_If_Provided()
            {
                // Given
                var fixture = new LightFixture();
                fixture.Settings.NoLogo = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("-nologo \"/Working/Test.wixobj\"", result.Args);
            }

            [Fact]
            public void Should_Add_Output_Directory_To_Arguments_If_Provided()
            {
                // Given
                var fixture = new LightFixture();
                fixture.Settings.OutputFile = "./bin/test.msi";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("-o \"/Working/bin/test.msi\" \"/Working/Test.wixobj\"", result.Args);
            }

            [Fact]
            public void Should_Add_RawArguments_To_Arguments_If_Provided()
            {
                // Given
                var fixture = new LightFixture();
                fixture.Settings.RawArguments = "-dFoo=Bar";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("-dFoo=Bar \"/Working/Test.wixobj\"", result.Args);
            }
        }
    }
}