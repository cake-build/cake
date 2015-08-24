using System.Collections.Generic;
using Cake.Common.Tests.Fixtures.Tools;
using Cake.Common.Tools.DupFinder;
using Cake.Core;
using Cake.Core.IO;
using NSubstitute;
using Xunit;

namespace Cake.Common.Tests.Unit.Tools.DupFinder
{
    public sealed class DupFinderRunnerTests
    {
        public sealed class TheRunMethodWithFiles
        {
            [Fact]
            public void Should_Throw_If_Projects_Are_Null()
            {
                // Given
                var fixture = new DupFinderRunnerFixture();
                var runner = fixture.CreateRunner();

                // When
                var result = Record.Exception(() => runner.Run((IEnumerable<FilePath>)null, new DupFinderSettings()));

                // Then
                Assert.IsArgumentNullException(result, "files");
            }

            [Fact]
            public void Should_Find_Inspect_Code_Runner()
            {
                // Given
                var fixture = new DupFinderRunnerFixture();
                var runner = fixture.CreateRunner();

                // When
                runner.Run(new[] { FilePath.FromString("./Test.sln") }, new DupFinderSettings());

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Is<FilePath>(p => p.FullPath == "/Working/tools/dupfinder.exe"),
                    Arg.Any<ProcessSettings>());
            }

            [Fact]
            public void Should_Throw_If_Process_Was_Not_Started()
            {
                // Given
                var fixture = new DupFinderRunnerFixture();
                fixture.ProcessRunner.Start(Arg.Any<FilePath>(), Arg.Any<ProcessSettings>()).Returns((IProcess)null);
                var runner = fixture.CreateRunner();

                // When
                var result =
                    Record.Exception(
                        () => runner.Run(new[] { FilePath.FromString("./Test.sln") }, new DupFinderSettings()));

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("DupFinder: Process was not started.", result.Message);
            }

            [Fact]
            public void Should_Throw_If_Has_A_Non_Zero_Exit_Code()
            {
                // Given
                var fixture = new DupFinderRunnerFixture();
                fixture.Process.GetExitCode().Returns(1);
                var runner = fixture.CreateRunner();

                // When
                var result =
                    Record.Exception(
                        () => runner.Run(new[] { FilePath.FromString("./Test.sln") }, new DupFinderSettings()));

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("DupFinder: Process returned an error.", result.Message);
            }

            [Fact]
            public void Should_Use_Provided_Files_In_Process_Arguments()
            {
                // Given
                var fixture = new DupFinderRunnerFixture();
                var runner = fixture.CreateRunner();

                // When
                runner.Run(new[] { FilePath.FromString("./Test.sln"), FilePath.FromString("./Test.csproj") }, new DupFinderSettings());

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(),
                    Arg.Any<ProcessSettings>());
                Assert.Equal("\"/Working/Test.sln\" \"/Working/Test.csproj\"", fixture.ProcessArguments);
            }

            [Fact]
            public void Should_Set_Output_File()
            {
                // Given
                var fixture = new DupFinderRunnerFixture();
                var runner = fixture.CreateRunner();

                // Then
                runner.Run(new[] { FilePath.FromString("./Test.sln") }, new DupFinderSettings
                {
                    OutputFile = FilePath.FromString("build/dupfinder.xml")
                });

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(),
                    Arg.Any<ProcessSettings>());
                Assert.Equal("\"/output=/Working/build/dupfinder.xml\" \"/Working/Test.sln\"", fixture.ProcessArguments);
            }

            [Fact]
            public void Should_Set_Debug_Switch()
            {
                // Given
                var fixture = new DupFinderRunnerFixture();
                var runner = fixture.CreateRunner();

                // Then
                runner.Run(new[] { FilePath.FromString("./Test.sln") }, new DupFinderSettings
                {
                    Debug = true
                });

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(),
                    Arg.Any<ProcessSettings>());
                Assert.Equal("/debug \"/Working/Test.sln\"", fixture.ProcessArguments);
            }

            [Fact]
            public void Should_Set_Discard_Cost()
            {
                // Given
                var fixture = new DupFinderRunnerFixture();
                var runner = fixture.CreateRunner();

                // Then
                runner.Run(new[] { FilePath.FromString("./Test.sln") }, new DupFinderSettings
                {
                    DiscardCost = 50,
                });

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(),
                    Arg.Any<ProcessSettings>());
                Assert.Equal("/discard-cost=50 \"/Working/Test.sln\"", fixture.ProcessArguments);
            }

            [Fact]
            public void Should_Set_Discard_Fields_Switch()
            {
                // Given
                var fixture = new DupFinderRunnerFixture();
                var runner = fixture.CreateRunner();

                // Then
                runner.Run(new[] { FilePath.FromString("./Test.sln") }, new DupFinderSettings
                {
                    DiscardFieldsName = true
                });

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(),
                    Arg.Any<ProcessSettings>());
                Assert.Equal("/discard-fields \"/Working/Test.sln\"", fixture.ProcessArguments);
            }

            [Fact]
            public void Should_Set_Discard_Literals_Switch()
            {
                // Given
                var fixture = new DupFinderRunnerFixture();
                var runner = fixture.CreateRunner();

                // Then
                runner.Run(new[] { FilePath.FromString("./Test.sln") }, new DupFinderSettings
                {
                    DiscardLiterals = true
                });

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(),
                    Arg.Any<ProcessSettings>());
                Assert.Equal("/discard-literals \"/Working/Test.sln\"", fixture.ProcessArguments);
            }

            [Fact]
            public void Should_Set_Discard_Local_Vars_Switch()
            {
                // Given
                var fixture = new DupFinderRunnerFixture();
                var runner = fixture.CreateRunner();

                // Then
                runner.Run(new[] { FilePath.FromString("./Test.sln") }, new DupFinderSettings
                {
                    DiscardLocalVariablesName = true
                });

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(),
                    Arg.Any<ProcessSettings>());
                Assert.Equal("/discard-local-vars \"/Working/Test.sln\"", fixture.ProcessArguments);
            }

            [Fact]
            public void Should_Set_Discard_Types_Switch()
            {
                // Given
                var fixture = new DupFinderRunnerFixture();
                var runner = fixture.CreateRunner();

                // Then
                runner.Run(new[] { FilePath.FromString("./Test.sln") }, new DupFinderSettings
                {
                    DiscardTypes = true
                });

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(),
                    Arg.Any<ProcessSettings>());
                Assert.Equal("/discard-types \"/Working/Test.sln\"", fixture.ProcessArguments);
            }

            [Fact]
            public void Should_Set_Idle_Priority_Switch()
            {
                // Given
                var fixture = new DupFinderRunnerFixture();
                var runner = fixture.CreateRunner();

                // Then
                runner.Run(new[] { FilePath.FromString("./Test.sln") }, new DupFinderSettings
                {
                    IdlePriority = true
                });

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(),
                    Arg.Any<ProcessSettings>());
                Assert.Equal("/idle-priority \"/Working/Test.sln\"", fixture.ProcessArguments);
            }

            [Fact]
            public void Should_Set_Exclude_By_Comment()
            {
                // Given
                var fixture = new DupFinderRunnerFixture();
                var runner = fixture.CreateRunner();

                // Then
                runner.Run(new[] { FilePath.FromString("./Test.sln") }, new DupFinderSettings
                {
                    ExcludeFilesByStartingCommentSubstring = new[] { "test", "asdf" }
                });

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(),
                    Arg.Any<ProcessSettings>());
                Assert.Equal("\"/exclude-by-comment=test;asdf\" \"/Working/Test.sln\"", fixture.ProcessArguments);
            }

            [Fact]
            public void Should_Set_Exclude_Code_Regions()
            {
                // Given
                var fixture = new DupFinderRunnerFixture();
                var runner = fixture.CreateRunner();

                // Then
                runner.Run(new[] { FilePath.FromString("./Test.sln") }, new DupFinderSettings
                {
                    ExcludeCodeRegionsByNameSubstring = new[] { "generated code", "test" }
                });

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(),
                    Arg.Any<ProcessSettings>());
                Assert.Equal("\"/exclude-code-regions=generated code;test\" \"/Working/Test.sln\"", fixture.ProcessArguments);
            }

            [Fact]
            public void Should_Set_Exclude_Pattern()
            {
                // Given
                var fixture = new DupFinderRunnerFixture();
                var runner = fixture.CreateRunner();

                // Then
                runner.Run(new[] { FilePath.FromString("./Test.sln") }, new DupFinderSettings
                {
                    ExcludePattern = new[] { "*Tests.cs", "*Test.cs" }
                });

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(),
                    Arg.Any<ProcessSettings>());
                Assert.Equal("\"/exclude=*Tests.cs;*Test.cs\" \"/Working/Test.sln\"", fixture.ProcessArguments);
            }

            [Fact]
            public void Should_Set_MSBuild_Properties()
            {
                // Given
                var fixture = new DupFinderRunnerFixture();
                var runner = fixture.CreateRunner();

                // Then
                runner.Run(new[] { FilePath.FromString("./Test.sln") }, new DupFinderSettings
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
                    Arg.Any<ProcessSettings>());
                Assert.Equal("\"/properties:TreatWarningsAsErrors=true\" \"/properties:Optimize=false\" \"/Working/Test.sln\"", fixture.ProcessArguments);
            }

            [Fact]
            public void Should_Set_Normalize_Types_Switch()
            {
                // Given
                var fixture = new DupFinderRunnerFixture();
                var runner = fixture.CreateRunner();

                // Then
                runner.Run(new[] { FilePath.FromString("./Test.sln") }, new DupFinderSettings
                {
                    NormalizeTypes = true
                });

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(),
                    Arg.Any<ProcessSettings>());
                Assert.Equal("/normalize-types \"/Working/Test.sln\"", fixture.ProcessArguments);
            }

            [Fact]
            public void Should_Set_Caches_Home()
            {
                // Given
                var fixture = new DupFinderRunnerFixture();
                var runner = fixture.CreateRunner();

                // Then
                runner.Run(new[] { FilePath.FromString("./Test.sln") }, new DupFinderSettings
                {
                    CachesHome = DirectoryPath.FromString("caches/")
                });

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(),
                    Arg.Any<ProcessSettings>());
                Assert.Equal("\"/caches-home=/Working/caches\" \"/Working/Test.sln\"", fixture.ProcessArguments);
            }

            [Fact]
            public void Should_Set_Show_Stats_Switch()
            {
                // Given
                var fixture = new DupFinderRunnerFixture();
                var runner = fixture.CreateRunner();

                // Then
                runner.Run(new[] { FilePath.FromString("./Test.sln") }, new DupFinderSettings
                {
                    ShowStats = true
                });

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(),
                    Arg.Any<ProcessSettings>());
                Assert.Equal("/show-stats \"/Working/Test.sln\"", fixture.ProcessArguments);
            }

            [Fact]
            public void Should_Set_Show_Text_Switch()
            {
                // Given
                var fixture = new DupFinderRunnerFixture();
                var runner = fixture.CreateRunner();

                // Then
                runner.Run(new[] { FilePath.FromString("./Test.sln") }, new DupFinderSettings
                {
                    ShowText = true
                });

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(),
                    Arg.Any<ProcessSettings>());
                Assert.Equal("/show-text \"/Working/Test.sln\"", fixture.ProcessArguments);
            }
        }

        public sealed class TheRunFromConfigMethod
        {
            [Fact]
            public void Should_Throw_If_Config_File_Is_Null()
            {
                // Given
                var fixture = new DupFinderRunnerFixture();
                var runner = fixture.CreateRunner();

                // When
                var result = Record.Exception(() => runner.RunFromConfig((FilePath)null));

                // Then
                Assert.IsArgumentNullException(result, "configFile");
            }

            [Fact]
            public void Should_Use_Provided_Config_File()
            {
                // Given
                var fixture = new DupFinderRunnerFixture();
                var runner = fixture.CreateRunner();

                // When
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