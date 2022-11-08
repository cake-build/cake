// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tests.Fixtures.Tools;
using Cake.Common.Tools.ILMerge;
using Cake.Core;
using Cake.Core.IO;
using Cake.Testing;
using Cake.Testing.Xunit;
using Xunit;

namespace Cake.Common.Tests.Unit.Tools.ILMerge
{
    public sealed class ILMergeRunnerTests
    {
        public sealed class TheMergeMethod
        {
            [Fact]
            public void Should_Throw_If_Output_Assembly_Path_Was_Null()
            {
                // Given
                var fixture = new ILMergeRunnerFixture();
                fixture.OutputAssemblyPath = null;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "outputAssemblyPath");
            }

            [Fact]
            public void Should_Throw_If_Primary_Assembly_Path_Was_Null()
            {
                // Given
                var fixture = new ILMergeRunnerFixture();
                fixture.PrimaryAssemblyPath = null;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "primaryAssemblyPath");
            }

            [Fact]
            public void Should_Throw_If_Assembly_Paths_Are_Null()
            {
                // Given
                var fixture = new ILMergeRunnerFixture();
                fixture.AssemblyPaths = null;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "assemblyPaths");
            }

            [Fact]
            public void Should_Throw_If_ILMerge_Executable_Was_Not_Found()
            {
                // Given
                var fixture = new ILMergeRunnerFixture();
                fixture.GivenDefaultToolDoNotExist();

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("ILMerge: Could not locate executable.", result?.Message);
            }

            [Theory]
            [InlineData("/bin/tools/ILMerge/ILMerge.exe", "/bin/tools/ILMerge/ILMerge.exe")]
            [InlineData("./tools/ILMerge/ILMerge.exe", "/Working/tools/ILMerge/ILMerge.exe")]
            public void Should_Use_ILMerge_Executable_From_Tool_Path_If_Provided(string toolPath, string expected)
            {
                // Given
                var fixture = new ILMergeRunnerFixture();
                fixture.Settings.ToolPath = toolPath;
                fixture.GivenSettingsToolPathExist();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Path.FullPath);
            }

            [WindowsTheory]
            [InlineData("C:/ILMerge/ILMerge.exe", "C:/ILMerge/ILMerge.exe")]
            public void Should_Use_ILMerge_Executable_From_Tool_Path_If_Provided_On_Windows(string toolPath, string expected)
            {
                // Given
                var fixture = new ILMergeRunnerFixture();
                fixture.Settings.ToolPath = toolPath;
                fixture.GivenSettingsToolPathExist();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Path.FullPath);
            }

            [Fact]
            public void Should_Find_ILMerge_Executable_If_Tool_Path_Was_Not_Provided()
            {
                // Given
                var fixture = new ILMergeRunnerFixture();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/Working/tools/ILMerge.exe", result.Path.FullPath);
            }

            [Fact]
            public void Should_Add_Provided_Assemblies_To_Process_Arguments()
            {
                // Given
                var fixture = new ILMergeRunnerFixture();
                fixture.AssemblyPaths.Add("C.dll");
                fixture.AssemblyPaths.Add("D.dll");

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/out:\"/Working/output.exe\" " +
                             "\"/Working/input.exe\" \"/Working/C.dll\" " +
                             "\"/Working/D.dll\"", result.Args);
            }

            [Fact]
            public void Should_Set_Working_Directory()
            {
                // Given
                var fixture = new ILMergeRunnerFixture();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/Working", result.Process.WorkingDirectory.FullPath);
            }

            [Fact]
            public void Should_Throw_If_Process_Was_Not_Started()
            {
                // Given
                var fixture = new ILMergeRunnerFixture();
                fixture.GivenProcessCannotStart();

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("ILMerge: Process was not started.", result?.Message);
            }

            [Fact]
            public void Should_Throw_If_Process_Has_A_Non_Zero_Exit_Code()
            {
                // Given
                var fixture = new ILMergeRunnerFixture();
                fixture.GivenProcessExitsWithCode(1);

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("ILMerge: Process returned an error (exit code 1).", result?.Message);
            }

            [Fact]
            public void Should_Add_SearchDirectories_If_Enabled_In_Settings()
            {
                // Given
                var fixture = new ILMergeRunnerFixture();
                fixture.Settings.SearchDirectories = new DirectoryPath[] { "/Working/DirectoryA", "/Working/DirectoryB" };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/lib:\"/Working/DirectoryA\" " +
                             "/lib:\"/Working/DirectoryB\" " +
                             "/out:\"/Working/output.exe\" \"/Working/input.exe\"", result.Args);
            }

            [Fact]
            public void Should_Add_Log_If_Enabled_In_Settings()
            {
                // Given
                var fixture = new ILMergeRunnerFixture();
                fixture.Settings.Log = true;
                fixture.Settings.LogFile = null;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/log " +
                             "/out:\"/Working/output.exe\" \"/Working/input.exe\"", result.Args);
            }

            [Fact]
            public void Should_Add_LogFile_If_Enabled_In_Settings()
            {
                // Given
                var fixture = new ILMergeRunnerFixture();
                fixture.Settings.Log = false;
                fixture.Settings.LogFile = "/Working/output.log";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/log:\"/Working/output.log\" " +
                             "/out:\"/Working/output.exe\" \"/Working/input.exe\"", result.Args);
            }

            [Fact]
            public void Should_Add_KeyFile_If_Enabled_In_Settings()
            {
                // Given
                var fixture = new ILMergeRunnerFixture();
                fixture.Settings.KeyFile = "/Working/input.key";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/keyfile:\"/Working/input.key\" " +
                             "/out:\"/Working/output.exe\" \"/Working/input.exe\"", result.Args);
            }

            [Fact]
            public void Should_Add_KeyFile_And_DelaySign_If_Enabled_In_Settings()
            {
                // Given
                var fixture = new ILMergeRunnerFixture();
                fixture.Settings.KeyFile = "/Working/input.key";
                fixture.Settings.DelaySign = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/keyfile:\"/Working/input.key\" " +
                             "/delaysign " +
                             "/out:\"/Working/output.exe\" \"/Working/input.exe\"", result.Args);
            }

            [Fact]
            public void Should_Add_KeyContainer_If_Enabled_In_Settings()
            {
                // Given
                var fixture = new ILMergeRunnerFixture();
                fixture.Settings.KeyContainer = "myContainer";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/keycontainer:\"myContainer\" " +
                             "/out:\"/Working/output.exe\" \"/Working/input.exe\"", result.Args);
            }

            [Fact]
            public void Should_Add_KeyContainer_And_DelaySign_If_Enabled_In_Settings()
            {
                // Given
                var fixture = new ILMergeRunnerFixture();
                fixture.Settings.KeyContainer = "myContainer";
                fixture.Settings.DelaySign = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/keycontainer:\"myContainer\" " +
                             "/delaysign " +
                             "/out:\"/Working/output.exe\" \"/Working/input.exe\"", result.Args);
            }

            [Fact]
            public void Should_Not_Add_DelaySign_If_KeyFile_Or_KeyContainer_Not_Enabled_In_Settings()
            {
                // Given
                var fixture = new ILMergeRunnerFixture();
                fixture.Settings.DelaySign = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/out:\"/Working/output.exe\" \"/Working/input.exe\"", result.Args);
            }

            [Fact]
            public void Should_Internalize_If_Enabled_In_Settings()
            {
                // Given
                var fixture = new ILMergeRunnerFixture();
                fixture.Settings.Internalize = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/internalize " +
                             "/out:\"/Working/output.exe\" \"/Working/input.exe\"", result.Args);
            }

            [Theory]
            [InlineData(TargetKind.Dll, "/target:\"dll\" /out:\"/Working/output.exe\" \"/Working/input.exe\"")]
            [InlineData(TargetKind.Exe, "/target:\"exe\" /out:\"/Working/output.exe\" \"/Working/input.exe\"")]
            [InlineData(TargetKind.WinExe, "/target:\"winexe\" /out:\"/Working/output.exe\" \"/Working/input.exe\"")]
            [InlineData(TargetKind.Default, "/out:\"/Working/output.exe\" \"/Working/input.exe\"")]
            public void Should_Set_Target_Kind_If_Enabled_In_Settings(TargetKind kind, string expected)
            {
                // Given
                var fixture = new ILMergeRunnerFixture();
                fixture.Settings.TargetKind = kind;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Fact]
            public void Should_Add_Closed_If_Enabled_In_Settings()
            {
                // Given
                var fixture = new ILMergeRunnerFixture();
                fixture.Settings.Closed = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/closed " +
                             "/out:\"/Working/output.exe\" \"/Working/input.exe\"", result.Args);
            }

            [Fact]
            public void Should_Add_NDebug_If_Enabled_In_Settings()
            {
                // Given
                var fixture = new ILMergeRunnerFixture();
                fixture.Settings.NDebug = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/ndebug " +
                             "/out:\"/Working/output.exe\" \"/Working/input.exe\"", result.Args);
            }

            [Fact]
            public void Should_Add_Version_If_Enabled_In_Settings()
            {
                // Given
                var fixture = new ILMergeRunnerFixture();
                fixture.Settings.Version = "1.2.3.4";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/ver:1.2.3.4 " +
                             "/out:\"/Working/output.exe\" \"/Working/input.exe\"", result.Args);
            }

            [Fact]
            public void Should_Add_CopyAttributes_If_Enabled_In_Settings()
            {
                // Given
                var fixture = new ILMergeRunnerFixture();
                fixture.Settings.CopyAttributes = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/copyattrs " +
                             "/out:\"/Working/output.exe\" \"/Working/input.exe\"", result.Args);
            }

            [Fact]
            public void Should_Add_CopyAttributes_And_AllowMultiple_If_Enabled_In_Settings()
            {
                // Given
                var fixture = new ILMergeRunnerFixture();
                fixture.Settings.CopyAttributes = true;
                fixture.Settings.AllowMultiple = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/copyattrs /allowMultiple " +
                             "/out:\"/Working/output.exe\" \"/Working/input.exe\"", result.Args);
            }

            [Fact]
            public void Should_Add_CopyAttributes_And_KeepFirst_If_Enabled_In_Settings()
            {
                // Given
                var fixture = new ILMergeRunnerFixture();
                fixture.Settings.CopyAttributes = true;
                fixture.Settings.KeepFirst = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/copyattrs /keepFirst " +
                             "/out:\"/Working/output.exe\" \"/Working/input.exe\"", result.Args);
            }

            [Fact]
            public void Should_Add_CopyAttributes_And_AllowMultiple_And_KeepFirst_If_Enabled_In_Settings()
            {
                // Given
                var fixture = new ILMergeRunnerFixture();
                fixture.Settings.CopyAttributes = true;
                fixture.Settings.AllowMultiple = true;
                fixture.Settings.KeepFirst = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/copyattrs /allowMultiple /keepFirst " +
                             "/out:\"/Working/output.exe\" \"/Working/input.exe\"", result.Args);
            }

            [Fact]
            public void Should_Not_Add_AllowMultiple_And_KeepFirst_If_CopyAttributes_Not_Enabled_In_Settings()
            {
                // Given
                var fixture = new ILMergeRunnerFixture();
                fixture.Settings.AllowMultiple = true;
                fixture.Settings.KeepFirst = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/out:\"/Working/output.exe\" \"/Working/input.exe\"", result.Args);
            }

            [Fact]
            public void Should_Add_XmlDocumentation_If_Enabled_In_Settings()
            {
                // Given
                var fixture = new ILMergeRunnerFixture();
                fixture.Settings.XmlDocumentation = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/xmldocs " +
                             "/out:\"/Working/output.exe\" \"/Working/input.exe\"", result.Args);
            }

            [Fact]
            public void Should_Add_AttributeFile_If_Enabled_In_Settings()
            {
                // Given
                var fixture = new ILMergeRunnerFixture();
                fixture.Settings.AttributeFile = "/Working/input.attributes";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/attr:\"/Working/input.attributes\" " +
                             "/out:\"/Working/output.exe\" \"/Working/input.exe\"", result.Args);
            }

            [Fact]
            public void Should_Set_Target_Platform_If_Enabled_In_Settings()
            {
                // Given
                const string path = @"/NetFramework";
                var directoryPath = new DirectoryPath(path);
                var fixture = new ILMergeRunnerFixture();
                fixture.Settings.TargetPlatform = new TargetPlatform(TargetPlatformVersion.v4, directoryPath);

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/targetPlatform:v4,\"/NetFramework\" " +
                             "/out:\"/Working/output.exe\" " +
                             "\"/Working/input.exe\"", result.Args);
            }

            [Fact]
            public void Should_Set_Target_Platform_Without_NETFramework_Path_If_Enabled_In_Settings()
            {
                // Given
                var fixture = new ILMergeRunnerFixture();
                fixture.Settings.TargetPlatform = new TargetPlatform(TargetPlatformVersion.v4);

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/targetPlatform:v4 " +
                             "/out:\"/Working/output.exe\" \"/Working/input.exe\"", result.Args);
            }

            [Fact]
            public void Should_Not_Set_Target_Platform_If_Not_Enabled_In_Settings()
            {
                // Given
                var fixture = new ILMergeRunnerFixture();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/out:\"/Working/output.exe\" " +
                             "\"/Working/input.exe\"", result.Args);
            }

            [Fact]
            public void Should_Add_UseFullPublicKeyForReferences_If_Enabled_In_Settings()
            {
                // Given
                var fixture = new ILMergeRunnerFixture();
                fixture.Settings.UseFullPublicKeyForReferences = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/useFullPublicKeyForReferences " +
                             "/out:\"/Working/output.exe\" \"/Working/input.exe\"", result.Args);
            }

            [Fact]
            public void Should_Add_WildCards_If_Enabled_In_Settings()
            {
                // Given
                var fixture = new ILMergeRunnerFixture();
                fixture.Settings.Wildcards = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/wildcards " +
                             "/out:\"/Working/output.exe\" \"/Working/input.exe\"", result.Args);
            }

            [Fact]
            public void Should_Add_ZeroPeKind_If_Enabled_In_Settings()
            {
                // Given
                var fixture = new ILMergeRunnerFixture();
                fixture.Settings.ZeroPeKind = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/zeroPeKind " +
                             "/out:\"/Working/output.exe\" \"/Working/input.exe\"", result.Args);
            }

            [Fact]
            public void Should_Add_AllowDuplicates_If_Enabled_In_Settings()
            {
                // Given
                var fixture = new ILMergeRunnerFixture();
                fixture.Settings.AllowDuplicateTypes = true;
                fixture.Settings.DuplicateTypes = null;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/allowDup " +
                             "/out:\"/Working/output.exe\" \"/Working/input.exe\"", result.Args);
            }

            [Fact]
            public void Should_Add_DuplicateType_If_Enabled_In_Settings()
            {
                // Given
                var fixture = new ILMergeRunnerFixture();
                fixture.Settings.AllowDuplicateTypes = false;
                fixture.Settings.DuplicateTypes = new string[] { "typeA", "typeB" };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/allowDup:typeA /allowDup:typeB " +
                             "/out:\"/Working/output.exe\" \"/Working/input.exe\"", result.Args);
            }

            [Fact]
            public void Should_Add_Union_If_Enabled_In_Settings()
            {
                // Given
                var fixture = new ILMergeRunnerFixture();
                fixture.Settings.Union = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/union " +
                             "/out:\"/Working/output.exe\" \"/Working/input.exe\"", result.Args);
            }

            [Fact]
            public void Should_Add_Align_If_Enabled_In_Settings()
            {
                // Given
                var fixture = new ILMergeRunnerFixture();
                fixture.Settings.Align = 13;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/align:13 " +
                             "/out:\"/Working/output.exe\" \"/Working/input.exe\"", result.Args);
            }
        }
    }
}