// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tests.Fixtures.Tools;
using Cake.Core;
using Cake.Core.IO;
using Cake.Testing;
using Cake.Testing.Xunit;
using Xunit;

namespace Cake.Common.Tests.Unit.Tools.GitLink
{
    public sealed class GitlinkRunnerTests
    {
        public sealed class TheRunMethod
        {
            [Fact]
            public void Should_Throw_If_Repository_Root_Path_Is_Null()
            {
                // Given
                var fixture = new GitLinkFixture();
                fixture.RepositoryRootPath = null;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "repositoryRootPath");
            }

            [Fact]
            public void Should_Find_GitLink_Runner()
            {
                // Given
                var fixture = new GitLinkFixture();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/Working/tools/gitlink.exe", result.Path.FullPath);
            }

            [Fact]
            public void Should_Throw_If_Process_Was_Not_Started()
            {
                // Given
                var fixture = new GitLinkFixture();
                fixture.GivenProcessCannotStart();

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("GitLink: Process was not started.", result?.Message);
            }

            [Fact]
            public void Should_Throw_If_Has_A_Non_Zero_Exit_Code()
            {
                // Given
                var fixture = new GitLinkFixture();
                fixture.GivenProcessExitsWithCode(1);

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("GitLink: Process returned an error (exit code 1).", result?.Message);
            }

            [Fact]
            public void Should_Use_Provided_Repository_Root_Path_In_Process_Arguments()
            {
                // Given
                var fixture = new GitLinkFixture();
                fixture.RepositoryRootPath = "source";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("\"/Working/source\"", result.Args);
            }

            [WindowsFact]
            public void Should_Set_RepositoryUrl()
            {
                // Given
                var fixture = new GitLinkFixture();
                fixture.Settings.RepositoryUrl = "http://mydomain.com";

                // Then
                var result = fixture.Run();

                // Then
                Assert.Equal("\"c:/temp\" -u \"http://mydomain.com\"", result.Args);
            }

            [WindowsFact]
            public void Should_Set_SolutionFileName()
            {
                // Given
                var fixture = new GitLinkFixture();
                fixture.Settings.SolutionFileName = "solution.sln";

                // Then
                var result = fixture.Run();

                // Then
                Assert.Equal("\"c:/temp\" -f \"solution.sln\"", result.Args);
            }

            [WindowsFact]
            public void Should_Set_Configuration()
            {
                // Given
                var fixture = new GitLinkFixture();
                fixture.Settings.Configuration = "Release";

                // Then
                var result = fixture.Run();

                // Then
                Assert.Equal("\"c:/temp\" -c \"Release\"", result.Args);
            }

            [WindowsFact]
            public void Should_Set_Platform()
            {
                // Given
                var fixture = new GitLinkFixture();
                fixture.Settings.Platform = "AnyCPU";

                // Then
                var result = fixture.Run();

                // Then
                Assert.Equal("\"c:/temp\" -p \"AnyCPU\"", result.Args);
            }

            [WindowsFact]
            public void Should_Set_Branch()
            {
                // Given
                var fixture = new GitLinkFixture();
                fixture.Settings.Branch = "master";

                // Then
                var result = fixture.Run();

                // Then
                Assert.Equal("\"c:/temp\" -b \"master\"", result.Args);
            }

            [WindowsFact]
            public void Should_Set_LogFilePath()
            {
                // Given
                var fixture = new GitLinkFixture();
                fixture.Settings.LogFilePath = @"/temp/log.txt";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("\"c:/temp\" -l \"/temp/log.txt\"", result.Args);
            }

            [WindowsFact]
            public void Should_Set_ShaHash()
            {
                // Given
                var fixture = new GitLinkFixture();
                fixture.Settings.ShaHash = "abcdef";

                // Then
                var result = fixture.Run();

                // Then
                Assert.Equal("\"c:/temp\" -s \"abcdef\"", result.Args);
            }

            [WindowsFact]
            public void Should_Set_PdbDirectory()
            {
                // Given
                var fixture = new GitLinkFixture();
                fixture.Settings.PdbDirectoryPath = DirectoryPath.FromString("pdb/");

                // Then
                var result = fixture.Run();

                // Then
                Assert.Equal("\"c:/temp\" -d \"/Working/pdb\"", result.Args);
            }

            [WindowsFact]
            public void Should_Set_PowerShell_Switch()
            {
                // Given
                var fixture = new GitLinkFixture();
                fixture.Settings.UsePowerShell = true;

                // Then
                var result = fixture.Run();

                // Then
                Assert.Equal("\"c:/temp\" -powershell", result.Args);
            }

            [WindowsFact]
            public void Should_Set_SkipVerify_Switch()
            {
                // Given
                var fixture = new GitLinkFixture();
                fixture.Settings.SkipVerify = true;

                // Then
                var result = fixture.Run();

                // Then
                Assert.Equal("\"c:/temp\" -skipverify", result.Args);
            }

            [WindowsFact]
            public void Should_Set_IsDebug_Switch()
            {
                // Given
                var fixture = new GitLinkFixture();
                fixture.Settings.IsDebug = true;

                // Then
                var result = fixture.Run();

                // Then
                Assert.Equal("\"c:/temp\" -debug", result.Args);
            }
        }
    }
}