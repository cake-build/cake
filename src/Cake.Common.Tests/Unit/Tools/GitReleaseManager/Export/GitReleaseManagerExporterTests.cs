using Cake.Common.Tests.Fixtures.Tools.GitReleaseManager;
using Cake.Core.IO;
using NSubstitute;
using Xunit;

namespace Cake.Common.Tests.Unit.Tools.GitReleaseManager.Export
{
    public sealed class GitReleaseManagerExporterTests
    {
        public sealed class TheExportMethod
        {
            [Fact]
            public void Should_Throw_If_UserName_Is_Null()
            {
                // Given
                var fixture = new GitReleaseManagerExporterFixture();
                fixture.UserName = string.Empty;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsArgumentNullException(result, "userName");
            }

            [Fact]
            public void Should_Throw_If_Password_Is_Null()
            {
                // Given
                var fixture = new GitReleaseManagerExporterFixture();
                fixture.Password = string.Empty;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsArgumentNullException(result, "password");
            }

            [Fact]
            public void Should_Throw_If_Owner_Is_Null()
            {
                // Given
                var fixture = new GitReleaseManagerExporterFixture();
                fixture.Owner = string.Empty;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsArgumentNullException(result, "owner");
            }

            [Fact]
            public void Should_Throw_If_Repository_Is_Null()
            {
                // Given
                var fixture = new GitReleaseManagerExporterFixture();
                fixture.Repository = string.Empty;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsArgumentNullException(result, "repository");
            }

            [Fact]
            public void Should_Throw_If_FileOutputPath_Is_Null()
            {
                // Given
                var fixture = new GitReleaseManagerExporterFixture();
                fixture.FileOutputPath = null;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsArgumentNullException(result, "fileOutputPath");
            }

            [Fact]
            public void Should_Throw_If_Settings_Are_Null()
            {
                // Given
                var fixture = new GitReleaseManagerExporterFixture();
                fixture.Settings = null;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsArgumentNullException(result, "settings");
            }

            [Fact]
            public void Should_Throw_If_GitReleaseManager_Executable_Was_Not_Found()
            {
                // Given
                var fixture = new GitReleaseManagerExporterFixture();
                fixture.GivenDefaultToolDoNotExist();

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsCakeException(result, "GitReleaseManager: Could not locate executable.");
            }

            [Theory]
            [InlineData("C:/GitReleaseManager/GitReleaseManager.exe", "C:/GitReleaseManager/GitReleaseManager.exe")]
            [InlineData("./tools/GitReleaseManager/GitReleaseManager.exe", "/Working/tools/GitReleaseManager/GitReleaseManager.exe")]
            public void Should_Use_NuGet_Executable_From_Tool_Path_If_Provided(string toolPath, string expected)
            {
                // Given
                var fixture = new GitReleaseManagerExporterFixture();
                fixture.Settings.ToolPath = toolPath;
                fixture.GivenSettingsToolPathExist();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.ToolPath.FullPath);
            }

            [Fact]
            public void Should_Throw_If_Process_Was_Not_Started()
            {
                // Given
                var fixture = new GitReleaseManagerExporterFixture();
                fixture.GivenProcessCannotStart();

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsCakeException(result, "GitReleaseManager: Process was not started.");
            }

            [Fact]
            public void Should_Throw_If_Process_Has_A_Non_Zero_Exit_Code()
            {
                // Given
                var fixture = new GitReleaseManagerExporterFixture();
                fixture.GivenProcessExitsWithCode(1);

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsCakeException(result, "GitReleaseManager: Process returned an error.");
            }

            [Fact]
            public void Should_Find_GitReleaseManager_Executable_If_Tool_Path_Not_Provided()
            {
                // Given
                var fixture = new GitReleaseManagerExporterFixture();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/Working/tools/GitReleaseManager.exe", result.ToolPath.FullPath);
            }

            [Fact]
            public void Should_Add_Mandatory_Arguments()
            {
                // Given
                var fixture = new GitReleaseManagerExporterFixture();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("export -u \"bob\" -p \"password\" " +
                             "-o \"repoOwner\" -r \"repo\" " +
                             "-f \"c:/temp\"", result.Args);
            }

            [Fact]
            public void Should_Add_TagName_To_Arguments_If_Set()
            {
                // Given
                var fixture = new GitReleaseManagerExporterFixture();
                fixture.Settings.TagName = "0.1.0";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("export -u \"bob\" -p \"password\" " +
                             "-o \"repoOwner\" -r \"repo\" -f \"c:/temp\" " +
                             "-t \"0.1.0\"", result.Args);
            }

            [Fact]
            public void Should_Add_TargetDirectory_To_Arguments_If_Set()
            {
                // Given
                var fixture = new GitReleaseManagerExporterFixture();
                fixture.Settings.TargetDirectory = @"c:/temp";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("export -u \"bob\" -p \"password\" " +
                             "-o \"repoOwner\" -r \"repo\" -f \"c:/temp\" " +
                             "-d \"c:/temp\"", result.Args);
            }

            [Fact]
            public void Should_Add_LogFilePath_To_Arguments_If_Set()
            {
                // Given
                var fixture = new GitReleaseManagerExporterFixture();
                fixture.Settings.LogFilePath = @"c:/temp/log.txt";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("export -u \"bob\" -p \"password\" " +
                             "-o \"repoOwner\" -r \"repo\" -f \"c:/temp\" " +
                             "-l \"c:/temp/log.txt\"", result.Args);
            }
        }
    }
}