using System;
using System.Collections.Generic;
using Cake.Common.Tests.Fixtures.Tools;
using Cake.Common.Tools.InspectCode;
using Cake.Core;
using Cake.Core.IO;
using NSubstitute;
using Xunit;

namespace Cake.Common.Tests.Unit.Tools.InspectCode
{
    public sealed class InspectCodeRunnerTests
    {
        public sealed class TheRunMethod
        {
            [Fact]
            public void Should_Throw_If_Solution_Is_Null()
            {
                // Given
                var fixture = new InspectCodeRunnerFixture();
                var runner = fixture.CreateRunner();

                // When
                var result = Record.Exception(() => runner.Run(null, new InspectCodeSettings()));

                // Then
                Assert.IsArgumentNullException(result, "solution");
            }

            [Fact]
            public void Should_Find_Inspect_Code_Runner()
            {
                // Given
                var fixture = new InspectCodeRunnerFixture();
                var runner = fixture.CreateRunner();

                // When
                runner.Run("./Test.sln", new InspectCodeSettings());

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Is<FilePath>(p => p.FullPath == "/Working/tools/inspectcode.exe"),
                    Arg.Any<ProcessSettings>());
            }

            [Fact]
            public void Should_Use_Provided_Solution_In_Process_Arguments()
            {
                // Given
                var fixture = new InspectCodeRunnerFixture();
                var runner = fixture.CreateRunner();

                // When
                runner.Run("./Test.sln", new InspectCodeSettings());

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(),
                    Arg.Any<ProcessSettings>());
                Assert.Equal("\"/Working/Test.sln\"", fixture.ProcessArguments);
            }

            [Fact]
            public void Should_Throw_If_Process_Was_Not_Started()
            {
                // Given
                var fixture = new InspectCodeRunnerFixture();
                fixture.ProcessRunner.Start(Arg.Any<FilePath>(), Arg.Any<ProcessSettings>()).Returns((IProcess)null);
                var runner = fixture.CreateRunner();

                // When
                var result = Record.Exception(() => runner.Run("./Test.sln", new InspectCodeSettings()));

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("InspectCode: Process was not started.", result.Message);
            }

            [Fact]
            public void Should_Throw_If_Process_Has_A_Non_Zero_Exit_Code()
            {
                // Given
                var fixture = new InspectCodeRunnerFixture();
                fixture.Process.GetExitCode().Returns(1);
                var runner = fixture.CreateRunner();

                // When
                var result = Record.Exception(() => runner.Run("./Test.sln", new InspectCodeSettings()));

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("InspectCode: Process returned an error.", result.Message);
            }

            [Fact]
            public void Should_Set_Output()
            {
                // Given
                var fixture = new InspectCodeRunnerFixture();
                var runner = fixture.CreateRunner();

                // When
                runner.Run("./Test.sln", new InspectCodeSettings
                    {
                        OutputFile = FilePath.FromString("build/inspect_code.xml")
                    });

                // Then
                fixture.ProcessRunner.Received(1).Start(
                   Arg.Any<FilePath>(),
                   Arg.Any<ProcessSettings>()
                );
                Assert.Equal("\"/output:/Working/build/inspect_code.xml\" \"/Working/Test.sln\"", fixture.ProcessArguments);
            }

            [Fact]
            public void Should_Throw_If_Solution_Wide_Analysis_Is_Both_Disabled_And_Enabled()
            {
                // Given
                var fixture = new InspectCodeRunnerFixture();
                var runner = fixture.CreateRunner();

                //When
                var result = Record.Exception(() => runner.Run("./Test.sln", new InspectCodeSettings
                    {
                        SolutionWideAnalysis = true,
                        NoSolutionWideAnalysis = true
                    }));

                // Then
                Assert.IsType<ArgumentException>(result);
                Assert.Equal("InspectCode: You can't set both SolutionWideAnalysis and NoSolutionWideAnalysis to true", result.Message);
            }

            [Fact]
            public void Should_Set_Solution_Wide_Analysis_Switch()
            {
                // Given
                var fixture = new InspectCodeRunnerFixture();
                var runner = fixture.CreateRunner();

                // When
                runner.Run("./Test.sln", new InspectCodeSettings
                       {
                           SolutionWideAnalysis = true
                       });

                // Then
                fixture.ProcessRunner.Received(1).Start(
                   Arg.Any<FilePath>(),
                   Arg.Any<ProcessSettings>()
                );
                Assert.Equal("/swea \"/Working/Test.sln\"", fixture.ProcessArguments);
            }

            [Fact]
            public void Should_Set_No_Solution_Wide_Analysis_Switch()
            {
                // Given
                var fixture = new InspectCodeRunnerFixture();
                var runner = fixture.CreateRunner();

                // When
                runner.Run("./Test.sln", new InspectCodeSettings
                       {
                           NoSolutionWideAnalysis = true
                       });

                // Then
                fixture.ProcessRunner.Received(1).Start(
                   Arg.Any<FilePath>(),
                   Arg.Any<ProcessSettings>()
                );
                Assert.Equal("/no-swea \"/Working/Test.sln\"", fixture.ProcessArguments);
            }

            [Fact]
            public void Should_Set_Project_Filter()
            {
                // Given
                var fixture = new InspectCodeRunnerFixture();
                var runner = fixture.CreateRunner();

                // When
                runner.Run("./Test.sln", new InspectCodeSettings
                {
                    ProjectFilter = "Test.*"
                });

                // Then
                fixture.ProcessRunner.Received(1).Start(
                   Arg.Any<FilePath>(),
                   Arg.Any<ProcessSettings>()
                );
                Assert.Equal("\"/project=Test.*\" \"/Working/Test.sln\"", fixture.ProcessArguments);
            }

            [Fact]
            public void Should_Set_MsBuild_Properties()
            {
                // Given
                var fixture = new InspectCodeRunnerFixture();
                var runner = fixture.CreateRunner();

                // When
                runner.Run("./Test.sln", new InspectCodeSettings
                {
                    MsBuildProperties = new Dictionary<string, string>
                    {
                        {"TreatWarningsAsErrors", "true"},
                        {"Optimize", "false"}
                    }
                });

                // Then
                fixture.ProcessRunner.Received(1).Start(
                  Arg.Any<FilePath>(),
                  Arg.Any<ProcessSettings>()
               );
                Assert.Equal("\"/properties:TreatWarningsAsErrors=true\" \"/properties:Optimize=false\" \"/Working/Test.sln\"", fixture.ProcessArguments);
            }

            [Fact]
            public void Should_Set_Caches_Home()
            {
                // Given
                var fixture = new InspectCodeRunnerFixture();
                var runner = fixture.CreateRunner();

                // When
                runner.Run("./Test.sln", new InspectCodeSettings
                {
                    CachesHome = DirectoryPath.FromString("caches/")
                });

                // Then
                fixture.ProcessRunner.Received(1).Start(
                   Arg.Any<FilePath>(),
                   Arg.Any<ProcessSettings>()
                );
                Assert.Equal("\"/caches-home=/Working/caches\" \"/Working/Test.sln\"", fixture.ProcessArguments);
            }

            [Fact]
            public void Should_Set_Resharper_Plugins()
            {
                // Given
                var fixture = new InspectCodeRunnerFixture();
                var runner = fixture.CreateRunner();

                // When
                runner.Run("./Test.sln", new InspectCodeSettings
                {
                    Extensions = new[] { "ReSharper.AgentSmith", "X.Y" }
                });

                // Then
                fixture.ProcessRunner.Received(1).Start(
                   Arg.Any<FilePath>(),
                   Arg.Any<ProcessSettings>()
                );
                Assert.Equal("\"/extensions=ReSharper.AgentSmith;X.Y\" \"/Working/Test.sln\"", fixture.ProcessArguments);
            }

            [Fact]
            public void Should_Set_Debug_Switch()
            {
                // Given
                var fixture = new InspectCodeRunnerFixture();
                var runner = fixture.CreateRunner();

                // When
                runner.Run("./Test.sln", new InspectCodeSettings
                {
                    Debug = true
                });

                // Then
                fixture.ProcessRunner.Received(1).Start(
                   Arg.Any<FilePath>(),
                   Arg.Any<ProcessSettings>()
                );
                Assert.Equal("/debug \"/Working/Test.sln\"", fixture.ProcessArguments);
            }

            [Fact]
            public void Should_Set_No_Buildin_Settings_Switch()
            {
                // Given
                var fixture = new InspectCodeRunnerFixture();
                var runner = fixture.CreateRunner();

                // When
                runner.Run("./Test.sln", new InspectCodeSettings
                {
                    NoBuildinSettings = true
                });

                // Then
                fixture.ProcessRunner.Received(1).Start(
                   Arg.Any<FilePath>(),
                   Arg.Any<ProcessSettings>()
                );
                Assert.Equal("/no-buildin-settings \"/Working/Test.sln\"", fixture.ProcessArguments);
            }

            [Fact]
            public void Should_Set_Disabled_Settings_Layers()
            {
                // Given
                var fixture = new InspectCodeRunnerFixture();
                var runner = fixture.CreateRunner();

                // When
                runner.Run("./Test.sln", new InspectCodeSettings
                {
                    DisabledSettingsLayers = new[]
                    {
                        SettingsLayer.GlobalAll,
                        SettingsLayer.GlobalPerProduct,
                        SettingsLayer.SolutionShared,
                        SettingsLayer.SolutionPersonal,
                        SettingsLayer.ProjectShared,
                        SettingsLayer.ProjectPersonal,
                    }
                });

                // Then
                fixture.ProcessRunner.Received(1).Start(
                   Arg.Any<FilePath>(),
                   Arg.Any<ProcessSettings>()
                );
                Assert.Equal("\"/dsl=GlobalAll;GlobalPerProduct;SolutionShared;SolutionPersonal;ProjectShared;ProjectPersonal\" \"/Working/Test.sln\"", fixture.ProcessArguments);
            }

            [Fact]
            public void Should_Set_Profile()
            {
                // Given
                var fixture = new InspectCodeRunnerFixture();
                var runner = fixture.CreateRunner();

                // When
                runner.Run("./Test.sln", new InspectCodeSettings
                {
                    Profile = FilePath.FromString("profile.DotSettings")
                });

                // Then
                fixture.ProcessRunner.Received(1).Start(
                   Arg.Any<FilePath>(),
                   Arg.Any<ProcessSettings>()
                );
                Assert.Equal("\"/profile=/Working/profile.DotSettings\" \"/Working/Test.sln\"", fixture.ProcessArguments);
            }
        }

        public sealed class TheRunFromConfigMethod
        {
            [Fact]
            public void Should_Throw_If_Config_File_Is_Null()
            {
                // Given
                var fixture = new InspectCodeRunnerFixture();
                var runner = fixture.CreateRunner();

                // When
                var result = Record.Exception(() => runner.RunFromConfig(null));

                // Then
                Assert.IsArgumentNullException(result, "configFile");
            }

            [Fact]
            public void Should_Use_Provided_Config_File()
            {
                // Given
                var fixture = new InspectCodeRunnerFixture();
                var runner = fixture.CreateRunner();

                // Then
                runner.RunFromConfig(FilePath.FromString("config.xml"));

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(),
                    Arg.Any<ProcessSettings>());
                Assert.Equal("\"/config=/Working/config.xml\"", fixture.ProcessArguments);
            }
        }
    }
}