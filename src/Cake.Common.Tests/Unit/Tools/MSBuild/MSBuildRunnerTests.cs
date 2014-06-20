using System.Diagnostics;
using Cake.Common.Tests.Fixtures;
using Cake.Common.Tools.MSBuild;
using Cake.Core;
using Cake.Core.IO;
using NSubstitute;
using Xunit;
using Xunit.Extensions;

namespace Cake.Common.Tests.Unit.Tools.MSBuild
{
    public sealed class MSBuildRunnerTests
    {
        public sealed class TheRunMethod
        {
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
                var runner = fixture.CreateRunner();

                // When
                runner.Run(new MSBuildSettings("./src/Solution.sln")
                {
                    PlatformTarget = target,
                    ToolVersion = version
                });

                // Then
                fixture.ProcessRunner.Received(1).Start(Arg.Is<ProcessStartInfo>(
                    p => p.FileName == expected));
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
                var runner = fixture.CreateRunner();

                // When
                runner.Run(new MSBuildSettings("./src/Solution.sln")
                {
                    ToolVersion = version,
                    PlatformTarget = target
                });

                // Then
                fixture.ProcessRunner.Received(1).Start(Arg.Is<ProcessStartInfo>(
                    p => p.FileName == expected));
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
                var runner = fixture.CreateRunner();

                // When
                runner.Run(new MSBuildSettings("./src/Solution.sln")
                {
                    ToolVersion = version,
                    PlatformTarget = target
                });

                // Then
                fixture.ProcessRunner.Received(1).Start(Arg.Is<ProcessStartInfo>(
                    p => p.FileName == expected));
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
                var runner = fixture.CreateRunner();

                // When
                runner.Run(new MSBuildSettings("./src/Solution.sln")
                {
                    ToolVersion = version,
                    PlatformTarget = target
                });

                // Then
                fixture.ProcessRunner.Received(1).Start(Arg.Is<ProcessStartInfo>(
                    p => p.FileName == expected));
            }

            [Fact]
            public void Should_Throw_If_MSBuild_Executable_Did_Not_Exist()
            {
                // Given
                var fixture = new MSBuildRunnerFixture(msBuildFileExist: false);
                var runner = fixture.CreateRunner();

                // When
                var result = Record.Exception(() => runner.Run( 
                    new MSBuildSettings("./src/Solution.sln") {
                        PlatformTarget = PlatformTarget.x86,
                        ToolVersion = MSBuildToolVersion.NET20
                    }));

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("Could not find MSBuild at /Windows/Microsoft.NET/Framework/v2.0.50727/MSBuild.exe", result.Message);
            }

            [Fact]
            public void Should_Use_Correct_Default_Target_In_Process_Arguments()
            {
                // Given
                var fixture = new MSBuildRunnerFixture();
                var runner = fixture.CreateRunner();

                // When
                runner.Run(new MSBuildSettings("./src/Solution.sln"));

                // Then
                fixture.ProcessRunner.Received(1).Start(Arg.Is<ProcessStartInfo>(
                    p => p.Arguments == "/target:Build src/Solution.sln"));
            }

            [Fact]
            public void Should_Append_Targets_To_Process_Arguments()
            {
                // Given
                var fixture = new MSBuildRunnerFixture();
                var runner = fixture.CreateRunner();

                var settings = new MSBuildSettings("./src/Solution.sln");
                settings.WithTarget("A");
                settings.WithTarget("B");

                // When
                runner.Run(settings);

                // Then
                fixture.ProcessRunner.Received(1).Start(Arg.Is<ProcessStartInfo>(
                    p => p.Arguments == "/target:A;B src/Solution.sln"));
            }

            [Fact]
            public void Should_Append_Property_To_Process_Arguments()
            {
                // Given
                var fixture = new MSBuildRunnerFixture();
                var runner = fixture.CreateRunner();

                var settings = new MSBuildSettings("./src/Solution.sln");
                settings.WithProperty("A", "B");
                settings.WithProperty("C", "D");

                // When
                runner.Run(settings);

                // Then
                fixture.ProcessRunner.Received(1).Start(Arg.Is<ProcessStartInfo>(
                    p => p.Arguments == "/property:A=B;C=D /target:Build src/Solution.sln"));
            }

            [Fact]
            public void Should_Append_Configuration_As_Property_To_Process_Arguments()
            {
                // Given
                var fixture = new MSBuildRunnerFixture();
                var runner = fixture.CreateRunner();

                var settings = new MSBuildSettings("./src/Solution.sln");
                settings.SetConfiguration("Release");

                // When
                runner.Run(settings);

                // Then
                fixture.ProcessRunner.Received(1).Start(Arg.Is<ProcessStartInfo>(
                    p => p.Arguments == "/property:Configuration=Release /target:Build src/Solution.sln"));
            }

            [Fact]
            public void Should_Set_Working_Directory()
            {
                // Given
                var fixture = new MSBuildRunnerFixture();
                var runner = fixture.CreateRunner();
                var settings = new MSBuildSettings("./src/Solution.sln");

                // When
                runner.Run(settings);

                // Then
                fixture.ProcessRunner.Received(1).Start(Arg.Is<ProcessStartInfo>(
                    p => p.WorkingDirectory == "/Working"));
            }

            [Fact]
            public void Should_Throw_If_Process_Was_Not_Started()
            {
                // Given
                var fixture = new MSBuildRunnerFixture();
                fixture.ProcessRunner.Start(Arg.Any<ProcessStartInfo>()).Returns((IProcess)null);
                var runner = fixture.CreateRunner();

                // When
                var result = Record.Exception(() => runner.Run(new MSBuildSettings("./src/Solution.sln")));

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("MSBuild process was not started.", result.Message);
            }

            [Fact]
            public void Should_Throw_If_Process_Has_A_Non_Zero_Exit_Code()
            {
                // Given
                var fixture = new MSBuildRunnerFixture();
                fixture.Process.GetExitCode().Returns(1);
                var runner = fixture.CreateRunner();

                // When
                var result = Record.Exception(() => runner.Run(new MSBuildSettings("./src/Solution.sln")));

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("Build failed.", result.Message);
            }
        }
    }
}
