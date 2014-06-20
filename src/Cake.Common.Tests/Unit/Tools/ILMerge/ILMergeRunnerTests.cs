using System;
using System.Diagnostics;
using System.Linq;
using Cake.Common.Tests.Fixtures;
using Cake.Common.Tools.ILMerge;
using Cake.Core;
using Cake.Core.IO;
using NSubstitute;
using Xunit;
using Xunit.Extensions;

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
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("outputAssemblyPath", ((ArgumentNullException) result).ParamName);
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
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("primaryAssemblyPath", ((ArgumentNullException)result).ParamName);
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
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("assemblyPaths", ((ArgumentNullException)result).ParamName);
            }

            [Fact]
            public void Should_Throw_If_ILMerge_Executable_Was_Not_Found()
            {
                // Given
                var fixture = new ILMergeRunnerFixture();
                fixture.Globber.Match("./tools/**/ILMerge.exe").Returns(Enumerable.Empty<Path>());

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("Could not find ILMerge.exe.", result.Message);
            }

            [Theory]
            [InlineData("C:/ILMerge/ILMerge.exe", "C:/ILMerge/ILMerge.exe")]
            [InlineData("./tools/ILMerge/ILMerge.exe", "/Working/tools/ILMerge/ILMerge.exe")]
            public void Should_Use_ILMerge_Executable_From_Tool_Path_If_Provided(string toolPath, string expected)
            {
                // Given
                var fixture = new ILMergeRunnerFixture();
                fixture.Settings.ToolPath = toolPath;               

                // When
                fixture.Run();

                // Then
                fixture.ProcessRunner.Received(1).Start(Arg.Is<ProcessStartInfo>(
                    p => p.FileName == expected));
            }

            [Fact]
            public void Should_Find_ILMerge_Executable_If_Tool_Path_Was_Not_Provided()
            {
                // Given
                var fixture = new ILMergeRunnerFixture();

                // When
                fixture.Run();

                // Then
                fixture.ProcessRunner.Received(1).Start(Arg.Is<ProcessStartInfo>(
                    p => p.FileName == "/Working/tools/ILMerge.exe"));
            }

            [Fact]
            public void Should_Add_Provided_Assemblies_To_Process_Arguments()
            {
                // Given
                var fixture = new ILMergeRunnerFixture();
                fixture.AssemblyPaths.Add("C.dll");
                fixture.AssemblyPaths.Add("D.dll");

                // When
                fixture.Run();

                // Then
                fixture.ProcessRunner.Received(1).Start(Arg.Is<ProcessStartInfo>(
                    p => p.Arguments == "/out:\"/Working/output.exe\" \"/Working/input.exe\" \"/Working/C.dll\" \"/Working/D.dll\""));
            }

            [Fact]
            public void Should_Set_Working_Directory()
            {
                // Given
                var fixture = new ILMergeRunnerFixture();

                // When
                fixture.Run();

                // Then
                fixture.ProcessRunner.Received(1).Start(Arg.Is<ProcessStartInfo>(
                    p => p.WorkingDirectory == "/Working"));
            }

            [Fact]
            public void Should_Throw_If_Process_Was_Not_Started()
            {
                // Given
                var fixture = new ILMergeRunnerFixture();
                fixture.ProcessRunner.Start(Arg.Any<ProcessStartInfo>()).Returns((IProcess)null);

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("ILMerge process was not started.", result.Message);
            }

            [Fact]
            public void Should_Throw_If_Process_Has_A_Non_Zero_Exit_Code()
            {
                // Given
                var fixture = new ILMergeRunnerFixture(); 
                fixture.Process.GetExitCode().Returns(1);

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("Failed to merge assemblies.", result.Message);
            }

            [Fact]
            public void Should_Internalize_If_Enabled_In_Settings()
            {
                // Given
                var fixture = new ILMergeRunnerFixture();
                fixture.Settings.Internalize = true;

                // When
                fixture.Run();

                // Then
                fixture.ProcessRunner.Received(1).Start(Arg.Is<ProcessStartInfo>(
                    p => p.Arguments == "/out:\"/Working/output.exe\" /internalize \"/Working/input.exe\""));
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
                fixture.Run();

                // Then
                fixture.ProcessRunner.Received(1).Start(Arg.Is<ProcessStartInfo>(
                    p => p.Arguments == expected));
            }
        }
    }
}
