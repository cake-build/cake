// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Cake.Common.Build;
using Cake.Common.Build.GitHubActions.Commands;
using Cake.Common.Build.GitHubActions.Commands.NuGet;
using Cake.Common.Build.GitHubActions.Data;
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
                var result = Record.Exception(() => new GitHubActionsCommands(null, null, null, null, null));

                // Then
                AssertEx.IsArgumentNullException(result, "environment");
            }

            [Fact]
            public void Should_Throw_If_FileSystem_Is_Null()
            {
                // Given
                var environment = Substitute.For<ICakeEnvironment>();

                // When
                var result = Record.Exception(() => new GitHubActionsCommands(environment, null, null, null, null));

                // Then
                AssertEx.IsArgumentNullException(result, "fileSystem");
            }

            [Fact]
            public void Should_Throw_If_Writer_Is_Null()
            {
                // Given
                var environment = Substitute.For<ICakeEnvironment>();
                var filesystem = Substitute.For<IFileSystem>();

                // When
                var result = Record.Exception(() => new GitHubActionsCommands(environment, filesystem, null, null, null));

                // Then
                AssertEx.IsArgumentNullException(result, "writer");
            }

            [Fact]
            public void Should_Throw_If_ActionsEnvironment_Is_Null()
            {
                // Given
                var environment = Substitute.For<ICakeEnvironment>();
                var filesystem = Substitute.For<IFileSystem>();
                var writer = Substitute.For<IBuildSystemServiceMessageWriter>();

                // When
                var result = Record.Exception(() => new GitHubActionsCommands(environment, filesystem, writer, null, null));

                // Then
                AssertEx.IsArgumentNullException(result, "actionsEnvironment");
            }

            [Fact]
            public void Should_Throw_If_CreateHttpClient_Is_Null()
            {
                // Given
                var environment = Substitute.For<ICakeEnvironment>();
                var filesystem = Substitute.For<IFileSystem>();
                var writer = Substitute.For<IBuildSystemServiceMessageWriter>();
                var actionsEnvironment = new GitHubActionsInfoFixture().CreateEnvironmentInfo();

                // When
                var result = Record.Exception(() => new GitHubActionsCommands(environment, filesystem, writer, actionsEnvironment, null));

                // Then
                AssertEx.IsArgumentNullException(result, "createHttpClient");
            }
        }

        public sealed class TheDebugMethod
        {
            [Fact]
            public void Should_WriteOutput()
            {
                // Given
                var fixture = new GitHubActionsCommandsFixture();
                var commands = fixture.CreateGitHubActionsCommands();

                // When
                commands.Debug("message");

                // Then
                var entry = Assert.Single(fixture.Writer.Entries);
                Assert.Equal("::debug::message", entry);
            }
        }

        public sealed class TheNoticeMethod
        {
            [Fact]
            public void Should_WriteOutput()
            {
                // Given
                var fixture = new GitHubActionsCommandsFixture();
                var commands = fixture.CreateGitHubActionsCommands();

                // When
                commands.Notice("message");

                // Then
                var entry = Assert.Single(fixture.Writer.Entries);
                Assert.Equal("::notice::message", entry);
            }

            [Fact]
            public void Should_WriteOutputWithAnnotation()
            {
                // Given
                var fixture = new GitHubActionsCommandsFixture();
                var commands = fixture.CreateGitHubActionsCommands();

                // When
                commands.Notice("message", new GitHubActionsAnnotation { Title = "title", File = "file", StartLine = 1, EndLine = 2, StartColumn = 10, EndColumn = 20 });

                // Then
                var entry = Assert.Single(fixture.Writer.Entries);
                Assert.Equal("::notice title=title,file=file,line=1,endLine=2,col=10,endColumn=20::message", entry);
            }
        }

        public sealed class TheWarningMethod
        {
            [Fact]
            public void Should_WriteOutput()
            {
                // Given
                var fixture = new GitHubActionsCommandsFixture();
                var commands = fixture.CreateGitHubActionsCommands();

                // When
                commands.Warning("message");

                // Then
                var entry = Assert.Single(fixture.Writer.Entries);
                Assert.Equal("::warning::message", entry);
            }

            [Fact]
            public void Should_WriteOutputWithAnnotation()
            {
                // Given
                var fixture = new GitHubActionsCommandsFixture();
                var commands = fixture.CreateGitHubActionsCommands();

                // When
                commands.Warning("message", new GitHubActionsAnnotation { Title = "title", File = "file", StartLine = 1, EndLine = 2, StartColumn = 10, EndColumn = 20 });

                // Then
                var entry = Assert.Single(fixture.Writer.Entries);
                Assert.Equal("::warning title=title,file=file,line=1,endLine=2,col=10,endColumn=20::message", entry);
            }
        }

        public sealed class TheErrorMethod
        {
            [Fact]
            public void Should_WriteOutput()
            {
                // Given
                var fixture = new GitHubActionsCommandsFixture();
                var commands = fixture.CreateGitHubActionsCommands();

                // When
                commands.Error("message");

                // Then
                var entry = Assert.Single(fixture.Writer.Entries);
                Assert.Equal("::error::message", entry);
            }

            [Fact]
            public void Should_WriteOutputWithAnnotation()
            {
                // Given
                var fixture = new GitHubActionsCommandsFixture();
                var commands = fixture.CreateGitHubActionsCommands();

                // When
                commands.Error("message", new GitHubActionsAnnotation { Title = "title", File = "file", StartLine = 1, EndLine = 2, StartColumn = 10, EndColumn = 20 });

                // Then
                var entry = Assert.Single(fixture.Writer.Entries);
                Assert.Equal("::error title=title,file=file,line=1,endLine=2,col=10,endColumn=20::message", entry);
            }
        }

        public sealed class TheStartGroupMethod
        {
            [Fact]
            public void Should_WriteOutput()
            {
                // Given
                var fixture = new GitHubActionsCommandsFixture();
                var commands = fixture.CreateGitHubActionsCommands();

                // When
                commands.StartGroup("title");

                // Then
                var entry = Assert.Single(fixture.Writer.Entries);
                Assert.Equal("::group::title", entry);
            }
        }

        public sealed class TheEndGroupMethod
        {
            [Fact]
            public void Should_WriteOutput()
            {
                // Given
                var fixture = new GitHubActionsCommandsFixture();
                var commands = fixture.CreateGitHubActionsCommands();

                // When
                commands.EndGroup();

                // Then
                var entry = Assert.Single(fixture.Writer.Entries);
                Assert.Equal("::endgroup::", entry);
            }
        }

        public sealed class TheSetSecretMethod
        {
            [Fact]
            public void Should_WriteOutput()
            {
                // Given
                var fixture = new GitHubActionsCommandsFixture();
                var commands = fixture.CreateGitHubActionsCommands();

                // When
                commands.SetSecret("secret");

                // Then
                var entry = Assert.Single(fixture.Writer.Entries);
                Assert.Equal("::add-mask::secret", entry);
            }
        }

        public sealed class TheWriteCommandMethod
        {
            [Fact]
            public void Should_EscapeCommandMessage()
            {
                // Given
                var fixture = new GitHubActionsCommandsFixture();
                var commands = fixture.CreateGitHubActionsCommands();

                // When
                commands.WriteCommand("test", "%\r\n");

                // Then
                var entry = Assert.Single(fixture.Writer.Entries);
                Assert.Equal("::test::%25%0D%0A", entry);
            }

            [Fact]
            public void Should_EscapeCommandParameter()
            {
                // Given
                var fixture = new GitHubActionsCommandsFixture();
                var commands = fixture.CreateGitHubActionsCommands();

                // When
                commands.WriteCommand("test", new Dictionary<string, string> { ["parameter"] = "%\r\n:," }, "message");

                // Then
                var entry = Assert.Single(fixture.Writer.Entries);
                Assert.Equal("::test parameter=%25%0D%0A%3A%2C::message", entry);
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
                var fixture = new GitHubActionsCommandsFixture();
                var commands = fixture.CreateGitHubActionsCommands();
                var path = "/temp/dev/bin";

                // When
                commands.AddPath(path);

                // Then
                Assert.Equal(
                    (path + System.Environment.NewLine).NormalizeLineEndings(),
                    fixture.FileSystem.GetFile("/opt/github.path").GetTextContent().NormalizeLineEndings());
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
                var fixture = new GitHubActionsCommandsFixture();
                var commands = fixture.CreateGitHubActionsCommands();
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
                    fixture.FileSystem.GetFile("/opt/github.env").GetTextContent().NormalizeLineEndings());
            }
        }

        public sealed class TheSetOutputParameterMethod
        {
            [Fact]
            public void Should_Throw_If_Key_Is_Null()
            {
                // Given
                var commands = new GitHubActionsCommandsFixture().CreateGitHubActionsCommands();

                // When
                var result = Record.Exception(() => commands.SetOutputParameter(null, null));

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
                var result = Record.Exception(() => commands.SetOutputParameter(key, null));

                // Then
                AssertEx.IsArgumentNullException(result, "value");
            }

            [Fact]
            public void Should_Throw_If_OutputPath_Is_Null()
            {
                // Given
                var commands = new GitHubActionsCommandsFixture()
                                .WithNoGitHubOutput()
                                .CreateGitHubActionsCommands();

                var key = "Key";
                var value = "Value";

                // When
                var result = Record.Exception(() => commands.SetOutputParameter(key, value));

                // Then
                AssertEx.IsCakeException(result, "GitHub Actions Runtime OutputPath missing.");
            }

            [Fact]
            public void Should_SetOutputParameter()
            {
                // Given
                var fixture = new GitHubActionsCommandsFixture();
                var commands = fixture.CreateGitHubActionsCommands();
                var key = "Key";
                var value = "Value";

                // When
                commands.SetOutputParameter(key, value);

                // Then
                Assert.Equal(
                    @"Key=Value
".NormalizeLineEndings(),
                    fixture.FileSystem.GetFile("/opt/github.output").GetTextContent().NormalizeLineEndings());
            }
        }

        public sealed class TheSetStepSummaryMethod
        {
            [Fact]
            public void Should_Throw_If_Summary_Is_Null()
            {
                // Given
                var commands = new GitHubActionsCommandsFixture().CreateGitHubActionsCommands();

                // When
                var result = Record.Exception(() => commands.SetStepSummary(null));

                // Then
                AssertEx.IsArgumentNullException(result, "summary");
            }

            [Fact]
            public void Should_Throw_If_StepSummary_Is_Null()
            {
                // Given
                var commands = new GitHubActionsCommandsFixture()
                                .WithNoGitHubStepSummary()
                                .CreateGitHubActionsCommands();

                var summary = "summary";

                // When
                var result = Record.Exception(() => commands.SetStepSummary(summary));

                // Then
                AssertEx.IsCakeException(result, "GitHub Actions Runtime StepSummary missing.");
            }

            [Fact]
            public void Should_SetStepSummary()
            {
                // Given
                var fixture = new GitHubActionsCommandsFixture();
                var commands = fixture.CreateGitHubActionsCommands();
                var summary = "## This is some markdown content :rocket:";

                // When
                commands.SetStepSummary(summary);

                // Then
                Assert.Equal(
                    string.Concat(summary, System.Environment.NewLine).NormalizeLineEndings(),
                    fixture.FileSystem.GetFile("/opt/github.stepsummary").GetTextContent().NormalizeLineEndings());
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
                    var fixture = new GitHubActionsCommandsFixture()
                                                        .WithWorkingDirectory(workingDirectory);
                    var testFilePath = FilePath.FromString(testPath);
                    var artifactName = "artifact";
                    fixture
                        .FileSystem
                        .CreateFile("/artifacts/artifact.txt")
                        .SetContent(artifactName);
                    var commands = fixture.CreateGitHubActionsCommands();

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
                    var fixture = new GitHubActionsCommandsFixture()
                                                        .WithWorkingDirectory(workingDirectory);
                    var testDirectoryPath = DirectoryPath.FromString(testPath);
                    var artifactName = "artifacts";
                    var directory = DirectoryPath.FromString("/src/artifacts");

                    fixture
                        .FileSystem
                        .CreateFile(directory.CombineWithFilePath("artifact.txt"))
                        .SetContent(artifactName);

                    fixture
                        .FileSystem
                        .CreateFile(directory.Combine("folder_a").CombineWithFilePath("artifact.txt"))
                        .SetContent(artifactName);

                    fixture
                        .FileSystem
                        .CreateFile(directory.Combine("folder_b").CombineWithFilePath("artifact.txt"))
                        .SetContent(artifactName);

                    fixture
                        .FileSystem
                        .CreateFile(directory.Combine("folder_b").Combine("folder_c").CombineWithFilePath("artifact.txt"))
                        .SetContent(artifactName);

                    var commands = fixture.CreateGitHubActionsCommands();

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
                var fixture = new GitHubActionsCommandsFixture()
                                                    .WithWorkingDirectory(workingDirectory);
                var testDirectoryPath = DirectoryPath.FromString(testPath);
                var artifactName = "artifact";
                var directory = DirectoryPath.FromString("/src/artifacts");
                var filePath = directory.CombineWithFilePath("test.txt");

                fixture
                    .FileSystem
                    .CreateDirectory(directory);

                var commands = fixture.CreateGitHubActionsCommands();

                // When
                await commands.DownloadArtifact(artifactName, testDirectoryPath);
                var file = fixture
                    .FileSystem
                    .GetFile(filePath);

                // Then
                Assert.True(file.Exists, $"{filePath.FullPath} doesn't exist.");
                Assert.Equal("Cake", file.GetTextContent());
            }
        }

        /// <summary>
        /// Tests for <see cref="GitHubActionsCommands.NuGetLogin(string, System.Threading.CancellationToken)"/> and
        /// <see cref="GitHubActionsCommands.NuGetLogin(GitHubNuGetLoginSettings, System.Threading.CancellationToken)"/>.
        /// </summary>
        public sealed class TheNuGetLoginMethod
        {
            [Fact]
            public async Task Should_Return_Api_Key_When_Login_With_UserName_Only()
            {
                // Given
                var fixture = new GitHubNuGetLoginCommandsFixture();
                var commands = fixture.CreateGitHubActionsCommands();

                // When
                var apiKey = await commands.NuGetLogin("testuser", TestContext.Current.CancellationToken);

                // Then
                Assert.Equal("expected-api-key-default-url", apiKey);
            }

            [Fact]
            public async Task Should_Mask_Sensitive_Values()
            {
                // Given
                var fixture = new GitHubNuGetLoginCommandsFixture();
                var commands = fixture.CreateGitHubActionsCommands();

                // When
                var apiKey = await commands.NuGetLogin("testuser", TestContext.Current.CancellationToken);

                // Then
                Assert.Equal("expected-api-key-default-url", apiKey);
                Assert.Equal(3, fixture.Writer.Entries.Count);
                Assert.Contains($"::add-mask::{GitHubNuGetLoginCommandsFixture.ActionsIdTokenRequestToken}", fixture.Writer.Entries);
                Assert.Contains("::add-mask::mock-oidc-jwt", fixture.Writer.Entries);
                Assert.Contains("::add-mask::expected-api-key-default-url", fixture.Writer.Entries);
            }

            [Fact]
            public async Task Should_Return_Api_Key_When_Settings_Use_Custom_Token_Service_Url()
            {
                // Given
                var fixture = new GitHubNuGetLoginCommandsFixture();
                var commands = fixture.CreateGitHubActionsCommands();
                var settings = new GitHubNuGetLoginSettings(
                    "testuser",
                    GitHubNuGetLoginCommandsFixture.TokenServiceUrl,
                    GitHubNuGetLoginCommandsFixture.DefaultAudience);

                // When
                var apiKey = await commands.NuGetLogin(settings, TestContext.Current.CancellationToken);

                // Then
                Assert.Equal("expected-api-key", apiKey);
            }

            [Fact]
            public async Task Should_Throw_If_Settings_Is_Null()
            {
                // Given
                var commands = new GitHubNuGetLoginCommandsFixture().CreateGitHubActionsCommands();
                GitHubNuGetLoginSettings settings = null;

                // When
                var result = await Record.ExceptionAsync(() => commands.NuGetLogin(settings, TestContext.Current.CancellationToken));

                // Then
                AssertEx.IsArgumentNullException(result, "settings");
            }

            [Fact]
            public async Task Should_Throw_If_UserName_Is_Null()
            {
                // Given
                var commands = new GitHubNuGetLoginCommandsFixture().CreateGitHubActionsCommands();

                // When
                var result = await Record.ExceptionAsync(() => commands.NuGetLogin((string)null, TestContext.Current.CancellationToken));

                // Then
                AssertEx.IsArgumentNullException(result, "userName");
            }

            [Fact]
            public async Task Should_Throw_If_UserName_Is_Whitespace()
            {
                // Given
                var commands = new GitHubNuGetLoginCommandsFixture().CreateGitHubActionsCommands();

                // When
                var result = await Record.ExceptionAsync(() => commands.NuGetLogin("  ", TestContext.Current.CancellationToken));

                // Then
                Assert.IsType<ArgumentException>(result);
                Assert.Equal("userName", ((ArgumentException)result).ParamName);
            }

            [Fact]
            public async Task Should_Throw_If_Settings_UserName_Is_Whitespace()
            {
                // Given
                var commands = new GitHubNuGetLoginCommandsFixture().CreateGitHubActionsCommands();
                var settings = new GitHubNuGetLoginSettings("  ");

                // When
                var result = await Record.ExceptionAsync(() => commands.NuGetLogin(settings, TestContext.Current.CancellationToken));

                // Then
                Assert.IsType<ArgumentException>(result);
                Assert.Equal("userName", ((ArgumentException)result).ParamName);
            }

            [Fact]
            public async Task Should_Throw_If_Token_Service_Url_Is_Null()
            {
                // Given
                var commands = new GitHubNuGetLoginCommandsFixture().CreateGitHubActionsCommands();
                var settings = new GitHubNuGetLoginSettings("user", null);

                // When
                var result = await Record.ExceptionAsync(() => commands.NuGetLogin(settings, TestContext.Current.CancellationToken));

                // Then
                AssertEx.IsArgumentNullException(result, "tokenServiceUrl");
            }

            [Fact]
            public async Task Should_Throw_If_Token_Service_Url_Is_Whitespace()
            {
                // Given
                var commands = new GitHubNuGetLoginCommandsFixture().CreateGitHubActionsCommands();
                var settings = new GitHubNuGetLoginSettings("user", "  ");

                // When
                var result = await Record.ExceptionAsync(() => commands.NuGetLogin(settings, TestContext.Current.CancellationToken));

                // Then
                Assert.IsType<ArgumentException>(result);
                Assert.Equal("tokenServiceUrl", ((ArgumentException)result).ParamName);
            }

            [Fact]
            public async Task Should_Throw_If_Audience_Is_Null()
            {
                // Given
                var commands = new GitHubNuGetLoginCommandsFixture().CreateGitHubActionsCommands();
                var settings = new GitHubNuGetLoginSettings("user", GitHubNuGetLoginCommandsFixture.DefaultTokenServiceUrl, null);

                // When
                var result = await Record.ExceptionAsync(() => commands.NuGetLogin(settings, TestContext.Current.CancellationToken));

                // Then
                AssertEx.IsArgumentNullException(result, "audience");
            }

            [Fact]
            public async Task Should_Throw_If_Audience_Is_Whitespace()
            {
                // Given
                var commands = new GitHubNuGetLoginCommandsFixture().CreateGitHubActionsCommands();
                var settings = new GitHubNuGetLoginSettings("user", GitHubNuGetLoginCommandsFixture.DefaultTokenServiceUrl, "  ");

                // When
                var result = await Record.ExceptionAsync(() => commands.NuGetLogin(settings, TestContext.Current.CancellationToken));

                // Then
                Assert.IsType<ArgumentException>(result);
                Assert.Equal("audience", ((ArgumentException)result).ParamName);
            }

            [Fact]
            public async Task Should_Throw_If_Missing_Oidc_Request_Token()
            {
                // Given
                var fixture = new GitHubNuGetLoginCommandsFixture();
                fixture.Environment.GetEnvironmentVariable("ACTIONS_ID_TOKEN_REQUEST_TOKEN").Returns((string)null);
                var commands = fixture.CreateGitHubActionsCommands();

                // When
                var result = await Record.ExceptionAsync(() => commands.NuGetLogin("user", TestContext.Current.CancellationToken));

                // Then
                AssertEx.IsCakeException(result, "Missing GitHub OIDC request environment variables.");
            }

            [Fact]
            public async Task Should_Throw_If_Missing_Oidc_Request_Url()
            {
                // Given
                var fixture = new GitHubNuGetLoginCommandsFixture();
                fixture.Environment.GetEnvironmentVariable("ACTIONS_ID_TOKEN_REQUEST_URL").Returns((string)null);
                var commands = fixture.CreateGitHubActionsCommands();

                // When
                var result = await Record.ExceptionAsync(() => commands.NuGetLogin("user", TestContext.Current.CancellationToken));

                // Then
                AssertEx.IsCakeException(result, "Missing GitHub OIDC request environment variables.");
            }

            [Fact]
            public async Task Should_Throw_If_Oidc_Request_Fails()
            {
                // Given
                var fixture = new BrokenOidcGitHubNuGetLoginFixture();
                var commands = fixture.CreateGitHubActionsCommands();

                // When
                var result = await Record.ExceptionAsync(() => commands.NuGetLogin("user", TestContext.Current.CancellationToken));

                // Then
                AssertEx.IsCakeException(result, "Failed to retrieve OIDC token from GitHub (401 Unauthorized): oidc-failed");
            }

            [Fact]
            public async Task Should_Throw_If_Token_Exchange_Fails_With_Json_Error()
            {
                // Given
                var fixture = new FailingTokenExchangeGitHubNuGetLoginFixture();
                var commands = fixture.CreateGitHubActionsCommands();
                var settings = new GitHubNuGetLoginSettings(
                    "user",
                    GitHubNuGetLoginCommandsFixture.TokenServiceUrl,
                    GitHubNuGetLoginCommandsFixture.DefaultAudience);

                // When
                var result = await Record.ExceptionAsync(() => commands.NuGetLogin(settings, TestContext.Current.CancellationToken));

                // Then
                AssertEx.IsCakeException(result, "Token exchange failed (422 UnprocessableEntity): invalid_grant");
            }

            private sealed class BrokenOidcGitHubNuGetLoginFixture : GitHubNuGetLoginCommandsFixture
            {
                protected override Task<HttpResponseMessage> HandleAsync(HttpRequestMessage request, CancellationToken cancellationToken)
                {
                    if (request.Method == HttpMethod.Get && request.RequestUri?.AbsoluteUri == ExpectedOidcGetUriDefaultAudience)
                    {
                        return Task.FromResult(new HttpResponseMessage(HttpStatusCode.Unauthorized)
                        {
                            Content = new StringContent("oidc-failed", Encoding.UTF8, "text/plain")
                        });
                    }

                    return base.HandleAsync(request, cancellationToken);
                }
            }

            private sealed class FailingTokenExchangeGitHubNuGetLoginFixture : GitHubNuGetLoginCommandsFixture
            {
                protected override Task<HttpResponseMessage> HandleAsync(HttpRequestMessage request, CancellationToken cancellationToken)
                {
                    if (request.Method == HttpMethod.Post && request.RequestUri?.AbsoluteUri == TokenServiceUrl)
                    {
                        return Task.FromResult(new HttpResponseMessage(HttpStatusCode.UnprocessableEntity)
                        {
                            Content = new StringContent("""{"error":"invalid_grant"}""", Encoding.UTF8, "application/json")
                        });
                    }

                    return base.HandleAsync(request, cancellationToken);
                }
            }
        }
    }
}
