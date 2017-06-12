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
            public void Should_Internalize_If_Enabled_In_Settings()
            {
                // Given
                var fixture = new ILMergeRunnerFixture();
                fixture.Settings.Internalize = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/out:\"/Working/output.exe\" " +
                             "/internalize \"/Working/input.exe\"", result.Args);
            }

            [Theory]
            [InlineData(TargetKind.Dll, "/out:\"/Working/output.exe\" /target:\"dll\" \"/Working/input.exe\"")]
            [InlineData(TargetKind.Exe, "/out:\"/Working/output.exe\" /target:\"exe\" \"/Working/input.exe\"")]
            [InlineData(TargetKind.WinExe, "/out:\"/Working/output.exe\" /target:\"winexe\" \"/Working/input.exe\"")]
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
                Assert.Equal("/out:\"/Working/output.exe\" " +
                             "/targetPlatform:v4,\"/NetFramework\" " +
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
                Assert.Equal("/out:\"/Working/output.exe\" " +
                             "/targetPlatform:v4 \"/Working/input.exe\"", result.Args);
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
        }
    }
}