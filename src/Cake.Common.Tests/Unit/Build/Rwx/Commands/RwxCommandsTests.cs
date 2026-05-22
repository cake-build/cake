// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.IO;
using Cake.Common.Tests.Fixtures.Build;
using Cake.Core;
using Cake.Core.IO;
using Cake.Testing;
using Xunit;

namespace Cake.Common.Tests.Unit.Build.Rwx.Commands
{
    public sealed class RwxCommandsTests
    {
        public sealed class TheSetValueMethod
        {
            [Fact]
            public void Should_Write_Value_File()
            {
                // Given
                var fixture = new RwxCommandsFixture();
                var commands = fixture.CreateRwxCommands();

                // When
                commands.SetValue("hello", "world");

                // Then
                var file = fixture.FileSystem.GetFile("/rwx/values/hello");
                Assert.True(file.Exists);
                Assert.Equal("world", file.GetTextContent());
            }

            [Fact]
            public void Should_Overwrite_Existing_Value()
            {
                // Given
                var fixture = new RwxCommandsFixture();
                var commands = fixture.CreateRwxCommands();
                commands.SetValue("hello", "first");

                // When
                commands.SetValue("hello", "second");

                // Then
                Assert.Equal("second", fixture.FileSystem.GetFile("/rwx/values/hello").GetTextContent());
            }

            [Fact]
            public void Should_Preserve_Multiline_Content_Verbatim()
            {
                // Given
                var fixture = new RwxCommandsFixture();
                var commands = fixture.CreateRwxCommands();
                const string value = "line one\nline two\nline three";

                // When
                commands.SetValue("multi", value);

                // Then
                Assert.Equal(value, fixture.FileSystem.GetFile("/rwx/values/multi").GetTextContent());
            }

            [Fact]
            public void Should_Create_Values_Directory_When_Missing()
            {
                // Given
                var fixture = new RwxCommandsFixture();
                fixture.FileSystem.GetDirectory("/rwx/values").Delete(true);
                var commands = fixture.CreateRwxCommands();

                // When
                commands.SetValue("hello", "world");

                // Then
                Assert.True(fixture.FileSystem.GetFile("/rwx/values/hello").Exists);
            }

            [Fact]
            public void Should_Throw_On_Null_Key()
            {
                // Given
                var commands = new RwxCommandsFixture().CreateRwxCommands();

                // When
                var result = Record.Exception(() => commands.SetValue(null, "value"));

                // Then
                AssertEx.IsArgumentNullException(result, "key");
            }

            [Fact]
            public void Should_Throw_On_Empty_Key()
            {
                // Given
                var commands = new RwxCommandsFixture().CreateRwxCommands();

                // When
                var result = Record.Exception(() => commands.SetValue(string.Empty, "value"));

                // Then
                AssertEx.IsArgumentNullException(result, "key");
            }

            [Fact]
            public void Should_Throw_On_Null_Value()
            {
                // Given
                var commands = new RwxCommandsFixture().CreateRwxCommands();

                // When
                var result = Record.Exception(() => commands.SetValue("key", null));

                // Then
                AssertEx.IsArgumentNullException(result, "value");
            }

            [Theory]
            [InlineData("foo/bar")]
            [InlineData("foo\\bar")]
            [InlineData("..")]
            [InlineData("../escape")]
            public void Should_Throw_On_Key_With_Path_Components(string key)
            {
                // Given
                var commands = new RwxCommandsFixture().CreateRwxCommands();

                // When
                var result = Record.Exception(() => commands.SetValue(key, "value"));

                // Then
                Assert.IsType<CakeException>(result);
            }

            [Fact]
            public void Should_Throw_When_Values_Path_Missing()
            {
                // Given
                var fixture = new RwxCommandsFixture().WithNoRwxValues();
                var commands = fixture.CreateRwxCommands();

                // When
                var result = Record.Exception(() => commands.SetValue("key", "value"));

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Contains("ValuesPath", result.Message);
            }
        }

        public sealed class TheSetEnvironmentVariableMethod
        {
            [Fact]
            public void Should_Write_Env_File()
            {
                // Given
                var fixture = new RwxCommandsFixture();
                var commands = fixture.CreateRwxCommands();

                // When
                commands.SetEnvironmentVariable("FOO", "bar");

                // Then
                var file = fixture.FileSystem.GetFile("/rwx/env/FOO");
                Assert.True(file.Exists);
                Assert.Equal("bar", file.GetTextContent());
            }

            [Fact]
            public void Should_Overwrite_Existing_Env_Var()
            {
                // Given
                var fixture = new RwxCommandsFixture();
                var commands = fixture.CreateRwxCommands();
                commands.SetEnvironmentVariable("FOO", "first");

                // When
                commands.SetEnvironmentVariable("FOO", "second");

                // Then
                Assert.Equal("second", fixture.FileSystem.GetFile("/rwx/env/FOO").GetTextContent());
            }

            [Fact]
            public void Should_Write_Value_Verbatim_Without_Trailing_Newline()
            {
                // Given
                var fixture = new RwxCommandsFixture();
                var commands = fixture.CreateRwxCommands();

                // When
                commands.SetEnvironmentVariable("FOO", "bar");

                // Then
                // RWX trims a single trailing \n when materializing the env var; writing verbatim
                // lets callers decide whether they want one, and `bar` stays `bar`.
                Assert.Equal("bar", fixture.FileSystem.GetFile("/rwx/env/FOO").GetTextContent());
            }

            [Fact]
            public void Should_Create_Env_Directory_When_Missing()
            {
                // Given
                var fixture = new RwxCommandsFixture();
                fixture.FileSystem.GetDirectory("/rwx/env").Delete(true);
                var commands = fixture.CreateRwxCommands();

                // When
                commands.SetEnvironmentVariable("FOO", "bar");

                // Then
                Assert.True(fixture.FileSystem.GetFile("/rwx/env/FOO").Exists);
            }

            [Fact]
            public void Should_Throw_On_Null_Name()
            {
                // Given
                var commands = new RwxCommandsFixture().CreateRwxCommands();

                // When
                var result = Record.Exception(() => commands.SetEnvironmentVariable(null, "value"));

                // Then
                AssertEx.IsArgumentNullException(result, "name");
            }

            [Fact]
            public void Should_Throw_On_Empty_Name()
            {
                // Given
                var commands = new RwxCommandsFixture().CreateRwxCommands();

                // When
                var result = Record.Exception(() => commands.SetEnvironmentVariable(string.Empty, "value"));

                // Then
                AssertEx.IsArgumentNullException(result, "name");
            }

            [Fact]
            public void Should_Throw_On_Null_Value()
            {
                // Given
                var commands = new RwxCommandsFixture().CreateRwxCommands();

                // When
                var result = Record.Exception(() => commands.SetEnvironmentVariable("FOO", null));

                // Then
                AssertEx.IsArgumentNullException(result, "value");
            }

            [Theory]
            [InlineData("foo/bar")]
            [InlineData("foo\\bar")]
            [InlineData("..")]
            [InlineData("../escape")]
            public void Should_Throw_On_Name_With_Path_Components(string name)
            {
                // Given
                var commands = new RwxCommandsFixture().CreateRwxCommands();

                // When
                var result = Record.Exception(() => commands.SetEnvironmentVariable(name, "value"));

                // Then
                Assert.IsType<CakeException>(result);
            }

            [Fact]
            public void Should_Throw_When_Env_Path_Missing()
            {
                // Given
                var fixture = new RwxCommandsFixture().WithNoRwxEnv();
                var commands = fixture.CreateRwxCommands();

                // When
                var result = Record.Exception(() => commands.SetEnvironmentVariable("FOO", "bar"));

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Contains("EnvPath", result.Message);
            }
        }

        public sealed class TheUploadArtifactFileMethod
        {
            [Fact]
            public void Should_Copy_File_To_Artifacts_Directory()
            {
                // Given
                var fixture = new RwxCommandsFixture();
                fixture.FileSystem.CreateFile("/work/output/report.txt").SetContent("payload");
                var commands = fixture.CreateRwxCommands();

                // When
                commands.UploadArtifact("/work/output/report.txt");

                // Then
                var artifact = fixture.FileSystem.GetFile("/rwx/artifacts/report.txt");
                Assert.True(artifact.Exists);
                Assert.Equal("payload", artifact.GetTextContent());
            }

            [Fact]
            public void Should_Resolve_Relative_Paths_Against_Working_Directory()
            {
                // Given
                var fixture = new RwxCommandsFixture();
                fixture.FileSystem.CreateFile("/work/report.txt").SetContent("payload");
                var commands = fixture.CreateRwxCommands();

                // When
                commands.UploadArtifact("report.txt");

                // Then
                Assert.True(fixture.FileSystem.GetFile("/rwx/artifacts/report.txt").Exists);
            }

            [Fact]
            public void Should_Overwrite_Existing_Artifact()
            {
                // Given
                var fixture = new RwxCommandsFixture();
                fixture.FileSystem.CreateFile("/work/report.txt").SetContent("new");
                fixture.FileSystem.CreateFile("/rwx/artifacts/report.txt").SetContent("old");
                var commands = fixture.CreateRwxCommands();

                // When
                commands.UploadArtifact("/work/report.txt");

                // Then
                Assert.Equal("new", fixture.FileSystem.GetFile("/rwx/artifacts/report.txt").GetTextContent());
            }

            [Fact]
            public void Should_Create_Artifacts_Directory_When_Missing()
            {
                // Given
                var fixture = new RwxCommandsFixture();
                fixture.FileSystem.GetDirectory("/rwx/artifacts").Delete(true);
                fixture.FileSystem.CreateFile("/work/report.txt").SetContent("payload");
                var commands = fixture.CreateRwxCommands();

                // When
                commands.UploadArtifact("/work/report.txt");

                // Then
                Assert.True(fixture.FileSystem.GetFile("/rwx/artifacts/report.txt").Exists);
            }

            [Fact]
            public void Should_Throw_On_Null_Path()
            {
                // Given
                var commands = new RwxCommandsFixture().CreateRwxCommands();

                // When
                var result = Record.Exception(() => commands.UploadArtifact((FilePath)null));

                // Then
                AssertEx.IsArgumentNullException(result, "path");
            }

            [Fact]
            public void Should_Throw_When_Source_File_Missing()
            {
                // Given
                var commands = new RwxCommandsFixture().CreateRwxCommands();

                // When
                var result = Record.Exception(() => commands.UploadArtifact("/work/does-not-exist.txt"));

                // Then
                Assert.IsType<FileNotFoundException>(result);
            }

            [Fact]
            public void Should_Throw_When_Artifacts_Path_Missing()
            {
                // Given
                var fixture = new RwxCommandsFixture().WithNoRwxArtifacts();
                fixture.FileSystem.CreateFile("/work/report.txt").SetContent("payload");
                var commands = fixture.CreateRwxCommands();

                // When
                var result = Record.Exception(() => commands.UploadArtifact("/work/report.txt"));

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Contains("ArtifactsPath", result.Message);
            }
        }
    }
}
