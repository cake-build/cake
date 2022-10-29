// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Build.GitLabCI;
using Cake.Common.Tests.Fixtures.Build;
using Cake.Core;
using Cake.Testing;
using Xunit;

namespace Cake.Common.Tests.Unit.Build.GitLabCI
{
    public sealed class GitLabCICommandsTests
    {
        public sealed class TheConstructor
        {
            [Fact]
            public void Should_Throw_If_FileSystem_Is_Null()
            {
                // When
                var result = Record.Exception(() => new GitLabCICommands(null));

                // Then
                AssertEx.IsArgumentNullException(result, "fileSystem");
            }
        }

        public sealed class TheSetEnvironmentVariableMethod
        {
            [Fact]
            public void Should_Throw_If_EnvPath_Is_Null()
            {
                // Given
                var commands = new GitLabCIFixture().CreateGitLabCIService().Commands;

                // When
                var result = Record.Exception(() => commands.SetEnvironmentVariable(null, null, null));

                // Then
                AssertEx.IsArgumentNullException(result, "envPath");
            }

            [Fact]
            public void Should_Throw_If_Key_Is_Null()
            {
                // Given
                var commands = new GitLabCIFixture().CreateGitLabCIService().Commands;
                var envPath = "cake.env";

                // When
                var result = Record.Exception(() => commands.SetEnvironmentVariable(envPath, null, null));

                // Then
                AssertEx.IsArgumentNullException(result, "key");
            }

            [Fact]
            public void Should_Throw_If_Value_Is_Null()
            {
                // Given
                var commands = new GitLabCIFixture().CreateGitLabCIService().Commands;
                var envPath = "cake.env";
                var key = "Key";

                // When
                var result = Record.Exception(() => commands.SetEnvironmentVariable(envPath, key, null));

                // Then
                AssertEx.IsArgumentNullException(result, "value");
            }

            [Fact]
            public void Should_SetEnvironmentVariable()
            {
                // Given
                var gitLabCIFixture = new GitLabCIFixture();
                var commands = gitLabCIFixture.CreateGitLabCIService().Commands;
                var envPath = "cake.env";
                var key = "Key";
                var value = "Value";

                // When
                commands.SetEnvironmentVariable(envPath, key, value);

                // Then
                Assert.Equal(
                    @"Key=Value
".NormalizeLineEndings(),
                    gitLabCIFixture.FileSystem.GetFile("cake.env").GetTextContent().NormalizeLineEndings());
            }
        }
    }
}