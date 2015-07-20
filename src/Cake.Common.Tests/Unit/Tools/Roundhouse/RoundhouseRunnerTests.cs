using Cake.Common.Tests.Fixtures.Tools;
using Cake.Common.Tools.Roundhouse;
using Cake.Core;
using Cake.Core.IO;
using NSubstitute;
using Xunit;

namespace Cake.Common.Tests.Unit.Tools.Roundhouse
{
    public sealed class RoundhouseRunnerTests
    {
        public sealed class TheRunMethod
        {
            [Fact]
            public void Should_Throw_if_Roundhouse_Executable_Was_Not_Found()
            {
                // Given
                var fixture = new RoundhouseRunnerFixture(defaultToolExist: false);
                var runner = fixture.CreateRunner();

                // When
                var result = Record.Exception(() => runner.Run(new RoundhouseSettings()));

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("Roundhouse: Could not locate executable.", result.Message);
            }

            [Theory]
            [InlineData("C:/rh/rh.exe", "C:/rh/rh.exe")]
            [InlineData("./tools/rh/rh.exe", "/Working/tools/rh/rh.exe")]
            public void Should_Use_Roundhouse_Executable_From_Tool_Path_If_Provided(string toolPath, string expected)
            {
                // Given
                var fixture = new RoundhouseRunnerFixture(expected);
                var runner = fixture.CreateRunner();

                // When
                runner.Run(new RoundhouseSettings
                {
                    ToolPath = toolPath,
                });

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Is<FilePath>(p => p.FullPath == expected),
                    Arg.Any<ProcessSettings>());
            }

            [Fact]
            public void Should_Find_Roundhouse_Executable_If_Tool_Path_Not_Provided()
            {
                // Given
                var fixture = new RoundhouseRunnerFixture();
                var runner = fixture.CreateRunner();

                // When
                runner.Run(new RoundhouseSettings());

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Is<FilePath>(p => p.FullPath == "/Working/tools/rh.exe"),
                    Arg.Any<ProcessSettings>());
            }

            [Fact]
            public void Should_Set_Working_Directory()
            {
                // Given
                var fixture = new RoundhouseRunnerFixture();
                var runner = fixture.CreateRunner();

                // When
                runner.Run(new RoundhouseSettings());

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(),
                    Arg.Is<ProcessSettings>(
                        p => p.WorkingDirectory.FullPath == "/Working"));
            }

            [Fact]
            public void Should_Throw_If_Process_Was_Not_Started()
            {
                // Given
                var fixture = new RoundhouseRunnerFixture();
                fixture.ProcessRunner.Start(Arg.Any<FilePath>(), Arg.Any<ProcessSettings>()).Returns((IProcess) null);
                var runner = fixture.CreateRunner();

                // When
                var result = Record.Exception(() => runner.Run(new RoundhouseSettings()));

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("Roundhouse: Process was not started.", result.Message);
            }

            [Fact]
            public void Should_Throw_If_Process_Has_A_Non_Zero_Exit_Code()
            {
                // Given
                var fixture = new RoundhouseRunnerFixture();
                fixture.Process.GetExitCode().Returns(1);
                var runner = fixture.CreateRunner();

                // When
                var result = Record.Exception(() => runner.Run(new RoundhouseSettings()));

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("Roundhouse: Process returned an error.", result.Message);
            }

            [Fact]
            public void Should_Execute_Process_With_Custom_Folder_Names()
            {
                // Given
                var fixture = new RoundhouseRunnerFixture();
                var runner = fixture.CreateRunner();

                // When
                runner.Run(new RoundhouseSettings
                {
                    AfterMigrationFolderName = "986b0e4a",
                    AlterDatabaseFolderName = "33a51d15",
                    BeforeMigrationFolderName = "829fb71e",
                    FunctionsFolderName = "6469e1bc",
                    IndexesFolderName = "87135f26",
                    PermissionsFolderName = "48dace7b",
                    RunAfterCreateDatabaseFolderName = "cac8f0e7",
                    RunAfterOtherAnyTimeScriptsFolderName = "d938b4d8",
                    RunBeforeUpFolderName = "92dfa577",
                    RunFirstAfterUpFolderName = "e415f686",
                    SprocsFolderName = "68117fd5",
                    ViewsFolderName = "eeb4dc87",
                    UpFolderName = "3b6998dd",
                });

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(),
                    Arg.Is<ProcessSettings>(
                        p => p.Arguments.Render() == "\"--amg=986b0e4a\" \"--ad=33a51d15\" \"--bmg=829fb71e\" \"--fu=6469e1bc\" " +
                             "\"--ix=87135f26\" \"--p=48dace7b\" \"--racd=cac8f0e7\" \"--ra=d938b4d8\" \"--rb=92dfa577\" \"--rf=e415f686\" " +
                             "\"--sp=68117fd5\" \"--vw=eeb4dc87\" \"--u=3b6998dd\""));
            }

            [Fact]
            public void Should_Execute_Process_With_Flags()
            {
                // Given
                var fixture = new RoundhouseRunnerFixture();
                var runner = fixture.CreateRunner();

                // When
                runner.Run(new RoundhouseSettings
                {
                    Drop = true,
                    DryRun = true,
                    Restore = true,
                    Silent = true,
                    WarnOnOneTimeScriptChanges = true,
                    WithTransaction = true,
                });

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(),
                    Arg.Is<ProcessSettings>(
                        p => p.Arguments.Render() == "--drop --dryrun --restore --silent --w --t"));
            }

            [Fact]
            public void Should_Execute_Process_With_Database_Settings()
            {
                // Given
                var fixture = new RoundhouseRunnerFixture();
                var runner = fixture.CreateRunner();

                // When
                runner.Run(new RoundhouseSettings
                {
                    CommandTimeout = 12,
                    CommandTimeoutAdmin = 23,
                    ConnectionString = "server=foo;db=bar",
                    ConnectionStringAdmin = "server=fooAd;db=barAd",
                    DatabaseName = "qux",
                    RecoveryMode = RecoveryMode.Full,
                    RestoreFilePath = "/backs/restore",
                    SchemaName = "RH",
                    ServerName = "Server Foo",
                });

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(),
                    Arg.Is<ProcessSettings>(
                        p => p.Arguments.RenderSafe() == "\"--ct=12\" \"--cta=23\" \"[REDACTED]\" \"[REDACTED]\" " +
                             "\"--d=qux\" \"--rcm=Full\" \"--rfp=/backs/restore\" \"--sc=RH\" \"--s=Server Foo\""));
            }

            [Fact]
            public void Should_Execute_Process_With_Roundhouse_Settings()
            {
                // Given
                var fixture = new RoundhouseRunnerFixture();
                var runner = fixture.CreateRunner();

                // When
                runner.Run(new RoundhouseSettings
                {
                    CreateDatabaseCustomScript = "cust-create.sql",
                    DatabaseType = "roundhouse.databases.postgresql",
                    Environment = "STAGING",
                    OutputPath = "out_path",
                    RepositoryPath = "git@github.com:chucknorris/roundhouse.git",
                    SqlFilesDirectory = "/db/scripts",
                    VersionFile = "version.xml",
                    VersionXPath = "/Build/Version",
                });

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(),
                    Arg.Is<ProcessSettings>(
                        p => p.Arguments.Render() == "\"--cds=cust-create.sql\" \"--dt=roundhouse.databases.postgresql\" \"--env=STAGING\" " +
                             "\"--o=out_path\" \"--r=git@github.com:chucknorris/roundhouse.git\" \"--f=/db/scripts\" \"--vf=version.xml\" \"--vx=/Build/Version\""));
            }
        }
    }
}