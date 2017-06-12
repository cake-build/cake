// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Commands;
using Cake.Tests.Fixtures;
using NSubstitute;
using Xunit;

namespace Cake.Tests.Unit
{
    public sealed class CakeApplicationTests
    {
        public sealed class TheConstructor
        {
            [Fact]
            public void Should_Throw_If_Command_Factory_Is_Null()
            {
                // Given
                var fixture = new CakeApplicationFixture();
                fixture.CommandFactory = null;

                // When
                var result = Record.Exception(() => fixture.CreateApplication());

                // Then
                AssertEx.IsArgumentNullException(result, "commandFactory");
            }
        }

        public sealed class TheRunMethod
        {
            [Fact]
            public void Should_Throw_If_Options_Are_Null()
            {
                // Given
                var fixture = new CakeApplicationFixture();
                fixture.Options = null;

                // When
                var result = Record.Exception(() => fixture.RunApplication());

                // Then
                AssertEx.IsArgumentNullException(result, "options");
            }

            [Fact]
            public void Should_Return_Success_If_No_Exception_Was_Thrown()
            {
                // Given
                var fixture = new CakeApplicationFixture();
                var command = Substitute.For<ICommand>();
                command.Execute(fixture.Options).Returns(true);
                fixture.Options.Script = "./build.cake";
                fixture.CommandFactory.CreateBuildCommand().Returns(command);

                // When
                var result = fixture.RunApplication();

                // Then
                Assert.Equal(0, result);
            }

            [Fact]
            public void Should_Create_Help_Command_If_Specified_In_Options()
            {
                // Given
                var fixture = new CakeApplicationFixture();
                fixture.Options.ShowHelp = true;

                // When
                fixture.RunApplication();

                // Then
                fixture.CommandFactory.Received(1).CreateHelpCommand();
            }

            [Fact]
            public void Should_Create_Version_Command_If_Specified_In_Options()
            {
                // Given
                var fixture = new CakeApplicationFixture();
                fixture.Options.ShowVersion = true;

                // When
                fixture.RunApplication();

                // Then
                fixture.CommandFactory.Received(1).CreateVersionCommand();
            }

            [Fact]
            public void Should_Create_Description_Command_If_Specified_In_Options()
            {
                // Given
                var fixture = new CakeApplicationFixture();
                fixture.Options.ShowDescription = true;
                fixture.Options.Script = "./build.cake";

                // When
                fixture.RunApplication();

                // Then
                fixture.CommandFactory.Received(1).CreateDescriptionCommand();
            }

            [Fact]
            public void Should_Create_Build_Command_If_Options_Contain_Script()
            {
                // Given
                var fixture = new CakeApplicationFixture();
                fixture.Options.Script = "./build.cake";

                // When
                fixture.RunApplication();

                // Then
                fixture.CommandFactory.Received(1).CreateBuildCommand();
            }

            [Fact]
            public void Should_Create_Help_Command_Even_If_Script_Is_Specified()
            {
                // Given
                var fixture = new CakeApplicationFixture();
                fixture.Options.Script = "./build.cake";
                fixture.Options.ShowHelp = true;

                // When
                fixture.RunApplication();

                // Then
                fixture.CommandFactory.Received(0).CreateBuildCommand();
                fixture.CommandFactory.Received(1).CreateHelpCommand();
            }

            [Fact]
            public void Should_Create_Version_Command_Even_If_Script_Is_Specified()
            {
                // Given
                var fixture = new CakeApplicationFixture();
                fixture.Options.Script = "./build.cake";
                fixture.Options.ShowVersion = true;

                // When
                fixture.RunApplication();

                // Then
                fixture.CommandFactory.Received(0).CreateBuildCommand();
                fixture.CommandFactory.Received(1).CreateVersionCommand();
            }

            [Fact]
            public void Should_Return_Error_If_No_Parameters_Are_Set()
            {
                // Given
                var fixture = new CakeApplicationFixture();
                fixture.Options = new CakeOptions();

                // When
                var result = fixture.RunApplication();

                // Then
                Assert.Equal(1, result);
            }

            [Fact]
            public void Should_Create_Dry_Run_Command_If_Specified_In_Options()
            {
                // Given
                var fixture = new CakeApplicationFixture();
                fixture.Options.PerformDryRun = true;
                fixture.Options.Script = "./build.cake";

                // When
                fixture.RunApplication();

                // Then
                fixture.CommandFactory.Received(1).CreateDryRunCommand();
            }
        }
    }
}