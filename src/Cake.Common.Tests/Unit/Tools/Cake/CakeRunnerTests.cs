// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.IO;
using Cake.Common.Tests.Fixtures.Tools;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Testing;
using Cake.Testing.Xunit;
using Xunit;

namespace Cake.Common.Tests.Unit.Tools.Cake
{
    public sealed class CakeRunnerTests
    {
        public sealed class TheExecuteScriptMethod
        {
            [Fact]
            public void Should_Throw_If_Script_Path_Was_Null()
            {
                // Given
                var fixture = new CakeRunnerFixture();
                fixture.ScriptPath = null;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "scriptPath");
            }

            [Fact]
            public void Should_Throw_If_Script_Is_Missing()
            {
                // Given
                var fixture = new CakeRunnerFixture();
                fixture.ScriptPath = "/Working/missing.cake";

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsType<FileNotFoundException>(result);
                Assert.Equal("Cake script file not found.", result?.Message);
            }

            [Fact]
            public void Should_Throw_If_Cake_Executable_Was_Not_Found()
            {
                // Given
                var fixture = new CakeRunnerFixture();
                fixture.GivenDefaultToolDoNotExist();

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("Cake: Could not locate executable.", result?.Message);
            }

            [Theory]
            [InlineData("/bin/Cake/Cake.exe", "/bin/Cake/Cake.exe")]
            [InlineData("./tools/Cake/Cake.exe", "/Working/tools/Cake/Cake.exe")]
            public void Should_Use_Cake_Executable_From_Tool_Path_If_Provided(string toolPath, string expected)
            {
                // Given
                var fixture = new CakeRunnerFixture();
                fixture.Settings.ToolPath = toolPath;
                fixture.GivenSettingsToolPathExist();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Path.FullPath);
            }

            [WindowsTheory]
            [InlineData("C:/Cake/Cake.exe", "C:/Cake/Cake.exe")]
            public void Should_Use_Cake_Executable_From_Tool_Path_If_Provided_On_Windows(string toolPath, string expected)
            {
                // Given
                var fixture = new CakeRunnerFixture();
                fixture.Settings.ToolPath = toolPath;
                fixture.GivenSettingsToolPathExist();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Path.FullPath);
            }

            [Fact]
            public void Should_Add_Provided_Script_To_Process_Arguments()
            {
                // Given
                var fixture = new CakeRunnerFixture();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("\"/Working/build.cake\"", result.Args);
            }

            [Fact]
            public void Should_Add_Provided_Verbosity_To_Process_Arguments()
            {
                // Given
                var fixture = new CakeRunnerFixture();
                fixture.Settings.Verbosity = Verbosity.Diagnostic;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("\"/Working/build.cake\" -verbosity=Diagnostic", result.Args);
            }

            [Fact]
            public void Should_Add_Provided_Arguments_To_Process_Arguments()
            {
                // Given
                var fixture = new CakeRunnerFixture();
                fixture.Settings.Arguments = new Dictionary<string, string>();
                fixture.Settings.Arguments.Add("target", "Build");
                fixture.Settings.Arguments.Add("configuration", "Debug");

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("\"/Working/build.cake\" " +
                             "-target=\"Build\" " +
                             "-configuration=\"Debug\"", result.Args);
            }
        }
    }
}