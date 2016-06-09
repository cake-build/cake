// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Cake.Common.Tests.Fixtures.Tools;
using Cake.Common.Tools.Roundhouse;
using Cake.Core;
using Cake.Testing;
using Cake.Testing.Xunit;
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
                var fixture = new RoundhouseRunnerFixture();
                fixture.GivenDefaultToolDoNotExist();

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("Roundhouse: Could not locate executable.", result.Message);
            }

            [Theory]
            [InlineData("/bin/tools/rh/rh.exe", "/bin/tools/rh/rh.exe")]
            [InlineData("./tools/rh/rh.exe", "/Working/tools/rh/rh.exe")]
            public void Should_Use_Roundhouse_Executable_From_Tool_Path_If_Provided(string toolPath, string expected)
            {
                // Given
                var fixture = new RoundhouseRunnerFixture();
                fixture.Settings.ToolPath = toolPath;
                fixture.GivenSettingsToolPathExist();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Path.FullPath);
            }

            [WindowsTheory]
            [InlineData("C:/rh/rh.exe", "C:/rh/rh.exe")]
            public void Should_Use_Roundhouse_Executable_From_Tool_Path_If_Provided_On_Windows(string toolPath, string expected)
            {
                // Given
                var fixture = new RoundhouseRunnerFixture();
                fixture.Settings.ToolPath = toolPath;
                fixture.GivenSettingsToolPathExist();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Path.FullPath);
            }

            [Fact]
            public void Should_Find_Roundhouse_Executable_If_Tool_Path_Not_Provided()
            {
                // Given
                var fixture = new RoundhouseRunnerFixture();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/Working/tools/rh.exe", result.Path.FullPath);
            }

            [Fact]
            public void Should_Set_Working_Directory()
            {
                // Given
                var fixture = new RoundhouseRunnerFixture();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/Working", result.Process.WorkingDirectory.FullPath);
            }

            [Fact]
            public void Should_Throw_If_Process_Was_Not_Started()
            {
                // Given
                var fixture = new RoundhouseRunnerFixture();
                fixture.GivenProcessCannotStart();

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("Roundhouse: Process was not started.", result.Message);
            }

            [Fact]
            public void Should_Throw_If_Process_Has_A_Non_Zero_Exit_Code()
            {
                // Given
                var fixture = new RoundhouseRunnerFixture();
                fixture.GivenProcessExitsWithCode(1);

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("Roundhouse: Process returned an error (exit code 1).", result.Message);
            }

            [Fact]
            public void Should_Execute_Process_With_Custom_Folder_Names()
            {
                // Given
                var fixture = new RoundhouseRunnerFixture();
                fixture.Settings.AfterMigrationFolderName = "986b0e4a";
                fixture.Settings.AlterDatabaseFolderName = "33a51d15";
                fixture.Settings.BeforeMigrationFolderName = "829fb71e";
                fixture.Settings.FunctionsFolderName = "6469e1bc";
                fixture.Settings.IndexesFolderName = "87135f26";
                fixture.Settings.PermissionsFolderName = "48dace7b";
                fixture.Settings.RunAfterCreateDatabaseFolderName = "cac8f0e7";
                fixture.Settings.RunAfterOtherAnyTimeScriptsFolderName = "d938b4d8";
                fixture.Settings.RunBeforeUpFolderName = "92dfa577";
                fixture.Settings.RunFirstAfterUpFolderName = "e415f686";
                fixture.Settings.SprocsFolderName = "68117fd5";
                fixture.Settings.ViewsFolderName = "eeb4dc87";
                fixture.Settings.UpFolderName = "3b6998dd";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("\"--amg=986b0e4a\" \"--ad=33a51d15\" \"--bmg=829fb71e\" \"--fu=6469e1bc\" " +
                             "\"--ix=87135f26\" \"--p=48dace7b\" \"--racd=cac8f0e7\" \"--ra=d938b4d8\" \"--rb=92dfa577\" \"--rf=e415f686\" " +
                             "\"--sp=68117fd5\" \"--vw=eeb4dc87\" \"--u=3b6998dd\"", result.Args);
            }

            [Fact]
            public void Should_Execute_Process_With_Flags()
            {
                // Given
                var fixture = new RoundhouseRunnerFixture();
                fixture.Settings.Drop = true;
                fixture.Settings.DryRun = true;
                fixture.Settings.Restore = true;
                fixture.Settings.Silent = true;
                fixture.Settings.WarnOnOneTimeScriptChanges = true;
                fixture.Settings.WithTransaction = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("--drop --dryrun --restore --silent --w --t", result.Args);
            }

            [Fact]
            public void Should_Execute_Process_With_Database_Settings()
            {
                // Given
                var fixture = new RoundhouseRunnerFixture();
                fixture.Settings.CommandTimeout = 12;
                fixture.Settings.CommandTimeoutAdmin = 23;
                fixture.Settings.ConnectionString = "server=foo;db=bar";
                fixture.Settings.ConnectionStringAdmin = "server=fooAd;db=barAd";
                fixture.Settings.DatabaseName = "qux";
                fixture.Settings.RecoveryMode = RecoveryMode.Full;
                fixture.Settings.RestoreFilePath = "/backs/restore";
                fixture.Settings.SchemaName = "RH";
                fixture.Settings.ServerName = "Server Foo";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("\"--ct=12\" \"--cta=23\" \"--cs=server=foo;db=bar\" " +
                             "\"--csa=server=fooAd;db=barAd\" \"--d=qux\" " +
                             "\"--rcm=Full\" \"--rfp=/backs/restore\" " +
                             "\"--sc=RH\" \"--s=Server Foo\"", result.Args);
            }

            [Fact]
            public void Should_Execute_Process_With_Roundhouse_Settings()
            {
                // Given
                var fixture = new RoundhouseRunnerFixture();
                fixture.Settings.CreateDatabaseCustomScript = "cust-create.sql";
                fixture.Settings.DatabaseType = "roundhouse.databases.postgresql";
                fixture.Settings.Environment = "STAGING";
                fixture.Settings.OutputPath = "out_path";
                fixture.Settings.RepositoryPath = "git@github.com:chucknorris/roundhouse.git";
                fixture.Settings.SqlFilesDirectory = "/db/scripts";
                fixture.Settings.VersionFile = "version.xml";
                fixture.Settings.VersionXPath = "/Build/Version";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("\"--cds=cust-create.sql\" " +
                             "\"--dt=roundhouse.databases.postgresql\" " +
                             "\"--env=STAGING\" \"--o=out_path\" " +
                             "\"--r=git@github.com:chucknorris/roundhouse.git\" " +
                             "\"--f=/db/scripts\" \"--vf=version.xml\" " +
                             "\"--vx=/Build/Version\"", result.Args);
            }
        }
    }
}
