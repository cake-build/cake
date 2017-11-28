// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tests.Fixtures.Tools;
using Cake.Common.Tools.GitVersion;
using Cake.Core;
using Cake.Testing;
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

            [Fact]
            public void Should_Add_OutputType_To_Arguments_If_Set()
            {
                // Given
                var fixture = new GitVersionRunnerFixture();
                fixture.Settings.OutputType = GitVersionOutput.Json;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("-output json", result.Args);
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
                        PreReleaseNumber = null,
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
                        Sha = "f2467748c78b3c8b37972ad0b30df2e15dfbf2cb",
                        NuGetVersionV2 = "0.1.1",
                        NuGetVersion = "0.1.1",
                        CommitsSinceVersionSource = null,
                        CommitsSinceVersionSourcePadded = "0002",
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
                        "  \"PreReleaseNumber\":\"\",",
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
                        "  \"Sha\":\"f2467748c78b3c8b37972ad0b30df2e15dfbf2cb\",",
                        "  \"NuGetVersionV2\":\"0.1.1\",",
                        "  \"NuGetVersion\":\"0.1.1\",",
                        "  \"CommitsSinceVersionSource\":\"\",",
                        "  \"CommitsSinceVersionSourcePadded\":\"0002\",",
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
                Assert.Equal(expect.PreReleaseNumber, result.PreReleaseNumber);
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
                Assert.Equal(expect.Sha, result.Sha);
                Assert.Equal(expect.NuGetVersionV2, result.NuGetVersionV2);
                Assert.Equal(expect.NuGetVersion, result.NuGetVersion);
                Assert.Equal(expect.CommitsSinceVersionSource, result.CommitsSinceVersionSource);
                Assert.Equal(expect.CommitsSinceVersionSourcePadded, result.CommitsSinceVersionSourcePadded);
                Assert.Equal(expect.CommitDate, result.CommitDate);
            }
        }
    }
}