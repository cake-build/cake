// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using Cake.Common.Tests.Fixtures.Tools;
using Cake.Common.Tools.ILMerge;
using Cake.Core;
using Cake.Core.IO;
using Cake.Testing;
using Cake.Testing.Xunit;
using Xunit;

namespace Cake.Common.Tests.Unit.Tools.ILRepack
{
    public sealed class ILRepackRunnerTests
    {
        public sealed class TheMergeMethod
        {
            [Fact]
            public void Should_Throw_If_Output_Assembly_Path_Was_Null()
            {
                // Given
                var fixture = new ILRepackRunnerFixture();
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
                var fixture = new ILRepackRunnerFixture();
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
                var fixture = new ILRepackRunnerFixture();
                fixture.AssemblyPaths = null;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "assemblyPaths");
            }

            [Fact]
            public void Should_Throw_If_ILRepack_Executable_Was_Not_Found()
            {
                // Given
                var fixture = new ILRepackRunnerFixture();
                fixture.GivenDefaultToolDoNotExist();

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("ILRepack: Could not locate executable.", result?.Message);
            }

            [Theory]
            [InlineData("/bin/tools/ILRepack/ILRepack.exe", "/bin/tools/ILRepack/ILRepack.exe")]
            [InlineData("./tools/ILRepack/ILRepack.exe", "/Working/tools/ILRepack/ILRepack.exe")]
            public void Should_Use_ILRepack_Executable_From_Tool_Path_If_Provided(string toolPath, string expected)
            {
                // Given
                var fixture = new ILRepackRunnerFixture();
                fixture.Settings.ToolPath = toolPath;
                fixture.GivenSettingsToolPathExist();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Path.FullPath);
            }

            [WindowsTheory]
            [InlineData("C:/ILRepack/ILRepack.exe", "C:/ILRepack/ILRepack.exe")]
            public void Should_Use_ILRepack_Executable_From_Tool_Path_If_Provided_On_Windows(string toolPath, string expected)
            {
                // Given
                var fixture = new ILRepackRunnerFixture();
                fixture.Settings.ToolPath = toolPath;
                fixture.GivenSettingsToolPathExist();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Path.FullPath);
            }

            [Fact]
            public void Should_Find_ILRepack_Executable_If_Tool_Path_Was_Not_Provided()
            {
                // Given
                var fixture = new ILRepackRunnerFixture();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/Working/tools/ILRepack.exe", result.Path.FullPath);
            }

            [Fact]
            public void Should_Add_Provided_Assemblies_To_Process_Arguments()
            {
                // Given
                var fixture = new ILRepackRunnerFixture();
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
                var fixture = new ILRepackRunnerFixture();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/Working", result.Process.WorkingDirectory.FullPath);
            }

            [Fact]
            public void Should_Throw_If_Process_Was_Not_Started()
            {
                // Given
                var fixture = new ILRepackRunnerFixture();
                fixture.GivenProcessCannotStart();

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("ILRepack: Process was not started.", result?.Message);
            }

            [Fact]
            public void Should_Throw_If_Process_Has_A_Non_Zero_Exit_Code()
            {
                // Given
                var fixture = new ILRepackRunnerFixture();
                fixture.GivenProcessExitsWithCode(1);

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("ILRepack: Process returned an error (exit code 1).", result?.Message);
            }

            [Fact]
            public void Should_Set_KeyFile_If_Enabled_In_Settings()
            {
                // Given
                var fixture = new ILRepackRunnerFixture();
                fixture.Settings.Keyfile = "/key.pfx";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/keyfile:\"/key.pfx\" /out:\"/Working/output.exe\" " +
                             "\"/Working/input.exe\"", result.Args);
            }

            [Fact]
            public void Should_Set_Log_If_Enabled_In_Settings()
            {
                // Given
                var fixture = new ILRepackRunnerFixture();
                fixture.Settings.Log = "/output.log";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/log:\"/output.log\" /out:\"/Working/output.exe\" " +
                             "\"/Working/input.exe\"", result.Args);
            }

            [Fact]
            public void Should_Set_Version_If_Enabled_In_Settings()
            {
                // Given
                var fixture = new ILRepackRunnerFixture();
                fixture.Settings.Version = new Version(1, 1, 0);

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/ver:1.1.0 /out:\"/Working/output.exe\" " +
                             "\"/Working/input.exe\"", result.Args);
            }

            [Fact]
            public void Should_Set_Union_If_Enabled_In_Settings()
            {
                // Given
                var fixture = new ILRepackRunnerFixture();
                fixture.Settings.Union = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/union /out:\"/Working/output.exe\" " +
                             "\"/Working/input.exe\"", result.Args);
            }

            [Fact]
            public void Should_Set_NDebug_If_Enabled_In_Settings()
            {
                // Given
                var fixture = new ILRepackRunnerFixture();
                fixture.Settings.NDebug = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/ndebug /out:\"/Working/output.exe\" " +
                             "\"/Working/input.exe\"", result.Args);
            }

            [Fact]
            public void Should_Set_CopyAttrs_If_Enabled_In_Settings()
            {
                // Given
                var fixture = new ILRepackRunnerFixture();
                fixture.Settings.CopyAttrs = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/copyattrs /out:\"/Working/output.exe\" " +
                             "\"/Working/input.exe\"", result.Args);
            }

            [Fact]
            public void Should_Set_Attr_If_Enabled_In_Settings()
            {
                // Given
                var fixture = new ILRepackRunnerFixture();
                fixture.Settings.Attr = "/CommonAssemblyInfo.cs";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/attr:\"/CommonAssemblyInfo.cs\" /out:\"/Working/output.exe\" " +
                             "\"/Working/input.exe\"", result.Args);
            }

            [Fact]
            public void Should_Set_AllowMultiple_If_Enabled_In_Settings()
            {
                // Given
                var fixture = new ILRepackRunnerFixture();
                fixture.Settings.AllowMultiple = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/allowmultiple /out:\"/Working/output.exe\" " +
                             "\"/Working/input.exe\"", result.Args);
            }

            [Theory]
            [InlineData(TargetKind.Dll, "/target:\"library\" /out:\"/Working/output.exe\" \"/Working/input.exe\"")]
            [InlineData(TargetKind.Exe, "/target:\"exe\" /out:\"/Working/output.exe\" \"/Working/input.exe\"")]
            [InlineData(TargetKind.WinExe, "/target:\"winexe\" /out:\"/Working/output.exe\" \"/Working/input.exe\"")]
            [InlineData(TargetKind.Default, "/out:\"/Working/output.exe\" \"/Working/input.exe\"")]
            public void Should_Set_Target_Kind_If_Enabled_In_Settings(TargetKind kind, string expected)
            {
                // Given
                var fixture = new ILRepackRunnerFixture();
                fixture.Settings.TargetKind = kind;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Fact]
            public void Should_Set_Target_Platform_If_Enabled_In_Settings()
            {
                // Given
                var fixture = new ILRepackRunnerFixture();
                fixture.Settings.TargetPlatform = TargetPlatformVersion.v4;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/targetplatform:v4 /out:\"/Working/output.exe\" " +
                             "\"/Working/input.exe\"", result.Args);
            }

            [Fact]
            public void Should_Not_Set_Target_Platform_If_Not_Enabled_In_Settings()
            {
                // Given
                var fixture = new ILRepackRunnerFixture();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/out:\"/Working/output.exe\" " +
                             "\"/Working/input.exe\"", result.Args);
            }

            [Fact]
            public void Should_Set_XmlDocs_If_Enabled_In_Settings()
            {
                // Given
                var fixture = new ILRepackRunnerFixture();
                fixture.Settings.XmlDocs = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/xmldocs /out:\"/Working/output.exe\" " +
                             "\"/Working/input.exe\"", result.Args);
            }

            [Fact]
            public void Should_Set_Libs_If_Enabled_In_Settings()
            {
                // Given
                var fixture = new ILRepackRunnerFixture();
                fixture.Settings.Libs = new List<FilePath> { "/lib1.dll", "/lib2.dll" };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/lib:\"/lib1.dll\" /lib:\"/lib2.dll\" /out:\"/Working/output.exe\" " +
                             "\"/Working/input.exe\"", result.Args);
            }

            [Fact]
            public void Should_Internalize_If_Enabled_In_Settings()
            {
                // Given
                var fixture = new ILRepackRunnerFixture();
                fixture.Settings.Internalize = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/internalize /out:\"/Working/output.exe\" " +
                             "\"/Working/input.exe\"", result.Args);
            }

            [Fact]
            public void Should_Set_DelaySign_If_Enabled_In_Settings()
            {
                // Given
                var fixture = new ILRepackRunnerFixture();
                fixture.Settings.DelaySign = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/delaysign /out:\"/Working/output.exe\" " +
                             "\"/Working/input.exe\"", result.Args);
            }

            [Fact]
            public void Should_Set_AllowDup_If_Enabled_In_Settings()
            {
                // Given
                var fixture = new ILRepackRunnerFixture();
                fixture.Settings.AllowDup = "TypeA";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/allowdup:TypeA /out:\"/Working/output.exe\" " +
                             "\"/Working/input.exe\"", result.Args);
            }

            [Fact]
            public void Should_Set_AllowDuplicateResources_If_Enabled_In_Settings()
            {
                // Given
                var fixture = new ILRepackRunnerFixture();
                fixture.Settings.AllowDuplicateResources = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/allowduplicateresources /out:\"/Working/output.exe\" " +
                             "\"/Working/input.exe\"", result.Args);
            }

            [Fact]
            public void Should_Set_ZeroPeKind_If_Enabled_In_Settings()
            {
                // Given
                var fixture = new ILRepackRunnerFixture();
                fixture.Settings.ZeroPeKind = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/zeropekind /out:\"/Working/output.exe\" " +
                             "\"/Working/input.exe\"", result.Args);
            }

            [Fact]
            public void Should_Set_Wildcards_If_Enabled_In_Settings()
            {
                // Given
                var fixture = new ILRepackRunnerFixture();
                fixture.Settings.Wildcards = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/wildcards /out:\"/Working/output.exe\" " +
                             "\"/Working/input.exe\"", result.Args);
            }

            [Fact]
            public void Should_Set_Parallel_If_Enabled_In_Settings()
            {
                // Given
                var fixture = new ILRepackRunnerFixture();
                fixture.Settings.Parallel = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/parallel /out:\"/Working/output.exe\" " +
                             "\"/Working/input.exe\"", result.Args);
            }

            [Fact]
            public void Should_Set_Pause_If_Enabled_In_Settings()
            {
                // Given
                var fixture = new ILRepackRunnerFixture();
                fixture.Settings.Pause = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/pause /out:\"/Working/output.exe\" " +
                             "\"/Working/input.exe\"", result.Args);
            }

            [Fact]
            public void Should_Set_Verbose_If_Enabled_In_Settings()
            {
                // Given
                var fixture = new ILRepackRunnerFixture();
                fixture.Settings.Verbose = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/verbose /out:\"/Working/output.exe\" " +
                             "\"/Working/input.exe\"", result.Args);
            }
        }
    }
}