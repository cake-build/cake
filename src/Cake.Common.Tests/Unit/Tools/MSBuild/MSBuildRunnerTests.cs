using Cake.Common.Tests.Fixtures;
using Cake.Common.Tests.Fixtures.Tools;
using Cake.Common.Tools.MSBuild;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using NSubstitute;
using Xunit;

namespace Cake.Common.Tests.Unit.Tools.MSBuild
{
    public sealed class MSBuildRunnerTests
    {
        public sealed class TheRunMethod
        {
            [Fact]
            public void Should_Return_The_Highest_MSBuild_Version_If_Tool_Version_Is_Set_To_Default()
            {
                // Given
                var existingToolPaths = new DirectoryPath[] {
                    "/Windows/Microsoft.NET/Framework64/v4.0.30319",
                    "/Windows/Microsoft.NET/Framework64/v2.0.50727"
                };

                var fixture = new MSBuildRunnerFixture(existingToolPaths);
                fixture.Settings.PlatformTarget = PlatformTarget.x64;
                fixture.Settings.ToolVersion = MSBuildToolVersion.Default;

                // When
                fixture.Run();

                // Then
                fixture.AssertReceivedFilePath("/Windows/Microsoft.NET/Framework64/v4.0.30319/MSBuild.exe");
            }

            [Theory]
            [InlineData(MSBuildToolVersion.VS2005, PlatformTarget.MSIL, true, "/Windows/Microsoft.NET/Framework64/v2.0.50727/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2005, PlatformTarget.MSIL, false, "/Windows/Microsoft.NET/Framework/v2.0.50727/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET20, PlatformTarget.MSIL, true, "/Windows/Microsoft.NET/Framework64/v2.0.50727/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET20, PlatformTarget.MSIL, false, "/Windows/Microsoft.NET/Framework/v2.0.50727/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET30, PlatformTarget.MSIL, true, "/Windows/Microsoft.NET/Framework64/v2.0.50727/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET30, PlatformTarget.MSIL, false, "/Windows/Microsoft.NET/Framework/v2.0.50727/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2005, PlatformTarget.x86, true, "/Windows/Microsoft.NET/Framework/v2.0.50727/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2005, PlatformTarget.x86, false, "/Windows/Microsoft.NET/Framework/v2.0.50727/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET20, PlatformTarget.x86, true, "/Windows/Microsoft.NET/Framework/v2.0.50727/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET20, PlatformTarget.x86, false, "/Windows/Microsoft.NET/Framework/v2.0.50727/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET30, PlatformTarget.x86, true, "/Windows/Microsoft.NET/Framework/v2.0.50727/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET30, PlatformTarget.x86, false, "/Windows/Microsoft.NET/Framework/v2.0.50727/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2005, PlatformTarget.x64, true, "/Windows/Microsoft.NET/Framework64/v2.0.50727/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2005, PlatformTarget.x64, false, "/Windows/Microsoft.NET/Framework64/v2.0.50727/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET20, PlatformTarget.x64, true, "/Windows/Microsoft.NET/Framework64/v2.0.50727/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET20, PlatformTarget.x64, false, "/Windows/Microsoft.NET/Framework64/v2.0.50727/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET30, PlatformTarget.x64, true, "/Windows/Microsoft.NET/Framework64/v2.0.50727/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET30, PlatformTarget.x64, false, "/Windows/Microsoft.NET/Framework64/v2.0.50727/MSBuild.exe")]
            public void Should_Get_Correct_Path_To_MSBuild_Version_2(MSBuildToolVersion version, PlatformTarget target, bool is64BitOperativeSystem, string expected)
            {
                // Given
                var fixture = new MSBuildRunnerFixture(is64BitOperativeSystem, true);
                fixture.Settings.ToolVersion = version;
                fixture.Settings.PlatformTarget = target;

                // When
                fixture.Run();

                // Then
                fixture.AssertReceivedFilePath(expected);
            }

            [Theory]
            [InlineData(MSBuildToolVersion.VS2008, PlatformTarget.MSIL, true, "/Windows/Microsoft.NET/Framework64/v3.5/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2008, PlatformTarget.MSIL, false, "/Windows/Microsoft.NET/Framework/v3.5/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET35, PlatformTarget.MSIL, true, "/Windows/Microsoft.NET/Framework64/v3.5/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET35, PlatformTarget.MSIL, false, "/Windows/Microsoft.NET/Framework/v3.5/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2008, PlatformTarget.x86, true, "/Windows/Microsoft.NET/Framework/v3.5/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2008, PlatformTarget.x86, false, "/Windows/Microsoft.NET/Framework/v3.5/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET35, PlatformTarget.x86, true, "/Windows/Microsoft.NET/Framework/v3.5/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET35, PlatformTarget.x86, false, "/Windows/Microsoft.NET/Framework/v3.5/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2008, PlatformTarget.x64, true, "/Windows/Microsoft.NET/Framework64/v3.5/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2008, PlatformTarget.x64, false, "/Windows/Microsoft.NET/Framework64/v3.5/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET35, PlatformTarget.x64, true, "/Windows/Microsoft.NET/Framework64/v3.5/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET35, PlatformTarget.x64, false, "/Windows/Microsoft.NET/Framework64/v3.5/MSBuild.exe")]
            public void Should_Get_Correct_Path_To_MSBuild_Version_35(MSBuildToolVersion version, PlatformTarget target, bool is64BitOperativeSystem, string expected)
            {
                // Given
                var fixture = new MSBuildRunnerFixture(is64BitOperativeSystem, true);
                fixture.Settings.ToolVersion = version;
                fixture.Settings.PlatformTarget = target;

                // When
                fixture.Run();

                // Then
                fixture.AssertReceivedFilePath(expected);
            }

            [Theory]
            [InlineData(MSBuildToolVersion.NET40, PlatformTarget.MSIL, true, "/Windows/Microsoft.NET/Framework64/v4.0.30319/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET40, PlatformTarget.MSIL, false, "/Windows/Microsoft.NET/Framework/v4.0.30319/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET45, PlatformTarget.MSIL, true, "/Windows/Microsoft.NET/Framework64/v4.0.30319/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET45, PlatformTarget.MSIL, false, "/Windows/Microsoft.NET/Framework/v4.0.30319/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2010, PlatformTarget.MSIL, true, "/Windows/Microsoft.NET/Framework64/v4.0.30319/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2010, PlatformTarget.MSIL, false, "/Windows/Microsoft.NET/Framework/v4.0.30319/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2011, PlatformTarget.MSIL, true, "/Windows/Microsoft.NET/Framework64/v4.0.30319/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2011, PlatformTarget.MSIL, false, "/Windows/Microsoft.NET/Framework/v4.0.30319/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2012, PlatformTarget.MSIL, true, "/Windows/Microsoft.NET/Framework64/v4.0.30319/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2012, PlatformTarget.MSIL, false, "/Windows/Microsoft.NET/Framework/v4.0.30319/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET40, PlatformTarget.x86, true, "/Windows/Microsoft.NET/Framework/v4.0.30319/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET40, PlatformTarget.x86, false, "/Windows/Microsoft.NET/Framework/v4.0.30319/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET45, PlatformTarget.x86, true, "/Windows/Microsoft.NET/Framework/v4.0.30319/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET45, PlatformTarget.x86, false, "/Windows/Microsoft.NET/Framework/v4.0.30319/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2010, PlatformTarget.x86, true, "/Windows/Microsoft.NET/Framework/v4.0.30319/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2010, PlatformTarget.x86, false, "/Windows/Microsoft.NET/Framework/v4.0.30319/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2011, PlatformTarget.x86, true, "/Windows/Microsoft.NET/Framework/v4.0.30319/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2011, PlatformTarget.x86, false, "/Windows/Microsoft.NET/Framework/v4.0.30319/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2012, PlatformTarget.x86, true, "/Windows/Microsoft.NET/Framework/v4.0.30319/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2012, PlatformTarget.x86, false, "/Windows/Microsoft.NET/Framework/v4.0.30319/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET40, PlatformTarget.x64, true, "/Windows/Microsoft.NET/Framework64/v4.0.30319/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET40, PlatformTarget.x64, false, "/Windows/Microsoft.NET/Framework64/v4.0.30319/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET45, PlatformTarget.x64, true, "/Windows/Microsoft.NET/Framework64/v4.0.30319/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET45, PlatformTarget.x64, false, "/Windows/Microsoft.NET/Framework64/v4.0.30319/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2010, PlatformTarget.x64, true, "/Windows/Microsoft.NET/Framework64/v4.0.30319/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2010, PlatformTarget.x64, false, "/Windows/Microsoft.NET/Framework64/v4.0.30319/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2011, PlatformTarget.x64, true, "/Windows/Microsoft.NET/Framework64/v4.0.30319/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2011, PlatformTarget.x64, false, "/Windows/Microsoft.NET/Framework64/v4.0.30319/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2012, PlatformTarget.x64, true, "/Windows/Microsoft.NET/Framework64/v4.0.30319/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2012, PlatformTarget.x64, false, "/Windows/Microsoft.NET/Framework64/v4.0.30319/MSBuild.exe")]
            public void Should_Get_Correct_Path_To_MSBuild_Version_4(MSBuildToolVersion version, PlatformTarget target, bool is64BitOperativeSystem, string expected)
            {
                // Given
                var fixture = new MSBuildRunnerFixture(is64BitOperativeSystem, true);
                fixture.Settings.ToolVersion = version;
                fixture.Settings.PlatformTarget = target;

                // When
                fixture.Run();

                // Then
                fixture.AssertReceivedFilePath(expected);
            }

            [Theory]
            [InlineData(MSBuildToolVersion.NET451, PlatformTarget.MSIL, false, "/Program86/MSBuild/12.0/Bin/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET451, PlatformTarget.MSIL, true, "/Program86/MSBuild/12.0/Bin/amd64/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET452, PlatformTarget.MSIL, false, "/Program86/MSBuild/12.0/Bin/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET452, PlatformTarget.MSIL, true, "/Program86/MSBuild/12.0/Bin/amd64/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2013, PlatformTarget.MSIL, false, "/Program86/MSBuild/12.0/Bin/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2013, PlatformTarget.MSIL, true, "/Program86/MSBuild/12.0/Bin/amd64/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET451, PlatformTarget.x86, false, "/Program86/MSBuild/12.0/Bin/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET451, PlatformTarget.x86, true, "/Program86/MSBuild/12.0/Bin/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET452, PlatformTarget.x86, false, "/Program86/MSBuild/12.0/Bin/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET452, PlatformTarget.x86, true, "/Program86/MSBuild/12.0/Bin/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2013, PlatformTarget.x86, false, "/Program86/MSBuild/12.0/Bin/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2013, PlatformTarget.x86, true, "/Program86/MSBuild/12.0/Bin/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET451, PlatformTarget.x64, false, "/Program86/MSBuild/12.0/Bin/amd64/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET451, PlatformTarget.x64, true, "/Program86/MSBuild/12.0/Bin/amd64/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET452, PlatformTarget.x64, false, "/Program86/MSBuild/12.0/Bin/amd64/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET452, PlatformTarget.x64, true, "/Program86/MSBuild/12.0/Bin/amd64/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2013, PlatformTarget.x64, false, "/Program86/MSBuild/12.0/Bin/amd64/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2013, PlatformTarget.x64, true, "/Program86/MSBuild/12.0/Bin/amd64/MSBuild.exe")]
            public void Should_Get_Correct_Path_To_MSBuild_Version_12(MSBuildToolVersion version, PlatformTarget target, bool is64BitOperativeSystem, string expected)
            {
                // Given
                var fixture = new MSBuildRunnerFixture(is64BitOperativeSystem, true);
                fixture.Settings.ToolVersion = version;
                fixture.Settings.PlatformTarget = target;

                // When
                fixture.Run();

                // Then
                fixture.AssertReceivedFilePath(expected);
            }

            [Theory]
            [InlineData(MSBuildToolVersion.NET46, PlatformTarget.MSIL, false, "/Program86/MSBuild/14.0/Bin/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET46, PlatformTarget.MSIL, true, "/Program86/MSBuild/14.0/Bin/amd64/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2015, PlatformTarget.MSIL, false, "/Program86/MSBuild/14.0/Bin/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2015, PlatformTarget.MSIL, true, "/Program86/MSBuild/14.0/Bin/amd64/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET46, PlatformTarget.x86, false, "/Program86/MSBuild/14.0/Bin/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET46, PlatformTarget.x86, true, "/Program86/MSBuild/14.0/Bin/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2015, PlatformTarget.x86, false, "/Program86/MSBuild/14.0/Bin/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2015, PlatformTarget.x86, true, "/Program86/MSBuild/14.0/Bin/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET46, PlatformTarget.x64, false, "/Program86/MSBuild/14.0/Bin/amd64/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.NET46, PlatformTarget.x64, true, "/Program86/MSBuild/14.0/Bin/amd64/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2015, PlatformTarget.x64, false, "/Program86/MSBuild/14.0/Bin/amd64/MSBuild.exe")]
            [InlineData(MSBuildToolVersion.VS2015, PlatformTarget.x64, true, "/Program86/MSBuild/14.0/Bin/amd64/MSBuild.exe")]
            public void Should_Get_Correct_Path_To_MSBuild_Version_14(MSBuildToolVersion version, PlatformTarget target, bool is64BitOperativeSystem, string expected)
            {
                // Given
                var fixture = new MSBuildRunnerFixture(is64BitOperativeSystem, true);
                fixture.Settings.ToolVersion = version;
                fixture.Settings.PlatformTarget = target;

                // When
                fixture.Run();

                // Then
                fixture.AssertReceivedFilePath(expected);
            }

            [Fact]
            public void Should_Throw_If_MSBuild_Executable_Did_Not_Exist()
            {
                // Given
                var fixture = new MSBuildRunnerFixture(false, false);
                fixture.Settings.PlatformTarget = PlatformTarget.x86;
                fixture.Settings.ToolVersion = MSBuildToolVersion.NET20;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsCakeException(result, "MSBuild: Could not locate executable.");
            }

            [Fact]
            public void Should_Use_As_Many_Processors_As_Possible_If_MaxCpuCount_Is_Zero()
            {
                // Given
                var fixture = new MSBuildRunnerFixture(false, true);
                fixture.Settings.MaxCpuCount = 0;

                // When
                fixture.Run();

                // Then
                fixture.AssertReceivedArguments(
                    "/m /v:normal /target:Build \"/Working/src/Solution.sln\"");
            }

            [Fact]
            public void Should_Use_Specified_Number_Of_Max_Processors()
            {
                // Given
                var fixture = new MSBuildRunnerFixture(false, true);
                fixture.Settings.MaxCpuCount = 4;

                // When
                fixture.Run();

                // Then
                fixture.AssertReceivedArguments(
                    "/m:4 /v:normal /target:Build \"/Working/src/Solution.sln\"");
            }

            [Fact]
            public void Should_Use_Correct_Default_Target_In_Process_Arguments()
            {
                // Given
                var fixture = new MSBuildRunnerFixture(false, true);

                // When
                fixture.Run();

                // Then
                fixture.AssertReceivedArguments(
                    "/m /v:normal /target:Build \"/Working/src/Solution.sln\"");
            }

            [Fact]
            public void Should_Use_Node_Reuse_If_Specified()
            {
                // Given
                var fixture = new MSBuildRunnerFixture(false, true);
                fixture.Settings.NodeReuse = true;

                // When
                fixture.Run();

                // Then
                fixture.AssertReceivedArguments(
                    "/m /v:normal /nr:true /target:Build \"/Working/src/Solution.sln\"");
            }

            [Fact]
            public void Should_Append_Targets_To_Process_Arguments()
            {
                // Given
                var fixture = new MSBuildRunnerFixture(false, true);
                fixture.Settings.WithTarget("A");
                fixture.Settings.WithTarget("B");

                // When
                fixture.Run();

                // Then
                fixture.AssertReceivedArguments(
                    "/m /v:normal /target:A;B \"/Working/src/Solution.sln\"");
            }

            [Fact]
            public void Should_Append_Property_To_Process_Arguments()
            {
                // Given
                var fixture = new MSBuildRunnerFixture(false, true);
                fixture.Settings.WithProperty("A", "B");
                fixture.Settings.WithProperty("C", "D");

                // When
                fixture.Run();

                // Then
                fixture.AssertReceivedArguments(
                    "/m /v:normal /p:\"A\"=\"B\" /p:\"C\"=\"D\" /target:Build \"/Working/src/Solution.sln\"");
            }

            [Fact]
            public void Should_Append_Property_With_Multiple_Values_To_Process_Arguments()
            {
                // Given
                var fixture = new MSBuildRunnerFixture(false, true);
                fixture.Settings.WithProperty("A", "B", "E");
                fixture.Settings.WithProperty("C", "D");

                // When
                fixture.Run();

                // Then
                fixture.AssertReceivedArguments(
                    "/m /v:normal /p:\"A\"=\"B\" /p:\"A\"=\"E\" /p:\"C\"=\"D\" /target:Build \"/Working/src/Solution.sln\"");
            }

            [Fact]
            public void Should_Append_Configuration_As_Property_To_Process_Arguments()
            {
                // Given
                var fixture = new MSBuildRunnerFixture(false, true);
                fixture.Settings.SetConfiguration("Release");

                // When
                fixture.Run();

                // Then
                fixture.AssertReceivedArguments(
                    "/m /v:normal /p:\"Configuration\"=\"Release\" /target:Build \"/Working/src/Solution.sln\"");
            }

            [Fact]
            public void Should_Set_Working_Directory()
            {
                // Given
                var fixture = new MSBuildRunnerFixture(false, true);

                // When
                fixture.Run();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(),
                    Arg.Is<ProcessSettings>(p =>
                        p.WorkingDirectory.FullPath == "/Working"));
            }

            [Fact]
            public void Should_Throw_If_Process_Was_Not_Started()
            {
                // Given
                var fixture = new MSBuildRunnerFixture(false, true);
                fixture.ProcessRunner
                    .Start(Arg.Any<FilePath>(), Arg.Any<ProcessSettings>())
                    .Returns((IProcess)null);

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsCakeException(result, "MSBuild: Process was not started.");
            }

            [Fact]
            public void Should_Throw_If_Process_Has_A_Non_Zero_Exit_Code()
            {
                // Given
                var fixture = new MSBuildRunnerFixture(false, true);
                fixture.Process.GetExitCode().Returns(1);

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsCakeException(result, "MSBuild: Process returned an error.");
            }

            [Theory]
            [InlineData(Verbosity.Quiet, "quiet")]
            [InlineData(Verbosity.Minimal, "minimal")]
            [InlineData(Verbosity.Normal, "normal")]
            [InlineData(Verbosity.Verbose, "verbose")]
            [InlineData(Verbosity.Diagnostic, "diagnostic")]
            public void Should_Append_Correct_Verbosity(Verbosity verbosity, string expected)
            {
                // Given
                var fixture = new MSBuildRunnerFixture(false, true);
                fixture.Settings.Verbosity = verbosity;

                // When
                fixture.Run();

                // Then
                fixture.AssertReceivedArguments(
                    "/m /v:{0} /target:Build \"/Working/src/Solution.sln\"", expected);
            }

            [Fact]
            public void Should_Throw_If_Verbosity_Is_Unknown()
            {
                // Given
                var fixture = new MSBuildRunnerFixture(false, true);
                fixture.Settings.Verbosity = (Verbosity)int.MaxValue;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsCakeException(result, "Encountered unknown MSBuild build log verbosity.");
            }
        }
    }
}
