// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tests.Fixtures.Tools.VSWhere.Legacy;
using Cake.Testing;
using Cake.Testing.Xunit;
using Xunit;

namespace Cake.Common.Tests.Unit.Tools.VSWhere.Legacy
{
    public class VSWhereLegacyTests
    {
        public sealed class TheLegacyMethod
        {
            [Fact]
            public void Should_Throw_If_Settings_Are_Null()
            {
                // Given
                var fixture = new VSWhereLegacyFixture();
                fixture.Settings = null;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "settings");
            }

            [Fact]
            public void Should_Throw_If_VSWhere_Executable_Was_Not_Found()
            {
                // Given
                var fixture = new VSWhereLegacyFixture();
                fixture.GivenDefaultToolDoNotExist();

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsCakeException(result, "VSWhere: Could not locate executable.");
            }

            [Theory]
            [InlineData("/bin/vswhere/vswhere.exe", "/bin/vswhere/vswhere.exe")]
            [InlineData("./tools/vswhere/vswhere.exe", "/Working/tools/vswhere/vswhere.exe")]
            public void Should_Use_VSWhere_Executable_From_Tool_Path_If_Provided(string toolPath, string expected)
            {
                // Given
                var fixture = new VSWhereLegacyFixture();
                fixture.Settings.ToolPath = toolPath;
                fixture.GivenSettingsToolPathExist();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Path.FullPath);
            }

            [WindowsTheory]
            [InlineData("C:/vswhere/vswhere.exe", "C:/vswhere/vswhere.exe")]
            public void Should_Use_VSWhere_Executable_From_Tool_Path_If_Provided_On_Windows(string toolPath, string expected)
            {
                // Given
                var fixture = new VSWhereLegacyFixture();
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
                var fixture = new VSWhereLegacyFixture();
                fixture.GivenProcessCannotStart();

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsCakeException(result, "VSWhere: Process was not started.");
            }

            [Fact]
            public void Should_Throw_If_Process_Has_A_Non_Zero_Exit_Code()
            {
                // Given
                var fixture = new VSWhereLegacyFixture();
                fixture.GivenProcessExitsWithCode(1);

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsCakeException(result, "VSWhere: Process returned an error (exit code 1).");
            }

            [Fact]
            public void Should_Find_VSWhere_Executable_If_Tool_Path_Not_Provided()
            {
                // Given
                var fixture = new VSWhereLegacyFixture();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/Working/tools/vswhere.exe", result.Path.FullPath);
            }

            [Fact]
            public void Should_Add_Mandatory_Arguments()
            {
                // Given
                var fixture = new VSWhereLegacyFixture();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("-legacy -property installationPath -nologo", result.Args);
            }

            [Fact]
            public void Should_Add_Version_To_Arguments_If_Set()
            {
                // Given
                var fixture = new VSWhereLegacyFixture();
                fixture.Settings.Version = "15";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("-legacy -version \"15\" -property installationPath -nologo", result.Args);
            }

            [Fact]
            public void Should_Not_Include_Property_To_Arguments_If_Set_To_Empty()
            {
                // Given
                var fixture = new VSWhereLegacyFixture();
                fixture.Settings.ReturnProperty = string.Empty;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("-legacy -nologo", result.Args);
            }

            [Fact]
            public void Should_Include_Latest_To_Arguments_If_Set_To_True()
            {
                // Given
                var fixture = new VSWhereLegacyFixture();
                fixture.Settings.Latest = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("-legacy -latest -property installationPath -nologo", result.Args);
            }
        }
    }
}
