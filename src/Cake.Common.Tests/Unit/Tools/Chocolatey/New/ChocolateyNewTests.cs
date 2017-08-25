// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Common.Tests.Fixtures.Tools.Chocolatey.New;
using Cake.Common.Tests.Properties;
using Cake.Common.Tools.Chocolatey.New;
using Cake.Core;
using Cake.Core.IO;
using Cake.Testing;
using Cake.Testing.Xunit;
using Xunit;

namespace Cake.Common.Tests.Unit.Tools.Chocolatey.New
{
    public sealed class ChocolateyNewTests
    {
        public sealed class TheNewMethod
        {
            [Fact]
            public void Should_Throw_If_Package_Id_Is_Null()
            {
                // Given
                var fixture = new ChocolateyNewFixture();
                fixture.PackageId = null;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "packageId");
            }

            [Fact]
            public void Should_Throw_If_Settings_Are_Null()
            {
                // Given
                var fixture = new ChocolateyNewFixture();
                fixture.Settings = null;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "settings");
            }

            [Fact]
            public void Should_Throw_If_Chocolatey_Executable_Was_Not_Found()
            {
                // Given
                var fixture = new ChocolateyNewFixture();
                fixture.GivenDefaultToolDoNotExist();

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsCakeException(result, "Chocolatey: Could not locate executable.");
            }

            [Theory]
            [InlineData("/bin/chocolatey/choco.exe", "/bin/chocolatey/choco.exe")]
            [InlineData("./chocolatey/choco.exe", "/Working/chocolatey/choco.exe")]
            public void Should_Use_Chocolatey_Executable_From_Tool_Path_If_Provided(string toolPath, string expected)
            {
                // Given
                var fixture = new ChocolateyNewFixture();
                fixture.Settings.ToolPath = toolPath;
                fixture.GivenSettingsToolPathExist();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Path.FullPath);
            }

            [WindowsTheory]
            [InlineData("C:/ProgramData/chocolatey/choco.exe", "C:/ProgramData/chocolatey/choco.exe")]
            public void Should_Use_Chocolatey_Executable_From_Tool_Path_If_Provided_On_Windows(string toolPath, string expected)
            {
                // Given
                var fixture = new ChocolateyNewFixture();
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
                var fixture = new ChocolateyNewFixture();
                fixture.GivenProcessCannotStart();

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsCakeException(result, "Chocolatey: Process was not started.");
            }

            [Fact]
            public void Should_Throw_If_Process_Has_A_Non_Zero_Exit_Code()
            {
                // Given
                var fixture = new ChocolateyNewFixture();
                fixture.GivenProcessExitsWithCode(1);

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsCakeException(result, "Chocolatey: Process returned an error (exit code 1).");
            }

            [Fact]
            public void Should_Find_Chocolatey_Executable_If_Tool_Path_Not_Provided()
            {
                // Given
                var fixture = new ChocolateyNewFixture();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/Working/tools/choco.exe", result.Path.FullPath);
            }

            [Fact]
            public void Should_Add_Mandatory_Arguments()
            {
                // Given
                var fixture = new ChocolateyNewFixture();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("new \"MyPackage\" -y", result.Args);
            }

            [Fact]
            public void Should_Add_Package_Version_To_Arguments_If_Not_Null()
            {
                // Given
                var fixture = new ChocolateyNewFixture();
                fixture.Settings.PackageVersion = "1.2.3";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("new \"MyPackage\" -y packageversion=\"1.2.3\"", result.Args);
            }

            [Fact]
            public void Should_Add_Maintainer_Name_To_Arguments_If_Not_Null()
            {
                // Given
                var fixture = new ChocolateyNewFixture();
                fixture.Settings.MaintainerName = "John Doe";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("new \"MyPackage\" -y maintainername=\"John Doe\"", result.Args);
            }

            [Fact]
            public void Should_Add_Maintainer_Repo_To_Arguments_If_Not_Null()
            {
                // Given
                var fixture = new ChocolateyNewFixture();
                fixture.Settings.MaintainerRepo = "johndoe";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("new \"MyPackage\" -y maintainerrepo=\"johndoe\"", result.Args);
            }

            [Fact]
            public void Should_Add_Installer_Type_To_Arguments_If_Not_Null()
            {
                // Given
                var fixture = new ChocolateyNewFixture();
                fixture.Settings.InstallerType = "msi";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("new \"MyPackage\" -y installertype=\"msi\"", result.Args);
            }

            [Fact]
            public void Should_Add_Url_To_Arguments_If_Not_Null()
            {
                // Given
                var fixture = new ChocolateyNewFixture();
                fixture.Settings.Url = "https://example.com";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("new \"MyPackage\" -y url=\"https://example.com\"", result.Args);
            }

            [Fact]
            public void Should_Add_Url64_To_Arguments_If_Not_Null()
            {
                // Given
                var fixture = new ChocolateyNewFixture();
                fixture.Settings.Url64 = "https://example.com";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("new \"MyPackage\" -y url64=\"https://example.com\"", result.Args);
            }

            [Theory]
            [InlineData("-Foo", "new \"MyPackage\" -y silentargs=\"-Foo\"")]
            [InlineData("--Foo", "new \"MyPackage\" -y silentargs=\"--Foo\"")]
            [InlineData("/Foo", "new \"MyPackage\" -y silentargs=\"/Foo\"")]
            [InlineData("-Foo=Bar", "new \"MyPackage\" -y silentargs=\"-Foo=Bar\"")]
            [InlineData("-Foo --Foo /Foo -Foo=Bar", "new \"MyPackage\" -y silentargs=\"-Foo --Foo /Foo -Foo=Bar\"")]
            public void Should_Add_Silent_Args_To_Arguments_If_Set(string silentArgs, string expected)
            {
                // Given
                var fixture = new ChocolateyNewFixture();
                fixture.Settings.SilentArgs = silentArgs;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData("Foo", "Bar", "new \"MyPackage\" -y Foo=\"Bar\"")]
            [InlineData("Foo", "Foo Bar", "new \"MyPackage\" -y Foo=\"Foo Bar\"")]
            public void Should_Add_Additional_Property_Value_To_Arguments_If_Set(string additionalPropertyName, string additionalPropertyValue, string expected)
            {
                // Given
                var fixture = new ChocolateyNewFixture();
                fixture.Settings.AdditionalPropertyValues.Add(additionalPropertyName, additionalPropertyValue);

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Fact]
            public void Should_Add_Additional_Property_Values_To_Arguments_If_Set()
            {
                // Given
                var fixture = new ChocolateyNewFixture();
                fixture.Settings.AdditionalPropertyValues.Add("Foo1", "Bar1");
                fixture.Settings.AdditionalPropertyValues.Add("Foo2", "Bar2");

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("new \"MyPackage\" -y Foo1=\"Bar1\" Foo2=\"Bar2\"", result.Args);
            }

            [Fact]
            public void Should_Add_Output_Directory_To_Arguments_If_Not_Null()
            {
                // Given
                var fixture = new ChocolateyNewFixture();
                fixture.Settings.OutputDirectory = "./Foo";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("new \"MyPackage\" -y --outputdirectory \"/Working/Foo\"", result.Args);
            }
        }
    }
}