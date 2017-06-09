// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tests.Fixtures.Tools;
using Cake.Common.Tools.Fixie;
using Cake.Core;
using Cake.Testing;
using Cake.Testing.Xunit;
using Xunit;

namespace Cake.Common.Tests.Unit.Tools.Fixie
{
    public sealed class FixieRunnerTests
    {
        public sealed class TheRunMethod
        {
            [Fact]
            public void Should_Throw_If_Assembly_Paths_Are_Null()
            {
                // Given
                var fixture = new FixieRunnerFixture();
                fixture.AssemblyPaths = null;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "assemblyPaths");
            }

            [Fact]
            public void Should_Throw_If_Fixie_Runner_Was_Not_Found()
            {
                // Given
                var fixture = new FixieRunnerFixture();
                fixture.GivenDefaultToolDoNotExist();

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("Fixie: Could not locate executable.", result?.Message);
            }

            [Theory]
            [InlineData("/bin/tools/Fixie/Fixie.Console.exe", "/bin/tools/Fixie/Fixie.Console.exe")]
            [InlineData("./tools/Fixie/Fixie.Console.exe", "/Working/tools/Fixie/Fixie.Console.exe")]
            public void Should_Use_Fixie_Runner_From_Tool_Path_If_Provided(string toolPath, string expected)
            {
                // Given
                var fixture = new FixieRunnerFixture();
                fixture.Settings.ToolPath = toolPath;
                fixture.GivenSettingsToolPathExist();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Path.FullPath);
            }

            [WindowsTheory]
            [InlineData("C:/Fixie/Fixie.Console.exe", "C:/Fixie/Fixie.Console.exe")]
            public void Should_Use_Fixie_Runner_From_Tool_Path_If_Provided_On_Windows(string toolPath, string expected)
            {
                // Given
                var fixture = new FixieRunnerFixture();
                fixture.Settings.ToolPath = toolPath;
                fixture.GivenSettingsToolPathExist();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Path.FullPath);
            }

            [Fact]
            public void Should_Find_Fixie_Runner_If_Tool_Path_Not_Provided()
            {
                // Given
                var fixture = new FixieRunnerFixture();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/Working/tools/Fixie.Console.exe", result.Path.FullPath);
            }

            [Fact]
            public void Should_Use_Provided_Assembly_Path_In_Process_Arguments()
            {
                // Given
                var fixture = new FixieRunnerFixture();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("\"/Working/Test1.dll\"", result.Args);
            }

            [Fact]
            public void Should_Use_Provided_Assembly_Paths_In_Process_Arguments()
            {
                // Given
                var fixture = new FixieRunnerFixture();
                fixture.AssemblyPaths.Clear();
                fixture.AssemblyPaths.Add("./Test1.dll");
                fixture.AssemblyPaths.Add("./Test2.dll");

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("\"/Working/Test1.dll\" \"/Working/Test2.dll\"", result.Args);
            }

            [Fact]
            public void Should_Set_Working_Directory()
            {
                // Given
                var fixture = new FixieRunnerFixture();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/Working", result.Process.WorkingDirectory.FullPath);
            }

            [Fact]
            public void Should_Throw_If_Process_Was_Not_Started()
            {
                // Given
                var fixture = new FixieRunnerFixture();
                fixture.GivenProcessCannotStart();

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("Fixie: Process was not started.", result?.Message);
            }

            [Fact]
            public void Should_Throw_If_Process_Has_A_Non_Zero_Exit_Code()
            {
                // Given
                var fixture = new FixieRunnerFixture();
                fixture.GivenProcessExitsWithCode(1);

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("Fixie: Process returned an error (exit code 1).", result?.Message);
            }

            [Fact]
            public void Should_Set_NUnitXml_Output_File()
            {
                // Given
                var fixture = new FixieRunnerFixture();
                fixture.Settings.NUnitXml = "nunit-style-results.xml";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("\"/Working/Test1.dll\" " +
                             "--NUnitXml \"/Working/nunit-style-results.xml\"",
                             result.Args);
            }

            [Fact]
            public void Should_Set_xUnitXml_Output_File()
            {
                // Given
                var fixture = new FixieRunnerFixture();
                fixture.Settings.XUnitXml = "xunit-results.xml";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("\"/Working/Test1.dll\" " +
                             "--xUnitXml \"/Working/xunit-results.xml\"",
                             result.Args);
            }

            [Theory]
            [InlineData(true, "on", "\"/Working/Test1.dll\" --TeamCity on")]
            [InlineData(false, "off", "\"/Working/Test1.dll\" --TeamCity off")]
            public void Should_Set_TeamCity_Value(bool teamCityOutput, string teamCityValue, string expected)
            {
                // Given
                var fixture = new FixieRunnerFixture();
                fixture.Settings.TeamCity = teamCityOutput;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Fact]
            public void Should_Set_Custom_Options()
            {
                // Given
                var fixture = new FixieRunnerFixture();
                fixture.Settings.WithOption("--include", "CategoryA");
                fixture.Settings.WithOption("--include", "CategoryB");

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("\"/Working/Test1.dll\" " +
                             "--include CategoryA " +
                             "--include CategoryB", result.Args);
            }

            [Fact]
            public void Should_Set_Multiple_Options()
            {
                // Given
                var fixture = new FixieRunnerFixture();
                fixture.Settings.WithOption("--type", "fast");
                fixture.Settings.WithOption("--include", "CategoryA", "CategoryB");
                fixture.Settings.WithOption("--output", "fixie-log.txt");

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("\"/Working/Test1.dll\" --type fast " +
                             "--include CategoryA --include CategoryB " +
                             "--output fixie-log.txt", result.Args);
            }
        }
    }
}