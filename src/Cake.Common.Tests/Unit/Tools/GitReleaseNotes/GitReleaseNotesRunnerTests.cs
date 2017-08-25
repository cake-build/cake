// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tests.Fixtures.Tools;
using Cake.Common.Tools.GitReleaseNotes;
using Cake.Core;
using Cake.Testing;
using Xunit;

namespace Cake.Common.Tests.Unit.Tools.GitReleaseNotes
{
    public sealed class GitReleaseNotesRunnerTests
    {
        public sealed class TheExecutable
        {
            [Fact]
            public void Should_Throw_If_OutputFile_Is_Null()
            {
                // Given
                var fixture = new GitReleaseNotesRunnerFixture();
                fixture.OutputFile = null;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "outputFile");
            }

            [Fact]
            public void Should_Throw_If_Settings_Is_Null()
            {
                // Given
                var fixture = new GitReleaseNotesRunnerFixture();
                fixture.Settings = null;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "settings");
            }

            [Fact]
            public void Should_Find_GitReleaseNotes_Runner()
            {
                // Given
                var fixture = new GitReleaseNotesRunnerFixture();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/Working/tools/GitReleaseNotes.exe", result.Path.FullPath);
            }

            [Fact]
            public void Should_Throw_If_Process_Was_Not_Started()
            {
                // Given
                var fixture = new GitReleaseNotesRunnerFixture();
                fixture.GivenProcessCannotStart();

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("GitReleaseNotes: Process was not started.", result?.Message);
            }

            [Fact]
            public void Should_Throw_If_Has_A_Non_Zero_Exit_Code()
            {
                // Given
                var fixture = new GitReleaseNotesRunnerFixture();
                fixture.GivenProcessExitsWithCode(1);

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("GitReleaseNotes: Process returned an error (exit code 1).", result?.Message);
            }

            [Fact]
            public void Should_Add_Mandatory_Arguments()
            {
                // Given
                var fixture = new GitReleaseNotesRunnerFixture();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/OutputFile \"/temp/releasenotes.md\"", result.Args);
            }

            [Fact]
            public void Should_Add_WorkingDirectory_To_Arguments_If_Set()
            {
                // Given
                var fixture = new GitReleaseNotesRunnerFixture();
                fixture.Settings.WorkingDirectory = "/temp";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("\"/temp\" /OutputFile \"/temp/releasenotes.md\"", result.Args);
            }

            [Fact]
            public void Should_Add_Verbose_To_Arguments_If_Set()
            {
                // Given
                var fixture = new GitReleaseNotesRunnerFixture();
                fixture.Settings.Verbose = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/OutputFile \"/temp/releasenotes.md\" /Verbose", result.Args);
            }

            [Theory]
            [InlineData(GitReleaseNotesIssueTracker.BitBucket, "/OutputFile \"/temp/releasenotes.md\" /IssueTracker BitBucket")]
            [InlineData(GitReleaseNotesIssueTracker.GitHub, "/OutputFile \"/temp/releasenotes.md\" /IssueTracker GitHub")]
            [InlineData(GitReleaseNotesIssueTracker.Jira, "/OutputFile \"/temp/releasenotes.md\" /IssueTracker Jira")]
            [InlineData(GitReleaseNotesIssueTracker.YouTrack, "/OutputFile \"/temp/releasenotes.md\" /IssueTracker YouTrack")]
            public void Should_Add_IssueTracker_To_Arguments_If_Set(GitReleaseNotesIssueTracker issueTracker, string expected)
            {
                // Given
                var fixture = new GitReleaseNotesRunnerFixture();
                fixture.Settings.IssueTracker = issueTracker;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Fact]
            public void Should_Add_AllTags_To_Arguments_If_Set()
            {
                // Given
                var fixture = new GitReleaseNotesRunnerFixture();
                fixture.Settings.AllTags = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/OutputFile \"/temp/releasenotes.md\" /AllTags", result.Args);
            }

            [Fact]
            public void Should_Add_RepoUserName_To_Arguments_If_Set()
            {
                // Given
                var fixture = new GitReleaseNotesRunnerFixture();
                fixture.Settings.RepoUserName = "bob";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/OutputFile \"/temp/releasenotes.md\" /RepoUsername \"bob\"", result.Args);
            }

            [Fact]
            public void Should_Add_RepoPassword_To_Arguments_If_Set()
            {
                // Given
                var fixture = new GitReleaseNotesRunnerFixture();
                fixture.Settings.RepoPassword = "password";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/OutputFile \"/temp/releasenotes.md\" /RepoPassword \"password\"", result.Args);
            }

            [Fact]
            public void Should_Add_RepoToken_To_Arguments_If_Set()
            {
                // Given
                var fixture = new GitReleaseNotesRunnerFixture();
                fixture.Settings.RepoToken = "token123";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/OutputFile \"/temp/releasenotes.md\" /RepoToken \"token123\"", result.Args);
            }

            [Fact]
            public void Should_Add_RepoUrl_To_Arguments_If_Set()
            {
                // Given
                var fixture = new GitReleaseNotesRunnerFixture();
                fixture.Settings.RepoUrl = "http://myrepo.co.uk";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/OutputFile \"/temp/releasenotes.md\" /RepoUrl \"http://myrepo.co.uk\"", result.Args);
            }

            [Fact]
            public void Should_Add_RepoBranch_To_Arguments_If_Set()
            {
                // Given
                var fixture = new GitReleaseNotesRunnerFixture();
                fixture.Settings.RepoBranch = "master";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/OutputFile \"/temp/releasenotes.md\" /RepoBranch \"master\"", result.Args);
            }

            [Fact]
            public void Should_Add_IssueTrackerUrl_To_Arguments_If_Set()
            {
                // Given
                var fixture = new GitReleaseNotesRunnerFixture();
                fixture.Settings.IssueTrackerUrl = "http://myissuetracker.co.uk";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/OutputFile \"/temp/releasenotes.md\" /IssueTrackerUrl \"http://myissuetracker.co.uk\"", result.Args);
            }

            [Fact]
            public void Should_Add_IssueTrackerUserName_To_Arguments_If_Set()
            {
                // Given
                var fixture = new GitReleaseNotesRunnerFixture();
                fixture.Settings.IssueTrackerUserName = "bob";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/OutputFile \"/temp/releasenotes.md\" /IssueTrackerUsername \"bob\"", result.Args);
            }

            [Fact]
            public void Should_Add_IssueTrackerPassword_To_Arguments_If_Set()
            {
                // Given
                var fixture = new GitReleaseNotesRunnerFixture();
                fixture.Settings.IssueTrackerPassword = "password";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/OutputFile \"/temp/releasenotes.md\" /IssueTrackerPassword \"password\"", result.Args);
            }

            [Fact]
            public void Should_Add_IssueTrackerToken_To_Arguments_If_Set()
            {
                // Given
                var fixture = new GitReleaseNotesRunnerFixture();
                fixture.Settings.IssueTrackerToken = "token123";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/OutputFile \"/temp/releasenotes.md\" /IssueTrackerToken \"token123\"", result.Args);
            }

            [Fact]
            public void Should_Add_IssueTrackerProjectId_To_Arguments_If_Set()
            {
                // Given
                var fixture = new GitReleaseNotesRunnerFixture();
                fixture.Settings.IssueTrackerProjectId = "ProjectId123";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/OutputFile \"/temp/releasenotes.md\" /IssueTrackerProjectId \"ProjectId123\"", result.Args);
            }

            [Fact]
            public void Should_Add_IssueTrackerFilter_To_Arguments_If_Set()
            {
                // Given
                var fixture = new GitReleaseNotesRunnerFixture();
                fixture.Settings.IssueTrackerFilter = "filter";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/OutputFile \"/temp/releasenotes.md\" /IssueTrackerFilter \"filter\"", result.Args);
            }

            [Fact]
            public void Should_Add_Categories_To_Arguments_If_Set()
            {
                // Given
                var fixture = new GitReleaseNotesRunnerFixture();
                fixture.Settings.Categories = "Category1";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/OutputFile \"/temp/releasenotes.md\" /Categories \"Category1\"", result.Args);
            }

            [Fact]
            public void Should_Add_Version_To_Arguments_If_Set()
            {
                // Given
                var fixture = new GitReleaseNotesRunnerFixture();
                fixture.Settings.Version = "1.2.3.4";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/OutputFile \"/temp/releasenotes.md\" /Version \"1.2.3.4\"", result.Args);
            }

            [Fact]
            public void Should_Add_AllLabels_To_Arguments_If_Set()
            {
                // Given
                var fixture = new GitReleaseNotesRunnerFixture();
                fixture.Settings.AllLabels = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/OutputFile \"/temp/releasenotes.md\" /AllLabels", result.Args);
            }
        }
    }
}