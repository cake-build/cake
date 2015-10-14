using Cake.Common.Tests.Fixtures.Tools.GitReleaseManager;
using Cake.Core.IO;
using NSubstitute;
using Xunit;

namespace Cake.Common.Tests.Unit.Tools.GitReleaseManager.Create
{
    public sealed class GitReleaseManagerCreatorTests
    {
        public sealed class TheCreateMethod
        {
            [Fact]
            public void Should_Throw_If_UserName_Is_Null()
            {
                // Given
                var fixture = new GitReleaseManagerCreatorFixture();
                fixture.UserName = string.Empty;

                // When
                var result = Record.Exception(() => fixture.Create());

                // Then
                Assert.IsArgumentNullException(result, "userName");
            }

            [Fact]
            public void Should_Throw_If_Password_Is_Null()
            {
                // Given
                var fixture = new GitReleaseManagerCreatorFixture();
                fixture.Password = string.Empty;

                // When
                var result = Record.Exception(() => fixture.Create());

                // Then
                Assert.IsArgumentNullException(result, "password");
            }

            [Fact]
            public void Should_Throw_If_Owner_Is_Null()
            {
                // Given
                var fixture = new GitReleaseManagerCreatorFixture();
                fixture.Owner = string.Empty;

                // When
                var result = Record.Exception(() => fixture.Create());

                // Then
                Assert.IsArgumentNullException(result, "owner");
            }

            [Fact]
            public void Should_Throw_If_Repository_Is_Null()
            {
                // Given
                var fixture = new GitReleaseManagerCreatorFixture();
                fixture.Repository = string.Empty;

                // When
                var result = Record.Exception(() => fixture.Create());

                // Then
                Assert.IsArgumentNullException(result, "repository");
            }

            [Fact]
            public void Should_Throw_If_Settings_Are_Null()
            {
                // Given
                var fixture = new GitReleaseManagerCreatorFixture();
                fixture.Settings = null;

                // When
                var result = Record.Exception(() => fixture.Create());

                // Then
                Assert.IsArgumentNullException(result, "settings");
            }

            [Fact]
            public void Should_Throw_If_GitReleaseManager_Executable_Was_Not_Found()
            {
                // Given
                var fixture = new GitReleaseManagerCreatorFixture();
                fixture.GivenDefaultToolDoNotExist();

                // When
                var result = Record.Exception(() => fixture.Create());

                // Then
                Assert.IsCakeException(result, "GitReleaseManager: Could not locate executable.");
            }

            [Theory]
            [InlineData("C:/GitReleaseManager/GitReleaseManager.exe", "C:/GitReleaseManager/GitReleaseManager.exe")]
            [InlineData("./tools/GitReleaseManager/GitReleaseManager.exe", "/Working/tools/GitReleaseManager/GitReleaseManager.exe")]
            public void Should_Use_NuGet_Executable_From_Tool_Path_If_Provided(string toolPath, string expected)
            {
                // Given
                var fixture = new GitReleaseManagerCreatorFixture();
                fixture.GivenCustomToolPathExist(expected);
                fixture.Settings.ToolPath = toolPath;

                // When
                fixture.Create();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Is<FilePath>(p => p.FullPath == expected),
                    Arg.Any<ProcessSettings>());
            }

            [Fact]
            public void Should_Throw_If_Process_Was_Not_Started()
            {
                // Given
                var fixture = new GitReleaseManagerCreatorFixture();
                fixture.GivenProcessCannotStart();

                // When
                var result = Record.Exception(() => fixture.Create());

                // Then
                Assert.IsCakeException(result, "GitReleaseManager: Process was not started.");
            }

            [Fact]
            public void Should_Throw_If_Process_Has_A_Non_Zero_Exit_Code()
            {
                // Given
                var fixture = new GitReleaseManagerCreatorFixture();
                fixture.GivenProcessReturnError();

                // When
                var result = Record.Exception(() => fixture.Create());

                // Then
                Assert.IsCakeException(result, "GitReleaseManager: Process returned an error.");
            }

            [Fact]
            public void Should_Find_GitReleaseManager_Executable_If_Tool_Path_Not_Provided()
            {
                // Given
                var fixture = new GitReleaseManagerCreatorFixture();

                // When
                fixture.Create();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Is<FilePath>(p => p.FullPath == "/Working/tools/GitReleaseManager.exe"),
                    Arg.Any<ProcessSettings>());
            }

            [Fact]
            public void Should_Add_Mandatory_Arguments()
            {
                // Given
                var fixture = new GitReleaseManagerCreatorFixture();

                // When
                fixture.Create();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() == "create -u \"bob\" -p \"password\" -o \"repoOwner\" -r \"repo\""));
            }

            [Fact]
            public void Should_Add_Milestone_To_Arguments_If_True()
            {
                // Given
                var fixture = new GitReleaseManagerCreatorFixture();
                fixture.Settings.Milestone = "1.0.0";

                // When
                fixture.Create();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() == "create -u \"bob\" -p \"password\" -o \"repoOwner\" -r \"repo\" -m \"1.0.0\""));
            }

            [Fact]
            public void Should_Add_Name_To_Arguments_If_Set()
            {
                // Given
                var fixture = new GitReleaseManagerCreatorFixture();
                fixture.Settings.Name = "1.0.0";

                // When
                fixture.Create();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() == "create -u \"bob\" -p \"password\" -o \"repoOwner\" -r \"repo\" -n \"1.0.0\""));
            }

            [Fact]
            public void Should_Add_InputFilePath_To_Arguments_If_Set()
            {
                // Given
                var fixture = new GitReleaseManagerCreatorFixture();
                fixture.Settings.InputFilePath = @"c:/temp";

                // When
                fixture.Create();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() == "create -u \"bob\" -p \"password\" -o \"repoOwner\" -r \"repo\" -i \"c:/temp\""));
            }

            [Fact]
            public void Should_Add_Prerelease_To_Arguments_If_True()
            {
                // Given
                var fixture = new GitReleaseManagerCreatorFixture();
                fixture.Settings.Prerelease = true;

                // When
                fixture.Create();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() == "create -u \"bob\" -p \"password\" -o \"repoOwner\" -r \"repo\" -pre"));
            }

            [Fact]
            public void Should_Add_Assets_To_Arguments_If_Set()
            {
                // Given
                var fixture = new GitReleaseManagerCreatorFixture();
                fixture.Settings.Assets = "asset1,asset2";

                // When
                fixture.Create();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() == "create -u \"bob\" -p \"password\" -o \"repoOwner\" -r \"repo\" -a \"asset1,asset2\""));
            }

            [Fact]
            public void Should_Add_TargetCommitish_To_Arguments_If_Set()
            {
                // Given
                var fixture = new GitReleaseManagerCreatorFixture();
                fixture.Settings.TargetCommitish = "master";

                // When
                fixture.Create();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() == "create -u \"bob\" -p \"password\" -o \"repoOwner\" -r \"repo\" -c \"master\""));
            }

            [Fact]
            public void Should_Add_TargetDirectory_To_Arguments_If_Set()
            {
                // Given
                var fixture = new GitReleaseManagerCreatorFixture();
                fixture.Settings.TargetDirectory = @"c:/temp";

                // When
                fixture.Create();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() == "create -u \"bob\" -p \"password\" -o \"repoOwner\" -r \"repo\" -d \"c:/temp\""));
            }

            [Fact]
            public void Should_Add_LogFilePath_To_Arguments_If_Set()
            {
                // Given
                var fixture = new GitReleaseManagerCreatorFixture();
                fixture.Settings.LogFilePath = @"c:/temp/log.txt";

                // When
                fixture.Create();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() == "create -u \"bob\" -p \"password\" -o \"repoOwner\" -r \"repo\" -l \"c:/temp/log.txt\""));
            }
        }
    }
}