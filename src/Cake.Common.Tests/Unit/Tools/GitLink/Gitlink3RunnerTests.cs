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
    public sealed class Gitlink3RunnerTests
    {
        public sealed class TheRunMethod
        {
            [Fact]
            public void Should_Throw_If_Pdb_Path_Is_Null()
            {
                // Given
                var fixture = new GitLink3Fixture();
                fixture.PdbFilePath = null;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "pdbFile");
            }

            [Fact]
            public void Should_Find_GitLink_Runner()
            {
                // Given
                var fixture = new GitLink3Fixture();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/Working/tools/gitlink.exe", result.Path.FullPath);
            }

            [Fact]
            public void Should_Throw_If_Process_Was_Not_Started()
            {
                // Given
                var fixture = new GitLink3Fixture();
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
                var fixture = new GitLink3Fixture();
                fixture.GivenProcessExitsWithCode(1);

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("GitLink: Process returned an error (exit code 1).", result?.Message);
            }

            [Fact]
            public void Should_Use_Provided_Pdb_File_Path_In_Process_Arguments()
            {
                // Given
                var fixture = new GitLink3Fixture();
                fixture.PdbFilePath = "source/my.pdb";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("\"/Working/source/my.pdb\"", result.Args);
            }

            [WindowsFact]
            public void Should_Set_RepositoryUrl()
            {
                // Given
                var fixture = new GitLink3Fixture();
                fixture.Settings.RepositoryUrl = "http://mydomain.com";

                // Then
                var result = fixture.Run();

                // Then
                Assert.Equal("-u \"http://mydomain.com\" \"c:/temp/my.pdb\"", result.Args);
            }

            [WindowsFact]
            public void Should_Set_ShaHash()
            {
                // Given
                var fixture = new GitLink3Fixture();
                fixture.Settings.ShaHash = "abcdef";

                // Then
                var result = fixture.Run();

                // Then
                Assert.Equal("--commit \"abcdef\" \"c:/temp/my.pdb\"", result.Args);
            }

            [WindowsFact]
            public void Should_Set_BaseDirectory()
            {
                // Given
                var fixture = new GitLink3Fixture();
                fixture.Settings.BaseDir = DirectoryPath.FromString("pdb/");

                // Then
                var result = fixture.Run();

                // Then
                Assert.Equal("--baseDir \"/Working/pdb\" \"c:/temp/my.pdb\"", result.Args);
            }

            [WindowsFact]
            public void Should_Set_PowerShell_Switch()
            {
                // Given
                var fixture = new GitLink3Fixture();
                fixture.Settings.UsePowerShell = true;

                // Then
                var result = fixture.Run();

                // Then
                Assert.Equal("-m Powershell \"c:/temp/my.pdb\"", result.Args);
            }

            [WindowsFact]
            public void Should_Set_SkipVerify_Switch()
            {
                // Given
                var fixture = new GitLink3Fixture();
                fixture.Settings.SkipVerify = true;

                // Then
                var result = fixture.Run();

                // Then
                Assert.Equal("-s \"c:/temp/my.pdb\"", result.Args);
            }
        }
    }
}