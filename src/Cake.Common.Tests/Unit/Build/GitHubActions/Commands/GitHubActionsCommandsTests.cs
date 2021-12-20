using System.IO;
using System.Threading.Tasks;
using Cake.Common.Build.GitHubActions.Commands;
using Cake.Common.Tests.Fixtures.Build;
using Cake.Core;
using Cake.Core.IO;
using Cake.Testing;
using NSubstitute;
using Xunit;

namespace Cake.Common.Tests.Unit.Build.GitHubActions.Commands
{
    public sealed class GitHubActionsCommandsTests
    {
        public sealed class TheConstructor
        {
            [Fact]
            public void Should_Throw_If_Environment_Is_Null()
            {
                // Given, When
                var result = Record.Exception(() => new GitHubActionsCommands(null, null, null, null));

                // Then
                AssertEx.IsArgumentNullException(result, "environment");
            }

            [Fact]
            public void Should_Throw_If_FileSystem_Is_Null()
            {
                // Given
                var environment = Substitute.For<ICakeEnvironment>();

                // When
                var result = Record.Exception(() => new GitHubActionsCommands(environment, null, null, null));

                // Then
                AssertEx.IsArgumentNullException(result, "fileSystem");
            }

            [Fact]
            public void Should_Throw_If_ActionsEnvironment_Is_Null()
            {
                // Given
                var environment = Substitute.For<ICakeEnvironment>();
                var filesystem = Substitute.For<IFileSystem>();

                // When
                var result = Record.Exception(() => new GitHubActionsCommands(environment, filesystem, null, null));

                // Then
                AssertEx.IsArgumentNullException(result, "actionsEnvironment");
            }

            [Fact]
            public void Should_Throw_If_CreateHttpClient_Is_Null()
            {
                // Given
                var environment = Substitute.For<ICakeEnvironment>();
                var filesystem = Substitute.For<IFileSystem>();
                var actionsEnvironment = new GitHubActionsInfoFixture().CreateEnvironmentInfo();

                // When
                var result = Record.Exception(() => new GitHubActionsCommands(environment, filesystem, actionsEnvironment, null));

                // Then
                AssertEx.IsArgumentNullException(result, "createHttpClient");
            }
        }

        public sealed class TheAddPathMethod
        {
            [Fact]
            public void Should_Throw_If_Path_Is_Null()
            {
                // Given
                var commands = new GitHubActionsCommandsFixture().CreateGitHubActionsCommands();

                // When
                var result = Record.Exception(() => commands.AddPath(null));

                // Then
                AssertEx.IsArgumentNullException(result, "path");
            }

            [Fact]
            public void Should_Throw_If_SystemPath_Is_Null()
            {
                // Given
                var commands = new GitHubActionsCommandsFixture()
                                .WithNoGitHubPath()
                                .CreateGitHubActionsCommands();

                var path = "/temp/dev/bin";

                // When
                var result = Record.Exception(() => commands.AddPath(path));

                // Then
                AssertEx.IsCakeException(result, "GitHub Actions Runtime SystemPath missing.");
            }

            [Fact]
            public void Should_AddPath()
            {
                // Given
                var gitHubActionsCommandsFixture = new GitHubActionsCommandsFixture();
                var commands = gitHubActionsCommandsFixture.CreateGitHubActionsCommands();
                var path = "/temp/dev/bin";

                // When
                commands.AddPath(path);

                // Then
                Assert.Equal(
                    (path + System.Environment.NewLine).NormalizeLineEndings(),
                    gitHubActionsCommandsFixture.FileSystem.GetFile("/opt/github.path").GetTextContent().NormalizeLineEndings());
            }
        }

        public sealed class TheSetEnvironmentVariableMethod
        {
            [Fact]
            public void Should_Throw_If_Key_Is_Null()
            {
                // Given
                var commands = new GitHubActionsCommandsFixture().CreateGitHubActionsCommands();

                // When
                var result = Record.Exception(() => commands.SetEnvironmentVariable(null, null));

                // Then
                AssertEx.IsArgumentNullException(result, "key");
            }

            [Fact]
            public void Should_Throw_If_Value_Is_Null()
            {
                // Given
                var commands = new GitHubActionsCommandsFixture().CreateGitHubActionsCommands();
                var key = "Key";

                // When
                var result = Record.Exception(() => commands.SetEnvironmentVariable(key, null));

                // Then
                AssertEx.IsArgumentNullException(result, "value");
            }

            [Fact]
            public void Should_Throw_If_EnvPath_Is_Null()
            {
                // Given
                var commands = new GitHubActionsCommandsFixture()
                                .WithNoGitHubEnv()
                                .CreateGitHubActionsCommands();

                var key = "Key";
                var value = "Value";

                // When
                var result = Record.Exception(() => commands.SetEnvironmentVariable(key, value));

                // Then
                AssertEx.IsCakeException(result, "GitHub Actions Runtime EnvPath missing.");
            }

            [Fact]
            public void Should_SetEnvironmentVariable()
            {
                // Given
                var gitHubActionsCommandsFixture = new GitHubActionsCommandsFixture();
                var commands = gitHubActionsCommandsFixture.CreateGitHubActionsCommands();
                var key = "Key";
                var value = "Value";

                // When
                commands.SetEnvironmentVariable(key, value);

                // Then
                Assert.Equal(
                    @"Key<<CAKEEOF
Value
CAKEEOF
".NormalizeLineEndings(),
                    gitHubActionsCommandsFixture.FileSystem.GetFile("/opt/github.env").GetTextContent().NormalizeLineEndings());
            }
        }

        public sealed class TheUploadArtifactMethod
        {
            public sealed class File
            {
                [Fact]
                public async Task Should_Throw_If_Path_Is_Null()
                {
                    // Given
                    var commands = new GitHubActionsCommandsFixture().CreateGitHubActionsCommands();
                    FilePath path = null;

                    // When
                    var result = await Record.ExceptionAsync(() => commands.UploadArtifact(path, null));

                    // Then
                    AssertEx.IsArgumentNullException(result, "path");
                }

                [Fact]
                public async Task Should_Throw_If_ArtifactName_Is_Null()
                {
                    // Given
                    var commands = new GitHubActionsCommandsFixture().CreateGitHubActionsCommands();
                    var path = FilePath.FromString("/artifacts/artifact.zip");

                    // When
                    var result = await Record.ExceptionAsync(() => commands.UploadArtifact(path, null));

                    // Then
                    AssertEx.IsArgumentNullException(result, "artifactName");
                }

                [Fact]
                public async Task Should_Throw_If_File_Missing()
                {
                    // Given
                    var commands = new GitHubActionsCommandsFixture().CreateGitHubActionsCommands();
                    var path = FilePath.FromString("/artifacts/artifact.zip");
                    var artifactName = "artifact";

                    // When
                    var result = await Record.ExceptionAsync(() => commands.UploadArtifact(path, artifactName));

                    // Then
                    AssertEx.IsExceptionWithMessage<FileNotFoundException>(result, "Artifact file not found.");
                }

                [Theory]
                [InlineData("/", "/artifacts/artifact.txt")]
                [InlineData("/artifacts", "artifact.txt")]
                public async Task Should_Upload(string workingDirectory, string testPath)
                {
                    // Given
                    var gitHubActionsCommandsFixture = new GitHubActionsCommandsFixture()
                                                        .WithWorkingDirectory(workingDirectory);
                    var testFilePath = FilePath.FromString(testPath);
                    var artifactName = "artifact";
                    gitHubActionsCommandsFixture
                        .FileSystem
                        .CreateFile("/artifacts/artifact.txt")
                        .SetContent(artifactName);
                    var commands = gitHubActionsCommandsFixture.CreateGitHubActionsCommands();

                    // When
                    await commands.UploadArtifact(testFilePath, artifactName);
                }
            }

            public sealed class Directory
            {
                [Fact]
                public async Task Should_Throw_If_Path_Is_Null()
                {
                    // Given
                    var commands = new GitHubActionsCommandsFixture().CreateGitHubActionsCommands();
                    DirectoryPath path = null;

                    // When
                    var result = await Record.ExceptionAsync(() => commands.UploadArtifact(path, null));

                    // Then
                    AssertEx.IsArgumentNullException(result, "path");
                }

                [Fact]
                public async Task Should_Throw_If_ArtifactName_Is_Null()
                {
                    // Given
                    var commands = new GitHubActionsCommandsFixture().CreateGitHubActionsCommands();
                    var path = DirectoryPath.FromString("/artifacts");

                    // When
                    var result = await Record.ExceptionAsync(() => commands.UploadArtifact(path, null));

                    // Then
                    AssertEx.IsArgumentNullException(result, "artifactName");
                }

                [Fact]
                public async Task Should_Throw_If_Directory_Missing()
                {
                    // Given
                    var commands = new GitHubActionsCommandsFixture().CreateGitHubActionsCommands();
                    var path = DirectoryPath.FromString("/artifacts");
                    var artifactName = "artifact";

                    // When
                    var result = await Record.ExceptionAsync(() => commands.UploadArtifact(path, artifactName));

                    // Then
                    AssertEx.IsExceptionWithMessage<DirectoryNotFoundException>(result, "Artifact directory /artifacts not found.");
                }

                [Theory]
                [InlineData("/", "/src/artifacts")]
                [InlineData("/src", "artifacts")]
                public async Task Should_Upload(string workingDirectory, string testPath)
                {
                    // Given
                    var gitHubActionsCommandsFixture = new GitHubActionsCommandsFixture()
                                                        .WithWorkingDirectory(workingDirectory);
                    var testDirectoryPath = DirectoryPath.FromString(testPath);
                    var artifactName = "artifacts";
                    var directory = DirectoryPath.FromString("/src/artifacts");

                    gitHubActionsCommandsFixture
                        .FileSystem
                        .CreateFile(directory.CombineWithFilePath("artifact.txt"))
                        .SetContent(artifactName);

                    gitHubActionsCommandsFixture
                        .FileSystem
                        .CreateFile(directory.Combine("folder_a").CombineWithFilePath("artifact.txt"))
                        .SetContent(artifactName);

                    gitHubActionsCommandsFixture
                        .FileSystem
                        .CreateFile(directory.Combine("folder_b").CombineWithFilePath("artifact.txt"))
                        .SetContent(artifactName);

                    gitHubActionsCommandsFixture
                        .FileSystem
                        .CreateFile(directory.Combine("folder_b").Combine("folder_c").CombineWithFilePath("artifact.txt"))
                        .SetContent(artifactName);

                    var commands = gitHubActionsCommandsFixture.CreateGitHubActionsCommands();

                    // When
                    await commands.UploadArtifact(testDirectoryPath, artifactName);
                }
            }
        }

        public sealed class TheDownloadArtifactMethod
        {
            [Fact]
            public async Task Should_Throw_If_ArtifactName_Is_Null()
            {
                // Given
                var commands = new GitHubActionsCommandsFixture().CreateGitHubActionsCommands();
                var path = DirectoryPath.FromString("/artifacts");

                // When
                var result = await Record.ExceptionAsync(() => commands.DownloadArtifact(null, path));

                // Then
                AssertEx.IsArgumentNullException(result, "artifactName");
            }

            [Fact]
            public async Task Should_Throw_If_Path_Is_Null()
            {
                // Given
                var commands = new GitHubActionsCommandsFixture().CreateGitHubActionsCommands();
                var artifactName = "artifactName";

                // When
                var result = await Record.ExceptionAsync(() => commands.DownloadArtifact(artifactName, null));

                // Then
                AssertEx.IsArgumentNullException(result, "path");
            }

            [Fact]
            public async Task Should_Throw_If_Directory_Missing()
            {
                // Given
                var commands = new GitHubActionsCommandsFixture().CreateGitHubActionsCommands();
                var path = DirectoryPath.FromString("/artifacts");
                var artifactName = "artifact";

                // When
                var result = await Record.ExceptionAsync(() => commands.DownloadArtifact(artifactName, path));

                // Then
                AssertEx.IsExceptionWithMessage<DirectoryNotFoundException>(result, "Local directory /artifacts not found.");
            }

            [Theory]
            [InlineData("/", "/src/artifacts")]
            [InlineData("/src", "artifacts")]
            public async Task Should_Download(string workingDirectory, string testPath)
            {
                // Given
                var gitHubActionsCommandsFixture = new GitHubActionsCommandsFixture()
                                                    .WithWorkingDirectory(workingDirectory);
                var testDirectoryPath = DirectoryPath.FromString(testPath);
                var artifactName = "artifact";
                var directory = DirectoryPath.FromString("/src/artifacts");
                var filePath = directory.CombineWithFilePath("test.txt");

                gitHubActionsCommandsFixture
                    .FileSystem
                    .CreateDirectory(directory);

                var commands = gitHubActionsCommandsFixture.CreateGitHubActionsCommands();

                // When
                await commands.DownloadArtifact(artifactName, testDirectoryPath);
                var file = gitHubActionsCommandsFixture
                    .FileSystem
                    .GetFile(filePath);

                // Then
                Assert.True(file.Exists, $"{filePath.FullPath} doesn't exist.");
                Assert.Equal("Cake", file.GetTextContent());
            }
        }
    }
}