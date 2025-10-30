// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tests.Fixtures.Build;
using Cake.Testing;
using Xunit;

namespace Cake.Common.Tests.Unit.Build.WoodpeckerCI.Commands
{
    public sealed class WoodpeckerCICommandsTests
    {
        public sealed class TheSetEnvironmentVariableMethod
        {
            [Fact]
            public void Should_Throw_If_Name_Is_Null()
            {
                // Given
                var commands = new WoodpeckerCICommandsFixture().CreateWoodpeckerCICommands();

                // When
                var result = Record.Exception(() => commands.SetEnvironmentVariable(null, "value"));

                // Then
                AssertEx.IsArgumentException(result, "name", "Environment variable name cannot be null or empty.");
            }

            [Fact]
            public void Should_Throw_If_Name_Is_Empty()
            {
                // Given
                var commands = new WoodpeckerCICommandsFixture().CreateWoodpeckerCICommands();

                // When
                var result = Record.Exception(() => commands.SetEnvironmentVariable(string.Empty, "value"));

                // Then
                AssertEx.IsArgumentException(result, "name", "Environment variable name cannot be null or empty.");
            }

            [Fact]
            public void Should_Throw_If_Name_Is_WhiteSpace()
            {
                // Given
                var commands = new WoodpeckerCICommandsFixture().CreateWoodpeckerCICommands();

                // When
                var result = Record.Exception(() => commands.SetEnvironmentVariable("   ", "value"));

                // Then
                AssertEx.IsArgumentException(result, "name", "Environment variable name cannot be null or empty.");
            }

            [Fact]
            public void Should_SetEnvironmentVariable()
            {
                // Given
                var fixture = new WoodpeckerCICommandsFixture();
                var commands = fixture.CreateWoodpeckerCICommands();
                var name = "MY_VAR";
                var value = "my_value";

                // When
                commands.SetEnvironmentVariable(name, value);

                // Then
                Assert.Equal(
                    "MY_VAR=my_value\n",
                    ((FakeFile)fixture.FileSystem.GetFile("/woodpecker/src/git.example.com/john-doe/my-repo/.woodpecker/env")).GetTextContent());
            }

            [Fact]
            public void Should_Append_To_Existing_Environment_File()
            {
                // Given
                var fixture = new WoodpeckerCICommandsFixture();
                var commands = fixture.CreateWoodpeckerCICommands();
                var envFile = (FakeFile)fixture.FileSystem.GetFile("/woodpecker/src/git.example.com/john-doe/my-repo/.woodpecker/env");
                envFile.SetContent("EXISTING_VAR=existing_value\n");

                // When
                commands.SetEnvironmentVariable("NEW_VAR", "new_value");

                // Then
                Assert.Equal(
                    "EXISTING_VAR=existing_value\nNEW_VAR=new_value\n",
                    envFile.GetTextContent());
            }
        }

        public sealed class TheGetEnvironmentVariableMethod
        {
            [Fact]
            public void Should_Throw_If_Name_Is_Null()
            {
                // Given
                var commands = new WoodpeckerCICommandsFixture().CreateWoodpeckerCICommands();

                // When
                var result = Record.Exception(() => commands.GetEnvironmentVariable(null));

                // Then
                AssertEx.IsArgumentException(result, "name", "Environment variable name cannot be null or empty.");
            }

            [Fact]
            public void Should_Throw_If_Name_Is_Empty()
            {
                // Given
                var commands = new WoodpeckerCICommandsFixture().CreateWoodpeckerCICommands();

                // When
                var result = Record.Exception(() => commands.GetEnvironmentVariable(string.Empty));

                // Then
                AssertEx.IsArgumentException(result, "name", "Environment variable name cannot be null or empty.");
            }

            [Fact]
            public void Should_Throw_If_Name_Is_WhiteSpace()
            {
                // Given
                var commands = new WoodpeckerCICommandsFixture().CreateWoodpeckerCICommands();

                // When
                var result = Record.Exception(() => commands.GetEnvironmentVariable("   "));

                // Then
                AssertEx.IsArgumentException(result, "name", "Environment variable name cannot be null or empty.");
            }

            [Fact]
            public void Should_Return_Environment_Variable_Value()
            {
                // Given
                var fixture = new WoodpeckerCICommandsFixture();
                ((FakeEnvironment)fixture.Environment).SetEnvironmentVariable("TEST_VAR", "test_value");
                var commands = fixture.CreateWoodpeckerCICommands();

                // When
                var result = commands.GetEnvironmentVariable("TEST_VAR");

                // Then
                Assert.Equal("test_value", result);
            }

            [Fact]
            public void Should_Return_Null_If_Environment_Variable_Not_Found()
            {
                // Given
                var commands = new WoodpeckerCICommandsFixture().CreateWoodpeckerCICommands();

                // When
                var result = commands.GetEnvironmentVariable("NONEXISTENT_VAR");

                // Then
                Assert.Null(result);
            }
        }
    }
}
