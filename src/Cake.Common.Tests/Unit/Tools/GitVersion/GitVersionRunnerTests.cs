// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Common.Tests.Fixtures.Tools;
using Cake.Common.Tools.GitVersion;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Testing;
using Cake.Testing.Extensions;
using Xunit;

namespace Cake.Common.Tests.Unit.Tools.GitVersion
{
    public sealed class GitVersionRunnerTests
    {
        public sealed class TheExecutable
        {
            [Fact]
            public void Should_Throw_If_Settings_Is_Null()
            {
                // Given
                var fixture = new GitVersionRunnerFixture();
                fixture.Settings = null;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "settings");
            }

            [Fact]
            public void Should_Find_GitVersion_Runner()
            {
                // Given
                var fixture = new GitVersionRunnerFixture();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/Working/tools/GitVersion.exe", result.Path.FullPath);
            }

            [Fact]
            public void Should_Throw_If_Process_Was_Not_Started()
            {
                // Given
                var fixture = new GitVersionRunnerFixture();
                fixture.GivenProcessCannotStart();

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("GitVersion: Process was not started.", result?.Message);
            }

            [Fact]
            public void Should_Throw_If_Has_A_Non_Zero_Exit_Code()
            {
                // Given
                var fixture = new GitVersionRunnerFixture();
                fixture.GivenProcessExitsWithCode(1);

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("GitVersion: Process returned an error (exit code 1).", result?.Message);
            }

            [Fact]
            public void Should_Set_Working_Directory()
            {
                // Given
                var fixture = new GitVersionRunnerFixture();
                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/Working", result.Process.WorkingDirectory.FullPath);
            }

            [Theory]
            [InlineData(GitVersionOutput.Json, "-output json")]
            [InlineData(GitVersionOutput.File, "-output file")]
            public void Should_Add_OutputType_To_Arguments_If_Set(GitVersionOutput outputType, string args)
            {
                // Given
                var fixture = new GitVersionRunnerFixture();
                fixture.Settings.OutputType = outputType;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(args, result.Args);
            }

            [Fact]
            public void Should_Add_OutputFile_If_Set_With_OutputType_File()
            {
                // Given
                var fixture = new GitVersionRunnerFixture();
                fixture.Settings.OutputType = GitVersionOutput.File;
                fixture.Settings.OutputFile = "GitVersion.json";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("-output file -outputfile \"GitVersion.json\"", result.Args);
            }

            [Fact]
            public void Should_Add_ShowVariable_To_Arguments_If_Set()
            {
                // Given
                var fixture = new GitVersionRunnerFixture();
                fixture.Settings.ShowVariable = "FullSemVer";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("-showvariable FullSemVer", result.Args);
            }

            [Fact]
            public void Should_Add_Username_And_Password_To_Arguments_If_Set()
            {
                // Given
                var fixture = new GitVersionRunnerFixture();
                fixture.Settings.UserName = "bob";
                fixture.Settings.Password = "password";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("-u \"bob\" -p \"password\"", result.Args);
            }

            [Fact]
            public void Should_Add_UpdateAssemblyInfo_To_Arguments_If_Set()
            {
                // Given
                var fixture = new GitVersionRunnerFixture();
                fixture.Settings.UpdateAssemblyInfo = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("-updateassemblyinfo", result.Args);
            }

            [Fact]
            public void Should_Add_UpdateAssemblyInfoFilePath_To_Arguments_If_Set()
            {
                // Given
                var fixture = new GitVersionRunnerFixture();
                fixture.Settings.UpdateAssemblyInfo = true;
                fixture.Settings.UpdateAssemblyInfoFilePath = "c:/temp/assemblyinfo.cs";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("-updateassemblyinfo \"c:/temp/assemblyinfo.cs\"", result.Args);
            }

            [Fact]
            public void Should_Add_ConfigFile_To_Arguments_If_Set()
            {
                // Given
                var fixture = new GitVersionRunnerFixture();
                fixture.Settings.ConfigFile = "c:/temp/gitversion.yml";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("-config \"c:/temp/gitversion.yml\"", result.Args);
            }

            [Fact]
            public void Should_Add_RepositoryPath_To_Arguments_If_Set()
            {
                // Given
                var fixture = new GitVersionRunnerFixture();
                fixture.Settings.RepositoryPath = "c:/temp";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("-targetpath \"c:/temp\"", result.Args);
            }

            [Fact]
            public void Should_Add_DynamicRepoSettings_To_Arguments_If_Set()
            {
                // Given
                var fixture = new GitVersionRunnerFixture();
                fixture.Settings.Url = "http://mygitrepo.co.uk";
                fixture.Settings.Branch = "master";
                fixture.Settings.Commit = "abcdef";
                fixture.Settings.DynamicRepositoryPath = "c:/temp";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("-url \"http://mygitrepo.co.uk\" -b master -c \"abcdef\" -dynamicRepoLocation \"c:/temp\"", result.Args);
            }

            [Fact]
            public void Should_Add_LogFilePath_To_Arguments_If_Set()
            {
                // Given
                var fixture = new GitVersionRunnerFixture();
                fixture.Settings.LogFilePath = "c:/temp/gitversion.log";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("-l \"c:/temp/gitversion.log\"", result.Args);
            }

            [Theory]
            [InlineData(true, "-nofetch")]
            [InlineData(false, "")]
            public void Should_Add_NoFetch_To_Arguments_If_Set(bool nofetch, string args)
            {
                // Given
                var fixture = new GitVersionRunnerFixture();
                fixture.Settings.NoFetch = nofetch;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(args, result.Args);
            }

            [Theory]
            [InlineData(true, "-nocache")]
            [InlineData(false, "")]
            public void Should_Add_NoCache_To_Arguments_If_Set(bool nocache, string args)
            {
                // Given
                var fixture = new GitVersionRunnerFixture();
                fixture.Settings.NoCache = nocache;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(args, result.Args);
            }

            [Theory]
            [InlineData(true, "-nonormalize")]
            [InlineData(false, "")]
            public void Should_Add_NoNormalize_To_Arguments_If_Set(bool nonormalize, string args)
            {
                // Given
                var fixture = new GitVersionRunnerFixture();
                fixture.Settings.NoNormalize = nonormalize;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(args, result.Args);
            }

            [Theory]
            [InlineData(true, "-diag")]
            [InlineData(false, "")]
            public void Should_Add_Diag_To_Arguments_If_Set(bool diag, string args)
            {
                // Given
                var fixture = new GitVersionRunnerFixture();
                fixture.Settings.Diag = diag;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(args, result.Args);
            }

            [Theory]
            [InlineData(true, "-updateprojectfiles")]
            [InlineData(false, "")]
            public void Should_Add_UpdateProjectFiles_To_Arguments_If_Set(bool updateProjectFiles, string args)
            {
                // Given
                var fixture = new GitVersionRunnerFixture();
                fixture.Settings.UpdateProjectFiles = updateProjectFiles;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(args, result.Args);
            }

            [Theory]
            [InlineData(true, "-ensureassemblyinfo")]
            [InlineData(false, "")]
            public void Should_Add_EnsureAssemblyInfo_To_Arguments_If_Set(bool ensureAssemblyInfo, string args)
            {
                // Given
                var fixture = new GitVersionRunnerFixture();
                fixture.Settings.EnsureAssemblyInfo = ensureAssemblyInfo;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(args, result.Args);
            }

            [Theory]
            [InlineData(true, "-updatewixversionfile")]
            [InlineData(false, "")]
            public void Should_Add_UpdateWixVersionFile_To_Arguments_If_Set(bool updateWixVersionFile, string args)
            {
                // Given
                var fixture = new GitVersionRunnerFixture();
                fixture.Settings.UpdateWixVersionFile = updateWixVersionFile;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(args, result.Args);
            }

            [Theory]
            [InlineData(Verbosity.Quiet)]
            [InlineData(Verbosity.Minimal)]
            [InlineData(Verbosity.Normal)]
            [InlineData(Verbosity.Verbose)]
            [InlineData(Verbosity.Diagnostic)]
            public void Should_Log_GitVersion_Errors_When_Verbosity_Less_Than_Diagnostic(Verbosity verbosity)
            {
                // Given
                var log = new FakeLog { Verbosity = verbosity };
                var fixture = new GitVersionRunnerFixture(new[]
                {
                    "  INFO [02/29/20 20:29:12:17] No local branch pointing at the commit '57a682f6012d1f27255de86240fa98e87fe1f765'. Fake branch needs to be created.  INFO [02/29/20 20:29:12:17] Fetching remote refs to see if there is a pull request ref  INFO [02/29/20 20:29:12:20] End: Normalizing git directory for branch '' (Took: 89.28ms)  ERROR [02/29/20 20:29:12:21] An unexpected error occurred:",
                    "LibGit2Sharp.LibGit2SharpException: this remote has never connected",
                    "   at LibGit2Sharp.Core.Ensure.HandleError(Int32 result)",
                    "...",
                    "  INFO [02/29/20 20:43:18:60] No local branch pointing at the commit 'c9d51bc9836a310145b3d8976a69b1859be36a35'. Fake branch needs to be created.",
                    "  INFO [02/29/20 20:43:18:60] Fetching remote refs to see if there is a pull request ref",
                    "  INFO [02/29/20 20:43:18:66] End: Normalizing git directory for branch '' (Took: 124.33ms)",
                    "  ERROR [02/29/20 20:43:18:68] An unexpected error occurred:",
                    "LibGit2Sharp.LibGit2SharpException: this remote has never connected",
                    "   at LibGit2Sharp.Core.Ensure.HandleError(Int32 result)",
                    "..."
                });
                fixture.Log = log;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.Equal(verbosity < Verbosity.Diagnostic
                    ? "  ERROR [02/29/20 20:29:12:21] An unexpected error occurred:" + Environment.NewLine +
                    "LibGit2Sharp.LibGit2SharpException: this remote has never connected" + Environment.NewLine +
                    "  ERROR [02/29/20 20:43:18:68] An unexpected error occurred:" + Environment.NewLine +
                    "LibGit2Sharp.LibGit2SharpException: this remote has never connected" + Environment.NewLine
                    : string.Empty,
                    log.AggregateLogMessages());
            }

            [Fact]
            public void Should_Tolerate_Bad_Json_Set()
            {
                // Given
                var expect = new Common.Tools.GitVersion.GitVersion
                    {
                        Major = 0,
                        Minor = 1,
                        Patch = 1,
                        PreReleaseTag = "PreReleaseTag",
                        PreReleaseTagWithDash = "PreReleaseTagWithDash",
                        PreReleaseLabel = "PreReleaseLabel",
                        PreReleaseLabelWithDash = "-PreReleaseLabel",
                        PreReleaseNumber = null,
                        WeightedPreReleaseNumber = null,
                        BuildMetaData = "BuildMetaData",
                        BuildMetaDataPadded = "BuildMetaDataPadded",
                        FullBuildMetaData = "Branch.master.Sha.f2467748c78b3c8b37972ad0b30df2e15dfbf2cb",
                        MajorMinorPatch = "0.1.1",
                        SemVer = "0.1.1",
                        LegacySemVer = "0.1.1",
                        LegacySemVerPadded = "0.1.1",
                        AssemblySemVer = "0.1.1.0",
                        AssemblySemFileVer = "0.1.1.0",
                        FullSemVer = "0.1.1",
                        InformationalVersion = "0.1.1+Branch.master.Sha.f2467748c78b3c8b37972ad0b30df2e15dfbf2cb",
                        BranchName = "master",
                        EscapedBranchName = "master",
                        Sha = "f2467748c78b3c8b37972ad0b30df2e15dfbf2cb",
                        ShortSha = "f2467748",
                        NuGetVersionV2 = "0.1.1",
                        NuGetVersion = "0.1.1",
                        NuGetPreReleaseTagV2 = "tag",
                        NuGetPreReleaseTag = "tag",
                        VersionSourceSha = "f2467748c78b3c8b37972ad0b30df2e15dfbf2cb",
                        CommitsSinceVersionSource = null,
                        CommitsSinceVersionSourcePadded = "0002",
                        UncommittedChanges = 0,
                        CommitDate = "2017-09-13",
                    }
                    ;
                var fixture = new GitVersionRunnerFixture(
                    new[]
                    {
                        "{",
                        "  \"Major\":0,",
                        "  \"Minor\":1,",
                        "  \"Patch\":1,",
                        "  \"PreReleaseTag\":\"PreReleaseTag\",",
                        "  \"PreReleaseTagWithDash\":\"PreReleaseTagWithDash\",",
                        "  \"PreReleaseLabel\":\"PreReleaseLabel\",",
                        "  \"PreReleaseLabelWithDash\":\"-PreReleaseLabel\",",
                        "  \"PreReleaseNumber\":\"\",",
                        "  \"WeightedPreReleaseNumber\":\"\",",
                        "  \"BuildMetaData\":\"BuildMetaData\",",
                        "  \"BuildMetaDataPadded\":\"BuildMetaDataPadded\",",
                        "  \"FullBuildMetaData\":\"Branch.master.Sha.f2467748c78b3c8b37972ad0b30df2e15dfbf2cb\",",
                        "  \"MajorMinorPatch\":\"0.1.1\",",
                        "  \"SemVer\":\"0.1.1\",",
                        "  \"LegacySemVer\":\"0.1.1\",",
                        "  \"LegacySemVerPadded\":\"0.1.1\",",
                        "  \"AssemblySemVer\":\"0.1.1.0\",",
                        "  \"AssemblySemFileVer\":\"0.1.1.0\",",
                        "  \"FullSemVer\":\"0.1.1\",",
                        "  \"InformationalVersion\":\"0.1.1+Branch.master.Sha.f2467748c78b3c8b37972ad0b30df2e15dfbf2cb\",",
                        "  \"BranchName\":\"master\",",
                        "  \"EscapedBranchName\":\"master\",",
                        "  \"Sha\":\"f2467748c78b3c8b37972ad0b30df2e15dfbf2cb\",",
                        "  \"ShortSha\":\"f2467748\",",
                        "  \"NuGetVersionV2\":\"0.1.1\",",
                        "  \"NuGetVersion\":\"0.1.1\",",
                        "  \"NuGetPreReleaseTagV2\":\"tag\",",
                        "  \"NuGetPreReleaseTag\":\"tag\",",
                        "  \"VersionSourceSha\":\"f2467748c78b3c8b37972ad0b30df2e15dfbf2cb\",",
                        "  \"CommitsSinceVersionSource\":\"\",",
                        "  \"CommitsSinceVersionSourcePadded\":\"0002\",",
                        "  \"UncommittedChanges\":\"0\",",
                        "  \"CommitDate\":\"2017-09-13\"",
                        "}"
                    });
                fixture.Settings.OutputType = GitVersionOutput.Json;

                // When
                var result = fixture.RunGitVersion();

                // Then
                Assert.Equal(expect.Major, result.Major);
                Assert.Equal(expect.Minor, result.Minor);
                Assert.Equal(expect.Patch, result.Patch);
                Assert.Equal(expect.PreReleaseTag, result.PreReleaseTag);
                Assert.Equal(expect.PreReleaseTagWithDash, result.PreReleaseTagWithDash);
                Assert.Equal(expect.PreReleaseLabel, result.PreReleaseLabel);
                Assert.Equal(expect.PreReleaseLabelWithDash, result.PreReleaseLabelWithDash);
                Assert.Equal(expect.PreReleaseNumber, result.PreReleaseNumber);
                Assert.Equal(expect.WeightedPreReleaseNumber, result.WeightedPreReleaseNumber);
                Assert.Equal(expect.BuildMetaData, result.BuildMetaData);
                Assert.Equal(expect.BuildMetaDataPadded, result.BuildMetaDataPadded);
                Assert.Equal(expect.FullBuildMetaData, result.FullBuildMetaData);
                Assert.Equal(expect.MajorMinorPatch, result.MajorMinorPatch);
                Assert.Equal(expect.SemVer, result.SemVer);
                Assert.Equal(expect.LegacySemVer, result.LegacySemVer);
                Assert.Equal(expect.LegacySemVerPadded, result.LegacySemVerPadded);
                Assert.Equal(expect.AssemblySemVer, result.AssemblySemVer);
                Assert.Equal(expect.AssemblySemFileVer, result.AssemblySemFileVer);
                Assert.Equal(expect.FullSemVer, result.FullSemVer);
                Assert.Equal(expect.InformationalVersion, result.InformationalVersion);
                Assert.Equal(expect.BranchName, result.BranchName);
                Assert.Equal(expect.EscapedBranchName, result.EscapedBranchName);
                Assert.Equal(expect.Sha, result.Sha);
                Assert.Equal(expect.ShortSha, result.ShortSha);
                Assert.Equal(expect.NuGetVersionV2, result.NuGetVersionV2);
                Assert.Equal(expect.NuGetVersion, result.NuGetVersion);
                Assert.Equal(expect.NuGetPreReleaseTagV2, result.NuGetPreReleaseTagV2);
                Assert.Equal(expect.NuGetPreReleaseTag, result.NuGetPreReleaseTag);
                Assert.Equal(expect.VersionSourceSha, result.VersionSourceSha);
                Assert.Equal(expect.CommitsSinceVersionSource, result.CommitsSinceVersionSource);
                Assert.Equal(expect.CommitsSinceVersionSourcePadded, result.CommitsSinceVersionSourcePadded);
                Assert.Equal(expect.UncommittedChanges, result.UncommittedChanges);
                Assert.Equal(expect.CommitDate, result.CommitDate);
            }

            [Theory]
            [InlineData(GitVersionVerbosity.Quiet, nameof(GitVersionVerbosity.Quiet))]
            [InlineData(GitVersionVerbosity.Diagnostic, nameof(GitVersionVerbosity.Diagnostic))]
            [InlineData(GitVersionVerbosity.Verbose, nameof(GitVersionVerbosity.Verbose))]
            [InlineData(GitVersionVerbosity.Normal, nameof(GitVersionVerbosity.Normal))]
            [InlineData(GitVersionVerbosity.Minimal, nameof(GitVersionVerbosity.Minimal))]
            public void Should_Add_Verbosity_To_Arguments_If_Set(GitVersionVerbosity verbosity, string arg)
            {
                // Given
                var fixture = new GitVersionRunnerFixture();
                fixture.Settings.Verbosity = verbosity;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal($"-verbosity {arg}", result.Args);
            }

            [Theory]
            [InlineData(Verbosity.Quiet, nameof(Verbosity.Quiet))]
            [InlineData(Verbosity.Diagnostic, nameof(Verbosity.Diagnostic))]
            [InlineData(Verbosity.Verbose, nameof(Verbosity.Verbose))]
            [InlineData(Verbosity.Minimal, nameof(Verbosity.Minimal))]
            public void Should_Add_Default_Verbosity_To_Arguments_If_Not_Set(Verbosity verbosity, string arg)
            {
                // Given
                var fixture = new GitVersionRunnerFixture();
                fixture.SetLogVerbosity(verbosity);

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal($"-verbosity {arg}", result.Args);
            }
        }
    }
}