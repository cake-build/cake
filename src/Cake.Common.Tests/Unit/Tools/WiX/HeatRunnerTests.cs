// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using Cake.Common.Tests.Fixtures.Tools.WiX;
using Cake.Common.Tools.WiX.Heat;
using Cake.Core;
using Cake.Testing;
using Cake.Testing.Xunit;
using Xunit;

namespace Cake.Common.Tests.Unit.Tools.WiX
{
    public sealed class HeatRunnerTests
    {
        public sealed class TheConstructor
        {
            [Fact]
            public void Should_Throw_If_Environment_Is_Null()
            {
                // Given
                var fixture = new HeatFixture();
                fixture.Environment = null;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "environment");
            }
        }

        public sealed class TheRunMethod
        {
            [Fact]
            public void Should_Throw_If_Directory_Path_Empty()
            {
                // Given
                var fixture = new HeatFixture();
                fixture.DirectoryPath = null;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("directoryPath", ((ArgumentException)result)?.ParamName);
            }

            [Fact]
            public void Should_Throw_If_Output_File_Empty()
            {
                // Given
                var fixture = new HeatFixture();
                fixture.OutputFile = null;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("outputFile", ((ArgumentException)result)?.ParamName);
            }

            [Fact]
            public void Should_Set_Working_Directory()
            {
                // Given
                var fixture = new HeatFixture();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/Working", result.Process.WorkingDirectory.FullPath);
            }

            // Not a Valid Test case based on the fixture setup
            public void Should_Throw_If_Settings_Is_Null()
            {
                // Given
                var fixture = new HeatFixture();
                fixture.Settings = null;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "settings");
            }

            [Fact]
            public void Should_Throw_If_Heat_Runner_Was_Not_Found()
            {
                // Given
                var fixture = new HeatFixture();
                fixture.GivenDefaultToolDoNotExist();

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("Heat: Could not locate executable.", result?.Message);
            }

            [Theory]
            [InlineData("/bin/tools/WiX/heat.exe", "/bin/tools/WiX/heat.exe")]
            [InlineData("./tools/WiX/heat.exe", "/Working/tools/WiX/heat.exe")]
            public void Should_Use_Heat_Runner_From_Tool_Path_If_Provided(string toolPath, string excepted)
            {
                // Given
                var fixture = new HeatFixture();
                fixture.Settings.ToolPath = toolPath;
                fixture.GivenSettingsToolPathExist();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(excepted, result.Path.FullPath);
            }

            [WindowsTheory]
            [InlineData("C:/WiX/heat.exe", "C:/WiX/heat.exe")]
            public void Should_Use_Heat_Runner_From_Tool_Path_If_Provided_On_Windows(string toolPath, string expected)
            {
                // Given
                var fixture = new HeatFixture();
                fixture.Settings.ToolPath = toolPath;
                fixture.GivenSettingsToolPathExist();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Path.FullPath);
            }

            [Fact]
            public void Should_Find_Heat_Runner_If_Tool_Path_Not_Provided()
            {
                // Given
                var fixture = new HeatFixture();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/Working/tools/heat.exe", result.Path.FullPath);
            }

            [Fact]
            public void Should_Throw_If_Process_Was_Not_Started()
            {
                // Given
                var fixture = new HeatFixture();
                fixture.GivenProcessCannotStart();

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("Heat: Process was not started.", result?.Message);
            }

            [Fact]
            public void Should_Throw_If_Process_Has_A_Non_Zero_Exit_Code()
            {
                // Given
                var fixture = new HeatFixture();
                fixture.GivenProcessExitsWithCode(1);

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("Heat: Process returned an error (exit code 1).", result?.Message);
            }

            [Fact]
            public void Should_Add_File_Harvest_Type_If_Provided()
            {
                // Given
                var fixture = new HeatFixture();
                fixture.HarvestType = WiXHarvestType.File;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("file \"/Working/Cake.dll\" -out \"/Working/cake.wxs\"", result.Args);
            }

            [Fact]
            public void Should_Add_Website_Harvest_Type_If_Provided()
            {
                // Given
                var fixture = new HeatFixture();
                fixture.HarvestType = WiXHarvestType.Website;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("website \"Default Web Site\" -out \"/Working/cake.wxs\"", result.Args);
            }

            [Fact]
            public void Should_Add_Performance_Harvest_Type_If_Provided()
            {
                // Given
                var fixture = new HeatFixture();
                fixture.HarvestType = WiXHarvestType.Perf;
                fixture.HarvestTarget = "Cake Category";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("perf \"Cake Category\" -out \"/Working/cake.wxs\"", result.Args);
            }

            [Fact]
            public void Should_Add_Extension_To_Arguments_If_Provided()
            {
                // Given
                var fixture = new HeatFixture();
                fixture.Settings.Extensions = new[] { "WiXSecurityExtensions" };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("dir \"/Working/src/Cake\" -ext WiXSecurityExtensions -out \"/Working/cake.wxs\"", result.Args);
            }

            [Fact]
            public void Should_Add_NoLogo_To_Arguments_If_Provided()
            {
                // Given
                var fixture = new HeatFixture();
                fixture.Settings.NoLogo = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("dir \"/Working/src/Cake\" -nologo -out \"/Working/cake.wxs\"", result.Args);
            }

            [Fact]
            public void Should_Add_Suppress_Specific_Warnings_To_Arguments_If_Provided()
            {
                // Given
                var fixture = new HeatFixture();
                fixture.Settings.SuppressSpecificWarnings = new[] { "0001", "0002" };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("dir \"/Working/src/Cake\" -sw0001 -sw0002 -out \"/Working/cake.wxs\"", result.Args);
            }

            [Fact]
            public void Should_Add_Treat_Specific_Warnings_As_Errors_To_Arguments_If_Provided()
            {
                // Given
                var fixture = new HeatFixture();
                fixture.Settings.TreatSpecificWarningsAsErrors = new[] { "1101", "1102" };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("dir \"/Working/src/Cake\" -wx1101 -wx1102 -out \"/Working/cake.wxs\"", result.Args);
            }

            [Fact]
            public void Should_Add_Auto_Generate_Guid_To_Arguments_If_Provided()
            {
                // Given
                var fixture = new HeatFixture();
                fixture.Settings.AutogeneratedGuid = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("dir \"/Working/src/Cake\" -ag -out \"/Working/cake.wxs\"", result.Args);
            }

            [Fact]
            public void Should_Add_Component_Group_Name_To_Arguments_If_Provided()
            {
                // Given
                var fixture = new HeatFixture();
                fixture.Settings.ComponentGroupName = "CakeComponentGroup";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("dir \"/Working/src/Cake\" -cg CakeComponentGroup -out \"/Working/cake.wxs\"", result.Args);
            }

            [Fact]
            public void Should_Add_Configuration_To_Arguments_If_Provided()
            {
                // Given
                var fixture = new HeatFixture();
                fixture.Settings.Configuration = "Release";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("dir \"/Working/src/Cake\" -configuration Release -out \"/Working/cake.wxs\"", result.Args);
            }

            [Fact]
            public void Should_Add_Directory_Id_To_Arguments_If_Provided()
            {
                // Given
                var fixture = new HeatFixture();
                fixture.Settings.DirectoryId = "CakeDirectoryId";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("dir \"/Working/src/Cake\" -directoryid CakeDirectoryId -out \"/Working/cake.wxs\"", result.Args);
            }

            [Fact]
            public void Should_Add_Directory_Reference_To_Arguments_If_Provided()
            {
                // Given
                var fixture = new HeatFixture();
                fixture.Settings.DirectoryReferenceId = "CakeAppDirectoryReference";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("dir \"/Working/src/Cake\" -dr CakeAppDirectoryReference -out \"/Working/cake.wxs\"", result.Args);
            }

            [Fact]
            public void Should_Add_Generate_To_Arguments_If_Provided()
            {
                // Given
                var fixture = new HeatFixture();
                fixture.Settings.Generate = WiXGenerateType.Container;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("dir \"/Working/src/Cake\" -generate container -out \"/Working/cake.wxs\"", result.Args);
            }

            [Fact]
            public void Should_Not_Add_Generate_To_Arguments_If_Default_Provided()
            {
                // Given
                var fixture = new HeatFixture();
                fixture.Settings.Generate = WiXGenerateType.Components;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("dir \"/Working/src/Cake\" -out \"/Working/cake.wxs\"", result.Args);
            }

            [Fact]
            public void Should_Add_Generate_Guid_To_Arguments_If_Provided()
            {
                // Given
                var fixture = new HeatFixture();
                fixture.Settings.GenerateGuid = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("dir \"/Working/src/Cake\" -gg -out \"/Working/cake.wxs\"", result.Args);
            }

            [Fact]
            public void Should_Add_Generate_Guid_Without_Braces_To_Arguments_If_Provided()
            {
                // Given
                var fixture = new HeatFixture();
                fixture.Settings.GenerateGuidWithoutBraces = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("dir \"/Working/src/Cake\" -g1 -out \"/Working/cake.wxs\"", result.Args);
            }

            [Fact]
            public void Should_Add_Keep_Empty_Directories_To_Arguments_If_Provided()
            {
                // Given
                var fixture = new HeatFixture();
                fixture.Settings.KeepEmptyDirectories = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("dir \"/Working/src/Cake\" -ke -out \"/Working/cake.wxs\"", result.Args);
            }

            [Fact]
            public void Should_Add_Platform_To_Arguments_If_Provided()
            {
                // Given
                var fixture = new HeatFixture();
                fixture.Settings.Platform = "osx";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("dir \"/Working/src/Cake\" -platform osx -out \"/Working/cake.wxs\"", result.Args);
            }

            [Fact]
            public void Should_Add_Binaries_Output_Group_To_Arguments_If_Provided()
            {
                // Given
                var fixture = new HeatFixture();
                fixture.Settings.OutputGroup = WiXOutputGroupType.Binaries;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("dir \"/Working/src/Cake\" -pog Binaries -out \"/Working/cake.wxs\"", result.Args);
            }

            [Fact]
            public void Should_Add_Symbols_Output_Group_To_Arguments_If_Provided()
            {
                // Given
                var fixture = new HeatFixture();
                fixture.Settings.OutputGroup = WiXOutputGroupType.Symbols;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("dir \"/Working/src/Cake\" -pog Symbols -out \"/Working/cake.wxs\"", result.Args);
            }

            [Fact]
            public void Should_Add_Documents_Output_Group_To_Arguments_If_Provided()
            {
                // Given
                var fixture = new HeatFixture();
                fixture.Settings.OutputGroup = WiXOutputGroupType.Documents;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("dir \"/Working/src/Cake\" -pog Documents -out \"/Working/cake.wxs\"", result.Args);
            }

            [Fact]
            public void Should_Add_Satellites_Output_Group_To_Arguments_If_Provided()
            {
                // Given
                var fixture = new HeatFixture();
                fixture.Settings.OutputGroup = WiXOutputGroupType.Satellites;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("dir \"/Working/src/Cake\" -pog Satellites -out \"/Working/cake.wxs\"", result.Args);
            }

            [Fact]
            public void Should_Add_Sources_Output_Group_To_Arguments_If_Provided()
            {
                // Given
                var fixture = new HeatFixture();
                fixture.Settings.OutputGroup = WiXOutputGroupType.Sources;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("dir \"/Working/src/Cake\" -pog Sources -out \"/Working/cake.wxs\"", result.Args);
            }

            [Fact]
            public void Should_Add_Content_Output_Group_To_Arguments_If_Provided()
            {
                // Given
                var fixture = new HeatFixture();
                fixture.Settings.OutputGroup = WiXOutputGroupType.Content;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("dir \"/Working/src/Cake\" -pog Content -out \"/Working/cake.wxs\"", result.Args);
            }

            [Fact]
            public void Should_Add_Project_Name_To_Arguments_If_Provided()
            {
                // Given
                var fixture = new HeatFixture();
                fixture.Settings.ProjectName = "Cake.Project";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("dir \"/Working/src/Cake\" -projectname Cake.Project -out \"/Working/cake.wxs\"", result.Args);
            }

            [Fact]
            public void Should_Add_Suppress_Com_To_Arguments_If_Provided()
            {
                // Given
                var fixture = new HeatFixture();
                fixture.Settings.SuppressCom = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("dir \"/Working/src/Cake\" -scom -out \"/Working/cake.wxs\"", result.Args);
            }

            [Fact]
            public void Should_Add_Suppress_Fragments_To_Arguments_If_Provided()
            {
                // Given
                var fixture = new HeatFixture();
                fixture.Settings.SuppressFragments = true;

                // When
                var results = fixture.Run();

                // Then
                Assert.Equal("dir \"/Working/src/Cake\" -sfrag -out \"/Working/cake.wxs\"", results.Args);
            }

            [Fact]
            public void Should_Add_Suppress_Unique_Ids_To_Arguments_If_Provided()
            {
                // Given
                var fixture = new HeatFixture();
                fixture.Settings.SuppressUniqueIds = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("dir \"/Working/src/Cake\" -suid -out \"/Working/cake.wxs\"", result.Args);
            }

            [Fact]
            public void Should_Add_Suppress_Root_Directory_To_Arguments_If_Provided()
            {
                // Given
                var fixture = new HeatFixture();
                fixture.Settings.SuppressRootDirectory = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("dir \"/Working/src/Cake\" -srd -out \"/Working/cake.wxs\"", result.Args);
            }

            [Fact]
            public void Should_Add_Suppress_Registry_To_Arguments_If_Provided()
            {
                // Given
                var fixture = new HeatFixture();
                fixture.Settings.SuppressRegistry = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("dir \"/Working/src/Cake\" -sreg -out \"/Working/cake.wxs\"", result.Args);
            }

            [Fact]
            public void Should_Add_Suppress_Vb6_Com_To_Arguments_If_Provided()
            {
                // Given
                var fixture = new HeatFixture();
                fixture.Settings.SuppressVb6Com = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("dir \"/Working/src/Cake\" -svb6 -out \"/Working/cake.wxs\"", result.Args);
            }

            [Fact]
            public void Should_Add_Template_Type_Fragment_To_Arguments_If_Provided()
            {
                // Given
                var fixture = new HeatFixture();
                fixture.Settings.Template = WiXTemplateType.Fragment;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("dir \"/Working/src/Cake\" -template fragment -out \"/Working/cake.wxs\"", result.Args);
            }

            [Fact]
            public void Should_Add_Template_Type_Module_To_Arguments_If_Provided()
            {
                // Given
                var fixture = new HeatFixture();
                fixture.Settings.Template = WiXTemplateType.Module;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("dir \"/Working/src/Cake\" -template module -out \"/Working/cake.wxs\"", result.Args);
            }

            [Fact]
            public void Should_Add_Template_Type_Product_To_Arguments_If_Provided()
            {
                // Given
                var fixture = new HeatFixture();
                fixture.Settings.Template = WiXTemplateType.Product;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("dir \"/Working/src/Cake\" -template product -out \"/Working/cake.wxs\"", result.Args);
            }

            [Fact]
            public void Should_Add_Transform_To_Arguments_If_Provided()
            {
                // Given
                var fixture = new HeatFixture();
                fixture.Settings.Transform = "cake.xslt";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("dir \"/Working/src/Cake\" -t \"cake.xslt\" -out \"/Working/cake.wxs\"", result.Args);
            }

            [Fact]
            public void Should_Add_Indent_To_Arguments_If_Provided()
            {
                // Given
                var fixture = new HeatFixture();
                fixture.Settings.Indent = 5;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("dir \"/Working/src/Cake\" -indent 5 -out \"/Working/cake.wxs\"", result.Args);
            }

            [Fact]
            public void Should_Add_Verbose_To_Arguments_If_Provided()
            {
                // Given
                var fixture = new HeatFixture();
                fixture.Settings.Verbose = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("dir \"/Working/src/Cake\" -v -out \"/Working/cake.wxs\"", result.Args);
            }

            [Fact]
            public void Should_Add_Generate_Binder_Variables_To_Arguments_If_Provided()
            {
                // Given
                var fixture = new HeatFixture();
                fixture.Settings.GenerateBinderVariables = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("dir \"/Working/src/Cake\" -wixvar -out \"/Working/cake.wxs\"", result.Args);
            }

            [Fact]
            public void Should_Default_To_Directory_Harvest_Type()
            {
                // Given
                var fixture = new HeatFixture();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("dir \"/Working/src/Cake\" -out \"/Working/cake.wxs\"", result.Args);
            }
        }
    }
}