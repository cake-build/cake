﻿using Cake.Common.Tests.Fixtures.Tools;
using Cake.Common.Tools.MSBuild;
using Cake.Core.Diagnostics;
using Cake.Testing;
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
                var fixture = new MSBuildRunnerFixture(true);
                fixture.Settings.PlatformTarget = PlatformTarget.x64;
                fixture.Settings.ToolVersion = MSBuildToolVersion.Default;

                fixture.GivenDefaultToolDoNotExist();
                fixture.GivenMSBuildIsNotInstalled();
                fixture.FileSystem.CreateFile("/Windows/Microsoft.NET/Framework64/v4.0.30319/MSBuild.exe");
                fixture.FileSystem.CreateFile("/Windows/Microsoft.NET/Framework64/v2.0.50727/MSBuild.exe");

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/Windows/Microsoft.NET/Framework64/v4.0.30319/MSBuild.exe", result.ToolPath.FullPath);
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
                var fixture = new MSBuildRunnerFixture(is64BitOperativeSystem);
                fixture.Settings.ToolVersion = version;
                fixture.Settings.PlatformTarget = target;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.ToolPath.FullPath);
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
                var fixture = new MSBuildRunnerFixture(is64BitOperativeSystem);
                fixture.Settings.ToolVersion = version;
                fixture.Settings.PlatformTarget = target;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.ToolPath.FullPath);
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
                var fixture = new MSBuildRunnerFixture(is64BitOperativeSystem);
                fixture.Settings.ToolVersion = version;
                fixture.Settings.PlatformTarget = target;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.ToolPath.FullPath);
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
                var fixture = new MSBuildRunnerFixture(is64BitOperativeSystem);
                fixture.Settings.ToolVersion = version;
                fixture.Settings.PlatformTarget = target;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.ToolPath.FullPath);
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
                var fixture = new MSBuildRunnerFixture(is64BitOperativeSystem);
                fixture.Settings.ToolVersion = version;
                fixture.Settings.PlatformTarget = target;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.ToolPath.FullPath);
            }

            [Fact]
            public void Should_Throw_If_MSBuild_Executable_Did_Not_Exist()
            {
                // Given
                var fixture = new MSBuildRunnerFixture(true);
                fixture.Settings.PlatformTarget = PlatformTarget.x86;
                fixture.Settings.ToolVersion = MSBuildToolVersion.NET20;

                fixture.GivenMSBuildIsNotInstalled();
                fixture.GivenDefaultToolDoNotExist();

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsCakeException(result, "MSBuild: Could not locate executable.");
            }

            [Fact]
            public void Should_Use_As_Many_Processors_As_Possible_If_MaxCpuCount_Is_Zero()
            {
                // Given
                var fixture = new MSBuildRunnerFixture(false);
                fixture.Settings.MaxCpuCount = 0;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/m /v:normal /target:Build " +
                             "\"/Working/src/Solution.sln\"", result.Args);
            }

            [Fact]
            public void Should_Use_Specified_Number_Of_Max_Processors()
            {
                // Given
                var fixture = new MSBuildRunnerFixture(false);
                fixture.Settings.MaxCpuCount = 4;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/m:4 /v:normal /target:Build " +
                             "\"/Working/src/Solution.sln\"", result.Args);
            }

            [Fact]
            public void Should_Use_Correct_Default_Target_In_Process_Arguments()
            {
                // Given
                var fixture = new MSBuildRunnerFixture(false);

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/m /v:normal /target:Build " +
                             "\"/Working/src/Solution.sln\"", result.Args);
            }

            [Fact]
            public void Should_Use_Node_Reuse_If_Specified()
            {
                // Given
                var fixture = new MSBuildRunnerFixture(false);
                fixture.Settings.NodeReuse = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/m /v:normal /nr:true /target:Build " +
                             "\"/Working/src/Solution.sln\"", result.Args);
            }

            [Fact]
            public void Should_Append_Targets_To_Process_Arguments()
            {
                // Given
                var fixture = new MSBuildRunnerFixture(false);
                fixture.Settings.WithTarget("A");
                fixture.Settings.WithTarget("B");

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/m /v:normal /target:A;B " +
                             "\"/Working/src/Solution.sln\"", result.Args);
            }

            [Fact]
            public void Should_Append_Property_To_Process_Arguments()
            {
                // Given
                var fixture = new MSBuildRunnerFixture(false);
                fixture.Settings.WithProperty("A", "B");
                fixture.Settings.WithProperty("C", "D");

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/m /v:normal /p:A=B /p:C=D /target:Build " +
                             "\"/Working/src/Solution.sln\"", result.Args);
            }

            [Fact]
            public void Should_Append_Property_With_Multiple_Values_To_Process_Arguments()
            {
                // Given
                var fixture = new MSBuildRunnerFixture(false);
                fixture.Settings.WithProperty("A", "B", "E");
                fixture.Settings.WithProperty("C", "D");

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/m /v:normal /p:A=B /p:A=E /p:C=D /target:Build " +
                             "\"/Working/src/Solution.sln\"", result.Args);
            }

            [Fact]
            public void Should_Append_Configuration_As_Property_To_Process_Arguments()
            {
                // Given
                var fixture = new MSBuildRunnerFixture(false);
                fixture.Settings.SetConfiguration("Release");

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/m /v:normal /p:Configuration=Release /target:Build " +
                             "\"/Working/src/Solution.sln\"", result.Args);
            }

            [Theory]
            [InlineData(PlatformTarget.MSIL, "/m /v:normal /p:Platform=\"Any CPU\" /target:Build \"/Working/src/Solution.sln\"")]
            [InlineData(PlatformTarget.x86, "/m /v:normal /p:Platform=x86 /target:Build \"/Working/src/Solution.sln\"")]
            [InlineData(PlatformTarget.x64, "/m /v:normal /p:Platform=x64 /target:Build \"/Working/src/Solution.sln\"")]
            public void Should_Append_Platform_As_Property_To_Process_Arguments(PlatformTarget platform, string expected)
            {
                // Given
                var fixture = new MSBuildRunnerFixture(false);
                fixture.Settings.SetPlatformTarget(platform);

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Fact]
            public void Should_Omit_Platform_Property_In_Process_Arguments_If_It_Is_Null()
            {
                // Given
                var fixture = new MSBuildRunnerFixture(false);

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/m /v:normal /target:Build " +
                             "\"/Working/src/Solution.sln\"", result.Args);
            }

            [Fact]
            public void Should_Set_Working_Directory()
            {
                // Given
                var fixture = new MSBuildRunnerFixture(false);

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/Working", result.Process.WorkingDirectory.FullPath);
            }

            [Fact]
            public void Should_Throw_If_Process_Was_Not_Started()
            {
                // Given
                var fixture = new MSBuildRunnerFixture(false);
                fixture.GivenProcessCannotStart();

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsCakeException(result, "MSBuild: Process was not started.");
            }

            [Fact]
            public void Should_Throw_If_Process_Has_A_Non_Zero_Exit_Code()
            {
                // Given
                var fixture = new MSBuildRunnerFixture(false);
                fixture.GivenProcessExitsWithCode(1);

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsCakeException(result, "MSBuild: Process returned an error.");
            }

            [Theory]
            [InlineData(Verbosity.Quiet, "/m /v:quiet /target:Build \"/Working/src/Solution.sln\"")]
            [InlineData(Verbosity.Minimal, "/m /v:minimal /target:Build \"/Working/src/Solution.sln\"")]
            [InlineData(Verbosity.Normal, "/m /v:normal /target:Build \"/Working/src/Solution.sln\"")]
            [InlineData(Verbosity.Verbose, "/m /v:detailed /target:Build \"/Working/src/Solution.sln\"")]
            [InlineData(Verbosity.Diagnostic, "/m /v:diagnostic /target:Build \"/Working/src/Solution.sln\"")]
            public void Should_Append_Correct_Verbosity(Verbosity verbosity, string expected)
            {
                // Given
                var fixture = new MSBuildRunnerFixture(false);
                fixture.Settings.Verbosity = verbosity;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Fact]
            public void Should_Throw_If_Verbosity_Is_Unknown()
            {
                // Given
                var fixture = new MSBuildRunnerFixture(false);
                fixture.Settings.Verbosity = (Verbosity)int.MaxValue;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsCakeException(result, "Encountered unknown MSBuild build log verbosity.");
            }
        }
    }
}