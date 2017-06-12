// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tests.Fixtures.Tools.GitReleaseManager;
using Cake.Testing;
using Cake.Testing.Xunit;
using Xunit;

namespace Cake.Common.Tests.Unit.Tools.GitReleaseManager.Publish
{
    public sealed class GitReleaseManagerPublisherTests
    {
        public sealed class ThePublishMethod
        {
            [Fact]
            public void Should_Throw_If_UserName_Is_Null()
            {
                // Given
                var fixture = new GitReleaseManagerPublisherFixture();
                fixture.UserName = string.Empty;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "userName");
            }

            [Fact]
            public void Should_Throw_If_Password_Is_Null()
            {
                // Given
                var fixture = new GitReleaseManagerPublisherFixture();
                fixture.Password = string.Empty;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "password");
            }

            [Fact]
            public void Should_Throw_If_Owner_Is_Null()
            {
                // Given
                var fixture = new GitReleaseManagerPublisherFixture();
                fixture.Owner = string.Empty;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "owner");
            }

            [Fact]
            public void Should_Throw_If_Repository_Is_Null()
            {
                // Given
                var fixture = new GitReleaseManagerPublisherFixture();
                fixture.Repository = string.Empty;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "repository");
            }

            [Fact]
            public void Should_Throw_If_TagName_Is_Null()
            {
                // Given
                var fixture = new GitReleaseManagerPublisherFixture();
                fixture.TagName = string.Empty;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "tagName");
            }

            [Fact]
            public void Should_Throw_If_Settings_Are_Null()
            {
                // Given
                var fixture = new GitReleaseManagerPublisherFixture();
                fixture.Settings = null;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "settings");
            }

            [Fact]
            public void Should_Throw_If_GitReleaseManager_Executable_Was_Not_Found()
            {
                // Given
                var fixture = new GitReleaseManagerPublisherFixture();
                fixture.GivenDefaultToolDoNotExist();

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsCakeException(result, "GitReleaseManager: Could not locate executable.");
            }

            [Theory]
            [InlineData("/bin/tools/GitReleaseManager/GitReleaseManager.exe", "/bin/tools/GitReleaseManager/GitReleaseManager.exe")]
            [InlineData("./tools/GitReleaseManager/GitReleaseManager.exe", "/Working/tools/GitReleaseManager/GitReleaseManager.exe")]
            public void Should_Use_NuGet_Executable_From_Tool_Path_If_Provided(string toolPath, string expected)
            {
                // Given
                var fixture = new GitReleaseManagerPublisherFixture();
                fixture.Settings.ToolPath = toolPath;
                fixture.GivenSettingsToolPathExist();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Path.FullPath);
            }

            [WindowsTheory]
            [InlineData("C:/GitReleaseManager/GitReleaseManager.exe", "C:/GitReleaseManager/GitReleaseManager.exe")]
            public void Should_Use_NuGet_Executable_From_Tool_Path_If_Provided_On_Windows(string toolPath, string expected)
            {
                // Given
                var fixture = new GitReleaseManagerPublisherFixture();
                fixture.Settings.ToolPath = toolPath;
                fixture.GivenSettingsToolPathExist();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Path.FullPath);
            }

            [Fact]
            public void Should_Throw_If_Process_Was_Not_Started()
            {
                // Given
                var fixture = new GitReleaseManagerPublisherFixture();
                fixture.GivenProcessCannotStart();

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsCakeException(result, "GitReleaseManager: Process was not started.");
            }

            [Fact]
            public void Should_Throw_If_Process_Has_A_Non_Zero_Exit_Code()
            {
                // Given
                var fixture = new GitReleaseManagerPublisherFixture();
                fixture.GivenProcessExitsWithCode(1);

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsCakeException(result, "GitReleaseManager: Process returned an error (exit code 1).");
            }

            [Fact]
            public void Should_Find_GitReleaseManager_Executable_If_Tool_Path_Not_Provided()
            {
                // Given
                var fixture = new GitReleaseManagerPublisherFixture();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(
                    "/Working/tools/GitReleaseManager.exe",
                    result.Path.FullPath);
            }

            [Fact]
            public void Should_Add_Mandatory_Arguments()
            {
                // Given
                var fixture = new GitReleaseManagerPublisherFixture();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("publish -u \"bob\" -p \"password\" " +
                             "-o \"repoOwner\" -r \"repo\" -t \"0.1.0\"",
                             result.Args);
            }

            [Fact]
            public void Should_Add_TargetDirectory_To_Arguments_If_Set()
            {
                // Given
                var fixture = new GitReleaseManagerPublisherFixture();
                fixture.Settings.TargetDirectory = @"/temp";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("publish -u \"bob\" -p \"password\" " +
                             "-o \"repoOwner\" -r \"repo\" -t \"0.1.0\" " +
                             "-d \"/temp\"", result.Args);
            }

            [Fact]
            public void Should_Add_LogFilePath_To_Arguments_If_Set()
            {
                // Given
                var fixture = new GitReleaseManagerPublisherFixture();
                fixture.Settings.LogFilePath = @"/temp/log.txt";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("publish -u \"bob\" -p \"password\" -o " +
                             "\"repoOwner\" -r \"repo\" -t \"0.1.0\" " +
                             "-l \"/temp/log.txt\"", result.Args);
            }
        }
    }
}